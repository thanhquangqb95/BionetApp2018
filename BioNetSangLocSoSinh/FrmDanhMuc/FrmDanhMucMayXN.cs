using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BioNetBLL;
using BioNetModel.Data;
using DevExpress.XtraGrid;

namespace BioNetSangLocSoSinh.FrmDanhMuc
{
    public partial class FrmDanhMucMayXN : DevExpress.XtraEditors.XtraForm
    {
        public IList<PSMapsViTriMayXN> dataSource = new BindingList<PSMapsViTriMayXN>();
        public FrmDanhMucMayXN()
        {
            InitializeComponent();
            ((System.ComponentModel.ISupportInitialize)(this.GCVTViTriGanMayXN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVCTViTriGanMayXN)).BeginInit();
            this.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GCVTViTriGanMayXN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GVCTViTriGanMayXN)).EndInit();
            this.ResumeLayout(false);
        }

        private void groupControl3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FrmDanhMucMayXN_Load(object sender, EventArgs e)
        {
           
            LoadDanhSachMay();
        }
        private void LoadDanhSachMay()
        {
            GCDanhSachMayXN.DataSource = null;
            GCDanhSachMayXN.DataSource = BioNet_Bus.GetDSMayXN();
        }

        private void GVDanhSachMayXN_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                if (this.GVDanhSachMayXN.RowCount > 0)
                {
                    if (this.GVDanhSachMayXN.GetFocusedRow() != null)
                    {
                        txtMaMay.Text = this.GVDanhSachMayXN.GetRowCellValue(this.GVDanhSachMayXN.FocusedRowHandle, this.col_MayXN_IDMay).ToString();
                        txtTenMay.Text = this.GVDanhSachMayXN.GetRowCellValue(this.GVDanhSachMayXN.FocusedRowHandle, this.col_MayXN_TenMayXN).ToString();
                        txtSoHieu.Text = this.GVDanhSachMayXN.GetRowCellValue(this.GVDanhSachMayXN.FocusedRowHandle, this.col_MayXN_KyHieu).ToString();
                        cckIsUse.EditValue=Boolean.Parse(this.GVDanhSachMayXN.GetRowCellValue(this.GVDanhSachMayXN.FocusedRowHandle, this.col_MayXN_isSuDung).ToString());
                        var vt = BioNet_Bus.GetDSMapViTriMayXN(txtMaMay.Text);
                        if (vt != null)
                        {
                            GCVTViTriGanMayXN.DataSource = null;
                            GCVTViTriGanMayXN.DataSource = vt;
                        }
                        var map = BioNet_Bus.GetMapMayDichVus(txtMaMay.Text);
                        if (map!=null)
                        {
                            this.GCMapMayDV.DataSource = null;
                            this.GCMapMayDV.DataSource = map;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi khi hiển thị thông tin máy ! \r\n Lỗi chi tiết :" + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GCMapMayDV_Click(object sender, EventArgs e)
        {

        }

        private void GCDanhSachMayXN_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtMaMay.Text = string.Empty;
            txtSoHieu.Text = string.Empty;
            txtTenMay.Text = string.Empty;
            this.GCMapMayDV.DataSource = new List<PSMapsViTriMayXN>();
            GCVTViTriGanMayXN.DataSource = null;

        }

        private void GVCTViTriGanMayXN_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {

        }

        private void GVCTViTriGanMayXN_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            int rowHandle = GVCTViTriGanMayXN.DataRowCount + 1;
            PSMapsViTriMayXN may = new PSMapsViTriMayXN();
            if (GVCTViTriGanMayXN.IsNewItemRow(rowHandle))
            {
                GVCTViTriGanMayXN.SetRowCellValue(rowHandle, col_ViTri_STTVT, rowHandle);
            }
        }

        private void GVCTViTriGanMayXN_ShowingEditor(object sender, CancelEventArgs e)
        {
            

        }

        private void GVCTViTriGanMayXN_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {

        }

        private void GVCTViTriGanMayXN_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            if (e.Row == null) return;
            if (e.RowHandle == GridControl.NewItemRowHandle)
            {
                e.Valid = !string.IsNullOrEmpty(((PSMapsViTriMayXN)e.Row).STT.ToString());
            }
        }

        private void GCVTViTriGanMayXN_Click(object sender, EventArgs e)
        {

        }

        private void GCVTViTriGanMayXN_EmbeddedNavigator_Click(object sender, EventArgs e)
        {

        }
        
    }
}