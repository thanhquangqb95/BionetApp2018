namespace BioNetSangLocSoSinh.Entry
{
    partial class FrmDMLanguage
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.GCForm = new DevExpress.XtraGrid.GridControl();
            this.GVForm = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_Form = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_FormName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_FormCapiton = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GCItem = new DevExpress.XtraGrid.GridControl();
            this.GVItem = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_IDItem = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_ITemName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_ItemVN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_ItemEN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GCForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GCItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.panel1.Controls.Add(this.splitContainerControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1005, 732);
            this.panel1.TabIndex = 0;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.GCForm);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.GCItem);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1005, 732);
            this.splitContainerControl1.SplitterPosition = 197;
            this.splitContainerControl1.TabIndex = 1;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // GCForm
            // 
            this.GCForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GCForm.Location = new System.Drawing.Point(0, 0);
            this.GCForm.MainView = this.GVForm;
            this.GCForm.Name = "GCForm";
            this.GCForm.Size = new System.Drawing.Size(197, 732);
            this.GCForm.TabIndex = 1;
            this.GCForm.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GVForm,
            this.gridView1});
            // 
            // GVForm
            // 
            this.GVForm.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_Form,
            this.col_FormName,
            this.col_FormCapiton});
            this.GVForm.GridControl = this.GCForm;
            this.GVForm.Name = "GVForm";
            this.GVForm.OptionsDetail.EnableMasterViewMode = false;
            this.GVForm.OptionsSelection.MultiSelect = true;
            this.GVForm.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.GVForm_RowCellClick);
            // 
            // col_Form
            // 
            this.col_Form.Caption = "IDForm";
            this.col_Form.FieldName = "IDForm";
            this.col_Form.Name = "col_Form";
            this.col_Form.OptionsColumn.AllowEdit = false;
            this.col_Form.OptionsColumn.ReadOnly = true;
            this.col_Form.Visible = true;
            this.col_Form.VisibleIndex = 0;
            // 
            // col_FormName
            // 
            this.col_FormName.Caption = "Tên Form";
            this.col_FormName.FieldName = "NameForm";
            this.col_FormName.Name = "col_FormName";
            this.col_FormName.OptionsColumn.AllowEdit = false;
            this.col_FormName.OptionsColumn.ReadOnly = true;
            this.col_FormName.Visible = true;
            this.col_FormName.VisibleIndex = 1;
            // 
            // col_FormCapiton
            // 
            this.col_FormCapiton.Caption = "Tên Form";
            this.col_FormCapiton.FieldName = "Capiton";
            this.col_FormCapiton.Name = "col_FormCapiton";
            this.col_FormCapiton.OptionsColumn.AllowEdit = false;
            this.col_FormCapiton.OptionsColumn.ReadOnly = true;
            this.col_FormCapiton.Visible = true;
            this.col_FormCapiton.VisibleIndex = 2;
            // 
            // GCItem
            // 
            this.GCItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GCItem.Location = new System.Drawing.Point(0, 0);
            this.GCItem.MainView = this.GVItem;
            this.GCItem.Name = "GCItem";
            this.GCItem.Size = new System.Drawing.Size(803, 732);
            this.GCItem.TabIndex = 2;
            this.GCItem.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GVItem});
            // 
            // GVItem
            // 
            this.GVItem.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_IDItem,
            this.col_ITemName,
            this.col_ItemVN,
            this.col_ItemEN});
            this.GVItem.GridControl = this.GCItem;
            this.GVItem.Name = "GVItem";
            this.GVItem.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.GVItem_ValidateRow);
            // 
            // col_IDItem
            // 
            this.col_IDItem.Caption = "ID Item";
            this.col_IDItem.FieldName = "IDItem";
            this.col_IDItem.Name = "col_IDItem";
            this.col_IDItem.OptionsColumn.AllowEdit = false;
            this.col_IDItem.OptionsColumn.ReadOnly = true;
            this.col_IDItem.Visible = true;
            this.col_IDItem.VisibleIndex = 0;
            // 
            // col_ITemName
            // 
            this.col_ITemName.Caption = "Tên Item";
            this.col_ITemName.FieldName = "ItemName";
            this.col_ITemName.Name = "col_ITemName";
            this.col_ITemName.OptionsColumn.AllowEdit = false;
            this.col_ITemName.OptionsColumn.ReadOnly = true;
            this.col_ITemName.Visible = true;
            this.col_ITemName.VisibleIndex = 1;
            // 
            // col_ItemVN
            // 
            this.col_ItemVN.Caption = "Tiếng Việt";
            this.col_ItemVN.FieldName = "VN";
            this.col_ItemVN.Name = "col_ItemVN";
            this.col_ItemVN.OptionsColumn.AllowEdit = false;
            this.col_ItemVN.OptionsColumn.ReadOnly = true;
            this.col_ItemVN.Visible = true;
            this.col_ItemVN.VisibleIndex = 2;
            // 
            // col_ItemEN
            // 
            this.col_ItemEN.Caption = "Tiếng Anh";
            this.col_ItemEN.FieldName = "Trans";
            this.col_ItemEN.Name = "col_ItemEN";
            this.col_ItemEN.Visible = true;
            this.col_ItemEN.VisibleIndex = 3;
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.GCForm;
            this.gridView1.Name = "gridView1";
            // 
            // FrmDMLanguage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 732);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDMLanguage";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.FrmDMLanguage_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GCForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GCItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.GridControl GCForm;
        private DevExpress.XtraGrid.Views.Grid.GridView GVForm;
        private DevExpress.XtraGrid.Columns.GridColumn col_Form;
        private DevExpress.XtraGrid.Columns.GridColumn col_FormName;
        private DevExpress.XtraGrid.Columns.GridColumn col_FormCapiton;
        private DevExpress.XtraGrid.GridControl GCItem;
        private DevExpress.XtraGrid.Views.Grid.GridView GVItem;
        private DevExpress.XtraGrid.Columns.GridColumn col_IDItem;
        private DevExpress.XtraGrid.Columns.GridColumn col_ITemName;
        private DevExpress.XtraGrid.Columns.GridColumn col_ItemVN;
        private DevExpress.XtraGrid.Columns.GridColumn col_ItemEN;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    }
}