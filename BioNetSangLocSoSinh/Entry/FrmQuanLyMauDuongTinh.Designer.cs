namespace BioNetSangLocSoSinh.Entry
{
    partial class FrmQuanLyMauDuongTinh
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
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.dllNgay = new UserControlDate.dllNgay();
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
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChiCuc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDonVi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl16);
            this.panelControl1.Controls.Add(this.labelControl9);
            this.panelControl1.Controls.Add(this.txtChiCuc);
            this.panelControl1.Controls.Add(this.txtDonVi);
            this.panelControl1.Controls.Add(this.dllNgay);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(849, 92);
            this.panelControl1.TabIndex = 0;
            // 
            // panelControl2
            // 
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 92);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(849, 384);
            this.panelControl2.TabIndex = 1;
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
            // labelControl16
            // 
            this.labelControl16.Location = new System.Drawing.Point(312, 35);
            this.labelControl16.Name = "labelControl16";
            this.labelControl16.Size = new System.Drawing.Size(38, 13);
            this.labelControl16.TabIndex = 1078;
            this.labelControl16.Text = "Đơn vị :";
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(307, 9);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(41, 13);
            this.labelControl9.TabIndex = 1077;
            this.labelControl9.Text = "Chi cục :";
            // 
            // txtChiCuc
            // 
            this.txtChiCuc.Location = new System.Drawing.Point(354, 5);
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
            this.txtDonVi.Location = new System.Drawing.Point(354, 31);
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
            // FrmQuanLyMauDuongTinh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 476);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimizeBox = false;
            this.Name = "FrmQuanLyMauDuongTinh";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChiCuc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDonVi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private UserControlDate.dllNgay dllNgay;
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
    }
}