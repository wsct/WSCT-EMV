using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using WSCT.EMV;
using WSCT.EMV.Card;
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

        private readonly List<EMVApplication> _emvApplications;

        private readonly PluginConfiguration _pluginConfiguration;
        private readonly TlvDictionary _tlvDictionary;
        private EMVApplication _emv;
        private PaymentSystemEnvironment _pse;

        #endregion

        #region >> Constructor

        public Gui()
        {
            InitializeComponent();

            _pluginConfiguration = SerializedObject<PluginConfiguration>.LoadFromXml(@"Config.EMVExplorer.xml");

            _tlvDictionary = SerializedObject<TlvDictionary>.LoadFromXml(@"Dictionary.EMVTag.xml");

            _certificationAuthorityRepository = _pluginConfiguration.terminalConfiguration.CertificationAuthorityRepository;

            _detailedLogs = new DetailedLogs(this);
            _detailedLogs.TlvDictionary = _tlvDictionary;

            guiPSEName.DataSource = _pluginConfiguration.terminalConfiguration.TerminalCapabilities.SupportedPses;
            guiPSEName.DisplayMember = "name";

            guiAC1Type.DataSource = Enum.GetValues(typeof(CryptogramType));
            guiAC1Type.SelectedItem = CryptogramType.TC;

            _emvApplications = new List<EMVApplication>();
        }

        #endregion

        #region >> Methods

        private TreeNode convertTLVDataToTreeNode(TlvData tlv)
        {
            return convertTLVDataToTreeNode(tlv, null);
        }

        private TreeNode convertTLVDataToTreeNode(TlvData tlv, TlvDictionary tlvManager)
        {
            TreeNode tlvNode;
            if (tlvManager != null && tlvManager.Get(String.Format("{0:T}", tlv)) != null)
            {
                var tlvObject = tlvManager.CreateInstance(tlv);
                tlvObject.Tlv = tlv;
                tlvNode = new TreeNode(String.Format("{0:N}: {0}", tlvObject));
            }
            else
            {
                tlvNode = new TreeNode(String.Format("T:{0:T} L:{0:L} V:{0:Vh}", tlv));
            }
            foreach (var subTLV in tlv.SubFields)
            {
                tlvNode.Nodes.Add(convertTLVDataToTreeNode(subTLV, tlvManager));
            }
            return tlvNode;
        }

        #endregion

        #region >> guiDo * Click

        private void guiDoSelectPSE_Click(object sender, EventArgs e)
        {
            try
            {
                // Create the PSE object
                _pse = new PaymentSystemEnvironment(SharedData.cardChannel);
                _pse.Name = guiPSEName.Text;

                // Adjust AID listing location
                _pse.SearchTagAidInFci = guiParamsTagAIDInFCI.Checked;

                // Attach observers
                _detailedLogs.ObservePSE(_pse);
                observePSE(_pse);

                // Select and Read the PSE
                if (_pse.Select() == 0x9000)
                {
                    if (_pse.TlvFci.HasTag(0x88))
                    {
                        _pse.Read();
                    }
                }

                // Enable next steps
                activateEMVSelectAID();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "PSE Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guiDoSelectAID_Click(object sender, EventArgs e)
        {
            // Get the EMV ApplicationID instance
            _emv = (EMVApplication)guiApplicationAID.SelectedItem;

            // Check if it is a valid instance
            if (_emv == null)
            {
                MessageBox.Show("An EMV application must be first selected.", "EMV application missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Set the Certification Authorities
            _emv.CertificationAuthorityRepository = _certificationAuthorityRepository;

            // Select the PSE
            _emv.Select();

            // Enable next step
            activateEMVGetProcessingOptions();
        }

        private void guiDoGetProcessingOptions_Click(object sender, EventArgs e)
        {
            // TODO: remove that patch ! (
            _emv.TlvTerminalData.Add(new TlvData(0x9F66, 0x02, new byte[2] { 0x80, 0x00 }));

            // Do Get Processing Options on EMV ApplicationID
            _emv.GetProcessingOptions();

            // Enable next step
            activateEMVReadRecord();
            activateEMVInternalAuthenticate();
        }

        private void guiDoReadRecords_Click(object sender, EventArgs e)
        {
            // Read Records targetted by AFL
            _emv.ReadApplicationData();

            // Enable next step
            activateEMVCardholderVerification();
            activateEMVGenerateAC1();

            // Update GUI
            updateCVMList_Content();
        }

        private void guiDoGetData_Click(object sender, EventArgs e)
        {
            // Get Data of the EMV ApplicationID
            _emv.GetData();
        }

        private void guiDoExplicitDiscoveryOfAID_Click(object sender, EventArgs e)
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
                        var emv = new EMVApplication(SharedData.cardChannel, new TlvData());
                        emv.Aid = app.Aid;
                        _detailedLogs.ObserveEMV(emv);
                        observeEMV(emv);
                        // If success, add the EMV ApplicationID instance to the candidate list
                        if (emv.Select() == 0x9000)
                        {
                            _emvApplications.Add(emv);
                        }
                    }
                }

                // Enable next step
                activateEMVSelectAID();

                // Update GUI
                updateApplicationsList();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "PSE Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guiDoCardLogRead_Click(object sender, EventArgs e)
        {
            // Read EMV transactions log file
            _emv.ReadLogFile();
        }

        private void guiDoCardLogSave_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = "EMV Card Log";
            dialog.AddExtension = true;
            dialog.Filter = "Log files (*.log)|*.log";
            dialog.ShowDialog();
            var fileName = dialog.FileName;
            if (fileName != "")
            {
                var sw = File.CreateText(fileName);
                foreach (ColumnHeader column in guiLogRecords.Columns)
                {
                    sw.Write(String.Format("[{0}] ", column.Text));
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

        private void guiDoSaveDetailedLogs_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = "EMV Card Log";
            dialog.AddExtension = true;
            dialog.Filter = "Log files (*.log)|*.log";
            dialog.ShowDialog();
            var fileName = dialog.FileName;
            if (fileName != "")
            {
                var sw = File.CreateText(fileName);
                sw.Write(guiDetailedLogs.Text);
                sw.Close();
            }
        }

        private void guiDoInternalAuthenticate_Click(object sender, EventArgs e)
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

        private void guiDoVerifyCardholder_Click(object sender, EventArgs e)
        {
            var cvRule = (CardholderVerificationMethodList.CvRule)guiCVMList.SelectedItem;
            PINBlock pinBlock;
            if (cvRule.CvmCode == CardholderVerificationMethodList.CvmCode.PlaintextpinIcc
                || cvRule.CvmCode == CardholderVerificationMethodList.CvmCode.PlaintextpinIccAndSign)
            {
                if (guiPINEntry.Enabled)
                {
                    pinBlock = new PlaintextPINBlock();
                    pinBlock.ClearPIN = guiPINEntry.Text.FromBcd((UInt32)guiPINEntry.Text.Length);
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

        private void guiDoGetChallenge_Click(object sender, EventArgs e)
        {
            _emv.GetChallenge();
        }

        private void guiDoGenerateAC1_Click(object sender, EventArgs e)
        {
            var cryptogramType = (CryptogramType)guiAC1Type.SelectedItem;
            if (cryptogramType == CryptogramType.Undefined)
            {
                MessageBox.Show(String.Format("Cryptogram type [{0}] unsupported", cryptogramType), "Unsupported feature", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                // Add default terminal data
                _emv.TlvTerminalData.AddRange(_pluginConfiguration.transactionContext.TlvDatas);

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

        private void guiDoExternalAuthenticate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature has not yet been implemented", "Not yet implemented", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void guiDoGenerateAC2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature has not yet been implemented", "Not yet implemented", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        #endregion

        #region >> gui * CheckedChanged

        private void guiPINEntryUsed_CheckedChanged(object sender, EventArgs e)
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

        private void guiCVMList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cvRule = (CardholderVerificationMethodList.CvRule)guiCVMList.SelectedItem;
            switch (cvRule.CvmCode)
            {
                case CardholderVerificationMethodList.CvmCode.EncipheredpinIcc:
                case CardholderVerificationMethodList.CvmCode.EncipheredpinIccAndSign:
                    guiDoGetChallenge.Enabled = true;
                    break;
                default:
                    guiDoGetChallenge.Enabled = false;
                    break;
            }
        }

        #endregion

        #region >> update *

        private void updateApplicationsList()
        {
            guiApplicationAID.DataSource = null;
            guiApplicationAID.Items.Clear();
            guiApplicationAID.DataSource = _emvApplications;
            guiApplicationAID.DisplayMember = "aid";
        }

        private void updateAfterAIDSelect_Content(EMVDefinitionFile df)
        {
            var emv = (EMVApplication)df;

            var emvAppNode = new TreeNode(emv.Aid);

            var fciNode = new TreeNode("File Control Information");
            emvAppNode.Nodes.Add(fciNode);

            if (emv.TlvFci != null)
            {
                fciNode.Nodes.Add(convertTLVDataToTreeNode(emv.TlvFci, _tlvDictionary));
            }
            else
            {
                var errorNode = new TreeNode(String.Format("EMV application [{0}] not found.", emv.Aid));
                fciNode.Nodes.Add(errorNode);
            }

            var optionsNode = new TreeNode("Processing Options");
            emvAppNode.Nodes.Add(optionsNode);

            if (emv.TlvProcessingOptions != null)
            {
                optionsNode.Nodes.Add(convertTLVDataToTreeNode(emv.TlvProcessingOptions, _tlvDictionary));
            }
            else
            {
                var errorNode = new TreeNode(String.Format("Processing Options not found."));
                optionsNode.Nodes.Add(errorNode);
            }

            var aipNode = new TreeNode("Application Interchange Profile");
            optionsNode.Nodes.Add(aipNode);

            if (emv.Aip != null)
            {
                aipNode.Nodes.Add(String.Format("{0}", emv.Aip));
            }
            else
            {
                var errorNode = new TreeNode(String.Format("AIP not found."));
                aipNode.Nodes.Add(errorNode);
            }

            var aflNode = new TreeNode("Application File Locator");
            optionsNode.Nodes.Add(aflNode);

            if (emv.Afl != null)
            {
                foreach (var file in emv.Afl.GetFiles())
                {
                    aflNode.Nodes.Add(new TreeNode(String.Format("{0}", file)));
                }
            }
            else
            {
                var errorNode = new TreeNode(String.Format("AFL not found."));
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
                    var recordNode = new TreeNode(String.Format("Record {0}", recordNumber));
                    recordNode.Nodes.Add(convertTLVDataToTreeNode(tlv70, _tlvDictionary));
                    recordsNode.Nodes.Add(recordNode);
                }
            }
            else
            {
                var errorNode = new TreeNode(String.Format("No records found."));
                recordsNode.Nodes.Add(errorNode);
            }

            var dataNode = new TreeNode("Get data");
            emvAppNode.Nodes.Add(dataNode);

            if (emv.TlvATC != null)
            {
                dataNode.Nodes.Add(convertTLVDataToTreeNode(emv.TlvATC, _tlvDictionary));
            }
            if (emv.TlvLastOnlineATCRegister != null)
            {
                dataNode.Nodes.Add(convertTLVDataToTreeNode(emv.TlvLastOnlineATCRegister, _tlvDictionary));
            }
            if (emv.TlvLogFormat != null)
            {
                dataNode.Nodes.Add(convertTLVDataToTreeNode(emv.TlvLogFormat, _tlvDictionary));
            }
            if (emv.TlvPINTryCounter != null)
            {
                dataNode.Nodes.Add(convertTLVDataToTreeNode(emv.TlvPINTryCounter, _tlvDictionary));
            }

            guiEMVApplicationsContent.Nodes.Clear();
            guiEMVApplicationsContent.Nodes.Add(emvAppNode);
            guiEMVApplicationsContent.ExpandAll();
        }

        private void updateAfterAIDSelect_Content(EMVApplication emv)
        {
            var emvAppNode = new TreeNode(emv.Aid);

            var fciNode = new TreeNode("File Control Information");
            emvAppNode.Nodes.Add(fciNode);

            if (emv.TlvFci != null)
            {
                fciNode.Nodes.Add(convertTLVDataToTreeNode(emv.TlvFci, _tlvDictionary));
            }
            else
            {
                var errorNode = new TreeNode(String.Format("EMV application [{0}] not found.", emv.Aid));
                fciNode.Nodes.Add(errorNode);
            }

            var optionsNode = new TreeNode("Processing Options");
            emvAppNode.Nodes.Add(optionsNode);

            if (emv.TlvProcessingOptions != null)
            {
                optionsNode.Nodes.Add(convertTLVDataToTreeNode(emv.TlvProcessingOptions, _tlvDictionary));
            }
            else
            {
                var errorNode = new TreeNode(String.Format("Processing Options not found."));
                optionsNode.Nodes.Add(errorNode);
            }

            var aipNode = new TreeNode("Application Interchange Profile");
            optionsNode.Nodes.Add(aipNode);

            if (emv.Aip != null)
            {
                aipNode.Nodes.Add(String.Format("{0}", emv.Aip));
            }
            else
            {
                var errorNode = new TreeNode(String.Format("AIP not found."));
                aipNode.Nodes.Add(errorNode);
            }

            var aflNode = new TreeNode("Application File Locator");
            optionsNode.Nodes.Add(aflNode);

            if (emv.Afl != null)
            {
                foreach (var file in emv.Afl.GetFiles())
                {
                    aflNode.Nodes.Add(new TreeNode(String.Format("{0}", file)));
                }
            }
            else
            {
                var errorNode = new TreeNode(String.Format("AFL not found."));
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
                    var recordNode = new TreeNode(String.Format("Record {0}", recordNumber));
                    recordNode.Nodes.Add(convertTLVDataToTreeNode(tlv70, _tlvDictionary));
                    recordsNode.Nodes.Add(recordNode);
                }
            }
            else
            {
                var errorNode = new TreeNode(String.Format("No records found."));
                recordsNode.Nodes.Add(errorNode);
            }

            var dataNode = new TreeNode("Get data");
            emvAppNode.Nodes.Add(dataNode);

            if (emv.TlvATC != null)
            {
                dataNode.Nodes.Add(convertTLVDataToTreeNode(emv.TlvATC, _tlvDictionary));
            }
            if (emv.TlvLastOnlineATCRegister != null)
            {
                dataNode.Nodes.Add(convertTLVDataToTreeNode(emv.TlvLastOnlineATCRegister, _tlvDictionary));
            }
            if (emv.TlvLogFormat != null)
            {
                dataNode.Nodes.Add(convertTLVDataToTreeNode(emv.TlvLogFormat, _tlvDictionary));
            }
            if (emv.TlvPINTryCounter != null)
            {
                dataNode.Nodes.Add(convertTLVDataToTreeNode(emv.TlvPINTryCounter, _tlvDictionary));
            }

            var internalAuthenticateNode = new TreeNode("Internal Authenticate");
            emvAppNode.Nodes.Add(internalAuthenticateNode);

            if (emv.TlvSignedDynamicApplicationResponse != null)
            {
                internalAuthenticateNode.Nodes.Add(convertTLVDataToTreeNode(emv.TlvInternalAuthenticateUnpredictableNumber, _tlvDictionary));
                internalAuthenticateNode.Nodes.Add(convertTLVDataToTreeNode(emv.TlvSignedDynamicApplicationResponse, _tlvDictionary));
            }
            else
            {
                var errorNode = new TreeNode("Internal authenticate APDU not done of failed.");
                internalAuthenticateNode.Nodes.Add(errorNode);
            }

            var sdaNode = new TreeNode("Static Data Authentication");
            emvAppNode.Nodes.Add(sdaNode);

            if (emv.Sda != null)
            {
                sdaNode.Nodes.Add(new TreeNode(String.Format("Hash Algorithm Indicator: {0:X2}", emv.Sda.HashAlgorithmIndicator)));
                sdaNode.Nodes.Add(new TreeNode(String.Format("Hash Result: {0}", emv.Sda.HashResult.ToHexa())));
                sdaNode.Nodes.Add(new TreeNode(String.Format("Data Authentication Code: {0}", emv.Sda.DataAuthenticationCode.ToHexa())));
            }
            else
            {
                sdaNode.Nodes.Add("SDA not done, supported or cryptographic error");
            }

            var ddaNode = new TreeNode("Dynamic Data Authentication");
            emvAppNode.Nodes.Add(ddaNode);

            if (emv.Dda != null)
            {
                ddaNode.Nodes.Add(new TreeNode(String.Format("Hash Algorithm Indicator: {0:X2}", emv.Dda.HashAlgorithmIndicator)));
                ddaNode.Nodes.Add(new TreeNode(String.Format("Hash Result: {0}", emv.Dda.HashResult.ToHexa())));
                ddaNode.Nodes.Add(new TreeNode(String.Format("ICC Dynamic Data Length:{0:X2}", emv.Dda.IccDynamicDataLength)));
                ddaNode.Nodes.Add(new TreeNode(String.Format("ICC Dynamic Data: {0}", emv.Dda.IccDynamicData.ToHexa())));
            }
            else
            {
                ddaNode.Nodes.Add("DDA not done, supported or cryptographic error");
            }

            var pinNode = new TreeNode("Cardholder Verification");
            emvAppNode.Nodes.Add(pinNode);
            pinNode.Nodes.Add(new TreeNode(String.Format("TVR :: Cardholder Verification: {0}", (emv.Tvr.CardholderVerificationFailed ? "failed" : "success"))));
            if (emv.VerifyPinStatusWord != 0x0000)
            {
                pinNode.Nodes.Add(new TreeNode(String.Format("VERIFY PIN status: 0x{0:X4}", emv.VerifyPinStatusWord)));
            }

            var AC1Node = new TreeNode("Application Cryptogram 1");
            emvAppNode.Nodes.Add(AC1Node);
            if (emv.TlvGenerateAC1Response != null)
            {
                AC1Node.Nodes.Add(new TreeNode(String.Format("Requested AC: {0}", emv.RequestedAC1Type)));
                AC1Node.Nodes.Add(new TreeNode(String.Format("Unpredictable Number: {0}", emv.TlvGenerateAC1UnpredictableNumber.Value.ToHexa())));
                var responseNode = new TreeNode(String.Format("Response: {0}", emv.TlvGenerateAC1Response));
                AC1Node.Nodes.Add(responseNode);
                responseNode.Nodes.Add(new TreeNode(String.Format("Cryptogram Information Data: {0}", emv.Cid1)));
                responseNode.Nodes.Add(new TreeNode(String.Format("Application Transaction Counter: {0}", emv.AtcFromAC1)));
            }

            guiEMVApplicationsContent.Nodes.Clear();
            guiEMVApplicationsContent.Nodes.Add(emvAppNode);
            guiEMVApplicationsContent.ExpandAll();
        }

        private void updateCVMList_Content()
        {
            if (_emv.CvmList != null)
            {
                guiCVMList.DataSource = _emv.CvmList.CvRules;
                guiCVMList.DisplayMember = "cvmCode";
            }
        }

        private void updateLogEntryAndFormat(EMVDefinitionFile df)
        {
            var emv = (EMVApplication)df;

            if (emv.LogFormat != null)
            {
                guiCardLogFormat.Text = String.Format("{0}", emv.LogFormat);
            }

            if (emv.LogEntry != null)
            {
                guiCardLogSFI.Text = String.Format("{0:X2}", emv.LogEntry.Sfi);
                guiCardLogLength.Text = String.Format("{0:X2}", emv.LogEntry.CyclicFileSize);
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

        private void updateLogRecords(EMVApplication emv)
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
                var tagStr = String.Format("{0:T}", dol);
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
                var item = new ListViewItem(String.Format(String.Format("{0}", recordNumber)));
                foreach (var tlvData in tlvDataList)
                {
                    // if tag is known by tlvManager, use the corresponding AbstractTLVObject else use BinaryTlvObject.
                    var tagStr = String.Format("{0:T}", tlvData);
                    if (_tlvDictionary.Get(tagStr) != null)
                    {
                        var tlvObject = _tlvDictionary.CreateInstance(tagStr);
                        tlvObject.Tlv = tlvData;
                        item.SubItems.Add(String.Format("{0}", tlvObject));
                    }
                    else
                    {
                        AbstractTlvObject tlvObject = new BinaryTlvObject();
                        tlvObject.Tlv = tlvData;
                        item.SubItems.Add(String.Format("{0}", tlvObject));
                    }
                }
                guiLogRecords.Items.Add(item);
            }
            guiLogRecords.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            guiDoCardLogSave.Enabled = true;
        }

        private void updatePublicKeysTab(EMVApplication emv)
        {
            // Certification Authority Public Key
            var aidObject = new ApplicationIdentifier(emv.Aid);
            guiPublicKeysAID.Text = emv.Aid;
            guiPublicKeysRID.Text = aidObject.StrRid;

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

        private void updateAuthenticationTabSignedData(EMVApplication emv)
        {
            if (emv.TlvOfflineRecords != null)
            {
                var index = 0;
                foreach (var tlv in emv.TlvOfflineRecords)
                {
                    index++;
                    var item = new ListViewItem(String.Format("{0}", index));
                    // tag
                    item.SubItems.Add(String.Format("{0:T}", tlv));
                    // name
                    if (_tlvDictionary != null && _tlvDictionary.Get(String.Format("{0:T}", tlv)) != null)
                    {
                        var tlvObject = _tlvDictionary.CreateInstance(tlv);
                        tlvObject.Tlv = tlv;
                        item.SubItems.Add(String.Format("{0:N}", tlvObject));
                    }
                    else
                    {
                        item.SubItems.Add(String.Format(""));
                    }
                    // value
                    item.SubItems.Add(String.Format("{0:V}", tlv));
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

        private void updateAuthenticationTabSDADone(EMVApplication emv)
        {
            if (emv.Sda != null)
            {
                guiSDARecoveredData.Text = emv.Sda.Recovered.ToHexa('\0');
                guiSDAHashResult.Text = emv.Sda.HashResult.ToHexa();
                guiSDADataAuthenticationCode.Text = emv.Sda.DataAuthenticationCode.ToHexa();
            }
        }

        private void updateAuthenticationTabDDADone(EMVApplication emv)
        {
            if (emv.Dda != null)
            {
                guiDDARecoveredData.Text = emv.Dda.Recovered.ToHexa('\0');
                guiDDAHashResult.Text = emv.Dda.HashResult.ToHexa();
                guiDDAICCDynamicData.Text = emv.Dda.IccDynamicData.ToHexa();
            }
        }

        private void updateAfterPSESelect_Content(EMVDefinitionFile df)
        {
            var pse = (PaymentSystemEnvironment)df;

            // Update list of applications found in PSE
            foreach (var emvFound in pse.GetApplications())
            {
                _detailedLogs.ObserveEMV(emvFound);
                observeEMV(emvFound);
                _emvApplications.Add(emvFound);
            }
            updateApplicationsList();

            var pseNode = new TreeNode(pse.Name);

            var fciNode = new TreeNode("File Control Information");
            pseNode.Nodes.Add(fciNode);

            if (pse.TlvFci != null)
            {
                fciNode.Nodes.Add(convertTLVDataToTreeNode(pse.TlvFci, _tlvDictionary));
            }
            else
            {
                var errorNode = new TreeNode(String.Format("PSE named '{0}'/[{1}] not found.", pse.Name, pse.Aid));
                fciNode.Nodes.Add(errorNode);
            }

            guiPSEContent.Nodes.Add(pseNode);
            guiPSEContent.ExpandAll();
        }

        private void updateAfterPSEReadRecords_Content(EMVDefinitionFile df)
        {
            var pse = (PaymentSystemEnvironment)df;

            // Update list of applications found in PSE
            foreach (var emvFound in pse.GetApplications())
            {
                _detailedLogs.ObserveEMV(emvFound);
                observeEMV(emvFound);
                _emvApplications.Add(emvFound);
            }
            updateApplicationsList();

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
                    var recordNode = new TreeNode(String.Format("Record {0}", recordNumber));
                    recordNode.Nodes.Add(convertTLVDataToTreeNode(tlv70, _tlvDictionary));
                    recordsNode.Nodes.Add(recordNode);
                }
            }
            else
            {
                var errorNode = new TreeNode(String.Format("No record found in PSE."));
                recordsNode.Nodes.Add(errorNode);
            }

            guiPSEContent.Nodes.Add(pseNode);
            guiPSEContent.ExpandAll();
        }

        private void updateTVR(EMVApplication emv)
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

        #region >> activateEMV*

        private void activateEMVSelectAID()
        {
            if (_emvApplications.Count != 0)
            {
                guiApplicationAID.Enabled = true;
                guiDoSelectAID.Enabled = true;
            }
        }

        private void activateEMVGetProcessingOptions()
        {
            if (_emv.TlvFci != null)
            {
                guiDoGetProcessingOptions.Enabled = true;
                guiDoGetData.Enabled = true;
            }
        }

        private void activateEMVReadRecord()
        {
            if (_emv.TlvProcessingOptions != null)
            {
                guiDoReadRecords.Enabled = true;
            }
        }

        private void activateEMVInternalAuthenticate()
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

        private void activateEMVCardholderVerification()
        {
            if (_emv.CvmList != null)
            {
                guiPINEntryUsed.Enabled = true;
                guiCVMList.Enabled = true;
                guiDoVerifyCardholder.Enabled = true;
            }
        }

        private void activateEMVGenerateAC1()
        {
            guiAC1UnpredictableNumber.Enabled = true;
            guiAC1Type.Enabled = true;
            guiDoGenerateAC1.Enabled = true;
        }

        #endregion

        #region >> *EventHandler

        private void afterPSESelectEventHandler(EMVDefinitionFile df)
        {
            if (InvokeRequired)
            {
                Invoke(new EMVDefinitionFile.AfterSelectEventHandler(afterPSESelectEventHandler), new Object[] { df });
            }
            else
            {
                updateAfterPSESelect_Content(df);
            }
        }

        private void afterPSEReadEventHandler(PaymentSystemEnvironment pse)
        {
            if (InvokeRequired)
            {
                Invoke(new PaymentSystemEnvironment.AfterReadEventHandler(afterPSEReadEventHandler), new Object[] { pse });
            }
            else
            {
                updateAfterPSEReadRecords_Content(pse);
            }
        }

        private void afterEMVSelectEventHandler(EMVDefinitionFile df)
        {
            if (InvokeRequired)
            {
                Invoke(new EMVDefinitionFile.AfterSelectEventHandler(afterEMVSelectEventHandler), new Object[] { df });
            }
            else
            {
                updateAfterAIDSelect_Content(df);
                updateLogEntryAndFormat(df);
            }
        }

        private void afterGetProcessingOptionsEventHandler(EMVApplication emv)
        {
            if (InvokeRequired)
            {
                Invoke(new EMVApplication.AfterGetProcessingOptionsEventHandler(afterGetProcessingOptionsEventHandler), new Object[] { emv });
            }
            else
            {
                updateAfterAIDSelect_Content(emv);
                updateTVR(emv);
            }
        }

        private void afterReadApplicationDataEventHandler(EMVApplication emv)
        {
            if (InvokeRequired)
            {
                Invoke(new EMVApplication.AfterReadApplicationDataEventHandler(afterReadApplicationDataEventHandler), new Object[] { emv });
            }
            else
            {
                updateAfterAIDSelect_Content(emv);
                updatePublicKeysTab(emv);
                updateAuthenticationTabSDADone(emv);
                updateAuthenticationTabSignedData(emv);
                updateTVR(emv);
            }
        }

        private void afterGetDataEventHandler(EMVApplication emv)
        {
            if (InvokeRequired)
            {
                Invoke(new EMVApplication.AfterGetDataEventHandler(afterGetDataEventHandler), new Object[] { emv });
            }
            else
            {
                updateAfterAIDSelect_Content(emv);
                updateLogEntryAndFormat(emv);
                updateTVR(emv);
            }
        }

        private void afterReadLogFileEventHandler(EMVApplication emv)
        {
            if (InvokeRequired)
            {
                Invoke(new EMVApplication.AfterReadLogFileEventHandler(afterReadLogFileEventHandler), new Object[] { emv });
            }
            else
            {
                updateLogRecords(emv);
                updateTVR(emv);
            }
        }

        private void afterInternalAuthenticateEventHandler(EMVApplication emv)
        {
            if (InvokeRequired)
            {
                Invoke(new EMVApplication.AfterInternalAuthenticateEventHandler(afterInternalAuthenticateEventHandler), new Object[] { emv });
            }
            else
            {
                updateAfterAIDSelect_Content(emv);
                updateAuthenticationTabDDADone(emv);
                updateTVR(emv);
            }
        }

        private void afterVerifyPinEventHandler(EMVApplication emv)
        {
            if (InvokeRequired)
            {
                Invoke(new EMVApplication.AfterVerifyPinEventHandler(afterVerifyPinEventHandler), new Object[] { emv });
            }
            else
            {
                updateAfterAIDSelect_Content(emv);
            }
        }

        private void afterGenerateAC1EventHandler(EMVApplication emv)
        {
            if (InvokeRequired)
            {
                Invoke(new EMVApplication.AfterGenerateAC1EventHandler(afterGenerateAC1EventHandler), new Object[] { emv });
            }
            else
            {
                updateAfterAIDSelect_Content(emv);
                updateTVR(emv);
            }
        }

        #endregion

        #region >> observe*

        private void observePSE(PaymentSystemEnvironment pse)
        {
            pse.AfterSelectEvent += afterPSESelectEventHandler;
            pse.AfterReadEvent += afterPSEReadEventHandler;
        }

        private void observeEMV(EMVApplication emv)
        {
            emv.AfterSelectEvent += afterEMVSelectEventHandler;
            emv.AfterGetProcessingOptionsEvent += afterGetProcessingOptionsEventHandler;
            emv.AfterReadApplicationDataEvent += afterReadApplicationDataEventHandler;
            emv.AfterGetDataEvent += afterGetDataEventHandler;
            emv.AfterReadLogFileEvent += afterReadLogFileEventHandler;
            emv.AfterInternalAuthenticateEvent += afterInternalAuthenticateEventHandler;
            emv.AfterVerifyPinEvent += afterVerifyPinEventHandler;
            emv.AfterGenerateAC1Event += afterGenerateAC1EventHandler;
        }

        #endregion
    }
}