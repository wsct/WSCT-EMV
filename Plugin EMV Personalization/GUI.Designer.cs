namespace WSCT.GUI.Plugins.EMV.Personalization
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
            this.guiDoSelectEmvPersonalizationFolder = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.guiAppletPin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.guiEMVAppletAID = new System.Windows.Forms.TextBox();
            this.guiDoRunEmvPersonalization = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.guiEmvFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.guiLogs = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.guiPSEAppletName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.guiDoSelectPsePersonalizationFolder = new System.Windows.Forms.Button();
            this.guiDoRunPsePersonalization = new System.Windows.Forms.Button();
            this.guiPseFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // guiDoSelectEmvPersonalizationFolder
            // 
            this.guiDoSelectEmvPersonalizationFolder.Location = new System.Drawing.Point(8, 29);
            this.guiDoSelectEmvPersonalizationFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guiDoSelectEmvPersonalizationFolder.Name = "guiDoSelectEmvPersonalizationFolder";
            this.guiDoSelectEmvPersonalizationFolder.Size = new System.Drawing.Size(432, 35);
            this.guiDoSelectEmvPersonalizationFolder.TabIndex = 8;
            this.guiDoSelectEmvPersonalizationFolder.Text = "Select EMV Personalization Folder";
            this.guiDoSelectEmvPersonalizationFolder.UseVisualStyleBackColor = true;
            this.guiDoSelectEmvPersonalizationFolder.Click += new System.EventHandler(this.guiDoSelectEmvPersonalizationFolder_ClickAsync);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Location = new System.Drawing.Point(0, 722);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip.Size = new System.Drawing.Size(978, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.guiAppletPin);
            this.groupBox2.Controls.Add(this.guiDoSelectEmvPersonalizationFolder);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.guiEMVAppletAID);
            this.groupBox2.Controls.Add(this.guiDoRunEmvPersonalization);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(13, 14);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(470, 202);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "EMV Applet Personalization";
            // 
            // guiAppletPin
            // 
            this.guiAppletPin.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guiAppletPin.Location = new System.Drawing.Point(54, 104);
            this.guiAppletPin.Name = "guiAppletPin";
            this.guiAppletPin.Size = new System.Drawing.Size(386, 26);
            this.guiAppletPin.TabIndex = 12;
            this.guiAppletPin.Text = "1234";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "PIN:";
            // 
            // guiEMVAppletAID
            // 
            this.guiEMVAppletAID.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guiEMVAppletAID.Location = new System.Drawing.Point(54, 72);
            this.guiEMVAppletAID.Name = "guiEMVAppletAID";
            this.guiEMVAppletAID.Size = new System.Drawing.Size(386, 26);
            this.guiEMVAppletAID.TabIndex = 10;
            this.guiEMVAppletAID.Text = "F0 43 41 45 4E 42 01";
            // 
            // guiDoRunEmvPersonalization
            // 
            this.guiDoRunEmvPersonalization.Location = new System.Drawing.Point(11, 138);
            this.guiDoRunEmvPersonalization.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guiDoRunEmvPersonalization.Name = "guiDoRunEmvPersonalization";
            this.guiDoRunEmvPersonalization.Size = new System.Drawing.Size(429, 35);
            this.guiDoRunEmvPersonalization.TabIndex = 7;
            this.guiDoRunEmvPersonalization.Text = "Run EMV Personalization";
            this.guiDoRunEmvPersonalization.UseVisualStyleBackColor = true;
            this.guiDoRunEmvPersonalization.Click += new System.EventHandler(this.guiDoRunEmvPersonalization_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "AID:";
            // 
            // guiLogs
            // 
            this.guiLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.guiLogs.DetectUrls = false;
            this.guiLogs.Location = new System.Drawing.Point(13, 224);
            this.guiLogs.Name = "guiLogs";
            this.guiLogs.ReadOnly = true;
            this.guiLogs.Size = new System.Drawing.Size(953, 495);
            this.guiLogs.TabIndex = 11;
            this.guiLogs.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.guiPSEAppletName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.guiDoSelectPsePersonalizationFolder);
            this.groupBox1.Controls.Add(this.guiDoRunPsePersonalization);
            this.groupBox1.Location = new System.Drawing.Point(491, 14);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(452, 202);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PSE Applet Personalization";
            // 
            // guiPSEAppletName
            // 
            this.guiPSEAppletName.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guiPSEAppletName.Location = new System.Drawing.Point(104, 72);
            this.guiPSEAppletName.Name = "guiPSEAppletName";
            this.guiPSEAppletName.Size = new System.Drawing.Size(336, 26);
            this.guiPSEAppletName.TabIndex = 10;
            this.guiPSEAppletName.Text = "1PAY.SYS.DDF01";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "PSE Name:";
            // 
            // guiDoSelectPsePersonalizationFolder
            // 
            this.guiDoSelectPsePersonalizationFolder.Location = new System.Drawing.Point(8, 29);
            this.guiDoSelectPsePersonalizationFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guiDoSelectPsePersonalizationFolder.Name = "guiDoSelectPsePersonalizationFolder";
            this.guiDoSelectPsePersonalizationFolder.Size = new System.Drawing.Size(432, 35);
            this.guiDoSelectPsePersonalizationFolder.TabIndex = 8;
            this.guiDoSelectPsePersonalizationFolder.Text = "Select PSE Personalization Folder";
            this.guiDoSelectPsePersonalizationFolder.UseVisualStyleBackColor = true;
            this.guiDoSelectPsePersonalizationFolder.Click += new System.EventHandler(this.guiDoSelectPsePersonalizationFolder_Click);
            // 
            // guiDoRunPsePersonalization
            // 
            this.guiDoRunPsePersonalization.Location = new System.Drawing.Point(8, 138);
            this.guiDoRunPsePersonalization.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guiDoRunPsePersonalization.Name = "guiDoRunPsePersonalization";
            this.guiDoRunPsePersonalization.Size = new System.Drawing.Size(429, 35);
            this.guiDoRunPsePersonalization.TabIndex = 7;
            this.guiDoRunPsePersonalization.Text = "Run PSE Personalization";
            this.guiDoRunPsePersonalization.UseVisualStyleBackColor = true;
            this.guiDoRunPsePersonalization.Click += new System.EventHandler(this.guiDoRunPsePersonalization_Click);
            // 
            // Gui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 744);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.guiLogs);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.statusStrip);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Gui";
            this.Text = "EMV Card Personalization";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button guiDoSelectEmvPersonalizationFolder;
        private System.Windows.Forms.Button guiDoRunEmvPersonalization;
        private System.Windows.Forms.FolderBrowserDialog guiEmvFolderBrowserDialog;
        private System.Windows.Forms.RichTextBox guiLogs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox guiEMVAppletAID;
        private System.Windows.Forms.TextBox guiAppletPin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox guiPSEAppletName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button guiDoSelectPsePersonalizationFolder;
        private System.Windows.Forms.Button guiDoRunPsePersonalization;
        private System.Windows.Forms.FolderBrowserDialog guiPseFolderBrowserDialog;
    }
}

