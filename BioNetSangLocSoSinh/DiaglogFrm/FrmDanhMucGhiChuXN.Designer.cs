namespace BioNetSangLocSoSinh.DiaglogFrm
{
    partial class FrmDanhMucGhiChuXN
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
            this.GCDMGhiChuXN = new DevExpress.XtraGrid.GridControl();
            this.GVDMGhiChuXN = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_IDRow = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_VietTat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_YNghia = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_isDung = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.GCDMGhiChuXN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVDMGhiChuXN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // GCDMGhiChuXN
            // 
            this.GCDMGhiChuXN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GCDMGhiChuXN.Location = new System.Drawing.Point(0, 0);
            this.GCDMGhiChuXN.MainView = this.GVDMGhiChuXN;
            this.GCDMGhiChuXN.Name = "GCDMGhiChuXN";
            this.GCDMGhiChuXN.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.GCDMGhiChuXN.Size = new System.Drawing.Size(849, 414);
            this.GCDMGhiChuXN.TabIndex = 2;
            this.GCDMGhiChuXN.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GVDMGhiChuXN});
            // 
            // GVDMGhiChuXN
            // 
            this.GVDMGhiChuXN.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_IDRow,
            this.col_VietTat,
            this.col_YNghia,
            this.col_isDung});
            this.GVDMGhiChuXN.GridControl = this.GCDMGhiChuXN;
            this.GVDMGhiChuXN.Name = "GVDMGhiChuXN";
            this.GVDMGhiChuXN.NewItemRowText = "Thêm ghi chú mới";
            this.GVDMGhiChuXN.OptionsView.ShowGroupPanel = false;
            this.GVDMGhiChuXN.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.GVDMGhiChuXN_RowCellClick);
            this.GVDMGhiChuXN.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.GVDMGhiChuXN_ValidateRow);
            this.GVDMGhiChuXN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GVDMGhiChuXN_KeyDown);
            // 
            // col_IDRow
            // 
            this.col_IDRow.Caption = "ID Ghi Chú";
            this.col_IDRow.FieldName = "IDRowGhiChuXN";
            this.col_IDRow.Name = "col_IDRow";
            this.col_IDRow.OptionsColumn.AllowEdit = false;
            this.col_IDRow.OptionsColumn.ReadOnly = true;
            this.col_IDRow.Visible = true;
            this.col_IDRow.VisibleIndex = 0;
            this.col_IDRow.Width = 143;
            // 
            // col_VietTat
            // 
            this.col_VietTat.Caption = "Viết tắt";
            this.col_VietTat.FieldName = "VietTatGhiChu";
            this.col_VietTat.Name = "col_VietTat";
            this.col_VietTat.Visible = true;
            this.col_VietTat.VisibleIndex = 1;
            this.col_VietTat.Width = 228;
            // 
            // col_YNghia
            // 
            this.col_YNghia.Caption = "Ý Nghĩa";
            this.col_YNghia.FieldName = "NoiDungGhiChuTruoc";
            this.col_YNghia.Name = "col_YNghia";
            this.col_YNghia.Visible = true;
            this.col_YNghia.VisibleIndex = 2;
            this.col_YNghia.Width = 228;
            // 
            // col_isDung
            // 
            this.col_isDung.Caption = "Đang sử dụng";
            this.col_isDung.ColumnEdit = this.repositoryItemCheckEdit1;
            this.col_isDung.FieldName = "isSuDung";
            this.col_isDung.Name = "col_isDung";
            this.col_isDung.Visible = true;
            this.col_isDung.VisibleIndex = 3;
            this.col_isDung.Width = 231;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // FrmDanhMucGhiChuXN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 414);
            this.Controls.Add(this.GCDMGhiChuXN);
            this.LookAndFeel.SkinName = "Office 2007 Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "FrmDanhMucGhiChuXN";
            this.Text = "Danh mục ghi chú phòng xét nghiệm";
            this.Load += new System.EventHandler(this.FrmDanhMucGhiChuXN_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GCDMGhiChuXN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVDMGhiChuXN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraGrid.GridControl GCDMGhiChuXN;
        private DevExpress.XtraGrid.Views.Grid.GridView GVDMGhiChuXN;
        private DevExpress.XtraGrid.Columns.GridColumn col_IDRow;
        private DevExpress.XtraGrid.Columns.GridColumn col_VietTat;
        private DevExpress.XtraGrid.Columns.GridColumn col_YNghia;
        private DevExpress.XtraGrid.Columns.GridColumn col_isDung;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
    }
}