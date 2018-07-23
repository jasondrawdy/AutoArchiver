namespace AutoArchiver
{
    partial class frmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.chkAskBeforeBackup = new AutoArchiver.SSCheckbox();
            this.chkStartArchivingOnStartup = new AutoArchiver.SSCheckbox();
            this.chkAddToStartup = new AutoArchiver.SSCheckbox();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.sep1 = new MaterialSkin.Controls.MaterialDivider();
            this.cmbChecksumAlgorithm = new System.Windows.Forms.ComboBox();
            this.lblChecksumAlgorithm = new System.Windows.Forms.Label();
            this.cmbCompressionLevel = new System.Windows.Forms.ComboBox();
            this.lblCompressionLevel = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblGeneralSettings = new MonoFlat.MonoFlat_HeaderLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.grpSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkAskBeforeBackup
            // 
            this.chkAskBeforeBackup.AutoSize = true;
            this.chkAskBeforeBackup.Location = new System.Drawing.Point(274, 120);
            this.chkAskBeforeBackup.Name = "chkAskBeforeBackup";
            this.chkAskBeforeBackup.Size = new System.Drawing.Size(118, 17);
            this.chkAskBeforeBackup.TabIndex = 0;
            this.chkAskBeforeBackup.Text = "Ask Before Backup";
            this.chkAskBeforeBackup.ToolTipText = "Ask for permission before creating a backup";
            this.chkAskBeforeBackup.UseVisualStyleBackColor = true;
            // 
            // chkStartArchivingOnStartup
            // 
            this.chkStartArchivingOnStartup.AutoSize = true;
            this.chkStartArchivingOnStartup.Location = new System.Drawing.Point(119, 120);
            this.chkStartArchivingOnStartup.Name = "chkStartArchivingOnStartup";
            this.chkStartArchivingOnStartup.Size = new System.Drawing.Size(149, 17);
            this.chkStartArchivingOnStartup.TabIndex = 1;
            this.chkStartArchivingOnStartup.Text = "Start Archiving On Startup";
            this.chkStartArchivingOnStartup.ToolTipText = "Begin creating backups as soon as Auto Archiver starts";
            this.chkStartArchivingOnStartup.UseVisualStyleBackColor = true;
            // 
            // chkAddToStartup
            // 
            this.chkAddToStartup.AutoSize = true;
            this.chkAddToStartup.Location = new System.Drawing.Point(15, 120);
            this.chkAddToStartup.Name = "chkAddToStartup";
            this.chkAddToStartup.Size = new System.Drawing.Size(98, 17);
            this.chkAddToStartup.TabIndex = 2;
            this.chkAddToStartup.Text = "Add To Startup";
            this.chkAddToStartup.ToolTipText = "Allow Auto Archiver to start when Windows starts";
            this.chkAddToStartup.UseVisualStyleBackColor = true;
            // 
            // grpSettings
            // 
            this.grpSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSettings.Controls.Add(this.sep1);
            this.grpSettings.Controls.Add(this.cmbChecksumAlgorithm);
            this.grpSettings.Controls.Add(this.lblChecksumAlgorithm);
            this.grpSettings.Controls.Add(this.cmbCompressionLevel);
            this.grpSettings.Controls.Add(this.lblCompressionLevel);
            this.grpSettings.Controls.Add(this.chkAskBeforeBackup);
            this.grpSettings.Controls.Add(this.chkAddToStartup);
            this.grpSettings.Controls.Add(this.chkStartArchivingOnStartup);
            this.grpSettings.Location = new System.Drawing.Point(12, 41);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(424, 156);
            this.grpSettings.TabIndex = 3;
            this.grpSettings.TabStop = false;
            // 
            // sep1
            // 
            this.sep1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sep1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.sep1.Depth = 0;
            this.sep1.Location = new System.Drawing.Point(15, 113);
            this.sep1.MouseState = MaterialSkin.MouseState.HOVER;
            this.sep1.Name = "sep1";
            this.sep1.Size = new System.Drawing.Size(395, 1);
            this.sep1.TabIndex = 7;
            this.sep1.Text = "materialDivider1";
            // 
            // cmbChecksumAlgorithm
            // 
            this.cmbChecksumAlgorithm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbChecksumAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChecksumAlgorithm.FormattingEnabled = true;
            this.cmbChecksumAlgorithm.Items.AddRange(new object[] {
            "MD5",
            "RIPEMD160",
            "SHA1",
            "SHA256",
            "SHA384",
            "SHA512"});
            this.cmbChecksumAlgorithm.Location = new System.Drawing.Point(15, 81);
            this.cmbChecksumAlgorithm.Name = "cmbChecksumAlgorithm";
            this.cmbChecksumAlgorithm.Size = new System.Drawing.Size(395, 21);
            this.cmbChecksumAlgorithm.TabIndex = 6;
            // 
            // lblChecksumAlgorithm
            // 
            this.lblChecksumAlgorithm.AutoSize = true;
            this.lblChecksumAlgorithm.Location = new System.Drawing.Point(12, 65);
            this.lblChecksumAlgorithm.Name = "lblChecksumAlgorithm";
            this.lblChecksumAlgorithm.Size = new System.Drawing.Size(106, 13);
            this.lblChecksumAlgorithm.TabIndex = 5;
            this.lblChecksumAlgorithm.Text = "Checksum Algorithm:";
            // 
            // cmbCompressionLevel
            // 
            this.cmbCompressionLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCompressionLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCompressionLevel.FormattingEnabled = true;
            this.cmbCompressionLevel.Items.AddRange(new object[] {
            "High (Extreme Compression)",
            "Medium (Moderate Compression)",
            "Low (Minimal Compression)",
            "Store (No Compression)"});
            this.cmbCompressionLevel.Location = new System.Drawing.Point(15, 41);
            this.cmbCompressionLevel.Name = "cmbCompressionLevel";
            this.cmbCompressionLevel.Size = new System.Drawing.Size(395, 21);
            this.cmbCompressionLevel.TabIndex = 4;
            // 
            // lblCompressionLevel
            // 
            this.lblCompressionLevel.AutoSize = true;
            this.lblCompressionLevel.Location = new System.Drawing.Point(12, 25);
            this.lblCompressionLevel.Name = "lblCompressionLevel";
            this.lblCompressionLevel.Size = new System.Drawing.Size(99, 13);
            this.lblCompressionLevel.TabIndex = 3;
            this.lblCompressionLevel.Text = "Compression Level:";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(324, 212);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(112, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(243, 212);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblGeneralSettings
            // 
            this.lblGeneralSettings.AutoSize = true;
            this.lblGeneralSettings.BackColor = System.Drawing.Color.Transparent;
            this.lblGeneralSettings.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblGeneralSettings.ForeColor = System.Drawing.Color.Black;
            this.lblGeneralSettings.Location = new System.Drawing.Point(8, 18);
            this.lblGeneralSettings.Name = "lblGeneralSettings";
            this.lblGeneralSettings.Size = new System.Drawing.Size(124, 20);
            this.lblGeneralSettings.TabIndex = 9;
            this.lblGeneralSettings.Text = "General Settings";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Compression Level:";
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(448, 247);
            this.Controls.Add(this.lblGeneralSettings);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auto Archiver - Version 1.0.0";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SSCheckbox chkAskBeforeBackup;
        private SSCheckbox chkStartArchivingOnStartup;
        private SSCheckbox chkAddToStartup;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private MaterialSkin.Controls.MaterialDivider sep1;
        private System.Windows.Forms.ComboBox cmbChecksumAlgorithm;
        private System.Windows.Forms.Label lblChecksumAlgorithm;
        private System.Windows.Forms.ComboBox cmbCompressionLevel;
        private System.Windows.Forms.Label lblCompressionLevel;
        private MonoFlat.MonoFlat_HeaderLabel lblGeneralSettings;
        private System.Windows.Forms.Label label1;
    }
}