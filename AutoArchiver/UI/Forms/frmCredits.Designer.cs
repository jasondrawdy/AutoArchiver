namespace AutoArchiver
{
    partial class frmCredits
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCredits));
            this.lblCredits = new MonoFlat.MonoFlat_HeaderLabel();
            this.sep1 = new MaterialSkin.Controls.MaterialDivider();
            this.lblIcon = new MonoFlat.MonoFlat_HeaderLabel();
            this.lblIconArtist = new System.Windows.Forms.Label();
            this.btnIconLink = new System.Windows.Forms.LinkLabel();
            this.lblCompression = new MonoFlat.MonoFlat_HeaderLabel();
            this.lblCompressionAuthur = new System.Windows.Forms.Label();
            this.btnCompressionLink = new System.Windows.Forms.LinkLabel();
            this.lblObjectListView = new MonoFlat.MonoFlat_HeaderLabel();
            this.lblObjectListViewAuthur = new System.Windows.Forms.Label();
            this.btnObjectListViewLink = new System.Windows.Forms.LinkLabel();
            this.sep2 = new MaterialSkin.Controls.MaterialDivider();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblCredits
            // 
            this.lblCredits.AutoSize = true;
            this.lblCredits.BackColor = System.Drawing.Color.Transparent;
            this.lblCredits.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCredits.ForeColor = System.Drawing.Color.Black;
            this.lblCredits.Location = new System.Drawing.Point(193, 31);
            this.lblCredits.Name = "lblCredits";
            this.lblCredits.Size = new System.Drawing.Size(63, 21);
            this.lblCredits.TabIndex = 0;
            this.lblCredits.Text = "Credits";
            // 
            // sep1
            // 
            this.sep1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.sep1.Depth = 0;
            this.sep1.Location = new System.Drawing.Point(119, 55);
            this.sep1.MouseState = MaterialSkin.MouseState.HOVER;
            this.sep1.Name = "sep1";
            this.sep1.Size = new System.Drawing.Size(210, 1);
            this.sep1.TabIndex = 5;
            this.sep1.Text = "materialDivider1";
            // 
            // lblIcon
            // 
            this.lblIcon.AutoSize = true;
            this.lblIcon.BackColor = System.Drawing.Color.Transparent;
            this.lblIcon.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIcon.ForeColor = System.Drawing.Color.Black;
            this.lblIcon.Location = new System.Drawing.Point(178, 71);
            this.lblIcon.Name = "lblIcon";
            this.lblIcon.Size = new System.Drawing.Size(34, 15);
            this.lblIcon.TabIndex = 6;
            this.lblIcon.Text = "Icon:";
            // 
            // lblIconArtist
            // 
            this.lblIconArtist.AutoSize = true;
            this.lblIconArtist.Location = new System.Drawing.Point(218, 72);
            this.lblIconArtist.Name = "lblIconArtist";
            this.lblIconArtist.Size = new System.Drawing.Size(53, 13);
            this.lblIconArtist.TabIndex = 7;
            this.lblIconArtist.Text = "Ampeross";
            // 
            // btnIconLink
            // 
            this.btnIconLink.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(105)))), ((int)(((byte)(200)))));
            this.btnIconLink.AutoSize = true;
            this.btnIconLink.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(146)))), ((int)(((byte)(255)))));
            this.btnIconLink.Location = new System.Drawing.Point(144, 86);
            this.btnIconLink.Name = "btnIconLink";
            this.btnIconLink.Size = new System.Drawing.Size(161, 13);
            this.btnIconLink.TabIndex = 8;
            this.btnIconLink.TabStop = true;
            this.btnIconLink.Text = "https://ampeross.deviantart.com";
            this.btnIconLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnIconLink_LinkClicked);
            // 
            // lblCompression
            // 
            this.lblCompression.AutoSize = true;
            this.lblCompression.BackColor = System.Drawing.Color.Transparent;
            this.lblCompression.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompression.ForeColor = System.Drawing.Color.Black;
            this.lblCompression.Location = new System.Drawing.Point(148, 108);
            this.lblCompression.Name = "lblCompression";
            this.lblCompression.Size = new System.Drawing.Size(81, 15);
            this.lblCompression.TabIndex = 6;
            this.lblCompression.Text = "Compression:";
            // 
            // lblCompressionAuthur
            // 
            this.lblCompressionAuthur.AutoSize = true;
            this.lblCompressionAuthur.Location = new System.Drawing.Point(235, 109);
            this.lblCompressionAuthur.Name = "lblCompressionAuthur";
            this.lblCompressionAuthur.Size = new System.Drawing.Size(65, 13);
            this.lblCompressionAuthur.TabIndex = 7;
            this.lblCompressionAuthur.Text = "icsharpcode";
            // 
            // btnCompressionLink
            // 
            this.btnCompressionLink.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(105)))), ((int)(((byte)(200)))));
            this.btnCompressionLink.AutoSize = true;
            this.btnCompressionLink.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(146)))), ((int)(((byte)(255)))));
            this.btnCompressionLink.Location = new System.Drawing.Point(114, 123);
            this.btnCompressionLink.Name = "btnCompressionLink";
            this.btnCompressionLink.Size = new System.Drawing.Size(220, 13);
            this.btnCompressionLink.TabIndex = 8;
            this.btnCompressionLink.TabStop = true;
            this.btnCompressionLink.Text = "https://github.com/icsharpcode/SharpZipLib";
            this.btnCompressionLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnCompressionLink_LinkClicked);
            // 
            // lblObjectListView
            // 
            this.lblObjectListView.AutoSize = true;
            this.lblObjectListView.BackColor = System.Drawing.Color.Transparent;
            this.lblObjectListView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObjectListView.ForeColor = System.Drawing.Color.Black;
            this.lblObjectListView.Location = new System.Drawing.Point(144, 145);
            this.lblObjectListView.Name = "lblObjectListView";
            this.lblObjectListView.Size = new System.Drawing.Size(94, 15);
            this.lblObjectListView.TabIndex = 6;
            this.lblObjectListView.Text = "ObjectListView:";
            // 
            // lblObjectListViewAuthur
            // 
            this.lblObjectListViewAuthur.AutoSize = true;
            this.lblObjectListViewAuthur.Location = new System.Drawing.Point(244, 146);
            this.lblObjectListViewAuthur.Name = "lblObjectListViewAuthur";
            this.lblObjectListViewAuthur.Size = new System.Drawing.Size(61, 13);
            this.lblObjectListViewAuthur.TabIndex = 7;
            this.lblObjectListViewAuthur.Text = "grammarian";
            // 
            // btnObjectListViewLink
            // 
            this.btnObjectListViewLink.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(105)))), ((int)(((byte)(200)))));
            this.btnObjectListViewLink.AutoSize = true;
            this.btnObjectListViewLink.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(146)))), ((int)(((byte)(255)))));
            this.btnObjectListViewLink.Location = new System.Drawing.Point(111, 160);
            this.btnObjectListViewLink.Name = "btnObjectListViewLink";
            this.btnObjectListViewLink.Size = new System.Drawing.Size(227, 13);
            this.btnObjectListViewLink.TabIndex = 8;
            this.btnObjectListViewLink.TabStop = true;
            this.btnObjectListViewLink.Text = "https://sourceforge.net/projects/objectlistview";
            this.btnObjectListViewLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnObjectListViewLink_LinkClicked);
            // 
            // sep2
            // 
            this.sep2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.sep2.Depth = 0;
            this.sep2.Location = new System.Drawing.Point(114, 193);
            this.sep2.MouseState = MaterialSkin.MouseState.HOVER;
            this.sep2.Name = "sep2";
            this.sep2.Size = new System.Drawing.Size(210, 1);
            this.sep2.TabIndex = 5;
            this.sep2.Text = "materialDivider1";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(179, 212);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmCredits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(449, 256);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnObjectListViewLink);
            this.Controls.Add(this.btnCompressionLink);
            this.Controls.Add(this.btnIconLink);
            this.Controls.Add(this.lblObjectListViewAuthur);
            this.Controls.Add(this.lblCompressionAuthur);
            this.Controls.Add(this.lblIconArtist);
            this.Controls.Add(this.lblObjectListView);
            this.Controls.Add(this.lblCompression);
            this.Controls.Add(this.lblIcon);
            this.Controls.Add(this.sep2);
            this.Controls.Add(this.sep1);
            this.Controls.Add(this.lblCredits);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCredits";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auto Archiver - Version 1.0.0";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MonoFlat.MonoFlat_HeaderLabel lblCredits;
        private MaterialSkin.Controls.MaterialDivider sep1;
        private MonoFlat.MonoFlat_HeaderLabel lblIcon;
        private System.Windows.Forms.Label lblIconArtist;
        private System.Windows.Forms.LinkLabel btnIconLink;
        private MonoFlat.MonoFlat_HeaderLabel lblCompression;
        private System.Windows.Forms.Label lblCompressionAuthur;
        private System.Windows.Forms.LinkLabel btnCompressionLink;
        private MonoFlat.MonoFlat_HeaderLabel lblObjectListView;
        private System.Windows.Forms.Label lblObjectListViewAuthur;
        private System.Windows.Forms.LinkLabel btnObjectListViewLink;
        private MaterialSkin.Controls.MaterialDivider sep2;
        private System.Windows.Forms.Button btnOk;
    }
}