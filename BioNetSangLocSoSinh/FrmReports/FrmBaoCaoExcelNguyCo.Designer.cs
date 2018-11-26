namespace BioNetSangLocSoSinh.FrmReports
{
    partial class FrmBaoCaoExcelNguyCo
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnLocDanhSach = new DevExpress.XtraEditors.SimpleButton();
            this.btnOpenFile = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.TabConTrol = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.GCFileExcel = new DevExpress.XtraGrid.GridControl();
            this.GVFileExcel = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.GCNghiNgo = new DevExpress.XtraGrid.GridControl();
            this.GVNghiNgo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.GCNguycocao = new DevExpress.XtraGrid.GridControl();
            this.GVNguycocao = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.xtraTabPage4 = new DevExpress.XtraTab.XtraTabPage();
            this.GCNguycothap2 = new DevExpress.XtraGrid.GridControl();
            this.GVNguycothap2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.xtraTabPage5 = new DevExpress.XtraTab.XtraTabPage();
            this.GCDuongtinh = new DevExpress.XtraGrid.GridControl();
            this.GVDuongTinh = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.xtraTabPage6 = new DevExpress.XtraTab.XtraTabPage();
            this.GCAmTinh = new DevExpress.XtraGrid.GridControl();
            this.GVAmTinh = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.TabPageBaoCaoTongHop = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage8 = new DevExpress.XtraTab.XtraTabPage();
            this.documentViewerDV = new DevExpress.XtraPrinting.Preview.DocumentViewer();
            this.openFileExel = new System.Windows.Forms.OpenFileDialog();
            this.previewBar1 = new DevExpress.XtraPrinting.Preview.PreviewBar();
            this.previewBar2 = new DevExpress.XtraPrinting.Preview.PreviewBar();
            this.previewBar3 = new DevExpress.XtraPrinting.Preview.PreviewBar();
            this.previewBar4 = new DevExpress.XtraPrinting.Preview.PreviewBar();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TabConTrol)).BeginInit();
            this.TabConTrol.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GCFileExcel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVFileExcel)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GCNghiNgo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVNghiNgo)).BeginInit();
            this.xtraTabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GCNguycocao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVNguycocao)).BeginInit();
            this.xtraTabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GCNguycothap2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVNguycothap2)).BeginInit();
            this.xtraTabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GCDuongtinh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVDuongTinh)).BeginInit();
            this.xtraTabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GCAmTinh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVAmTinh)).BeginInit();
            this.xtraTabPage8.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Controls.Add(this.btnLocDanhSach);
            this.panelControl1.Controls.Add(this.btnOpenFile);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1370, 42);
            this.panelControl1.TabIndex = 0;
            this.panelControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.panelControl1_Paint);
            // 
            // btnLocDanhSach
            // 
            this.btnLocDanhSach.Location = new System.Drawing.Point(93, 5);
            this.btnLocDanhSach.Name = "btnLocDanhSach";
            this.btnLocDanhSach.Size = new System.Drawing.Size(105, 23);
            this.btnLocDanhSach.TabIndex = 6;
            this.btnLocDanhSach.Text = "Lọc danh sách";
            this.btnLocDanhSach.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(12, 7);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFile.TabIndex = 0;
            this.btnOpenFile.Text = "Mở File";
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.TabConTrol);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 42);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1370, 659);
            this.panelControl2.TabIndex = 1;
            // 
            // TabConTrol
            // 
            this.TabConTrol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabConTrol.Location = new System.Drawing.Point(2, 2);
            this.TabConTrol.Name = "TabConTrol";
            this.TabConTrol.SelectedTabPage = this.xtraTabPage1;
            this.TabConTrol.Size = new System.Drawing.Size(1366, 655);
            this.TabConTrol.TabIndex = 1;
            this.TabConTrol.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.xtraTabPage3,
            this.xtraTabPage4,
            this.xtraTabPage5,
            this.xtraTabPage6,
            this.TabPageBaoCaoTongHop,
            this.xtraTabPage8});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.GCFileExcel);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1360, 627);
            this.xtraTabPage1.Text = "Danh sách dữ liệu";
            // 
            // GCFileExcel
            // 
            this.GCFileExcel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GCFileExcel.Location = new System.Drawing.Point(0, 0);
            this.GCFileExcel.MainView = this.GVFileExcel;
            this.GCFileExcel.Name = "GCFileExcel";
            this.GCFileExcel.Size = new System.Drawing.Size(1360, 627);
            this.GCFileExcel.TabIndex = 0;
            this.GCFileExcel.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GVFileExcel});
            // 
            // GVFileExcel
            // 
            this.GVFileExcel.GridControl = this.GCFileExcel;
            this.GVFileExcel.Name = "GVFileExcel";
            this.GVFileExcel.OptionsView.ColumnAutoWidth = false;
            this.GVFileExcel.OptionsView.ShowFooter = true;
            this.GVFileExcel.OptionsView.ShowGroupPanel = false;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.GCNghiNgo);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(1360, 627);
            this.xtraTabPage2.Text = "Nghi ngờ";
            // 
            // GCNghiNgo
            // 
            this.GCNghiNgo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GCNghiNgo.Location = new System.Drawing.Point(0, 0);
            this.GCNghiNgo.MainView = this.GVNghiNgo;
            this.GCNghiNgo.Name = "GCNghiNgo";
            this.GCNghiNgo.Size = new System.Drawing.Size(1360, 627);
            this.GCNghiNgo.TabIndex = 1;
            this.GCNghiNgo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GVNghiNgo});
            // 
            // GVNghiNgo
            // 
            this.GVNghiNgo.GridControl = this.GCNghiNgo;
            this.GVNghiNgo.Name = "GVNghiNgo";
            this.GVNghiNgo.OptionsView.ColumnAutoWidth = false;
            this.GVNghiNgo.OptionsView.ShowGroupPanel = false;
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Controls.Add(this.GCNguycocao);
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Size = new System.Drawing.Size(1360, 627);
            this.xtraTabPage3.Text = "Nguy cơ cao";
            this.xtraTabPage3.Paint += new System.Windows.Forms.PaintEventHandler(this.xtraTabPage3_Paint);
            // 
            // GCNguycocao
            // 
            this.GCNguycocao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GCNguycocao.Location = new System.Drawing.Point(0, 0);
            this.GCNguycocao.MainView = this.GVNguycocao;
            this.GCNguycocao.Name = "GCNguycocao";
            this.GCNguycocao.Size = new System.Drawing.Size(1360, 627);
            this.GCNguycocao.TabIndex = 2;
            this.GCNguycocao.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GVNguycocao});
            // 
            // GVNguycocao
            // 
            this.GVNguycocao.GridControl = this.GCNguycocao;
            this.GVNguycocao.Name = "GVNguycocao";
            this.GVNguycocao.OptionsView.ColumnAutoWidth = false;
            this.GVNguycocao.OptionsView.ShowGroupPanel = false;
            // 
            // xtraTabPage4
            // 
            this.xtraTabPage4.Controls.Add(this.GCNguycothap2);
            this.xtraTabPage4.Name = "xtraTabPage4";
            this.xtraTabPage4.Size = new System.Drawing.Size(1360, 627);
            this.xtraTabPage4.Text = "Nguy cơ thấp lần 2";
            this.xtraTabPage4.Paint += new System.Windows.Forms.PaintEventHandler(this.xtraTabPage4_Paint);
            // 
            // GCNguycothap2
            // 
            this.GCNguycothap2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GCNguycothap2.Location = new System.Drawing.Point(0, 0);
            this.GCNguycothap2.MainView = this.GVNguycothap2;
            this.GCNguycothap2.Name = "GCNguycothap2";
            this.GCNguycothap2.Size = new System.Drawing.Size(1360, 627);
            this.GCNguycothap2.TabIndex = 2;
            this.GCNguycothap2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GVNguycothap2});
            // 
            // GVNguycothap2
            // 
            this.GVNguycothap2.GridControl = this.GCNguycothap2;
            this.GVNguycothap2.Name = "GVNguycothap2";
            this.GVNguycothap2.OptionsView.ColumnAutoWidth = false;
            this.GVNguycothap2.OptionsView.ShowGroupPanel = false;
            // 
            // xtraTabPage5
            // 
            this.xtraTabPage5.Controls.Add(this.GCDuongtinh);
            this.xtraTabPage5.Name = "xtraTabPage5";
            this.xtraTabPage5.Size = new System.Drawing.Size(1360, 627);
            this.xtraTabPage5.Text = "Danh sách dương tính";
            // 
            // GCDuongtinh
            // 
            this.GCDuongtinh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GCDuongtinh.Location = new System.Drawing.Point(0, 0);
            this.GCDuongtinh.MainView = this.GVDuongTinh;
            this.GCDuongtinh.Name = "GCDuongtinh";
            this.GCDuongtinh.Size = new System.Drawing.Size(1360, 627);
            this.GCDuongtinh.TabIndex = 2;
            this.GCDuongtinh.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GVDuongTinh});
            // 
            // GVDuongTinh
            // 
            this.GVDuongTinh.GridControl = this.GCDuongtinh;
            this.GVDuongTinh.Name = "GVDuongTinh";
            this.GVDuongTinh.OptionsView.ColumnAutoWidth = false;
            this.GVDuongTinh.OptionsView.ShowGroupPanel = false;
            // 
            // xtraTabPage6
            // 
            this.xtraTabPage6.Controls.Add(this.GCAmTinh);
            this.xtraTabPage6.Name = "xtraTabPage6";
            this.xtraTabPage6.Size = new System.Drawing.Size(1360, 627);
            this.xtraTabPage6.Text = "Danh sách âm tính";
            // 
            // GCAmTinh
            // 
            this.GCAmTinh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GCAmTinh.Location = new System.Drawing.Point(0, 0);
            this.GCAmTinh.MainView = this.GVAmTinh;
            this.GCAmTinh.Name = "GCAmTinh";
            this.GCAmTinh.Size = new System.Drawing.Size(1360, 627);
            this.GCAmTinh.TabIndex = 2;
            this.GCAmTinh.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GVAmTinh});
            // 
            // GVAmTinh
            // 
            this.GVAmTinh.GridControl = this.GCAmTinh;
            this.GVAmTinh.Name = "GVAmTinh";
            this.GVAmTinh.OptionsView.ColumnAutoWidth = false;
            this.GVAmTinh.OptionsView.ShowGroupPanel = false;
            // 
            // TabPageBaoCaoTongHop
            // 
            this.TabPageBaoCaoTongHop.Name = "TabPageBaoCaoTongHop";
            this.TabPageBaoCaoTongHop.Size = new System.Drawing.Size(1360, 627);
            this.TabPageBaoCaoTongHop.Text = "Báo cáo tổng hợp";
            // 
            // xtraTabPage8
            // 
            this.xtraTabPage8.Controls.Add(this.documentViewerDV);
            this.xtraTabPage8.Name = "xtraTabPage8";
            this.xtraTabPage8.Size = new System.Drawing.Size(1360, 627);
            this.xtraTabPage8.Text = "Báo cáo theo đơn vị";
            // 
            // documentViewerDV
            // 
            this.documentViewerDV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentViewerDV.DocumentSource = typeof(BioNetSangLocSoSinh.Reports.RepostsBaoCao.rptBaoCaoExcelNguyCo);
            this.documentViewerDV.IsMetric = false;
            this.documentViewerDV.Location = new System.Drawing.Point(0, 0);
            this.documentViewerDV.Margin = new System.Windows.Forms.Padding(1);
            this.documentViewerDV.Name = "documentViewerDV";
            this.documentViewerDV.Size = new System.Drawing.Size(1360, 627);
            this.documentViewerDV.TabIndex = 7;
            // 
            // openFileExel
            // 
            this.openFileExel.FileName = "openFileExcel";
            // 
            // previewBar1
            // 
            this.previewBar1.BarName = "Toolbar";
            this.previewBar1.DockCol = 0;
            this.previewBar1.DockRow = 1;
            this.previewBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.previewBar1.Text = "Toolbar";
            // 
            // previewBar2
            // 
            this.previewBar2.BarName = "Toolbar";
            this.previewBar2.DockCol = 0;
            this.previewBar2.DockRow = 1;
            this.previewBar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.previewBar2.Text = "Toolbar";
            // 
            // previewBar3
            // 
            this.previewBar3.BarName = "Toolbar";
            this.previewBar3.DockCol = 0;
            this.previewBar3.DockRow = 1;
            this.previewBar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.previewBar3.Text = "Toolbar";
            // 
            // previewBar4
            // 
            this.previewBar4.BarName = "Main Menu";
            this.previewBar4.DockCol = 0;
            this.previewBar4.DockRow = 0;
            this.previewBar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.previewBar4.OptionsBar.MultiLine = true;
            this.previewBar4.OptionsBar.UseWholeRow = true;
            this.previewBar4.Text = "Main Menu";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(204, 5);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(122, 23);
            this.simpleButton1.TabIndex = 7;
            this.simpleButton1.Text = "Lưu Excel các nhóm";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click_1);
            // 
            // FrmBaoCaoExcelNguyCo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 701);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.None;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmBaoCaoExcelNguyCo";
            this.Text = "FrmBaoCaoExcelNguyCo";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TabConTrol)).EndInit();
            this.TabConTrol.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GCFileExcel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVFileExcel)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GCNghiNgo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVNghiNgo)).EndInit();
            this.xtraTabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GCNguycocao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVNguycocao)).EndInit();
            this.xtraTabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GCNguycothap2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVNguycothap2)).EndInit();
            this.xtraTabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GCDuongtinh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVDuongTinh)).EndInit();
            this.xtraTabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GCAmTinh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVAmTinh)).EndInit();
            this.xtraTabPage8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnOpenFile;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraTab.XtraTabControl TabConTrol;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraGrid.GridControl GCFileExcel;
        private DevExpress.XtraGrid.Views.Grid.GridView GVFileExcel;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private System.Windows.Forms.OpenFileDialog openFileExel;
        private DevExpress.XtraEditors.SimpleButton btnLocDanhSach;
        private DevExpress.XtraGrid.GridControl GCNghiNgo;
        private DevExpress.XtraGrid.Views.Grid.GridView GVNghiNgo;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage3;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage4;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage5;
        private DevExpress.XtraGrid.GridControl GCNguycocao;
        private DevExpress.XtraGrid.Views.Grid.GridView GVNguycocao;
        private DevExpress.XtraGrid.GridControl GCNguycothap2;
        private DevExpress.XtraGrid.Views.Grid.GridView GVNguycothap2;
        private DevExpress.XtraGrid.GridControl GCDuongtinh;
        private DevExpress.XtraGrid.Views.Grid.GridView GVDuongTinh;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage6;
        private DevExpress.XtraGrid.GridControl GCAmTinh;
        private DevExpress.XtraGrid.Views.Grid.GridView GVAmTinh;
        private DevExpress.XtraTab.XtraTabPage TabPageBaoCaoTongHop;
        private DevExpress.XtraPrinting.Preview.PreviewBar previewBar1;
        private DevExpress.XtraPrinting.Preview.PreviewBar previewBar2;
        private DevExpress.XtraPrinting.Preview.PreviewBar previewBar3;
        private DevExpress.XtraPrinting.Preview.PreviewBar previewBar4;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage8;
        private DevExpress.XtraPrinting.Preview.DocumentViewer documentViewerDV;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}