using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSCT.Core;
using WSCT.EMV.Card;
using WSCT.EMV.Commands;
using WSCT.EMV.Personalization;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;
using WSCT.Helpers.Json;
using WSCT.ISO7816;
using WSCT.Wrapper;

namespace WSCT.GUI.Plugins.EMV.Personalization.Services
{
    public enum PersonalizationLogStatus
    {
        Pending,
        Success,
        Failure
    }

    public sealed class PersonalizationLogEntry
    {
        public string Action { get; set; } = string.Empty;
        public PersonalizationLogStatus Status { get; set; } = PersonalizationLogStatus.Pending;
        public string? Detail { get; set; }
    }

    public sealed class EmvPersonalizationSessionService : IDisposable
    {
        public const string DefaultEmvFolder = "emv-personalization";
        public const string DefaultPseFolder = "pse-personalization";

        public static readonly string[] RequiredEmvFiles =
        [
            "emv-card-model.json",
            "emv-card-data.json",
            "emv-issuer-context.json",
            "emv-icc-context.json"
        ];

        public static readonly string[] RequiredPseFiles =
        [
            "pse-card-model.json",
            "pse-card-data.json"
        ];

        private readonly List<PersonalizationLogEntry> _logs = [];
        private readonly Dictionary<string, string> _uploadedEmvFiles = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _uploadedPseFiles = new(StringComparer.OrdinalIgnoreCase);
        private readonly object _logLock = new();

        public event Action? OnStateChanged;

        public EmvPersonalizationSessionService()
        {
            EmvPersonalizationFolder = ResolveDefaultFolder(DefaultEmvFolder);
            PsePersonalizationFolder = ResolveDefaultFolder(DefaultPseFolder);

            SharedData.CardChannelChangedEvent += HandleCardChannelChanged;
        }

        public string EmvPersonalizationFolder { get; set; }
        public string PsePersonalizationFolder { get; set; }
        public string EmvAppletAid { get; set; } = "F0 43 41 45 4E 42 01";
        public string AppletPin { get; set; } = "1234";
        public string PseAppletName { get; set; } = "1PAY.SYS.DDF01";
        public bool IsBusy { get; private set; }

        public IReadOnlyList<PersonalizationLogEntry> Logs
        {
            get
            {
                lock (_logLock)
                {
                    return [.. _logs];
                }
            }
        }

        public bool HasUploadedEmvFile(string fileName) => _uploadedEmvFiles.ContainsKey(fileName);
        public bool HasUploadedPseFile(string fileName) => _uploadedPseFiles.ContainsKey(fileName);

        public bool IsEmvFileAvailable(string fileName)
        {
            return _uploadedEmvFiles.ContainsKey(fileName)
                || (!string.IsNullOrWhiteSpace(EmvPersonalizationFolder)
                    && File.Exists(Path.Combine(EmvPersonalizationFolder, fileName)));
        }

        public bool IsPseFileAvailable(string fileName)
        {
            return _uploadedPseFiles.ContainsKey(fileName)
                || (!string.IsNullOrWhiteSpace(PsePersonalizationFolder)
                    && File.Exists(Path.Combine(PsePersonalizationFolder, fileName)));
        }

        public void SetUploadedEmvFile(string fileName, string content)
        {
            _uploadedEmvFiles[fileName] = content;
            NotifyStateChanged();
        }

        public void ClearUploadedEmvFiles()
        {
            _uploadedEmvFiles.Clear();
            NotifyStateChanged();
        }

        public void SetUploadedPseFile(string fileName, string content)
        {
            _uploadedPseFiles[fileName] = content;
            NotifyStateChanged();
        }

        public void ClearUploadedPseFiles()
        {
            _uploadedPseFiles.Clear();
            NotifyStateChanged();
        }

        public void ClearLogs()
        {
            lock (_logLock)
            {
                _logs.Clear();
            }
            NotifyStateChanged();
        }

        public string BuildLogsExportText()
        {
            var sb = new StringBuilder();
            foreach (var entry in Logs)
            {
                var statusText = entry.Status switch
                {
                    PersonalizationLogStatus.Success => "Success",
                    PersonalizationLogStatus.Failure => "Failure",
                    _ => "..."
                };
                sb.AppendLine($"{entry.Action}: {statusText}");
                if (!string.IsNullOrWhiteSpace(entry.Detail))
                {
                    sb.AppendLine($"  {entry.Detail}");
                }
            }
            return sb.ToString();
        }

        public async Task RunEmvPersonalizationAsync()
        {
            if (IsBusy)
            {
                return;
            }

            if (!SharedData.IsValidChannel)
            {
                ClearLogs();
                LogActionStart("Card Channel");
                LogFailure("No active smart card channel. Connect to a card in WSCT GUI first.");
                return;
            }

            IsBusy = true;
            ClearLogs();
            NotifyStateChanged();

            try
            {
                var emvAidAsString = EmvAppletAid;
                byte[] emvAid;

                LogActionStart($"Verifying AID: {emvAidAsString}");
                try
                {
                    emvAid = emvAidAsString.FromHexa();
                    LogSuccess();
                }
                catch (Exception)
                {
                    LogFailure("The AID is malformed");
                    return;
                }

                var emvData = await Task.Run(LoadEmvPersonalizationData);
                if (emvData.Model is null || emvData.Data is null || emvData.IssuerContext is null || emvData.IccContext is null || emvData.PinBlock is null)
                {
                    LogActionStart("Personalization aborted");
                    LogFailure("One or more required EMV personalization inputs failed to load.");
                    return;
                }

                var emvDgis = await Task.Run(() => BuildDgi(emvData));
                await Task.Run(() => TransmitToEmvApplet(emvDgis, emvAid));
            }
            finally
            {
                IsBusy = false;
                NotifyStateChanged();
            }
        }

        public async Task RunPsePersonalizationAsync()
        {
            if (IsBusy)
            {
                return;
            }

            if (!SharedData.IsValidChannel)
            {
                ClearLogs();
                LogActionStart("Card Channel");
                LogFailure("No active smart card channel. Connect to a card in WSCT GUI first.");
                return;
            }

            IsBusy = true;
            ClearLogs();
            NotifyStateChanged();

            try
            {
                var pseNameAsString = PseAppletName;
                byte[] dfName;

                LogActionStart($"Verifying AID: {pseNameAsString}");
                try
                {
                    dfName = pseNameAsString.FromString();
                    LogSuccess();
                }
                catch (Exception)
                {
                    LogFailure("The DF Name is malformed");
                    return;
                }

                var pseData = await Task.Run(LoadPsePersonalizationData);
                if (pseData.Model is null || pseData.Data is null)
                {
                    LogActionStart("Personalization aborted");
                    LogFailure("One or more required PSE personalization inputs failed to load.");
                    return;
                }

                var pseDgis = await Task.Run(() => BuildDgi(pseData));
                await Task.Run(() => TransmitToPseApplet(pseDgis, dfName));
            }
            finally
            {
                IsBusy = false;
                NotifyStateChanged();
            }
        }

        private EmvApplicationInformation LoadEmvPersonalizationData()
        {
            var model = LoadData<EmvPersonalizationModel>(EmvPersonalizationFolder, "emv-card-model.json", _uploadedEmvFiles);
            var data = LoadData<EmvPersonalizationData>(EmvPersonalizationFolder, "emv-card-data.json", _uploadedEmvFiles);
            var issuerContext = LoadData<EmvIssuerContext>(EmvPersonalizationFolder, "emv-issuer-context.json", _uploadedEmvFiles);
            var iccContext = LoadData<EmvIccContext>(EmvPersonalizationFolder, "emv-icc-context.json", _uploadedEmvFiles);
            var pinBlock = LoadPinBlockFromString(AppletPin);

            return new EmvApplicationInformation(model!, data!, issuerContext!, iccContext!, pinBlock!);
        }

        private PseApplicationInformation LoadPsePersonalizationData()
        {
            var model = LoadData<PsePersonalizationModel>(PsePersonalizationFolder, "pse-card-model.json", _uploadedPseFiles);
            var data = LoadData<PsePersonalizationData>(PsePersonalizationFolder, "pse-card-data.json", _uploadedPseFiles);

            return new PseApplicationInformation(model!, data!);
        }

        private EmvCardDgis BuildDgi(EmvApplicationInformation cardData)
        {
            var builder = new DgiBuilder(cardData.Model, cardData.Data, cardData.IssuerContext, cardData.IccContext);

            var fci = builder.BuildDgi(cardData.Model.Fci!);
            var gpo = builder.BuildDgi(cardData.Model.Gpo!);
            var acid = builder.BuildDgi(cardData.Model.Acid!);

            var records = new List<string>();
            foreach (var record in cardData.Model.Records ?? [])
            {
                LogActionStart($"Building DGI Record '{record.Sfi}.{record.Index}'");
                try
                {
                    records.Add(builder.BuildDgi(record));
                    LogSuccess();
                }
                catch (Exception exception)
                {
                    LogException(exception);
                }
            }

            LogActionStart($"Building DGI PINBlock '{AppletPin}'");
            var pin = string.Empty;
            try
            {
                pin = $"8010{cardData.PinBlock.PinBlock.Length:X2}{cardData.PinBlock.PinBlock.ToHexa('\0')}";
                LogSuccess();
            }
            catch (Exception exception)
            {
                LogException(exception);
            }

            LogActionStart("Building DGI ICC Private Key");
            var iccPrivateKey = string.Empty;
            var iccModulus = string.Empty;
            try
            {
                iccPrivateKey = $"8101{TlvDataHelper.ToBerEncodedL((uint)cardData.IccContext.IccPrivateKey!.Modulus!.Length / 2).ToHexa('\0')}{cardData.IccContext.IccPrivateKey.PrivateExponent}";

                var iccModulusBytes = cardData.IccContext.IccPrivateKey.Modulus.FromHexa();
                if (iccModulusBytes[0] == 0x00)
                {
                    iccModulus = $"8103{TlvDataHelper.ToBerEncodedL((uint)iccModulusBytes.Length - 1).ToHexa('\0')}{iccModulusBytes[1..].ToHexa('\0')}";
                }
                else
                {
                    iccModulus = $"8103{TlvDataHelper.ToBerEncodedL((uint)iccModulusBytes.Length).ToHexa('\0')}{iccModulusBytes.ToHexa('\0')}";
                }

                LogSuccess();
            }
            catch (Exception exception)
            {
                LogException(exception);
            }

            return new EmvCardDgis(fci, gpo, acid, records, pin, iccPrivateKey, iccModulus);
        }

        private PseCardDgis BuildDgi(PseApplicationInformation cardData)
        {
            var builder = new PseDgiBuilder(cardData.Model, cardData.Data);

            var fci = builder.BuildDgi(cardData.Model.Fci!);

            var records = new List<string>();
            foreach (var pseRecords in (cardData.Data.Records ?? []).GroupBy(r => r.Index))
            {
                LogActionStart($"Building DGI Record '0x01.{pseRecords.Key}'");
                try
                {
                    records.Add(builder.BuildDgi(0x01, pseRecords.Key, pseRecords));
                    LogSuccess();
                }
                catch (Exception exception)
                {
                    LogException(exception);
                }
            }

            return new PseCardDgis(fci, records);
        }

        private T? LoadData<T>(string folderPath, string fileName, Dictionary<string, string> uploadedOverrides)
        {
            var fromUpload = uploadedOverrides.TryGetValue(fileName, out var jsonContent);
            LogActionStart(fromUpload ? $"Loading uploaded '{fileName}'" : $"Loading '{fileName}'");

            try
            {
                var result = fromUpload
                    ? jsonContent!.CreateFromJsonString<T>()
                    : Path.Combine(folderPath, fileName).CreateFromJsonFile<T>();

                LogSuccess();
                return result;
            }
            catch (Exception exception)
            {
                LogException(exception);
                return default;
            }
        }

        private PINBlock? LoadPinBlockFromString(string pinAsString)
        {
            LogActionStart($"Loading PIN '{pinAsString}'");

            try
            {
                var result = new PlaintextPINBlock { ClearPIN = pinAsString.FromBcd() };
                LogSuccess();
                return result;
            }
            catch (Exception exception)
            {
                LogException(exception);
                return default;
            }
        }

        private void TransmitToEmvApplet(EmvCardDgis cardDgis, byte[] emvAid)
        {
            LogActionStart($"SELECT {emvAid.ToHexa('\0')}");
            TransmitToCard(new EMVSelectByNameCommand(emvAid));

            LogActionStart("STORE DATA (FCI)");
            TransmitToCard(new EMVStoreDataCommand(0, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, cardDgis.Fci));

            LogActionStart("STORE DATA (GPO)");
            TransmitToCard(new EMVStoreDataCommand(1, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, cardDgis.Gpo));

            LogActionStart("STORE DATA (ACID)");
            TransmitToCard(new EMVStoreDataCommand(2, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, cardDgis.Acid));

            byte dgiId = 2;
            foreach (var record in cardDgis.Records)
            {
                dgiId++;
                var label = record.Length >= 4 ? record[..4] : record;
                LogActionStart($"STORE DATA (Record {label})");
                TransmitToCard(new EMVStoreDataCommand(dgiId, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, record));
            }

            LogActionStart("STORE DATA (PIN)");
            TransmitToCard(new EMVStoreDataCommand(++dgiId, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, cardDgis.Pin));

            LogActionStart("STORE DATA (KEY1)");
            TransmitToCard(new EMVStoreDataCommand(++dgiId, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, cardDgis.IccPrivateKey));

            LogActionStart("STORE DATA (KEY2)");
            TransmitToCard(new EMVStoreDataCommand(++dgiId, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, cardDgis.IccModulus));
        }

        private void TransmitToPseApplet(PseCardDgis cardDgis, byte[] dfName)
        {
            LogActionStart($"SELECT {Encoding.UTF8.GetString(dfName)}");
            TransmitToCard(new EMVSelectByNameCommand(dfName));

            LogActionStart("STORE DATA (FCI)");
            TransmitToCard(new EMVStoreDataCommand(0, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, cardDgis.Fci));

            byte dgiId = 0;
            foreach (var record in cardDgis.Records)
            {
                dgiId++;
                var label = record.Length >= 4 ? record[..4] : record;
                LogActionStart($"STORE DATA (Record {label})");
                TransmitToCard(new EMVStoreDataCommand(dgiId, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, record));
            }
        }

        private void TransmitToCard(CommandAPDU command)
        {
            if (!SharedData.IsValidChannel)
            {
                LogFailure("No active card channel");
                return;
            }

            try
            {
                var crp = new CommandResponsePair(command);
                var errorCode = crp.Transmit(SharedData.CardChannel);

                if (errorCode != ErrorCode.Success)
                {
                    LogFailure($"ErrorCode={errorCode}");
                }
                else if (crp.RApdu.StatusWord != 0x9000)
                {
                    LogFailure($"SW={crp.RApdu.StatusWord:X4}");
                }
                else
                {
                    LogSuccess();
                }
            }
            catch (Exception exception)
            {
                LogException(exception);
            }
        }

        private void LogActionStart(string message)
        {
            lock (_logLock)
            {
                _logs.Add(new PersonalizationLogEntry
                {
                    Action = message,
                    Status = PersonalizationLogStatus.Pending
                });
            }
            NotifyStateChanged();
        }

        private void LogSuccess()
        {
            lock (_logLock)
            {
                if (_logs.Count > 0)
                {
                    _logs[^1].Status = PersonalizationLogStatus.Success;
                }
            }
            NotifyStateChanged();
        }

        private void LogFailure(string message)
        {
            lock (_logLock)
            {
                if (_logs.Count > 0)
                {
                    _logs[^1].Status = PersonalizationLogStatus.Failure;
                    _logs[^1].Detail = message;
                }
            }
            NotifyStateChanged();
        }

        private void LogException(Exception exception)
        {
            LogFailure(exception.Message);
        }

        private static string ResolveDefaultFolder(string folderName)
        {
            var baseDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName);
            if (Directory.Exists(baseDirPath))
            {
                return baseDirPath;
            }

            var cwdPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (Directory.Exists(cwdPath))
            {
                return cwdPath;
            }

            return baseDirPath;
        }

        private void HandleCardChannelChanged()
        {
            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            OnStateChanged?.Invoke();
        }

        public void Dispose()
        {
            SharedData.CardChannelChangedEvent -= HandleCardChannelChanged;
        }
    }
}
