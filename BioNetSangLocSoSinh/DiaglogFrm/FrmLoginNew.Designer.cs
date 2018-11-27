namespace BioNetSangLocSoSinh.DiaglogFrm
{
    partial class FrmLoginNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLoginNew));
            this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.lblCopyright = new DevExpress.XtraEditors.LabelControl();
            this.txtUsername = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.txtPassword = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.pictureEdit3 = new DevExpress.XtraEditors.PictureEdit();
            this.pictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
            this.picLogo = new DevExpress.XtraEditors.PictureEdit();
            this.btnLogin = new Bunifu.Framework.UI.BunifuThinButton2();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblExit = new System.Windows.Forms.Label();
            this.lblHeader2 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblUpdate = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.lblCapNhat = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 5;
            this.bunifuElipse1.TargetControl = this;
            // 
            // lblCopyright
            // 
            this.lblCopyright.Appearance.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblCopyright.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lblCopyright.Appearance.Options.UseFont = true;
            this.lblCopyright.Appearance.Options.UseForeColor = true;
            this.lblCopyright.Location = new System.Drawing.Point(34, 397);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(244, 16);
            this.lblCopyright.TabIndex = 1;
            this.lblCopyright.Text = "©Copyright 2018 Bionet Việt Nam";
            // 
            // txtUsername
            // 
            this.txtUsername.BackColor = System.Drawing.Color.White;
            this.txtUsername.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI Historic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.txtUsername.HintForeColor = System.Drawing.Color.Empty;
            this.txtUsername.HintText = "Tên tài khoản";
            this.txtUsername.isPassword = false;
            this.txtUsername.LineFocusedColor = System.Drawing.Color.DeepSkyBlue;
            this.txtUsername.LineIdleColor = System.Drawing.Color.DeepSkyBlue;
            this.txtUsername.LineMouseHoverColor = System.Drawing.Color.DeepSkyBlue;
            this.txtUsername.LineThickness = 3;
            this.txtUsername.Location = new System.Drawing.Point(52, 192);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(4);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(224, 33);
            this.txtUsername.TabIndex = 1;
            this.txtUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtUsername.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUsername_KeyDown);
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.White;
            this.txtPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI Historic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.txtPassword.HintForeColor = System.Drawing.Color.Empty;
            this.txtPassword.HintText = "";
            this.txtPassword.ImeMode = System.Windows.Forms.ImeMode.On;
            this.txtPassword.isPassword = true;
            this.txtPassword.LineFocusedColor = System.Drawing.Color.DeepSkyBlue;
            this.txtPassword.LineIdleColor = System.Drawing.Color.DeepSkyBlue;
            this.txtPassword.LineMouseHoverColor = System.Drawing.Color.DeepSkyBlue;
            this.txtPassword.LineThickness = 3;
            this.txtPassword.Location = new System.Drawing.Point(52, 232);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(225, 33);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.Text = "123";
            this.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtPassword.Click += new System.EventHandler(this.txtPassword_Click);
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // pictureEdit3
            // 
            this.pictureEdit3.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureEdit3.EditValue = global::BioNetSangLocSoSinh.Properties.Resources.password;
            this.pictureEdit3.Location = new System.Drawing.Point(16, 235);
            this.pictureEdit3.Name = "pictureEdit3";
            this.pictureEdit3.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit3.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit3.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit3.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit3.Size = new System.Drawing.Size(39, 31);
            this.pictureEdit3.TabIndex = 6;
            // 
            // pictureEdit2
            // 
            this.pictureEdit2.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureEdit2.EditValue = global::BioNetSangLocSoSinh.Properties.Resources.login;
            this.pictureEdit2.Location = new System.Drawing.Point(16, 196);
            this.pictureEdit2.Name = "pictureEdit2";
            this.pictureEdit2.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit2.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit2.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit2.Size = new System.Drawing.Size(39, 31);
            this.pictureEdit2.TabIndex = 5;
            // 
            // picLogo
            // 
            this.picLogo.Cursor = System.Windows.Forms.Cursors.Default;
            this.picLogo.EditValue = global::BioNetSangLocSoSinh.Properties.Resources.logo;
            this.picLogo.Location = new System.Drawing.Point(54, 12);
            this.picLogo.Name = "picLogo";
            this.picLogo.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.picLogo.Properties.Appearance.Options.UseBackColor = true;
            this.picLogo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picLogo.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.picLogo.Size = new System.Drawing.Size(215, 90);
            this.picLogo.TabIndex = 0;
            // 
            // btnLogin
            // 
            this.btnLogin.ActiveBorderThickness = 1;
            this.btnLogin.ActiveCornerRadius = 20;
            this.btnLogin.ActiveFillColor = System.Drawing.Color.LightSkyBlue;
            this.btnLogin.ActiveForecolor = System.Drawing.Color.DarkOrchid;
            this.btnLogin.ActiveLineColor = System.Drawing.Color.LightSkyBlue;
            this.btnLogin.BackColor = System.Drawing.Color.White;
            this.btnLogin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLogin.BackgroundImage")));
            this.btnLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLogin.ButtonText = "Đăng Nhập";
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnLogin.IdleBorderThickness = 1;
            this.btnLogin.IdleCornerRadius = 20;
            this.btnLogin.IdleFillColor = System.Drawing.Color.White;
            this.btnLogin.IdleForecolor = System.Drawing.Color.DeepSkyBlue;
            this.btnLogin.IdleLineColor = System.Drawing.Color.DeepSkyBlue;
            this.btnLogin.Location = new System.Drawing.Point(44, 297);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(238, 44);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            this.btnLogin.Enter += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblHeader.Location = new System.Drawing.Point(107, 105);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(103, 25);
            this.lblHeader.TabIndex = 7;
            this.lblHeader.Text = "BIONET";
            // 
            // lblExit
            // 
            this.lblExit.AutoSize = true;
            this.lblExit.BackColor = System.Drawing.Color.Transparent;
            this.lblExit.ForeColor = System.Drawing.Color.SaddleBrown;
            this.lblExit.Location = new System.Drawing.Point(67, 345);
            this.lblExit.Name = "lblExit";
            this.lblExit.Size = new System.Drawing.Size(185, 13);
            this.lblExit.TabIndex = 4;
            this.lblExit.Text = "Bạn muốn thoát phần mềm? Thoát";
            this.lblExit.Click += new System.EventHandler(this.lblExit_Click);
            this.lblExit.MouseLeave += new System.EventHandler(this.lblExit_MouseLeave);
            this.lblExit.MouseHover += new System.EventHandler(this.lblExit_MouseHover);
            // 
            // lblHeader2
            // 
            this.lblHeader2.AutoSize = true;
            this.lblHeader2.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader2.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader2.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblHeader2.Location = new System.Drawing.Point(41, 130);
            this.lblHeader2.Name = "lblHeader2";
            this.lblHeader2.Size = new System.Drawing.Size(241, 25);
            this.lblHeader2.TabIndex = 8;
            this.lblHeader2.Text = "SÀNG LỌC SƠ SINH";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.ForeColor = System.Drawing.Color.SaddleBrown;
            this.lblVersion.Location = new System.Drawing.Point(43, 158);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(145, 13);
            this.lblVersion.TabIndex = 9;
            this.lblVersion.Text = "Phiên bản: Beta2.0.2018.07";
            // 
            // lblUpdate
            // 
            this.lblUpdate.AutoSize = true;
            this.lblUpdate.BackColor = System.Drawing.Color.Transparent;
            this.lblUpdate.ForeColor = System.Drawing.Color.SaddleBrown;
            this.lblUpdate.Location = new System.Drawing.Point(43, 178);
            this.lblUpdate.Name = "lblUpdate";
            this.lblUpdate.Size = new System.Drawing.Size(143, 13);
            this.lblUpdate.TabIndex = 10;
            this.lblUpdate.Text = "Ngày cập nhật: 05/07/2018";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.BackColor = System.Drawing.Color.Transparent;
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(44, 282);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(233, 20);
            this.lblError.TabIndex = 11;
            this.lblError.Text = "Vui lòng kiểm tra lại tài khoản hoặc mật khẩu.";
            this.lblError.UseCompatibleTextRendering = true;
            this.lblError.Visible = false;
            // 
            // lblCapNhat
            // 
            this.lblCapNhat.AutoSize = true;
            this.lblCapNhat.BackColor = System.Drawing.Color.Transparent;
            this.lblCapNhat.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCapNhat.ForeColor = System.Drawing.Color.Crimson;
            this.lblCapNhat.Location = new System.Drawing.Point(29, 381);
            this.lblCapNhat.Name = "lblCapNhat";
            this.lblCapNhat.Size = new System.Drawing.Size(253, 13);
            this.lblCapNhat.TabIndex = 12;
            this.lblCapNhat.Text = "Phiên bản dùng chính thức từ ngày 10/11/2018";
            // 
            // FrmLoginNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::BioNetSangLocSoSinh.Properties.Resources._8cda90f704c93bc4c3b4a0a9c188d061;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(322, 425);
            this.Controls.Add(this.lblCapNhat);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblUpdate);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblHeader2);
            this.Controls.Add(this.lblExit);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.pictureEdit3);
            this.Controls.Add(this.pictureEdit2);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.picLogo);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmLoginNew";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmLoginNew";
            this.Load += new System.EventHandler(this.FrmLoginNew_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bunifu.Framework.UI.BunifuElipse bunifuElipse1;
        private DevExpress.XtraEditors.PictureEdit picLogo;
        private Bunifu.Framework.UI.BunifuMaterialTextbox txtUsername;
        private DevExpress.XtraEditors.LabelControl lblCopyright;
        private Bunifu.Framework.UI.BunifuMaterialTextbox txtPassword;
        private DevExpress.XtraEditors.PictureEdit pictureEdit2;
        private DevExpress.XtraEditors.PictureEdit pictureEdit3;
        private System.Windows.Forms.Label lblExit;
        private Bunifu.Framework.UI.BunifuThinButton2 btnLogin;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblHeader2;
        private System.Windows.Forms.Label lblUpdate;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label lblCapNhat;
    }
}