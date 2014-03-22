namespace WSCT.GUI.Plugins.EMVExplorer
{
    partial class Gui
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TabPage guiTabParameters;
            System.Windows.Forms.GroupBox groupBox9;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.GroupBox groupBox8;
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.GroupBox groupBox2;
            System.Windows.Forms.GroupBox groupBox3;
            System.Windows.Forms.GroupBox groupBox4;
            System.Windows.Forms.GroupBox groupBox10;
            System.Windows.Forms.GroupBox groupBox11;
            System.Windows.Forms.GroupBox groupBox12;
            System.Windows.Forms.GroupBox groupBox13;
            System.Windows.Forms.TabPage guiTabDetailedLogs;
            System.Windows.Forms.TabPage guiTabPSE;
            System.Windows.Forms.TabPage guiTabEMVApplications;
            System.Windows.Forms.TabPage guiTabEMVCardLog;
            System.Windows.Forms.TabPage guiTabPublicKeys;
            System.Windows.Forms.GroupBox groupBox18;
            System.Windows.Forms.Label label15;
            System.Windows.Forms.Label label16;
            System.Windows.Forms.Label label14;
            System.Windows.Forms.Label label17;
            System.Windows.Forms.GroupBox groupBox16;
            System.Windows.Forms.Label label11;
            System.Windows.Forms.Label label12;
            System.Windows.Forms.Label label13;
            System.Windows.Forms.Label label10;
            System.Windows.Forms.GroupBox groupBox14;
            System.Windows.Forms.Label label9;
            System.Windows.Forms.Label label8;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.TabPage guiTabAuthentication;
            System.Windows.Forms.TabControl guiAuthentication;
            System.Windows.Forms.TabPage guiSDA;
            System.Windows.Forms.Label label23;
            System.Windows.Forms.Label label22;
            System.Windows.Forms.Label label21;
            System.Windows.Forms.ColumnHeader guiSDASignedDataColumnName;
            System.Windows.Forms.ColumnHeader guiSDASignedDataColumnValue;
            System.Windows.Forms.Label label18;
            System.Windows.Forms.TabPage guiDDA;
            System.Windows.Forms.ColumnHeader columnHeader3;
            System.Windows.Forms.ColumnHeader columnHeader4;
            System.Windows.Forms.Label guiDDALabel_ICCDynamicNumber;
            System.Windows.Forms.Label label25;
            System.Windows.Forms.Label label26;
            System.Windows.Forms.Label label19;
            System.Windows.Forms.TabPage guiCDA;
            System.Windows.Forms.ColumnHeader columnHeader7;
            System.Windows.Forms.ColumnHeader columnHeader8;
            System.Windows.Forms.Label label20;
            System.Windows.Forms.GroupBox groupBox17;
            System.Windows.Forms.GroupBox groupBox19;
            System.Windows.Forms.GroupBox groupBox20;
            System.Windows.Forms.GroupBox groupBox21;
            System.Windows.Forms.GroupBox groupBox22;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Gui));
            this.guiParamsTagAIDInFCI = new System.Windows.Forms.CheckBox();
            this.guiDoExplicitDiscoveryOfAID = new System.Windows.Forms.Button();
            this.guiPSEName = new System.Windows.Forms.ComboBox();
            this.guiDoSelectPSE = new System.Windows.Forms.Button();
            this.guiApplicationAID = new System.Windows.Forms.ComboBox();
            this.guiDoSelectAID = new System.Windows.Forms.Button();
            this.guiDoGetProcessingOptions = new System.Windows.Forms.Button();
            this.guiDoGetData = new System.Windows.Forms.Button();
            this.guiDoReadRecords = new System.Windows.Forms.Button();
            this.guiDoGetChallenge = new System.Windows.Forms.Button();
            this.guiPINEntryUsed = new System.Windows.Forms.CheckBox();
            this.guiPINEntry = new System.Windows.Forms.TextBox();
            this.guiCVMList = new System.Windows.Forms.ComboBox();
            this.guiDoVerifyCardholder = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.guiInternalAuthenticateUnpredictableNumber = new System.Windows.Forms.TextBox();
            this.guiDoInternalAuthenticate = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.guiAC1UnpredictableNumber = new System.Windows.Forms.TextBox();
            this.guiAC1Type = new System.Windows.Forms.ComboBox();
            this.guiDoGenerateAC1 = new System.Windows.Forms.Button();
            this.guiDoExternalAuthenticate = new System.Windows.Forms.Button();
            this.guiDoGenerateAC2 = new System.Windows.Forms.Button();
            this.guiAC2Type = new System.Windows.Forms.ComboBox();
            this.guiDoSaveDetailedLogs = new System.Windows.Forms.Button();
            this.guiDetailedLogs = new System.Windows.Forms.RichTextBox();
            this.guiPSEContent = new System.Windows.Forms.TreeView();
            this.guiEMVApplicationsContent = new System.Windows.Forms.TreeView();
            this.guiLogRecords = new System.Windows.Forms.ListView();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.guiDoCardLogSave = new System.Windows.Forms.Button();
            this.guiDoCardLogRead = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.guiCardLogFormat = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.guiCardLogLength = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.guiCardLogSFI = new System.Windows.Forms.TextBox();
            this.guiLogFilePresence = new System.Windows.Forms.CheckBox();
            this.guiPublicKeysICCPKModulus = new System.Windows.Forms.TextBox();
            this.guiPublicKeysICCPKExponent = new System.Windows.Forms.TextBox();
            this.guiPublicKeysICCPKHashResult = new System.Windows.Forms.TextBox();
            this.guiPublicKeysICCPKRecoveredData = new System.Windows.Forms.TextBox();
            this.guiPublicKeysIssuerPKModulus = new System.Windows.Forms.TextBox();
            this.guiPublicKeysIssuerPKExponent = new System.Windows.Forms.TextBox();
            this.guiPublicKeysIssuerPKHashResult = new System.Windows.Forms.TextBox();
            this.guiPublicKeysIssuerPKRecoveredData = new System.Windows.Forms.TextBox();
            this.guiPublicKeysCertificationAuthorityPKModulus = new System.Windows.Forms.TextBox();
            this.guiPublicKeysCertificationAuthorityPKExponent = new System.Windows.Forms.TextBox();
            this.guiPublicKeysCAIndex = new System.Windows.Forms.TextBox();
            this.guiPublicKeysAID = new System.Windows.Forms.TextBox();
            this.guiPublicKeysRID = new System.Windows.Forms.TextBox();
            this.guiSDAAuthenticationData = new System.Windows.Forms.GroupBox();
            this.guiSDADataAuthenticationCode = new System.Windows.Forms.TextBox();
            this.guiSDAHashResult = new System.Windows.Forms.TextBox();
            this.guiSDARecoveredData = new System.Windows.Forms.TextBox();
            this.guiSDASignedData = new System.Windows.Forms.ListView();
            this.guiSDASignedDataColumnId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.guiSDASignedDataTag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.guiDDASignedData = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.guiDDAICCDynamicData = new System.Windows.Forms.TextBox();
            this.guiDDAHashResult = new System.Windows.Forms.TextBox();
            this.guiDDARecoveredData = new System.Windows.Forms.TextBox();
            this.guiCDASignedData = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.guiTVR1_7 = new System.Windows.Forms.CheckBox();
            this.guiTVR1_8 = new System.Windows.Forms.CheckBox();
            this.guiTVR1_6 = new System.Windows.Forms.CheckBox();
            this.guiTVR1_5 = new System.Windows.Forms.CheckBox();
            this.guiTVR1_4 = new System.Windows.Forms.CheckBox();
            this.guiTVR1_3 = new System.Windows.Forms.CheckBox();
            this.guiTVR_1_2 = new System.Windows.Forms.CheckBox();
            this.guiTVR_1_1 = new System.Windows.Forms.CheckBox();
            this.guiTVR2_7 = new System.Windows.Forms.CheckBox();
            this.guiTVR2_8 = new System.Windows.Forms.CheckBox();
            this.guiTVR2_6 = new System.Windows.Forms.CheckBox();
            this.guiTVR2_5 = new System.Windows.Forms.CheckBox();
            this.guiTVR2_4 = new System.Windows.Forms.CheckBox();
            this.guiTVR2_3 = new System.Windows.Forms.CheckBox();
            this.guiTVR2_2 = new System.Windows.Forms.CheckBox();
            this.guiTVR2_1 = new System.Windows.Forms.CheckBox();
            this.guiTVR3_7 = new System.Windows.Forms.CheckBox();
            this.guiTVR3_8 = new System.Windows.Forms.CheckBox();
            this.guiTVR3_6 = new System.Windows.Forms.CheckBox();
            this.guiTVR3_5 = new System.Windows.Forms.CheckBox();
            this.guiTVR3_4 = new System.Windows.Forms.CheckBox();
            this.guiTVR3_3 = new System.Windows.Forms.CheckBox();
            this.guiTVR3_2 = new System.Windows.Forms.CheckBox();
            this.guiTVR3_1 = new System.Windows.Forms.CheckBox();
            this.guiTVR4_7 = new System.Windows.Forms.CheckBox();
            this.guiTVR4_8 = new System.Windows.Forms.CheckBox();
            this.guiTVR4_6 = new System.Windows.Forms.CheckBox();
            this.guiTVR4_5 = new System.Windows.Forms.CheckBox();
            this.guiTVR4_4 = new System.Windows.Forms.CheckBox();
            this.guiTVR4_3 = new System.Windows.Forms.CheckBox();
            this.guiTVR4_2 = new System.Windows.Forms.CheckBox();
            this.guiTVR4_1 = new System.Windows.Forms.CheckBox();
            this.guiTVR5_7 = new System.Windows.Forms.CheckBox();
            this.guiTVR5_8 = new System.Windows.Forms.CheckBox();
            this.guiTVR5_6 = new System.Windows.Forms.CheckBox();
            this.guiTVR5_5 = new System.Windows.Forms.CheckBox();
            this.guiTVR5_4 = new System.Windows.Forms.CheckBox();
            this.guiTVR5_3 = new System.Windows.Forms.CheckBox();
            this.guiTVR5_2 = new System.Windows.Forms.CheckBox();
            this.guiTVR5_1 = new System.Windows.Forms.CheckBox();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabTVR = new System.Windows.Forms.TabPage();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            guiTabParameters = new System.Windows.Forms.TabPage();
            groupBox9 = new System.Windows.Forms.GroupBox();
            label3 = new System.Windows.Forms.Label();
            groupBox8 = new System.Windows.Forms.GroupBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            groupBox3 = new System.Windows.Forms.GroupBox();
            groupBox4 = new System.Windows.Forms.GroupBox();
            groupBox10 = new System.Windows.Forms.GroupBox();
            groupBox11 = new System.Windows.Forms.GroupBox();
            groupBox12 = new System.Windows.Forms.GroupBox();
            groupBox13 = new System.Windows.Forms.GroupBox();
            guiTabDetailedLogs = new System.Windows.Forms.TabPage();
            guiTabPSE = new System.Windows.Forms.TabPage();
            guiTabEMVApplications = new System.Windows.Forms.TabPage();
            guiTabEMVCardLog = new System.Windows.Forms.TabPage();
            guiTabPublicKeys = new System.Windows.Forms.TabPage();
            groupBox18 = new System.Windows.Forms.GroupBox();
            label15 = new System.Windows.Forms.Label();
            label16 = new System.Windows.Forms.Label();
            label14 = new System.Windows.Forms.Label();
            label17 = new System.Windows.Forms.Label();
            groupBox16 = new System.Windows.Forms.GroupBox();
            label11 = new System.Windows.Forms.Label();
            label12 = new System.Windows.Forms.Label();
            label13 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            groupBox14 = new System.Windows.Forms.GroupBox();
            label9 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            guiTabAuthentication = new System.Windows.Forms.TabPage();
            guiAuthentication = new System.Windows.Forms.TabControl();
            guiSDA = new System.Windows.Forms.TabPage();
            label23 = new System.Windows.Forms.Label();
            label22 = new System.Windows.Forms.Label();
            label21 = new System.Windows.Forms.Label();
            guiSDASignedDataColumnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            guiSDASignedDataColumnValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            label18 = new System.Windows.Forms.Label();
            guiDDA = new System.Windows.Forms.TabPage();
            columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            guiDDALabel_ICCDynamicNumber = new System.Windows.Forms.Label();
            label25 = new System.Windows.Forms.Label();
            label26 = new System.Windows.Forms.Label();
            label19 = new System.Windows.Forms.Label();
            guiCDA = new System.Windows.Forms.TabPage();
            columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            label20 = new System.Windows.Forms.Label();
            groupBox17 = new System.Windows.Forms.GroupBox();
            groupBox19 = new System.Windows.Forms.GroupBox();
            groupBox20 = new System.Windows.Forms.GroupBox();
            groupBox21 = new System.Windows.Forms.GroupBox();
            groupBox22 = new System.Windows.Forms.GroupBox();
            guiTabParameters.SuspendLayout();
            groupBox9.SuspendLayout();
            groupBox8.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox10.SuspendLayout();
            groupBox11.SuspendLayout();
            groupBox12.SuspendLayout();
            groupBox13.SuspendLayout();
            guiTabDetailedLogs.SuspendLayout();
            guiTabPSE.SuspendLayout();
            guiTabEMVApplications.SuspendLayout();
            guiTabEMVCardLog.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            guiTabPublicKeys.SuspendLayout();
            groupBox18.SuspendLayout();
            groupBox16.SuspendLayout();
            groupBox14.SuspendLayout();
            guiTabAuthentication.SuspendLayout();
            guiAuthentication.SuspendLayout();
            guiSDA.SuspendLayout();
            this.guiSDAAuthenticationData.SuspendLayout();
            guiDDA.SuspendLayout();
            this.groupBox15.SuspendLayout();
            guiCDA.SuspendLayout();
            groupBox17.SuspendLayout();
            groupBox19.SuspendLayout();
            groupBox20.SuspendLayout();
            groupBox21.SuspendLayout();
            groupBox22.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabTVR.SuspendLayout();
            this.SuspendLayout();
            // 
            // guiTabParameters
            // 
            guiTabParameters.Controls.Add(groupBox9);
            guiTabParameters.Controls.Add(groupBox8);
            guiTabParameters.Location = new System.Drawing.Point(4, 22);
            guiTabParameters.Name = "guiTabParameters";
            guiTabParameters.Padding = new System.Windows.Forms.Padding(3);
            guiTabParameters.Size = new System.Drawing.Size(682, 699);
            guiTabParameters.TabIndex = 4;
            guiTabParameters.Text = "Parameters";
            guiTabParameters.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            groupBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            groupBox9.Controls.Add(label3);
            groupBox9.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            groupBox9.Location = new System.Drawing.Point(6, 619);
            groupBox9.Name = "groupBox9";
            groupBox9.Size = new System.Drawing.Size(670, 38);
            groupBox9.TabIndex = 1;
            groupBox9.TabStop = false;
            groupBox9.Text = "Warning";
            // 
            // label3
            // 
            label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(3, 22);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(424, 13);
            label3.TabIndex = 0;
            label3.Text = "Note that modifying these parameters will break conformity with base EMV specific" +
                "ations.";
            // 
            // groupBox8
            // 
            groupBox8.AutoSize = true;
            groupBox8.Controls.Add(this.guiParamsTagAIDInFCI);
            groupBox8.Location = new System.Drawing.Point(6, 6);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new System.Drawing.Size(250, 55);
            groupBox8.TabIndex = 0;
            groupBox8.TabStop = false;
            groupBox8.Text = "Tags Locations";
            // 
            // guiParamsTagAIDInFCI
            // 
            this.guiParamsTagAIDInFCI.AutoSize = true;
            this.guiParamsTagAIDInFCI.Location = new System.Drawing.Point(6, 19);
            this.guiParamsTagAIDInFCI.Name = "guiParamsTagAIDInFCI";
            this.guiParamsTagAIDInFCI.Size = new System.Drawing.Size(126, 17);
            this.guiParamsTagAIDInFCI.TabIndex = 0;
            this.guiParamsTagAIDInFCI.Text = "Search for AID in FCI";
            this.guiParamsTagAIDInFCI.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.AutoSize = true;
            groupBox1.Controls.Add(this.guiDoExplicitDiscoveryOfAID);
            groupBox1.Controls.Add(this.guiPSEName);
            groupBox1.Controls.Add(this.guiDoSelectPSE);
            groupBox1.Location = new System.Drawing.Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(300, 90);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Application Selection";
            // 
            // guiDoExplicitDiscoveryOfAID
            // 
            this.guiDoExplicitDiscoveryOfAID.Location = new System.Drawing.Point(6, 48);
            this.guiDoExplicitDiscoveryOfAID.Name = "guiDoExplicitDiscoveryOfAID";
            this.guiDoExplicitDiscoveryOfAID.Size = new System.Drawing.Size(288, 23);
            this.guiDoExplicitDiscoveryOfAID.TabIndex = 2;
            this.guiDoExplicitDiscoveryOfAID.Text = "Explicit Discovery of AIDs";
            this.guiDoExplicitDiscoveryOfAID.UseVisualStyleBackColor = true;
            this.guiDoExplicitDiscoveryOfAID.Click += new System.EventHandler(this.guiDoExplicitDiscoveryOfAID_Click);
            // 
            // guiPSEName
            // 
            this.guiPSEName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.guiPSEName.FormattingEnabled = true;
            this.guiPSEName.Location = new System.Drawing.Point(6, 19);
            this.guiPSEName.Name = "guiPSEName";
            this.guiPSEName.Size = new System.Drawing.Size(147, 21);
            this.guiPSEName.TabIndex = 0;
            // 
            // guiDoSelectPSE
            // 
            this.guiDoSelectPSE.Location = new System.Drawing.Point(159, 19);
            this.guiDoSelectPSE.Name = "guiDoSelectPSE";
            this.guiDoSelectPSE.Size = new System.Drawing.Size(135, 23);
            this.guiDoSelectPSE.TabIndex = 1;
            this.guiDoSelectPSE.Text = "Select PSE";
            this.guiDoSelectPSE.UseVisualStyleBackColor = true;
            this.guiDoSelectPSE.Click += new System.EventHandler(this.guiDoSelectPSE_Click);
            // 
            // groupBox2
            // 
            groupBox2.AutoSize = true;
            groupBox2.Controls.Add(this.guiApplicationAID);
            groupBox2.Controls.Add(this.guiDoSelectAID);
            groupBox2.Controls.Add(this.guiDoGetProcessingOptions);
            groupBox2.Location = new System.Drawing.Point(12, 108);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(300, 90);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Initiate Application Processing";
            // 
            // guiApplicationAID
            // 
            this.guiApplicationAID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.guiApplicationAID.Enabled = false;
            this.guiApplicationAID.FormattingEnabled = true;
            this.guiApplicationAID.Location = new System.Drawing.Point(6, 19);
            this.guiApplicationAID.Name = "guiApplicationAID";
            this.guiApplicationAID.Size = new System.Drawing.Size(147, 21);
            this.guiApplicationAID.TabIndex = 0;
            // 
            // guiDoSelectAID
            // 
            this.guiDoSelectAID.Enabled = false;
            this.guiDoSelectAID.Location = new System.Drawing.Point(159, 19);
            this.guiDoSelectAID.Name = "guiDoSelectAID";
            this.guiDoSelectAID.Size = new System.Drawing.Size(135, 23);
            this.guiDoSelectAID.TabIndex = 1;
            this.guiDoSelectAID.Text = "Select AID";
            this.guiDoSelectAID.UseVisualStyleBackColor = true;
            this.guiDoSelectAID.Click += new System.EventHandler(this.guiDoSelectAID_Click);
            // 
            // guiDoGetProcessingOptions
            // 
            this.guiDoGetProcessingOptions.Enabled = false;
            this.guiDoGetProcessingOptions.Location = new System.Drawing.Point(6, 48);
            this.guiDoGetProcessingOptions.Name = "guiDoGetProcessingOptions";
            this.guiDoGetProcessingOptions.Size = new System.Drawing.Size(288, 23);
            this.guiDoGetProcessingOptions.TabIndex = 2;
            this.guiDoGetProcessingOptions.Text = "Get Processing Options";
            this.guiDoGetProcessingOptions.UseVisualStyleBackColor = true;
            this.guiDoGetProcessingOptions.Click += new System.EventHandler(this.guiDoGetProcessingOptions_Click);
            // 
            // groupBox3
            // 
            groupBox3.AutoSize = true;
            groupBox3.Controls.Add(this.guiDoGetData);
            groupBox3.Controls.Add(this.guiDoReadRecords);
            groupBox3.Location = new System.Drawing.Point(12, 204);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(300, 90);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Read Application Data";
            // 
            // guiDoGetData
            // 
            this.guiDoGetData.Enabled = false;
            this.guiDoGetData.Location = new System.Drawing.Point(6, 48);
            this.guiDoGetData.Name = "guiDoGetData";
            this.guiDoGetData.Size = new System.Drawing.Size(288, 23);
            this.guiDoGetData.TabIndex = 1;
            this.guiDoGetData.Text = "Get Data";
            this.guiDoGetData.UseVisualStyleBackColor = true;
            this.guiDoGetData.Click += new System.EventHandler(this.guiDoGetData_Click);
            // 
            // guiDoReadRecords
            // 
            this.guiDoReadRecords.Enabled = false;
            this.guiDoReadRecords.Location = new System.Drawing.Point(6, 19);
            this.guiDoReadRecords.Name = "guiDoReadRecords";
            this.guiDoReadRecords.Size = new System.Drawing.Size(288, 23);
            this.guiDoReadRecords.TabIndex = 0;
            this.guiDoReadRecords.Text = "Read Application Data";
            this.guiDoReadRecords.UseVisualStyleBackColor = true;
            this.guiDoReadRecords.Click += new System.EventHandler(this.guiDoReadRecords_Click);
            // 
            // groupBox4
            // 
            groupBox4.AutoSize = true;
            groupBox4.Controls.Add(this.guiDoGetChallenge);
            groupBox4.Controls.Add(this.guiPINEntryUsed);
            groupBox4.Controls.Add(this.guiPINEntry);
            groupBox4.Controls.Add(this.guiCVMList);
            groupBox4.Controls.Add(this.guiDoVerifyCardholder);
            groupBox4.Location = new System.Drawing.Point(12, 365);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new System.Drawing.Size(300, 116);
            groupBox4.TabIndex = 4;
            groupBox4.TabStop = false;
            groupBox4.Text = "Cardholder Verification";
            // 
            // guiDoGetChallenge
            // 
            this.guiDoGetChallenge.Enabled = false;
            this.guiDoGetChallenge.Location = new System.Drawing.Point(6, 19);
            this.guiDoGetChallenge.Name = "guiDoGetChallenge";
            this.guiDoGetChallenge.Size = new System.Drawing.Size(288, 23);
            this.guiDoGetChallenge.TabIndex = 0;
            this.guiDoGetChallenge.Text = "Get Challenge";
            this.guiDoGetChallenge.UseVisualStyleBackColor = true;
            this.guiDoGetChallenge.Click += new System.EventHandler(this.guiDoGetChallenge_Click);
            // 
            // guiPINEntryUsed
            // 
            this.guiPINEntryUsed.AutoSize = true;
            this.guiPINEntryUsed.Enabled = false;
            this.guiPINEntryUsed.Location = new System.Drawing.Point(6, 48);
            this.guiPINEntryUsed.Name = "guiPINEntryUsed";
            this.guiPINEntryUsed.Size = new System.Drawing.Size(93, 17);
            this.guiPINEntryUsed.TabIndex = 1;
            this.guiPINEntryUsed.Text = "Plain text PIN:";
            this.guiPINEntryUsed.UseVisualStyleBackColor = true;
            this.guiPINEntryUsed.CheckedChanged += new System.EventHandler(this.guiPINEntryUsed_CheckedChanged);
            // 
            // guiPINEntry
            // 
            this.guiPINEntry.Enabled = false;
            this.guiPINEntry.Location = new System.Drawing.Point(105, 48);
            this.guiPINEntry.MaxLength = 12;
            this.guiPINEntry.Name = "guiPINEntry";
            this.guiPINEntry.PasswordChar = '*';
            this.guiPINEntry.Size = new System.Drawing.Size(129, 20);
            this.guiPINEntry.TabIndex = 2;
            // 
            // guiCVMList
            // 
            this.guiCVMList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.guiCVMList.Enabled = false;
            this.guiCVMList.FormattingEnabled = true;
            this.guiCVMList.Location = new System.Drawing.Point(6, 74);
            this.guiCVMList.Name = "guiCVMList";
            this.guiCVMList.Size = new System.Drawing.Size(147, 21);
            this.guiCVMList.TabIndex = 3;
            this.guiCVMList.SelectedIndexChanged += new System.EventHandler(this.guiCVMList_SelectedIndexChanged);
            // 
            // guiDoVerifyCardholder
            // 
            this.guiDoVerifyCardholder.Enabled = false;
            this.guiDoVerifyCardholder.Location = new System.Drawing.Point(159, 74);
            this.guiDoVerifyCardholder.Name = "guiDoVerifyCardholder";
            this.guiDoVerifyCardholder.Size = new System.Drawing.Size(135, 23);
            this.guiDoVerifyCardholder.TabIndex = 4;
            this.guiDoVerifyCardholder.Text = "Verify";
            this.guiDoVerifyCardholder.UseVisualStyleBackColor = true;
            this.guiDoVerifyCardholder.Click += new System.EventHandler(this.guiDoVerifyCardholder_Click);
            // 
            // groupBox10
            // 
            groupBox10.AutoSize = true;
            groupBox10.Controls.Add(this.label4);
            groupBox10.Controls.Add(this.guiInternalAuthenticateUnpredictableNumber);
            groupBox10.Controls.Add(this.guiDoInternalAuthenticate);
            groupBox10.Location = new System.Drawing.Point(12, 300);
            groupBox10.Name = "groupBox10";
            groupBox10.Size = new System.Drawing.Size(300, 59);
            groupBox10.TabIndex = 3;
            groupBox10.TabStop = false;
            groupBox10.Text = "Offline Data Authentication";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Un.Number:";
            // 
            // guiInternalAuthenticateUnpredictableNumber
            // 
            this.guiInternalAuthenticateUnpredictableNumber.Enabled = false;
            this.guiInternalAuthenticateUnpredictableNumber.Location = new System.Drawing.Point(76, 19);
            this.guiInternalAuthenticateUnpredictableNumber.MaxLength = 8;
            this.guiInternalAuthenticateUnpredictableNumber.Name = "guiInternalAuthenticateUnpredictableNumber";
            this.guiInternalAuthenticateUnpredictableNumber.Size = new System.Drawing.Size(77, 20);
            this.guiInternalAuthenticateUnpredictableNumber.TabIndex = 1;
            this.guiInternalAuthenticateUnpredictableNumber.Text = "01020304";
            // 
            // guiDoInternalAuthenticate
            // 
            this.guiDoInternalAuthenticate.Enabled = false;
            this.guiDoInternalAuthenticate.Location = new System.Drawing.Point(159, 17);
            this.guiDoInternalAuthenticate.Name = "guiDoInternalAuthenticate";
            this.guiDoInternalAuthenticate.Size = new System.Drawing.Size(135, 23);
            this.guiDoInternalAuthenticate.TabIndex = 2;
            this.guiDoInternalAuthenticate.Text = "Internal Authenticate";
            this.guiDoInternalAuthenticate.UseVisualStyleBackColor = true;
            this.guiDoInternalAuthenticate.Click += new System.EventHandler(this.guiDoInternalAuthenticate_Click);
            // 
            // groupBox11
            // 
            groupBox11.AutoSize = true;
            groupBox11.Controls.Add(this.label24);
            groupBox11.Controls.Add(this.guiAC1UnpredictableNumber);
            groupBox11.Controls.Add(this.guiAC1Type);
            groupBox11.Controls.Add(this.guiDoGenerateAC1);
            groupBox11.Location = new System.Drawing.Point(12, 487);
            groupBox11.Name = "groupBox11";
            groupBox11.Size = new System.Drawing.Size(300, 85);
            groupBox11.TabIndex = 5;
            groupBox11.TabStop = false;
            groupBox11.Text = "Card Action Analysis";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(6, 22);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(64, 13);
            this.label24.TabIndex = 2;
            this.label24.Text = "Un.Number:";
            // 
            // guiAC1UnpredictableNumber
            // 
            this.guiAC1UnpredictableNumber.Enabled = false;
            this.guiAC1UnpredictableNumber.Location = new System.Drawing.Point(76, 19);
            this.guiAC1UnpredictableNumber.MaxLength = 8;
            this.guiAC1UnpredictableNumber.Name = "guiAC1UnpredictableNumber";
            this.guiAC1UnpredictableNumber.Size = new System.Drawing.Size(77, 20);
            this.guiAC1UnpredictableNumber.TabIndex = 3;
            this.guiAC1UnpredictableNumber.Text = "01020304";
            // 
            // guiAC1Type
            // 
            this.guiAC1Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.guiAC1Type.Enabled = false;
            this.guiAC1Type.FormattingEnabled = true;
            this.guiAC1Type.Location = new System.Drawing.Point(6, 45);
            this.guiAC1Type.Name = "guiAC1Type";
            this.guiAC1Type.Size = new System.Drawing.Size(147, 21);
            this.guiAC1Type.TabIndex = 0;
            // 
            // guiDoGenerateAC1
            // 
            this.guiDoGenerateAC1.Enabled = false;
            this.guiDoGenerateAC1.Location = new System.Drawing.Point(159, 43);
            this.guiDoGenerateAC1.Name = "guiDoGenerateAC1";
            this.guiDoGenerateAC1.Size = new System.Drawing.Size(135, 23);
            this.guiDoGenerateAC1.TabIndex = 1;
            this.guiDoGenerateAC1.Text = "Generate AC";
            this.guiDoGenerateAC1.UseVisualStyleBackColor = true;
            this.guiDoGenerateAC1.Click += new System.EventHandler(this.guiDoGenerateAC1_Click);
            // 
            // groupBox12
            // 
            groupBox12.AutoSize = true;
            groupBox12.Controls.Add(this.guiDoExternalAuthenticate);
            groupBox12.Location = new System.Drawing.Point(12, 578);
            groupBox12.Name = "groupBox12";
            groupBox12.Size = new System.Drawing.Size(300, 61);
            groupBox12.TabIndex = 6;
            groupBox12.TabStop = false;
            groupBox12.Text = "Issuer Authentication";
            // 
            // guiDoExternalAuthenticate
            // 
            this.guiDoExternalAuthenticate.Enabled = false;
            this.guiDoExternalAuthenticate.Location = new System.Drawing.Point(6, 19);
            this.guiDoExternalAuthenticate.Name = "guiDoExternalAuthenticate";
            this.guiDoExternalAuthenticate.Size = new System.Drawing.Size(288, 23);
            this.guiDoExternalAuthenticate.TabIndex = 0;
            this.guiDoExternalAuthenticate.Text = "External Authenticate";
            this.guiDoExternalAuthenticate.UseVisualStyleBackColor = true;
            this.guiDoExternalAuthenticate.Click += new System.EventHandler(this.guiDoExternalAuthenticate_Click);
            // 
            // groupBox13
            // 
            groupBox13.AutoSize = true;
            groupBox13.Controls.Add(this.guiDoGenerateAC2);
            groupBox13.Controls.Add(this.guiAC2Type);
            groupBox13.Location = new System.Drawing.Point(12, 645);
            groupBox13.Name = "groupBox13";
            groupBox13.Size = new System.Drawing.Size(300, 61);
            groupBox13.TabIndex = 7;
            groupBox13.TabStop = false;
            groupBox13.Text = "Completion";
            // 
            // guiDoGenerateAC2
            // 
            this.guiDoGenerateAC2.Enabled = false;
            this.guiDoGenerateAC2.Location = new System.Drawing.Point(159, 19);
            this.guiDoGenerateAC2.Name = "guiDoGenerateAC2";
            this.guiDoGenerateAC2.Size = new System.Drawing.Size(135, 23);
            this.guiDoGenerateAC2.TabIndex = 1;
            this.guiDoGenerateAC2.Text = "Generate AC";
            this.guiDoGenerateAC2.UseVisualStyleBackColor = true;
            this.guiDoGenerateAC2.Click += new System.EventHandler(this.guiDoGenerateAC2_Click);
            // 
            // guiAC2Type
            // 
            this.guiAC2Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.guiAC2Type.Enabled = false;
            this.guiAC2Type.FormattingEnabled = true;
            this.guiAC2Type.Location = new System.Drawing.Point(6, 19);
            this.guiAC2Type.Name = "guiAC2Type";
            this.guiAC2Type.Size = new System.Drawing.Size(147, 21);
            this.guiAC2Type.TabIndex = 0;
            // 
            // guiTabDetailedLogs
            // 
            guiTabDetailedLogs.Controls.Add(this.guiDoSaveDetailedLogs);
            guiTabDetailedLogs.Controls.Add(this.guiDetailedLogs);
            guiTabDetailedLogs.Location = new System.Drawing.Point(4, 22);
            guiTabDetailedLogs.Name = "guiTabDetailedLogs";
            guiTabDetailedLogs.Padding = new System.Windows.Forms.Padding(3);
            guiTabDetailedLogs.Size = new System.Drawing.Size(682, 699);
            guiTabDetailedLogs.TabIndex = 1;
            guiTabDetailedLogs.Text = "Detailed Logs";
            guiTabDetailedLogs.UseVisualStyleBackColor = true;
            // 
            // guiDoSaveDetailedLogs
            // 
            this.guiDoSaveDetailedLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.guiDoSaveDetailedLogs.Location = new System.Drawing.Point(524, 634);
            this.guiDoSaveDetailedLogs.Name = "guiDoSaveDetailedLogs";
            this.guiDoSaveDetailedLogs.Size = new System.Drawing.Size(150, 23);
            this.guiDoSaveDetailedLogs.TabIndex = 1;
            this.guiDoSaveDetailedLogs.Text = "Save Detailed Logs";
            this.guiDoSaveDetailedLogs.UseVisualStyleBackColor = true;
            this.guiDoSaveDetailedLogs.Click += new System.EventHandler(this.guiDoSaveDetailedLogs_Click);
            // 
            // guiDetailedLogs
            // 
            this.guiDetailedLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.guiDetailedLogs.Location = new System.Drawing.Point(6, 6);
            this.guiDetailedLogs.Name = "guiDetailedLogs";
            this.guiDetailedLogs.ReadOnly = true;
            this.guiDetailedLogs.Size = new System.Drawing.Size(670, 622);
            this.guiDetailedLogs.TabIndex = 0;
            this.guiDetailedLogs.Text = "";
            // 
            // guiTabPSE
            // 
            guiTabPSE.Controls.Add(this.guiPSEContent);
            guiTabPSE.Location = new System.Drawing.Point(4, 22);
            guiTabPSE.Name = "guiTabPSE";
            guiTabPSE.Padding = new System.Windows.Forms.Padding(3);
            guiTabPSE.Size = new System.Drawing.Size(682, 699);
            guiTabPSE.TabIndex = 2;
            guiTabPSE.Text = "PSE";
            guiTabPSE.ToolTipText = "Detailed Payment System Environment";
            guiTabPSE.UseVisualStyleBackColor = true;
            // 
            // guiPSEContent
            // 
            this.guiPSEContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.guiPSEContent.Location = new System.Drawing.Point(6, 6);
            this.guiPSEContent.Name = "guiPSEContent";
            this.guiPSEContent.Size = new System.Drawing.Size(668, 651);
            this.guiPSEContent.TabIndex = 0;
            // 
            // guiTabEMVApplications
            // 
            guiTabEMVApplications.Controls.Add(this.guiEMVApplicationsContent);
            guiTabEMVApplications.Location = new System.Drawing.Point(4, 22);
            guiTabEMVApplications.Name = "guiTabEMVApplications";
            guiTabEMVApplications.Padding = new System.Windows.Forms.Padding(3);
            guiTabEMVApplications.Size = new System.Drawing.Size(682, 699);
            guiTabEMVApplications.TabIndex = 3;
            guiTabEMVApplications.Text = "EMV Application";
            guiTabEMVApplications.UseVisualStyleBackColor = true;
            // 
            // guiEMVApplicationsContent
            // 
            this.guiEMVApplicationsContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.guiEMVApplicationsContent.Location = new System.Drawing.Point(6, 6);
            this.guiEMVApplicationsContent.Name = "guiEMVApplicationsContent";
            this.guiEMVApplicationsContent.Size = new System.Drawing.Size(668, 651);
            this.guiEMVApplicationsContent.TabIndex = 1;
            // 
            // guiTabEMVCardLog
            // 
            guiTabEMVCardLog.Controls.Add(this.guiLogRecords);
            guiTabEMVCardLog.Controls.Add(this.groupBox5);
            guiTabEMVCardLog.Location = new System.Drawing.Point(4, 22);
            guiTabEMVCardLog.Name = "guiTabEMVCardLog";
            guiTabEMVCardLog.Padding = new System.Windows.Forms.Padding(3);
            guiTabEMVCardLog.Size = new System.Drawing.Size(682, 699);
            guiTabEMVCardLog.TabIndex = 0;
            guiTabEMVCardLog.Text = "EMV Card Log";
            guiTabEMVCardLog.UseVisualStyleBackColor = true;
            // 
            // guiLogRecords
            // 
            this.guiLogRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.guiLogRecords.FullRowSelect = true;
            this.guiLogRecords.Location = new System.Drawing.Point(6, 217);
            this.guiLogRecords.Name = "guiLogRecords";
            this.guiLogRecords.Size = new System.Drawing.Size(668, 440);
            this.guiLogRecords.TabIndex = 1;
            this.guiLogRecords.UseCompatibleStateImageBehavior = false;
            this.guiLogRecords.View = System.Windows.Forms.View.Details;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.guiDoCardLogSave);
            this.groupBox5.Controls.Add(this.guiDoCardLogRead);
            this.groupBox5.Controls.Add(this.groupBox7);
            this.groupBox5.Controls.Add(this.groupBox6);
            this.groupBox5.Controls.Add(this.guiLogFilePresence);
            this.groupBox5.Location = new System.Drawing.Point(6, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(506, 205);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Log File Informations";
            // 
            // guiDoCardLogSave
            // 
            this.guiDoCardLogSave.Enabled = false;
            this.guiDoCardLogSave.Location = new System.Drawing.Point(350, 77);
            this.guiDoCardLogSave.Name = "guiDoCardLogSave";
            this.guiDoCardLogSave.Size = new System.Drawing.Size(150, 23);
            this.guiDoCardLogSave.TabIndex = 3;
            this.guiDoCardLogSave.Text = "Save EMV Card Log";
            this.guiDoCardLogSave.UseVisualStyleBackColor = true;
            this.guiDoCardLogSave.Click += new System.EventHandler(this.guiDoCardLogSave_Click);
            // 
            // guiDoCardLogRead
            // 
            this.guiDoCardLogRead.Enabled = false;
            this.guiDoCardLogRead.Location = new System.Drawing.Point(350, 19);
            this.guiDoCardLogRead.Name = "guiDoCardLogRead";
            this.guiDoCardLogRead.Size = new System.Drawing.Size(150, 23);
            this.guiDoCardLogRead.TabIndex = 2;
            this.guiDoCardLogRead.Text = "Read Records";
            this.guiDoCardLogRead.UseVisualStyleBackColor = true;
            this.guiDoCardLogRead.Click += new System.EventHandler(this.guiDoCardLogRead_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.guiCardLogFormat);
            this.groupBox7.Location = new System.Drawing.Point(6, 106);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(494, 80);
            this.groupBox7.TabIndex = 4;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Log Format";
            // 
            // guiCardLogFormat
            // 
            this.guiCardLogFormat.Location = new System.Drawing.Point(6, 19);
            this.guiCardLogFormat.Multiline = true;
            this.guiCardLogFormat.Name = "guiCardLogFormat";
            this.guiCardLogFormat.ReadOnly = true;
            this.guiCardLogFormat.Size = new System.Drawing.Size(482, 40);
            this.guiCardLogFormat.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.AutoSize = true;
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.guiCardLogLength);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.guiCardLogSFI);
            this.groupBox6.Location = new System.Drawing.Point(6, 42);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(233, 58);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Log Entry";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(84, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Number of records:";
            // 
            // guiCardLogLength
            // 
            this.guiCardLogLength.Location = new System.Drawing.Point(187, 19);
            this.guiCardLogLength.Name = "guiCardLogLength";
            this.guiCardLogLength.ReadOnly = true;
            this.guiCardLogLength.Size = new System.Drawing.Size(40, 20);
            this.guiCardLogLength.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "SFI:";
            // 
            // guiCardLogSFI
            // 
            this.guiCardLogSFI.Location = new System.Drawing.Point(38, 19);
            this.guiCardLogSFI.Name = "guiCardLogSFI";
            this.guiCardLogSFI.ReadOnly = true;
            this.guiCardLogSFI.Size = new System.Drawing.Size(40, 20);
            this.guiCardLogSFI.TabIndex = 1;
            // 
            // guiLogFilePresence
            // 
            this.guiLogFilePresence.AutoCheck = false;
            this.guiLogFilePresence.AutoSize = true;
            this.guiLogFilePresence.Location = new System.Drawing.Point(6, 19);
            this.guiLogFilePresence.Name = "guiLogFilePresence";
            this.guiLogFilePresence.Size = new System.Drawing.Size(110, 17);
            this.guiLogFilePresence.TabIndex = 0;
            this.guiLogFilePresence.Text = "Log File presence";
            this.guiLogFilePresence.UseVisualStyleBackColor = true;
            // 
            // guiTabPublicKeys
            // 
            guiTabPublicKeys.Controls.Add(groupBox18);
            guiTabPublicKeys.Controls.Add(groupBox16);
            guiTabPublicKeys.Controls.Add(groupBox14);
            guiTabPublicKeys.Location = new System.Drawing.Point(4, 22);
            guiTabPublicKeys.Name = "guiTabPublicKeys";
            guiTabPublicKeys.Padding = new System.Windows.Forms.Padding(3);
            guiTabPublicKeys.Size = new System.Drawing.Size(682, 699);
            guiTabPublicKeys.TabIndex = 5;
            guiTabPublicKeys.Text = "Public Keys";
            guiTabPublicKeys.UseVisualStyleBackColor = true;
            // 
            // groupBox18
            // 
            groupBox18.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            groupBox18.Controls.Add(label15);
            groupBox18.Controls.Add(this.guiPublicKeysICCPKModulus);
            groupBox18.Controls.Add(label16);
            groupBox18.Controls.Add(this.guiPublicKeysICCPKExponent);
            groupBox18.Controls.Add(this.guiPublicKeysICCPKHashResult);
            groupBox18.Controls.Add(label14);
            groupBox18.Controls.Add(label17);
            groupBox18.Controls.Add(this.guiPublicKeysICCPKRecoveredData);
            groupBox18.Location = new System.Drawing.Point(6, 403);
            groupBox18.Name = "groupBox18";
            groupBox18.Size = new System.Drawing.Size(668, 235);
            groupBox18.TabIndex = 2;
            groupBox18.TabStop = false;
            groupBox18.Text = "ICC Public Key";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new System.Drawing.Point(6, 159);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(50, 13);
            label15.TabIndex = 6;
            label15.Text = "Modulus:";
            // 
            // guiPublicKeysICCPKModulus
            // 
            this.guiPublicKeysICCPKModulus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.guiPublicKeysICCPKModulus.Location = new System.Drawing.Point(67, 156);
            this.guiPublicKeysICCPKModulus.Multiline = true;
            this.guiPublicKeysICCPKModulus.Name = "guiPublicKeysICCPKModulus";
            this.guiPublicKeysICCPKModulus.ReadOnly = true;
            this.guiPublicKeysICCPKModulus.Size = new System.Drawing.Size(595, 60);
            this.guiPublicKeysICCPKModulus.TabIndex = 7;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new System.Drawing.Point(6, 133);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(55, 13);
            label16.TabIndex = 4;
            label16.Text = "Exponent:";
            // 
            // guiPublicKeysICCPKExponent
            // 
            this.guiPublicKeysICCPKExponent.Location = new System.Drawing.Point(67, 130);
            this.guiPublicKeysICCPKExponent.Name = "guiPublicKeysICCPKExponent";
            this.guiPublicKeysICCPKExponent.ReadOnly = true;
            this.guiPublicKeysICCPKExponent.Size = new System.Drawing.Size(60, 20);
            this.guiPublicKeysICCPKExponent.TabIndex = 5;
            // 
            // guiPublicKeysICCPKHashResult
            // 
            this.guiPublicKeysICCPKHashResult.Location = new System.Drawing.Point(73, 104);
            this.guiPublicKeysICCPKHashResult.Name = "guiPublicKeysICCPKHashResult";
            this.guiPublicKeysICCPKHashResult.ReadOnly = true;
            this.guiPublicKeysICCPKHashResult.Size = new System.Drawing.Size(400, 20);
            this.guiPublicKeysICCPKHashResult.TabIndex = 3;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new System.Drawing.Point(6, 107);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(68, 13);
            label14.TabIndex = 2;
            label14.Text = "Hash Result:";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new System.Drawing.Point(6, 22);
            label17.Name = "label17";
            label17.Size = new System.Drawing.Size(235, 13);
            label17.TabIndex = 0;
            label17.Text = "Recovered Data from ICC Public Key Certificate:";
            // 
            // guiPublicKeysICCPKRecoveredData
            // 
            this.guiPublicKeysICCPKRecoveredData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.guiPublicKeysICCPKRecoveredData.Location = new System.Drawing.Point(6, 38);
            this.guiPublicKeysICCPKRecoveredData.Multiline = true;
            this.guiPublicKeysICCPKRecoveredData.Name = "guiPublicKeysICCPKRecoveredData";
            this.guiPublicKeysICCPKRecoveredData.ReadOnly = true;
            this.guiPublicKeysICCPKRecoveredData.Size = new System.Drawing.Size(656, 60);
            this.guiPublicKeysICCPKRecoveredData.TabIndex = 1;
            // 
            // groupBox16
            // 
            groupBox16.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            groupBox16.Controls.Add(label11);
            groupBox16.Controls.Add(this.guiPublicKeysIssuerPKModulus);
            groupBox16.Controls.Add(label12);
            groupBox16.Controls.Add(this.guiPublicKeysIssuerPKExponent);
            groupBox16.Controls.Add(this.guiPublicKeysIssuerPKHashResult);
            groupBox16.Controls.Add(label13);
            groupBox16.Controls.Add(label10);
            groupBox16.Controls.Add(this.guiPublicKeysIssuerPKRecoveredData);
            groupBox16.Location = new System.Drawing.Point(6, 162);
            groupBox16.Name = "groupBox16";
            groupBox16.Size = new System.Drawing.Size(668, 235);
            groupBox16.TabIndex = 1;
            groupBox16.TabStop = false;
            groupBox16.Text = "Issuer Public Key";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(6, 159);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(50, 13);
            label11.TabIndex = 7;
            label11.Text = "Modulus:";
            // 
            // guiPublicKeysIssuerPKModulus
            // 
            this.guiPublicKeysIssuerPKModulus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.guiPublicKeysIssuerPKModulus.Location = new System.Drawing.Point(67, 156);
            this.guiPublicKeysIssuerPKModulus.Multiline = true;
            this.guiPublicKeysIssuerPKModulus.Name = "guiPublicKeysIssuerPKModulus";
            this.guiPublicKeysIssuerPKModulus.ReadOnly = true;
            this.guiPublicKeysIssuerPKModulus.Size = new System.Drawing.Size(595, 60);
            this.guiPublicKeysIssuerPKModulus.TabIndex = 0;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(6, 133);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(55, 13);
            label12.TabIndex = 5;
            label12.Text = "Exponent:";
            // 
            // guiPublicKeysIssuerPKExponent
            // 
            this.guiPublicKeysIssuerPKExponent.Location = new System.Drawing.Point(67, 130);
            this.guiPublicKeysIssuerPKExponent.Name = "guiPublicKeysIssuerPKExponent";
            this.guiPublicKeysIssuerPKExponent.ReadOnly = true;
            this.guiPublicKeysIssuerPKExponent.Size = new System.Drawing.Size(60, 20);
            this.guiPublicKeysIssuerPKExponent.TabIndex = 6;
            // 
            // guiPublicKeysIssuerPKHashResult
            // 
            this.guiPublicKeysIssuerPKHashResult.Location = new System.Drawing.Point(73, 104);
            this.guiPublicKeysIssuerPKHashResult.Name = "guiPublicKeysIssuerPKHashResult";
            this.guiPublicKeysIssuerPKHashResult.ReadOnly = true;
            this.guiPublicKeysIssuerPKHashResult.Size = new System.Drawing.Size(400, 20);
            this.guiPublicKeysIssuerPKHashResult.TabIndex = 4;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new System.Drawing.Point(6, 107);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(68, 13);
            label13.TabIndex = 3;
            label13.Text = "Hash Result:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(6, 22);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(246, 13);
            label10.TabIndex = 1;
            label10.Text = "Recovered Data from Issuer Public Key Certificate:";
            // 
            // guiPublicKeysIssuerPKRecoveredData
            // 
            this.guiPublicKeysIssuerPKRecoveredData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.guiPublicKeysIssuerPKRecoveredData.Location = new System.Drawing.Point(6, 38);
            this.guiPublicKeysIssuerPKRecoveredData.Multiline = true;
            this.guiPublicKeysIssuerPKRecoveredData.Name = "guiPublicKeysIssuerPKRecoveredData";
            this.guiPublicKeysIssuerPKRecoveredData.ReadOnly = true;
            this.guiPublicKeysIssuerPKRecoveredData.Size = new System.Drawing.Size(656, 60);
            this.guiPublicKeysIssuerPKRecoveredData.TabIndex = 2;
            // 
            // groupBox14
            // 
            groupBox14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            groupBox14.Controls.Add(label9);
            groupBox14.Controls.Add(this.guiPublicKeysCertificationAuthorityPKModulus);
            groupBox14.Controls.Add(label8);
            groupBox14.Controls.Add(this.guiPublicKeysCertificationAuthorityPKExponent);
            groupBox14.Controls.Add(this.guiPublicKeysCAIndex);
            groupBox14.Controls.Add(label7);
            groupBox14.Controls.Add(this.guiPublicKeysAID);
            groupBox14.Controls.Add(label6);
            groupBox14.Controls.Add(label5);
            groupBox14.Controls.Add(this.guiPublicKeysRID);
            groupBox14.Location = new System.Drawing.Point(6, 6);
            groupBox14.Name = "groupBox14";
            groupBox14.Size = new System.Drawing.Size(668, 150);
            groupBox14.TabIndex = 0;
            groupBox14.TabStop = false;
            groupBox14.Text = "Certification Authority";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(6, 71);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(50, 13);
            label9.TabIndex = 0;
            label9.Text = "Modulus:";
            // 
            // guiPublicKeysCertificationAuthorityPKModulus
            // 
            this.guiPublicKeysCertificationAuthorityPKModulus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.guiPublicKeysCertificationAuthorityPKModulus.Location = new System.Drawing.Point(67, 71);
            this.guiPublicKeysCertificationAuthorityPKModulus.Multiline = true;
            this.guiPublicKeysCertificationAuthorityPKModulus.Name = "guiPublicKeysCertificationAuthorityPKModulus";
            this.guiPublicKeysCertificationAuthorityPKModulus.ReadOnly = true;
            this.guiPublicKeysCertificationAuthorityPKModulus.Size = new System.Drawing.Size(595, 60);
            this.guiPublicKeysCertificationAuthorityPKModulus.TabIndex = 1;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(6, 48);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(55, 13);
            label8.TabIndex = 8;
            label8.Text = "Exponent:";
            // 
            // guiPublicKeysCertificationAuthorityPKExponent
            // 
            this.guiPublicKeysCertificationAuthorityPKExponent.Location = new System.Drawing.Point(67, 45);
            this.guiPublicKeysCertificationAuthorityPKExponent.Name = "guiPublicKeysCertificationAuthorityPKExponent";
            this.guiPublicKeysCertificationAuthorityPKExponent.ReadOnly = true;
            this.guiPublicKeysCertificationAuthorityPKExponent.Size = new System.Drawing.Size(60, 20);
            this.guiPublicKeysCertificationAuthorityPKExponent.TabIndex = 9;
            // 
            // guiPublicKeysCAIndex
            // 
            this.guiPublicKeysCAIndex.Location = new System.Drawing.Point(461, 19);
            this.guiPublicKeysCAIndex.Name = "guiPublicKeysCAIndex";
            this.guiPublicKeysCAIndex.ReadOnly = true;
            this.guiPublicKeysCAIndex.Size = new System.Drawing.Size(20, 20);
            this.guiPublicKeysCAIndex.TabIndex = 7;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(317, 22);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(138, 13);
            label7.TabIndex = 6;
            label7.Text = "Certification Authority Index:";
            // 
            // guiPublicKeysAID
            // 
            this.guiPublicKeysAID.Location = new System.Drawing.Point(40, 19);
            this.guiPublicKeysAID.Name = "guiPublicKeysAID";
            this.guiPublicKeysAID.ReadOnly = true;
            this.guiPublicKeysAID.Size = new System.Drawing.Size(150, 20);
            this.guiPublicKeysAID.TabIndex = 1;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(6, 22);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(28, 13);
            label6.TabIndex = 0;
            label6.Text = "AID:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(196, 22);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(29, 13);
            label5.TabIndex = 2;
            label5.Text = "RID:";
            // 
            // guiPublicKeysRID
            // 
            this.guiPublicKeysRID.Location = new System.Drawing.Point(231, 19);
            this.guiPublicKeysRID.Name = "guiPublicKeysRID";
            this.guiPublicKeysRID.ReadOnly = true;
            this.guiPublicKeysRID.Size = new System.Drawing.Size(80, 20);
            this.guiPublicKeysRID.TabIndex = 3;
            // 
            // guiTabAuthentication
            // 
            guiTabAuthentication.Controls.Add(guiAuthentication);
            guiTabAuthentication.Location = new System.Drawing.Point(4, 22);
            guiTabAuthentication.Name = "guiTabAuthentication";
            guiTabAuthentication.Size = new System.Drawing.Size(682, 699);
            guiTabAuthentication.TabIndex = 6;
            guiTabAuthentication.Text = "Authentication";
            guiTabAuthentication.UseVisualStyleBackColor = true;
            // 
            // guiAuthentication
            // 
            guiAuthentication.Controls.Add(guiSDA);
            guiAuthentication.Controls.Add(guiDDA);
            guiAuthentication.Controls.Add(guiCDA);
            guiAuthentication.Dock = System.Windows.Forms.DockStyle.Fill;
            guiAuthentication.Location = new System.Drawing.Point(0, 0);
            guiAuthentication.Name = "guiAuthentication";
            guiAuthentication.SelectedIndex = 0;
            guiAuthentication.Size = new System.Drawing.Size(682, 699);
            guiAuthentication.TabIndex = 0;
            // 
            // guiSDA
            // 
            guiSDA.Controls.Add(this.guiSDAAuthenticationData);
            guiSDA.Controls.Add(this.guiSDASignedData);
            guiSDA.Controls.Add(label18);
            guiSDA.Location = new System.Drawing.Point(4, 22);
            guiSDA.Name = "guiSDA";
            guiSDA.Padding = new System.Windows.Forms.Padding(3);
            guiSDA.Size = new System.Drawing.Size(674, 673);
            guiSDA.TabIndex = 0;
            guiSDA.Text = "Static (SDA)";
            guiSDA.UseVisualStyleBackColor = true;
            // 
            // guiSDAAuthenticationData
            // 
            this.guiSDAAuthenticationData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.guiSDAAuthenticationData.Controls.Add(this.guiSDADataAuthenticationCode);
            this.guiSDAAuthenticationData.Controls.Add(label23);
            this.guiSDAAuthenticationData.Controls.Add(this.guiSDAHashResult);
            this.guiSDAAuthenticationData.Controls.Add(label22);
            this.guiSDAAuthenticationData.Controls.Add(label21);
            this.guiSDAAuthenticationData.Controls.Add(this.guiSDARecoveredData);
            this.guiSDAAuthenticationData.Location = new System.Drawing.Point(6, 185);
            this.guiSDAAuthenticationData.Name = "guiSDAAuthenticationData";
            this.guiSDAAuthenticationData.Size = new System.Drawing.Size(662, 163);
            this.guiSDAAuthenticationData.TabIndex = 2;
            this.guiSDAAuthenticationData.TabStop = false;
            this.guiSDAAuthenticationData.Text = "Authentication Data";
            // 
            // guiSDADataAuthenticationCode
            // 
            this.guiSDADataAuthenticationCode.Location = new System.Drawing.Point(144, 124);
            this.guiSDADataAuthenticationCode.Name = "guiSDADataAuthenticationCode";
            this.guiSDADataAuthenticationCode.ReadOnly = true;
            this.guiSDADataAuthenticationCode.Size = new System.Drawing.Size(200, 20);
            this.guiSDADataAuthenticationCode.TabIndex = 0;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new System.Drawing.Point(6, 127);
            label23.Name = "label23";
            label23.Size = new System.Drawing.Size(132, 13);
            label23.TabIndex = 5;
            label23.Text = "Data Authentication Code:";
            // 
            // guiSDAHashResult
            // 
            this.guiSDAHashResult.Location = new System.Drawing.Point(80, 98);
            this.guiSDAHashResult.Name = "guiSDAHashResult";
            this.guiSDAHashResult.ReadOnly = true;
            this.guiSDAHashResult.Size = new System.Drawing.Size(400, 20);
            this.guiSDAHashResult.TabIndex = 3;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new System.Drawing.Point(6, 101);
            label22.Name = "label22";
            label22.Size = new System.Drawing.Size(68, 13);
            label22.TabIndex = 2;
            label22.Text = "Hash Result:";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new System.Drawing.Point(6, 16);
            label21.Name = "label21";
            label21.Size = new System.Drawing.Size(256, 13);
            label21.TabIndex = 0;
            label21.Text = "Recovered Data from Signed Static Application Data";
            // 
            // guiSDARecoveredData
            // 
            this.guiSDARecoveredData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.guiSDARecoveredData.Location = new System.Drawing.Point(6, 32);
            this.guiSDARecoveredData.Multiline = true;
            this.guiSDARecoveredData.Name = "guiSDARecoveredData";
            this.guiSDARecoveredData.ReadOnly = true;
            this.guiSDARecoveredData.Size = new System.Drawing.Size(650, 60);
            this.guiSDARecoveredData.TabIndex = 1;
            // 
            // guiSDASignedData
            // 
            this.guiSDASignedData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.guiSDASignedData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.guiSDASignedDataColumnId,
            this.guiSDASignedDataTag,
            guiSDASignedDataColumnName,
            guiSDASignedDataColumnValue});
            this.guiSDASignedData.FullRowSelect = true;
            this.guiSDASignedData.Location = new System.Drawing.Point(6, 19);
            this.guiSDASignedData.MultiSelect = false;
            this.guiSDASignedData.Name = "guiSDASignedData";
            this.guiSDASignedData.Size = new System.Drawing.Size(662, 160);
            this.guiSDASignedData.TabIndex = 1;
            this.guiSDASignedData.UseCompatibleStateImageBehavior = false;
            this.guiSDASignedData.View = System.Windows.Forms.View.Details;
            // 
            // guiSDASignedDataColumnId
            // 
            this.guiSDASignedDataColumnId.Text = "";
            this.guiSDASignedDataColumnId.Width = 20;
            // 
            // guiSDASignedDataTag
            // 
            this.guiSDASignedDataTag.Text = "Tag";
            this.guiSDASignedDataTag.Width = 50;
            // 
            // guiSDASignedDataColumnName
            // 
            guiSDASignedDataColumnName.Text = "Data Name";
            guiSDASignedDataColumnName.Width = 230;
            // 
            // guiSDASignedDataColumnValue
            // 
            guiSDASignedDataColumnValue.Text = "Value";
            guiSDASignedDataColumnValue.Width = 350;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new System.Drawing.Point(6, 3);
            label18.Name = "label18";
            label18.Size = new System.Drawing.Size(66, 13);
            label18.TabIndex = 0;
            label18.Text = "Signed Data";
            // 
            // guiDDA
            // 
            guiDDA.Controls.Add(this.guiDDASignedData);
            guiDDA.Controls.Add(this.groupBox15);
            guiDDA.Controls.Add(label19);
            guiDDA.Location = new System.Drawing.Point(4, 22);
            guiDDA.Name = "guiDDA";
            guiDDA.Padding = new System.Windows.Forms.Padding(3);
            guiDDA.Size = new System.Drawing.Size(674, 673);
            guiDDA.TabIndex = 1;
            guiDDA.Text = "Dynamic (DDA)";
            guiDDA.UseVisualStyleBackColor = true;
            // 
            // guiDDASignedData
            // 
            this.guiDDASignedData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.guiDDASignedData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            columnHeader3,
            columnHeader4});
            this.guiDDASignedData.FullRowSelect = true;
            this.guiDDASignedData.Location = new System.Drawing.Point(6, 19);
            this.guiDDASignedData.MultiSelect = false;
            this.guiDDASignedData.Name = "guiDDASignedData";
            this.guiDDASignedData.Size = new System.Drawing.Size(662, 160);
            this.guiDDASignedData.TabIndex = 1;
            this.guiDDASignedData.UseCompatibleStateImageBehavior = false;
            this.guiDDASignedData.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 20;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Tag";
            this.columnHeader2.Width = 50;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Data Name";
            columnHeader3.Width = 230;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Value";
            columnHeader4.Width = 350;
            // 
            // groupBox15
            // 
            this.groupBox15.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox15.Controls.Add(this.guiDDAICCDynamicData);
            this.groupBox15.Controls.Add(guiDDALabel_ICCDynamicNumber);
            this.groupBox15.Controls.Add(this.guiDDAHashResult);
            this.groupBox15.Controls.Add(label25);
            this.groupBox15.Controls.Add(label26);
            this.groupBox15.Controls.Add(this.guiDDARecoveredData);
            this.groupBox15.Location = new System.Drawing.Point(6, 185);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(662, 163);
            this.groupBox15.TabIndex = 2;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Authentication Data";
            // 
            // guiDDAICCDynamicData
            // 
            this.guiDDAICCDynamicData.Location = new System.Drawing.Point(144, 124);
            this.guiDDAICCDynamicData.Name = "guiDDAICCDynamicData";
            this.guiDDAICCDynamicData.ReadOnly = true;
            this.guiDDAICCDynamicData.Size = new System.Drawing.Size(200, 20);
            this.guiDDAICCDynamicData.TabIndex = 5;
            // 
            // guiDDALabel_ICCDynamicNumber
            // 
            guiDDALabel_ICCDynamicNumber.AutoSize = true;
            guiDDALabel_ICCDynamicNumber.Location = new System.Drawing.Point(6, 127);
            guiDDALabel_ICCDynamicNumber.Name = "guiDDALabel_ICCDynamicNumber";
            guiDDALabel_ICCDynamicNumber.Size = new System.Drawing.Size(97, 13);
            guiDDALabel_ICCDynamicNumber.TabIndex = 4;
            guiDDALabel_ICCDynamicNumber.Text = "ICC Dynamic Data:";
            // 
            // guiDDAHashResult
            // 
            this.guiDDAHashResult.Location = new System.Drawing.Point(80, 98);
            this.guiDDAHashResult.Name = "guiDDAHashResult";
            this.guiDDAHashResult.ReadOnly = true;
            this.guiDDAHashResult.Size = new System.Drawing.Size(400, 20);
            this.guiDDAHashResult.TabIndex = 3;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new System.Drawing.Point(6, 101);
            label25.Name = "label25";
            label25.Size = new System.Drawing.Size(68, 13);
            label25.TabIndex = 2;
            label25.Text = "Hash Result:";
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new System.Drawing.Point(6, 16);
            label26.Name = "label26";
            label26.Size = new System.Drawing.Size(270, 13);
            label26.TabIndex = 0;
            label26.Text = "Recovered Data from Signed Dynamic Application Data";
            // 
            // guiDDARecoveredData
            // 
            this.guiDDARecoveredData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.guiDDARecoveredData.Location = new System.Drawing.Point(6, 32);
            this.guiDDARecoveredData.Multiline = true;
            this.guiDDARecoveredData.Name = "guiDDARecoveredData";
            this.guiDDARecoveredData.ReadOnly = true;
            this.guiDDARecoveredData.Size = new System.Drawing.Size(650, 60);
            this.guiDDARecoveredData.TabIndex = 1;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new System.Drawing.Point(6, 3);
            label19.Name = "label19";
            label19.Size = new System.Drawing.Size(66, 13);
            label19.TabIndex = 0;
            label19.Text = "Signed Data";
            // 
            // guiCDA
            // 
            guiCDA.Controls.Add(this.guiCDASignedData);
            guiCDA.Controls.Add(label20);
            guiCDA.Location = new System.Drawing.Point(4, 22);
            guiCDA.Name = "guiCDA";
            guiCDA.Size = new System.Drawing.Size(674, 673);
            guiCDA.TabIndex = 2;
            guiCDA.Text = "Combined (CDA)";
            guiCDA.UseVisualStyleBackColor = true;
            // 
            // guiCDASignedData
            // 
            this.guiCDASignedData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.guiCDASignedData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            columnHeader7,
            columnHeader8});
            this.guiCDASignedData.FullRowSelect = true;
            this.guiCDASignedData.Location = new System.Drawing.Point(6, 19);
            this.guiCDASignedData.MultiSelect = false;
            this.guiCDASignedData.Name = "guiCDASignedData";
            this.guiCDASignedData.Size = new System.Drawing.Size(662, 160);
            this.guiCDASignedData.TabIndex = 1;
            this.guiCDASignedData.UseCompatibleStateImageBehavior = false;
            this.guiCDASignedData.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "";
            this.columnHeader5.Width = 20;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Tag";
            this.columnHeader6.Width = 50;
            // 
            // columnHeader7
            // 
            columnHeader7.Text = "Data Name";
            columnHeader7.Width = 230;
            // 
            // columnHeader8
            // 
            columnHeader8.Text = "Value";
            columnHeader8.Width = 350;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new System.Drawing.Point(6, 3);
            label20.Name = "label20";
            label20.Size = new System.Drawing.Size(66, 13);
            label20.TabIndex = 0;
            label20.Text = "Signed Data";
            // 
            // groupBox17
            // 
            groupBox17.AutoSize = true;
            groupBox17.Controls.Add(this.guiTVR1_7);
            groupBox17.Controls.Add(this.guiTVR1_8);
            groupBox17.Controls.Add(this.guiTVR1_6);
            groupBox17.Controls.Add(this.guiTVR1_5);
            groupBox17.Controls.Add(this.guiTVR1_4);
            groupBox17.Controls.Add(this.guiTVR1_3);
            groupBox17.Controls.Add(this.guiTVR_1_2);
            groupBox17.Controls.Add(this.guiTVR_1_1);
            groupBox17.Location = new System.Drawing.Point(6, 6);
            groupBox17.Name = "groupBox17";
            groupBox17.Size = new System.Drawing.Size(285, 216);
            groupBox17.TabIndex = 8;
            groupBox17.TabStop = false;
            groupBox17.Text = "TVR Byte 1";
            // 
            // guiTVR1_7
            // 
            this.guiTVR1_7.AutoSize = true;
            this.guiTVR1_7.Location = new System.Drawing.Point(7, 157);
            this.guiTVR1_7.Name = "guiTVR1_7";
            this.guiTVR1_7.Size = new System.Drawing.Size(48, 17);
            this.guiTVR1_7.TabIndex = 15;
            this.guiTVR1_7.Text = "RFU";
            this.guiTVR1_7.UseVisualStyleBackColor = true;
            // 
            // guiTVR1_8
            // 
            this.guiTVR1_8.AutoSize = true;
            this.guiTVR1_8.Location = new System.Drawing.Point(7, 180);
            this.guiTVR1_8.Name = "guiTVR1_8";
            this.guiTVR1_8.Size = new System.Drawing.Size(48, 17);
            this.guiTVR1_8.TabIndex = 14;
            this.guiTVR1_8.Text = "RFU";
            this.guiTVR1_8.UseVisualStyleBackColor = true;
            // 
            // guiTVR1_6
            // 
            this.guiTVR1_6.AutoSize = true;
            this.guiTVR1_6.Location = new System.Drawing.Point(6, 134);
            this.guiTVR1_6.Name = "guiTVR1_6";
            this.guiTVR1_6.Size = new System.Drawing.Size(76, 17);
            this.guiTVR1_6.TabIndex = 13;
            this.guiTVR1_6.Text = "CDA failed";
            this.guiTVR1_6.UseVisualStyleBackColor = true;
            // 
            // guiTVR1_5
            // 
            this.guiTVR1_5.AutoSize = true;
            this.guiTVR1_5.Location = new System.Drawing.Point(6, 111);
            this.guiTVR1_5.Name = "guiTVR1_5";
            this.guiTVR1_5.Size = new System.Drawing.Size(77, 17);
            this.guiTVR1_5.TabIndex = 12;
            this.guiTVR1_5.Text = "DDA failed";
            this.guiTVR1_5.UseVisualStyleBackColor = true;
            // 
            // guiTVR1_4
            // 
            this.guiTVR1_4.AutoSize = true;
            this.guiTVR1_4.Location = new System.Drawing.Point(6, 88);
            this.guiTVR1_4.Name = "guiTVR1_4";
            this.guiTVR1_4.Size = new System.Drawing.Size(208, 17);
            this.guiTVR1_4.TabIndex = 11;
            this.guiTVR1_4.Text = "Card appears on terminal exception file";
            this.guiTVR1_4.UseVisualStyleBackColor = true;
            // 
            // guiTVR1_3
            // 
            this.guiTVR1_3.AutoSize = true;
            this.guiTVR1_3.Location = new System.Drawing.Point(6, 65);
            this.guiTVR1_3.Name = "guiTVR1_3";
            this.guiTVR1_3.Size = new System.Drawing.Size(107, 17);
            this.guiTVR1_3.TabIndex = 10;
            this.guiTVR1_3.Text = "ICC Data Missing";
            this.guiTVR1_3.UseVisualStyleBackColor = true;
            // 
            // guiTVR_1_2
            // 
            this.guiTVR_1_2.AutoSize = true;
            this.guiTVR_1_2.Location = new System.Drawing.Point(6, 42);
            this.guiTVR_1_2.Name = "guiTVR_1_2";
            this.guiTVR_1_2.Size = new System.Drawing.Size(76, 17);
            this.guiTVR_1_2.TabIndex = 9;
            this.guiTVR_1_2.Text = "SDA failed";
            this.guiTVR_1_2.UseVisualStyleBackColor = true;
            // 
            // guiTVR_1_1
            // 
            this.guiTVR_1_1.AutoSize = true;
            this.guiTVR_1_1.Location = new System.Drawing.Point(6, 19);
            this.guiTVR_1_1.Name = "guiTVR_1_1";
            this.guiTVR_1_1.Size = new System.Drawing.Size(221, 17);
            this.guiTVR_1_1.TabIndex = 8;
            this.guiTVR_1_1.Text = "Offline Data Authentication not performed";
            this.guiTVR_1_1.UseVisualStyleBackColor = true;
            // 
            // groupBox19
            // 
            groupBox19.AutoSize = true;
            groupBox19.Controls.Add(this.guiTVR2_7);
            groupBox19.Controls.Add(this.guiTVR2_8);
            groupBox19.Controls.Add(this.guiTVR2_6);
            groupBox19.Controls.Add(this.guiTVR2_5);
            groupBox19.Controls.Add(this.guiTVR2_4);
            groupBox19.Controls.Add(this.guiTVR2_3);
            groupBox19.Controls.Add(this.guiTVR2_2);
            groupBox19.Controls.Add(this.guiTVR2_1);
            groupBox19.Location = new System.Drawing.Point(297, 6);
            groupBox19.Name = "groupBox19";
            groupBox19.Size = new System.Drawing.Size(285, 216);
            groupBox19.TabIndex = 16;
            groupBox19.TabStop = false;
            groupBox19.Text = "TVR Byte 2";
            // 
            // guiTVR2_7
            // 
            this.guiTVR2_7.AutoSize = true;
            this.guiTVR2_7.Location = new System.Drawing.Point(7, 157);
            this.guiTVR2_7.Name = "guiTVR2_7";
            this.guiTVR2_7.Size = new System.Drawing.Size(48, 17);
            this.guiTVR2_7.TabIndex = 15;
            this.guiTVR2_7.Text = "RFU";
            this.guiTVR2_7.UseVisualStyleBackColor = true;
            // 
            // guiTVR2_8
            // 
            this.guiTVR2_8.AutoSize = true;
            this.guiTVR2_8.Location = new System.Drawing.Point(7, 180);
            this.guiTVR2_8.Name = "guiTVR2_8";
            this.guiTVR2_8.Size = new System.Drawing.Size(48, 17);
            this.guiTVR2_8.TabIndex = 14;
            this.guiTVR2_8.Text = "RFU";
            this.guiTVR2_8.UseVisualStyleBackColor = true;
            // 
            // guiTVR2_6
            // 
            this.guiTVR2_6.AutoSize = true;
            this.guiTVR2_6.Location = new System.Drawing.Point(6, 134);
            this.guiTVR2_6.Name = "guiTVR2_6";
            this.guiTVR2_6.Size = new System.Drawing.Size(48, 17);
            this.guiTVR2_6.TabIndex = 13;
            this.guiTVR2_6.Text = "RFU";
            this.guiTVR2_6.UseVisualStyleBackColor = true;
            // 
            // guiTVR2_5
            // 
            this.guiTVR2_5.AutoSize = true;
            this.guiTVR2_5.Location = new System.Drawing.Point(6, 111);
            this.guiTVR2_5.Name = "guiTVR2_5";
            this.guiTVR2_5.Size = new System.Drawing.Size(72, 17);
            this.guiTVR2_5.TabIndex = 12;
            this.guiTVR2_5.Text = "New card";
            this.guiTVR2_5.UseVisualStyleBackColor = true;
            // 
            // guiTVR2_4
            // 
            this.guiTVR2_4.AutoSize = true;
            this.guiTVR2_4.Location = new System.Drawing.Point(6, 88);
            this.guiTVR2_4.Name = "guiTVR2_4";
            this.guiTVR2_4.Size = new System.Drawing.Size(211, 17);
            this.guiTVR2_4.TabIndex = 11;
            this.guiTVR2_4.Text = "Requested service not allowed for card";
            this.guiTVR2_4.UseVisualStyleBackColor = true;
            // 
            // guiTVR2_3
            // 
            this.guiTVR2_3.AutoSize = true;
            this.guiTVR2_3.Location = new System.Drawing.Point(6, 65);
            this.guiTVR2_3.Name = "guiTVR2_3";
            this.guiTVR2_3.Size = new System.Drawing.Size(157, 17);
            this.guiTVR2_3.TabIndex = 10;
            this.guiTVR2_3.Text = "Application not yet effective";
            this.guiTVR2_3.UseVisualStyleBackColor = true;
            // 
            // guiTVR2_2
            // 
            this.guiTVR2_2.AutoSize = true;
            this.guiTVR2_2.Location = new System.Drawing.Point(6, 42);
            this.guiTVR2_2.Name = "guiTVR2_2";
            this.guiTVR2_2.Size = new System.Drawing.Size(115, 17);
            this.guiTVR2_2.TabIndex = 9;
            this.guiTVR2_2.Text = "Expired application";
            this.guiTVR2_2.UseVisualStyleBackColor = true;
            // 
            // guiTVR2_1
            // 
            this.guiTVR2_1.AutoSize = true;
            this.guiTVR2_1.Location = new System.Drawing.Point(6, 19);
            this.guiTVR2_1.Name = "guiTVR2_1";
            this.guiTVR2_1.Size = new System.Drawing.Size(213, 17);
            this.guiTVR2_1.TabIndex = 8;
            this.guiTVR2_1.Text = "ICC and terminal have different versions";
            this.guiTVR2_1.UseVisualStyleBackColor = true;
            // 
            // groupBox20
            // 
            groupBox20.AutoSize = true;
            groupBox20.Controls.Add(this.guiTVR3_7);
            groupBox20.Controls.Add(this.guiTVR3_8);
            groupBox20.Controls.Add(this.guiTVR3_6);
            groupBox20.Controls.Add(this.guiTVR3_5);
            groupBox20.Controls.Add(this.guiTVR3_4);
            groupBox20.Controls.Add(this.guiTVR3_3);
            groupBox20.Controls.Add(this.guiTVR3_2);
            groupBox20.Controls.Add(this.guiTVR3_1);
            groupBox20.Location = new System.Drawing.Point(6, 228);
            groupBox20.Name = "groupBox20";
            groupBox20.Size = new System.Drawing.Size(285, 216);
            groupBox20.TabIndex = 17;
            groupBox20.TabStop = false;
            groupBox20.Text = "TVR Byte 3";
            // 
            // guiTVR3_7
            // 
            this.guiTVR3_7.AutoSize = true;
            this.guiTVR3_7.Location = new System.Drawing.Point(7, 157);
            this.guiTVR3_7.Name = "guiTVR3_7";
            this.guiTVR3_7.Size = new System.Drawing.Size(48, 17);
            this.guiTVR3_7.TabIndex = 15;
            this.guiTVR3_7.Text = "RFU";
            this.guiTVR3_7.UseVisualStyleBackColor = true;
            // 
            // guiTVR3_8
            // 
            this.guiTVR3_8.AutoSize = true;
            this.guiTVR3_8.Location = new System.Drawing.Point(7, 180);
            this.guiTVR3_8.Name = "guiTVR3_8";
            this.guiTVR3_8.Size = new System.Drawing.Size(48, 17);
            this.guiTVR3_8.TabIndex = 14;
            this.guiTVR3_8.Text = "RFU";
            this.guiTVR3_8.UseVisualStyleBackColor = true;
            // 
            // guiTVR3_6
            // 
            this.guiTVR3_6.AutoSize = true;
            this.guiTVR3_6.Location = new System.Drawing.Point(6, 134);
            this.guiTVR3_6.Name = "guiTVR3_6";
            this.guiTVR3_6.Size = new System.Drawing.Size(116, 17);
            this.guiTVR3_6.TabIndex = 13;
            this.guiTVR3_6.Text = "Online PIN entered";
            this.guiTVR3_6.UseVisualStyleBackColor = true;
            // 
            // guiTVR3_5
            // 
            this.guiTVR3_5.AutoSize = true;
            this.guiTVR3_5.Location = new System.Drawing.Point(6, 111);
            this.guiTVR3_5.Name = "guiTVR3_5";
            this.guiTVR3_5.Size = new System.Drawing.Size(232, 17);
            this.guiTVR3_5.TabIndex = 12;
            this.guiTVR3_5.Text = "PIN entry required, but PIN was not entered";
            this.guiTVR3_5.UseVisualStyleBackColor = true;
            // 
            // guiTVR3_4
            // 
            this.guiTVR3_4.AutoSize = true;
            this.guiTVR3_4.Location = new System.Drawing.Point(6, 88);
            this.guiTVR3_4.Name = "guiTVR3_4";
            this.guiTVR3_4.Size = new System.Drawing.Size(230, 17);
            this.guiTVR3_4.TabIndex = 11;
            this.guiTVR3_4.Text = "PIN entry required and PIN pad not present";
            this.guiTVR3_4.UseVisualStyleBackColor = true;
            // 
            // guiTVR3_3
            // 
            this.guiTVR3_3.AutoSize = true;
            this.guiTVR3_3.Location = new System.Drawing.Point(6, 65);
            this.guiTVR3_3.Name = "guiTVR3_3";
            this.guiTVR3_3.Size = new System.Drawing.Size(136, 17);
            this.guiTVR3_3.TabIndex = 10;
            this.guiTVR3_3.Text = "PIN Try Limit exceeded";
            this.guiTVR3_3.UseVisualStyleBackColor = true;
            // 
            // guiTVR3_2
            // 
            this.guiTVR3_2.AutoSize = true;
            this.guiTVR3_2.Location = new System.Drawing.Point(6, 42);
            this.guiTVR3_2.Name = "guiTVR3_2";
            this.guiTVR3_2.Size = new System.Drawing.Size(118, 17);
            this.guiTVR3_2.TabIndex = 9;
            this.guiTVR3_2.Text = "Unrecognised CVM";
            this.guiTVR3_2.UseVisualStyleBackColor = true;
            // 
            // guiTVR3_1
            // 
            this.guiTVR3_1.AutoSize = true;
            this.guiTVR3_1.Location = new System.Drawing.Point(6, 19);
            this.guiTVR3_1.Name = "guiTVR3_1";
            this.guiTVR3_1.Size = new System.Drawing.Size(224, 17);
            this.guiTVR3_1.TabIndex = 8;
            this.guiTVR3_1.Text = "Cardholder verification was not successful";
            this.guiTVR3_1.UseVisualStyleBackColor = true;
            // 
            // groupBox21
            // 
            groupBox21.AutoSize = true;
            groupBox21.Controls.Add(this.guiTVR4_7);
            groupBox21.Controls.Add(this.guiTVR4_8);
            groupBox21.Controls.Add(this.guiTVR4_6);
            groupBox21.Controls.Add(this.guiTVR4_5);
            groupBox21.Controls.Add(this.guiTVR4_4);
            groupBox21.Controls.Add(this.guiTVR4_3);
            groupBox21.Controls.Add(this.guiTVR4_2);
            groupBox21.Controls.Add(this.guiTVR4_1);
            groupBox21.Location = new System.Drawing.Point(297, 228);
            groupBox21.Name = "groupBox21";
            groupBox21.Size = new System.Drawing.Size(285, 216);
            groupBox21.TabIndex = 18;
            groupBox21.TabStop = false;
            groupBox21.Text = "TVR Byte 4";
            // 
            // guiTVR4_7
            // 
            this.guiTVR4_7.AutoSize = true;
            this.guiTVR4_7.Location = new System.Drawing.Point(7, 157);
            this.guiTVR4_7.Name = "guiTVR4_7";
            this.guiTVR4_7.Size = new System.Drawing.Size(48, 17);
            this.guiTVR4_7.TabIndex = 15;
            this.guiTVR4_7.Text = "RFU";
            this.guiTVR4_7.UseVisualStyleBackColor = true;
            // 
            // guiTVR4_8
            // 
            this.guiTVR4_8.AutoSize = true;
            this.guiTVR4_8.Location = new System.Drawing.Point(7, 180);
            this.guiTVR4_8.Name = "guiTVR4_8";
            this.guiTVR4_8.Size = new System.Drawing.Size(48, 17);
            this.guiTVR4_8.TabIndex = 14;
            this.guiTVR4_8.Text = "RFU";
            this.guiTVR4_8.UseVisualStyleBackColor = true;
            // 
            // guiTVR4_6
            // 
            this.guiTVR4_6.AutoSize = true;
            this.guiTVR4_6.Location = new System.Drawing.Point(6, 134);
            this.guiTVR4_6.Name = "guiTVR4_6";
            this.guiTVR4_6.Size = new System.Drawing.Size(48, 17);
            this.guiTVR4_6.TabIndex = 13;
            this.guiTVR4_6.Text = "RFU";
            this.guiTVR4_6.UseVisualStyleBackColor = true;
            // 
            // guiTVR4_5
            // 
            this.guiTVR4_5.AutoSize = true;
            this.guiTVR4_5.Location = new System.Drawing.Point(6, 111);
            this.guiTVR4_5.Name = "guiTVR4_5";
            this.guiTVR4_5.Size = new System.Drawing.Size(190, 17);
            this.guiTVR4_5.TabIndex = 12;
            this.guiTVR4_5.Text = "Merchant forced transaction online";
            this.guiTVR4_5.UseVisualStyleBackColor = true;
            // 
            // guiTVR4_4
            // 
            this.guiTVR4_4.AutoSize = true;
            this.guiTVR4_4.Location = new System.Drawing.Point(6, 88);
            this.guiTVR4_4.Name = "guiTVR4_4";
            this.guiTVR4_4.Size = new System.Drawing.Size(270, 17);
            this.guiTVR4_4.TabIndex = 11;
            this.guiTVR4_4.Text = "Transaction selected randomly for online processing";
            this.guiTVR4_4.UseVisualStyleBackColor = true;
            // 
            // guiTVR4_3
            // 
            this.guiTVR4_3.AutoSize = true;
            this.guiTVR4_3.Location = new System.Drawing.Point(6, 65);
            this.guiTVR4_3.Name = "guiTVR4_3";
            this.guiTVR4_3.Size = new System.Drawing.Size(217, 17);
            this.guiTVR4_3.TabIndex = 10;
            this.guiTVR4_3.Text = "Upper consecutive offline limit exceeded";
            this.guiTVR4_3.UseVisualStyleBackColor = true;
            // 
            // guiTVR4_2
            // 
            this.guiTVR4_2.AutoSize = true;
            this.guiTVR4_2.Location = new System.Drawing.Point(6, 42);
            this.guiTVR4_2.Name = "guiTVR4_2";
            this.guiTVR4_2.Size = new System.Drawing.Size(217, 17);
            this.guiTVR4_2.TabIndex = 9;
            this.guiTVR4_2.Text = "Lower consecutive offline limit exceeded";
            this.guiTVR4_2.UseVisualStyleBackColor = true;
            // 
            // guiTVR4_1
            // 
            this.guiTVR4_1.AutoSize = true;
            this.guiTVR4_1.Location = new System.Drawing.Point(6, 19);
            this.guiTVR4_1.Name = "guiTVR4_1";
            this.guiTVR4_1.Size = new System.Drawing.Size(163, 17);
            this.guiTVR4_1.TabIndex = 8;
            this.guiTVR4_1.Text = "Transaction exceed floor limit";
            this.guiTVR4_1.UseVisualStyleBackColor = true;
            // 
            // groupBox22
            // 
            groupBox22.AutoSize = true;
            groupBox22.Controls.Add(this.guiTVR5_7);
            groupBox22.Controls.Add(this.guiTVR5_8);
            groupBox22.Controls.Add(this.guiTVR5_6);
            groupBox22.Controls.Add(this.guiTVR5_5);
            groupBox22.Controls.Add(this.guiTVR5_4);
            groupBox22.Controls.Add(this.guiTVR5_3);
            groupBox22.Controls.Add(this.guiTVR5_2);
            groupBox22.Controls.Add(this.guiTVR5_1);
            groupBox22.Location = new System.Drawing.Point(6, 450);
            groupBox22.Name = "groupBox22";
            groupBox22.Size = new System.Drawing.Size(285, 216);
            groupBox22.TabIndex = 18;
            groupBox22.TabStop = false;
            groupBox22.Text = "TVR Byte 5";
            // 
            // guiTVR5_7
            // 
            this.guiTVR5_7.AutoSize = true;
            this.guiTVR5_7.Location = new System.Drawing.Point(7, 157);
            this.guiTVR5_7.Name = "guiTVR5_7";
            this.guiTVR5_7.Size = new System.Drawing.Size(48, 17);
            this.guiTVR5_7.TabIndex = 15;
            this.guiTVR5_7.Text = "RFU";
            this.guiTVR5_7.UseVisualStyleBackColor = true;
            // 
            // guiTVR5_8
            // 
            this.guiTVR5_8.AutoSize = true;
            this.guiTVR5_8.Location = new System.Drawing.Point(7, 180);
            this.guiTVR5_8.Name = "guiTVR5_8";
            this.guiTVR5_8.Size = new System.Drawing.Size(48, 17);
            this.guiTVR5_8.TabIndex = 14;
            this.guiTVR5_8.Text = "RFU";
            this.guiTVR5_8.UseVisualStyleBackColor = true;
            // 
            // guiTVR5_6
            // 
            this.guiTVR5_6.AutoSize = true;
            this.guiTVR5_6.Location = new System.Drawing.Point(6, 134);
            this.guiTVR5_6.Name = "guiTVR5_6";
            this.guiTVR5_6.Size = new System.Drawing.Size(48, 17);
            this.guiTVR5_6.TabIndex = 13;
            this.guiTVR5_6.Text = "RFU";
            this.guiTVR5_6.UseVisualStyleBackColor = true;
            // 
            // guiTVR5_5
            // 
            this.guiTVR5_5.AutoSize = true;
            this.guiTVR5_5.Location = new System.Drawing.Point(6, 111);
            this.guiTVR5_5.Name = "guiTVR5_5";
            this.guiTVR5_5.Size = new System.Drawing.Size(48, 17);
            this.guiTVR5_5.TabIndex = 12;
            this.guiTVR5_5.Text = "RFU";
            this.guiTVR5_5.UseVisualStyleBackColor = true;
            // 
            // guiTVR5_4
            // 
            this.guiTVR5_4.AutoSize = true;
            this.guiTVR5_4.Location = new System.Drawing.Point(6, 88);
            this.guiTVR5_4.Name = "guiTVR5_4";
            this.guiTVR5_4.Size = new System.Drawing.Size(260, 17);
            this.guiTVR5_4.TabIndex = 11;
            this.guiTVR5_4.Text = "Script processing failed after final GENERATE AC";
            this.guiTVR5_4.UseVisualStyleBackColor = true;
            // 
            // guiTVR5_3
            // 
            this.guiTVR5_3.AutoSize = true;
            this.guiTVR5_3.Location = new System.Drawing.Point(6, 65);
            this.guiTVR5_3.Name = "guiTVR5_3";
            this.guiTVR5_3.Size = new System.Drawing.Size(269, 17);
            this.guiTVR5_3.TabIndex = 10;
            this.guiTVR5_3.Text = "Script processing failed before final GENERATE AC";
            this.guiTVR5_3.UseVisualStyleBackColor = true;
            // 
            // guiTVR5_2
            // 
            this.guiTVR5_2.AutoSize = true;
            this.guiTVR5_2.Location = new System.Drawing.Point(6, 42);
            this.guiTVR5_2.Name = "guiTVR5_2";
            this.guiTVR5_2.Size = new System.Drawing.Size(152, 17);
            this.guiTVR5_2.TabIndex = 9;
            this.guiTVR5_2.Text = "Issuer authentication failed";
            this.guiTVR5_2.UseVisualStyleBackColor = true;
            // 
            // guiTVR5_1
            // 
            this.guiTVR5_1.AutoSize = true;
            this.guiTVR5_1.Location = new System.Drawing.Point(6, 19);
            this.guiTVR5_1.Name = "guiTVR5_1";
            this.guiTVR5_1.Size = new System.Drawing.Size(118, 17);
            this.guiTVR5_1.TabIndex = 8;
            this.guiTVR5_1.Text = "Default TDOL used";
            this.guiTVR5_1.UseVisualStyleBackColor = true;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMain.Controls.Add(guiTabEMVApplications);
            this.tabControlMain.Controls.Add(guiTabPSE);
            this.tabControlMain.Controls.Add(guiTabPublicKeys);
            this.tabControlMain.Controls.Add(guiTabAuthentication);
            this.tabControlMain.Controls.Add(this.tabTVR);
            this.tabControlMain.Controls.Add(guiTabEMVCardLog);
            this.tabControlMain.Controls.Add(guiTabDetailedLogs);
            this.tabControlMain.Controls.Add(guiTabParameters);
            this.tabControlMain.Location = new System.Drawing.Point(318, 12);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(690, 725);
            this.tabControlMain.TabIndex = 8;
            // 
            // tabTVR
            // 
            this.tabTVR.Controls.Add(groupBox22);
            this.tabTVR.Controls.Add(groupBox21);
            this.tabTVR.Controls.Add(groupBox20);
            this.tabTVR.Controls.Add(groupBox19);
            this.tabTVR.Controls.Add(groupBox17);
            this.tabTVR.Location = new System.Drawing.Point(4, 22);
            this.tabTVR.Name = "tabTVR";
            this.tabTVR.Padding = new System.Windows.Forms.Padding(3);
            this.tabTVR.Size = new System.Drawing.Size(682, 699);
            this.tabTVR.TabIndex = 7;
            this.tabTVR.Text = "TVR live";
            this.tabTVR.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 740);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1008, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 762);
            this.Controls.Add(groupBox13);
            this.Controls.Add(groupBox12);
            this.Controls.Add(groupBox11);
            this.Controls.Add(groupBox10);
            this.Controls.Add(groupBox4);
            this.Controls.Add(groupBox3);
            this.Controls.Add(groupBox2);
            this.Controls.Add(groupBox1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControlMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GUI";
            this.Text = "EMV Explorer";
            guiTabParameters.ResumeLayout(false);
            guiTabParameters.PerformLayout();
            groupBox9.ResumeLayout(false);
            groupBox9.PerformLayout();
            groupBox8.ResumeLayout(false);
            groupBox8.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox10.ResumeLayout(false);
            groupBox10.PerformLayout();
            groupBox11.ResumeLayout(false);
            groupBox11.PerformLayout();
            groupBox12.ResumeLayout(false);
            groupBox13.ResumeLayout(false);
            guiTabDetailedLogs.ResumeLayout(false);
            guiTabPSE.ResumeLayout(false);
            guiTabEMVApplications.ResumeLayout(false);
            guiTabEMVCardLog.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            guiTabPublicKeys.ResumeLayout(false);
            groupBox18.ResumeLayout(false);
            groupBox18.PerformLayout();
            groupBox16.ResumeLayout(false);
            groupBox16.PerformLayout();
            groupBox14.ResumeLayout(false);
            groupBox14.PerformLayout();
            guiTabAuthentication.ResumeLayout(false);
            guiAuthentication.ResumeLayout(false);
            guiSDA.ResumeLayout(false);
            guiSDA.PerformLayout();
            this.guiSDAAuthenticationData.ResumeLayout(false);
            this.guiSDAAuthenticationData.PerformLayout();
            guiDDA.ResumeLayout(false);
            guiDDA.PerformLayout();
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            guiCDA.ResumeLayout(false);
            guiCDA.PerformLayout();
            groupBox17.ResumeLayout(false);
            groupBox17.PerformLayout();
            groupBox19.ResumeLayout(false);
            groupBox19.PerformLayout();
            groupBox20.ResumeLayout(false);
            groupBox20.PerformLayout();
            groupBox21.ResumeLayout(false);
            groupBox21.PerformLayout();
            groupBox22.ResumeLayout(false);
            groupBox22.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabTVR.ResumeLayout(false);
            this.tabTVR.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RichTextBox guiDetailedLogs;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ComboBox guiPSEName;
        private System.Windows.Forms.Button guiDoSelectPSE;
        private System.Windows.Forms.ComboBox guiApplicationAID;
        private System.Windows.Forms.Button guiDoSelectAID;
        private System.Windows.Forms.Button guiDoGetData;
        private System.Windows.Forms.Button guiDoReadRecords;
        private System.Windows.Forms.Button guiDoGetProcessingOptions;
        private System.Windows.Forms.Button guiDoExplicitDiscoveryOfAID;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button guiDoCardLogRead;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox guiCardLogFormat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox guiCardLogLength;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox guiCardLogSFI;
        private System.Windows.Forms.CheckBox guiLogFilePresence;
        public System.Windows.Forms.ListView guiLogRecords;
        public System.Windows.Forms.TreeView guiPSEContent;
        public System.Windows.Forms.TreeView guiEMVApplicationsContent;
        private System.Windows.Forms.Button guiDoCardLogSave;
        private System.Windows.Forms.Button guiDoSaveDetailedLogs;
        private System.Windows.Forms.CheckBox guiParamsTagAIDInFCI;
        private System.Windows.Forms.ComboBox guiCVMList;
        private System.Windows.Forms.Button guiDoVerifyCardholder;
        private System.Windows.Forms.TextBox guiPINEntry;
        private System.Windows.Forms.CheckBox guiPINEntryUsed;
        private System.Windows.Forms.Button guiDoInternalAuthenticate;
        private System.Windows.Forms.Button guiDoGenerateAC2;
        private System.Windows.Forms.ComboBox guiAC2Type;
        private System.Windows.Forms.ComboBox guiAC1Type;
        private System.Windows.Forms.Button guiDoGenerateAC1;
        private System.Windows.Forms.Button guiDoExternalAuthenticate;
        private System.Windows.Forms.TextBox guiInternalAuthenticateUnpredictableNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button guiDoGetChallenge;
        private System.Windows.Forms.TextBox guiPublicKeysAID;
        private System.Windows.Forms.TextBox guiPublicKeysRID;
        private System.Windows.Forms.TextBox guiPublicKeysCAIndex;
        private System.Windows.Forms.TextBox guiPublicKeysIssuerPKRecoveredData;
        private System.Windows.Forms.TextBox guiPublicKeysIssuerPKHashResult;
        private System.Windows.Forms.TextBox guiPublicKeysCertificationAuthorityPKModulus;
        private System.Windows.Forms.TextBox guiPublicKeysCertificationAuthorityPKExponent;
        private System.Windows.Forms.TextBox guiPublicKeysICCPKHashResult;
        private System.Windows.Forms.TextBox guiPublicKeysICCPKRecoveredData;
        private System.Windows.Forms.TextBox guiPublicKeysIssuerPKModulus;
        private System.Windows.Forms.TextBox guiPublicKeysIssuerPKExponent;
        private System.Windows.Forms.TextBox guiPublicKeysICCPKModulus;
        private System.Windows.Forms.TextBox guiPublicKeysICCPKExponent;
        private System.Windows.Forms.ListView guiSDASignedData;
        private System.Windows.Forms.GroupBox guiSDAAuthenticationData;
        private System.Windows.Forms.TextBox guiSDARecoveredData;
        private System.Windows.Forms.TextBox guiSDAHashResult;
        private System.Windows.Forms.TextBox guiSDADataAuthenticationCode;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.TextBox guiDDAICCDynamicData;
        private System.Windows.Forms.TextBox guiDDAHashResult;
        private System.Windows.Forms.TextBox guiDDARecoveredData;
        private System.Windows.Forms.ColumnHeader guiSDASignedDataColumnId;
        private System.Windows.Forms.ColumnHeader guiSDASignedDataTag;
        private System.Windows.Forms.ListView guiDDASignedData;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ListView guiCDASignedData;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox guiAC1UnpredictableNumber;
        private System.Windows.Forms.TabPage tabTVR;
        private System.Windows.Forms.CheckBox guiTVR2_7;
        private System.Windows.Forms.CheckBox guiTVR2_8;
        private System.Windows.Forms.CheckBox guiTVR2_6;
        private System.Windows.Forms.CheckBox guiTVR2_5;
        private System.Windows.Forms.CheckBox guiTVR2_4;
        private System.Windows.Forms.CheckBox guiTVR2_3;
        private System.Windows.Forms.CheckBox guiTVR2_2;
        private System.Windows.Forms.CheckBox guiTVR2_1;
        private System.Windows.Forms.CheckBox guiTVR1_7;
        private System.Windows.Forms.CheckBox guiTVR1_8;
        private System.Windows.Forms.CheckBox guiTVR1_6;
        private System.Windows.Forms.CheckBox guiTVR1_5;
        private System.Windows.Forms.CheckBox guiTVR1_4;
        private System.Windows.Forms.CheckBox guiTVR1_3;
        private System.Windows.Forms.CheckBox guiTVR_1_2;
        private System.Windows.Forms.CheckBox guiTVR_1_1;
        private System.Windows.Forms.CheckBox guiTVR4_7;
        private System.Windows.Forms.CheckBox guiTVR4_8;
        private System.Windows.Forms.CheckBox guiTVR4_6;
        private System.Windows.Forms.CheckBox guiTVR4_5;
        private System.Windows.Forms.CheckBox guiTVR4_4;
        private System.Windows.Forms.CheckBox guiTVR4_3;
        private System.Windows.Forms.CheckBox guiTVR4_2;
        private System.Windows.Forms.CheckBox guiTVR4_1;
        private System.Windows.Forms.CheckBox guiTVR3_7;
        private System.Windows.Forms.CheckBox guiTVR3_8;
        private System.Windows.Forms.CheckBox guiTVR3_6;
        private System.Windows.Forms.CheckBox guiTVR3_5;
        private System.Windows.Forms.CheckBox guiTVR3_4;
        private System.Windows.Forms.CheckBox guiTVR3_3;
        private System.Windows.Forms.CheckBox guiTVR3_2;
        private System.Windows.Forms.CheckBox guiTVR3_1;
        private System.Windows.Forms.CheckBox guiTVR5_7;
        private System.Windows.Forms.CheckBox guiTVR5_8;
        private System.Windows.Forms.CheckBox guiTVR5_6;
        private System.Windows.Forms.CheckBox guiTVR5_5;
        private System.Windows.Forms.CheckBox guiTVR5_4;
        private System.Windows.Forms.CheckBox guiTVR5_3;
        private System.Windows.Forms.CheckBox guiTVR5_2;
        private System.Windows.Forms.CheckBox guiTVR5_1;

    }
}

