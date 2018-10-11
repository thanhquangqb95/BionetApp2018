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

namespace BioNetSangLocSoSinh.FrmDanhMuc
{
    public partial class FrmDanhMucMayXN : DevExpress.XtraEditors.XtraForm
    {
        public FrmDanhMucMayXN()
        {
            InitializeComponent();
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
                        string IDMay = this.GVDanhSachMayXN.GetRowCellValue(this.GVDanhSachMayXN.FocusedRowHandle, this.col_MayXN_IDMay).ToString();
                        string TenMay = this.GVDanhSachMayXN.GetRowCellValue(this.GVDanhSachMayXN.FocusedRowHandle, this.col_MayXN_TenMayXN).ToString();
                        //string KyHieu = this.GVDanhSachMayXN.GetRowCellValue(this.GVDanhSachMayXN.FocusedRowHandle, this.col_MayXN_KyHieu).ToString();
                        var vt = BioNet_Bus.GetDSMapViTriMayXN(IDMay);
                        if (vt != null)
                        {
                            GCVTViTriGanMayXN.DataSource = null;
                            GCVTViTriGanMayXN.DataSource = vt;
                        }
                        var map = BioNet_Bus.GetMapMayDichVus(IDMay);
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
    }
}