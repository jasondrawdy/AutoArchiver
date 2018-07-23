/*
==============================================================================
Copyright © Jason Tanner (Antebyte)

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
using System.Windows.Forms;
using AutoArchiver.API.Tools;
using Microsoft.Win32;
using NotificationType = MonoFlat.MonoFlat_NotificationBox.Type;

#endregion
namespace AutoArchiver
{
    public partial class frmSettings : Form
    {
        #region Variables

        private frmMain Main { get; set; }

        #endregion
        #region Controls

        public frmSettings(frmMain form)
        {
            InitializeComponent();
            Main = form;
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            SetControls();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SetSettings();
            if (SettingsManager.SaveSettings())
                Main.DisplayNotification(NotificationType.Success, "Your settings have been saved!", 5000);
            else
                Main.DisplayNotification(NotificationType.Error, "There was an issue saving your settings.", 5000);
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
        #region Methods

        private void SetControls()
        {
            // Load our settings from our file.
            SettingsManager.LoadSettings(); // Reload settings since we might not be reloading the application.
            
            // Set our controls accordingly.
            cmbCompressionLevel.SelectedIndex = SettingsManager.CompressionLevel;
            cmbChecksumAlgorithm.SelectedIndex = SettingsManager.ChecksumAlgorithm;
            chkAddToStartup.Checked = SettingsManager.AddToStartup;
            chkStartArchivingOnStartup.Checked = SettingsManager.ArchiveOnStartup;
            chkAskBeforeBackup.Checked = SettingsManager.AskBeforeBackup;
        }

        private void SetSettings()
        {
            // Set our settings from our controls.
            SettingsManager.CompressionLevel = cmbCompressionLevel.SelectedIndex;
            SettingsManager.ChecksumAlgorithm = cmbChecksumAlgorithm.SelectedIndex;
            SettingsManager.AddToStartup = chkAddToStartup.Checked;
            SettingsManager.ArchiveOnStartup = chkStartArchivingOnStartup.Checked;
            SettingsManager.AskBeforeBackup = chkAskBeforeBackup.Checked;

            // Set our settings from our main form.
            Main.SetSettings();

            // Add any registry entries if necessary.
            if (chkAddToStartup.Checked)
                AddToStartup();
            else
                RemoveFromStartup();
        }

        private bool AddToStartup()
        {
            try
            {
                // Add registry entry to run app on startup.
                string startpath = Application.StartupPath + @"\" + Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".exe";
                RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                key.SetValue("AutoArchiver", startpath);
                return true;
            }
            catch { return false; }
        }

        private bool RemoveFromStartup()
        {
            try
            {
                // Remove registry entry to disable running on startup.
                RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                key.DeleteValue("AutoArchiver", true);
                return true;
            }
            catch { return false; }
        }
        #endregion
    }
}
