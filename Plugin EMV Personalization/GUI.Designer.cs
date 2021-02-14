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
            this.guiDoSelectPersonalizationFolder = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.guiDoRunPersonalization = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.guiEMVAppletAID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.guiFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.guiLogs = new System.Windows.Forms.RichTextBox();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // guiDoSelectPersonalizationFolder
            // 
            this.guiDoSelectPersonalizationFolder.Location = new System.Drawing.Point(8, 29);
            this.guiDoSelectPersonalizationFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guiDoSelectPersonalizationFolder.Name = "guiDoSelectPersonalizationFolder";
            this.guiDoSelectPersonalizationFolder.Size = new System.Drawing.Size(432, 35);
            this.guiDoSelectPersonalizationFolder.TabIndex = 8;
            this.guiDoSelectPersonalizationFolder.Text = "Select Personalization Folder";
            this.guiDoSelectPersonalizationFolder.UseVisualStyleBackColor = true;
            this.guiDoSelectPersonalizationFolder.Click += new System.EventHandler(this.guiDoSelectPersonalizationFolder_ClickAsync);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Location = new System.Drawing.Point(0, 522);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip.Size = new System.Drawing.Size(478, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.guiDoSelectPersonalizationFolder);
            this.groupBox2.Location = new System.Drawing.Point(13, 14);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(452, 93);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Personalization Data";
            // 
            // guiDoRunPersonalization
            // 
            this.guiDoRunPersonalization.Location = new System.Drawing.Point(252, 29);
            this.guiDoRunPersonalization.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guiDoRunPersonalization.Name = "guiDoRunPersonalization";
            this.guiDoRunPersonalization.Size = new System.Drawing.Size(188, 35);
            this.guiDoRunPersonalization.TabIndex = 7;
            this.guiDoRunPersonalization.Text = "Run Personalization";
            this.guiDoRunPersonalization.UseVisualStyleBackColor = true;
            this.guiDoRunPersonalization.Click += new System.EventHandler(this.guiDoRunPersonalization_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.AutoSize = true;
            this.groupBox4.Controls.Add(this.guiEMVAppletAID);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.guiDoRunPersonalization);
            this.groupBox4.Location = new System.Drawing.Point(13, 117);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Size = new System.Drawing.Size(452, 94);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "EMV Applet Personalization";
            // 
            // guiEMVAppletAID
            // 
            this.guiEMVAppletAID.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guiEMVAppletAID.Location = new System.Drawing.Point(54, 33);
            this.guiEMVAppletAID.Name = "guiEMVAppletAID";
            this.guiEMVAppletAID.Size = new System.Drawing.Size(191, 26);
            this.guiEMVAppletAID.TabIndex = 10;
            this.guiEMVAppletAID.Text = "F0 43 41 45 4E 42 01";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 36);
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
            this.guiLogs.Location = new System.Drawing.Point(13, 219);
            this.guiLogs.Name = "guiLogs";
            this.guiLogs.ReadOnly = true;
            this.guiLogs.Size = new System.Drawing.Size(452, 300);
            this.guiLogs.TabIndex = 11;
            this.guiLogs.Text = "";
            // 
            // Gui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 544);
            this.Controls.Add(this.guiLogs);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.statusStrip);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Gui";
            this.Text = "EMV Card Personalization";
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button guiDoSelectPersonalizationFolder;
        private System.Windows.Forms.Button guiDoRunPersonalization;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.FolderBrowserDialog guiFolderBrowserDialog;
        private System.Windows.Forms.RichTextBox guiLogs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox guiEMVAppletAID;
    }
}

