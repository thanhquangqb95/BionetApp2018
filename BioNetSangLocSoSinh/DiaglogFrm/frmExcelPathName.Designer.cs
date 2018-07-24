namespace BioNetSangLocSoSinh.DiaglogFrm
{
    partial class frmExcelPathName
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExcelPathName));
            this.btAccept = new DevExpress.XtraEditors.SimpleButton();
            this.btClose = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.butPath = new DevExpress.XtraEditors.SimpleButton();
            this.txtPath = new DevExpress.XtraEditors.TextEdit();
            this.lbUsername = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPath.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btAccept
            // 
            this.btAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btAccept.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.btAccept.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btAccept.ImageOptions.Image")));
            this.btAccept.Location = new System.Drawing.Point(286, 72);
            this.btAccept.Name = "btAccept";
            this.btAccept.Size = new System.Drawing.Size(82, 25);
            this.btAccept.TabIndex = 3;
            this.btAccept.Text = "Xuất File";
            this.btAccept.Click += new System.EventHandler(this.btAccept_Click);
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btClose.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.btClose.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btClose.ImageOptions.Image")));
            this.btClose.Location = new System.Drawing.Point(368, 72);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(82, 25);
            this.btClose.TabIndex = 4;
            this.btClose.Text = "Thoát";
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("groupControl1.CaptionImageOptions.Image")));
            this.groupControl1.Controls.Add(this.butPath);
            this.groupControl1.Controls.Add(this.txtPath);
            this.groupControl1.Controls.Add(this.lbUsername);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.btClose);
            this.groupControl1.Controls.Add(this.btAccept);
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(460, 134);
            this.groupControl1.TabIndex = 1016;
            this.groupControl1.Text = "Chọn đường dẫn";
            // 
            // butPath
            // 
            this.butPath.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.butPath.Appearance.Options.UseForeColor = true;
            this.butPath.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.butPath.Location = new System.Drawing.Point(424, 46);
            this.butPath.Name = "butPath";
            this.butPath.Size = new System.Drawing.Size(26, 20);
            this.butPath.TabIndex = 1019;
            this.butPath.Text = "...";
            this.butPath.Click += new System.EventHandler(this.butPath_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(82, 45);
            this.txtPath.Name = "txtPath";
            this.txtPath.Properties.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(336, 20);
            this.txtPath.TabIndex = 1018;
            // 
            // lbUsername
            // 
            this.lbUsername.Location = new System.Drawing.Point(18, 49);
            this.lbUsername.Name = "lbUsername";
            this.lbUsername.Size = new System.Drawing.Size(61, 13);
            this.lbUsername.TabIndex = 1017;
            this.lbUsername.Text = "Đường dẫn :";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(349, 115);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(91, 13);
            this.labelControl1.TabIndex = 1016;
            this.labelControl1.Text = "Powersoft Co., Ltd";
            // 
            // frmExcelPathName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 136);
            this.Controls.Add(this.groupControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmExcelPathName";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đường dẫn lưu trữ";
            this.Load += new System.EventHandler(this.frmExcelPathName_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPath.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btAccept;
        private DevExpress.XtraEditors.SimpleButton btClose;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton butPath;
        private DevExpress.XtraEditors.TextEdit txtPath;
        private DevExpress.XtraEditors.LabelControl lbUsername;

    }
}