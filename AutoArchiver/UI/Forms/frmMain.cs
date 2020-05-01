/*
==============================================================================
Copyright © Jason Drawdy 

All rights reserved.

The MIT License (MIT)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

Except as contained in this notice, the name of the above copyright holder
shall not be used in advertising or otherwise to promote the sale, use or
other dealings in this Software without prior written authorization.
==============================================================================
*/

#region Imports

using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Xml.Linq;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Lzw;
using ICSharpCode.SharpZipLib.Core;
using AutoArchiver.API.Objects;
using AutoArchiver.API.Tools;
using BrightIdeasSoftware;
using LongFile = Pri.LongPath.File;
using NotificationType = MonoFlat.MonoFlat_NotificationBox.Type;

#endregion
namespace AutoArchiver
{
    public partial class frmMain : Form
    {
        #region Variables

        internal enum CompressionMethod
        {
            ZIP = 0,
            TAR = 1,
            TAR_GZIP = 2,
        }

        private CancellationTokenSource Token = new CancellationTokenSource();
        private bool Cancelling = false;
        private bool Monitoring = false;
        private Thread Monitor = null;
        private Dictionary<string, Thread> Notifications = new Dictionary<string, Thread>();

        #endregion
        #region Initialization

        public frmMain()
        {
            InitializeComponent();

            // Create text overlays with colors to match our theme.
            TextOverlay fileOverlay = lvBackupTargets.EmptyListMsgOverlay as TextOverlay;
            fileOverlay.BorderWidth = 0f;
            fileOverlay.Font = new Font(Font.FontFamily, 12);
            fileOverlay.TextColor = Color.DimGray;
            fileOverlay.BackColor = Color.FromArgb(255, 255, 255);
            fileOverlay.BorderColor = Color.FromArgb(40, 146, 255);

            // Create our HotTracking decoration.
            RowBorderDecoration rbd = new RowBorderDecoration();
            rbd.BorderPen = new Pen(Color.FromArgb(64, Color.White), 0);
            rbd.FillBrush = new SolidBrush(Color.FromArgb(64, SystemColors.Highlight));
            rbd.BoundsPadding = new Size(0, 0);
            rbd.CornerRounding = 0.0f;
            lvBackupTargets.HotItemStyle = new HotItemStyle();
            lvBackupTargets.HotItemStyle.Decoration = rbd;

            //Set our list's image getter.
            ImageList imgs = new ImageList();
            imgs.ColorDepth = ColorDepth.Depth32Bit;
            imgs.Images.Add(Properties.Resources.question_mark_16);
            imgs.Images.Add(Properties.Resources.folder_16);
            imgs.Images.Add(Properties.Resources.file_16);
            lvBackupTargets.SmallImageList = imgs;
            lvBackupTargets.LargeImageList = imgs;
            colFileName.ImageGetter += delegate (object rowObject)
            {
                int index = 0;
                MonitorTarget target = (MonitorTarget)rowObject;
                FileAttributes attributes = new FileAttributes();
                try { attributes = File.GetAttributes(target.FilePath); }
                catch { return index; }

                if (attributes.HasFlag(FileAttributes.Directory))
                    index = 1;
                else
                    index = 2;
                return index;
            };

            //// Set listview column aspects programmatically since we're utilizing executable encryption.
            //colFileName.AspectGetter += delegate (object x)
            //{ return ((FileObject)x).Name; };
            colFileName.AspectGetter += delegate (object x) { return ((MonitorTarget)x).FileName; };
            colFilePath.AspectGetter += delegate (object x) { return ((MonitorTarget)x).FilePath; };
            colBackupExtension.AspectGetter += delegate (object x) { return ((MonitorTarget)x).BackupExtension; };
            colFileSize.AspectGetter += delegate (object x) { return ((MonitorTarget)x).FileSize; };
            colChecksum.AspectGetter += delegate (object x) { return ((MonitorTarget)x).Checksum; };
            colLastChecked.AspectGetter += delegate (object x) { return ((MonitorTarget)x).LastChecked; };

            // Load our settings from our settings file.
            SettingsManager.LoadSettings();

            // Check if we should start minimized or not.
            if (SettingsManager.ArchiveOnStartup)
            {
                    WindowState = FormWindowState.Minimized;
                    ShowInTaskbar = false;
            }
        }

        #endregion
        #region Controls

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Load our settings and setup our controls.
            LoadSettings();
            UpdateExtensions();
            tmrTime.Start();
            Task.Run(() => IsMonitoring());

            // Cleanup any temp files.
            if (IsValidPath(txtBackupLocation.Text))
                Task.Run(() => CleanupTemps(txtBackupLocation.Text, true));

            // Check if we should start archiving right away or not.
            if (SettingsManager.ArchiveOnStartup)
            {
                if (btnStartMonitoring.Enabled)
                    conStartMonitoring.PerformClick();
                else
                {
                    WindowState = FormWindowState.Normal;
                    ShowInTaskbar = true;
                }
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!chkEncryptBackup.Checked)
            {
                txtBackupPassword.Text = "";
                chkShowPassword.Checked = false;
            }
            SetSettings();
            SettingsManager.SaveSettings();
        }

        private async void btnStartMonitoring_Click(object sender, EventArgs e)
        {
            if (btnStartMonitoring.Text == "Start Monitoring")
            {
                string path = txtBackupLocation.Text;
                int interval = (int)numInterval.Value;
                CompressionMethod method = (CompressionMethod)cmbBackupCompression.SelectedIndex;
                string password = (chkEncryptBackup.Checked) ? txtBackupPassword.Text : null;
                bool ask = SettingsManager.AskBeforeBackup;
                Monitoring = true;
                DisableControls();
                Invoke(new Action(() => btnStartMonitoring.Text = "Stop Monitoring"));
                Invoke(new Action(() => conStartMonitoring.Enabled = false));
                (Monitor = new Thread(async () => await StartMonitoring(path, interval, method, password, ask))).Start();
            }
            else
            {
                try { Token.Cancel(); } catch { }
                StreamUtils.ThrowException(); // Stop writing a file if we're writing.
                Invoke(new Action(() => btnStartMonitoring.Enabled = false));
                Invoke(new Action(() => conStartMonitoring.Enabled = false));
                Invoke(new Action(() => conStopMonitoring.Enabled = false));
                Invoke(new Action(() => lvBackupTargets.Focus())); // Do this so focus isn't on the search control.
                Cancelling = true;
                while (Monitoring)
                {
                    await Task.Delay(100);
                }
                Cancelling = false;
                Monitor = null;
                StreamUtils.ResetFlags();
                EnableControls();
                Invoke(new Action(() => btnStartMonitoring.Text = "Start Monitoring"));
                Invoke(new Action(() => btnStartMonitoring.Enabled = true));
                Invoke(new Action(() => conStartMonitoring.Enabled = true));
                Invoke(new Action(() => conStopMonitoring.Enabled = true));
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            SetSettings();
            SettingsManager.SaveSettings();
            Close();
        }

        private void btnMinimizeToTray_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            if (Monitoring)
            {
                int count = lvBackupTargets.Items.Count;
                if (count == 1)
                    niMain.ShowBalloonTip(5000, "Auto Archiver", "Auto Archiver has been minimized and is currently monitoring 1 target.", ToolTipIcon.Info);
                else
                    niMain.ShowBalloonTip(5000, "Auto Archiver", "Auto Archiver has been minimized and is currently monitoring " + count.ToString() + " targets.", ToolTipIcon.Info);
            }
            else
                niMain.ShowBalloonTip(5000, "Auto Archiver", "Auto Archiver has been minimized to the tasktray.", ToolTipIcon.Info);
        }

        private void txtBackupLocation_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.Description = "Select a destination folder";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                bool exists = false;
                string path = fbd.SelectedPath;

                if (lvBackupTargets.Items.Count > 0)
                {
                    foreach (MonitorTarget target in lvBackupTargets.Objects)
                    {
                        if (path == target.FilePath)
                        {
                            exists = true;
                            break;
                        }
                    }
                }

                if (exists)
                    MessageBox.Show("The selected path already exists as a backup item!", "Auto Archiver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    txtBackupLocation.Text = path;
            }
        }

        private void cmbBackupCompression_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvBackupTargets.Items.Count > 0)
            {
                List<MonitorTarget> targets = lvBackupTargets.Objects.Cast<MonitorTarget>().ToList();
                foreach (MonitorTarget target in targets)
                    target.UpdateExtension(((CompressionMethod)cmbBackupCompression.SelectedIndex).ToString());
                RefreshList(targets);
            }
        }

        private void menAddFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Title = "Select files to monitor";
            ofd.Filter = "All Files (*.*)|*.*";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                List<MonitorTarget> targets = new List<MonitorTarget>();
                foreach (string file in ofd.FileNames)
                {
                    string filename = Path.GetFileName(file);
                    string filepath = file;
                    string extension = ((CompressionMethod)cmbBackupCompression.SelectedIndex).ToString();
                    string size = new FileInfo(file).Length.ToFileSize();
                    MonitorTarget target = new MonitorTarget(filename, filepath, extension, size);
                    targets.Add(target);
                }
                if (targets.Count > 0)
                    lvBackupTargets.AddObjects(targets);
            }
        }

        private void menAddFolder_Click(object sender, EventArgs e)
        {
            lblStatusInfo.Text = "Test";
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select a folder to monitor";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                // Create an obect and add it to the list.
                string name = Path.GetFileName(fbd.SelectedPath);
                string path = fbd.SelectedPath;
                string extension = ((CompressionMethod)cmbBackupCompression.SelectedIndex).ToString();
                string size = "N/A";

                if (path == txtBackupLocation.Text)
                    MessageBox.Show("The selected path already exists as the backup location!", "Auto Archiver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    // Add our object to the list.
                    MonitorTarget target = new MonitorTarget(name, path, extension, size);
                    lvBackupTargets.AddObject(target);

                    // Get the object from the lists' collection in order to be able to update it.
                    foreach (MonitorTarget mo in lvBackupTargets.Objects)
                    {
                        if (mo == target)
                        {
                            Task.Run(() => GetFolderSize(mo));
                            break;
                        }
                    }
                }
            }
        }

        private void menFileCopyChecksum_Click(object sender, EventArgs e)
        {
            try
            {
                MonitorTarget target = (MonitorTarget)lvBackupTargets.SelectedObject;
                Clipboard.Clear();
                Clipboard.SetText(target.Checksum);
                MessageBox.Show("The selected checksum has been copied to the clipboard.", "Auto Archiver", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch { MessageBox.Show("The selected checksum could not be copied to the clipboard.", "Auto Archiver", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void menResetChecksum_Click(object sender, EventArgs e)
        {
            if (lvBackupTargets.SelectedObjects.Count == 1)
            {
                if (MessageBox.Show("Are you sure you would like to reset this checksum?", "Auto Archiver",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    MonitorTarget target = (MonitorTarget)lvBackupTargets.SelectedObject;
                    target.UpdateChecksum("N/A");
                    RefreshList(target);
                }
            }
            else
            {
                if (MessageBox.Show("Are you sure you would like to reset these checksums?", "Auto Archiver",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    List<MonitorTarget> targets = lvBackupTargets.SelectedObjects.Cast<MonitorTarget>().ToList();
                    foreach (MonitorTarget target in targets)
                        target.UpdateChecksum("N/A");
                    RefreshList(targets);
                }
            }
        }

        private void menResetAllChecksums_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you would like to reset all checksums?", "Auto Archiver",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                List<MonitorTarget> targets = lvBackupTargets.Objects.Cast<MonitorTarget>().ToList();
                foreach (MonitorTarget target in targets)
                    target.UpdateChecksum("N/A");
                RefreshList(targets);
            }
        }

        private void menRemove_Click(object sender, EventArgs e)
        {
            if (lvBackupTargets.SelectedObjects.Count > 1)
            {
                if (MessageBox.Show("Are you sure you would like to remove the selected items?", "Auto Archiver",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    lvBackupTargets.RemoveObjects(lvBackupTargets.SelectedObjects);
            }
            else
            {
                if (MessageBox.Show("Are you sure you would like to remove this item?", "Auto Archiver",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    lvBackupTargets.RemoveObject(lvBackupTargets.SelectedObject);
            }
        }

        private void menMinimizeToTray_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            if (Monitoring)
            {
                int count = lvBackupTargets.Items.Count;
                if (count == 1)
                    niMain.ShowBalloonTip(5000, "Auto Archiver", "Auto Archiver has been minimized and is currently monitoring 1 target.", ToolTipIcon.Info);
                else
                    niMain.ShowBalloonTip(5000, "Auto Archiver", "Auto Archiver has been minimized and is currently monitoring " + count.ToString() + " targets.", ToolTipIcon.Info);
            }
            else
                niMain.ShowBalloonTip(5000, "Auto Archiver", "Auto Archiver has been minimized to the tasktray.", ToolTipIcon.Info);
        }

        private void menExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menShowStatusbar_Click(object sender, EventArgs e)
        {
            if (menShowStatusbar.Checked)
            {
                menShowStatusbar.Checked = false;
                ssMain.Visible = false;
            }
            else
            {
                menShowStatusbar.Checked = true;
                ssMain.Visible = true;
            }
        }

        private void menShowStatus_Click(object sender, EventArgs e)
        {
            if (menShowStatus.Checked)
            {
                menShowStatus.Checked = false;
                lblStatus.Visible = false;
                lblStatusInfo.Visible = false;
                if (menShowTargetCount.Checked)
                    lblSep1.Visible = false;
            }
            else
            {
                menShowStatus.Checked = true;
                lblStatus.Visible = true;
                lblStatusInfo.Visible = true;
                if (menShowTargetCount.Checked)
                    lblSep1.Visible = true;
            }
        }

        private void menShowTargetCount_Click(object sender, EventArgs e)
        {
            if (menShowTargetCount.Checked)
            {
                menShowTargetCount.Checked = false;
                lblTargets.Visible = false;
                lblTargetInfo.Visible = false;
                if (menShowStatus.Checked)
                    lblSep1.Visible = false;
            }
            else
            {
                menShowTargetCount.Checked = true;
                lblTargets.Visible = true;
                lblTargetInfo.Visible = true;
                if (menShowStatus.Checked)
                    lblSep1.Visible = true;
            }
        }

        private void menShowCurrentTime_Click(object sender, EventArgs e)
        {
            if (menShowCurrentTime.Checked)
            {
                menShowCurrentTime.Checked = false;
                lblTime.Visible = false;
            }
            else
            {
                menShowCurrentTime.Checked = true;
                lblTime.Visible = true;
            }
        }

        private void menViewHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("There currently is no help documentation.", "Auto Archiver", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menAboutAutoArchiver_Click(object sender, EventArgs e)
        {
            frmAbout about = new frmAbout();
            about.ShowDialog();
        }

        private void tmrTime_Tick(object sender, EventArgs e)
        {
            string timestamp = DateTime.Now.ToLongTimeString();
            lblTime.Text = timestamp;
        }

        private void txtBackupPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtBackupPassword.Text == null || txtBackupPassword.Text == "")
                btnStartMonitoring.Enabled = false;
            else
            {
                if (lvBackupTargets.Items.Count > 0)
                {
                    if (txtBackupLocation.Text != null && txtBackupLocation.Text != "Click to browse...")
                        btnStartMonitoring.Enabled = true;
                }
                else
                    btnStartMonitoring.Enabled = false;
            }
        }

        private void lvBackupTargets_ItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            if (lvBackupTargets.Items.Count == 0)
            {
                srchTargets.Enabled = false;
                btnStartMonitoring.Enabled = false;
                lvBackupTargets.View = View.List;
                lblTargetInfo.Text = "0";
            }
            else
            {
                srchTargets.Enabled = true;
                lvBackupTargets.View = View.Details;
                lvBackupTargets.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                lblTargetInfo.Text = lvBackupTargets.Items.Count.ToString();
                if (txtBackupLocation.Text != null && txtBackupLocation.Text != "Click to browse...")
                {
                    if (chkEncryptBackup.Checked)
                    {
                        if (txtBackupPassword.Text != null)
                            btnStartMonitoring.Enabled = true;
                    }
                    else
                        btnStartMonitoring.Enabled = true;
                }
                else
                    btnStartMonitoring.Enabled = false;
            }
        }

        private void srchTargets_TextChanged(object sender, EventArgs e)
        {
            lvBackupTargets.ModelFilter = TextMatchFilter.Contains(lvBackupTargets, srchTargets.Text);
            if (lvBackupTargets.Items.Count == 0)
                lvBackupTargets.View = View.List;
            else
                lvBackupTargets.View = View.Details;
        }

        private void niMain_DoubleClick(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
        }

        private void conShowAutoArchiver_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
        }

        private void conStartMonitoring_Click(object sender, EventArgs e)
        {
            string path = txtBackupLocation.Text;
            int interval = (int)numInterval.Value;
            CompressionMethod method = (CompressionMethod)cmbBackupCompression.SelectedIndex;
            string password = (chkEncryptBackup.Checked) ? txtBackupPassword.Text : null;
            bool ask = SettingsManager.AskBeforeBackup;
            Monitoring = true;
            DisableControls();
            Invoke(new Action(() => btnStartMonitoring.Text = "Stop Monitoring"));
            Invoke(new Action(() => conStartMonitoring.Enabled = false));
            (Monitor = new Thread(async () => await StartMonitoring(path, interval, method, password, ask))).Start();
            if (WindowState == FormWindowState.Minimized)
            {
                int targets = lvBackupTargets.Items.Count;
                if (targets == 1)
                    niMain.ShowBalloonTip(5000, "Auto Archiver", "Auto Archiver is now actively monitoring " + targets.ToString() + " target.", ToolTipIcon.Info);
                else
                    niMain.ShowBalloonTip(5000, "Auto Archiver", "Auto Archiver is now actively monitoring " + targets.ToString() + " targets.", ToolTipIcon.Info);
            }
        }

        private async void conStopMonitoring_Click(object sender, EventArgs e)
        {
            StreamUtils.ThrowException(); // Stop writing a file if we're writing.
            Invoke(new Action(() => btnStartMonitoring.Enabled = false));
            Invoke(new Action(() => conStartMonitoring.Enabled = false));
            Invoke(new Action(() => conStopMonitoring.Enabled = false));
            Invoke(new Action(() => lvBackupTargets.Focus())); // Do this so focus isn't on the search control.
            Cancelling = true;
            while (Monitoring)
            {
                await Task.Delay(100);
            }
            Cancelling = false;
            Monitor = null;
            StreamUtils.ResetFlags();
            EnableControls();
            Invoke(new Action(() => btnStartMonitoring.Text = "Start Monitoring"));
            Invoke(new Action(() => btnStartMonitoring.Enabled = true));
            Invoke(new Action(() => conStartMonitoring.Enabled = true));
            Invoke(new Action(() => conStopMonitoring.Enabled = true));
        }

        private void conExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void conAddFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Title = "Select files to monitor";
            ofd.Filter = "All Files (*.*)|*.*";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                List<MonitorTarget> targets = new List<MonitorTarget>();
                foreach (string file in ofd.FileNames)
                {
                    string filename = Path.GetFileName(file);
                    string filepath = file;
                    string extension = ((CompressionMethod)cmbBackupCompression.SelectedIndex).ToString();
                    string size = new FileInfo(file).Length.ToFileSize();
                    MonitorTarget target = new MonitorTarget(filename, filepath, extension, size);
                    targets.Add(target);
                }
                if (targets.Count > 0)
                    lvBackupTargets.AddObjects(targets);
            }
        }

        private void conAddFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select a folder to monitor";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                // Create an obect and add it to the list.
                string name = Path.GetFileName(fbd.SelectedPath);
                string path = fbd.SelectedPath;
                string extension = ((CompressionMethod)cmbBackupCompression.SelectedIndex).ToString();
                string size = "N/A";

                if (path == txtBackupLocation.Text)
                    MessageBox.Show("The selected path already exists as the backup location!", "Auto Archiver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    // Add our object to the list.
                    MonitorTarget target = new MonitorTarget(name, path, extension, size);
                    lvBackupTargets.AddObject(target);

                    // Get the object from the lists' collection in order to be able to update it.
                    foreach (MonitorTarget mo in lvBackupTargets.Objects)
                    {
                        if (mo == target)
                        {
                            Task.Run(() => GetFolderSize(mo));
                            break;
                        }
                    }
                }
            }
        }

        private void conCopyChecksum_Click(object sender, EventArgs e)
        {
            try
            {
                MonitorTarget target = (MonitorTarget)lvBackupTargets.SelectedObject;
                Clipboard.Clear();
                Clipboard.SetText(target.Checksum);
                MessageBox.Show("The selected checksum has been copied to the clipboard.", "Auto Archiver", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch { MessageBox.Show("The selected checksum could not be copied to the clipboard.", "Auto Archiver", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void conResetChecksum_Click(object sender, EventArgs e)
        {
            if (lvBackupTargets.SelectedObjects.Count == 1)
            {
                if (MessageBox.Show("Are you sure you would like to reset this checksum?", "Auto Archiver",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    MonitorTarget target = (MonitorTarget)lvBackupTargets.SelectedObject;
                    target.UpdateChecksum("N/A");
                    RefreshList(target);
                }
            }
            else
            {
                if (MessageBox.Show("Are you sure you would like to reset these checksums?", "Auto Archiver",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    List<MonitorTarget> targets = lvBackupTargets.SelectedObjects.Cast<MonitorTarget>().ToList();
                    foreach (MonitorTarget target in targets)
                        target.UpdateChecksum("N/A");
                    RefreshList(targets);
                }
            }
        }

        private void conResetAllChecksums_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you would like to reset all checksums?", "Auto Archiver",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                List<MonitorTarget> targets = lvBackupTargets.Objects.Cast<MonitorTarget>().ToList();
                foreach (MonitorTarget target in targets)
                    target.UpdateChecksum("N/A");
                RefreshList(targets);
            }
        }

        private void conRemove_Click(object sender, EventArgs e)
        {
            if (lvBackupTargets.SelectedObjects.Count > 1)
            {
                if (MessageBox.Show("Are you sure you would like to remove the selected items?", "Auto Archiver",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    lvBackupTargets.RemoveObjects(lvBackupTargets.SelectedObjects);
            }
            else
            {
                if (MessageBox.Show("Are you sure you would like to remove this item?", "Auto Archiver",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    lvBackupTargets.RemoveObject(lvBackupTargets.SelectedObject);
            }
        }

        private void conNotifyIcon_Opening(object sender, CancelEventArgs e)
        {
            if (Monitoring)
            {
                conStartMonitoring.Enabled = false;
                conStopMonitoring.Enabled = true;
            }
            else
            {
                if (lvBackupTargets.Items.Count > 0)
                {
                    if (txtBackupLocation.Text != null && txtBackupLocation.Text != "Click to browse...")
                    {
                        if (chkEncryptBackup.Checked)
                        {
                            if (txtBackupPassword.Text != null && txtBackupPassword.Text != "")
                                conStartMonitoring.Enabled = true;
                            else
                                conStartMonitoring.Enabled = false;
                        }
                        else
                            conStartMonitoring.Enabled = true;
                    }
                    else
                        conStartMonitoring.Enabled = false;
                }
                else
                    conStartMonitoring.Enabled = false;
                conStopMonitoring.Enabled = false;
            }

            if (WindowState == FormWindowState.Minimized)
                conShowAutoArchiver.Enabled = true;
            else
                conShowAutoArchiver.Enabled = false;
        }

        private void chkEncryptBackup_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEncryptBackup.Checked)
            {
                txtBackupPassword.Enabled = true;
                chkShowPassword.Enabled = true;
                if (txtBackupPassword.Text != null)
                {
                    if (txtBackupLocation.Text != null && txtBackupLocation.Text != "Click to browse...")
                    {
                        if (lvBackupTargets.Items.Count > 0)
                        {
                            if (txtBackupPassword.Text != null && txtBackupPassword.Text != "")
                                btnStartMonitoring.Enabled = true;
                            else
                                btnStartMonitoring.Enabled = false;
                        }
                        else
                            btnStartMonitoring.Enabled = false;
                    }
                    else
                        btnStartMonitoring.Enabled = false;
                }
                else btnStartMonitoring.Enabled = false;
            }
            else
            {
                txtBackupPassword.Enabled = false;
                chkShowPassword.Enabled = false;
                if (txtBackupLocation.Text != null && txtBackupLocation.Text != "Click to browse...")
                {
                    if (lvBackupTargets.Items.Count > 0)
                        btnStartMonitoring.Enabled = true;
                    else
                        btnStartMonitoring.Enabled = false;
                }
                else
                    btnStartMonitoring.Enabled = false;
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword.Checked)
                txtBackupPassword.PasswordChar = ('\0');
            else
                txtBackupPassword.PasswordChar = ('•');
        }

        private void txtBackupLocation_TextChanged(object sender, EventArgs e)
        {
            if (txtBackupLocation.Text != null && txtBackupLocation.Text != "Click to browse...")
            {
                if (lvBackupTargets.Items.Count > 0)
                {
                    if (chkEncryptBackup.Checked)
                    {
                        if (txtBackupPassword.Text != null && txtBackupPassword.Text != "")
                            btnStartMonitoring.Enabled = true;
                        else
                            btnStartMonitoring.Enabled = false;
                    }
                    else
                        btnStartMonitoring.Enabled = true;
                }
                else
                    btnStartMonitoring.Enabled = false;
            }
            else
                btnStartMonitoring.Enabled = false;
        }

        private void conMain_Opening(object sender, CancelEventArgs e)
        {
            if (!Monitoring)
            {
                if (lvBackupTargets.Items.Count == 0)
                    conRemove.Enabled = false;
                else
                {
                    if (lvBackupTargets.SelectedItems.Count > 0)
                        conRemove.Enabled = true;
                    else
                        conRemove.Enabled = false;
                }
            }

            if (lvBackupTargets.Items.Count > 0)
            {
                if (lvBackupTargets.SelectedObject != null)
                    conCopyChecksum.Enabled = true;
                else
                    conCopyChecksum.Enabled = false;
                conResetAllChecksums.Enabled = true;
                if (lvBackupTargets.SelectedItems.Count > 0)
                    conResetChecksum.Enabled = true;
                else
                    conResetChecksum.Enabled = false;
            }
            else
            {
                conResetChecksum.Enabled = false;
                conResetAllChecksums.Enabled = false;
            }
        }

        private void menFile_DropDownOpening(object sender, EventArgs e)
        {
            if (!Monitoring)
            {
                if (lvBackupTargets.Items.Count == 0)
                    menRemove.Enabled = false;
                else
                {
                    if (lvBackupTargets.SelectedItems.Count > 0)
                        menRemove.Enabled = true;
                    else
                        menRemove.Enabled = false;
                }
            }

            if (lvBackupTargets.Items.Count > 0)
            {
                if (lvBackupTargets.SelectedObject != null)
                    menFileCopyChecksum.Enabled = true;
                else
                    menFileCopyChecksum.Enabled = false;
                menResetAllChecksums.Enabled = true;
                if (lvBackupTargets.SelectedItems.Count > 0)
                    menResetChecksum.Enabled = true;
                else
                    menResetChecksum.Enabled = false;
            }
            else
            {
                menResetChecksum.Enabled = false;
                menResetAllChecksums.Enabled = false;
            }
        }

        private void menSettings_Click(object sender, EventArgs e)
        {
            frmSettings settings = new frmSettings(this);
            settings.ShowDialog();
        }

        #endregion
        #region Methods

        private void LoadSettings()
        {
            // Set our controls accordingly.
            if (SettingsManager.BackupLocation != null && SettingsManager.BackupLocation != "")
                if (IsValidPath(SettingsManager.BackupLocation))
                    txtBackupLocation.Text = SettingsManager.BackupLocation;
            numInterval.Value = SettingsManager.BackupInterval;
            cmbBackupCompression.SelectedIndex = SettingsManager.BackupCompression;
            txtBackupPassword.Text = SettingsManager.BackupPassword;
            if (SettingsManager.TargetData != null)
                lvBackupTargets.AddObjects((List<MonitorTarget>)SettingsManager.TargetData.Deserialize());
            chkEncryptBackup.Checked = SettingsManager.EncryptBackup;
            chkShowPassword.Checked = SettingsManager.ShowPassword;
            menShowStatusbar.Checked = SettingsManager.ShowStatusbar;
            menShowStatus.Checked = SettingsManager.ShowStatus;
            menShowTargetCount.Checked = SettingsManager.ShowTargetCount;
            menShowCurrentTime.Checked = SettingsManager.ShowCurrentTime;
            CheckStatusbar();

            // Check if we should start archiving right away or not.
            if (SettingsManager.ArchiveOnStartup)
            {
                if (btnStartMonitoring.Enabled)
                    conStartMonitoring.PerformClick();
            }
        }

        internal void SetSettings()
        {
            // Set our settings based off of our controls.
            if (txtBackupLocation.Text != null && txtBackupLocation.Text != "Click to browse...")
                SettingsManager.BackupLocation = txtBackupLocation.Text;
            SettingsManager.BackupInterval = (int)numInterval.Value;
            SettingsManager.BackupCompression = cmbBackupCompression.SelectedIndex;
            SettingsManager.BackupPassword = txtBackupPassword.Text;
            if (lvBackupTargets.Items.Count > 0)
                SettingsManager.TargetData = lvBackupTargets.Objects.Cast<MonitorTarget>().ToList().Serialize();
            else
                SettingsManager.TargetData = null;
            SettingsManager.EncryptBackup = chkEncryptBackup.Checked;
            SettingsManager.ShowPassword = chkShowPassword.Checked;
            SettingsManager.ShowStatusbar = menShowStatusbar.Checked;
            SettingsManager.ShowStatus = menShowStatus.Checked;
            SettingsManager.ShowTargetCount = menShowTargetCount.Checked;
            SettingsManager.ShowCurrentTime = menShowCurrentTime.Checked;
        }

        private void CheckStatusbar()
        {
            // Make sure the statusbar is visible or hidden.
            if (menShowStatusbar.Checked)
                ssMain.Visible = true;
            else
                ssMain.Visible = false;

            // Make sure our status labels are shown or hidden along with the seperator.
            if (menShowStatus.Checked)
            {
                lblStatus.Visible = true;
                lblStatusInfo.Visible = true;
                if (menShowTargetCount.Checked)
                    lblSep1.Visible = true;
                else
                    lblSep1.Visible = false;
            }
            else
            {
                lblStatus.Visible = false;
                lblStatusInfo.Visible = false;
                lblSep1.Visible = false;
            }

            // Make sure our target count is shown or hidden along with the seperator.
            if (menShowTargetCount.Checked)
            {
                lblTargets.Visible = true;
                lblTargetInfo.Visible = true;
                if (menShowStatus.Checked)
                    lblSep1.Visible = true;
                else
                    lblSep1.Visible = false;
            }
            else
            {
                lblTargets.Visible = false;
                lblTargetInfo.Visible = false;
            }

            // Make sure our time is shown.
            if (menShowCurrentTime.Checked)
                lblTime.Visible = true;
            else
                lblTime.Visible = false;
        }

        private void UpdateExtensions()
        {
            if (lvBackupTargets.Items.Count > 0)
            {
                List<MonitorTarget> targets = lvBackupTargets.Objects.Cast<MonitorTarget>().ToList();
                foreach (MonitorTarget target in targets)
                    target.UpdateExtension(((CompressionMethod)cmbBackupCompression.SelectedIndex).ToString());
                RefreshList(targets);
            }
        }

        private bool IsValidPath(string path)
        {
            try
            {
                // Attempt to create a temporary file in the provided path.
                byte[] test = ("Hello, World!").ToBytes();
                string temp = path + "\\test.tmp";
                File.WriteAllBytes(temp, test);

                // Check if the file was actually written.
                if (File.Exists(temp))
                {
                    // Read the file, cleanup, and then compare our results.
                    byte[] read = File.ReadAllBytes(temp);
                    File.Delete(temp);
                    if (read.Length == test.Length)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch { return false; }
        }

        private void RefreshList(List<MonitorTarget> targets)
        {
            Invoke(new Action(() => lvBackupTargets.BeginUpdate()));
            Invoke(new Action(() => lvBackupTargets.RefreshObjects(targets)));
            Invoke(new Action(() => lvBackupTargets.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)));
            Invoke(new Action(() => lvBackupTargets.EndUpdate()));
        }

        private void RefreshList(MonitorTarget target)
        {
            Invoke(new Action(() => lvBackupTargets.BeginUpdate()));
            Invoke(new Action(() => lvBackupTargets.RefreshObject(target)));
            Invoke(new Action(() => lvBackupTargets.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)));
            Invoke(new Action(() => lvBackupTargets.EndUpdate()));
        }

        private void EnableControls()
        {
            Invoke(new Action(() => txtBackupLocation.Enabled = true));
            Invoke(new Action(() => numInterval.Enabled = true));
            Invoke(new Action(() => cmbBackupCompression.Enabled = true));
            if (chkEncryptBackup.Checked) Invoke(new Action(() => txtBackupPassword.Enabled = true));
            Invoke(new Action(() => chkEncryptBackup.Enabled = true));
            Invoke(new Action(() => menFile.Enabled = true));
            Invoke(new Action(() => conStartMonitoring.Enabled = true));
            Invoke(new Action(() => menAddFiles.Enabled = true));
            Invoke(new Action(() => menAddFolder.Enabled = true));
            Invoke(new Action(() => menRemove.Enabled = true));
            Invoke(new Action(() => conAddFiles.Enabled = true));
            Invoke(new Action(() => conAddFolder.Enabled = true));
            Invoke(new Action(() => conRemove.Enabled = true));
            //Invoke(new Action(() => srchTargets.Enabled = true));
            Invoke(new Action(() => menSettings.Enabled = true));
            Invoke(new Action(() => lvBackupTargets.Focus())); // Do this so focus isn't on the search control.
        }

        private void DisableControls()
        {
            Invoke(new Action(() => txtBackupLocation.Enabled = false));
            Invoke(new Action(() => numInterval.Enabled = false));
            Invoke(new Action(() => cmbBackupCompression.Enabled = false));
            Invoke(new Action(() => txtBackupPassword.Enabled = false));
            Invoke(new Action(() => chkEncryptBackup.Enabled = false));
            Invoke(new Action(() => conStartMonitoring.Enabled = false));
            Invoke(new Action(() => menAddFiles.Enabled = false));
            Invoke(new Action(() => menAddFolder.Enabled = false));
            Invoke(new Action(() => menRemove.Enabled = false));
            Invoke(new Action(() => conAddFiles.Enabled = false));
            Invoke(new Action(() => conAddFolder.Enabled = false));
            Invoke(new Action(() => conRemove.Enabled = false));
            //Invoke(new Action(() => srchTargets.Enabled = false));
            Invoke(new Action(() => menSettings.Enabled = false));
        }

        private async Task<bool> StartMonitoring(string path, int interval, CompressionMethod compression,
                                                 string key = null, bool ask = false, bool silent = false)
        {
            // Start a thread to monitor for temp files so they're cleaned up.
            new Thread(() => Task.Run(() => CleanupTemps(path))).Start();

            // Generate a new cancellation token.
            Token = new CancellationTokenSource();
            CancellationToken token = Token.Token;

            // Save our settings before doing any work.
            Invoke(new Action(() => SetSettings()));
            SettingsManager.SaveSettings();

            // Get our extension to use for backups.
            string extension = null;
            switch (compression)
            {
                case CompressionMethod.ZIP:
                    extension = ".zip";
                    break;
                case CompressionMethod.TAR:
                    extension = ".tar";
                    break;
                case CompressionMethod.TAR_GZIP:
                    extension = ".tar.gz";
                    break;
            }

            // Start to monitor our targets.
            while (true)
            {
                // Set the current target model in case of exceptions.
                MonitorTarget current = null;
                try
                {
                    // Check if we should stop monitoring.
                    if (Cancelling)
                    {
                        Monitoring = false;
                        return true;
                    }

                    // Check if our backup location exists.
                    if (Directory.Exists(path))
                    {
                        // Check if there are any items to monitor.
                        if (lvBackupTargets.Items.Count > 0)
                        {
                            // Check our targets and do what we need to.
                            List<MonitorTarget> ghosts = new List<MonitorTarget>();
                            foreach (MonitorTarget target in lvBackupTargets.Objects)
                            {
                                // Save the current target for later.
                                current = target;

                                // Check if we should stop monitoring.
                                if (Cancelling)
                                {
                                    Monitoring = false;
                                    return true;
                                }

                                // Check if the target exists or not.
                                bool exists = false;
                                FileAttributes attributes = File.GetAttributes(target.FilePath);
                                if (attributes.HasFlag(FileAttributes.Directory))
                                    exists = Directory.Exists(target.FilePath);
                                else
                                    exists = File.Exists(target.FilePath);

                                // Continue with our target if it exists.
                                if (exists)
                                {
                                    // Check if we're supposed to do a backup.
                                    if (Monitoring)
                                    {
                                        // Check our target's attributes.
                                        if (attributes.HasFlag(FileAttributes.Directory))
                                        {
                                            // Check if the folder is in use or not.
                                            if (await FolderIsReady(target.FilePath))
                                            {
                                                // Update our targets information.
                                                string checksum = target.Checksum;
                                                string hash = Hashing.GetDirectoryHash(target.FilePath);
                                                if (hash == null || hash == "") hash = "N/A";
                                                //target.UpdateChecksum(hash);
                                                target.UpdateLastChecked((DateTime.Now.ToLongDateString() + " @ " + DateTime.Now.ToLongTimeString()));
                                                //target.UpdateFileSize("Calculating...");

                                                // Update the list's object and resize our headers.
                                                RefreshList(target);

                                                // Begin calculating the directory's size.
                                                await CalculateTargetSize(target);

                                                // Update the lists' object once more since we're done getting the directory's size.
                                                RefreshList(target);

                                                // Check if we should backup our directory.
                                                if (checksum != hash)
                                                {
                                                    // Take a backup of our directory.
                                                    string timestamp = " (" + DateTime.Now.ToShortDateString().Replace("/", null) + "@" +
                                                                       DateTime.Now.ToString("HH:mm:ss").Replace(":", null).Replace(" ", string.Empty) + ")";
                                                    string archive = path + "\\" + Path.GetFileName(target.FilePath) + timestamp + ".tmp";

                                                    // Check if we should ask before backing up.
                                                    if (ask)
                                                    {
                                                        if (MessageBox.Show("Auto Archiver is attempting to create a backup of: \"" + target.FileName +
                                                            "\"\nWould you like to proceed with the backup?", "Auto Archiver", MessageBoxButtons.YesNo,
                                                            MessageBoxIcon.Question) == DialogResult.Yes)
                                                        {
                                                            target.UpdateChecksum(hash);
                                                            RefreshList(target);
                                                            if (!await Backup(target.FilePath, archive, extension, compression, key))
                                                            {
                                                                target.UpdateChecksum("N/A");
                                                                RefreshList(target);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        target.UpdateChecksum(hash);
                                                        RefreshList(target);
                                                        if (!await Backup(target.FilePath, archive, extension, compression, key))
                                                        {
                                                            target.UpdateChecksum("N/A");
                                                            RefreshList(target);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // Check if the file is in use or not.
                                            if (FileIsReady(target.FilePath))
                                            {
                                                // Update our targets information.
                                                string checksum = target.Checksum;
                                                string hash = Hashing.GetFileHash(target.FilePath);
                                                target.UpdateFileSize(new FileInfo(target.FilePath).Length.ToFileSize());
                                                //target.UpdateChecksum(hash);
                                                target.UpdateLastChecked((DateTime.Now.ToLongDateString() + " @ " + DateTime.Now.ToLongTimeString()));

                                                // Update the list's object and resize our headers.
                                                Invoke(new Action(() => lvBackupTargets.RefreshObject(target)));
                                                Invoke(new Action(() => lvBackupTargets.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)));

                                                // Check if we should backup our file.
                                                if (checksum != hash)
                                                {
                                                    // Try backing up the file.
                                                    string timestamp = " (" + DateTime.Now.ToShortDateString().Replace("/", null) + "@" +
                                                                       DateTime.Now.ToString("HH:mm:ss").Replace(":", null).Replace(" ", string.Empty) + ")";
                                                    string archive = path + "\\" + Path.GetFileName(target.FilePath.Replace(Path.GetExtension(target.FilePath), string.Empty)) + timestamp + ".tmp";

                                                    // Check if we should ask before backing up.
                                                    if (ask)
                                                    {
                                                        if (MessageBox.Show("Auto Archiver is attempting to create a backup of: \"" + target.FileName +
                                                            "\"\nWould you like to proceed with the backup?", "Auto Archiver", MessageBoxButtons.YesNo,
                                                            MessageBoxIcon.Question) == DialogResult.Yes)
                                                        {
                                                            target.UpdateChecksum(hash);
                                                            RefreshList(target);
                                                            if (!await Backup(target.FilePath, archive, extension, compression, key))
                                                            {
                                                                target.UpdateChecksum("N/A");
                                                                RefreshList(target);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        target.UpdateChecksum(hash);
                                                        RefreshList(target);
                                                        if (!await Backup(target.FilePath, archive, extension, compression, key))
                                                        {
                                                            target.UpdateChecksum("N/A");
                                                            RefreshList(target);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                    ghosts.Add(target);
                            }

                            // Remove all ghosts from the main list.
                            if (ghosts.Count > 0)
                                Invoke(new Action(() => lvBackupTargets.RemoveObjects(ghosts)));
                        }
                        else
                        {
                            Monitoring = false;
                            EnableControls();
                            Invoke(new Action(() => btnStartMonitoring.Text = "Start Monitoring"));
                            Invoke(new Action(() => btnStartMonitoring.Enabled = false));
                            MessageBox.Show("There are no items to monitor!", "Auto Archiver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return true;
                        }

                        // Let the thread sleep until the next cycle.
                        await Task.Delay(interval, token);
                    }
                    else
                    {
                        Monitoring = false;
                        EnableControls();
                        Invoke(new Action(() => btnStartMonitoring.Text = "Start Monitoring"));
                        Invoke(new Action(() => btnStartMonitoring.Enabled = false));
                        MessageBox.Show("The provided backup path no longer exists!", "Auto Archiver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return true;
                    }
                }
                catch
                {
                    if (current != null)
                    {
                        try
                        {
                            // This will catch if the file/folder doesn't exist.
                            FileAttributes attributes = File.GetAttributes(current.FilePath);
                        }
                        catch { Invoke(new Action(() => lvBackupTargets.RemoveObject(current))); }
                    }
                }
            }
        }

        private async Task<bool> Backup(string source, string destination, string extension, CompressionMethod compression, string password = null)
        {
            // Change our file's extension.
            string archive = Path.ChangeExtension(destination, extension);
            string temp = archive + ".tmp"; // for encryption.
            string encrypted = temp.Replace(".tmp", ".arc");

            // Get our compression level.
            int level = 9; // The default will be max compression.
            switch (SettingsManager.BackupCompression)
            {
                case 0:
                    level = 9;
                    break;
                case 1:
                    level = 6;
                    break;
                case 2:
                    level = 3;
                    break;
                case 3:
                    level = 0;
                    break;
                default:
                    level = 9;
                    break;
            }

            // Check which compression method we'll be using.
            switch (compression)
            {
                case CompressionMethod.ZIP:

                    if ((await CreateZIP(source, destination, level, password)).Successful)
                    {
                        File.Move(destination, archive);
                        
                        // Check if we should encrypt our archive.
                        if (password != null && password != "")
                        {
                            if (await Cryptography.EncryptFileAsync(archive, temp, password.ToBytes()))
                            {
                                if (File.Exists(archive))
                                    try { File.Delete(archive); }
                                    catch { await DeleteFile(archive); }
                                File.Move(temp, encrypted);
                                return true;
                            }
                            else
                            {
                                if (File.Exists(temp))
                                    try { File.Delete(temp); }
                                    catch { await DeleteFile(temp); }
                                return false;
                            }
                        }
                        else
                            return true;
                    }
                    else
                    {
                        if (File.Exists(destination))
                            try { File.Delete(destination); }
                            catch { await DeleteFile(destination); }
                        return false;
                    }
                case CompressionMethod.TAR:

                    if ((await CreateTAR(source, destination, password)).Successful)
                    {
                        File.Move(destination, archive);
                        if (password != null && password != "")
                        {
                            if (await Cryptography.EncryptFileAsync(archive, temp, password.ToBytes()))
                            {
                                if (File.Exists(archive))
                                    try { File.Delete(archive); }
                                    catch { await DeleteFile(archive); }
                                File.Move(temp, encrypted);
                                return true;
                            }
                            else
                            {
                                if (File.Exists(temp))
                                    try { File.Delete(temp); }
                                    catch { await DeleteFile(temp); }
                                return false;
                            }
                        }
                        else
                            return true;
                    }
                    else
                    {
                        if (File.Exists(destination))
                            try { File.Delete(destination); }
                            catch { await DeleteFile(destination); }
                        return false;
                    }
                case CompressionMethod.TAR_GZIP:
                    if ((await CreateTAR_GZIP(source, destination, level, password)).Successful)
                    {
                        File.Move(destination, archive);
                        if (password != null && password  != "")
                        {
                            if (await Cryptography.EncryptFileAsync(archive, temp, password.ToBytes()))
                            {
                                if (File.Exists(archive))
                                    try { File.Delete(archive); }
                                    catch { await DeleteFile(archive); }
                                File.Move(temp, encrypted);
                                return true;
                            }
                            else
                            {
                                if (File.Exists(temp))
                                    try { File.Delete(temp); }
                                    catch { await DeleteFile(temp); }
                                return false;
                            }
                        }
                        else
                            return true;
                    }
                    else
                    {
                        if (File.Exists(destination))
                            try { File.Delete(destination); }
                            catch { await DeleteFile(destination); }
                        return false;
                    }
            }
            return false;
        }

        private async Task<TaskResult> CreateZIP(string source, string destination, int level, string password = null)
        {
            // The first folder in the dirctory-hierarchy is the folder itself.
            int sourceIndex = source.LastIndexOf(@"\") + 1;

            // Get all directories and files for a given path.
            List<string> files = new List<string>();
            List<string> dirs = new List<string>();
            FileAttributes attributes = File.GetAttributes(source);
            if (attributes.HasFlag(FileAttributes.Directory))
            {
                files = Directory.GetFiles(source, "*.*", SearchOption.TopDirectoryOnly).ToList();
                dirs = Directory.GetDirectories(source, "*.*", SearchOption.AllDirectories).ToList();
            }
            else
                files.Add(source);

            // Create our zip file with our dirs and files.
            try
            {
                using (ZipOutputStream zip = new ZipOutputStream(File.Create(destination)))
                {
                    // Create a blank zip entry and set our compression level.
                    ZipEntry entry = null;
                    zip.SetLevel(level); // 0-9 with 0 being a store 9 being the best compression.

                    // Create all folder entries in the archive.
                    foreach (var item in dirs)
                    {
                        if (Directory.GetFiles(item).Count() == 0)
                        {
                            // Create an empty directory entry.
                            entry = new ZipEntry(item.Substring(sourceIndex) + "/");
                            entry.Size = 0;
                            zip.PutNextEntry(entry);
                        }
                        else
                        {
                            // Create a directory entry with files.
                            foreach (var f in Directory.GetFiles(item))
                            {
                                // Check if we should stop monitoring.
                                if (Cancelling)
                                {
                                    zip.Close();
                                    Monitoring = false;
                                    return new TaskResult(false);
                                }

                                // Create the file's entry in the archive.
                                entry = new ZipEntry(f.Substring(sourceIndex));
                                entry.Size = (new FileInfo(f)).Length;
                                zip.PutNextEntry(entry);

                                // Actually write the file's contents to the archive.
                                byte[] buffer = new byte[32 * 1024];
                                using (FileStream streamReader = File.OpenRead(f))
                                    await StreamUtils.CopyAsync(streamReader, zip, buffer);

                                // Close the entry.
                                zip.CloseEntry();
                            }
                        }
                    }

                    // Create all file entries in the archive.
                    foreach (var item in files)
                    {
                        // Check if we should stop monitoring.
                        if (Cancelling)
                        {
                            zip.Close();
                            Monitoring = false;
                            return new TaskResult(false);
                        }

                        entry = new ZipEntry(item.Substring(sourceIndex));
                        entry.Size = (new FileInfo(item)).Length;
                        zip.PutNextEntry(entry);
                        
                        byte[] buffer = new byte[32 * 1024];
                        using (FileStream streamReader = File.OpenRead(item))
                        {
                            await StreamUtils.CopyAsync(streamReader, zip, buffer);
                        }
                        zip.CloseEntry();
                    }
                }
                return new TaskResult(true);
            }
            catch { return new TaskResult(false); }
        }

        private async Task<TaskResult> CreateTAR(string source, string destination, string password = null)
        {
            try
            {
                // Check if our source is a directory or file.
                FileAttributes attributes = File.GetAttributes(source);
                bool directory = attributes.HasFlag(FileAttributes.Directory);

                // Create our archive's stream.
                using (Stream file = File.Create(destination))
                using (TarOutputStream output = new TarOutputStream(file))
                {
                    // Write our archive's contents.
                    int index = source.LastIndexOf(@"\") + 1;
                    if (await WriteTarAsync(output, source, index, directory))
                        output.Close();
                    else
                    {
                        output.Close();
                        throw new Exception("User is most likely cancelling.");
                    }
                }
                return new TaskResult(true);
            }
            catch { return new TaskResult(false); }
        }

        private async Task<TaskResult> CreateTAR_GZIP(string source, string destination, int level, string password = null)
        {
            try
            {
                // Check if our source is a directory or file.
                FileAttributes attributes = File.GetAttributes(source);
                bool directory = attributes.HasFlag(FileAttributes.Directory);

                // Create our archive's stream.
                using (Stream file = File.Create(destination))
                using (GZipOutputStream gzo = new GZipOutputStream(file))
                using (TarOutputStream output = new TarOutputStream(gzo))
                {
                    gzo.SetLevel(level);

                    // Write our archive's contents.
                    int index = source.LastIndexOf(@"\") + 1;
                    if (await WriteTarGzipAsync(output, source, index, directory))
                        output.Close();
                    else
                    {
                        output.Close();
                        throw new Exception("User is most likely cancelling.");
                    }
                }
                return new TaskResult(true);
            }
            catch { return new TaskResult(false); }
        }


        private bool WriteTar(TarOutputStream output, string source, int index)
        {
            try
            {
                // Get all files in the provided directory.
                string[] filenames = Directory.GetFiles(source);
                foreach (string filename in filenames)
                {
                    // Check if we should stop monitoring.
                    if (Cancelling)
                    {
                        output.Close();
                        Monitoring = false;
                        throw new Exception("User requested cancellation");
                    }

                    using (Stream file = File.OpenRead(filename))
                    {
                        // Create and customize our Tar entry.
                        TarEntry entry = TarEntry.CreateTarEntry(filename.Substring(index));
                        entry.Size = file.Length; // Must set size, otherwise TarOutputStream will fail when output exceeds.

                        // Add the entry to the tar stream, before writing the data.
                        output.PutNextEntry(entry);

                        // Read the files' bytes and write them to the tar stream.
                        byte[] localBuffer = new byte[32 * 1024];
                        while (true)
                        {
                            // Check if we should stop monitoring.
                            if (Cancelling)
                            {
                                output.Close();
                                Monitoring = false;
                                throw new Exception("User requested cancellation");
                            }

                            // Write our files' bytes to disk.
                            int numRead = file.Read(localBuffer, 0, localBuffer.Length);
                            if (numRead <= 0)
                                break;
                            output.Write(localBuffer, 0, numRead);
                        }
                    }
                    output.CloseEntry();
                }

                // Get all directories in the provided directory.
                string[] directories = Directory.GetDirectories(source);
                foreach (string directory in directories)
                {
                    // Check if we should stop monitoring.
                    if (Cancelling)
                    {
                        output.Close();
                        Monitoring = false;
                        throw new Exception("User requested cancellation");
                    }
                    WriteTar(output, directory, index);
                }
                return true;
            }
            catch { output.Close(); return false; }
        }
        private async Task<bool> WriteTarAsync(TarOutputStream output, string source, int index, bool isDirectory = false)
        {
            try
            {
                if (isDirectory)
                {
                    // Get all files in the provided directory.
                    string[] filenames = Directory.GetFiles(source);
                    foreach (string filename in filenames)
                    {
                        // Check if we should stop monitoring.
                        if (Cancelling)
                        {
                            output.Close();
                            Monitoring = false;
                            throw new Exception("User requested cancellation");
                        }

                        using (Stream file = File.OpenRead(filename))
                        {
                            // Create and customize our Tar entry.
                            TarEntry entry = TarEntry.CreateTarEntry(filename.Substring(index));
                            entry.Size = file.Length; // Must set size, otherwise TarOutputStream will fail when output exceeds.

                            // Add the entry to the tar stream, before writing the data.
                            output.PutNextEntry(entry);

                            // Read the files' bytes and write them to the tar stream.
                            byte[] localBuffer = new byte[32 * 1024];
                            while (true)
                            {
                                // Check if we should stop monitoring.
                                if (Cancelling)
                                {
                                    output.Close();
                                    Monitoring = false;
                                    throw new Exception("User requested cancellation");
                                }

                                // Write our files' bytes to disk.
                                int numRead = file.Read(localBuffer, 0, localBuffer.Length);
                                if (numRead <= 0)
                                    break;
                                await output.WriteAsync(localBuffer, 0, numRead);
                            }
                        }
                        output.CloseEntry();
                    }

                    // Get all directories in the provided directory.
                    string[] directories = Directory.GetDirectories(source);
                    foreach (string directory in directories)
                    {
                        // Check if we should stop monitoring.
                        if (Cancelling)
                        {
                            output.Close();
                            Monitoring = false;
                            throw new Exception("User requested cancellation");
                        }
                        WriteTar(output, directory, index);
                    }
                }
                else
                {
                    using (Stream file = File.OpenRead(source))
                    {
                        // Create and customize our Tar entry.
                        TarEntry entry = TarEntry.CreateTarEntry(source.Substring(index));
                        entry.Size = file.Length; // Must set size, otherwise TarOutputStream will fail when output exceeds.

                        // Add the entry to the tar stream, before writing the data.
                        output.PutNextEntry(entry);

                        // Read the files' bytes and write them to the tar stream.
                        byte[] localBuffer = new byte[32 * 1024];
                        while (true)
                        {
                            // Check if we should stop monitoring.
                            if (Cancelling)
                            {
                                output.Close();
                                Monitoring = false;
                                throw new Exception("User requested cancellation");
                            }

                            // Write the files' bytes to disk.
                            int numRead = file.Read(localBuffer, 0, localBuffer.Length);
                            if (numRead <= 0)
                                break;
                            await output.WriteAsync(localBuffer, 0, numRead);
                        }
                    }
                    output.CloseEntry();
                }
                return true;
            }
            catch { output.Close(); return false; }
        }

        private bool WriteTarGzip(TarOutputStream output, string source, int index)
        {
            try
            {
                // Get all files in the provided directory.
                string[] filenames = Directory.GetFiles(source);
                foreach (string filename in filenames)
                {
                    // Check if we should stop monitoring.
                    if (Cancelling)
                    {
                        output.Close();
                        Monitoring = false;
                        throw new Exception("User requested cancellation");
                    }

                    using (Stream file = File.OpenRead(filename))
                    {
                        // Create and customize our Tar entry.
                        TarEntry entry = TarEntry.CreateTarEntry(filename.Substring(index));
                        entry.Size = file.Length; // Must set size, otherwise TarOutputStream will fail when output exceeds.

                        // Add the entry to the tar stream, before writing the data.
                        output.PutNextEntry(entry);

                        // Read the files' bytes and write them to the tar stream.
                        byte[] localBuffer = new byte[32 * 1024];
                        while (true)
                        {
                            // Check if we should stop monitoring.
                            if (Cancelling)
                            {
                                output.Close();
                                Monitoring = false;
                                throw new Exception("User requested cancellation");
                            }

                            // Write our files' bytes to disk.
                            int numRead = file.Read(localBuffer, 0, localBuffer.Length);
                            if (numRead <= 0)
                                break;
                            output.Write(localBuffer, 0, numRead);
                        }
                    }
                    output.CloseEntry();
                }

                // Get all directories in the provided directory.
                string[] directories = Directory.GetDirectories(source);
                foreach (string directory in directories)
                {
                    // Check if we should stop monitoring.
                    if (Cancelling)
                    {
                        output.Close();
                        Monitoring = false;
                        throw new Exception("User requested cancellation");
                    }
                    WriteTarGzip(output, directory, index);
                }
                return true;
            }
            catch { output.Close(); return false; }
        }
        private async Task<bool> WriteTarGzipAsync(TarOutputStream output, string source, int index, bool isDirectory = false)
        {
            try
            {
                if (isDirectory)
                {
                    // Get all files in the provided directory.
                    string[] filenames = Directory.GetFiles(source);
                    foreach (string filename in filenames)
                    {
                        // Check if we should stop monitoring.
                        if (Cancelling)
                        {
                            output.Close();
                            Monitoring = false;
                            throw new Exception("User requested cancellation");
                        }

                        using (Stream file = File.OpenRead(filename))
                        {
                            // Create and customize our Tar entry.
                            TarEntry entry = TarEntry.CreateTarEntry(filename.Substring(index));
                            entry.Size = file.Length; // Must set size, otherwise TarOutputStream will fail when output exceeds.

                            // Add the entry to the tar stream, before writing the data.
                            output.PutNextEntry(entry);

                            // Read the files' bytes and write them to the tar stream.
                            byte[] localBuffer = new byte[32 * 1024];
                            while (true)
                            {
                                // Check if we should stop monitoring.
                                if (Cancelling)
                                {
                                    output.Close();
                                    Monitoring = false;
                                    throw new Exception("User requested cancellation");
                                }

                                // Write our files' bytes to disk.
                                int numRead = file.Read(localBuffer, 0, localBuffer.Length);
                                if (numRead <= 0)
                                    break;
                                await output.WriteAsync(localBuffer, 0, numRead);
                            }
                        }
                        output.CloseEntry();
                    }

                    // Get all directories in the provided directory.
                    string[] directories = Directory.GetDirectories(source);
                    foreach (string directory in directories)
                    {
                        // Check if we should stop monitoring.
                        if (Cancelling)
                        {
                            output.Close();
                            Monitoring = false;
                            throw new Exception("User requested cancellation");
                        }
                        WriteTarGzip(output, directory, index);
                    }
                }
                else
                {
                    using (Stream file = File.OpenRead(source))
                    {
                        // Create and customize our Tar entry.
                        TarEntry entry = TarEntry.CreateTarEntry(source.Substring(index));
                        entry.Size = file.Length; // Must set size, otherwise TarOutputStream will fail when output exceeds.

                        // Add the entry to the tar stream, before writing the data.
                        output.PutNextEntry(entry);

                        // Read the files' bytes and write them to the tar stream.
                        byte[] localBuffer = new byte[32 * 1024];
                        while (true)
                        {
                            // Check if we should stop monitoring.
                            if (Cancelling)
                            {
                                output.Close();
                                Monitoring = false;
                                throw new Exception("User requested cancellation");
                            }

                            // Write our files' bytes to disk.
                            int numRead = file.Read(localBuffer, 0, localBuffer.Length);
                            if (numRead <= 0)
                                break;
                            await output.WriteAsync(localBuffer, 0, numRead);
                        }
                    }
                    output.CloseEntry();
                }
                return true;
            }
            catch { output.Close(); return false; }
        }

        private async Task<bool> DeleteFile(string path)
        {
            int count = 0;
            if (File.Exists(path))
            {
                while (File.Exists(path))
                {
                    if (count >= 100) return false;
                    try { File.Delete(path); } catch { count++; await Task.Delay(100); }
                }
                return true;
            }
            else
                return true;
        }

        private async Task<bool> CleanupTemps(string path, bool runOnce = false)
        {
            try
            {
                while (true)
                {
                    // Check if we should continue looping.
                    if (!runOnce)
                    {
                        if (!Monitoring)
                            break;
                        if (Cancelling)
                            return true;
                    }
                    
                    // If the path exists then check for temp files.
                    if (Directory.Exists(path))
                    {
                        foreach (var file in Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly))
                        {
                            if (file.Contains(".tmp"))
                            {
                                if (FileIsReady(file))
                                    try { File.Delete(file); } catch { }
                            }
                        }
                    }
                    else
                        return false;

                    // Break from the loop if we're not monitoring.
                    if (runOnce) break;
                    await Task.Delay(100);
                }
                return true;
            }
            catch { return false; }
        }

        private bool FileIsReady(string path)
        {
            // One exception per file rather than several like in the polling pattern.
            try
            {
                // If we can't open the file, it's still copying.
                using (var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return true;
                }
            }
            catch { return false; }
        }

        private async Task<bool> FolderIsReady(string path)
        {
            int checks = 5;
            long currentSize = await CalculateDirectorySize(path);
            long previousSize = 0;
            if (currentSize != previousSize)
            {
                for (int i = 0; i < checks; i++)
                {
                    currentSize = await CalculateDirectorySize(path);
                    if (currentSize == previousSize)
                        return true;
                    else
                    {
                        previousSize = currentSize;
                        await Task.Delay(1000);
                    }
                }
                return false;
            }
            else
                return true;
        }

        private void MergeFolder(string source, string destination)
        {
            try
            {
                // Merge files into the original folder.
                foreach (string file in Directory.EnumerateFiles(source, "*", SearchOption.TopDirectoryOnly))
                {
                    string original = destination + @"\" + Path.GetFileName(file);
                    if (File.Exists(original)) { File.Delete(original); }
                    File.Move(file, destination + @"\" + Path.GetFileName(file));
                }

                // Merge directories into the original folder.
                foreach (string folder in Directory.EnumerateDirectories(source, "*", SearchOption.TopDirectoryOnly))
                {
                    string original = destination + @"\" + Path.GetFileName(folder);
                    if (Directory.Exists(original)) { MergeFolder(folder, original); }
                    else { Directory.Move(folder, original); }
                }

                // Delete the source folder after the merge is complete
                if (Directory.Exists(source)) { Directory.Delete(source, true); }
            }
            catch { }
        }

        private async void IsMonitoring()
        {
            while (true)
            {
                try
                {
                    if (Monitoring)
                    {
                        Invoke(new Action(() => lblStatusInfo.ForeColor = Color.DodgerBlue));
                        Invoke(new Action(() => lblStatusInfo.Text = "Active"));
                        Invoke(new Action(() => niMain.Text = (lblTargetInfo.Text == "1") ? "Auto Archiver - Active (1 Target)" :
                                                                                            "Auto Archiver - Active (" + lblTargetInfo.Text + " Targets)"));
                    }
                    else
                    {
                        Invoke(new Action(() => lblStatusInfo.ForeColor = Color.DimGray));
                        Invoke(new Action(() => lblStatusInfo.Text = "Idle"));
                        Invoke(new Action(() => niMain.Text = "Auto Archiver - Idle"));
                    }
                }
                catch { }

                await Task.Delay(25);
            }
        }

        private async Task<bool> GetFolderSize(MonitorTarget folder)
        {
            try
            {
                string size = (await CalculateDirectorySize(folder.FilePath)).ToFileSize();
                folder.UpdateFileSize(size);
                RefreshList(folder);
                return true;
            }
            catch { return false; }
        }

        private async Task<long> CalculateDirectorySize(string path)
        {
            // Get the path's size and update the list with the new target.
            try
            {
                long total = 0;

                // Calculate top-level files;
                try
                {
                    foreach (string file in Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly))
                    {
                        try { total += new FileInfo(file).Length; }
                        catch (FileNotFoundException) { }
                    }
                }
                catch (UnauthorizedAccessException) { }

                // Calculate sub-dir sizes.
                string[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                {
                    try
                    {
                        foreach (string file in Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories))
                        {
                            try { total += new FileInfo(file).Length; }
                            catch (FileNotFoundException) { }
                        }
                    }
                    catch (UnauthorizedAccessException) { }
                }
                await Task.Delay(0); // Temp call to run method async.
                return total;
            }
            catch (FileNotFoundException) { return 0; }
            catch (DirectoryNotFoundException) { return 0; }
            catch (InvalidOperationException) { return 0; }
        }
        private async Task<bool> CalculateTargetSize(MonitorTarget target)
        {
            // Get the path's size and update the list with the new target.
            try
            {
                long total = 0;
                string fileSize = "";

                string[] files = Directory.GetFiles(target.FilePath, "*.*", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    try
                    {
                        try { total += new FileInfo(file).Length; }
                        catch (FileNotFoundException) { }
                    }
                    catch (UnauthorizedAccessException) { }
                }

                string[] dirs = Directory.GetDirectories(target.FilePath);
                foreach (string dir in dirs)
                {
                    try
                    {
                        foreach (string file in Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories))
                        {
                            try { total += new FileInfo(file).Length; }
                            catch (FileNotFoundException) { }
                        }
                    }
                    catch (UnauthorizedAccessException) { }
                }

                if (total == 0) { fileSize = "0 Bytes"; }
                else { fileSize = total.ToFileSize(); }
                target.UpdateFileSize(fileSize);
                await Task.Delay(0); // Temp call to run method async.
            }
            catch (FileNotFoundException) { target.UpdateFileSize("0 Bytes"); }
            catch (DirectoryNotFoundException) { target.UpdateFileSize("0 Bytes"); }
            catch (InvalidOperationException) { return false; }
            return true;
        }

        internal void DisplayNotification(NotificationType type, string message, int timeout)
        {
            // Search our thread list for our old notification and remove it.
            try
            {
                Notifications["notificationThread"].Abort();
                Notifications.Remove("notificationThread");
                Invoke(new Action(() => nbMain.Visible = false));
            }
            catch { }

            // Show our newly created notification and add it to the thread list.
            try
            {
                Thread thread = new Thread(() => NotifyThread(type, message, timeout));
                thread.Name = "notificationThread";
                thread.Start();
                Notifications.Add("notificationThread", thread);
            }
            catch { }
        }

        private void NotifyThread(NotificationType type, string message, int timeout)
        {
            try
            {
                // Configure our notification.
                Invoke(new Action(() => nbMain.NotificationType = type));
                Invoke(new Action(() => nbMain.Text = message));
                Invoke(new Action(() => nbMain.Visible = true));

                // Show our notification for the duration and then hide it.
                Thread.Sleep(timeout);
                Invoke(new Action(() => nbMain.Visible = false));
            }
            catch { }
        }

        //private bool StartupExists()
        //{
        //    RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false);
        //    if (key.GetValue("AutoArchiver") == null)
        //        return false;
        //    else
        //        return true;
        //}

        #endregion
    }
}

namespace AutoArchiver.API.Objects
{
    public struct TaskResult
    {
        #region Variables

        public bool Successful { get; private set; }

        #endregion
        #region Initialization

        public TaskResult(bool successful) { Successful = successful; }

        #endregion
    }

    [Serializable]
    internal class FileContainer
    {
        #region Variables

        internal string File { get; private set; }
        internal string Size { get; private set; }
        internal string Created { get; private set; }
        internal string Modified { get; private set; }

        #endregion
        #region Methods

        internal FileContainer(string filename, string filesize, string created, string modified)
        {
            File = filename;
            Size = filesize;
            Created = created;
            Modified = modified;
        }

        #endregion
    }

    [Serializable]
    internal class MonitorTarget
    {
        #region Variables

        internal string FileName { get; private set; }
        internal string FilePath { get; private set; }
        internal string BackupExtension { get; private set; }
        internal string FileSize { get; private set; }
        internal string Checksum { get; private set; }
        internal string LastChecked { get; private set; }

        #endregion
        #region Initialization

        internal MonitorTarget(string filename, string filepath, string extension, string size)
        {
            FileName = filename;
            FilePath = filepath;
            BackupExtension = extension;
            FileSize = size;
            Checksum = "N/A";
            LastChecked = "N/A";
        }

        #endregion
        #region Methods

        internal void UpdateExtension(string extension) { BackupExtension = extension; }
        internal void UpdateFileSize(string filesize) { FileSize = filesize; }
        internal void UpdateChecksum(string checksum) { Checksum = checksum; }
        internal void UpdateLastChecked(string lastChecked) { LastChecked = lastChecked; }

        #endregion
    }
}

namespace AutoArchiver.API.Tools
{
    public static class SettingsManager
    {
        #region Variables

        public static string BackupLocation { get; set; } = null;
        public static int BackupInterval { get; set; } = 0;
        public static int BackupCompression { get; set; } = 0;
        public static string BackupPassword { get; set; } = null;
        public static byte[] TargetData { get; set; } = null;
        public static int CompressionLevel { get; set; } = 0;
        public static int ChecksumAlgorithm { get; set; } = 0;
        public static bool AddToStartup { get; set; } = false;
        public static bool ArchiveOnStartup { get; set; } = false;
        public static bool AskBeforeBackup { get; set; } = false;
        public static bool EncryptBackup { get; set; } = false;
        public static bool ShowPassword { get; set; } = false;
        public static bool ShowStatusbar { get; set; } = true;
        public static bool ShowStatus { get; set; } = true;
        public static bool ShowTargetCount { get; set; } = true;
        public static bool ShowCurrentTime { get; set; } = true;
        public static string SettingsFile { get; private set; } = "settings.xml";
        public static string SettingsPath { get; private set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Auto Archiver";

        #endregion
        #region Methods

        public static bool SaveSettings()
        {
            try
            {
                // Check if our settings directory exists.
                if (!Directory.Exists(SettingsPath))
                    Directory.CreateDirectory(SettingsPath);
                
                // Try to save our document and return successful or not.
                XDocument doc = SettingsDocument();
                doc.Save(SettingsPath + "\\" + SettingsFile);
                
                return true;
            }
            catch(Exception) { return false; }
        }

        public static bool LoadSettings()
        {
            string settings = SettingsPath + "\\" + SettingsFile;
            if (Directory.Exists(SettingsPath))
            {
                if (File.Exists(settings))
                {
                    try
                    {
                        XDocument doc = XDocument.Load(settings);
                        var data = from item in doc.Descendants("Settings")
                                   select new
                                   {
                                       location = item.TryGetElementValue("BackupLocation"),
                                       interval = item.TryGetElementValue("BackupInterval"),
                                       compression = item.TryGetElementValue("BackupCompression"),
                                       password = item.TryGetElementValue("BackupPassword"),
                                       targets = item.TryGetElementValue("TargetData"),
                                       level = item.TryGetElementValue("CompressionLevel"),
                                       algorithm = item.TryGetElementValue("ChecksumAlgorithm"),
                                       startup = item.TryGetElementValue("AddToStartup"),
                                       archive = item.TryGetElementValue("ArchiveOnStartup"),
                                       ask = item.TryGetElementValue("AskBeforeBackup"),
                                       encrypt = item.TryGetElementValue("EncryptBackup"),
                                       show = item.TryGetElementValue("ShowPassword"),
                                       statusbar = item.TryGetElementValue("ShowStatusbar"),
                                       status = item.TryGetElementValue("ShowStatus"),
                                       count = item.TryGetElementValue("ShowTargetCount"),
                                       time = item.TryGetElementValue("ShowCurrentTime")
                                   };
                        foreach (var item in data)
                        {
                            BackupLocation = item.location;
                            BackupInterval = item.interval.ParseInt();
                            BackupCompression = item.compression.ParseInt();
                            BackupPassword = item.password;
                            TargetData = item.targets.FromBase64();
                            CompressionLevel = item.level.ParseInt();
                            ChecksumAlgorithm = item.algorithm.ParseInt();
                            AddToStartup = item.startup.ParseBool();
                            ArchiveOnStartup = item.archive.ParseBool();
                            AskBeforeBackup = item.ask.ParseBool();
                            EncryptBackup = item.encrypt.ParseBool();
                            ShowPassword = item.show.ParseBool();

                            // Parse our statusbar setting.
                            int index = -1;
                            index = item.statusbar.ParseSetting();
                            if (index != -1)
                            {
                                if (index == 0) ShowStatusbar = true;
                                else if (index == 1) ShowStatusbar = false;
                            }

                            // Parse our status setting.
                            index = item.status.ParseSetting();
                            if (index != -1)
                            {
                                if (index == 0) ShowStatus = true;
                                else if (index == 1) ShowStatus = false;
                            }

                            // Parse our target count setting.
                            index = item.count.ParseSetting();
                            if (index != -1)
                            {
                                if (index == 0) ShowTargetCount = true;
                                else if (index == 1) ShowTargetCount = false;
                            }

                            // Parse our current time setting.
                            index = item.time.ParseSetting();
                            if (index != -1)
                            {
                                if (index == 0) ShowCurrentTime = true;
                                else if (index == 1) ShowCurrentTime = false;
                            }
                        }
                        return true;
                    }
                    catch { return false; }
                }
                else
                    return false;
            }
            else
                return false;
        }

        private static XDocument SettingsDocument()
        {
            XElement root = new XElement("AutoArchiver");
            XElement row = new XElement("Settings");
            XElement col = null;
            if (BackupLocation != "Click to browse...")
            {
                col = new XElement("BackupLocation", BackupLocation);
                row.Add(col);
            }
            col = new XElement("BackupInterval", BackupInterval.ToString());
            row.Add(col);
            col = new XElement("BackupCompression", BackupCompression.ToString());
            row.Add(col);
            col = new XElement("BackupPassword", BackupPassword);
            row.Add(col);
            if (TargetData != null)
            {
                col = new XElement("TargetData", TargetData.ToBase64());
                row.Add(col);
            }
            else
            {
                col = new XElement("TargetData", "");
                row.Add(col);
            }
            col = new XElement("CompressionLevel", CompressionLevel.ToString());
            row.Add(col);
            col = new XElement("ChecksumAlgorithm", ChecksumAlgorithm.ToString());
            row.Add(col);
            col = new XElement("AddToStartup", AddToStartup.ToString());
            row.Add(col);
            col = new XElement("ArchiveOnStartup", ArchiveOnStartup.ToString());
            row.Add(col);
            col = new XElement("AskBeforeBackup", AskBeforeBackup.ToString());
            row.Add(col);
            col = new XElement("EncryptBackup", EncryptBackup.ToString());
            row.Add(col);
            col = new XElement("ShowPassword", ShowPassword.ToString());
            row.Add(col);
            col = new XElement("ShowStatusbar", ShowStatusbar.ToString());
            row.Add(col);
            col = new XElement("ShowStatus", ShowStatus.ToString());
            row.Add(col);
            col = new XElement("ShowTargetCount", ShowTargetCount.ToString());
            row.Add(col);
            col = new XElement("ShowCurrentTime", ShowCurrentTime.ToString());
            row.Add(col);
            root.Add(row);
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                          new XComment(Hashing.GetMessageHash("Auto Archiver Settings - Version 1.0.0")),
                                          root);
            return doc;
        }

        #endregion
    }

    public static class Serializer
    {
        #region Methods

        /// <summary>
        /// Serializes an object of any type into an array of bytes.
        /// </summary>
        /// <param name="obj">The object to be converted to bytes.</param>
        /// <returns>An array of bytes containing an object.</returns>
        public static byte[] Serialize(this object obj)
        {
            try
            {
                var formatter = new BinaryFormatter();
                var stream = new MemoryStream();
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
            catch { return null; }
        }
        /// <summary>
        /// Deserializes a byte array into an object.
        /// </summary>
        /// <param name="obj">The bytes of an object to be deserialized.</param>
        /// <returns>An object from a byte array.</returns>
        public static object Deserialize(this byte[] obj)
        {
            try
            {
                var stream = new MemoryStream();
                var formatter = new BinaryFormatter();
                stream.Write(obj, 0, obj.Length);
                stream.Position = 0;
                return formatter.Deserialize(stream);
            }
            catch { return null; }
        }

        #endregion
    }

    internal class Cryptography
    {
        #region Methods

        public static bool EncryptFile(string originalFile, string encryptedFile, byte[] key)
        {
            try
            {
                string originalFilename = originalFile;
                string encryptedFilename = encryptedFile;

                using (var originalStream = LongFile.Open(originalFile, FileMode.Open))
                using (var encryptedStream = LongFile.Create(encryptedFile, 8192))
                using (var encTransform = new SecurityDriven.Inferno.EtM_EncryptTransform(key: key))
                using (var crypoStream = new CryptoStream(encryptedStream, encTransform, CryptoStreamMode.Write))
                {
                    originalStream.CopyTo(crypoStream);
                }
                return true;
            }
            catch { return false; }
        }

        public static bool DecryptFile(string originalFile, string decryptedFile, byte[] key)
        {
            try
            {
                string originalFilename = originalFile;
                string decryptedFilename = decryptedFile;

                using (var encryptedStream = LongFile.Open(originalFilename, FileMode.Open))
                using (var decryptedStream = LongFile.Create(decryptedFilename, 8192))
                using (var decTransform = new SecurityDriven.Inferno.EtM_DecryptTransform(key: key))
                {
                    using (var cryptoStream = new CryptoStream(encryptedStream, decTransform, CryptoStreamMode.Read))
                        cryptoStream.CopyTo(decryptedStream);

                    if (!decTransform.IsComplete) { throw new Exception("Not all blocks have been decrypted."); }
                }
                return true;
            }
            catch { return false; }
        }

        public static async Task<bool> EncryptFileAsync(string originalFile, string encryptedFile, byte[] key)
        {
            try
            {
                string originalFilename = originalFile;
                string encryptedFilename = encryptedFile;

                using (var originalStream = LongFile.Open(originalFile, FileMode.Open))
                using (var encryptedStream = LongFile.Create(encryptedFile, 8192))
                using (var encTransform = new SecurityDriven.Inferno.EtM_EncryptTransform(key: key))
                using (var crypoStream = new CryptoStream(encryptedStream, encTransform, CryptoStreamMode.Write))
                {
                    await originalStream.CopyToAsync(crypoStream);
                    return true;
                }
            }
            catch { return false; }
        }

        public static async Task<bool> DecryptFileAsync(string originalFile, string decryptedFile, byte[] key)
        {
            try
            {
                string originalFilename = originalFile;
                string decryptedFilename = decryptedFile;

                using (var encryptedStream = LongFile.Open(originalFilename, FileMode.Open))
                using (var decryptedStream = LongFile.Create(decryptedFilename, 8192))
                using (var decTransform = new SecurityDriven.Inferno.EtM_DecryptTransform(key: key))
                {
                    using (var cryptoStream = new CryptoStream(encryptedStream, decTransform, CryptoStreamMode.Read))
                        await cryptoStream.CopyToAsync(decryptedStream);

                    if (!decTransform.IsComplete) { throw new Exception("Not all blocks have been decrypted."); }
                }
                return true;
            }
            catch { return false; }
        }

        public static byte[] EncryptData(byte[] data, byte[] password)
        {
            using (var original = new MemoryStream(data))
            using (var encrypted = new MemoryStream())
            using (var transform = new SecurityDriven.Inferno.EtM_EncryptTransform(key: password))
            using (var crypto = new CryptoStream(encrypted, transform, CryptoStreamMode.Write))
            {
                original.CopyTo(encrypted);
                return encrypted.ToArray();
            }
        }

        public static byte[] DecryptData(byte[] data, byte[] password)
        {
            using (var original = new MemoryStream(data))
            using (var decrypted = new MemoryStream())
            using (var transform = new SecurityDriven.Inferno.EtM_DecryptTransform(key: password))
            {
                using (var crypto = new CryptoStream(original, transform, CryptoStreamMode.Read))
                    crypto.CopyTo(decrypted);
                if (!transform.IsComplete) { throw new Exception("Not all blocks have been decrypted."); }
                return decrypted.ToArray();
            }
        }

        public string EncryptText(string input, byte[] password)
        {
            return Convert.ToBase64String(EncryptData(input.ToBytes(), password));
        }

        #endregion
    }

    internal class Hashing
    {
        #region Methods

        public enum HashType
        {
            MD5,
            RIPEMD160,
            SHA1,
            SHA256,
            SHA384,
            SHA512
        }

        private static byte[] GetHash(byte[] input, HashType hash)
        {
            byte[] inputBytes = input;

            switch (hash)
            {
                case HashType.MD5:
                    return MD5.Create().ComputeHash(inputBytes);
                case HashType.RIPEMD160:
                    return RIPEMD160.Create().ComputeHash(inputBytes);
                case HashType.SHA1:
                    return SHA1.Create().ComputeHash(inputBytes);
                case HashType.SHA256:
                    return SHA256.Create().ComputeHash(inputBytes);
                case HashType.SHA384:
                    return SHA384.Create().ComputeHash(inputBytes);
                case HashType.SHA512:
                    return SHA512.Create().ComputeHash(inputBytes);
                default:
                    return inputBytes;
            }

        }

        private static byte[] GetHash(string input, HashType hash)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);

            switch (hash)
            {
                case HashType.MD5:
                    return MD5.Create().ComputeHash(inputBytes);
                case HashType.RIPEMD160:
                    return RIPEMD160.Create().ComputeHash(inputBytes);
                case HashType.SHA1:
                    return SHA1.Create().ComputeHash(inputBytes);
                case HashType.SHA256:
                    return SHA256.Create().ComputeHash(inputBytes);
                case HashType.SHA384:
                    return SHA384.Create().ComputeHash(inputBytes);
                case HashType.SHA512:
                    return SHA512.Create().ComputeHash(inputBytes);
                default:
                    return inputBytes;
            }

        }

        private static byte[] GetFileHash(string path, HashType hash)
        {
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, (1024 * 1024)))
            {
                switch (hash)
                {
                    case HashType.MD5:
                        return MD5.Create().ComputeHash(file);
                    case HashType.RIPEMD160:
                        return RIPEMD160.Create().ComputeHash(file);
                    case HashType.SHA1:
                        return SHA1.Create().ComputeHash(file);
                    case HashType.SHA256:
                        return SHA256.Create().ComputeHash(file);
                    case HashType.SHA384:
                        return SHA384.Create().ComputeHash(file);
                    case HashType.SHA512:
                        return SHA512.Create().ComputeHash(file);
                    default:
                        return null;
                }
            }
        }

        private static string ComputeHash(byte[] input, HashType type)
        {
            try
            {
                byte[] hash = GetHash(input, type);
                StringBuilder result = new StringBuilder();

                for (int i = 0; i <= hash.Length - 1; i++)
                {
                    result.Append(hash[i].ToString("x2"));
                }
                return result.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string ComputeMessageHash(string input, HashType selection)
        {
            try
            {
                byte[] hash = GetHash(input, selection);
                StringBuilder result = new StringBuilder();

                for (int i = 0; i <= hash.Length - 1; i++)
                {
                    result.Append(hash[i].ToString("x2"));
                }
                return result.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string ComputeFileHash(string input, HashType selection)
        {
            try
            {
                byte[] hash = GetFileHash(input, selection);
                StringBuilder result = new StringBuilder();

                for (int i = 0; i <= hash.Length - 1; i++)
                {
                    result.Append(hash[i].ToString("x2"));
                }

                return result.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        private static HashType GetHashType(int index)
        {
            switch (index)
            {
                case 0:
                    return HashType.MD5;
                case 1:
                    return HashType.RIPEMD160;
                case 2:
                    return HashType.SHA1;
                case 3:
                    return HashType.SHA256;
                case 4:
                    return HashType.SHA384;
                case 5:
                    return HashType.SHA512;
                default:
                    return HashType.MD5;
            }
        }
        internal static string GetMessageHash(string message)
        {
            return ComputeMessageHash(message, GetHashType(SettingsManager.ChecksumAlgorithm));
        }

        internal static string GetFileHash(string file)
        {
            return ComputeFileHash(file, GetHashType(SettingsManager.ChecksumAlgorithm));
        }

        internal static string GetDirectoryHash(string directory)
        {
            FileAttributes attributes = File.GetAttributes(directory);
            if (attributes.HasFlag(FileAttributes.Directory))
            {
                List<FileContainer> files = new List<FileContainer>();
                foreach (string file in Directory.EnumerateFiles(directory, "*.*", SearchOption.AllDirectories))
                {
                    FileInfo info = new FileInfo(file);
                    string size = info.Length.ToString();
                    string created = info.CreationTime.ToString();
                    string modified = info.LastWriteTime.ToString();
                    FileContainer container = new FileContainer(file, size, created, modified);
                    files.Add(container);
                }
                if (files.Count > 0)
                {
                    byte[] serialized = files.Serialize();
                    string checksum = ComputeHash(serialized, GetHashType(SettingsManager.ChecksumAlgorithm));
                    return checksum;
                }
            }
            return null;
        }

        #endregion
    }
}