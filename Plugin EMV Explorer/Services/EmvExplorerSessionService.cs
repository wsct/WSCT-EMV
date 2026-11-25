using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WSCT.Core;
using WSCT.EMV.Card;
using WSCT.EMV.Exceptions;
using WSCT.EMV.Objects;
using WSCT.EMV.Security;
using WSCT.EMV.Terminal;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.GUI.Plugins.EMVExplorer.Services
{
    public sealed class EmvExplorerSessionService : IDisposable
    {
        private readonly PluginConfiguration _pluginConfiguration;
        private readonly TlvDictionary _tlvDictionary;
        private readonly CertificationAuthorityRepository _certificationAuthorityRepository;
        private readonly EmvDetailedLogObserver _detailedLogsObserver;

        private readonly List<EmvApplication> _emvApplications = [];
        private readonly List<EmvDetailedLogEntry> _detailedLogs = [];
        private readonly List<EmvTreeNode> _pseTreeNodes = [];
        private readonly List<EmvTreeNode> _emvApplicationTreeNodes = [];
        private readonly List<EmvSignedDataItem> _sdaSignedData = [];
        private readonly List<EmvSignedDataItem> _ddaSignedData = [];
        private readonly List<EmvSignedDataItem> _cdaSignedData = [];

        private EmvApplication? _emv;
        private PaymentSystemEnvironment? _pse;

        public event Action? OnStateChanged;

        public EmvExplorerSessionService()
        {
            var configPath = ResolveDataFile("Config.EMVExplorer.xml");
            var dictPath = ResolveDataFile("Dictionary.EMVTag.xml");

            _pluginConfiguration = SerializedObject<PluginConfiguration>.LoadFromXml(configPath) ?? new PluginConfiguration();
            _tlvDictionary = SerializedObject<TlvDictionary>.LoadFromXml(dictPath) ?? new TlvDictionary();
            _certificationAuthorityRepository = _pluginConfiguration.terminalConfiguration.CertificationAuthorityRepository;

            _detailedLogsObserver = new EmvDetailedLogObserver(AppendDetailedLog, _tlvDictionary);

            SupportedPses = _pluginConfiguration.terminalConfiguration.TerminalCapabilities.SupportedPses ?? [];
            SelectedPseName = SupportedPses.FirstOrDefault()?.Name ?? "1PAY.SYS.DDF01";

            SharedData.CardChannelChangedEvent += HandleCardChannelChanged;
        }

        // Configuration & lists
        public IReadOnlyList<TerminalCapabilities.SupportedPSE> SupportedPses { get; }
        public IReadOnlyList<EmvApplication> DiscoveredApplications => _emvApplications;
        public IReadOnlyList<EmvDetailedLogEntry> DetailedLogs => _detailedLogs;
        public IReadOnlyList<EmvTreeNode> PseTreeNodes => _pseTreeNodes;
        public IReadOnlyList<EmvTreeNode> EmvApplicationTreeNodes => _emvApplicationTreeNodes;
        public IReadOnlyList<EmvSignedDataItem> SdaSignedData => _sdaSignedData;
        public IReadOnlyList<EmvSignedDataItem> DdaSignedData => _ddaSignedData;
        public IReadOnlyList<EmvSignedDataItem> CdaSignedData => _cdaSignedData;
        public IReadOnlyList<CardholderVerificationMethodList.CvRule> CvmRules { get; private set; } = [];

        // Snapshots
        public EmvPublicKeysSnapshot PublicKeys { get; } = new();
        public EmvSdaSnapshot Sda { get; } = new();
        public EmvDdaSnapshot Dda { get; } = new();
        public EmvCardLogSnapshot CardLog { get; } = new();
        public EmvTvrSnapshot Tvr { get; } = new();
        public EmvNotification? Notification { get; private set; }

        // Interactive form inputs preserved across tab switches
        public string SelectedPseName { get; set; }
        public bool SearchTagAidInFci { get; set; }
        public string SelectedApplicationAid { get; set; } = string.Empty;
        public string InternalAuthenticateUnpredictableNumber { get; set; } = "01020304";
        public bool PinEntryUsed { get; set; }
        public string PinEntry { get; set; } = string.Empty;
        public int SelectedCvmIndex { get; set; }
        public string Ac1UnpredictableNumber { get; set; } = "01020304";
        public CryptogramType SelectedAc1Type { get; set; } = CryptogramType.TC;

        // Workflow enablement flags
        public bool CanSelectAid => _emvApplications.Count > 0;
        public bool CanGetProcessingOptions { get; private set; }
        public bool CanGetData { get; private set; }
        public bool CanReadRecords { get; private set; }
        public bool CanInternalAuthenticate { get; private set; }
        public bool CanVerifyCardholder { get; private set; }
        public bool CanGetChallenge
        {
            get
            {
                if (!CanVerifyCardholder || SelectedCvmIndex < 0 || SelectedCvmIndex >= CvmRules.Count)
                {
                    return false;
                }

                return CvmRules[SelectedCvmIndex].CvmCode is
                    CardholderVerificationMethodList.CvmCode.EncipheredPinIcc or
                    CardholderVerificationMethodList.CvmCode.EncipheredPinIccAndSign;
            }
        }
        public bool CanGenerateAc1 { get; private set; }
        public bool CanReadCardLog => CardLog.IsPresent;

        public void ClearNotification()
        {
            Notification = null;
            NotifyStateChanged();
        }

        public void ClearDetailedLogs()
        {
            _detailedLogs.Clear();
            NotifyStateChanged();
        }

        public void ResetSession()
        {
            _emv = null;
            _pse = null;
            _emvApplications.Clear();
            _pseTreeNodes.Clear();
            _emvApplicationTreeNodes.Clear();
            _sdaSignedData.Clear();
            _ddaSignedData.Clear();
            _cdaSignedData.Clear();
            _detailedLogs.Clear();
            CvmRules = [];
            SelectedApplicationAid = string.Empty;
            SelectedCvmIndex = 0;
            PinEntryUsed = false;
            PinEntry = string.Empty;

            CanGetProcessingOptions = false;
            CanGetData = false;
            CanReadRecords = false;
            CanInternalAuthenticate = false;
            CanVerifyCardholder = false;
            CanGenerateAc1 = false;

            ResetSnapshots();
            Notification = null;
            NotifyStateChanged();
        }

        public void SelectPse()
        {
            if (!EnsureValidChannel())
            {
                return;
            }

            ClearNotification();

            try
            {
                _pse = new PaymentSystemEnvironment(SharedData.CardChannel)
                {
                    Name = SelectedPseName,
                    SearchTagAidInFci = SearchTagAidInFci
                };

                _detailedLogsObserver.ObservePse(_pse);
                ObservePse(_pse);

                if (_pse.Select() == 0x9000)
                {
                    if (_pse.TlvFci is not null && _pse.TlvFci.HasTag(0x88))
                    {
                        _pse.Read();
                    }
                }
            }
            catch (Exception exception)
            {
                SetNotification("PSE Selection Error", exception.Message, "alert-danger");
            }

            NotifyStateChanged();
        }

        public void ExplicitDiscoveryOfAids()
        {
            if (!EnsureValidChannel())
            {
                return;
            }

            ClearNotification();

            try
            {
                var supportedApps = _pluginConfiguration.terminalConfiguration.TerminalCapabilities.SupportedApplications ?? [];
                foreach (var app in supportedApps)
                {
                    if (string.IsNullOrWhiteSpace(app.Aid))
                    {
                        continue;
                    }

                    var alreadyFound = _emvApplications.Any(emvFound => emvFound.Aid == app.Aid);
                    if (!alreadyFound)
                    {
                        var emv = new EmvApplication(SharedData.CardChannel, new TlvData())
                        {
                            Aid = app.Aid
                        };
                        _detailedLogsObserver.ObserveEmv(emv);
                        ObserveEmv(emv);

                        if (emv.Select() == 0x9000)
                        {
                            _emvApplications.Add(emv);
                            if (string.IsNullOrEmpty(SelectedApplicationAid))
                            {
                                SelectedApplicationAid = emv.Aid;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                SetNotification("Explicit AID Discovery Error", exception.Message, "alert-danger");
            }

            NotifyStateChanged();
        }

        public void SelectAid()
        {
            if (!EnsureValidChannel())
            {
                return;
            }

            ClearNotification();

            _emv = _emvApplications.FirstOrDefault(a => a.Aid == SelectedApplicationAid);
            if (_emv is null)
            {
                SetNotification("EMV application missing", "An EMV application must be first selected.", "alert-warning");
                return;
            }

            try
            {
                _emv.CertificationAuthorityRepository = _certificationAuthorityRepository;
                _emv.TlvTerminalData.AddRange(_pluginConfiguration.transactionContext.TlvDatas);

                _emv.Select();

                if (_emv.TlvFci is not null)
                {
                    CanGetProcessingOptions = true;
                    CanGetData = true;
                }
            }
            catch (Exception exception)
            {
                SetNotification("Select AID Error", exception.Message, "alert-danger");
            }

            NotifyStateChanged();
        }

        public void GetProcessingOptions()
        {
            if (!EnsureValidChannel() || _emv is null)
            {
                return;
            }

            ClearNotification();

            try
            {
                // Preserve legacy GPO terminal data behavior (tag 9F66)
                _emv.TlvTerminalData.Add(new TlvData(0x9F66, 0x02, [0x80, 0x00]));

                _emv.GetProcessingOptions();

                if (_emv.TlvProcessingOptions is not null)
                {
                    CanReadRecords = true;

                    try
                    {
                        if (_emv.Aip is not null && _emv.Aip.Dda)
                        {
                            var unpredictableNumber = new byte[4];
                            Random.Shared.NextBytes(unpredictableNumber);
                            InternalAuthenticateUnpredictableNumber = unpredictableNumber.ToHexa('\0');
                            CanInternalAuthenticate = true;
                        }
                    }
                    catch (EMVApplicationException)
                    {
                        // AIP unavailable
                    }
                }
            }
            catch (Exception exception)
            {
                SetNotification("Get Processing Options Error", exception.Message, "alert-danger");
            }

            NotifyStateChanged();
        }

        public void ReadApplicationData()
        {
            if (!EnsureValidChannel() || _emv is null)
            {
                return;
            }

            ClearNotification();

            try
            {
                _emv.ReadApplicationData();

                if (_emv.CvmList is not null)
                {
                    CvmRules = _emv.CvmList.CvRules;
                    SelectedCvmIndex = 0;
                    CanVerifyCardholder = true;
                }

                CanGenerateAc1 = true;
            }
            catch (Exception exception)
            {
                SetNotification("Read Application Data Error", exception.Message, "alert-danger");
            }

            NotifyStateChanged();
        }

        public void GetData()
        {
            if (!EnsureValidChannel() || _emv is null)
            {
                return;
            }

            ClearNotification();

            try
            {
                _emv.GetData();
            }
            catch (Exception exception)
            {
                SetNotification("Get Data Error", exception.Message, "alert-danger");
            }

            NotifyStateChanged();
        }

        public void InternalAuthenticate()
        {
            if (!EnsureValidChannel() || _emv is null)
            {
                return;
            }

            ClearNotification();

            byte[] unpredictableNumber;
            try
            {
                unpredictableNumber = InternalAuthenticateUnpredictableNumber.FromHexa();
            }
            catch (Exception)
            {
                SetNotification("Format error", "Bad format of unpredictable number. Should be hexadecimal.", "alert-danger");
                return;
            }

            try
            {
                _emv.InternalAuthenticate(unpredictableNumber);
            }
            catch (Exception exception)
            {
                SetNotification("Internal Authenticate Error", exception.Message, "alert-danger");
            }

            NotifyStateChanged();
        }

        public void GetChallenge()
        {
            if (!EnsureValidChannel() || _emv is null)
            {
                return;
            }

            ClearNotification();

            try
            {
                _emv.GetChallenge();
            }
            catch (Exception exception)
            {
                SetNotification("Get Challenge Error", exception.Message, "alert-danger");
            }

            NotifyStateChanged();
        }

        public void VerifyCardholder()
        {
            if (!EnsureValidChannel() || _emv is null)
            {
                return;
            }

            ClearNotification();

            if (SelectedCvmIndex < 0 || SelectedCvmIndex >= CvmRules.Count)
            {
                SetNotification("CVM missing", "Select a cardholder verification method first.", "alert-warning");
                return;
            }

            var cvRule = CvmRules[SelectedCvmIndex];
            if (cvRule.CvmCode == CardholderVerificationMethodList.CvmCode.PlaintextPinIcc
                || cvRule.CvmCode == CardholderVerificationMethodList.CvmCode.PlaintextPinIccAndSign)
            {
                if (PinEntryUsed)
                {
                    try
                    {
                        var pinBlock = new PlaintextPINBlock
                        {
                            ClearPIN = PinEntry.FromBcd((uint)PinEntry.Length)
                        };
                        _emv.VerifyPin(pinBlock);
                    }
                    catch (Exception exception)
                    {
                        SetNotification("Verify PIN Error", exception.Message, "alert-danger");
                    }
                }
                else
                {
                    SetNotification("PIN required", "A PIN must be keyed in.", "alert-warning");
                }
            }
            else
            {
                SetNotification("Unsupported CVM", "Unsupported cardholder verification method.", "alert-info");
            }

            NotifyStateChanged();
        }

        public void GenerateAc1()
        {
            if (!EnsureValidChannel() || _emv is null)
            {
                return;
            }

            ClearNotification();

            if (SelectedAc1Type == CryptogramType.Undefined)
            {
                SetNotification("Unsupported feature", $"Cryptogram type [{SelectedAc1Type}] unsupported.", "alert-danger");
                return;
            }

            byte[] unpredictableNumber;
            try
            {
                unpredictableNumber = Ac1UnpredictableNumber.FromHexa();
            }
            catch (Exception)
            {
                SetNotification("Format error", "Bad format of unpredictable number. Should be hexadecimal.", "alert-danger");
                return;
            }

            try
            {
                _emv.GenerateAc1(SelectedAc1Type, unpredictableNumber);
            }
            catch (Exception exception)
            {
                SetNotification("Generate AC1 Error", exception.Message, "alert-danger");
            }

            NotifyStateChanged();
        }

        public void ReadCardLog()
        {
            if (!EnsureValidChannel() || _emv is null)
            {
                return;
            }

            ClearNotification();

            try
            {
                _emv.ReadLogFile();
            }
            catch (Exception exception)
            {
                SetNotification("Read Log File Error", exception.Message, "alert-danger");
            }

            NotifyStateChanged();
        }

        public void NotifyNotYetImplemented(string featureName)
        {
            SetNotification("Not yet implemented", $"{featureName}: this feature has not yet been implemented.", "alert-info");
        }

        public string BuildCardLogExportText()
        {
            var sb = new StringBuilder();
            foreach (var col in CardLog.Columns)
            {
                sb.Append($"[{col}] ");
            }
            sb.AppendLine();

            foreach (var row in CardLog.Rows)
            {
                foreach (var cell in row)
                {
                    sb.Append(cell);
                    sb.Append(' ');
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public string BuildDetailedLogsExportText()
        {
            var sb = new StringBuilder();
            foreach (var entry in _detailedLogs)
            {
                if (entry.Kind == EmvLogEntryKind.Header)
                {
                    sb.AppendLine();
                    sb.AppendLine(entry.Text);
                    sb.AppendLine();
                }
                else
                {
                    sb.AppendLine(entry.Text);
                }
            }
            return sb.ToString();
        }

        public void ExpandAllEmvNodes(bool expanded)
        {
            foreach (var node in _emvApplicationTreeNodes)
            {
                node.SetExpandedRecursive(expanded);
            }
            NotifyStateChanged();
        }

        public void ExpandAllPseNodes(bool expanded)
        {
            foreach (var node in _pseTreeNodes)
            {
                node.SetExpandedRecursive(expanded);
            }
            NotifyStateChanged();
        }

        private void ObservePse(PaymentSystemEnvironment pse)
        {
            pse.AfterSelectEvent += AfterPseSelectEventHandler;
            pse.AfterReadEvent += AfterPseReadEventHandler;
        }

        private void ObserveEmv(EmvApplication emv)
        {
            emv.AfterSelectEvent += AfterEmvSelectEventHandler;
            emv.AfterGetProcessingOptionsEvent += AfterGetProcessingOptionsEventHandler;
            emv.AfterReadApplicationDataEvent += AfterReadApplicationDataEventHandler;
            emv.AfterGetDataEvent += AfterGetDataEventHandler;
            emv.AfterReadLogFileEvent += AfterReadLogFileEventHandler;
            emv.AfterInternalAuthenticateEvent += AfterInternalAuthenticateEventHandler;
            emv.AfterVerifyPinEvent += AfterVerifyPinEventHandler;
            emv.AfterGenerateAC1Event += AfterGenerateAc1EventHandler;
        }

        private void AfterPseSelectEventHandler(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not PaymentSystemEnvironment pse)
            {
                return;
            }

            RegisterDiscoveredApplications(pse.GetApplications());

            var pseNode = new EmvTreeNode(pse.Name);
            var fciNode = new EmvTreeNode("File Control Information");
            pseNode.Children.Add(fciNode);

            if (pse.TlvFci is not null)
            {
                fciNode.Children.Add(ConvertTlvDataToTreeNode(pse.TlvFci));
            }
            else
            {
                fciNode.Children.Add(new EmvTreeNode($"PSE named '{pse.Name}'/[{pse.Aid}] not found.", isError: true));
            }

            _pseTreeNodes.Add(pseNode);
            NotifyStateChanged();
        }

        private void AfterPseReadEventHandler(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not PaymentSystemEnvironment pse)
            {
                return;
            }

            RegisterDiscoveredApplications(pse.GetApplications());

            var pseNode = new EmvTreeNode(pse.Name);
            var recordsNode = new EmvTreeNode("Records");
            pseNode.Children.Add(recordsNode);

            if (pse.TlvRecords.Count != 0)
            {
                byte recordNumber = 0;
                foreach (var tlv70 in pse.TlvRecords)
                {
                    recordNumber++;
                    var recordNode = new EmvTreeNode($"Record {recordNumber}");
                    recordNode.Children.Add(ConvertTlvDataToTreeNode(tlv70));
                    recordsNode.Children.Add(recordNode);
                }
            }
            else
            {
                recordsNode.Children.Add(new EmvTreeNode("No record found in PSE.", isError: true));
            }

            _pseTreeNodes.Add(pseNode);
            NotifyStateChanged();
        }

        private void AfterEmvSelectEventHandler(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            UpdateEmvApplicationTree(emv);
            UpdateLogEntryAndFormat(emv);
            NotifyStateChanged();
        }

        private void AfterGetProcessingOptionsEventHandler(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            UpdateEmvApplicationTree(emv);
            UpdateTvr(emv);
            NotifyStateChanged();
        }

        private void AfterReadApplicationDataEventHandler(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            UpdateEmvApplicationTree(emv);
            UpdatePublicKeys(emv);
            UpdateSda(emv);
            UpdateSignedData(emv);
            UpdateTvr(emv);
            NotifyStateChanged();
        }

        private void AfterGetDataEventHandler(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            UpdateEmvApplicationTree(emv);
            UpdateLogEntryAndFormat(emv);
            UpdateTvr(emv);
            NotifyStateChanged();
        }

        private void AfterReadLogFileEventHandler(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            UpdateLogRecords(emv);
            UpdateTvr(emv);
            NotifyStateChanged();
        }

        private void AfterInternalAuthenticateEventHandler(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            UpdateEmvApplicationTree(emv);
            UpdateDda(emv);
            UpdateTvr(emv);
            NotifyStateChanged();
        }

        private void AfterVerifyPinEventHandler(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            UpdateEmvApplicationTree(emv);
            UpdateTvr(emv);
            NotifyStateChanged();
        }

        private void AfterGenerateAc1EventHandler(object? sender, EmvEventArgs eventArgs)
        {
            if (sender is not EmvApplication emv)
            {
                return;
            }

            UpdateEmvApplicationTree(emv);
            UpdateTvr(emv);
            NotifyStateChanged();
        }

        private void RegisterDiscoveredApplications(IEnumerable<EmvApplication> applications)
        {
            foreach (var emvFound in applications)
            {
                if (_emvApplications.Any(existing => existing.Aid == emvFound.Aid))
                {
                    continue;
                }

                _detailedLogsObserver.ObserveEmv(emvFound);
                ObserveEmv(emvFound);
                _emvApplications.Add(emvFound);
            }

            if (string.IsNullOrEmpty(SelectedApplicationAid) && _emvApplications.Count > 0)
            {
                SelectedApplicationAid = _emvApplications[0].Aid;
            }
        }

        private EmvTreeNode ConvertTlvDataToTreeNode(TlvData tlv)
        {
            EmvTreeNode tlvNode;
            var tagHex = $"{tlv:T}";

            if (_tlvDictionary.Get(tagHex) is not null)
            {
                var tlvObject = _tlvDictionary.CreateInstance(tlv);
                tlvObject.Tlv = tlv;
                tlvNode = new EmvTreeNode($"{tlvObject:N}")
                {
                    TagHex = tagHex,
                    Value = $"{tlvObject}"
                };
            }
            else
            {
                tlvNode = new EmvTreeNode($"T:{tlv:T} L:{tlv:L}")
                {
                    TagHex = tagHex,
                    Value = $"{tlv:Vh}"
                };
            }

            foreach (var subTlv in tlv.InnerTlvs)
            {
                tlvNode.Children.Add(ConvertTlvDataToTreeNode(subTlv));
            }

            return tlvNode;
        }

        private void UpdateEmvApplicationTree(EmvApplication emv)
        {
            var emvAppNode = new EmvTreeNode(emv.Aid);

            // FCI
            var fciNode = new EmvTreeNode("File Control Information");
            emvAppNode.Children.Add(fciNode);
            if (emv.TlvFci is not null)
            {
                fciNode.Children.Add(ConvertTlvDataToTreeNode(emv.TlvFci));
            }
            else
            {
                fciNode.Children.Add(new EmvTreeNode($"EMV application [{emv.Aid}] not found.", isError: true));
            }

            // Processing Options
            var optionsNode = new EmvTreeNode("Processing Options");
            emvAppNode.Children.Add(optionsNode);
            if (emv.TlvProcessingOptions is not null)
            {
                optionsNode.Children.Add(ConvertTlvDataToTreeNode(emv.TlvProcessingOptions));
            }
            else
            {
                optionsNode.Children.Add(new EmvTreeNode("Processing Options not found.", isError: true));
            }

            // AIP
            var aipNode = new EmvTreeNode("Application Interchange Profile");
            optionsNode.Children.Add(aipNode);
            try
            {
                aipNode.Children.Add(new EmvTreeNode($"{emv.Aip}"));
            }
            catch (EMVApplicationException)
            {
                aipNode.Children.Add(new EmvTreeNode("AIP not found.", isError: true));
            }

            // AFL
            var aflNode = new EmvTreeNode("Application File Locator");
            optionsNode.Children.Add(aflNode);
            try
            {
                foreach (var file in emv.Afl.Files)
                {
                    aflNode.Children.Add(new EmvTreeNode($"{file}"));
                }
            }
            catch (EMVApplicationException)
            {
                aflNode.Children.Add(new EmvTreeNode("AFL not found.", isError: true));
            }

            // Records
            var recordsNode = new EmvTreeNode("Records");
            emvAppNode.Children.Add(recordsNode);
            if (emv.TlvRecords.Count != 0)
            {
                byte recordNumber = 0;
                foreach (var tlv70 in emv.TlvRecords)
                {
                    recordNumber++;
                    var recordNode = new EmvTreeNode($"Record {recordNumber}");
                    recordNode.Children.Add(ConvertTlvDataToTreeNode(tlv70));
                    recordsNode.Children.Add(recordNode);
                }
            }
            else
            {
                recordsNode.Children.Add(new EmvTreeNode("No records found.", isError: true));
            }

            // Get Data
            var dataNode = new EmvTreeNode("Get data");
            emvAppNode.Children.Add(dataNode);
            if (emv.TlvATC is not null)
            {
                dataNode.Children.Add(ConvertTlvDataToTreeNode(emv.TlvATC));
            }
            if (emv.TlvLastOnlineATCRegister is not null)
            {
                dataNode.Children.Add(ConvertTlvDataToTreeNode(emv.TlvLastOnlineATCRegister));
            }
            if (emv.TlvLogFormat is not null)
            {
                dataNode.Children.Add(ConvertTlvDataToTreeNode(emv.TlvLogFormat));
            }
            if (emv.TlvPINTryCounter is not null)
            {
                dataNode.Children.Add(ConvertTlvDataToTreeNode(emv.TlvPINTryCounter));
            }

            // Internal Authenticate
            var internalAuthNode = new EmvTreeNode("Internal Authenticate");
            emvAppNode.Children.Add(internalAuthNode);
            if (emv.TlvSignedDynamicApplicationResponse is not null)
            {
                if (emv.TlvInternalAuthenticateUnpredictableNumber is not null)
                {
                    internalAuthNode.Children.Add(ConvertTlvDataToTreeNode(emv.TlvInternalAuthenticateUnpredictableNumber));
                }
                internalAuthNode.Children.Add(ConvertTlvDataToTreeNode(emv.TlvSignedDynamicApplicationResponse));
            }
            else
            {
                internalAuthNode.Children.Add(new EmvTreeNode("Internal authenticate APDU not done or failed.", isError: true));
            }

            // SDA
            var sdaNode = new EmvTreeNode("Static Data Authentication");
            emvAppNode.Children.Add(sdaNode);
            if (emv.Sda is not null)
            {
                sdaNode.Children.Add(new EmvTreeNode($"Hash Algorithm Indicator: {emv.Sda.HashAlgorithmIndicator:X2}"));
                sdaNode.Children.Add(new EmvTreeNode($"Hash Result: {emv.Sda.HashResult.ToHexa()}"));
                sdaNode.Children.Add(new EmvTreeNode($"Data Authentication Code: {emv.Sda.DataAuthenticationCode.ToHexa()}"));
            }
            else
            {
                sdaNode.Children.Add(new EmvTreeNode("SDA not done, supported or cryptographic error", isError: true));
            }

            // DDA
            var ddaNode = new EmvTreeNode("Dynamic Data Authentication");
            emvAppNode.Children.Add(ddaNode);
            if (emv.Dda is not null)
            {
                ddaNode.Children.Add(new EmvTreeNode($"Hash Algorithm Indicator: {emv.Dda.HashAlgorithmIndicator:X2}"));
                ddaNode.Children.Add(new EmvTreeNode($"Hash Result: {emv.Dda.HashResult.ToHexa()}"));
                ddaNode.Children.Add(new EmvTreeNode($"ICC Dynamic Data Length: {emv.Dda.IccDynamicDataLength:X2}"));
                ddaNode.Children.Add(new EmvTreeNode($"ICC Dynamic Data: {emv.Dda.IccDynamicData.ToHexa()}"));
            }
            else
            {
                ddaNode.Children.Add(new EmvTreeNode("DDA not done, supported or cryptographic error", isError: true));
            }

            // Cardholder Verification
            var pinNode = new EmvTreeNode("Cardholder Verification");
            emvAppNode.Children.Add(pinNode);
            pinNode.Children.Add(new EmvTreeNode($"TVR :: Cardholder Verification: {(emv.Tvr.CardholderVerificationFailed ? "failed" : "success")}"));
            if (emv.VerifyPinStatusWord != 0x0000)
            {
                pinNode.Children.Add(new EmvTreeNode($"VERIFY PIN status: 0x{emv.VerifyPinStatusWord:X4}"));
            }

            // AC1
            var ac1Node = new EmvTreeNode("Application Cryptogram 1");
            emvAppNode.Children.Add(ac1Node);
            if (emv.TlvGenerateAC1Response is not null)
            {
                ac1Node.Children.Add(new EmvTreeNode($"Requested AC: {emv.RequestedAC1Type}"));
                if (emv.TlvGenerateAC1UnpredictableNumber is not null)
                {
                    ac1Node.Children.Add(new EmvTreeNode($"Unpredictable Number: {emv.TlvGenerateAC1UnpredictableNumber.Value.ToHexa()}"));
                }
                var responseNode = new EmvTreeNode($"Response: {emv.TlvGenerateAC1Response}");
                ac1Node.Children.Add(responseNode);
                responseNode.Children.Add(new EmvTreeNode($"Cryptogram Information Data: {emv.Cid1}"));
                responseNode.Children.Add(new EmvTreeNode($"Application Transaction Counter: {emv.AtcFromAC1}"));
            }

            _emvApplicationTreeNodes.Clear();
            _emvApplicationTreeNodes.Add(emvAppNode);
        }

        private void UpdatePublicKeys(EmvApplication emv)
        {
            var aidObject = new ApplicationIdentifier(emv.Aid);
            PublicKeys.Aid = emv.Aid;
            PublicKeys.Rid = aidObject.Rid;

            if (emv.TlvDataRecords.HasTag(0x8F))
            {
                PublicKeys.CaIndex = emv.TlvDataRecords.GetTag(0x8F).Value.ToHexa();
            }

            if (emv.CertificationAuthorityPublicKey is not null)
            {
                PublicKeys.CaModulus = emv.CertificationAuthorityPublicKey.Modulus ?? string.Empty;
                PublicKeys.CaExponent = emv.CertificationAuthorityPublicKey.Exponent ?? string.Empty;
            }

            if (emv.IssuerPublicKeyCertificate is not null && emv.IssuerPublicKey is not null)
            {
                PublicKeys.IssuerRecoveredData = emv.IssuerPublicKeyCertificate.Recovered.ToHexa('\0');
                PublicKeys.IssuerHashResult = emv.IssuerPublicKeyCertificate.HashResult.ToHexa();
                PublicKeys.IssuerModulus = emv.IssuerPublicKey.Modulus ?? string.Empty;
                PublicKeys.IssuerExponent = emv.IssuerPublicKey.Exponent ?? string.Empty;
            }

            if (emv.IccPublicKeyCertificate is not null && emv.IccPublicKey is not null)
            {
                PublicKeys.IccRecoveredData = emv.IccPublicKeyCertificate.Recovered.ToHexa('\0');
                PublicKeys.IccHashResult = emv.IccPublicKeyCertificate.HashResult.ToHexa();
                PublicKeys.IccModulus = emv.IccPublicKey.Modulus ?? string.Empty;
                PublicKeys.IccExponent = emv.IccPublicKey.Exponent ?? string.Empty;
            }
        }

        private void UpdateSignedData(EmvApplication emv)
        {
            _sdaSignedData.Clear();
            _ddaSignedData.Clear();
            _cdaSignedData.Clear();

            if (emv.TlvOfflineRecords is null)
            {
                return;
            }

            ApplicationInterchangeProfile? aip = null;
            try
            {
                aip = emv.Aip;
            }
            catch (EMVApplicationException)
            {
                return;
            }

            var index = 0;
            foreach (var tlv in emv.TlvOfflineRecords)
            {
                index++;
                var tagHex = $"{tlv:T}";
                var dataName = string.Empty;

                if (_tlvDictionary.Get(tagHex) is not null)
                {
                    var tlvObject = _tlvDictionary.CreateInstance(tlv);
                    tlvObject.Tlv = tlv;
                    dataName = $"{tlvObject:N}";
                }

                var item = new EmvSignedDataItem(index, tagHex, dataName, $"{tlv:V}");
                if (aip.Sda)
                {
                    _sdaSignedData.Add(item);
                }
                if (aip.Dda)
                {
                    _ddaSignedData.Add(item);
                }
                if (aip.Cda)
                {
                    _cdaSignedData.Add(item);
                }
            }
        }

        private void UpdateSda(EmvApplication emv)
        {
            if (emv.Sda is not null)
            {
                Sda.RecoveredData = emv.Sda.Recovered.ToHexa('\0');
                Sda.HashResult = emv.Sda.HashResult.ToHexa();
                Sda.DataAuthenticationCode = emv.Sda.DataAuthenticationCode.ToHexa();
            }
        }

        private void UpdateDda(EmvApplication emv)
        {
            if (emv.Dda is not null)
            {
                Dda.RecoveredData = emv.Dda.Recovered.ToHexa('\0');
                Dda.HashResult = emv.Dda.HashResult.ToHexa();
                Dda.IccDynamicData = emv.Dda.IccDynamicData.ToHexa();
            }
        }

        private void UpdateLogEntryAndFormat(EmvApplication emv)
        {
            if (emv.LogFormat is not null)
            {
                CardLog.LogFormatText = $"{emv.LogFormat}";
            }

            if (emv.LogEntry is not null)
            {
                CardLog.SfiHex = $"{emv.LogEntry.Sfi:X2}";
                CardLog.LengthHex = $"{emv.LogEntry.CyclicFileSize:X2}";
            }

            CardLog.IsPresent = emv.LogEntry is not null && emv.LogFormat is not null;
        }

        private void UpdateLogRecords(EmvApplication emv)
        {
            if (emv.LogRecords is null || emv.LogFormat is null)
            {
                return;
            }

            CardLog.Columns.Clear();
            CardLog.Rows.Clear();

            CardLog.Columns.Add("Record");
            foreach (var dol in emv.LogFormat.GetDataObjectDefinitions())
            {
                var tagStr = $"{dol:T}";
                var def = _tlvDictionary.Get(tagStr);
                CardLog.Columns.Add(def is not null ? def.Name : tagStr);
            }

            byte recordNumber = 0;
            foreach (var tlvDataList in emv.LogRecords)
            {
                recordNumber++;
                var row = new List<string> { $"{recordNumber}" };

                foreach (var tlvData in tlvDataList)
                {
                    var tagStr = $"{tlvData:T}";
                    if (_tlvDictionary.Get(tagStr) is not null)
                    {
                        var tlvObject = _tlvDictionary.CreateInstance(tagStr);
                        tlvObject.Tlv = tlvData;
                        row.Add($"{tlvObject}");
                    }
                    else
                    {
                        var tlvObject = new BinaryTlvObject { Tlv = tlvData };
                        row.Add($"{tlvObject}");
                    }
                }

                CardLog.Rows.Add(row);
            }
        }

        private void UpdateTvr(EmvApplication emv)
        {
            // Byte 1
            Tvr.OfflineDataAuthenticationNotPerformed = emv.Tvr.OfflineDataAuthenticationNotPerformed;
            Tvr.SdaFailed = emv.Tvr.SdaFailed;
            Tvr.IccDataMissing = emv.Tvr.IccDataMissing;
            Tvr.TerminalExceptionFile = emv.Tvr.TerminalExceptionFile;
            Tvr.DdaFailed = emv.Tvr.DdaFailed;
            Tvr.CdaFailed = emv.Tvr.CdaFailed;

            // Byte 2
            Tvr.IccAndTerminalVersionsDifferent = emv.Tvr.IccAndTerminalVersionsDifferent;
            Tvr.ExpiredApplication = emv.Tvr.ExpiredApplication;
            Tvr.NotYetEffectiveApplication = emv.Tvr.NotYetEffectiveApplication;
            Tvr.ServiceNotAllowed = emv.Tvr.ServiceNotAllowed;
            Tvr.NewCard = emv.Tvr.NewCard;

            // Byte 3
            Tvr.CardholderVerificationFailed = emv.Tvr.CardholderVerificationFailed;
            Tvr.UnrecognisedCvm = emv.Tvr.UnrecognisedCvm;
            Tvr.PinTryLimitExceeded = emv.Tvr.PINTryLimitExceeded;
            Tvr.PinpadError = emv.Tvr.PinpadError;
            Tvr.PinNotEntered = emv.Tvr.PINNotEntered;
            Tvr.OnlinePinEntered = emv.Tvr.OnlinePinEntered;

            // Byte 4
            Tvr.TransactionExceedFloorLimit = emv.Tvr.TransactionExceedFloorLimit;
            Tvr.LowerConsecutiveOfflineLimitExceeded = emv.Tvr.LowerConsecutiveOfflineLimitExceeded;
            Tvr.UpperConsecutiveOfflineLimitExceeded = emv.Tvr.UpperConsecutiveOfflineLimitExceeded;
            Tvr.TransactionRandomlySelectedOnline = emv.Tvr.TransactionRandomlySelectedOnline;
            Tvr.MerchantForcedTransactionOnline = emv.Tvr.MerchantForcedTransactionOnline;

            // Byte 5
            Tvr.DefaultTdolUsed = emv.Tvr.DefaultTdolUsed;
            Tvr.IssuerAuthenticationFailed = emv.Tvr.IssuerAuthenticationFailed;
            Tvr.ScriptProcessingFailedBeforeGenerateAC = emv.Tvr.ScriptProcessingFailedBeforeGenerateAC;
            Tvr.ScriptProcessingFailedAfterGenerateAC = emv.Tvr.ScriptProcessingFailedAfterGenerateAC;
        }

        private void ResetSnapshots()
        {
            PublicKeys.Aid = string.Empty;
            PublicKeys.Rid = string.Empty;
            PublicKeys.CaIndex = string.Empty;
            PublicKeys.CaModulus = string.Empty;
            PublicKeys.CaExponent = string.Empty;
            PublicKeys.IssuerRecoveredData = string.Empty;
            PublicKeys.IssuerHashResult = string.Empty;
            PublicKeys.IssuerModulus = string.Empty;
            PublicKeys.IssuerExponent = string.Empty;
            PublicKeys.IccRecoveredData = string.Empty;
            PublicKeys.IccHashResult = string.Empty;
            PublicKeys.IccModulus = string.Empty;
            PublicKeys.IccExponent = string.Empty;

            Sda.RecoveredData = string.Empty;
            Sda.HashResult = string.Empty;
            Sda.DataAuthenticationCode = string.Empty;

            Dda.RecoveredData = string.Empty;
            Dda.HashResult = string.Empty;
            Dda.IccDynamicData = string.Empty;

            CardLog.IsPresent = false;
            CardLog.SfiHex = string.Empty;
            CardLog.LengthHex = string.Empty;
            CardLog.LogFormatText = string.Empty;
            CardLog.Columns.Clear();
            CardLog.Rows.Clear();

            Tvr.OfflineDataAuthenticationNotPerformed = false;
            Tvr.SdaFailed = false;
            Tvr.IccDataMissing = false;
            Tvr.TerminalExceptionFile = false;
            Tvr.DdaFailed = false;
            Tvr.CdaFailed = false;
            Tvr.IccAndTerminalVersionsDifferent = false;
            Tvr.ExpiredApplication = false;
            Tvr.NotYetEffectiveApplication = false;
            Tvr.ServiceNotAllowed = false;
            Tvr.NewCard = false;
            Tvr.CardholderVerificationFailed = false;
            Tvr.UnrecognisedCvm = false;
            Tvr.PinTryLimitExceeded = false;
            Tvr.PinpadError = false;
            Tvr.PinNotEntered = false;
            Tvr.OnlinePinEntered = false;
            Tvr.TransactionExceedFloorLimit = false;
            Tvr.LowerConsecutiveOfflineLimitExceeded = false;
            Tvr.UpperConsecutiveOfflineLimitExceeded = false;
            Tvr.TransactionRandomlySelectedOnline = false;
            Tvr.MerchantForcedTransactionOnline = false;
            Tvr.DefaultTdolUsed = false;
            Tvr.IssuerAuthenticationFailed = false;
            Tvr.ScriptProcessingFailedBeforeGenerateAC = false;
            Tvr.ScriptProcessingFailedAfterGenerateAC = false;
        }

        private bool EnsureValidChannel()
        {
            if (SharedData.IsValidChannel)
            {
                return true;
            }

            SetNotification("No Card Channel", "Connect to a smart card in WSCT GUI before running EMV commands.", "alert-warning");
            return false;
        }

        private void AppendDetailedLog(string text, EmvLogEntryKind kind)
        {
            _detailedLogs.Add(new EmvDetailedLogEntry(text, kind));
            NotifyStateChanged();
        }

        private void SetNotification(string title, string message, string alertClass)
        {
            Notification = new EmvNotification(title, message, alertClass);
            NotifyStateChanged();
        }

        private void HandleCardChannelChanged()
        {
            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            OnStateChanged?.Invoke();
        }

        private static string ResolveDataFile(string fileName)
        {
            var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            return File.Exists(basePath) ? basePath : fileName;
        }

        public void Dispose()
        {
            SharedData.CardChannelChangedEvent -= HandleCardChannelChanged;
        }
    }
}
