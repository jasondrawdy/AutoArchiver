namespace AutoArchiver
{
    partial class frmAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            this.lblSoftware = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.sep1 = new MaterialSkin.Controls.MaterialDivider();
            this.btnCredits = new System.Windows.Forms.LinkLabel();
            this.sep2 = new MaterialSkin.Controls.MaterialDivider();
            this.pbMain = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbMain)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSoftware
            // 
            this.lblSoftware.AutoSize = true;
            this.lblSoftware.Location = new System.Drawing.Point(233, 226);
            this.lblSoftware.Name = "lblSoftware";
            this.lblSoftware.Size = new System.Drawing.Size(71, 13);
            this.lblSoftware.TabIndex = 1;
            this.lblSoftware.Text = "Auto Archiver";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(185, 239);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(161, 13);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "Version 1.0.0 - Venus (Build 100)";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Location = new System.Drawing.Point(166, 252);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(213, 13);
            this.lblCopyright.TabIndex = 2;
            this.lblCopyright.Text = "© 2019 Jason Drawdy. All Rights Reserved.";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(224, 308);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // sep1
            // 
            this.sep1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.sep1.Depth = 0;
            this.sep1.Location = new System.Drawing.Point(169, 273);
            this.sep1.MouseState = MaterialSkin.MouseState.HOVER;
            this.sep1.Name = "sep1";
            this.sep1.Size = new System.Drawing.Size(210, 1);
            this.sep1.TabIndex = 4;
            this.sep1.Text = "materialDivider1";
            // 
            // btnCredits
            // 
            this.btnCredits.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(105)))), ((int)(((byte)(200)))));
            this.btnCredits.AutoSize = true;
            this.btnCredits.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(146)))), ((int)(((byte)(255)))));
            this.btnCredits.Location = new System.Drawing.Point(249, 277);
            this.btnCredits.Name = "btnCredits";
            this.btnCredits.Size = new System.Drawing.Size(39, 13);
            this.btnCredits.TabIndex = 5;
            this.btnCredits.TabStop = true;
            this.btnCredits.Text = "Credits";
            this.btnCredits.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnCredits_LinkClicked);
            // 
            // sep2
            // 
            this.sep2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.sep2.Depth = 0;
            this.sep2.Location = new System.Drawing.Point(169, 293);
            this.sep2.MouseState = MaterialSkin.MouseState.HOVER;
            this.sep2.Name = "sep2";
            this.sep2.Size = new System.Drawing.Size(210, 1);
            this.sep2.TabIndex = 4;
            this.sep2.Text = "materialDivider1";
            // 
            // pbMain
            // 
            this.pbMain.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbMain.BackgroundImage")));
            this.pbMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbMain.Location = new System.Drawing.Point(189, 48);
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(166, 166);
            this.pbMain.TabIndex = 0;
            this.pbMain.TabStop = false;
            // 
            // frmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(544, 350);
            this.Controls.Add(this.btnCredits);
            this.Controls.Add(this.sep2);
            this.Controls.Add(this.sep1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblSoftware);
            this.Controls.Add(this.pbMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auto Archiver - Version 1.0.0";
            ((System.ComponentModel.ISupportInitialize)(this.pbMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbMain;
        private System.Windows.Forms.Label lblSoftware;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Button btnOk;
        private MaterialSkin.Controls.MaterialDivider sep1;
        private System.Windows.Forms.LinkLabel btnCredits;
        private MaterialSkin.Controls.MaterialDivider sep2;
    }
}