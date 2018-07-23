namespace AutoArchiver
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSep1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTargets = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTargetInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.tmrTime = new System.Windows.Forms.Timer(this.components);
            this.conMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.conAddFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.conAddFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.conSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.conCopyChecksum = new System.Windows.Forms.ToolStripMenuItem();
            this.conSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.conResetChecksum = new System.Windows.Forms.ToolStripMenuItem();
            this.conResetAllChecksums = new System.Windows.Forms.ToolStripMenuItem();
            this.conSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.conRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.menMain = new System.Windows.Forms.MenuStrip();
            this.menFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menAddFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.menAddFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menFileSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.menFileCopyChecksum = new System.Windows.Forms.ToolStripMenuItem();
            this.menFileSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.menResetChecksum = new System.Windows.Forms.ToolStripMenuItem();
            this.menResetAllChecksums = new System.Windows.Forms.ToolStripMenuItem();
            this.menFileSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.menRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.menFileSep4 = new System.Windows.Forms.ToolStripSeparator();
            this.menMinimizeToTray = new System.Windows.Forms.ToolStripMenuItem();
            this.menFileSep5 = new System.Windows.Forms.ToolStripSeparator();
            this.menExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menView = new System.Windows.Forms.ToolStripMenuItem();
            this.menShowStatusbar = new System.Windows.Forms.ToolStripMenuItem();
            this.menViewSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.menShowStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.menShowTargetCount = new System.Windows.Forms.ToolStripMenuItem();
            this.menShowCurrentTime = new System.Windows.Forms.ToolStripMenuItem();
            this.menSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menViewHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menHelpSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.menAboutAutoArchiver = new System.Windows.Forms.ToolStripMenuItem();
            this.panMain = new System.Windows.Forms.Panel();
            this.nbMain = new MonoFlat.MonoFlat_NotificationBox();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.txtBackupLocation = new AutoArchiver.SSTextbox();
            this.cmbBackupCompression = new System.Windows.Forms.ComboBox();
            this.lblBackupCompression = new System.Windows.Forms.Label();
            this.srchTargets = new DevExpress.XtraEditors.SearchControl();
            this.btnMinimizeToTray = new System.Windows.Forms.LinkLabel();
            this.lblBackupInterval = new System.Windows.Forms.Label();
            this.chkShowPassword = new AutoArchiver.SSCheckbox();
            this.chkEncryptBackup = new AutoArchiver.SSCheckbox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnStartMonitoring = new System.Windows.Forms.Button();
            this.txtBackupPassword = new System.Windows.Forms.TextBox();
            this.lblBackupPassword = new System.Windows.Forms.Label();
            this.lvBackupTargets = new BrightIdeasSoftware.ObjectListView();
            this.colFileName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colFilePath = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colBackupExtension = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colFileSize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colChecksum = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colLastChecked = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lblBackupTargets = new System.Windows.Forms.Label();
            this.lblBackupLocation = new System.Windows.Forms.Label();
            this.niMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.conNotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.conShowAutoArchiver = new System.Windows.Forms.ToolStripMenuItem();
            this.conNISep1 = new System.Windows.Forms.ToolStripSeparator();
            this.conStartMonitoring = new System.Windows.Forms.ToolStripMenuItem();
            this.conStopMonitoring = new System.Windows.Forms.ToolStripMenuItem();
            this.conNISep2 = new System.Windows.Forms.ToolStripSeparator();
            this.conExit = new System.Windows.Forms.ToolStripMenuItem();
            this.imgMain = new System.Windows.Forms.ImageList(this.components);
            this.ssMain.SuspendLayout();
            this.conMain.SuspendLayout();
            this.menMain.SuspendLayout();
            this.panMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.srchTargets.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lvBackupTargets)).BeginInit();
            this.conNotifyIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // ssMain
            // 
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblStatusInfo,
            this.lblSep1,
            this.lblTargets,
            this.lblTargetInfo,
            this.lblSpring,
            this.lblTime});
            this.ssMain.Location = new System.Drawing.Point(0, 673);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(1236, 22);
            this.ssMain.TabIndex = 6;
            this.ssMain.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(42, 17);
            this.lblStatus.Text = "Status:";
            // 
            // lblStatusInfo
            // 
            this.lblStatusInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblStatusInfo.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatusInfo.Name = "lblStatusInfo";
            this.lblStatusInfo.Size = new System.Drawing.Size(26, 17);
            this.lblStatusInfo.Text = "Idle";
            // 
            // lblSep1
            // 
            this.lblSep1.BackColor = System.Drawing.Color.Transparent;
            this.lblSep1.Name = "lblSep1";
            this.lblSep1.Size = new System.Drawing.Size(10, 17);
            this.lblSep1.Text = "|";
            // 
            // lblTargets
            // 
            this.lblTargets.BackColor = System.Drawing.Color.Transparent;
            this.lblTargets.Name = "lblTargets";
            this.lblTargets.Size = new System.Drawing.Size(51, 17);
            this.lblTargets.Text = "Targets: ";
            // 
            // lblTargetInfo
            // 
            this.lblTargetInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblTargetInfo.Name = "lblTargetInfo";
            this.lblTargetInfo.Size = new System.Drawing.Size(13, 17);
            this.lblTargetInfo.Text = "0";
            // 
            // lblSpring
            // 
            this.lblSpring.BackColor = System.Drawing.Color.Transparent;
            this.lblSpring.Name = "lblSpring";
            this.lblSpring.Size = new System.Drawing.Size(1045, 17);
            this.lblSpring.Spring = true;
            // 
            // lblTime
            // 
            this.lblTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(34, 17);
            this.lblTime.Text = "Time";
            // 
            // tmrTime
            // 
            this.tmrTime.Tick += new System.EventHandler(this.tmrTime_Tick);
            // 
            // conMain
            // 
            this.conMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.conAddFiles,
            this.conAddFolder,
            this.conSep1,
            this.conCopyChecksum,
            this.conSep2,
            this.conResetChecksum,
            this.conResetAllChecksums,
            this.conSep3,
            this.conRemove});
            this.conMain.Name = "conMain";
            this.conMain.Size = new System.Drawing.Size(204, 154);
            this.conMain.Opening += new System.ComponentModel.CancelEventHandler(this.conMain_Opening);
            // 
            // conAddFiles
            // 
            this.conAddFiles.Image = global::AutoArchiver.Properties.Resources.add_file_16;
            this.conAddFiles.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.conAddFiles.Name = "conAddFiles";
            this.conAddFiles.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.conAddFiles.Size = new System.Drawing.Size(203, 22);
            this.conAddFiles.Text = "Add Files";
            this.conAddFiles.ToolTipText = "Add files that should be automatically backed up";
            this.conAddFiles.Click += new System.EventHandler(this.conAddFiles_Click);
            // 
            // conAddFolder
            // 
            this.conAddFolder.Image = global::AutoArchiver.Properties.Resources.add_folder_16;
            this.conAddFolder.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.conAddFolder.Name = "conAddFolder";
            this.conAddFolder.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.conAddFolder.Size = new System.Drawing.Size(203, 22);
            this.conAddFolder.Text = "Add Folder";
            this.conAddFolder.ToolTipText = "Add a folder that should be automatically backed up";
            this.conAddFolder.Click += new System.EventHandler(this.conAddFolder_Click);
            // 
            // conSep1
            // 
            this.conSep1.Name = "conSep1";
            this.conSep1.Size = new System.Drawing.Size(200, 6);
            // 
            // conCopyChecksum
            // 
            this.conCopyChecksum.Image = global::AutoArchiver.Properties.Resources.clipboard_16;
            this.conCopyChecksum.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.conCopyChecksum.Name = "conCopyChecksum";
            this.conCopyChecksum.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.conCopyChecksum.Size = new System.Drawing.Size(203, 22);
            this.conCopyChecksum.Text = "Copy Checksum";
            this.conCopyChecksum.ToolTipText = "Copy the currently selected items checksum to the clipboard";
            this.conCopyChecksum.Click += new System.EventHandler(this.conCopyChecksum_Click);
            // 
            // conSep2
            // 
            this.conSep2.Name = "conSep2";
            this.conSep2.Size = new System.Drawing.Size(200, 6);
            // 
            // conResetChecksum
            // 
            this.conResetChecksum.Image = global::AutoArchiver.Properties.Resources.reset_checksum_16;
            this.conResetChecksum.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.conResetChecksum.Name = "conResetChecksum";
            this.conResetChecksum.Size = new System.Drawing.Size(203, 22);
            this.conResetChecksum.Text = "Reset Checksum";
            this.conResetChecksum.ToolTipText = "Reset the currently selected items checksums to their default value";
            this.conResetChecksum.Click += new System.EventHandler(this.conResetChecksum_Click);
            // 
            // conResetAllChecksums
            // 
            this.conResetAllChecksums.Image = global::AutoArchiver.Properties.Resources.reset_all_checksums_16;
            this.conResetAllChecksums.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.conResetAllChecksums.Name = "conResetAllChecksums";
            this.conResetAllChecksums.Size = new System.Drawing.Size(203, 22);
            this.conResetAllChecksums.Text = "Reset All Checksums";
            this.conResetAllChecksums.ToolTipText = "Reset all checksums to their default value";
            this.conResetAllChecksums.Click += new System.EventHandler(this.conResetAllChecksums_Click);
            // 
            // conSep3
            // 
            this.conSep3.Name = "conSep3";
            this.conSep3.Size = new System.Drawing.Size(200, 6);
            // 
            // conRemove
            // 
            this.conRemove.Image = global::AutoArchiver.Properties.Resources.remove_16;
            this.conRemove.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.conRemove.Name = "conRemove";
            this.conRemove.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.conRemove.Size = new System.Drawing.Size(203, 22);
            this.conRemove.Text = "Remove";
            this.conRemove.ToolTipText = "Remove the currently selected items from the backup list";
            this.conRemove.Click += new System.EventHandler(this.conRemove_Click);
            // 
            // menMain
            // 
            this.menMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menFile,
            this.menView,
            this.menSettings,
            this.menHelp});
            this.menMain.Location = new System.Drawing.Point(0, 0);
            this.menMain.Name = "menMain";
            this.menMain.Size = new System.Drawing.Size(1236, 24);
            this.menMain.TabIndex = 13;
            this.menMain.Text = "menuStrip1";
            // 
            // menFile
            // 
            this.menFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menAddFiles,
            this.menAddFolder,
            this.menFileSep1,
            this.menFileCopyChecksum,
            this.menFileSep2,
            this.menResetChecksum,
            this.menResetAllChecksums,
            this.menFileSep3,
            this.menRemove,
            this.menFileSep4,
            this.menMinimizeToTray,
            this.menFileSep5,
            this.menExit});
            this.menFile.Name = "menFile";
            this.menFile.Size = new System.Drawing.Size(37, 20);
            this.menFile.Text = "File";
            this.menFile.DropDownOpening += new System.EventHandler(this.menFile_DropDownOpening);
            // 
            // menAddFiles
            // 
            this.menAddFiles.Image = global::AutoArchiver.Properties.Resources.add_file_16;
            this.menAddFiles.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menAddFiles.Name = "menAddFiles";
            this.menAddFiles.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.menAddFiles.Size = new System.Drawing.Size(207, 22);
            this.menAddFiles.Text = "Add Files";
            this.menAddFiles.ToolTipText = "Add files that should be automatically backed up";
            this.menAddFiles.Click += new System.EventHandler(this.menAddFiles_Click);
            // 
            // menAddFolder
            // 
            this.menAddFolder.Image = global::AutoArchiver.Properties.Resources.add_folder_16;
            this.menAddFolder.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menAddFolder.Name = "menAddFolder";
            this.menAddFolder.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.menAddFolder.Size = new System.Drawing.Size(207, 22);
            this.menAddFolder.Text = "Add Folder";
            this.menAddFolder.ToolTipText = "Add a folder that should be automatically backed up";
            this.menAddFolder.Click += new System.EventHandler(this.menAddFolder_Click);
            // 
            // menFileSep1
            // 
            this.menFileSep1.Name = "menFileSep1";
            this.menFileSep1.Size = new System.Drawing.Size(204, 6);
            // 
            // menFileCopyChecksum
            // 
            this.menFileCopyChecksum.Image = global::AutoArchiver.Properties.Resources.clipboard_16;
            this.menFileCopyChecksum.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menFileCopyChecksum.Name = "menFileCopyChecksum";
            this.menFileCopyChecksum.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.menFileCopyChecksum.Size = new System.Drawing.Size(207, 22);
            this.menFileCopyChecksum.Text = "Copy Checksum";
            this.menFileCopyChecksum.ToolTipText = "Copy the currently selected items checksum to the clipboard";
            this.menFileCopyChecksum.Click += new System.EventHandler(this.menFileCopyChecksum_Click);
            // 
            // menFileSep2
            // 
            this.menFileSep2.Name = "menFileSep2";
            this.menFileSep2.Size = new System.Drawing.Size(204, 6);
            // 
            // menResetChecksum
            // 
            this.menResetChecksum.Image = global::AutoArchiver.Properties.Resources.reset_checksum_16;
            this.menResetChecksum.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menResetChecksum.Name = "menResetChecksum";
            this.menResetChecksum.Size = new System.Drawing.Size(207, 22);
            this.menResetChecksum.Text = "Reset Checksum";
            this.menResetChecksum.ToolTipText = "Reset the currently selected items checksums to their default value";
            this.menResetChecksum.Click += new System.EventHandler(this.menResetChecksum_Click);
            // 
            // menResetAllChecksums
            // 
            this.menResetAllChecksums.Image = global::AutoArchiver.Properties.Resources.reset_all_checksums_16;
            this.menResetAllChecksums.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menResetAllChecksums.Name = "menResetAllChecksums";
            this.menResetAllChecksums.Size = new System.Drawing.Size(207, 22);
            this.menResetAllChecksums.Text = "Reset All Checksums";
            this.menResetAllChecksums.ToolTipText = "Reset all checksums to their default value";
            this.menResetAllChecksums.Click += new System.EventHandler(this.menResetAllChecksums_Click);
            // 
            // menFileSep3
            // 
            this.menFileSep3.Name = "menFileSep3";
            this.menFileSep3.Size = new System.Drawing.Size(204, 6);
            // 
            // menRemove
            // 
            this.menRemove.Image = global::AutoArchiver.Properties.Resources.remove_16;
            this.menRemove.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menRemove.Name = "menRemove";
            this.menRemove.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.menRemove.Size = new System.Drawing.Size(207, 22);
            this.menRemove.Text = "Remove";
            this.menRemove.ToolTipText = "Remove the currently selected items from the backup list";
            this.menRemove.Click += new System.EventHandler(this.menRemove_Click);
            // 
            // menFileSep4
            // 
            this.menFileSep4.Name = "menFileSep4";
            this.menFileSep4.Size = new System.Drawing.Size(204, 6);
            // 
            // menMinimizeToTray
            // 
            this.menMinimizeToTray.Image = global::AutoArchiver.Properties.Resources.minimize_16;
            this.menMinimizeToTray.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menMinimizeToTray.Name = "menMinimizeToTray";
            this.menMinimizeToTray.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.menMinimizeToTray.Size = new System.Drawing.Size(207, 22);
            this.menMinimizeToTray.Text = "Minimize to Tray";
            this.menMinimizeToTray.ToolTipText = "Minimize Auto Archiver to the task tray";
            this.menMinimizeToTray.Click += new System.EventHandler(this.menMinimizeToTray_Click);
            // 
            // menFileSep5
            // 
            this.menFileSep5.Name = "menFileSep5";
            this.menFileSep5.Size = new System.Drawing.Size(204, 6);
            // 
            // menExit
            // 
            this.menExit.Image = global::AutoArchiver.Properties.Resources.close_16;
            this.menExit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menExit.Name = "menExit";
            this.menExit.Size = new System.Drawing.Size(207, 22);
            this.menExit.Text = "Exit";
            this.menExit.ToolTipText = "Close the application";
            this.menExit.Click += new System.EventHandler(this.menExit_Click);
            // 
            // menView
            // 
            this.menView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menShowStatusbar,
            this.menViewSep1,
            this.menShowStatus,
            this.menShowTargetCount,
            this.menShowCurrentTime});
            this.menView.Name = "menView";
            this.menView.Size = new System.Drawing.Size(44, 20);
            this.menView.Text = "View";
            // 
            // menShowStatusbar
            // 
            this.menShowStatusbar.Checked = true;
            this.menShowStatusbar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menShowStatusbar.Name = "menShowStatusbar";
            this.menShowStatusbar.Size = new System.Drawing.Size(176, 22);
            this.menShowStatusbar.Text = "Show Statusbar";
            this.menShowStatusbar.ToolTipText = "Show the Auto Archiver statusbar";
            this.menShowStatusbar.Click += new System.EventHandler(this.menShowStatusbar_Click);
            // 
            // menViewSep1
            // 
            this.menViewSep1.Name = "menViewSep1";
            this.menViewSep1.Size = new System.Drawing.Size(173, 6);
            // 
            // menShowStatus
            // 
            this.menShowStatus.Checked = true;
            this.menShowStatus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menShowStatus.Name = "menShowStatus";
            this.menShowStatus.Size = new System.Drawing.Size(176, 22);
            this.menShowStatus.Text = "Show Status";
            this.menShowStatus.ToolTipText = "Show the current status of Auto Archiver";
            this.menShowStatus.Click += new System.EventHandler(this.menShowStatus_Click);
            // 
            // menShowTargetCount
            // 
            this.menShowTargetCount.Checked = true;
            this.menShowTargetCount.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menShowTargetCount.Name = "menShowTargetCount";
            this.menShowTargetCount.Size = new System.Drawing.Size(176, 22);
            this.menShowTargetCount.Text = "Show Target Count";
            this.menShowTargetCount.ToolTipText = "Show how many items are currently in the list for backup";
            this.menShowTargetCount.Click += new System.EventHandler(this.menShowTargetCount_Click);
            // 
            // menShowCurrentTime
            // 
            this.menShowCurrentTime.Checked = true;
            this.menShowCurrentTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menShowCurrentTime.Name = "menShowCurrentTime";
            this.menShowCurrentTime.Size = new System.Drawing.Size(176, 22);
            this.menShowCurrentTime.Text = "Show Current Time";
            this.menShowCurrentTime.ToolTipText = "Show the current available time";
            this.menShowCurrentTime.Click += new System.EventHandler(this.menShowCurrentTime_Click);
            // 
            // menSettings
            // 
            this.menSettings.Name = "menSettings";
            this.menSettings.Size = new System.Drawing.Size(61, 20);
            this.menSettings.Text = "Settings";
            this.menSettings.Click += new System.EventHandler(this.menSettings_Click);
            // 
            // menHelp
            // 
            this.menHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menViewHelp,
            this.menHelpSep1,
            this.menAboutAutoArchiver});
            this.menHelp.Name = "menHelp";
            this.menHelp.Size = new System.Drawing.Size(44, 20);
            this.menHelp.Text = "Help";
            // 
            // menViewHelp
            // 
            this.menViewHelp.Image = global::AutoArchiver.Properties.Resources.help_16;
            this.menViewHelp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menViewHelp.Name = "menViewHelp";
            this.menViewHelp.Size = new System.Drawing.Size(183, 22);
            this.menViewHelp.Text = "View Help";
            this.menViewHelp.ToolTipText = "View the available help documentation for Auto Archiver";
            this.menViewHelp.Click += new System.EventHandler(this.menViewHelp_Click);
            // 
            // menHelpSep1
            // 
            this.menHelpSep1.Name = "menHelpSep1";
            this.menHelpSep1.Size = new System.Drawing.Size(180, 6);
            // 
            // menAboutAutoArchiver
            // 
            this.menAboutAutoArchiver.Image = global::AutoArchiver.Properties.Resources.about_16;
            this.menAboutAutoArchiver.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menAboutAutoArchiver.Name = "menAboutAutoArchiver";
            this.menAboutAutoArchiver.Size = new System.Drawing.Size(183, 22);
            this.menAboutAutoArchiver.Text = "About Auto Archiver";
            this.menAboutAutoArchiver.ToolTipText = "View about information for Auto Archiver";
            this.menAboutAutoArchiver.Click += new System.EventHandler(this.menAboutAutoArchiver_Click);
            // 
            // panMain
            // 
            this.panMain.Controls.Add(this.nbMain);
            this.panMain.Controls.Add(this.numInterval);
            this.panMain.Controls.Add(this.txtBackupLocation);
            this.panMain.Controls.Add(this.cmbBackupCompression);
            this.panMain.Controls.Add(this.lblBackupCompression);
            this.panMain.Controls.Add(this.srchTargets);
            this.panMain.Controls.Add(this.btnMinimizeToTray);
            this.panMain.Controls.Add(this.lblBackupInterval);
            this.panMain.Controls.Add(this.chkShowPassword);
            this.panMain.Controls.Add(this.chkEncryptBackup);
            this.panMain.Controls.Add(this.btnClose);
            this.panMain.Controls.Add(this.btnStartMonitoring);
            this.panMain.Controls.Add(this.txtBackupPassword);
            this.panMain.Controls.Add(this.lblBackupPassword);
            this.panMain.Controls.Add(this.lvBackupTargets);
            this.panMain.Controls.Add(this.lblBackupTargets);
            this.panMain.Controls.Add(this.lblBackupLocation);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 24);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(1236, 649);
            this.panMain.TabIndex = 14;
            // 
            // nbMain
            // 
            this.nbMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nbMain.BorderCurve = 8;
            this.nbMain.Font = new System.Drawing.Font("Tahoma", 9F);
            this.nbMain.Image = null;
            this.nbMain.Location = new System.Drawing.Point(0, 608);
            this.nbMain.MinimumSize = new System.Drawing.Size(100, 40);
            this.nbMain.Name = "nbMain";
            this.nbMain.NotificationType = MonoFlat.MonoFlat_NotificationBox.Type.Notice;
            this.nbMain.RoundCorners = false;
            this.nbMain.ShowCloseButton = true;
            this.nbMain.Size = new System.Drawing.Size(1236, 40);
            this.nbMain.TabIndex = 35;
            this.nbMain.Text = "Hey, Jason!";
            this.nbMain.Visible = false;
            // 
            // numInterval
            // 
            this.numInterval.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numInterval.Location = new System.Drawing.Point(12, 68);
            this.numInterval.Maximum = new decimal(new int[] {
            3600000,
            0,
            0,
            0});
            this.numInterval.Name = "numInterval";
            this.numInterval.Size = new System.Drawing.Size(1212, 20);
            this.numInterval.TabIndex = 34;
            // 
            // txtBackupLocation
            // 
            this.txtBackupLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBackupLocation.Location = new System.Drawing.Point(12, 29);
            this.txtBackupLocation.Name = "txtBackupLocation";
            this.txtBackupLocation.ReadOnly = true;
            this.txtBackupLocation.Size = new System.Drawing.Size(1212, 20);
            this.txtBackupLocation.TabIndex = 33;
            this.txtBackupLocation.Text = "Click to browse...";
            this.txtBackupLocation.ToolTipText = "Select the location that backups will be stored";
            this.txtBackupLocation.Click += new System.EventHandler(this.txtBackupLocation_Click);
            this.txtBackupLocation.TextChanged += new System.EventHandler(this.txtBackupLocation_TextChanged);
            // 
            // cmbBackupCompression
            // 
            this.cmbBackupCompression.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBackupCompression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBackupCompression.FormattingEnabled = true;
            this.cmbBackupCompression.Items.AddRange(new object[] {
            "ZIP File (.zip)",
            "TAR FIle (.tar)",
            "TAR/GZIP FIle (.tar.gz)"});
            this.cmbBackupCompression.Location = new System.Drawing.Point(12, 107);
            this.cmbBackupCompression.Name = "cmbBackupCompression";
            this.cmbBackupCompression.Size = new System.Drawing.Size(1212, 21);
            this.cmbBackupCompression.TabIndex = 22;
            this.cmbBackupCompression.SelectedIndexChanged += new System.EventHandler(this.cmbBackupCompression_SelectedIndexChanged);
            // 
            // lblBackupCompression
            // 
            this.lblBackupCompression.AutoSize = true;
            this.lblBackupCompression.Location = new System.Drawing.Point(9, 91);
            this.lblBackupCompression.Name = "lblBackupCompression";
            this.lblBackupCompression.Size = new System.Drawing.Size(110, 13);
            this.lblBackupCompression.TabIndex = 31;
            this.lblBackupCompression.Text = "Backup Compression:";
            // 
            // srchTargets
            // 
            this.srchTargets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.srchTargets.Enabled = false;
            this.srchTargets.Location = new System.Drawing.Point(12, 186);
            this.srchTargets.Name = "srchTargets";
            this.srchTargets.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Repository.ClearButton(),
            new DevExpress.XtraEditors.Repository.SearchButton()});
            this.srchTargets.Properties.NullValuePrompt = "Search targets...";
            this.srchTargets.Size = new System.Drawing.Size(1212, 20);
            this.srchTargets.TabIndex = 24;
            this.srchTargets.TextChanged += new System.EventHandler(this.srchTargets_TextChanged);
            // 
            // btnMinimizeToTray
            // 
            this.btnMinimizeToTray.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(105)))), ((int)(((byte)(200)))));
            this.btnMinimizeToTray.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimizeToTray.AutoSize = true;
            this.btnMinimizeToTray.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(146)))), ((int)(((byte)(255)))));
            this.btnMinimizeToTray.Location = new System.Drawing.Point(928, 618);
            this.btnMinimizeToTray.Name = "btnMinimizeToTray";
            this.btnMinimizeToTray.Size = new System.Drawing.Size(83, 13);
            this.btnMinimizeToTray.TabIndex = 30;
            this.btnMinimizeToTray.TabStop = true;
            this.btnMinimizeToTray.Text = "Minimize to Tray";
            this.btnMinimizeToTray.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnMinimizeToTray_LinkClicked);
            // 
            // lblBackupInterval
            // 
            this.lblBackupInterval.AutoSize = true;
            this.lblBackupInterval.Location = new System.Drawing.Point(9, 52);
            this.lblBackupInterval.Name = "lblBackupInterval";
            this.lblBackupInterval.Size = new System.Drawing.Size(107, 13);
            this.lblBackupInterval.TabIndex = 29;
            this.lblBackupInterval.Text = "Backup Interval (ms):";
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.Enabled = false;
            this.chkShowPassword.Location = new System.Drawing.Point(120, 617);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(102, 17);
            this.chkShowPassword.TabIndex = 28;
            this.chkShowPassword.Text = "Show Password";
            this.chkShowPassword.ToolTipText = "Remove the password char and show the users\' password";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);
            // 
            // chkEncryptBackup
            // 
            this.chkEncryptBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkEncryptBackup.AutoSize = true;
            this.chkEncryptBackup.Location = new System.Drawing.Point(12, 617);
            this.chkEncryptBackup.Name = "chkEncryptBackup";
            this.chkEncryptBackup.Size = new System.Drawing.Size(102, 17);
            this.chkEncryptBackup.TabIndex = 27;
            this.chkEncryptBackup.Text = "Encrypt Backup";
            this.chkEncryptBackup.ToolTipText = "Encrypt all backups with a master password";
            this.chkEncryptBackup.UseVisualStyleBackColor = true;
            this.chkEncryptBackup.CheckedChanged += new System.EventHandler(this.chkEncryptBackup_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(1017, 613);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnStartMonitoring
            // 
            this.btnStartMonitoring.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartMonitoring.Enabled = false;
            this.btnStartMonitoring.Location = new System.Drawing.Point(1098, 613);
            this.btnStartMonitoring.Name = "btnStartMonitoring";
            this.btnStartMonitoring.Size = new System.Drawing.Size(126, 23);
            this.btnStartMonitoring.TabIndex = 17;
            this.btnStartMonitoring.Text = "Start Monitoring";
            this.btnStartMonitoring.UseVisualStyleBackColor = true;
            this.btnStartMonitoring.Click += new System.EventHandler(this.btnStartMonitoring_Click);
            // 
            // txtBackupPassword
            // 
            this.txtBackupPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBackupPassword.Enabled = false;
            this.txtBackupPassword.Location = new System.Drawing.Point(12, 147);
            this.txtBackupPassword.Name = "txtBackupPassword";
            this.txtBackupPassword.PasswordChar = '•';
            this.txtBackupPassword.Size = new System.Drawing.Size(1212, 20);
            this.txtBackupPassword.TabIndex = 23;
            this.txtBackupPassword.TextChanged += new System.EventHandler(this.txtBackupPassword_TextChanged);
            // 
            // lblBackupPassword
            // 
            this.lblBackupPassword.AutoSize = true;
            this.lblBackupPassword.Location = new System.Drawing.Point(9, 131);
            this.lblBackupPassword.Name = "lblBackupPassword";
            this.lblBackupPassword.Size = new System.Drawing.Size(96, 13);
            this.lblBackupPassword.TabIndex = 21;
            this.lblBackupPassword.Text = "Backup Password:";
            // 
            // lvBackupTargets
            // 
            this.lvBackupTargets.AllColumns.Add(this.colFileName);
            this.lvBackupTargets.AllColumns.Add(this.colFilePath);
            this.lvBackupTargets.AllColumns.Add(this.colBackupExtension);
            this.lvBackupTargets.AllColumns.Add(this.colFileSize);
            this.lvBackupTargets.AllColumns.Add(this.colChecksum);
            this.lvBackupTargets.AllColumns.Add(this.colLastChecked);
            this.lvBackupTargets.AlternateRowBackColor = System.Drawing.SystemColors.ButtonFace;
            this.lvBackupTargets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvBackupTargets.CellEditUseWholeCell = false;
            this.lvBackupTargets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFileName,
            this.colFilePath,
            this.colBackupExtension,
            this.colFileSize,
            this.colChecksum,
            this.colLastChecked});
            this.lvBackupTargets.ContextMenuStrip = this.conMain;
            this.lvBackupTargets.Cursor = System.Windows.Forms.Cursors.Default;
            this.lvBackupTargets.EmptyListMsg = "No Targets";
            this.lvBackupTargets.FullRowSelect = true;
            this.lvBackupTargets.GridLines = true;
            this.lvBackupTargets.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvBackupTargets.Location = new System.Drawing.Point(12, 212);
            this.lvBackupTargets.Name = "lvBackupTargets";
            this.lvBackupTargets.SelectColumnsOnRightClick = false;
            this.lvBackupTargets.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.None;
            this.lvBackupTargets.ShowFilterMenuOnRightClick = false;
            this.lvBackupTargets.ShowGroups = false;
            this.lvBackupTargets.ShowHeaderInAllViews = false;
            this.lvBackupTargets.Size = new System.Drawing.Size(1212, 386);
            this.lvBackupTargets.TabIndex = 25;
            this.lvBackupTargets.UseAlternatingBackColors = true;
            this.lvBackupTargets.UseCompatibleStateImageBehavior = false;
            this.lvBackupTargets.UseFilterIndicator = true;
            this.lvBackupTargets.UseFiltering = true;
            this.lvBackupTargets.UseHotItem = true;
            this.lvBackupTargets.View = System.Windows.Forms.View.List;
            this.lvBackupTargets.ItemsChanged += new System.EventHandler<BrightIdeasSoftware.ItemsChangedEventArgs>(this.lvBackupTargets_ItemsChanged);
            // 
            // colFileName
            // 
            this.colFileName.Text = "File Name";
            // 
            // colFilePath
            // 
            this.colFilePath.Text = "Path";
            // 
            // colBackupExtension
            // 
            this.colBackupExtension.Text = "Backup Extension";
            // 
            // colFileSize
            // 
            this.colFileSize.Text = "Size";
            // 
            // colChecksum
            // 
            this.colChecksum.Text = "Checksum";
            // 
            // colLastChecked
            // 
            this.colLastChecked.Text = "Last Checked";
            // 
            // lblBackupTargets
            // 
            this.lblBackupTargets.AutoSize = true;
            this.lblBackupTargets.Location = new System.Drawing.Point(9, 170);
            this.lblBackupTargets.Name = "lblBackupTargets";
            this.lblBackupTargets.Size = new System.Drawing.Size(86, 13);
            this.lblBackupTargets.TabIndex = 18;
            this.lblBackupTargets.Text = "Backup Targets:";
            // 
            // lblBackupLocation
            // 
            this.lblBackupLocation.AutoSize = true;
            this.lblBackupLocation.Location = new System.Drawing.Point(9, 13);
            this.lblBackupLocation.Name = "lblBackupLocation";
            this.lblBackupLocation.Size = new System.Drawing.Size(91, 13);
            this.lblBackupLocation.TabIndex = 15;
            this.lblBackupLocation.Text = "Backup Location:";
            // 
            // niMain
            // 
            this.niMain.ContextMenuStrip = this.conNotifyIcon;
            this.niMain.Icon = ((System.Drawing.Icon)(resources.GetObject("niMain.Icon")));
            this.niMain.Text = "Auto Archiver - Version 1.0.0";
            this.niMain.Visible = true;
            this.niMain.DoubleClick += new System.EventHandler(this.niMain_DoubleClick);
            // 
            // conNotifyIcon
            // 
            this.conNotifyIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.conShowAutoArchiver,
            this.conNISep1,
            this.conStartMonitoring,
            this.conStopMonitoring,
            this.conNISep2,
            this.conExit});
            this.conNotifyIcon.Name = "conNotifyIcon";
            this.conNotifyIcon.Size = new System.Drawing.Size(180, 104);
            this.conNotifyIcon.Opening += new System.ComponentModel.CancelEventHandler(this.conNotifyIcon_Opening);
            // 
            // conShowAutoArchiver
            // 
            this.conShowAutoArchiver.Image = global::AutoArchiver.Properties.Resources.show_16;
            this.conShowAutoArchiver.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.conShowAutoArchiver.Name = "conShowAutoArchiver";
            this.conShowAutoArchiver.Size = new System.Drawing.Size(179, 22);
            this.conShowAutoArchiver.Text = "Show Auto Archiver";
            this.conShowAutoArchiver.ToolTipText = "Bring Auto Archiver to the foreground";
            this.conShowAutoArchiver.Click += new System.EventHandler(this.conShowAutoArchiver_Click);
            // 
            // conNISep1
            // 
            this.conNISep1.Name = "conNISep1";
            this.conNISep1.Size = new System.Drawing.Size(176, 6);
            // 
            // conStartMonitoring
            // 
            this.conStartMonitoring.Image = global::AutoArchiver.Properties.Resources.start_16;
            this.conStartMonitoring.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.conStartMonitoring.Name = "conStartMonitoring";
            this.conStartMonitoring.Size = new System.Drawing.Size(179, 22);
            this.conStartMonitoring.Text = "Start Monitoring";
            this.conStartMonitoring.ToolTipText = "Start monitoring all list items for backups";
            this.conStartMonitoring.Click += new System.EventHandler(this.conStartMonitoring_Click);
            // 
            // conStopMonitoring
            // 
            this.conStopMonitoring.Image = global::AutoArchiver.Properties.Resources.stop_16;
            this.conStopMonitoring.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.conStopMonitoring.Name = "conStopMonitoring";
            this.conStopMonitoring.Size = new System.Drawing.Size(179, 22);
            this.conStopMonitoring.Text = "Stop Monitoring";
            this.conStopMonitoring.ToolTipText = "Stop monitoring list items";
            this.conStopMonitoring.Click += new System.EventHandler(this.conStopMonitoring_Click);
            // 
            // conNISep2
            // 
            this.conNISep2.Name = "conNISep2";
            this.conNISep2.Size = new System.Drawing.Size(176, 6);
            // 
            // conExit
            // 
            this.conExit.Image = global::AutoArchiver.Properties.Resources.close_16;
            this.conExit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.conExit.Name = "conExit";
            this.conExit.Size = new System.Drawing.Size(179, 22);
            this.conExit.Text = "Exit";
            this.conExit.ToolTipText = "Exit the application";
            this.conExit.Click += new System.EventHandler(this.conExit_Click);
            // 
            // imgMain
            // 
            this.imgMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgMain.ImageStream")));
            this.imgMain.TransparentColor = System.Drawing.Color.Transparent;
            this.imgMain.Images.SetKeyName(0, "folder-16.png");
            this.imgMain.Images.SetKeyName(1, "file-16.png");
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1236, 695);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.menMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menMain;
            this.MinimumSize = new System.Drawing.Size(809, 369);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auto Archiver - Version 1.0.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            this.conMain.ResumeLayout(false);
            this.menMain.ResumeLayout(false);
            this.menMain.PerformLayout();
            this.panMain.ResumeLayout(false);
            this.panMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.srchTargets.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lvBackupTargets)).EndInit();
            this.conNotifyIcon.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblSpring;
        private System.Windows.Forms.ToolStripStatusLabel lblTime;
        private System.Windows.Forms.Timer tmrTime;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusInfo;
        private System.Windows.Forms.ToolStripStatusLabel lblSep1;
        private System.Windows.Forms.ToolStripStatusLabel lblTargets;
        private System.Windows.Forms.ToolStripStatusLabel lblTargetInfo;
        private System.Windows.Forms.ContextMenuStrip conMain;
        private System.Windows.Forms.ToolStripMenuItem conAddFiles;
        private System.Windows.Forms.ToolStripMenuItem conAddFolder;
        private System.Windows.Forms.ToolStripSeparator conSep1;
        private System.Windows.Forms.ToolStripMenuItem conRemove;
        private System.Windows.Forms.MenuStrip menMain;
        private System.Windows.Forms.ToolStripMenuItem menFile;
        private System.Windows.Forms.ToolStripMenuItem menView;
        private System.Windows.Forms.ToolStripMenuItem menSettings;
        private System.Windows.Forms.ToolStripMenuItem menHelp;
        private System.Windows.Forms.ToolStripMenuItem menViewHelp;
        private System.Windows.Forms.ToolStripSeparator menHelpSep1;
        private System.Windows.Forms.ToolStripMenuItem menAboutAutoArchiver;
        private System.Windows.Forms.ToolStripMenuItem menAddFiles;
        private System.Windows.Forms.ToolStripMenuItem menAddFolder;
        private System.Windows.Forms.ToolStripMenuItem menRemove;
        private System.Windows.Forms.ToolStripSeparator menFileSep3;
        private System.Windows.Forms.ToolStripMenuItem menMinimizeToTray;
        private System.Windows.Forms.ToolStripSeparator menFileSep4;
        private System.Windows.Forms.ToolStripMenuItem menExit;
        private System.Windows.Forms.ToolStripMenuItem menShowStatusbar;
        private System.Windows.Forms.ToolStripSeparator menViewSep1;
        private System.Windows.Forms.ToolStripMenuItem menShowStatus;
        private System.Windows.Forms.ToolStripMenuItem menShowTargetCount;
        private System.Windows.Forms.ToolStripMenuItem menShowCurrentTime;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.ComboBox cmbBackupCompression;
        private System.Windows.Forms.Label lblBackupCompression;
        private DevExpress.XtraEditors.SearchControl srchTargets;
        private System.Windows.Forms.LinkLabel btnMinimizeToTray;
        private System.Windows.Forms.Label lblBackupInterval;
        private SSCheckbox chkShowPassword;
        private SSCheckbox chkEncryptBackup;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnStartMonitoring;
        private System.Windows.Forms.TextBox txtBackupPassword;
        private System.Windows.Forms.Label lblBackupPassword;
        private BrightIdeasSoftware.ObjectListView lvBackupTargets;
        private BrightIdeasSoftware.OLVColumn colFileName;
        private BrightIdeasSoftware.OLVColumn colFilePath;
        private BrightIdeasSoftware.OLVColumn colBackupExtension;
        private BrightIdeasSoftware.OLVColumn colFileSize;
        private BrightIdeasSoftware.OLVColumn colChecksum;
        private BrightIdeasSoftware.OLVColumn colLastChecked;
        private System.Windows.Forms.Label lblBackupTargets;
        private System.Windows.Forms.Label lblBackupLocation;
        private AutoArchiver.SSTextbox txtBackupLocation;
        private System.Windows.Forms.NotifyIcon niMain;
        private System.Windows.Forms.ContextMenuStrip conNotifyIcon;
        private System.Windows.Forms.ToolStripMenuItem conShowAutoArchiver;
        private System.Windows.Forms.ToolStripSeparator conNISep1;
        private System.Windows.Forms.ToolStripMenuItem conStartMonitoring;
        private System.Windows.Forms.ToolStripMenuItem conStopMonitoring;
        private System.Windows.Forms.ToolStripSeparator conNISep2;
        private System.Windows.Forms.ToolStripMenuItem conExit;
        private System.Windows.Forms.NumericUpDown numInterval;
        private MonoFlat.MonoFlat_NotificationBox nbMain;
        private System.Windows.Forms.ToolStripSeparator menFileSep1;
        private System.Windows.Forms.ToolStripMenuItem menResetChecksum;
        private System.Windows.Forms.ToolStripMenuItem menResetAllChecksums;
        private System.Windows.Forms.ToolStripSeparator menFileSep2;
        private System.Windows.Forms.ToolStripMenuItem conResetChecksum;
        private System.Windows.Forms.ToolStripMenuItem conResetAllChecksums;
        private System.Windows.Forms.ToolStripSeparator conSep2;
        private System.Windows.Forms.ToolStripMenuItem menFileCopyChecksum;
        private System.Windows.Forms.ToolStripSeparator menFileSep5;
        private System.Windows.Forms.ToolStripMenuItem conCopyChecksum;
        private System.Windows.Forms.ToolStripSeparator conSep3;
        private System.Windows.Forms.ImageList imgMain;
    }
}

