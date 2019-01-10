namespace BioNetSangLocSoSinh.Entry
{
    partial class FrmDMGoiDichVuChiTiet
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
            this.gridControl_GoiDichVuChung = new DevExpress.XtraGrid.GridControl();
            this.gridView_GoiDichVuChung = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_th_Stt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_th_IDGoiDichVuChung = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_th_TenGoiDichVuChung = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_th_GroupTraKQ = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemImageComboBoxNhomTraKQ = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnLuuSTT = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.repositoryItemCheckEdit_Check = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridView_DichVu = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_th_IDDichVu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_th_Check = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_th_TenDichVu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_th_TenHienThiDichVu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_th_KieuTraKQ = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemImageComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.col_th_IDKyThuatXN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemGridLookUpEditKyThuat = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.repositoryItemGridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControl_DichVu = new DevExpress.XtraGrid.GridControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_GoiDichVuChung)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_GoiDichVuChung)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBoxNhomTraKQ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit_Check)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_DichVu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEditKyThuat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_DichVu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControl_GoiDichVuChung
            // 
            this.gridControl_GoiDichVuChung.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_GoiDichVuChung.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControl_GoiDichVuChung.Location = new System.Drawing.Point(2, 22);
            this.gridControl_GoiDichVuChung.MainView = this.gridView_GoiDichVuChung;
            this.gridControl_GoiDichVuChung.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControl_GoiDichVuChung.Name = "gridControl_GoiDichVuChung";
            this.gridControl_GoiDichVuChung.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemImageComboBoxNhomTraKQ});
            this.gridControl_GoiDichVuChung.Size = new System.Drawing.Size(359, 672);
            this.gridControl_GoiDichVuChung.TabIndex = 0;
            this.gridControl_GoiDichVuChung.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView_GoiDichVuChung});
            // 
            // gridView_GoiDichVuChung
            // 
            this.gridView_GoiDichVuChung.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_th_Stt,
            this.col_th_IDGoiDichVuChung,
            this.col_th_TenGoiDichVuChung,
            this.col_th_GroupTraKQ});
            this.gridView_GoiDichVuChung.GridControl = this.gridControl_GoiDichVuChung;
            this.gridView_GoiDichVuChung.Name = "gridView_GoiDichVuChung";
            this.gridView_GoiDichVuChung.OptionsView.ShowGroupPanel = false;
            this.gridView_GoiDichVuChung.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gridView_GoiDichVuChung_RowCellClick);
            // 
            // col_th_Stt
            // 
            this.col_th_Stt.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.col_th_Stt.AppearanceHeader.Options.UseFont = true;
            this.col_th_Stt.AppearanceHeader.Options.UseTextOptions = true;
            this.col_th_Stt.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_th_Stt.Caption = "STT";
            this.col_th_Stt.FieldName = "Stt";
            this.col_th_Stt.Name = "col_th_Stt";
            this.col_th_Stt.Visible = true;
            this.col_th_Stt.VisibleIndex = 0;
            this.col_th_Stt.Width = 51;
            // 
            // col_th_IDGoiDichVuChung
            // 
            this.col_th_IDGoiDichVuChung.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.col_th_IDGoiDichVuChung.AppearanceHeader.Options.UseFont = true;
            this.col_th_IDGoiDichVuChung.Caption = "Mã gói";
            this.col_th_IDGoiDichVuChung.FieldName = "IDGoiDichVuChung";
            this.col_th_IDGoiDichVuChung.Name = "col_th_IDGoiDichVuChung";
            this.col_th_IDGoiDichVuChung.OptionsColumn.AllowEdit = false;
            this.col_th_IDGoiDichVuChung.OptionsColumn.AllowFocus = false;
            this.col_th_IDGoiDichVuChung.OptionsColumn.ReadOnly = true;
            this.col_th_IDGoiDichVuChung.Width = 84;
            // 
            // col_th_TenGoiDichVuChung
            // 
            this.col_th_TenGoiDichVuChung.AppearanceCell.Options.UseTextOptions = true;
            this.col_th_TenGoiDichVuChung.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.col_th_TenGoiDichVuChung.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.col_th_TenGoiDichVuChung.AppearanceHeader.Options.UseFont = true;
            this.col_th_TenGoiDichVuChung.AppearanceHeader.Options.UseTextOptions = true;
            this.col_th_TenGoiDichVuChung.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_th_TenGoiDichVuChung.Caption = "Tên gói";
            this.col_th_TenGoiDichVuChung.FieldName = "TenGoiDichVuChung";
            this.col_th_TenGoiDichVuChung.Name = "col_th_TenGoiDichVuChung";
            this.col_th_TenGoiDichVuChung.OptionsColumn.AllowEdit = false;
            this.col_th_TenGoiDichVuChung.OptionsColumn.AllowFocus = false;
            this.col_th_TenGoiDichVuChung.OptionsColumn.ReadOnly = true;
            this.col_th_TenGoiDichVuChung.Visible = true;
            this.col_th_TenGoiDichVuChung.VisibleIndex = 1;
            this.col_th_TenGoiDichVuChung.Width = 206;
            // 
            // col_th_GroupTraKQ
            // 
            this.col_th_GroupTraKQ.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.col_th_GroupTraKQ.AppearanceHeader.Options.UseFont = true;
            this.col_th_GroupTraKQ.Caption = "Nhóm Trả KQ";
            this.col_th_GroupTraKQ.ColumnEdit = this.repositoryItemImageComboBoxNhomTraKQ;
            this.col_th_GroupTraKQ.FieldName = "GroupTraKQ";
            this.col_th_GroupTraKQ.Name = "col_th_GroupTraKQ";
            this.col_th_GroupTraKQ.Visible = true;
            this.col_th_GroupTraKQ.VisibleIndex = 2;
            this.col_th_GroupTraKQ.Width = 85;
            // 
            // repositoryItemImageComboBoxNhomTraKQ
            // 
            this.repositoryItemImageComboBoxNhomTraKQ.AutoHeight = false;
            this.repositoryItemImageComboBoxNhomTraKQ.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemImageComboBoxNhomTraKQ.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Bionet", 1, -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("NeoGen Labs", 2, -1)});
            this.repositoryItemImageComboBoxNhomTraKQ.Name = "repositoryItemImageComboBoxNhomTraKQ";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnLuuSTT);
            this.panelControl1.Controls.Add(this.btnSave);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(2, 662);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(968, 32);
            this.panelControl1.TabIndex = 2;
            // 
            // btnLuuSTT
            // 
            this.btnLuuSTT.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.btnLuuSTT.Location = new System.Drawing.Point(89, 5);
            this.btnLuuSTT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLuuSTT.Name = "btnLuuSTT";
            this.btnLuuSTT.Size = new System.Drawing.Size(64, 24);
            this.btnLuuSTT.TabIndex = 1;
            this.btnLuuSTT.Text = "Lưu";
            this.btnLuuSTT.Click += new System.EventHandler(this.btnLuuSTT_Click);
            // 
            // btnSave
            // 
            this.btnSave.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.btnSave.Location = new System.Drawing.Point(16, 5);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(64, 24);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Lưu";
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // repositoryItemCheckEdit_Check
            // 
            this.repositoryItemCheckEdit_Check.AutoHeight = false;
            this.repositoryItemCheckEdit_Check.Name = "repositoryItemCheckEdit_Check";
            // 
            // gridView_DichVu
            // 
            this.gridView_DichVu.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_th_IDDichVu,
            this.col_th_Check,
            this.col_th_TenDichVu,
            this.col_th_TenHienThiDichVu,
            this.col_th_KieuTraKQ,
            this.col_th_IDKyThuatXN});
            this.gridView_DichVu.GridControl = this.gridControl_DichVu;
            this.gridView_DichVu.Name = "gridView_DichVu";
            this.gridView_DichVu.OptionsView.ShowGroupPanel = false;
            this.gridView_DichVu.DoubleClick += new System.EventHandler(this.gridView_DichVu_DoubleClick);
            // 
            // col_th_IDDichVu
            // 
            this.col_th_IDDichVu.Caption = "IDDichVu";
            this.col_th_IDDichVu.FieldName = "IDDichVu";
            this.col_th_IDDichVu.Name = "col_th_IDDichVu";
            // 
            // col_th_Check
            // 
            this.col_th_Check.AppearanceCell.Options.UseTextOptions = true;
            this.col_th_Check.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_th_Check.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold);
            this.col_th_Check.AppearanceHeader.Options.UseFont = true;
            this.col_th_Check.AppearanceHeader.Options.UseTextOptions = true;
            this.col_th_Check.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_th_Check.Caption = "Chọn";
            this.col_th_Check.FieldName = "Check";
            this.col_th_Check.Name = "col_th_Check";
            this.col_th_Check.Visible = true;
            this.col_th_Check.VisibleIndex = 0;
            this.col_th_Check.Width = 78;
            // 
            // col_th_TenDichVu
            // 
            this.col_th_TenDichVu.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold);
            this.col_th_TenDichVu.AppearanceHeader.Options.UseFont = true;
            this.col_th_TenDichVu.AppearanceHeader.Options.UseTextOptions = true;
            this.col_th_TenDichVu.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_th_TenDichVu.Caption = "Tên dịch vụ";
            this.col_th_TenDichVu.FieldName = "PSDanhMucDichVu.TenDichVu";
            this.col_th_TenDichVu.Name = "col_th_TenDichVu";
            this.col_th_TenDichVu.OptionsColumn.AllowEdit = false;
            this.col_th_TenDichVu.OptionsColumn.AllowFocus = false;
            this.col_th_TenDichVu.OptionsColumn.ReadOnly = true;
            this.col_th_TenDichVu.Visible = true;
            this.col_th_TenDichVu.VisibleIndex = 1;
            this.col_th_TenDichVu.Width = 94;
            // 
            // col_th_TenHienThiDichVu
            // 
            this.col_th_TenHienThiDichVu.Caption = "Tên hiện thị";
            this.col_th_TenHienThiDichVu.FieldName = "PSDanhMucDichVu.TenHienThiDichVu";
            this.col_th_TenHienThiDichVu.Name = "col_th_TenHienThiDichVu";
            this.col_th_TenHienThiDichVu.Visible = true;
            this.col_th_TenHienThiDichVu.VisibleIndex = 2;
            this.col_th_TenHienThiDichVu.Width = 531;
            // 
            // col_th_KieuTraKQ
            // 
            this.col_th_KieuTraKQ.Caption = "Kiểu trả KQ";
            this.col_th_KieuTraKQ.ColumnEdit = this.repositoryItemImageComboBox;
            this.col_th_KieuTraKQ.FieldName = "PSDanhMucDichVu.KieuTraKQ";
            this.col_th_KieuTraKQ.Name = "col_th_KieuTraKQ";
            this.col_th_KieuTraKQ.Visible = true;
            this.col_th_KieuTraKQ.VisibleIndex = 3;
            this.col_th_KieuTraKQ.Width = 66;
            // 
            // repositoryItemImageComboBox
            // 
            this.repositoryItemImageComboBox.AutoHeight = false;
            this.repositoryItemImageComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemImageComboBox.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Kết luận theo giá trị", 1, -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Kết luận theo test", 2, -1)});
            this.repositoryItemImageComboBox.Name = "repositoryItemImageComboBox";
            // 
            // col_th_IDKyThuatXN
            // 
            this.col_th_IDKyThuatXN.Caption = "Kỹ thuật XN";
            this.col_th_IDKyThuatXN.ColumnEdit = this.repositoryItemGridLookUpEditKyThuat;
            this.col_th_IDKyThuatXN.FieldName = "PSDanhMucDichVu.IDKyThuatXN";
            this.col_th_IDKyThuatXN.Name = "col_th_IDKyThuatXN";
            this.col_th_IDKyThuatXN.Visible = true;
            this.col_th_IDKyThuatXN.VisibleIndex = 4;
            this.col_th_IDKyThuatXN.Width = 182;
            // 
            // repositoryItemGridLookUpEditKyThuat
            // 
            this.repositoryItemGridLookUpEditKyThuat.AutoHeight = false;
            this.repositoryItemGridLookUpEditKyThuat.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemGridLookUpEditKyThuat.DisplayMember = "TenKyThuat";
            this.repositoryItemGridLookUpEditKyThuat.Name = "repositoryItemGridLookUpEditKyThuat";
            this.repositoryItemGridLookUpEditKyThuat.ValueMember = "IDKyThuatXN";
            this.repositoryItemGridLookUpEditKyThuat.View = this.repositoryItemGridLookUpEdit1View;
            // 
            // repositoryItemGridLookUpEdit1View
            // 
            this.repositoryItemGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemGridLookUpEdit1View.Name = "repositoryItemGridLookUpEdit1View";
            this.repositoryItemGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemGridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // gridControl_DichVu
            // 
            this.gridControl_DichVu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_DichVu.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControl_DichVu.Location = new System.Drawing.Point(2, 22);
            this.gridControl_DichVu.MainView = this.gridView_DichVu;
            this.gridControl_DichVu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControl_DichVu.Name = "gridControl_DichVu";
            this.gridControl_DichVu.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit_Check,
            this.repositoryItemImageComboBox,
            this.repositoryItemGridLookUpEditKyThuat});
            this.gridControl_DichVu.Size = new System.Drawing.Size(968, 640);
            this.gridControl_DichVu.TabIndex = 1;
            this.gridControl_DichVu.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView_DichVu});
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupControl1.AppearanceCaption.ForeColor = System.Drawing.Color.Maroon;
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseForeColor = true;
            this.groupControl1.Controls.Add(this.gridControl_GoiDichVuChung);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.LookAndFeel.SkinName = "Office 2007 Blue";
            this.groupControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(363, 696);
            this.groupControl1.TabIndex = 3;
            this.groupControl1.Text = "Danh sách gói bệnh";
            // 
            // groupControl2
            // 
            this.groupControl2.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.groupControl2.AppearanceCaption.ForeColor = System.Drawing.Color.Maroon;
            this.groupControl2.AppearanceCaption.Options.UseFont = true;
            this.groupControl2.AppearanceCaption.Options.UseForeColor = true;
            this.groupControl2.Controls.Add(this.gridControl_DichVu);
            this.groupControl2.Controls.Add(this.panelControl1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(363, 0);
            this.groupControl2.LookAndFeel.SkinName = "Office 2007 Blue";
            this.groupControl2.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(972, 696);
            this.groupControl2.TabIndex = 4;
            this.groupControl2.Text = "Chi tiết gói bệnh";
            // 
            // FrmDMGoiDichVuChiTiet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1335, 696);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.LookAndFeel.TouchUIMode = DevExpress.Utils.DefaultBoolean.False;
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmDMGoiDichVuChiTiet";
            this.Text = "FrmDMGoiDichVuChiTiet";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmDMGoiDichVuChiTiet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_GoiDichVuChung)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_GoiDichVuChung)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBoxNhomTraKQ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit_Check)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_DichVu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEditKyThuat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_DichVu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl_GoiDichVuChung;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_GoiDichVuChung;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.Columns.GridColumn col_th_IDGoiDichVuChung;
        private DevExpress.XtraGrid.Columns.GridColumn col_th_TenGoiDichVuChung;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit_Check;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_DichVu;
        private DevExpress.XtraGrid.Columns.GridColumn col_th_IDDichVu;
        private DevExpress.XtraGrid.Columns.GridColumn col_th_Check;
        private DevExpress.XtraGrid.Columns.GridColumn col_th_TenDichVu;
        private DevExpress.XtraGrid.GridControl gridControl_DichVu;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraGrid.Columns.GridColumn col_th_Stt;
        private DevExpress.XtraEditors.SimpleButton btnLuuSTT;
        private DevExpress.XtraGrid.Columns.GridColumn col_th_GroupTraKQ;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageComboBoxNhomTraKQ;
        private DevExpress.XtraGrid.Columns.GridColumn col_th_TenHienThiDichVu;
        private DevExpress.XtraGrid.Columns.GridColumn col_th_KieuTraKQ;
        private DevExpress.XtraGrid.Columns.GridColumn col_th_IDKyThuatXN;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageComboBox;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit repositoryItemGridLookUpEditKyThuat;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit1View;
    }
}