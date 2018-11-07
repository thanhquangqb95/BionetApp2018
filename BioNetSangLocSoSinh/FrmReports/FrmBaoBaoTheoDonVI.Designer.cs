namespace BioNetSangLocSoSinh.FrmReports
{
    partial class FrmBaoBaoTheoDonVI
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
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnClear = new DevExpress.XtraEditors.SimpleButton();
            this.btnThongke = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl16 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.txtChiCuc = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_idChiCuc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_TenChiCuc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtDonVi = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dllNgay = new UserControlDate.dllNgay();
            this.GCDanhSachDonVi = new DevExpress.XtraGrid.GridControl();
            this.GVDanhSachDonVi = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.bandedGridColumn1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.col_MaDonVi = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.LookupeditDV = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.repositoryItemGridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.LookUpEditTenVietTat = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.gridView5 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnXuatFile = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtChiCuc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDonVi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GCDanhSachDonVi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVDanhSachDonVi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LookupeditDV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LookUpEditTenVietTat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView5)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.btnClear);
            this.panelControl1.Controls.Add(this.btnThongke);
            this.panelControl1.Controls.Add(this.btnXuatFile);
            this.panelControl1.Controls.Add(this.labelControl16);
            this.panelControl1.Controls.Add(this.labelControl9);
            this.panelControl1.Controls.Add(this.txtChiCuc);
            this.panelControl1.Controls.Add(this.txtDonVi);
            this.panelControl1.Controls.Add(this.dllNgay);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "Office 2007 Blue";
            this.panelControl1.LookAndFeel.TouchUIMode = DevExpress.Utils.DefaultBoolean.False;
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1288, 88);
            this.panelControl1.TabIndex = 3;
            // 
            // btnClear
            // 
            this.btnClear.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnClear.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.btnClear.Appearance.Options.UseFont = true;
            this.btnClear.Appearance.Options.UseForeColor = true;
            this.btnClear.ImageOptions.Image = global::BioNetSangLocSoSinh.Properties.Resources.eraser;
            this.btnClear.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnClear.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnClear.Location = new System.Drawing.Point(596, 34);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(120, 23);
            toolTipTitleItem1.Text = "Bỏ lọc";
            superToolTip1.Items.Add(toolTipTitleItem1);
            this.btnClear.SuperTip = superToolTip1;
            this.btnClear.TabIndex = 1080;
            this.btnClear.Text = "Bỏ lọc";
            // 
            // btnThongke
            // 
            this.btnThongke.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThongke.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnThongke.Appearance.Options.UseFont = true;
            this.btnThongke.Appearance.Options.UseForeColor = true;
            this.btnThongke.ImageOptions.Image = global::BioNetSangLocSoSinh.Properties.Resources.graph;
            this.btnThongke.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnThongke.Location = new System.Drawing.Point(596, 6);
            this.btnThongke.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnThongke.Name = "btnThongke";
            this.btnThongke.Size = new System.Drawing.Size(120, 23);
            this.btnThongke.TabIndex = 1079;
            this.btnThongke.Text = "Thống kê";
            this.btnThongke.Click += new System.EventHandler(this.btnThongke_Click);
            // 
            // labelControl16
            // 
            this.labelControl16.Location = new System.Drawing.Point(307, 35);
            this.labelControl16.Name = "labelControl16";
            this.labelControl16.Size = new System.Drawing.Size(31, 13);
            this.labelControl16.TabIndex = 1078;
            this.labelControl16.Text = "Đơn vị";
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(307, 9);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(34, 13);
            this.labelControl9.TabIndex = 1077;
            this.labelControl9.Text = "Chi cục";
            // 
            // txtChiCuc
            // 
            this.txtChiCuc.Location = new System.Drawing.Point(348, 5);
            this.txtChiCuc.Name = "txtChiCuc";
            this.txtChiCuc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtChiCuc.Properties.DisplayMember = "TenChiCuc";
            this.txtChiCuc.Properties.NullText = "Chọn";
            this.txtChiCuc.Properties.PopupFormMinSize = new System.Drawing.Size(350, 350);
            this.txtChiCuc.Properties.PopupFormSize = new System.Drawing.Size(270, 300);
            this.txtChiCuc.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.LiveResize;
            this.txtChiCuc.Properties.ShowFooter = false;
            this.txtChiCuc.Properties.ValueMember = "MaChiCuc";
            this.txtChiCuc.Properties.View = this.gridView2;
            this.txtChiCuc.Size = new System.Drawing.Size(239, 20);
            this.txtChiCuc.TabIndex = 1075;
            this.txtChiCuc.EditValueChanged += new System.EventHandler(this.txtChiCuc_EditValueChanged);
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_idChiCuc,
            this.col_TenChiCuc});
            this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ShowColumnHeaders = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // col_idChiCuc
            // 
            this.col_idChiCuc.Caption = "Mã";
            this.col_idChiCuc.FieldName = "MaChiCuc";
            this.col_idChiCuc.Name = "col_idChiCuc";
            // 
            // col_TenChiCuc
            // 
            this.col_TenChiCuc.Caption = "ChiCuc";
            this.col_TenChiCuc.FieldName = "TenChiCuc";
            this.col_TenChiCuc.Name = "col_TenChiCuc";
            this.col_TenChiCuc.Visible = true;
            this.col_TenChiCuc.VisibleIndex = 0;
            // 
            // txtDonVi
            // 
            this.txtDonVi.Location = new System.Drawing.Point(348, 31);
            this.txtDonVi.Name = "txtDonVi";
            this.txtDonVi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDonVi.Properties.DisplayMember = "TenDVCS";
            this.txtDonVi.Properties.NullText = "Chọn";
            this.txtDonVi.Properties.PopupFormMinSize = new System.Drawing.Size(350, 350);
            this.txtDonVi.Properties.PopupFormSize = new System.Drawing.Size(270, 300);
            this.txtDonVi.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.LiveResize;
            this.txtDonVi.Properties.ShowFooter = false;
            this.txtDonVi.Properties.ValueMember = "MaDVCS";
            this.txtDonVi.Properties.View = this.gridView1;
            this.txtDonVi.Size = new System.Drawing.Size(239, 20);
            this.txtDonVi.TabIndex = 1076;
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowColumnHeaders = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Mã";
            this.gridColumn1.FieldName = "MaDVCS";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Đơn vị";
            this.gridColumn2.FieldName = "TenDVCS";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            // 
            // dllNgay
            // 
            this.dllNgay.BackColor = System.Drawing.Color.Transparent;
            this.dllNgay.Location = new System.Drawing.Point(5, 6);
            this.dllNgay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dllNgay.Name = "dllNgay";
            this.dllNgay.Size = new System.Drawing.Size(295, 73);
            this.dllNgay.TabIndex = 1070;
            // 
            // GCDanhSachDonVi
            // 
            this.GCDanhSachDonVi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GCDanhSachDonVi.Location = new System.Drawing.Point(0, 88);
            this.GCDanhSachDonVi.LookAndFeel.SkinName = "Office 2007 Blue";
            this.GCDanhSachDonVi.LookAndFeel.UseDefaultLookAndFeel = false;
            this.GCDanhSachDonVi.MainView = this.GVDanhSachDonVi;
            this.GCDanhSachDonVi.Name = "GCDanhSachDonVi";
            this.GCDanhSachDonVi.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.LookupeditDV,
            this.LookUpEditTenVietTat});
            this.GCDanhSachDonVi.Size = new System.Drawing.Size(1288, 629);
            this.GCDanhSachDonVi.TabIndex = 4;
            this.GCDanhSachDonVi.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GVDanhSachDonVi});
            // 
            // GVDanhSachDonVi
            // 
            this.GVDanhSachDonVi.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1});
            this.GVDanhSachDonVi.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.bandedGridColumn1,
            this.col_MaDonVi,
            this.gridColumn4,
            this.gridColumn3});
            this.GVDanhSachDonVi.GridControl = this.GCDanhSachDonVi;
            this.GVDanhSachDonVi.Name = "GVDanhSachDonVi";
            this.GVDanhSachDonVi.OptionsBehavior.ReadOnly = true;
            this.GVDanhSachDonVi.OptionsView.ColumnAutoWidth = false;
            this.GVDanhSachDonVi.OptionsView.ShowFooter = true;
            this.GVDanhSachDonVi.OptionsView.ShowGroupPanel = false;
            // 
            // gridBand1
            // 
            this.gridBand1.AppearanceHeader.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.gridBand1.AppearanceHeader.Options.UseFont = true;
            this.gridBand1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand1.Caption = "Thông tin đơn vị";
            this.gridBand1.Columns.Add(this.bandedGridColumn1);
            this.gridBand1.Columns.Add(this.col_MaDonVi);
            this.gridBand1.Columns.Add(this.gridColumn4);
            this.gridBand1.Columns.Add(this.gridColumn3);
            this.gridBand1.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.VisibleIndex = 0;
            this.gridBand1.Width = 300;
            // 
            // bandedGridColumn1
            // 
            this.bandedGridColumn1.Caption = "STT";
            this.bandedGridColumn1.FieldName = "STT";
            this.bandedGridColumn1.Name = "bandedGridColumn1";
            this.bandedGridColumn1.Visible = true;
            // 
            // col_MaDonVi
            // 
            this.col_MaDonVi.Caption = "Mã Đơn Vị";
            this.col_MaDonVi.FieldName = "MaDV";
            this.col_MaDonVi.Name = "col_MaDonVi";
            this.col_MaDonVi.Visible = true;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Tên Đơn Vị";
            this.gridColumn4.ColumnEdit = this.LookupeditDV;
            this.gridColumn4.FieldName = "MaDV";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            // 
            // LookupeditDV
            // 
            this.LookupeditDV.AutoHeight = false;
            this.LookupeditDV.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LookupeditDV.DisplayMember = "TenDVCS";
            this.LookupeditDV.Name = "LookupeditDV";
            this.LookupeditDV.ValueMember = "MaDVCS";
            this.LookupeditDV.View = this.repositoryItemGridLookUpEdit1View;
            // 
            // repositoryItemGridLookUpEdit1View
            // 
            this.repositoryItemGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemGridLookUpEdit1View.Name = "repositoryItemGridLookUpEdit1View";
            this.repositoryItemGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemGridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Viết Tắt";
            this.gridColumn3.ColumnEdit = this.LookUpEditTenVietTat;
            this.gridColumn3.FieldName = "MaDV";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            // 
            // LookUpEditTenVietTat
            // 
            this.LookUpEditTenVietTat.AutoHeight = false;
            this.LookUpEditTenVietTat.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LookUpEditTenVietTat.DisplayMember = "VietTatDV";
            this.LookUpEditTenVietTat.Name = "LookUpEditTenVietTat";
            this.LookUpEditTenVietTat.ValueMember = "MaDVCS";
            this.LookUpEditTenVietTat.View = this.gridView5;
            // 
            // gridView5
            // 
            this.gridView5.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView5.Name = "gridView5";
            this.gridView5.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView5.OptionsView.ShowGroupPanel = false;
            // 
            // btnXuatFile
            // 
            this.btnXuatFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXuatFile.Appearance.BorderColor = System.Drawing.Color.Maroon;
            this.btnXuatFile.Appearance.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.btnXuatFile.Appearance.ForeColor = System.Drawing.Color.DarkRed;
            this.btnXuatFile.Appearance.Options.UseBorderColor = true;
            this.btnXuatFile.Appearance.Options.UseFont = true;
            this.btnXuatFile.Appearance.Options.UseForeColor = true;
            this.btnXuatFile.ImageOptions.Image = global::BioNetSangLocSoSinh.Properties.Resources.import__2_;
            this.btnXuatFile.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnXuatFile.Location = new System.Drawing.Point(722, 6);
            this.btnXuatFile.Name = "btnXuatFile";
            this.btnXuatFile.Size = new System.Drawing.Size(119, 23);
            this.btnXuatFile.TabIndex = 2;
            this.btnXuatFile.Text = "Xuất file";
            // 
            // FrmBaoBaoTheoDonVI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1288, 717);
            this.ControlBox = false;
            this.Controls.Add(this.GCDanhSachDonVi);
            this.Controls.Add(this.panelControl1);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.None;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimizeBox = false;
            this.Name = "FrmBaoBaoTheoDonVI";
            this.Text = "XtraForm2";
            this.Load += new System.EventHandler(this.FrmBaoBaoTheoDonVI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtChiCuc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDonVi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GCDanhSachDonVi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVDanhSachDonVi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LookupeditDV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LookUpEditTenVietTat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnClear;
        private DevExpress.XtraEditors.SimpleButton btnThongke;
        private DevExpress.XtraEditors.LabelControl labelControl16;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.SearchLookUpEdit txtChiCuc;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn col_idChiCuc;
        private DevExpress.XtraGrid.Columns.GridColumn col_TenChiCuc;
        private DevExpress.XtraEditors.SearchLookUpEdit txtDonVi;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private UserControlDate.dllNgay dllNgay;
        private DevExpress.XtraGrid.GridControl GCDanhSachDonVi;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView GVDanhSachDonVi;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col_MaDonVi;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn4;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn3;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit LookupeditDV;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit1View;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit LookUpEditTenVietTat;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView5;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraEditors.SimpleButton btnXuatFile;
    }
}