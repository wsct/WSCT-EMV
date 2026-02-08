using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using WSCT.EMV.Card;
using WSCT.EMV.Exceptions;
using WSCT.EMV.Objects;
using WSCT.EMV.Security;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.GUI.Plugins.EMVExplorer
{
    public partial class Gui : Form
    {
        #region >> Fields

        private readonly CertificationAuthorityRepository _certificationAuthorityRepository;
        private readonly DetailedLogs _detailedLogs;

        private readonly List<EmvApplication> _emvApplications;

        private readonly PluginConfiguration _pluginConfiguration;
        private readonly TlvDictionary _tlvDictionary;
        private EmvApplication _emv;
        private PaymentSystemEnvironment _pse;

        #endregion

        #region >> Constructor

        public Gui()
        {
            InitializeComponent();

            Icon = Common.Resources.Icons.WSCT;

            _pluginConfiguration = SerializedObject<PluginConfiguration>.LoadFromXml(@"Config.EMVExplorer.xml");

            _tlvDictionary = SerializedObject<TlvDictionary>.LoadFromXml(@"Dictionary.EMVTag.xml");

            _certificationAuthorityRepository = _pluginConfiguration.terminalConfiguration.CertificationAuthorityRepository;

            _detailedLogs = new DetailedLogs(this)
            {
                TlvDictionary = _tlvDictionary
            };

            guiPSEName.DataSource = _pluginConfiguration.terminalConfiguration.TerminalCapabilities.SupportedPses;
            guiPSEName.DisplayMember = "name";

            guiAC1Type.DataSource = Enum.GetValues<CryptogramType>();
            guiAC1Type.SelectedItem = CryptogramType.TC;

            _emvApplications = [];
        }

        #endregion

        #region >> Methods

        private static TreeNode ConvertTlvDataToTreeNode(TlvData tlv, TlvDictionary tlvManager)
        {
            TreeNode tlvNode;
            if (tlvManager != null && tlvManager.Get($"{tlv:T}") != null)
            {
                var tlvObject = tlvManager.CreateInstance(tlv);
                tlvObject.Tlv = tlv;
                tlvNode = new TreeNode($"{tlvObject:N}: {tlvObject}");
            }
            else
            {
                tlvNode = new TreeNode($"T:{tlv:T} L:{tlv:L} V:{tlv:Vh}");
            }
            foreach (var subTLV in tlv.InnerTlvs)
            {
                tlvNode.Nodes.Add(ConvertTlvDataToTreeNode(subTLV, tlvManager));
            }
            return tlvNode;
        }

        #endregion

        #region >> guiDo * Click

        private void GuiDoSelectPSE_Click(object sender, EventArgs e)
        {
            try
            {
                // Create the PSE object
                _pse = new PaymentSystemEnvironment(SharedData.CardChannel)
                {
                    Name = guiPSEName.Text,
                    // Adjust AID listing location
                    SearchTagAidInFci = guiParamsTagAIDInFCI.Checked
                };

                // Attach observers
                _detailedLogs.ObservePse(_pse);
                ObservePse(_pse);

                // Select and Read the PSE
                if (_pse.Select() == 0x9000)
                {
                    if (_pse.TlvFci.HasTag(0x88))
                    {
                        _pse.Read();
                    }
                }

                // Enable next steps
                ActivateEMVSelectAid();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "PSE Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GuiDoSelectAID_Click(object sender, EventArgs e)
        {
            // Get the EMV ApplicationID instance
            _emv = (EmvApplication)guiApplicationAID.SelectedItem;

            // Check if it is a valid instance
            if (_emv == null)
            {
                MessageBox.Show("An EMV application must be first selected.", "EMV application missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Set the Certification Authorities
            _emv.CertificationAuthorityRepository = _certificationAuthorityRepository;

            // Add default terminal data
            _emv.TlvTerminalData.AddRange(_pluginConfiguration.transactionContext.TlvDatas);

            // Select the PSE
            _emv.Select();

            // Enable next step
            ActivateEMVGetProcessingOptions();
        }

        private void GuiDoGetProcessingOptions_Click(object sender, EventArgs e)
        {
            // TODO: remove that patch ! (
            _emv.TlvTerminalData.Add(new TlvData(0x9F66, 0x02, [0x80, 0x00]));

            // Do Get Processing Options on EMV ApplicationID
            _emv.GetProcessingOptions();

            // Enable next step
            ActivateEMVReadRecord();
            ActivateEMVInternalAuthenticate();
        }

        private void GuiDoReadRecords_Click(object sender, EventArgs e)
        {
            // Read Records targetted by AFL
            _emv.ReadApplicationData();

            // Enable next step
            ActivateEMVCardholderVerification();
            ActivateEMVGenerateAC1();

            // Update GUI
            UpdateCVMList_Content();
        }

        private void GuiDoGetData_Click(object sender, EventArgs e)
        {
            // Get Data of the EMV ApplicationID
            _emv.GetData();
        }

        private void GuiDoExplicitDiscoveryOfAID_Click(object sender, EventArgs e)
        {
            try
            {
                // Try to select AID known by the terminal and undiscovered in PSE
                foreach (var app in _pluginConfiguration.terminalConfiguration.TerminalCapabilities.SupportedApplications)
                {
                    var notFound = true;
                    foreach (var emvFound in _emvApplications)
                    {
                        if (emvFound.Aid == app.Aid)
                        {
                            notFound = false;
                        }
                    }
                    // If AID not discovered, try to select it
                    if (notFound)
                    {
                        var emv = new EmvApplication(SharedData.CardChannel, new TlvData())
                        {
                            Aid = app.Aid
                        };
                        _detailedLogs.ObserveEmv(emv);
                        ObserveEmv(emv);
                        // If success, add the EMV ApplicationID instance to the candidate list
                        if (emv.Select() == 0x9000)
                        {
                            _emvApplications.Add(emv);
                        }
                    }
                }

                // Enable next step
                ActivateEMVSelectAid();

                // Update GUI
                UpdateApplicationsList();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "PSE Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GuiDoCardLogRead_Click(object sender, EventArgs e)
        {
            // Read EMV transactions log file
            _emv.ReadLogFile();
        }

        private void GuiDoCardLogSave_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                DefaultExt = "EMV Card Log",
                AddExtension = true,
                Filter = "Log files (*.log)|*.log"
            };
            dialog.ShowDialog();
            var fileName = dialog.FileName;
            if (fileName != "")
            {
                var sw = File.CreateText(fileName);
                foreach (ColumnHeader column in guiLogRecords.Columns)
                {
                    sw.Write("[{0}] ", column.Text);
                }
                sw.WriteLine();
                foreach (ListViewItem item in guiLogRecords.Items)
                {
                    foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                    {
                        sw.Write(subItem.Text + " ");
                    }
                    sw.WriteLine();
                }
                sw.Close();
            }
        }

        private void GuiDoSaveDetailedLogs_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                DefaultExt = "EMV Card Log",
                AddExtension = true,
                Filter = "Log files (*.log)|*.log"
            };
            dialog.ShowDialog();
            var fileName = dialog.FileName;
            if (fileName != "")
            {
                var sw = File.CreateText(fileName);
                sw.Write(guiDetailedLogs.Text);
                sw.Close();
            }
        }

        private void GuiDoInternalAuthenticate_Click(object sender, EventArgs e)
        {
            byte[] unpredictableNumber;
            try
            {
                unpredictableNumber = guiInternalAuthenticateUnpredictableNumber.Text.FromHexa();
            }
            catch (Exception)
            {
                MessageBox.Show("Bad format of unpredictable number. Should be hexadecimal.", "Format error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Internal authenticate for DDA card
            _emv.InternalAuthenticate(unpredictableNumber);
        }

        private void GuiDoVerifyCardholder_Click(object sender, EventArgs e)
        {
            var cvRule = (CardholderVerificationMethodList.CvRule)guiCVMList.SelectedItem;
            PINBlock pinBlock;
            if (cvRule.CvmCode == CardholderVerificationMethodList.CvmCode.PlaintextPinIcc
                || cvRule.CvmCode == CardholderVerificationMethodList.CvmCode.PlaintextPinIccAndSign)
            {
                if (guiPINEntry.Enabled)
                {
                    pinBlock = new PlaintextPINBlock
                    {
                        ClearPIN = guiPINEntry.Text.FromBcd((uint)guiPINEntry.Text.Length)
                    };
                    _emv.VerifyPin(pinBlock);
                }
                else
                {
                    MessageBox.Show("A PIN must be keyed in.", "PIN required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Unsupported cardholder verfication method.", "Unsupported CVM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GuiDoGetChallenge_Click(object sender, EventArgs e)
        {
            _emv.GetChallenge();
        }

        private void GuiDoGenerateAC1_Click(object sender, EventArgs e)
        {
            var cryptogramType = (CryptogramType)guiAC1Type.SelectedItem;
            if (cryptogramType == CryptogramType.Undefined)
            {
                MessageBox.Show($"Cryptogram type [{cryptogramType}] unsupported", "Unsupported feature", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                byte[] unpredictableNumber;
                try
                {
                    unpredictableNumber = guiAC1UnpredictableNumber.Text.FromHexa();
                }
                catch (Exception)
                {
                    MessageBox.Show("Bad format of unpredictable number. Should be hexadecimal.", "Format error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _emv.GenerateAc1(cryptogramType, unpredictableNumber);
            }
        }

        private void GuiDoExternalAuthenticate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature has not yet been implemented", "Not yet implemented", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void GuiDoGenerateAC2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature has not yet been implemented", "Not yet implemented", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        #endregion

        #region >> gui * CheckedChanged

        private void GuiPINEntryUsed_CheckedChanged(object sender, EventArgs e)
        {
            if (guiPINEntryUsed.Checked)
            {
                guiPINEntry.Enabled = true;
            }
            else
            {
                guiPINEntry.Enabled = false;
                guiPINEntry.Text = "";
            }
        }

        #endregion

        #region >> gui * IndexChanged

        private void GuiCVMList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cvRule = (CardholderVerificationMethodList.CvRule)guiCVMList.SelectedItem;
            guiDoGetChallenge.Enabled = cvRule.CvmCode switch
            {
                CardholderVerificationMethodList.CvmCode.EncipheredPinIcc => true,
                CardholderVerificationMethodList.CvmCode.EncipheredPinIccAndSign => true,
                _ => false
            };
        }

        #endregion

        #region >> update *

        private void UpdateApplicationsList()
        {
            guiApplicationAID.DataSource = null;
            guiApplicationAID.Items.Clear();
            guiApplicationAID.DataSource = _emvApplications;
            guiApplicationAID.DisplayMember = "aid";
        }

        private void UpdateAfterAIDSelect_Content(EmvDefinitionFile df)
        {
            var emv = (EmvApplication)df;

            var emvAppNode = new TreeNode(emv.Aid);

            var fciNode = new TreeNode("File Control Information");
            emvAppNode.Nodes.Add(fciNode);

            if (emv.TlvFci is not null)
            {
                fciNode.Nodes.Add(ConvertTlvDataToTreeNode(emv.TlvFci, _tlvDictionary));
            }
            else
            {
                var errorNode = new TreeNode($"EMV application [{emv.Aid}] not found.");
                fciNode.Nodes.Add(errorNode);
            }

            var optionsNode = new TreeNode("Processing Options");
            emvAppNode.Nodes.Add(optionsNode);

            if (emv.TlvProcessingOptions != null)
            {
                optionsNode.Nodes.Add(ConvertTlvDataToTreeNode(emv.TlvProcessingOptions, _tlvDictionary));
            }
            else
            {
                var errorNode = new TreeNode("Processing Options not found.");
                optionsNode.Nodes.Add(errorNode);
            }

            // >>> AIP
            var aipNode = new TreeNode("Application Interchange Profile");
            optionsNode.Nodes.Add(aipNode);

            try
            {
                aipNode.Nodes.Add($"{emv.Aip}");
            }
            catch (EMVApplicationException)
            {
                var errorNode = new TreeNode("AIP not found.");
                aipNode.Nodes.Add(errorNode);
            }

            // >>> AFL
            var aflNode = new TreeNode("Application File Locator");
            optionsNode.Nodes.Add(aflNode);

            try
            {
                foreach (var file in emv.Afl.Files)
                {
                    aflNode.Nodes.Add(new TreeNode($"{file}"));
                }
            }
            catch (EMVApplicationException)
            {
                var errorNode = new TreeNode("AFL not found.");
                aflNode.Nodes.Add(errorNode);
            }

            var recordsNode = new TreeNode("Records");
            emvAppNode.Nodes.Add(recordsNode);

            if (emv.TlvRecords.Count != 0)
            {
                byte recordNumber = 0;
                foreach (var tlv70 in emv.TlvRecords)
                {
                    recordNumber++;
                    var recordNode = new TreeNode($"Record {recordNumber}");
                    recordNode.Nodes.Add(ConvertTlvDataToTreeNode(tlv70, _tlvDictionary));
                    recordsNode.Nodes.Add(recordNode);
                }
            }
            else
            {
                var errorNode = new TreeNode("No records found.");
                recordsNode.Nodes.Add(errorNode);
            }

            var dataNode = new TreeNode("Get data");
            emvAppNode.Nodes.Add(dataNode);

            if (emv.TlvATC != null)
            {
                dataNode.Nodes.Add(ConvertTlvDataToTreeNode(emv.TlvATC, _tlvDictionary));
            }
            if (emv.TlvLastOnlineATCRegister != null)
            {
                dataNode.Nodes.Add(ConvertTlvDataToTreeNode(emv.TlvLastOnlineATCRegister, _tlvDictionary));
            }
            if (emv.TlvLogFormat != null)
            {
                dataNode.Nodes.Add(ConvertTlvDataToTreeNode(emv.TlvLogFormat, _tlvDictionary));
            }
            if (emv.TlvPINTryCounter != null)
            {
                dataNode.Nodes.Add(ConvertTlvDataToTreeNode(emv.TlvPINTryCounter, _tlvDictionary));
            }

            guiEMVApplicationsContent.Nodes.Clear();
            guiEMVApplicationsContent.Nodes.Add(emvAppNode);
            guiEMVApplicationsContent.ExpandAll();
        }

        private void UpdateAfterAIDSelect_Content(EmvApplication emv)
        {
            var emvAppNode = new TreeNode(emv.Aid);

            var fciNode = new TreeNode("File Control Information");
            emvAppNode.Nodes.Add(fciNode);

            if (emv.TlvFci is not null)
            {
                fciNode.Nodes.Add(ConvertTlvDataToTreeNode(emv.TlvFci, _tlvDictionary));
            }
            else
            {
                var errorNode = new TreeNode($"EMV application [{emv.Aid}] not found.");
                fciNode.Nodes.Add(errorNode);
            }

            var optionsNode = new TreeNode("Processing Options");
            emvAppNode.Nodes.Add(optionsNode);

            if (emv.TlvProcessingOptions != null)
            {
                optionsNode.Nodes.Add(ConvertTlvDataToTreeNode(emv.TlvProcessingOptions, _tlvDictionary));
            }
            else
            {
                var errorNode = new TreeNode("Processing Options not found.");
                optionsNode.Nodes.Add(errorNode);
            }

            var aipNode = new TreeNode("Application Interchange Profile");
            optionsNode.Nodes.Add(aipNode);

            if (emv.Aip != null)
            {
                aipNode.Nodes.Add($"{emv.Aip}");
            }
            else
            {
                var errorNode = new TreeNode("AIP not found.");
                aipNode.Nodes.Add(errorNode);
            }

            var aflNode = new TreeNode("Application File Locator");
            optionsNode.Nodes.Add(aflNode);

            if (emv.Afl != null)
            {
                foreach (var file in emv.Afl.Files)
                {
                    aflNode.Nodes.Add(new TreeNode($"{file}"));
                }
            }
            else
            {
                var errorNode = new TreeNode("AFL not found.");
                aflNode.Nodes.Add(errorNode);
            }

            var recordsNode = new TreeNode("Records");
            emvAppNode.Nodes.Add(recordsNode);

            if (emv.TlvRecords.Count != 0)
            {
                byte recordNumber = 0;
                foreach (var tlv70 in emv.TlvRecords)
                {
                    recordNumber++;
                    var recordNode = new TreeNode($"Record {recordNumber}");
                    recordNode.Nodes.Add(ConvertTlvDataToTreeNode(tlv70, _tlvDictionary));
                    recordsNode.Nodes.Add(recordNode);
                }
            }
            else
            {
                var errorNode = new TreeNode("No records found.");
                recordsNode.Nodes.Add(errorNode);
            }

            var dataNode = new TreeNode("Get data");
            emvAppNode.Nodes.Add(dataNode);

            if (emv.TlvATC != null)
            {
                dataNode.Nodes.Add(ConvertTlvDataToTreeNode(emv.TlvATC, _tlvDictionary));
            }
            if (emv.TlvLastOnlineATCRegister != null)
            {
                dataNode.Nodes.Add(ConvertTlvDataToTreeNode(emv.TlvLastOnlineATCRegister, _tlvDictionary));
            }
            if (emv.TlvLogFormat != null)
            {
                dataNode.Nodes.Add(ConvertTlvDataToTreeNode(emv.TlvLogFormat, _tlvDictionary));
            }
            if (emv.TlvPINTryCounter != null)
            {
                dataNode.Nodes.Add(ConvertTlvDataToTreeNode(emv.TlvPINTryCounter, _tlvDictionary));
            }

            var internalAuthenticateNode = new TreeNode("Internal Authenticate");
            emvAppNode.Nodes.Add(internalAuthenticateNode);

            if (emv.TlvSignedDynamicApplicationResponse != null)
            {
                internalAuthenticateNode.Nodes.Add(ConvertTlvDataToTreeNode(emv.TlvInternalAuthenticateUnpredictableNumber, _tlvDictionary));
                internalAuthenticateNode.Nodes.Add(ConvertTlvDataToTreeNode(emv.TlvSignedDynamicApplicationResponse, _tlvDictionary));
            }
            else
            {
                var errorNode = new TreeNode("Internal authenticate APDU not done or failed.");
                internalAuthenticateNode.Nodes.Add(errorNode);
            }

            var sdaNode = new TreeNode("Static Data Authentication");
            emvAppNode.Nodes.Add(sdaNode);

            if (emv.Sda != null)
            {
                sdaNode.Nodes.Add(new TreeNode($"Hash Algorithm Indicator: {emv.Sda.HashAlgorithmIndicator:X2}"));
                sdaNode.Nodes.Add(new TreeNode($"Hash Result: {emv.Sda.HashResult.ToHexa()}"));
                sdaNode.Nodes.Add(new TreeNode($"Data Authentication Code: {emv.Sda.DataAuthenticationCode.ToHexa()}"));
            }
            else
            {
                sdaNode.Nodes.Add("SDA not done, supported or cryptographic error");
            }

            var ddaNode = new TreeNode("Dynamic Data Authentication");
            emvAppNode.Nodes.Add(ddaNode);

            if (emv.Dda != null)
            {
                ddaNode.Nodes.Add(new TreeNode($"Hash Algorithm Indicator: {emv.Dda.HashAlgorithmIndicator:X2}"));
                ddaNode.Nodes.Add(new TreeNode($"Hash Result: {emv.Dda.HashResult.ToHexa()}"));
                ddaNode.Nodes.Add(new TreeNode($"ICC Dynamic Data Length:{emv.Dda.IccDynamicDataLength:X2}"));
                ddaNode.Nodes.Add(new TreeNode($"ICC Dynamic Data: {emv.Dda.IccDynamicData.ToHexa()}"));
            }
            else
            {
                ddaNode.Nodes.Add("DDA not done, supported or cryptographic error");
            }

            var pinNode = new TreeNode("Cardholder Verification");
            emvAppNode.Nodes.Add(pinNode);
            pinNode.Nodes.Add(new TreeNode($"TVR :: Cardholder Verification: {(emv.Tvr.CardholderVerificationFailed ? "failed" : "success")}"));
            if (emv.VerifyPinStatusWord != 0x0000)
            {
                pinNode.Nodes.Add(new TreeNode($"VERIFY PIN status: 0x{emv.VerifyPinStatusWord:X4}"));
            }

            var AC1Node = new TreeNode("Application Cryptogram 1");
            emvAppNode.Nodes.Add(AC1Node);
            if (emv.TlvGenerateAC1Response != null)
            {
                AC1Node.Nodes.Add(new TreeNode($"Requested AC: {emv.RequestedAC1Type}"));
                AC1Node.Nodes.Add(new TreeNode($"Unpredictable Number: {emv.TlvGenerateAC1UnpredictableNumber.Value.ToHexa()}"));
                var responseNode = new TreeNode($"Response: {emv.TlvGenerateAC1Response}");
                AC1Node.Nodes.Add(responseNode);
                responseNode.Nodes.Add(new TreeNode($"Cryptogram Information Data: {emv.Cid1}"));
                responseNode.Nodes.Add(new TreeNode($"Application Transaction Counter: {emv.AtcFromAC1}"));
            }

            guiEMVApplicationsContent.Nodes.Clear();
            guiEMVApplicationsContent.Nodes.Add(emvAppNode);
            guiEMVApplicationsContent.ExpandAll();
        }

        private void UpdateCVMList_Content()
        {
            if (_emv.CvmList != null)
            {
                guiCVMList.DataSource = _emv.CvmList.CvRules;
                guiCVMList.DisplayMember = "cvmCode";
            }
        }

        private void UpdateLogEntryAndFormat(EmvDefinitionFile df)
        {
            var emv = (EmvApplication)df;

            if (emv.LogFormat != null)
            {
                guiCardLogFormat.Text = $"{emv.LogFormat}";
            }

            if (emv.LogEntry != null)
            {
                guiCardLogSFI.Text = $"{emv.LogEntry.Sfi:X2}";
                guiCardLogLength.Text = $"{emv.LogEntry.CyclicFileSize:X2}";
            }

            if (emv.LogEntry != null && emv.LogFormat != null)
            {
                guiLogFilePresence.Checked = true;
                guiDoCardLogRead.Enabled = true;
            }
            else
            {
                guiLogFilePresence.Checked = false;
                guiDoCardLogRead.Enabled = false;
            }
        }

        private void UpdateLogRecords(EmvApplication emv)
        {
            if (emv.LogRecords == null)
            {
                return;
            }

            // Initialize the output
            guiLogRecords.Columns.Clear();
            guiLogRecords.Items.Clear();

            // Set column names. If tag is known by tlvManager, its name is used instead of the tag number.
            guiLogRecords.Columns.Add("Record");
            foreach (var dol in emv.LogFormat.GetDataObjectDefinitions())
            {
                var tagStr = $"{dol:T}";
                if (_tlvDictionary.Get(tagStr) != null)
                {
                    guiLogRecords.Columns.Add(_tlvDictionary.Get(tagStr).Name);
                }
                else
                {
                    guiLogRecords.Columns.Add(tagStr);
                }
            }

            // Fill the list view: 1 line (list view item) by record
            byte recordNumber = 0;
            foreach (var tlvDataList in emv.LogRecords)
            {
                recordNumber++;
                var item = new ListViewItem($"{recordNumber}");
                foreach (var tlvData in tlvDataList)
                {
                    // if tag is known by tlvManager, use the corresponding AbstractTLVObject else use BinaryTlvObject.
                    var tagStr = $"{tlvData:T}";
                    if (_tlvDictionary.Get(tagStr) != null)
                    {
                        var tlvObject = _tlvDictionary.CreateInstance(tagStr);
                        tlvObject.Tlv = tlvData;
                        item.SubItems.Add($"{tlvObject}");
                    }
                    else
                    {
                        AbstractTlvObject tlvObject = new BinaryTlvObject
                        {
                            Tlv = tlvData
                        };
                        item.SubItems.Add($"{tlvObject}");
                    }
                }
                guiLogRecords.Items.Add(item);
            }
            guiLogRecords.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            guiDoCardLogSave.Enabled = true;
        }

        private void UpdatePublicKeysTab(EmvApplication emv)
        {
            // Certification Authority Public Key
            var aidObject = new ApplicationIdentifier(emv.Aid);
            guiPublicKeysAID.Text = emv.Aid;
            guiPublicKeysRID.Text = aidObject.Rid;

            if (emv.TlvDataRecords.HasTag(0x8F))
            {
                guiPublicKeysCAIndex.Text = emv.TlvDataRecords.GetTag(0x8F).Value.ToHexa();
            }

            if (emv.CertificationAuthorityPublicKey != null)
            {
                guiPublicKeysCertificationAuthorityPKModulus.Text = emv.CertificationAuthorityPublicKey.Modulus;
                guiPublicKeysCertificationAuthorityPKExponent.Text = emv.CertificationAuthorityPublicKey.Exponent;
            }

            // Issuer Public Key
            if (emv.IssuerPublicKeyCertificate != null)
            {
                guiPublicKeysIssuerPKRecoveredData.Text = emv.IssuerPublicKeyCertificate.Recovered.ToHexa('\0');
                guiPublicKeysIssuerPKHashResult.Text = emv.IssuerPublicKeyCertificate.HashResult.ToHexa();
                guiPublicKeysIssuerPKModulus.Text = emv.IssuerPublicKey.Modulus;
                guiPublicKeysIssuerPKExponent.Text = emv.IssuerPublicKey.Exponent;
            }

            // ICC Public Key
            if (emv.IccPublicKeyCertificate != null)
            {
                guiPublicKeysICCPKRecoveredData.Text = emv.IccPublicKeyCertificate.Recovered.ToHexa('\0');
                guiPublicKeysICCPKHashResult.Text = emv.IccPublicKeyCertificate.HashResult.ToHexa();
                guiPublicKeysICCPKModulus.Text = emv.IccPublicKey.Modulus;
                guiPublicKeysICCPKExponent.Text = emv.IccPublicKey.Exponent;
            }
        }

        private void UpdateAuthenticationTabSignedData(EmvApplication emv)
        {
            if (emv.TlvOfflineRecords != null)
            {
                var index = 0;
                foreach (var tlv in emv.TlvOfflineRecords)
                {
                    index++;
                    var item = new ListViewItem($"{index}");
                    // tag
                    item.SubItems.Add($"{tlv:T}");
                    // name
                    if (_tlvDictionary != null && _tlvDictionary.Get($"{tlv:T}") != null)
                    {
                        var tlvObject = _tlvDictionary.CreateInstance(tlv);
                        tlvObject.Tlv = tlv;
                        item.SubItems.Add($"{tlvObject:N}");
                    }
                    else
                    {
                        item.SubItems.Add("");
                    }
                    // value
                    item.SubItems.Add($"{tlv:V}");
                    // insert item in SDA, DDA and/or CDA list
                    if (emv.Aip.Sda)
                    {
                        guiSDASignedData.Items.Add((ListViewItem)item.Clone());
                    }
                    if (emv.Aip.Dda)
                    {
                        guiDDASignedData.Items.Add((ListViewItem)item.Clone());
                    }
                    if (emv.Aip.Cda)
                    {
                        guiCDASignedData.Items.Add((ListViewItem)item.Clone());
                    }
                }
                //if (emv.tlvDataRecords.hasTag(0x9F4A))
                //{
                //    DataObjectList sdaTagList = new DataObjectList();
                //    sdaTagList.tlv = emv.tlvDataRecords.getTag(0x9F4A);
                //}
            }
        }

        private void UpdateAuthenticationTabSdaDone(EmvApplication emv)
        {
            if (emv.Sda != null)
            {
                guiSDARecoveredData.Text = emv.Sda.Recovered.ToHexa('\0');
                guiSDAHashResult.Text = emv.Sda.HashResult.ToHexa();
                guiSDADataAuthenticationCode.Text = emv.Sda.DataAuthenticationCode.ToHexa();
            }
        }

        private void UpdateAuthenticationTabDdaDone(EmvApplication emv)
        {
            if (emv.Dda != null)
            {
                guiDDARecoveredData.Text = emv.Dda.Recovered.ToHexa('\0');
                guiDDAHashResult.Text = emv.Dda.HashResult.ToHexa();
                guiDDAICCDynamicData.Text = emv.Dda.IccDynamicData.ToHexa();
            }
        }

        private void UpdateAfterPSESelect_Content(EmvDefinitionFile df)
        {
            var pse = (PaymentSystemEnvironment)df;

            // Update list of applications found in PSE
            foreach (var emvFound in pse.GetApplications())
            {
                _detailedLogs.ObserveEmv(emvFound);
                ObserveEmv(emvFound);
                _emvApplications.Add(emvFound);
            }
            UpdateApplicationsList();

            var pseNode = new TreeNode(pse.Name);

            var fciNode = new TreeNode("File Control Information");
            pseNode.Nodes.Add(fciNode);

            if (pse.TlvFci is not null)
            {
                fciNode.Nodes.Add(ConvertTlvDataToTreeNode(pse.TlvFci, _tlvDictionary));
            }
            else
            {
                var errorNode = new TreeNode($"PSE named '{pse.Name}'/[{pse.Aid}] not found.");
                fciNode.Nodes.Add(errorNode);
            }

            guiPSEContent.Nodes.Add(pseNode);
            guiPSEContent.ExpandAll();
        }

        private void UpdateAfterPSEReadRecords_Content(EmvDefinitionFile df)
        {
            var pse = (PaymentSystemEnvironment)df;

            // Update list of applications found in PSE
            foreach (var emvFound in pse.GetApplications())
            {
                _detailedLogs.ObserveEmv(emvFound);
                ObserveEmv(emvFound);
                _emvApplications.Add(emvFound);
            }
            UpdateApplicationsList();

            // Update content of the PSE Records
            var pseNode = new TreeNode(pse.Name);

            var recordsNode = new TreeNode("Records");
            pseNode.Nodes.Add(recordsNode);

            if (pse.TlvRecords.Count != 0)
            {
                byte recordNumber = 0;
                foreach (var tlv70 in pse.TlvRecords)
                {
                    recordNumber++;
                    var recordNode = new TreeNode($"Record {recordNumber}");
                    recordNode.Nodes.Add(ConvertTlvDataToTreeNode(tlv70, _tlvDictionary));
                    recordsNode.Nodes.Add(recordNode);
                }
            }
            else
            {
                var errorNode = new TreeNode("No record found in PSE.");
                recordsNode.Nodes.Add(errorNode);
            }

            guiPSEContent.Nodes.Add(pseNode);
            guiPSEContent.ExpandAll();
        }

        private void UpdateTvr(EmvApplication emv)
        {
            // byte 1
            guiTVR_1_1.Checked = emv.Tvr.OfflineDataAuthenticationNotPerformed;
            guiTVR_1_2.Checked = emv.Tvr.SdaFailed;
            guiTVR1_3.Checked = emv.Tvr.IccDataMissing;
            guiTVR1_4.Checked = emv.Tvr.TerminalExceptionFile;
            guiTVR1_5.Checked = emv.Tvr.DdaFailed;
            guiTVR1_6.Checked = emv.Tvr.CdaFailed;

            // byte 2
            guiTVR2_1.Checked = emv.Tvr.IccAndTerminalVersionsDifferent;
            guiTVR2_2.Checked = emv.Tvr.ExpiredApplication;
            guiTVR2_3.Checked = emv.Tvr.NotYetEffectiveApplication;
            guiTVR2_4.Checked = emv.Tvr.ServiceNotAllowed;
            guiTVR2_5.Checked = emv.Tvr.NewCard;

            // byte 3
            guiTVR3_1.Checked = emv.Tvr.CardholderVerificationFailed;
            guiTVR3_2.Checked = emv.Tvr.UnrecognisedCvm;
            guiTVR3_3.Checked = emv.Tvr.PINTryLimitExceeded;
            guiTVR3_4.Checked = emv.Tvr.PinpadError;
            guiTVR3_5.Checked = emv.Tvr.PINNotEntered;
            guiTVR3_6.Checked = emv.Tvr.OnlinePinEntered;

            // byte 4
            guiTVR4_1.Checked = emv.Tvr.TransactionExceedFloorLimit;
            guiTVR4_2.Checked = emv.Tvr.LowerConsecutiveOfflineLimitExceeded;
            guiTVR4_3.Checked = emv.Tvr.UpperConsecutiveOfflineLimitExceeded;
            guiTVR4_4.Checked = emv.Tvr.TransactionRandomlySelectedOnline;
            guiTVR4_5.Checked = emv.Tvr.MerchantForcedTransactionOnline;

            // byte 5
            guiTVR5_1.Checked = emv.Tvr.DefaultTdolUsed;
            guiTVR5_2.Checked = emv.Tvr.IssuerAuthenticationFailed;
            guiTVR5_3.Checked = emv.Tvr.ScriptProcessingFailedBeforeGenerateAC;
            guiTVR5_4.Checked = emv.Tvr.ScriptProcessingFailedAfterGenerateAC;
        }

        #endregion

        #region >> ActivateEMV*

        private void ActivateEMVSelectAid()
        {
            if (_emvApplications.Count != 0)
            {
                guiApplicationAID.Enabled = true;
                guiDoSelectAID.Enabled = true;
            }
        }

        private void ActivateEMVGetProcessingOptions()
        {
            if (_emv.TlvFci is not null)
            {
                guiDoGetProcessingOptions.Enabled = true;
                guiDoGetData.Enabled = true;
            }
        }

        private void ActivateEMVReadRecord()
        {
            if (_emv.TlvProcessingOptions != null)
            {
                guiDoReadRecords.Enabled = true;
            }
        }

        private void ActivateEMVInternalAuthenticate()
        {
            if (_emv.Aip != null && _emv.Aip.Dda)
            {
                // Generates a new unpredictable number
                var unpredictableNumber = new byte[4];
                new Random().NextBytes(unpredictableNumber);
                guiInternalAuthenticateUnpredictableNumber.Text = unpredictableNumber.ToHexa('\0');

                guiInternalAuthenticateUnpredictableNumber.Enabled = true;
                guiDoInternalAuthenticate.Enabled = true;
            }
        }

        private void ActivateEMVCardholderVerification()
        {
            if (_emv.CvmList != null)
            {
                guiPINEntryUsed.Enabled = true;
                guiCVMList.Enabled = true;
                guiDoVerifyCardholder.Enabled = true;
            }
        }

        private void ActivateEMVGenerateAC1()
        {
            guiAC1UnpredictableNumber.Enabled = true;
            guiAC1Type.Enabled = true;
            guiDoGenerateAC1.Enabled = true;
        }

        #endregion

        #region >> *EventHandler

        private void AfterPSESelectEventHandler(Object sender, EmvEventArgs eventArgs)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => AfterPSESelectEventHandler(sender, eventArgs)));
                return;
            }

            var df = sender as EmvDefinitionFile;
            UpdateAfterPSESelect_Content(df);
        }

        private void AfterPSEReadEventHandler(Object sender, EmvEventArgs eventArgs)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => AfterPSEReadEventHandler(sender, eventArgs)));
                return;
            }

            var pse = sender as PaymentSystemEnvironment;
            UpdateAfterPSEReadRecords_Content(pse);
        }

        private void AfterEMVSelectEventHandler(Object sender, EmvEventArgs eventArgs)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => AfterEMVSelectEventHandler(sender, eventArgs)));
                return;
            }

            var df = sender as EmvDefinitionFile;
            UpdateAfterAIDSelect_Content(df);
            UpdateLogEntryAndFormat(df);
        }

        private void AfterGetProcessingOptionsEventHandler(Object sender, EmvEventArgs eventArgs)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => AfterGetProcessingOptionsEventHandler(sender, eventArgs)));
                return;
            }

            var emv = sender as EmvApplication;
            UpdateAfterAIDSelect_Content(emv);
            UpdateTvr(emv);
        }

        private void AfterReadApplicationDataEventHandler(Object sender, EmvEventArgs eventArgs)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => AfterReadApplicationDataEventHandler(sender, eventArgs)));
                return;
            }

            var emv = sender as EmvApplication;
            UpdateAfterAIDSelect_Content(emv);
            UpdatePublicKeysTab(emv);
            UpdateAuthenticationTabSdaDone(emv);
            UpdateAuthenticationTabSignedData(emv);
            UpdateTvr(emv);
        }

        private void AfterGetDataEventHandler(Object sender, EmvEventArgs eventArgs)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => AfterGetDataEventHandler(sender, eventArgs)));
                return;
            }

            var emv = sender as EmvApplication;
            UpdateAfterAIDSelect_Content(emv);
            UpdateLogEntryAndFormat(emv);
            UpdateTvr(emv);
        }

        private void AfterReadLogFileEventHandler(Object sender, EmvEventArgs eventArgs)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => AfterReadLogFileEventHandler(sender, eventArgs)));
                return;
            }

            var emv = sender as EmvApplication;
            UpdateLogRecords(emv);
            UpdateTvr(emv);
        }

        private void AfterInternalAuthenticateEventHandler(Object sender, EmvEventArgs eventArgs)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => AfterInternalAuthenticateEventHandler(sender, eventArgs)));
                return;
            }

            var emv = sender as EmvApplication;
            UpdateAfterAIDSelect_Content(emv);
            UpdateAuthenticationTabDdaDone(emv);
            UpdateTvr(emv);
        }

        private void AfterVerifyPinEventHandler(Object sender, EmvEventArgs eventArgs)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => AfterVerifyPinEventHandler(sender, eventArgs)));
                return;
            }

            UpdateAfterAIDSelect_Content(sender as EmvApplication);
        }

        private void AfterGenerateAC1EventHandler(Object sender, EmvEventArgs eventArgs)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => AfterGenerateAC1EventHandler(sender, eventArgs)));
                return;
            }

            var emv = sender as EmvApplication;
            UpdateAfterAIDSelect_Content(emv);
            UpdateTvr(emv);
        }

        #endregion

        #region >> observe*

        private void ObservePse(PaymentSystemEnvironment pse)
        {
            pse.AfterSelectEvent += AfterPSESelectEventHandler;
            pse.AfterReadEvent += AfterPSEReadEventHandler;
        }

        private void ObserveEmv(EmvApplication emv)
        {
            emv.AfterSelectEvent += AfterEMVSelectEventHandler;
            emv.AfterGetProcessingOptionsEvent += AfterGetProcessingOptionsEventHandler;
            emv.AfterReadApplicationDataEvent += AfterReadApplicationDataEventHandler;
            emv.AfterGetDataEvent += AfterGetDataEventHandler;
            emv.AfterReadLogFileEvent += AfterReadLogFileEventHandler;
            emv.AfterInternalAuthenticateEvent += AfterInternalAuthenticateEventHandler;
            emv.AfterVerifyPinEvent += AfterVerifyPinEventHandler;
            emv.AfterGenerateAC1Event += AfterGenerateAC1EventHandler;
        }

        #endregion
    }
}