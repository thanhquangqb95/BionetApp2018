namespace BioNetSangLocSoSinh.Entry
{
    partial class FrmShowSync
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmShowSync));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.GCShowKQSync = new DevExpress.XtraGrid.GridControl();
            this.GVShowKQSync = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_DateSync = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_TableSync = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_NoiDungLoi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_KQSync = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.butOK = new DevExpress.XtraEditors.SimpleButton();
            this.dllNgay = new UserControlDate.dllNgay();
            this.popupMenuGVChuaKetQua = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.col_STT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GCShowKQSync)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVShowKQSync)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuGVChuaKetQua)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(912, 495);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.GCShowKQSync);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 82);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(912, 413);
            this.panel3.TabIndex = 1;
            // 
            // GCShowKQSync
            // 
            this.GCShowKQSync.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GCShowKQSync.Location = new System.Drawing.Point(0, 0);
            this.GCShowKQSync.MainView = this.GVShowKQSync;
            this.GCShowKQSync.Name = "GCShowKQSync";
            this.GCShowKQSync.Size = new System.Drawing.Size(912, 413);
            this.GCShowKQSync.TabIndex = 0;
            this.GCShowKQSync.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GVShowKQSync});
            this.GCShowKQSync.Load += new System.EventHandler(this.GCShowKQSync_Load);
            // 
            // GVShowKQSync
            // 
            this.GVShowKQSync.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_DateSync,
            this.col_TableSync,
            this.col_NoiDungLoi,
            this.col_KQSync,
            this.col_STT});
            this.GVShowKQSync.GridControl = this.GCShowKQSync;
            this.GVShowKQSync.Name = "GVShowKQSync";
            this.GVShowKQSync.OptionsLayout.Columns.AddNewColumns = false;
            this.GVShowKQSync.OptionsMenu.ShowAddNewSummaryItem = DevExpress.Utils.DefaultBoolean.False;
            this.GVShowKQSync.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.GVShowKQSync.OptionsView.RowAutoHeight = true;
            this.GVShowKQSync.OptionsView.ShowGroupPanel = false;
            this.GVShowKQSync.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.GVShowKQSync_RowCellStyle);
            this.GVShowKQSync.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.GVShowKQSync_RowStyle);
            this.GVShowKQSync.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.GVShowKQSync_PopupMenuShowing);
            // 
            // col_DateSync
            // 
            this.col_DateSync.Caption = "Ngày Đồng bộ";
            this.col_DateSync.FieldName = "DateDB";
            this.col_DateSync.Name = "col_DateSync";
            this.col_DateSync.OptionsColumn.AllowEdit = false;
            this.col_DateSync.OptionsColumn.ReadOnly = true;
            this.col_DateSync.Visible = true;
            this.col_DateSync.VisibleIndex = 0;
            this.col_DateSync.Width = 81;
            // 
            // col_TableSync
            // 
            this.col_TableSync.Caption = "Bảng dự liệu đồng bộ";
            this.col_TableSync.FieldName = "TableSync";
            this.col_TableSync.Name = "col_TableSync";
            this.col_TableSync.OptionsColumn.AllowEdit = false;
            this.col_TableSync.OptionsColumn.ReadOnly = true;
            this.col_TableSync.Visible = true;
            this.col_TableSync.VisibleIndex = 1;
            this.col_TableSync.Width = 126;
            // 
            // col_NoiDungLoi
            // 
            this.col_NoiDungLoi.AppearanceCell.Options.UseTextOptions = true;
            this.col_NoiDungLoi.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.col_NoiDungLoi.Caption = "Nội dung lỗi";
            this.col_NoiDungLoi.FieldName = "NoiDungLoi";
            this.col_NoiDungLoi.Name = "col_NoiDungLoi";
            this.col_NoiDungLoi.OptionsColumn.AllowEdit = false;
            this.col_NoiDungLoi.OptionsColumn.ReadOnly = true;
            this.col_NoiDungLoi.Visible = true;
            this.col_NoiDungLoi.VisibleIndex = 2;
            this.col_NoiDungLoi.Width = 599;
            // 
            // col_KQSync
            // 
            this.col_KQSync.Caption = "Kết quả đồng bộ";
            this.col_KQSync.FieldName = "TrangThaiDB";
            this.col_KQSync.Name = "col_KQSync";
            this.col_KQSync.OptionsColumn.AllowEdit = false;
            this.col_KQSync.OptionsColumn.ReadOnly = true;
            this.col_KQSync.Visible = true;
            this.col_KQSync.VisibleIndex = 3;
            this.col_KQSync.Width = 88;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.butOK);
            this.panel2.Controls.Add(this.dllNgay);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(912, 82);
            this.panel2.TabIndex = 0;
            // 
            // butOK
            // 
            this.butOK.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.butOK.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("butOK.ImageOptions.Image")));
            this.butOK.Location = new System.Drawing.Point(320, 9);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(152, 32);
            this.butOK.TabIndex = 1068;
            this.butOK.Text = "Lấy số liệu";
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // dllNgay
            // 
            this.dllNgay.BackColor = System.Drawing.Color.Transparent;
            this.dllNgay.Location = new System.Drawing.Point(3, 4);
            this.dllNgay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dllNgay.Name = "dllNgay";
            this.dllNgay.Size = new System.Drawing.Size(311, 73);
            this.dllNgay.TabIndex = 1067;
            // 
            // popupMenuGVChuaKetQua
            // 
            this.popupMenuGVChuaKetQua.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1)});
            this.popupMenuGVChuaKetQua.Manager = this.barManager1;
            this.popupMenuGVChuaKetQua.Name = "popupMenuGVChuaKetQua";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Hoàn phiếu lỗi";
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1});
            this.barManager1.MaxItemId = 1;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(912, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 495);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(912, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 495);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(912, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 495);
            // 
            // col_STT
            // 
            this.col_STT.Caption = "stt";
            this.col_STT.FieldName = "STT";
            this.col_STT.Name = "col_STT";
            // 
            // FrmShowSync
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 495);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmShowSync";
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GCShowKQSync)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVShowKQSync)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuGVChuaKetQua)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraGrid.GridControl GCShowKQSync;
        private DevExpress.XtraGrid.Views.Grid.GridView GVShowKQSync;
        private DevExpress.XtraGrid.Columns.GridColumn col_DateSync;
        private DevExpress.XtraGrid.Columns.GridColumn col_TableSync;
        private DevExpress.XtraGrid.Columns.GridColumn col_NoiDungLoi;
        private DevExpress.XtraGrid.Columns.GridColumn col_KQSync;
        private UserControlDate.dllNgay dllNgay;
        private DevExpress.XtraEditors.SimpleButton butOK;
        private DevExpress.XtraBars.PopupMenu popupMenuGVChuaKetQua;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.Columns.GridColumn col_STT;
    }
}