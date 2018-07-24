namespace BioNetSangLocSoSinh.DiaglogFrm
{
    partial class FrmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            this.grMain = new DevExpress.XtraEditors.GroupControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.butCANCEL = new DevExpress.XtraEditors.SimpleButton();
            this.butOK = new DevExpress.XtraEditors.SimpleButton();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.txtUsername = new DevExpress.XtraEditors.TextEdit();
            this.lbVersion = new DevExpress.XtraEditors.LabelControl();
            this.lblNgayCapNhat = new DevExpress.XtraEditors.LabelControl();
            this.lbPassword = new DevExpress.XtraEditors.LabelControl();
            this.lbUsername = new DevExpress.XtraEditors.LabelControl();
            this.psLogo = new System.Windows.Forms.PictureBox();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.grMain)).BeginInit();
            this.grMain.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.psLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // grMain
            // 
            this.grMain.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.grMain.Appearance.Options.UseBackColor = true;
            this.grMain.AppearanceCaption.BackColor = System.Drawing.Color.Transparent;
            this.grMain.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grMain.AppearanceCaption.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.grMain.AppearanceCaption.Options.UseBackColor = true;
            this.grMain.AppearanceCaption.Options.UseFont = true;
            this.grMain.AppearanceCaption.Options.UseForeColor = true;
            this.grMain.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.grMain.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("grMain.CaptionImageOptions.Image")));
            this.grMain.Controls.Add(this.panel1);
            this.grMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grMain.Location = new System.Drawing.Point(0, 0);
            this.grMain.Name = "grMain";
            this.grMain.Size = new System.Drawing.Size(424, 152);
            this.grMain.TabIndex = 101;
            this.grMain.Text = "iHIS - Bệnh viện điện tử (POWERSOFT JSC) - Đăng Nhập";
            this.grMain.Click += new System.EventHandler(this.frmLogin_Load);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Controls.Add(this.butCANCEL);
            this.panel1.Controls.Add(this.butOK);
            this.panel1.Controls.Add(this.txtPassword);
            this.panel1.Controls.Add(this.txtUsername);
            this.panel1.Controls.Add(this.lbVersion);
            this.panel1.Controls.Add(this.lblNgayCapNhat);
            this.panel1.Controls.Add(this.lbPassword);
            this.panel1.Controls.Add(this.lbUsername);
            this.panel1.Controls.Add(this.psLogo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(424, 152);
            this.panel1.TabIndex = 0;
            // 
            // butCANCEL
            // 
            this.butCANCEL.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.butCANCEL.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("butCANCEL.ImageOptions.Image")));
            this.butCANCEL.Location = new System.Drawing.Point(320, 107);
            this.butCANCEL.Name = "butCANCEL";
            this.butCANCEL.Size = new System.Drawing.Size(88, 30);
            this.butCANCEL.TabIndex = 112;
            this.butCANCEL.Text = "Hủy bỏ";
            this.butCANCEL.Click += new System.EventHandler(this.butCANCEL_Click);
            // 
            // butOK
            // 
            this.butOK.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.butOK.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("butOK.ImageOptions.Image")));
            this.butOK.Location = new System.Drawing.Point(224, 107);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(94, 30);
            this.butOK.TabIndex = 111;
            this.butOK.Text = "Đồng ý";
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(224, 75);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtPassword.Properties.MaxLength = 50;
            this.txtPassword.Properties.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(184, 20);
            this.txtPassword.TabIndex = 110;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(224, 52);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtUsername.Properties.MaxLength = 50;
            this.txtUsername.Size = new System.Drawing.Size(184, 20);
            this.txtUsername.TabIndex = 109;
            this.txtUsername.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUsername_KeyDown);
            // 
            // lbVersion
            // 
            this.lbVersion.Appearance.ForeColor = System.Drawing.Color.Green;
            this.lbVersion.Appearance.Options.UseForeColor = true;
            this.lbVersion.Location = new System.Drawing.Point(283, 33);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(35, 13);
            this.lbVersion.TabIndex = 113;
            this.lbVersion.Text = "Version";
            // 
            // lblNgayCapNhat
            // 
            this.lblNgayCapNhat.Appearance.ForeColor = System.Drawing.Color.Green;
            this.lblNgayCapNhat.Appearance.Options.UseForeColor = true;
            this.lblNgayCapNhat.Location = new System.Drawing.Point(142, 32);
            this.lblNgayCapNhat.Name = "lblNgayCapNhat";
            this.lblNgayCapNhat.Size = new System.Drawing.Size(80, 13);
            this.lblNgayCapNhat.TabIndex = 114;
            this.lblNgayCapNhat.Text = "Ngày cập nhật - ";
            // 
            // lbPassword
            // 
            this.lbPassword.Location = new System.Drawing.Point(171, 77);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(51, 13);
            this.lbPassword.TabIndex = 115;
            this.lbPassword.Text = "Mật khẩu :";
            // 
            // lbUsername
            // 
            this.lbUsername.Location = new System.Drawing.Point(144, 54);
            this.lbUsername.Name = "lbUsername";
            this.lbUsername.Size = new System.Drawing.Size(79, 13);
            this.lbUsername.TabIndex = 116;
            this.lbUsername.Text = "Tên đăng nhập :";
            // 
            // psLogo
            // 
            this.psLogo.Image = ((System.Drawing.Image)(resources.GetObject("psLogo.Image")));
            this.psLogo.Location = new System.Drawing.Point(15, 3);
            this.psLogo.Name = "psLogo";
            this.psLogo.Size = new System.Drawing.Size(121, 145);
            this.psLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.psLogo.TabIndex = 108;
            this.psLogo.TabStop = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Green;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(227, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(104, 25);
            this.labelControl1.TabIndex = 117;
            this.labelControl1.Text = "Đăng nhập";
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 152);
            this.Controls.Add(this.grMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grMain)).EndInit();
            this.grMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.psLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl grMain;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton butCANCEL;
        private DevExpress.XtraEditors.SimpleButton butOK;
        private DevExpress.XtraEditors.TextEdit txtPassword;
        private DevExpress.XtraEditors.TextEdit txtUsername;
        public DevExpress.XtraEditors.LabelControl lbVersion;
        private DevExpress.XtraEditors.LabelControl lblNgayCapNhat;
        private DevExpress.XtraEditors.LabelControl lbPassword;
        private DevExpress.XtraEditors.LabelControl lbUsername;
        private System.Windows.Forms.PictureBox psLogo;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}