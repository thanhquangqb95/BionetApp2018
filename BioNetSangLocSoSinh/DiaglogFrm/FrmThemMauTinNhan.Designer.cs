namespace BioNetSangLocSoSinh.DiaglogFrm
{
    partial class FrmThemMauTinNhan
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
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.txtNameMauSMS = new DevExpress.XtraEditors.TextEdit();
            this.lblSKTDemoSMS = new System.Windows.Forms.Label();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.txtAdDemo = new DevExpress.XtraEditors.MemoEdit();
            this.lblSKTSMS = new System.Windows.Forms.Label();
            this.cbbNoiDungSMS = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.cbbDoiTuongSMS = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.labelControl16 = new DevExpress.XtraEditors.LabelControl();
            this.cbbHinhThucSMS = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.txtCTNoiDungSMS = new DevExpress.XtraEditors.MemoEdit();
            this.btnHuy = new DevExpress.XtraEditors.SimpleButton();
            this.btnLuu = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl22 = new DevExpress.XtraEditors.LabelControl();
            this.cbbKieukitu = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.PanelDanhSachKiTu = new DevExpress.XtraEditors.GroupControl();
            this.labelControl19 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtNameMauSMS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAdDemo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbNoiDungSMS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbDoiTuongSMS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbHinhThucSMS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCTNoiDungSMS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbKieukitu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelDanhSachKiTu)).BeginInit();
            this.PanelDanhSachKiTu.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelControl12
            // 
            this.labelControl12.Location = new System.Drawing.Point(12, 31);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(64, 13);
            this.labelControl12.TabIndex = 1098;
            this.labelControl12.Text = "Tên mẫu SMS";
            // 
            // txtNameMauSMS
            // 
            this.txtNameMauSMS.Location = new System.Drawing.Point(82, 27);
            this.txtNameMauSMS.Name = "txtNameMauSMS";
            this.txtNameMauSMS.Size = new System.Drawing.Size(205, 20);
            this.txtNameMauSMS.TabIndex = 1097;
            // 
            // lblSKTDemoSMS
            // 
            this.lblSKTDemoSMS.AutoSize = true;
            this.lblSKTDemoSMS.ForeColor = System.Drawing.Color.Maroon;
            this.lblSKTDemoSMS.Location = new System.Drawing.Point(246, 255);
            this.lblSKTDemoSMS.Name = "lblSKTDemoSMS";
            this.lblSKTDemoSMS.Size = new System.Drawing.Size(35, 13);
            this.lblSKTDemoSMS.TabIndex = 1096;
            this.lblSKTDemoSMS.Text = "0/160";
            // 
            // labelControl14
            // 
            this.labelControl14.Location = new System.Drawing.Point(12, 255);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(177, 13);
            this.labelControl14.TabIndex = 1095;
            this.labelControl14.Text = "Demo chi tiết nội dung tin nhắn gửi đi";
            // 
            // txtAdDemo
            // 
            this.txtAdDemo.Location = new System.Drawing.Point(12, 274);
            this.txtAdDemo.Name = "txtAdDemo";
            this.txtAdDemo.Size = new System.Drawing.Size(478, 98);
            this.txtAdDemo.TabIndex = 1094;
            // 
            // lblSKTSMS
            // 
            this.lblSKTSMS.AutoSize = true;
            this.lblSKTSMS.ForeColor = System.Drawing.Color.Maroon;
            this.lblSKTSMS.Location = new System.Drawing.Point(246, 130);
            this.lblSKTSMS.Name = "lblSKTSMS";
            this.lblSKTSMS.Size = new System.Drawing.Size(35, 13);
            this.lblSKTSMS.TabIndex = 1093;
            this.lblSKTSMS.Text = "0/160";
            // 
            // cbbNoiDungSMS
            // 
            this.cbbNoiDungSMS.Location = new System.Drawing.Point(136, 102);
            this.cbbNoiDungSMS.Name = "cbbNoiDungSMS";
            this.cbbNoiDungSMS.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbNoiDungSMS.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Kết quả XN", "result", -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Trạng thái phiếu", "status", -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Công nợ", "money", -1)});
            this.cbbNoiDungSMS.Size = new System.Drawing.Size(151, 20);
            this.cbbNoiDungSMS.TabIndex = 1090;
            // 
            // labelControl15
            // 
            this.labelControl15.Location = new System.Drawing.Point(12, 109);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(102, 13);
            this.labelControl15.TabIndex = 1089;
            this.labelControl15.Text = "Nội dung gửi tin nhắn";
            // 
            // cbbDoiTuongSMS
            // 
            this.cbbDoiTuongSMS.Location = new System.Drawing.Point(136, 76);
            this.cbbDoiTuongSMS.Name = "cbbDoiTuongSMS";
            this.cbbDoiTuongSMS.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbDoiTuongSMS.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Bệnh nhân", "patient", -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Đơn vị cơ sở", "unit", -1)});
            this.cbbDoiTuongSMS.Size = new System.Drawing.Size(151, 20);
            this.cbbDoiTuongSMS.TabIndex = 1088;
            // 
            // labelControl16
            // 
            this.labelControl16.Location = new System.Drawing.Point(12, 83);
            this.labelControl16.Name = "labelControl16";
            this.labelControl16.Size = new System.Drawing.Size(117, 13);
            this.labelControl16.TabIndex = 1087;
            this.labelControl16.Text = "Đối tượng nhận tin nhắn";
            // 
            // cbbHinhThucSMS
            // 
            this.cbbHinhThucSMS.Location = new System.Drawing.Point(136, 51);
            this.cbbHinhThucSMS.Name = "cbbHinhThucSMS";
            this.cbbHinhThucSMS.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbHinhThucSMS.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Tin nhắn SMS", "sms", -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Tin nhắn Email", "email", -1)});
            this.cbbHinhThucSMS.Size = new System.Drawing.Size(151, 20);
            this.cbbHinhThucSMS.TabIndex = 1086;
            // 
            // labelControl17
            // 
            this.labelControl17.Location = new System.Drawing.Point(12, 58);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(106, 13);
            this.labelControl17.TabIndex = 1085;
            this.labelControl17.Text = "Hình thức gửi tin nhắn";
            // 
            // labelControl13
            // 
            this.labelControl13.Location = new System.Drawing.Point(12, 128);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(120, 13);
            this.labelControl13.TabIndex = 1084;
            this.labelControl13.Text = "Chi tiết nội dung tin nhắn";
            // 
            // txtCTNoiDungSMS
            // 
            this.txtCTNoiDungSMS.Location = new System.Drawing.Point(12, 148);
            this.txtCTNoiDungSMS.Name = "txtCTNoiDungSMS";
            this.txtCTNoiDungSMS.Size = new System.Drawing.Size(478, 98);
            this.txtCTNoiDungSMS.TabIndex = 1083;
            this.txtCTNoiDungSMS.EditValueChanged += new System.EventHandler(this.txtCTNoiDungSMS_EditValueChanged);
            // 
            // btnHuy
            // 
            this.btnHuy.ImageOptions.Image = global::BioNetSangLocSoSinh.Properties.Resources.delete__1_;
            this.btnHuy.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnHuy.ImageOptions.ImageToTextIndent = 5;
            this.btnHuy.Location = new System.Drawing.Point(249, 378);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(78, 22);
            this.btnHuy.TabIndex = 1092;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.ImageOptions.Image = global::BioNetSangLocSoSinh.Properties.Resources.save;
            this.btnLuu.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnLuu.ImageOptions.ImageToTextIndent = 5;
            this.btnLuu.Location = new System.Drawing.Point(120, 378);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(87, 22);
            this.btnLuu.TabIndex = 1091;
            this.btnLuu.Text = "Lưu";
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(184, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(134, 17);
            this.labelControl1.TabIndex = 1099;
            this.labelControl1.Text = "Thêm mẫu tin nhắn";
            // 
            // labelControl22
            // 
            this.labelControl22.Location = new System.Drawing.Point(311, 252);
            this.labelControl22.Name = "labelControl22";
            this.labelControl22.Size = new System.Drawing.Size(44, 13);
            this.labelControl22.TabIndex = 1101;
            this.labelControl22.Text = "Kiểu kí tự";
            // 
            // cbbKieukitu
            // 
            this.cbbKieukitu.Location = new System.Drawing.Point(361, 249);
            this.cbbKieukitu.Name = "cbbKieukitu";
            this.cbbKieukitu.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbKieukitu.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Không dấu", false, -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Có dấu", true, -1)});
            this.cbbKieukitu.Size = new System.Drawing.Size(129, 20);
            this.cbbKieukitu.TabIndex = 1100;
            this.cbbKieukitu.EditValueChanged += new System.EventHandler(this.cbbKieukitu_EditValueChanged);
            // 
            // PanelDanhSachKiTu
            // 
            this.PanelDanhSachKiTu.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.PanelDanhSachKiTu.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.PanelDanhSachKiTu.AppearanceCaption.Options.UseFont = true;
            this.PanelDanhSachKiTu.AppearanceCaption.Options.UseForeColor = true;
            this.PanelDanhSachKiTu.Controls.Add(this.labelControl19);
            this.PanelDanhSachKiTu.Location = new System.Drawing.Point(293, 27);
            this.PanelDanhSachKiTu.LookAndFeel.SkinName = "Office 2007 Green";
            this.PanelDanhSachKiTu.LookAndFeel.UseDefaultLookAndFeel = false;
            this.PanelDanhSachKiTu.Name = "PanelDanhSachKiTu";
            this.PanelDanhSachKiTu.Size = new System.Drawing.Size(197, 114);
            this.PanelDanhSachKiTu.TabIndex = 1102;
            this.PanelDanhSachKiTu.Text = "Danh sách kí tự";
            // 
            // labelControl19
            // 
            this.labelControl19.Appearance.Options.UseTextOptions = true;
            this.labelControl19.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.labelControl19.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl19.Location = new System.Drawing.Point(5, 24);
            this.labelControl19.Name = "labelControl19";
            this.labelControl19.Size = new System.Drawing.Size(181, 90);
            this.labelControl19.TabIndex = 1072;
            this.labelControl19.Text = "#maphieu=mã phiếu #trangthaiphieu=trạng thái phiếu #tentre=Tên trẻ               " +
    "    #tennguoinhan=tên người nhận #ngaysinh=ngày sinh trẻ #ketqua=kết quả #ketlua" +
    "n=kết luận";
            // 
            // FrmThemMauTinNhan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 404);
            this.ControlBox = false;
            this.Controls.Add(this.PanelDanhSachKiTu);
            this.Controls.Add(this.labelControl22);
            this.Controls.Add(this.cbbKieukitu);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl12);
            this.Controls.Add(this.txtNameMauSMS);
            this.Controls.Add(this.lblSKTDemoSMS);
            this.Controls.Add(this.labelControl14);
            this.Controls.Add(this.txtAdDemo);
            this.Controls.Add(this.lblSKTSMS);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.cbbNoiDungSMS);
            this.Controls.Add(this.labelControl15);
            this.Controls.Add(this.cbbDoiTuongSMS);
            this.Controls.Add(this.labelControl16);
            this.Controls.Add(this.cbbHinhThucSMS);
            this.Controls.Add(this.labelControl17);
            this.Controls.Add(this.labelControl13);
            this.Controls.Add(this.txtCTNoiDungSMS);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Glow;
            this.LookAndFeel.SkinMaskColor = System.Drawing.Color.Silver;
            this.LookAndFeel.SkinMaskColor2 = System.Drawing.Color.Silver;
            this.LookAndFeel.SkinName = "Office 2007 Black";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "FrmThemMauTinNhan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.txtNameMauSMS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAdDemo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbNoiDungSMS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbDoiTuongSMS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbHinhThucSMS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCTNoiDungSMS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbKieukitu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelDanhSachKiTu)).EndInit();
            this.PanelDanhSachKiTu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.TextEdit txtNameMauSMS;
        private System.Windows.Forms.Label lblSKTDemoSMS;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.MemoEdit txtAdDemo;
        private System.Windows.Forms.Label lblSKTSMS;
        private DevExpress.XtraEditors.SimpleButton btnHuy;
        private DevExpress.XtraEditors.SimpleButton btnLuu;
        private DevExpress.XtraEditors.ImageComboBoxEdit cbbNoiDungSMS;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private DevExpress.XtraEditors.ImageComboBoxEdit cbbDoiTuongSMS;
        private DevExpress.XtraEditors.LabelControl labelControl16;
        private DevExpress.XtraEditors.ImageComboBoxEdit cbbHinhThucSMS;
        private DevExpress.XtraEditors.LabelControl labelControl17;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.MemoEdit txtCTNoiDungSMS;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl22;
        private DevExpress.XtraEditors.ImageComboBoxEdit cbbKieukitu;
        private DevExpress.XtraEditors.GroupControl PanelDanhSachKiTu;
        private DevExpress.XtraEditors.LabelControl labelControl19;
    }
}