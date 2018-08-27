using DevExpress.XtraGrid.Views.Grid;

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
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.groupControlAdd = new DevExpress.XtraEditors.GroupControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddOk = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtVietTat = new DevExpress.XtraEditors.TextEdit();
            this.txtYNghia = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.GCDMGhiChuXN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVDMGhiChuXN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlAdd)).BeginInit();
            this.groupControlAdd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVietTat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYNghia.Properties)).BeginInit();
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
            this.GVDMGhiChuXN.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.GVDMGhiChuXN.OptionsView.ShowGroupPanel = false;
            this.GVDMGhiChuXN.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.GVDMGhiChuXN_RowCellClick);
            this.GVDMGhiChuXN.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.GVDMGhiChuXN_InitNewRow);
            this.GVDMGhiChuXN.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.GVDMGhiChuXN_ValidateRow);
            this.GVDMGhiChuXN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GVDMGhiChuXN_KeyDown);
            // 
            // col_IDRow
            // 
            this.col_IDRow.AppearanceHeader.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.col_IDRow.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.col_IDRow.AppearanceHeader.Options.UseFont = true;
            this.col_IDRow.AppearanceHeader.Options.UseForeColor = true;
            this.col_IDRow.AppearanceHeader.Options.UseTextOptions = true;
            this.col_IDRow.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
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
            this.col_VietTat.AppearanceHeader.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.col_VietTat.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.col_VietTat.AppearanceHeader.Options.UseFont = true;
            this.col_VietTat.AppearanceHeader.Options.UseForeColor = true;
            this.col_VietTat.AppearanceHeader.Options.UseTextOptions = true;
            this.col_VietTat.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_VietTat.Caption = "Viết tắt";
            this.col_VietTat.FieldName = "VietTatGhiChu";
            this.col_VietTat.Name = "col_VietTat";
            this.col_VietTat.Visible = true;
            this.col_VietTat.VisibleIndex = 1;
            this.col_VietTat.Width = 228;
            // 
            // col_YNghia
            // 
            this.col_YNghia.AppearanceHeader.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.col_YNghia.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.col_YNghia.AppearanceHeader.Options.UseFont = true;
            this.col_YNghia.AppearanceHeader.Options.UseForeColor = true;
            this.col_YNghia.AppearanceHeader.Options.UseTextOptions = true;
            this.col_YNghia.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_YNghia.Caption = "Ý Nghĩa";
            this.col_YNghia.FieldName = "NoiDungGhiChuTruoc";
            this.col_YNghia.Name = "col_YNghia";
            this.col_YNghia.Visible = true;
            this.col_YNghia.VisibleIndex = 2;
            this.col_YNghia.Width = 228;
            // 
            // col_isDung
            // 
            this.col_isDung.AppearanceHeader.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.col_isDung.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.col_isDung.AppearanceHeader.Options.UseFont = true;
            this.col_isDung.AppearanceHeader.Options.UseForeColor = true;
            this.col_isDung.AppearanceHeader.Options.UseTextOptions = true;
            this.col_isDung.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
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
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnAdd.Appearance.ForeColor = System.Drawing.Color.Green;
            this.btnAdd.Appearance.Options.UseFont = true;
            this.btnAdd.Appearance.Options.UseForeColor = true;
            this.btnAdd.ImageOptions.Image = global::BioNetSangLocSoSinh.Properties.Resources.add;
            this.btnAdd.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnAdd.Location = new System.Drawing.Point(17, 21);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(832, 24);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Thêm ghi chú";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // groupControlAdd
            // 
            this.groupControlAdd.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.groupControlAdd.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.groupControlAdd.AppearanceCaption.Options.UseFont = true;
            this.groupControlAdd.AppearanceCaption.Options.UseForeColor = true;
            this.groupControlAdd.AppearanceCaption.Options.UseTextOptions = true;
            this.groupControlAdd.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupControlAdd.Controls.Add(this.btnCancel);
            this.groupControlAdd.Controls.Add(this.btnAddOk);
            this.groupControlAdd.Controls.Add(this.label2);
            this.groupControlAdd.Controls.Add(this.label1);
            this.groupControlAdd.Controls.Add(this.txtVietTat);
            this.groupControlAdd.Controls.Add(this.txtYNghia);
            this.groupControlAdd.Location = new System.Drawing.Point(283, 110);
            this.groupControlAdd.Name = "groupControlAdd";
            this.groupControlAdd.Size = new System.Drawing.Size(287, 177);
            this.groupControlAdd.TabIndex = 4;
            this.groupControlAdd.Text = "Thêm ghi chú";
            this.groupControlAdd.Paint += new System.Windows.Forms.PaintEventHandler(this.groupControl1_Paint);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Appearance.ForeColor = System.Drawing.Color.DarkRed;
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.Appearance.Options.UseForeColor = true;
            this.btnCancel.ImageOptions.Image = global::BioNetSangLocSoSinh.Properties.Resources.x_button;
            this.btnCancel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnCancel.ImageOptions.ImageToTextIndent = 5;
            this.btnCancel.Location = new System.Drawing.Point(148, 145);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(131, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAddOk
            // 
            this.btnAddOk.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnAddOk.Appearance.ForeColor = System.Drawing.Color.Green;
            this.btnAddOk.Appearance.Options.UseFont = true;
            this.btnAddOk.Appearance.Options.UseForeColor = true;
            this.btnAddOk.ImageOptions.Image = global::BioNetSangLocSoSinh.Properties.Resources.add;
            this.btnAddOk.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnAddOk.ImageOptions.ImageToTextIndent = 5;
            this.btnAddOk.Location = new System.Drawing.Point(8, 145);
            this.btnAddOk.Name = "btnAddOk";
            this.btnAddOk.Size = new System.Drawing.Size(134, 23);
            this.btnAddOk.TabIndex = 4;
            this.btnAddOk.Text = "Thêm";
            this.btnAddOk.Click += new System.EventHandler(this.btnAddOk_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ý nghĩa";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Viết tắt";
            // 
            // txtVietTat
            // 
            this.txtVietTat.Location = new System.Drawing.Point(53, 30);
            this.txtVietTat.Name = "txtVietTat";
            this.txtVietTat.Size = new System.Drawing.Size(226, 20);
            this.txtVietTat.TabIndex = 0;
            // 
            // txtYNghia
            // 
            this.txtYNghia.Location = new System.Drawing.Point(53, 62);
            this.txtYNghia.Name = "txtYNghia";
            this.txtYNghia.Properties.Appearance.Options.UseTextOptions = true;
            this.txtYNghia.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.txtYNghia.Size = new System.Drawing.Size(226, 77);
            this.txtYNghia.TabIndex = 1;
            // 
            // FrmDanhMucGhiChuXN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 414);
            this.Controls.Add(this.groupControlAdd);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.GCDMGhiChuXN);
            this.LookAndFeel.SkinName = "Office 2007 Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximumSize = new System.Drawing.Size(865, 453);
            this.MinimumSize = new System.Drawing.Size(865, 453);
            this.Name = "FrmDanhMucGhiChuXN";
            this.Text = "Danh mục ghi chú phòng xét nghiệm";
            this.Load += new System.EventHandler(this.FrmDanhMucGhiChuXN_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GCDMGhiChuXN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVDMGhiChuXN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlAdd)).EndInit();
            this.groupControlAdd.ResumeLayout(false);
            this.groupControlAdd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVietTat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYNghia.Properties)).EndInit();
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
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.GroupControl groupControlAdd;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnAddOk;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtVietTat;
        private DevExpress.XtraEditors.MemoEdit txtYNghia;
    }
}