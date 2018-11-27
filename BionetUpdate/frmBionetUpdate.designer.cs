namespace BionetUpdate
{
    partial class FrmBionetUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBionetUpdate));
            this.label1 = new System.Windows.Forms.Label();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.timer1 = new System.Windows.Forms.Timer();
            this.btnSync = new DevExpress.XtraEditors.SimpleButton();
            this.progressBarControlDownload = new DevExpress.XtraEditors.ProgressBarControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lblCopyLink = new DevExpress.XtraEditors.LabelControl();
            this.lblFileCopy = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControlDownload.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkRed;
            this.label1.Location = new System.Drawing.Point(53, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(390, 22);
            this.label1.TabIndex = 19;
            this.label1.Text = "Cập nhật phiên bản BionetSangLocSoSinh";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.DarkRed;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.labelControl1.Location = new System.Drawing.Point(139, 149);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(191, 13);
            this.labelControl1.TabIndex = 16;
            this.labelControl1.Text = "Copyright © 2018 Bionet Việt Nam";
            // 
            // btnSync
            // 
            this.btnSync.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnSync.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.btnSync.Appearance.Options.UseFont = true;
            this.btnSync.Appearance.Options.UseForeColor = true;
            this.btnSync.ImageOptions.Image = global::BionetUpdate.Properties.Resources.recycle;
            this.btnSync.Location = new System.Drawing.Point(70, 117);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(147, 26);
            this.btnSync.TabIndex = 22;
            this.btnSync.Text = "Cập nhật";
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // progressBarControlDownload
            // 
            this.progressBarControlDownload.Location = new System.Drawing.Point(12, 78);
            this.progressBarControlDownload.Name = "progressBarControlDownload";
            this.progressBarControlDownload.Size = new System.Drawing.Size(471, 30);
            this.progressBarControlDownload.TabIndex = 23;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.simpleButton1.Appearance.ForeColor = System.Drawing.Color.Firebrick;
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Appearance.Options.UseForeColor = true;
            this.simpleButton1.ImageOptions.Image = global::BionetUpdate.Properties.Resources.x_button;
            this.simpleButton1.Location = new System.Drawing.Point(260, 117);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(147, 26);
            this.simpleButton1.TabIndex = 24;
            this.simpleButton1.Text = "Hủy";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.DimGray;
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.labelControl2.Location = new System.Drawing.Point(70, 37);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(67, 13);
            this.labelControl2.TabIndex = 25;
            this.labelControl2.Text = "Copy file từ:";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.DimGray;
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Appearance.Options.UseForeColor = true;
            this.labelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.labelControl3.Location = new System.Drawing.Point(70, 59);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(52, 13);
            this.labelControl3.TabIndex = 26;
            this.labelControl3.Text = "File copy:";
            // 
            // lblCopyLink
            // 
            this.lblCopyLink.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCopyLink.Appearance.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblCopyLink.Appearance.Options.UseFont = true;
            this.lblCopyLink.Appearance.Options.UseForeColor = true;
            this.lblCopyLink.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.lblCopyLink.Location = new System.Drawing.Point(143, 37);
            this.lblCopyLink.Name = "lblCopyLink";
            this.lblCopyLink.Size = new System.Drawing.Size(43, 13);
            this.lblCopyLink.TabIndex = 27;
            this.lblCopyLink.Text = "Link file";
            // 
            // lblFileCopy
            // 
            this.lblFileCopy.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileCopy.Appearance.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblFileCopy.Appearance.Options.UseFont = true;
            this.lblFileCopy.Appearance.Options.UseForeColor = true;
            this.lblFileCopy.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.lblFileCopy.Location = new System.Drawing.Point(128, 59);
            this.lblFileCopy.Name = "lblFileCopy";
            this.lblFileCopy.Size = new System.Drawing.Size(43, 13);
            this.lblFileCopy.TabIndex = 28;
            this.lblFileCopy.Text = "Link file";
            // 
            // FrmBionetUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.BackgroundImage = global::BionetUpdate.Properties.Resources._6478051_abstract_background;
            this.ClientSize = new System.Drawing.Size(497, 166);
            this.Controls.Add(this.lblFileCopy);
            this.Controls.Add(this.lblCopyLink);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.progressBarControlDownload);
            this.Controls.Add(this.btnSync);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmBionetUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cập nhật phiên bản";
            this.Load += new System.EventHandler(this.FrmBionetUpdate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControlDownload.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.SimpleButton btnSync;
        private DevExpress.XtraEditors.ProgressBarControl progressBarControlDownload;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl lblCopyLink;
        private DevExpress.XtraEditors.LabelControl lblFileCopy;
    }
}

