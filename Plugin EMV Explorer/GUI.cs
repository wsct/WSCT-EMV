using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

using WSCT.EMV;
using WSCT.EMV.Objects;
using WSCT.EMV.Security;


namespace WSCT.GUI.Plugins.EMVExplorer
{
    public partial class GUI : Form
    {
        #region >> Fields

        DetailedLogs _detailedLogs;

        List<EMV.Card.EMVApplication> _emvApplications;

        TLVManager _tlvManager;

        PluginParameters _parameters;
        CertificationAuthorityRepository certificationAuthorityRepository;

        EMV.Card.PaymentSystemEnvironment _pse;
        EMV.Card.EMVApplication _emv;

        #endregion

        #region >> Constructor

        public GUI()
        {
            InitializeComponent();

            _tlvManager = new TLVManager();
            _tlvManager.loadFromXml("Dictionary.EMVTag.xml");

            _parameters = new PluginParameters();
            _parameters.loadFromXml("Config.EMVExplorer.xml");

            certificationAuthorityRepository = _parameters.terminalParameters.certificationAuthorityRepository;

            _detailedLogs = new DetailedLogs(this);

            _detailedLogs.tagsManager = _tlvManager;

            List<String> pseNames = new List<string>();
            pseNames.Add("1PAY.SYS.DDF01");
            pseNames.Add("2PAY.SYS.DDF01");
            guiPSEName.DataSource = pseNames;

            guiAC1Type.DataSource = Enum.GetValues(typeof(CryptogramType));
            guiAC1Type.SelectedItem = CryptogramType.TC;

            _emvApplications = new List<EMV.Card.EMVApplication>();
        }

        #endregion

        #region >> Methods

        private TreeNode convertTLVDataToTreeNode(TLVData tlv)
        {
            return convertTLVDataToTreeNode(tlv, null);
        }

        private TreeNode convertTLVDataToTreeNode(TLVData tlv, TLVManager tlvManager)
        {
            TreeNode tlvNode;
            if (tlvManager != null && tlvManager.get(String.Format("{0:T}", tlv)) != null)
            {
                AbstractTLVObject tlvObject = tlvManager.createInstance(tlv);
                tlvObject.tlv = tlv;
                tlvNode = new TreeNode(String.Format("{0:N}: {0}", tlvObject));
            }
            else
            {
                tlvNode = new TreeNode(String.Format("T:{0:T} L:{0:L} V:{0:Vh}", tlv));
            }
            foreach (TLVData subTLV in tlv.subFields)
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
                _pse = new EMV.Card.PaymentSystemEnvironment(SharedData.cardChannel);
                _pse.name = guiPSEName.Text;

                // Adjust AID listing location
                _pse.searchTagAIDInFCI = guiParamsTagAIDInFCI.Checked;

                // Attach observers
                _detailedLogs.observePSE(_pse);
                observePSE(_pse);

                // Select and Read the PSE
                if (_pse.select() == 0x9000)
                {
                    if (_pse.tlvFCI.hasTag(0x88))
                    {
                        _pse.read();
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
            // Get the EMV Application instance
            _emv = (EMV.Card.EMVApplication)guiApplicationAID.SelectedItem;

            // Check if it is a valid instance
            if (_emv == null)
            {
                MessageBox.Show("An EMV application must be first selected.", "EMV application missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Set the Certification Authorities
            _emv.certificationAuthorityRepository = certificationAuthorityRepository;

            // Select the PSE
            _emv.select();

            // Enable next step
            activateEMVGetProcessingOptions();
        }

        private void guiDoGetProcessingOptions_Click(object sender, EventArgs e)
        {
            // TODO: remove that patch ! (
            _emv.tlvTerminalData.Add(new TLVData(0x9F66, 0x02, new Byte[2] { 0x80, 0x00 }));

            // Do Get Processing Options on EMV Application
            _emv.getProcessingOptions();

            // Enable next step
            activateEMVReadRecord();
            activateEMVInternalAuthenticate();
        }

        private void guiDoReadRecords_Click(object sender, EventArgs e)
        {
            // Read Records targetted by AFL
            _emv.readApplicationData();

            // Enable next step
            activateEMVCardholderVerification();
            activateEMVGenerateAC1();

            // Update GUI
            updateCVMList_Content();
        }

        private void guiDoGetData_Click(object sender, EventArgs e)
        {
            // Get Data of the EMV Application
            _emv.getData();
        }

        private void guiDoExplicitDiscoveryOfAID_Click(object sender, EventArgs e)
        {
            try
            {
                // Try to select AID known by the terminal and undiscovered in PSE
                foreach (String aid in _parameters.terminalParameters.knownAIDs)
                {
                    Boolean notFound = true;
                    foreach (EMV.Card.EMVApplication emvFound in _emvApplications)
                    {
                        if (emvFound.aid == aid)
                            notFound = false;
                    }
                    // If AID not discovered, try to select it
                    if (notFound)
                    {
                        EMV.Card.EMVApplication emv = new EMV.Card.EMVApplication(SharedData.cardChannel, new TLVData());
                        emv.aid = aid;
                        _detailedLogs.observeEMV(emv);
                        observeEMV(emv);
                        // If success, add the EMV Application instance to the candidate list
                        if (emv.select() == 0x9000)
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
            _emv.readLogFile();
        }

        private void guiDoCardLogSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = "EMV Card Log";
            dialog.AddExtension = true;
            dialog.Filter = "Log files (*.log)|*.log";
            dialog.ShowDialog();
            String fileName = dialog.FileName;
            if (fileName != "")
            {
                System.IO.StreamWriter sw = System.IO.File.CreateText(fileName);
                foreach (ColumnHeader column in guiLogRecords.Columns)
                    sw.Write(String.Format("[{0}] ", column.Text));
                sw.WriteLine();
                foreach (ListViewItem item in guiLogRecords.Items)
                {
                    foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                        sw.Write(subItem.Text + " ");
                    sw.WriteLine();
                }
                sw.Close();
            }
        }

        private void guiDoSaveDetailedLogs_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = "EMV Card Log";
            dialog.AddExtension = true;
            dialog.Filter = "Log files (*.log)|*.log";
            dialog.ShowDialog();
            String fileName = dialog.FileName;
            if (fileName != "")
            {
                System.IO.StreamWriter sw = System.IO.File.CreateText(fileName);
                sw.Write(guiDetailedLogs.Text);
                sw.Close();
            }
        }

        private void guiDoInternalAuthenticate_Click(object sender, EventArgs e)
        {
            Byte[] unpredictableNumber;
            try
            {
                unpredictableNumber = guiInternalAuthenticateUnpredictableNumber.Text.fromHexa();
            }
            catch (Exception)
            {
                MessageBox.Show("Bad format of unpredictable number. Should be hexadecimal.", "Format error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Internal authenticate for DDA card
            _emv.internalAuthenticate(unpredictableNumber);
        }

        private void guiDoVerifyCardholder_Click(object sender, EventArgs e)
        {
            CardholderVerificationMethodList.CVRule cvRule = (CardholderVerificationMethodList.CVRule)guiCVMList.SelectedItem;
            EMV.Card.PINBlock pinBlock;
            if (cvRule.cvmCode == CardholderVerificationMethodList.CVMCode.PLAINTEXTPIN_ICC
                || cvRule.cvmCode == CardholderVerificationMethodList.CVMCode.PLAINTEXTPIN_ICC_AND_SIGN)
            {
                if (guiPINEntry.Enabled)
                {
                    pinBlock = new EMV.Card.PlaintextPINBlock();
                    pinBlock.clearPIN = guiPINEntry.Text.fromBCD((UInt32)guiPINEntry.Text.Length);
                    _emv.verifyPin(pinBlock);
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
            _emv.getChallenge();
        }

        private void guiDoGenerateAC1_Click(object sender, EventArgs e)
        {
            CryptogramType cryptogramType = (CryptogramType)guiAC1Type.SelectedItem;
            if (cryptogramType == CryptogramType.UNDEFINED)
            {
                MessageBox.Show(String.Format("Cryptogram type [{0}] unsupported", cryptogramType), "Unsupported feature", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                // Add default terminal data
                _emv.tlvTerminalData.AddRange(_parameters.transactionParameters.tlvDatas);

                Byte[] unpredictableNumber;
                try
                {
                    unpredictableNumber = guiAC1UnpredictableNumber.Text.fromHexa();
                }
                catch (Exception)
                {
                    MessageBox.Show("Bad format of unpredictable number. Should be hexadecimal.", "Format error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _emv.generateAC1(cryptogramType, unpredictableNumber);
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
            CardholderVerificationMethodList.CVRule cvRule = (CardholderVerificationMethodList.CVRule)guiCVMList.SelectedItem;
            switch (cvRule.cvmCode)
            {
                case CardholderVerificationMethodList.CVMCode.ENCIPHEREDPIN_ICC:
                case CardholderVerificationMethodList.CVMCode.ENCIPHEREDPIN_ICC_AND_SIGN:
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

        private void updateAfterAIDSelect_Content(EMV.Card.EMVDefinitionFile df)
        {
            EMV.Card.EMVApplication emv = (EMV.Card.EMVApplication)df;

            TreeNode emvAppNode = new TreeNode(emv.aid);

            TreeNode fciNode = new TreeNode("File Control Information");
            emvAppNode.Nodes.Add(fciNode);

            if (emv.tlvFCI != null)
            {
                fciNode.Nodes.Add(convertTLVDataToTreeNode(emv.tlvFCI, _tlvManager));
            }
            else
            {
                TreeNode errorNode = new TreeNode(String.Format("EMV application [{0}] not found.", emv.aid));
                fciNode.Nodes.Add(errorNode);
            }

            TreeNode optionsNode = new TreeNode("Processing Options");
            emvAppNode.Nodes.Add(optionsNode);

            if (emv.tlvProcessingOptions != null)
            {
                optionsNode.Nodes.Add(convertTLVDataToTreeNode(emv.tlvProcessingOptions, _tlvManager));
            }
            else
            {
                TreeNode errorNode = new TreeNode(String.Format("Processing Options not found."));
                optionsNode.Nodes.Add(errorNode);
            }

            TreeNode aipNode = new TreeNode("Application Interchange Profile");
            optionsNode.Nodes.Add(aipNode);

            if (emv.aip != null)
            {
                aipNode.Nodes.Add(String.Format("{0}", emv.aip));
            }
            else
            {
                TreeNode errorNode = new TreeNode(String.Format("AIP not found."));
                aipNode.Nodes.Add(errorNode);
            }

            TreeNode aflNode = new TreeNode("Application File Locator");
            optionsNode.Nodes.Add(aflNode);

            if (emv.afl != null)
            {
                foreach (EMV.Objects.ApplicationFileLocator.FileIdentification file in emv.afl.getFiles())
                {
                    aflNode.Nodes.Add(new TreeNode(String.Format("{0}", file)));
                }
            }
            else
            {
                TreeNode errorNode = new TreeNode(String.Format("AFL not found."));
                aflNode.Nodes.Add(errorNode);
            }

            TreeNode recordsNode = new TreeNode("Records");
            emvAppNode.Nodes.Add(recordsNode);

            if (emv.tlvRecords.Count != 0)
            {
                Byte recordNumber = 0;
                foreach (TLVData tlv70 in emv.tlvRecords)
                {
                    recordNumber++;
                    TreeNode recordNode = new TreeNode(String.Format("Record {0}", recordNumber));
                    recordNode.Nodes.Add(convertTLVDataToTreeNode(tlv70, _tlvManager));
                    recordsNode.Nodes.Add(recordNode);
                }
            }
            else
            {
                TreeNode errorNode = new TreeNode(String.Format("No records found."));
                recordsNode.Nodes.Add(errorNode);
            }

            TreeNode dataNode = new TreeNode("Get data");
            emvAppNode.Nodes.Add(dataNode);

            if (emv.tlvATC != null)
                dataNode.Nodes.Add(convertTLVDataToTreeNode(emv.tlvATC, _tlvManager));
            if (emv.tlvLastOnlineATCRegister != null)
                dataNode.Nodes.Add(convertTLVDataToTreeNode(emv.tlvLastOnlineATCRegister, _tlvManager));
            if (emv.tlvLogFormat != null)
                dataNode.Nodes.Add(convertTLVDataToTreeNode(emv.tlvLogFormat, _tlvManager));
            if (emv.tlvPINTryCounter != null)
                dataNode.Nodes.Add(convertTLVDataToTreeNode(emv.tlvPINTryCounter, _tlvManager));

            guiEMVApplicationsContent.Nodes.Clear();
            guiEMVApplicationsContent.Nodes.Add(emvAppNode);
            guiEMVApplicationsContent.ExpandAll();
        }

        private void updateAfterAIDSelect_Content(EMV.Card.EMVApplication emv)
        {
            TreeNode emvAppNode = new TreeNode(emv.aid);

            TreeNode fciNode = new TreeNode("File Control Information");
            emvAppNode.Nodes.Add(fciNode);

            if (emv.tlvFCI != null)
            {
                fciNode.Nodes.Add(convertTLVDataToTreeNode(emv.tlvFCI, _tlvManager));
            }
            else
            {
                TreeNode errorNode = new TreeNode(String.Format("EMV application [{0}] not found.", emv.aid));
                fciNode.Nodes.Add(errorNode);
            }

            TreeNode optionsNode = new TreeNode("Processing Options");
            emvAppNode.Nodes.Add(optionsNode);

            if (emv.tlvProcessingOptions != null)
            {
                optionsNode.Nodes.Add(convertTLVDataToTreeNode(emv.tlvProcessingOptions, _tlvManager));
            }
            else
            {
                TreeNode errorNode = new TreeNode(String.Format("Processing Options not found."));
                optionsNode.Nodes.Add(errorNode);
            }

            TreeNode aipNode = new TreeNode("Application Interchange Profile");
            optionsNode.Nodes.Add(aipNode);

            if (emv.aip != null)
            {
                aipNode.Nodes.Add(String.Format("{0}", emv.aip));
            }
            else
            {
                TreeNode errorNode = new TreeNode(String.Format("AIP not found."));
                aipNode.Nodes.Add(errorNode);
            }

            TreeNode aflNode = new TreeNode("Application File Locator");
            optionsNode.Nodes.Add(aflNode);

            if (emv.afl != null)
            {
                foreach (EMV.Objects.ApplicationFileLocator.FileIdentification file in emv.afl.getFiles())
                {
                    aflNode.Nodes.Add(new TreeNode(String.Format("{0}", file)));
                }
            }
            else
            {
                TreeNode errorNode = new TreeNode(String.Format("AFL not found."));
                aflNode.Nodes.Add(errorNode);
            }

            TreeNode recordsNode = new TreeNode("Records");
            emvAppNode.Nodes.Add(recordsNode);

            if (emv.tlvRecords.Count != 0)
            {
                Byte recordNumber = 0;
                foreach (TLVData tlv70 in emv.tlvRecords)
                {
                    recordNumber++;
                    TreeNode recordNode = new TreeNode(String.Format("Record {0}", recordNumber));
                    recordNode.Nodes.Add(convertTLVDataToTreeNode(tlv70, _tlvManager));
                    recordsNode.Nodes.Add(recordNode);
                }
            }
            else
            {
                TreeNode errorNode = new TreeNode(String.Format("No records found."));
                recordsNode.Nodes.Add(errorNode);
            }

            TreeNode dataNode = new TreeNode("Get data");
            emvAppNode.Nodes.Add(dataNode);

            if (emv.tlvATC != null)
                dataNode.Nodes.Add(convertTLVDataToTreeNode(emv.tlvATC, _tlvManager));
            if (emv.tlvLastOnlineATCRegister != null)
                dataNode.Nodes.Add(convertTLVDataToTreeNode(emv.tlvLastOnlineATCRegister, _tlvManager));
            if (emv.tlvLogFormat != null)
                dataNode.Nodes.Add(convertTLVDataToTreeNode(emv.tlvLogFormat, _tlvManager));
            if (emv.tlvPINTryCounter != null)
                dataNode.Nodes.Add(convertTLVDataToTreeNode(emv.tlvPINTryCounter, _tlvManager));

            TreeNode internalAuthenticateNode = new TreeNode("Internal Authenticate");
            emvAppNode.Nodes.Add(internalAuthenticateNode);

            if (emv.tlvSignedDynamicApplicationResponse != null)
            {
                internalAuthenticateNode.Nodes.Add(convertTLVDataToTreeNode(emv.tlvInternalAuthenticateUnpredictableNumber, _tlvManager));
                internalAuthenticateNode.Nodes.Add(convertTLVDataToTreeNode(emv.tlvSignedDynamicApplicationResponse, _tlvManager));
            }
            else
            {
                TreeNode errorNode = new TreeNode("Internal authenticate APDU not done of failed.");
                internalAuthenticateNode.Nodes.Add(errorNode);
            }

            TreeNode sdaNode = new TreeNode("Static Data Authentication");
            emvAppNode.Nodes.Add(sdaNode);

            if (emv.sda != null)
            {
                sdaNode.Nodes.Add(new TreeNode(String.Format("Hash Algorithm Indicator: {0:X2}", emv.sda.hashAlgorithmIndicator)));
                sdaNode.Nodes.Add(new TreeNode(String.Format("Hash Result: {0}", emv.sda.hashResult.toHexa())));
                sdaNode.Nodes.Add(new TreeNode(String.Format("Data Authentication Code: {0}", emv.sda.dataAuthenticationCode.toHexa())));
            }
            else
            {
                sdaNode.Nodes.Add("SDA not done, supported or cryptographic error");
            }

            TreeNode ddaNode = new TreeNode("Dynamic Data Authentication");
            emvAppNode.Nodes.Add(ddaNode);

            if (emv.dda != null)
            {
                ddaNode.Nodes.Add(new TreeNode(String.Format("Hash Algorithm Indicator: {0:X2}", emv.dda.hashAlgorithmIndicator)));
                ddaNode.Nodes.Add(new TreeNode(String.Format("Hash Result: {0}", emv.dda.hashResult.toHexa())));
                ddaNode.Nodes.Add(new TreeNode(String.Format("ICC Dynamic Data Length:{0:X2}", emv.dda.iccDynamicDataLength)));
                ddaNode.Nodes.Add(new TreeNode(String.Format("ICC Dynamic Data: {0}", emv.dda.iccDynamicData.toHexa())));
            }
            else
            {
                ddaNode.Nodes.Add("DDA not done, supported or cryptographic error");
            }

            TreeNode pinNode = new TreeNode("Cardholder Verification");
            emvAppNode.Nodes.Add(pinNode);
            pinNode.Nodes.Add(new TreeNode(String.Format("TVR :: Cardholder Verification: {0}", (emv.tvr.cardholderVerificationFailed ? "failed" : "success"))));
            if (emv.verifyPinStatusWord != 0x0000)
            {
                pinNode.Nodes.Add(new TreeNode(String.Format("VERIFY PIN status: 0x{0:X4}", emv.verifyPinStatusWord)));
            }

            TreeNode AC1Node = new TreeNode("Application Cryptogram 1");
            emvAppNode.Nodes.Add(AC1Node);
            if (emv.tlvGenerateAC1Response != null)
            {
                AC1Node.Nodes.Add(new TreeNode(String.Format("Requested AC: {0}", emv.requestedAC1Type)));
                AC1Node.Nodes.Add(new TreeNode(String.Format("Unpredictable Number: {0}", emv.tlvGenerateAC1UnpredictableNumber.value.toHexa())));
                TreeNode responseNode = new TreeNode(String.Format("Response: {0}", emv.tlvGenerateAC1Response));
                AC1Node.Nodes.Add(responseNode);
                responseNode.Nodes.Add(new TreeNode(String.Format("Cryptogram Information Data: {0}", emv.cid1)));
                responseNode.Nodes.Add(new TreeNode(String.Format("Application Transaction Counter: {0}", emv.atcFromAC1)));
            }

            guiEMVApplicationsContent.Nodes.Clear();
            guiEMVApplicationsContent.Nodes.Add(emvAppNode);
            guiEMVApplicationsContent.ExpandAll();
        }

        private void updateCVMList_Content()
        {
            if (_emv.cvmList != null)
            {
                guiCVMList.DataSource = _emv.cvmList.cvRules;
                guiCVMList.DisplayMember = "cvmCode";
            }
        }

        private void updateLogEntryAndFormat(EMV.Card.EMVDefinitionFile df)
        {
            EMV.Card.EMVApplication emv = (EMV.Card.EMVApplication)df;

            if (emv.logFormat != null)
            {
                guiCardLogFormat.Text = String.Format("{0}", emv.logFormat);
            }

            if (emv.logEntry != null)
            {
                guiCardLogSFI.Text = String.Format("{0:X2}", emv.logEntry.sfi);
                guiCardLogLength.Text = String.Format("{0:X2}", emv.logEntry.cyclicFileSize);
            }

            if (emv.logEntry != null && emv.logFormat != null)
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

        private void updateLogRecords(EMV.Card.EMVApplication emv)
        {
            if (emv.logRecords == null)
                return;

            // Initialize the output
            guiLogRecords.Columns.Clear();
            guiLogRecords.Items.Clear();

            // Set column names. If tag is known by tlvManager, its name is used instead of the tag number.
            guiLogRecords.Columns.Add("Record");
            foreach (DataObjectList.DataObjectDefinition dol in emv.logFormat.getDataObjectDefinitions())
            {
                String tagStr = String.Format("{0:T}", dol);
                if (_tlvManager.get(tagStr) != null)
                {
                    guiLogRecords.Columns.Add(_tlvManager.get(tagStr).name);
                }
                else
                {
                    guiLogRecords.Columns.Add(tagStr);
                }
            }

            // Fill the list view: 1 line (list view item) by record
            Byte recordNumber = 0;
            foreach (List<TLVData> tlvDataList in emv.logRecords)
            {
                recordNumber++;
                ListViewItem item = new ListViewItem(String.Format(String.Format("{0}", recordNumber)));
                foreach (TLVData tlvData in tlvDataList)
                {
                    // if tag is known by tlvManager, use the corresponding AbstractTLVObject else use BinaryTLVObject.
                    String tagStr = String.Format("{0:T}", tlvData);
                    if (_tlvManager.get(tagStr) != null)
                    {
                        AbstractTLVObject tlvObject = _tlvManager.createInstance(tagStr);
                        tlvObject.tlv = tlvData;
                        item.SubItems.Add(String.Format("{0}", tlvObject));
                    }
                    else
                    {
                        AbstractTLVObject tlvObject = new BinaryTLVObject();
                        tlvObject.tlv = tlvData;
                        item.SubItems.Add(String.Format("{0}", tlvObject));
                    }
                }
                guiLogRecords.Items.Add(item);
            }
            guiLogRecords.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            guiDoCardLogSave.Enabled = true;
        }

        private void updatePublicKeysTab(EMV.Card.EMVApplication emv)
        {
            // Certification Authority Public Key
            ApplicationIdentifier aidObject = new ApplicationIdentifier(emv.aid);
            guiPublicKeysAID.Text = emv.aid;
            guiPublicKeysRID.Text = aidObject.strRID;

            if (emv.tlvDataRecords.hasTag(0x8F))
            {
                guiPublicKeysCAIndex.Text = emv.tlvDataRecords.getTag(0x8F).value.toHexa();
            }

            if (emv.certificationAuthorityPublicKey != null)
            {
                guiPublicKeysCertificationAuthorityPKModulus.Text = emv.certificationAuthorityPublicKey.modulus;
                guiPublicKeysCertificationAuthorityPKExponent.Text = emv.certificationAuthorityPublicKey.exponent;
            }

            // Issuer Public Key
            if (emv.issuerPublicKeyCertificate != null)
            {
                guiPublicKeysIssuerPKRecoveredData.Text = emv.issuerPublicKeyCertificate.recovered.toHexa('\0');
                guiPublicKeysIssuerPKHashResult.Text = emv.issuerPublicKeyCertificate.hashResult.toHexa();
                guiPublicKeysIssuerPKModulus.Text = emv.issuerPublicKey.modulus;
                guiPublicKeysIssuerPKExponent.Text = emv.issuerPublicKey.exponent;
            }

            // ICC Public Key
            if (emv.iccPublicKeyCertificate != null)
            {
                guiPublicKeysICCPKRecoveredData.Text = emv.iccPublicKeyCertificate.recovered.toHexa('\0');
                guiPublicKeysICCPKHashResult.Text = emv.iccPublicKeyCertificate.hashResult.toHexa();
                guiPublicKeysICCPKModulus.Text = emv.iccPublicKey.modulus;
                guiPublicKeysICCPKExponent.Text = emv.iccPublicKey.exponent;
            }
        }

        private void updateAuthenticationTabSignedData(EMV.Card.EMVApplication emv)
        {
            if (emv.tlvOfflineRecords != null)
            {
                int index = 0;
                foreach (TLVData tlv in emv.tlvOfflineRecords)
                {
                    index++;
                    ListViewItem item = new ListViewItem(String.Format("{0}", index));
                    // tag
                    item.SubItems.Add(String.Format("{0:T}", tlv));
                    // name
                    if (_tlvManager != null && _tlvManager.get(String.Format("{0:T}", tlv)) != null)
                    {
                        AbstractTLVObject tlvObject = _tlvManager.createInstance(tlv);
                        tlvObject.tlv = tlv;
                        item.SubItems.Add(String.Format("{0:N}", tlvObject));
                    }
                    else
                    {
                        item.SubItems.Add(String.Format(""));
                    }
                    // value
                    item.SubItems.Add(String.Format("{0:V}", tlv));
                    // insert item in SDA, DDA and/or CDA list
                    if (emv.aip.sda)
                        guiSDASignedData.Items.Add(item);
                    if (emv.aip.dda)
                        guiDDASignedData.Items.Add(item);
                    if (emv.aip.cda)
                        guiCDASignedData.Items.Add(item);
                }
                //if (emv.tlvDataRecords.hasTag(0x9F4A))
                //{
                //    DataObjectList sdaTagList = new DataObjectList();
                //    sdaTagList.tlv = emv.tlvDataRecords.getTag(0x9F4A);
                //}
            }
        }

        private void updateAuthenticationTabSDADone(EMV.Card.EMVApplication emv)
        {
            if (emv.sda != null)
            {
                guiSDARecoveredData.Text = emv.sda.recovered.toHexa('\0');
                guiSDAHashResult.Text = emv.sda.hashResult.toHexa();
                guiSDADataAuthenticationCode.Text = emv.sda.dataAuthenticationCode.toHexa();
            }
        }

        private void updateAuthenticationTabDDADone(EMV.Card.EMVApplication emv)
        {
            if (emv.dda != null)
            {
                guiDDARecoveredData.Text = emv.dda.recovered.toHexa('\0');
                guiDDAHashResult.Text = emv.dda.hashResult.toHexa();
                guiDDAICCDynamicData.Text = emv.dda.iccDynamicData.toHexa();
            }
        }

        private void updateAfterPSESelect_Content(EMV.Card.EMVDefinitionFile df)
        {
            EMV.Card.PaymentSystemEnvironment pse = (EMV.Card.PaymentSystemEnvironment)df;

            // Update list of applications found in PSE
            foreach (EMV.Card.EMVApplication emvFound in pse.getApplications())
            {
                _detailedLogs.observeEMV(emvFound);
                observeEMV(emvFound);
                _emvApplications.Add(emvFound);
            }
            updateApplicationsList();

            TreeNode pseNode = new TreeNode(pse.name);

            TreeNode fciNode = new TreeNode("File Control Information");
            pseNode.Nodes.Add(fciNode);

            if (pse.tlvFCI != null)
            {
                fciNode.Nodes.Add(convertTLVDataToTreeNode(pse.tlvFCI, _tlvManager));
            }
            else
            {
                TreeNode errorNode = new TreeNode(String.Format("PSE named '{0}'/[{1}] not found.", pse.name, pse.aid));
                fciNode.Nodes.Add(errorNode);
            }

            guiPSEContent.Nodes.Add(pseNode);
            guiPSEContent.ExpandAll();
        }

        private void updateAfterPSEReadRecords_Content(EMV.Card.EMVDefinitionFile df)
        {
            EMV.Card.PaymentSystemEnvironment pse = (EMV.Card.PaymentSystemEnvironment)df;

            // Update list of applications found in PSE
            foreach (EMV.Card.EMVApplication emvFound in pse.getApplications())
            {
                _detailedLogs.observeEMV(emvFound);
                observeEMV(emvFound);
                _emvApplications.Add(emvFound);
            }
            updateApplicationsList();

            // Update content of the PSE Records
            TreeNode pseNode = new TreeNode(pse.name);

            TreeNode recordsNode = new TreeNode("Records");
            pseNode.Nodes.Add(recordsNode);

            if (pse.tlvRecords.Count != 0)
            {
                Byte recordNumber = 0;
                foreach (TLVData tlv70 in pse.tlvRecords)
                {
                    recordNumber++;
                    TreeNode recordNode = new TreeNode(String.Format("Record {0}", recordNumber));
                    recordNode.Nodes.Add(convertTLVDataToTreeNode(tlv70, _tlvManager));
                    recordsNode.Nodes.Add(recordNode);
                }
            }
            else
            {
                TreeNode errorNode = new TreeNode(String.Format("No record found in PSE."));
                recordsNode.Nodes.Add(errorNode);
            }

            guiPSEContent.Nodes.Add(pseNode);
            guiPSEContent.ExpandAll();

        }

        private void updateTVR(EMV.Card.EMVApplication emv)
        {
            // byte 1
            guiTVR_1_1.Checked = emv.tvr.offlineDataAuthenticationNotPerformed;
            guiTVR_1_2.Checked = emv.tvr.sdaFailed;
            guiTVR1_3.Checked = emv.tvr.iccDataMissing;
            guiTVR1_4.Checked = emv.tvr.terminalExceptionFile;
            guiTVR1_5.Checked = emv.tvr.ddaFailed;
            guiTVR1_6.Checked = emv.tvr.cdaFailed;

            // byte 2
            guiTVR2_1.Checked = emv.tvr.IccAndTerminalVersionsDifferent;
            guiTVR2_2.Checked = emv.tvr.expiredApplication;
            guiTVR2_3.Checked = emv.tvr.notYetEffectiveApplication;
            guiTVR2_4.Checked = emv.tvr.serviceNotAllowed;
            guiTVR2_5.Checked = emv.tvr.newCard;

            // byte 3
            guiTVR3_1.Checked = emv.tvr.cardholderVerificationFailed;
            guiTVR3_2.Checked = emv.tvr.unrecognisedCVM;
            guiTVR3_3.Checked = emv.tvr.pinTryLimitExceeded;
            guiTVR3_4.Checked = emv.tvr.pinpadError;
            guiTVR3_5.Checked = emv.tvr.pinNotEntered;
            guiTVR3_6.Checked = emv.tvr.onlinePinEntered;

            // byte 4
            guiTVR4_1.Checked = emv.tvr.transactionExceedFloorLimit;
            guiTVR4_2.Checked = emv.tvr.lowerConsecutiveOfflineLimitExceeded;
            guiTVR4_3.Checked = emv.tvr.upperConsecutiveOfflineLimitExceeded;
            guiTVR4_4.Checked = emv.tvr.transactionRandomlySelectedOnline;
            guiTVR4_5.Checked = emv.tvr.merchantForcedTransactionOnline;

            // byte 5
            guiTVR5_1.Checked = emv.tvr.defaultTDOLUsed;
            guiTVR5_2.Checked = emv.tvr.issuerAuthenticationFailed;
            guiTVR5_3.Checked = emv.tvr.scriptProcessingFailedBeforeGenerateAC;
            guiTVR5_4.Checked = emv.tvr.scriptProcessingFailedAfterGenerateAC;
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
            if (_emv.tlvFCI != null)
            {
                guiDoGetProcessingOptions.Enabled = true;
                guiDoGetData.Enabled = true;
            }
        }

        private void activateEMVReadRecord()
        {
            if (_emv.tlvProcessingOptions != null)
            {
                guiDoReadRecords.Enabled = true;
            }
        }

        private void activateEMVInternalAuthenticate()
        {
            if (_emv.aip != null && _emv.aip.dda)
            {
                // Generates a new unpredictable number
                Byte[] unpredictableNumber = new Byte[4];
                new Random().NextBytes(unpredictableNumber);
                guiInternalAuthenticateUnpredictableNumber.Text = unpredictableNumber.toHexa('\0');

                guiInternalAuthenticateUnpredictableNumber.Enabled = true;
                guiDoInternalAuthenticate.Enabled = true;
            }
        }

        private void activateEMVCardholderVerification()
        {
            if (_emv.cvmList != null)
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

        private void afterPSESelectEventHandler(EMV.Card.EMVDefinitionFile df)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EMV.Card.EMVDefinitionFile.afterSelectEventHandler(afterPSESelectEventHandler), new Object[] { df });
            }
            else
            {
                updateAfterPSESelect_Content(df);
            }
        }

        private void afterPSEReadEventHandler(EMV.Card.PaymentSystemEnvironment pse)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EMV.Card.PaymentSystemEnvironment.afterReadEventHandler(afterPSEReadEventHandler), new Object[] { pse });
            }
            else
            {
                updateAfterPSEReadRecords_Content(pse);
            }
        }

        private void afterEMVSelectEventHandler(EMV.Card.EMVDefinitionFile df)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EMV.Card.EMVDefinitionFile.afterSelectEventHandler(afterEMVSelectEventHandler), new Object[] { df });
            }
            else
            {
                updateAfterAIDSelect_Content(df);
                updateLogEntryAndFormat(df);
            }
        }

        private void afterGetProcessingOptionsEventHandler(EMV.Card.EMVApplication emv)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EMV.Card.EMVApplication.afterGetProcessingOptionsEventHandler(afterGetProcessingOptionsEventHandler), new Object[] { emv });
            }
            else
            {
                updateAfterAIDSelect_Content(emv);
                updateTVR(emv);
            }
        }

        private void afterReadApplicationDataEventHandler(EMV.Card.EMVApplication emv)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EMV.Card.EMVApplication.afterReadApplicationDataEventHandler(afterReadApplicationDataEventHandler), new Object[] { emv });
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

        private void afterGetDataEventHandler(EMV.Card.EMVApplication emv)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EMV.Card.EMVApplication.afterGetDataEventHandler(afterGetDataEventHandler), new Object[] { emv });
            }
            else
            {
                updateAfterAIDSelect_Content(emv);
                updateLogEntryAndFormat(emv);
                updateTVR(emv);
            }
        }

        private void afterReadLogFileEventHandler(EMV.Card.EMVApplication emv)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EMV.Card.EMVApplication.afterReadLogFileEventHandler(afterReadLogFileEventHandler), new Object[] { emv });
            }
            else
            {
                updateLogRecords(emv);
                updateTVR(emv);
            }
        }

        private void afterInternalAuthenticateEventHandler(EMV.Card.EMVApplication emv)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EMV.Card.EMVApplication.afterInternalAuthenticateEventHandler(afterInternalAuthenticateEventHandler), new Object[] { emv });
            }
            else
            {
                updateAfterAIDSelect_Content(emv);
                updateAuthenticationTabDDADone(emv);
                updateTVR(emv);
            }
        }

        private void afterVerifyPinEventHandler(EMV.Card.EMVApplication emv)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EMV.Card.EMVApplication.afterVerifyPinEventHandler(afterVerifyPinEventHandler), new Object[] { emv });
            }
            else
            {
                updateAfterAIDSelect_Content(emv);
            }
        }

        private void afterGenerateAC1EventHandler(EMV.Card.EMVApplication emv)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EMV.Card.EMVApplication.afterGenerateAC1EventHandler(afterGenerateAC1EventHandler), new Object[] { emv });
            }
            else
            {
                updateAfterAIDSelect_Content(emv);
                updateTVR(emv);
            }
        }

        #endregion

        #region >> observe*

        private void observePSE(EMV.Card.PaymentSystemEnvironment pse)
        {
            pse.afterSelectEvent += afterPSESelectEventHandler;
            pse.afterReadEvent += afterPSEReadEventHandler;
        }

        private void observeEMV(EMV.Card.EMVApplication emv)
        {
            emv.afterSelectEvent += afterEMVSelectEventHandler;
            emv.afterGetProcessingOptionsEvent += afterGetProcessingOptionsEventHandler;
            emv.afterReadApplicationDataEvent += afterReadApplicationDataEventHandler;
            emv.afterGetDataEvent += afterGetDataEventHandler;
            emv.afterReadLogFileEvent += afterReadLogFileEventHandler;
            emv.afterInternalAuthenticateEvent += afterInternalAuthenticateEventHandler;
            emv.afterVerifyPinEvent += afterVerifyPinEventHandler;
            emv.afterGenerateAC1Event += afterGenerateAC1EventHandler;
        }

        #endregion
    }
}