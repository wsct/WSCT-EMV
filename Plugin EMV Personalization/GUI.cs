using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WSCT.EMV.Card;
using WSCT.EMV.Commands;
using WSCT.EMV.Personalization;
using WSCT.Helpers;
using WSCT.Helpers.Json;
using WSCT.ISO7816;
using WSCT.Wrapper;

namespace WSCT.GUI.Plugins.EMV.Personalization
{
    /// <summary>
    /// Plugin dedicated form.
    /// </summary>
    public partial class Gui : Form
    {
        private const string DefaultEmvFolder = "emv-personalization";
        private const string DefaultPseFolder = "pse-personalization";
        private string EmvPersonalizationFolder => guiEmvFolderBrowserDialog.SelectedPath;
        private string PsePersonalizationFolder => guiPseFolderBrowserDialog.SelectedPath;

        #region >> Constructor

        /// <inheritdoc />
        public Gui()
        {
            InitializeComponent();

            Icon = Common.Resources.Icons.WSCT;

            guiEmvFolderBrowserDialog.SelectedPath = Directory.GetCurrentDirectory();
            if (Directory.Exists(DefaultEmvFolder))
            {
                guiEmvFolderBrowserDialog.SelectedPath = Path.Combine(Directory.GetCurrentDirectory(), DefaultEmvFolder);
            }
            if (Directory.Exists(DefaultPseFolder))
            {
                guiPseFolderBrowserDialog.SelectedPath = Path.Combine(Directory.GetCurrentDirectory(), DefaultPseFolder);
            }
        }

        #endregion

        #region >> gui * Click

        private async void guiDoRunEmvPersonalization_Click(object sender, EventArgs e)
        {
            guiLogs.Clear();

            var emvAidAsString = guiEMVAppletAID.Text;
            byte[] emvAid;

            LogActionStart($"Verifying AID: {emvAidAsString}");

            try
            {
                emvAid = emvAidAsString.FromHexa();

                LogSuccess();
            }
            catch (Exception _)
            {
                LogFailure("The AID is malformed");
                return;
            }


            var emvData = await Task.Run(LoadEmvPersonalizationFolder);

            var emvDgis = await Task.Run(() => BuildDgi(emvData));

            await Task.Run(() => TransmitToEmvApplet(emvDgis, emvAid));
        }

        private async void guiDoRunPsePersonalization_Click(object sender, EventArgs e)
        {
            guiLogs.Clear();

            var pseNameAsString = guiPSEAppletName.Text;
            byte[] emvAid;

            LogActionStart($"Verifying AID: {pseNameAsString}");

            try
            {
                emvAid = pseNameAsString.FromString();

                LogSuccess();
            }
            catch (Exception _)
            {
                LogFailure("The DF Name is malformed");
                return;
            }


            var pseData = await Task.Run(LoadPsePersonalizationFolder);

            var pseDgis = await Task.Run(() => BuildDgi(pseData));

            await Task.Run(() => TransmitToPseApplet(pseDgis, emvAid));
        }

        private void guiDoSelectEmvPersonalizationFolder_ClickAsync(object sender, EventArgs e)
        {
            if (guiEmvFolderBrowserDialog.ShowDialog() == DialogResult.OK && guiEmvFolderBrowserDialog.SelectedPath != "")
            {
                // TODO
            }
        }

        private void guiDoSelectPsePersonalizationFolder_Click(object sender, EventArgs e)
        {
            if (guiPseFolderBrowserDialog.ShowDialog() == DialogResult.OK && guiPseFolderBrowserDialog.SelectedPath != "")
            {
                // TODO
            }
        }

        #endregion

        private EmvPersonalizationData LoadEmvPersonalizationFolder()
        {
            var model = LoadDataFromFile<EmvPersonalizationModel>(EmvPersonalizationFolder, @"emv-card-model.json");
            var data = LoadDataFromFile<WSCT.EMV.Personalization.EmvPersonalizationData>(EmvPersonalizationFolder, @"emv-card-data.json");
            var issuerContext = LoadDataFromFile<EmvIssuerContext>(EmvPersonalizationFolder, @"emv-issuer-context.json");
            var iccContext = LoadDataFromFile<EmvIccContext>(EmvPersonalizationFolder, @"emv-icc-context.json");
            var pinBlock = LoadPinBlockFromString(guiAppletPin.Text);

            return new EmvPersonalizationData(model, data, issuerContext, iccContext, pinBlock);
        }

        private PsePersonalizationData LoadPsePersonalizationFolder()
        {
            var model = LoadDataFromFile<PsePersonalizationModel>(PsePersonalizationFolder, @"pse-card-model.json");
            var data = LoadDataFromFile<WSCT.EMV.Personalization.PsePersonalizationData>(PsePersonalizationFolder, @"pse-card-data.json");

            return new PsePersonalizationData(model, data);
        }

        private EmvCardDgis BuildDgi(EmvPersonalizationData cardData)
        {
            var builder = new DgiBuilder(cardData.Model, cardData.Data, cardData.IssuerContext, cardData.IccContext);

            var fci = builder.BuildDgi(cardData.Model.Fci);
            var gpo = builder.BuildDgi(cardData.Model.Gpo);
            var acid = builder.BuildDgi(cardData.Model.Acid);

            var records = new List<string>();
            foreach (var record in cardData.Model.Records)
            {
                LogActionStart($"Building DGI Record '{record.Sfi}.{record.Index}': ");

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

            LogActionStart($"Building DGI PINBlock '{guiAppletPin.Text}': ");

            string pin = default;
            try
            {
                pin = $"8010{cardData.PinBlock.PinBlock.Length:X2}{cardData.PinBlock.PinBlock.ToHexa('\0')}";

                LogSuccess();
            }
            catch (Exception exception)
            {
                LogException(exception);
            }

            return new EmvCardDgis(fci, gpo, acid, records, pin);
        }

        private PseCardDgis BuildDgi(PsePersonalizationData cardData)
        {
            var builder = new PseDgiBuilder(cardData.Model, cardData.Data);

            var fci = builder.BuildDgi(cardData.Model.Fci);

            var records = new List<string>();
            foreach (var pseRecords in cardData.Data.Records.GroupBy(r => r.Index))
            {
                LogActionStart($"Building DGI Record '0x01.{pseRecords.Key}': ");

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

        private T LoadDataFromFile<T>(string path, string fileName)
        {
            T result = default;

            LogActionStart($"Loading '{fileName}': ");

            try
            {
                result = Path.Combine(path, fileName).CreateFromJsonFile<T>();

                LogSuccess();
            }
            catch (Exception exception)
            {
                LogException(exception);
            }

            return result;
        }

        private PINBlock LoadPinBlockFromString(string pinAsString)
        {
            PINBlock result = default;

            LogActionStart($"Loading PIN '{pinAsString}': ");

            try
            {
                result = new PlaintextPINBlock { ClearPIN = pinAsString.FromBcd() };

                LogSuccess();
            }
            catch (Exception exception)
            {
                LogException(exception);
            }

            return result;
        }

        private void TransmitToEmvApplet(EmvCardDgis cardDgis, byte[] emvAid)
        {
            LogActionStart($"SELECT {emvAid.ToHexa('\0')}");
            TransmitToCard(new EMVSelectByNameCommand(emvAid));

            LogActionStart($"STORE DATA (FCI)");
            TransmitToCard(new EMVStoreDataCommand(0, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, cardDgis.Fci));

            LogActionStart($"STORE DATA (GPO)");
            TransmitToCard(new EMVStoreDataCommand(1, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, cardDgis.Gpo));

            LogActionStart($"STORE DATA (ACID)");
            TransmitToCard(new EMVStoreDataCommand(2, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, cardDgis.Acid));

            // Records
            byte dgiId = 2;
            foreach (var record in cardDgis.Records)
            {
                dgiId++;
                LogActionStart($"STORE DATA (Record {record.Substring(0, 4)}");
                TransmitToCard(new EMVStoreDataCommand(dgiId, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, record));
            }

            LogActionStart($"STORE DATA (PIN)");
            TransmitToCard(new EMVStoreDataCommand(++dgiId, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, cardDgis.Pin));

            LogActionStart($"STORE DATA (KEY1)");
            TransmitToCard(new EMVStoreDataCommand(++dgiId, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, "810181801B7275A03FD397B2E5B14E4E9ADFBB491AA2F2CD28F55E623FEED0E564576351157EAE3E94505999DF9E6AA457B977F4A36AFA54FB52ECA0FB373608E48E545B716C25DCC8CF4343490A500A8DF26A2D81777969D4F584842E771BA36563EB63B7EBC87AEA35FC3A208D6A1D01795E326873597FCFD0CB7339A889A3075EF921"));

            LogActionStart($"STORE DATA (KEY2)");
            TransmitToCard(new EMVStoreDataCommand(++dgiId, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, "81038180986965A7274E4165C127C6847AF8EA7ED5CBDAA10F46BF192C70BEB5B83B355A70E7FFB941BCE0440C7A7E552D4256F8B427C0F7395A9301D8841FBC6186CD087F44AD46BC322811ECD90A5FF1B9F93229BA8F85F9C1C5B791DC02073034853D1F89B9E3D9543C1803EAFF85C340AB6C0A352D626B56B961DC88A41FF6E1562D"));
        }

        private void TransmitToPseApplet(PseCardDgis cardDgis, byte[] dfName)
        {
            LogActionStart($"SELECT {Encoding.UTF8.GetString(dfName)}");
            TransmitToCard(new EMVSelectByNameCommand(dfName));

            LogActionStart($"STORE DATA (FCI)");
            TransmitToCard(new EMVStoreDataCommand(0, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, cardDgis.Fci));

            // Records
            byte dgiId = 0;
            foreach (var record in cardDgis.Records)
            {
                dgiId++;
                LogActionStart($"STORE DATA (Record {record.Substring(0, 4)}");
                TransmitToCard(new EMVStoreDataCommand(dgiId, false, EMVStoreDataCommand.Encryption.NoDGIEncrypted, record));
            }
        }

        private void TransmitToCard(CommandAPDU command)
        {
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
            if (guiLogs.InvokeRequired)
            {
                guiLogs.Invoke(new MethodInvoker(() => LogActionStart(message)));
                return;
            }

            guiLogs.AppendText($"{message}: ");
        }

        private void LogException(Exception exception)
        {
            if (guiLogs.InvokeRequired)
            {
                guiLogs.Invoke(new MethodInvoker(() => LogException(exception)));
                return;
            }

            guiLogs.AppendText($"Failure {Environment.NewLine}");
            guiLogs.AppendText($"{exception.Message}{Environment.NewLine}");
        }

        private void LogFailure(string message)
        {
            if (guiLogs.InvokeRequired)
            {
                guiLogs.Invoke(new MethodInvoker(() => LogFailure(message)));
                return;
            }

            guiLogs.SelectionColor = Color.DarkRed;
            guiLogs.AppendText($"Failure {Environment.NewLine}");
            guiLogs.AppendText($"{message}{Environment.NewLine}");
        }

        private void LogSuccess()
        {
            if (guiLogs.InvokeRequired)
            {
                guiLogs.Invoke(new MethodInvoker(LogSuccess));
                return;
            }

            guiLogs.SelectionColor = Color.DarkGreen;
            guiLogs.Invoke(new MethodInvoker(() => guiLogs.AppendText($"Success {Environment.NewLine}")));
        }
    }
}