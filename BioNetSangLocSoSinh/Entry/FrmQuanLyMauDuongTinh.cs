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

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmQuanLyMauDuongTinh : DevExpress.XtraEditors.XtraForm
    {
        public FrmQuanLyMauDuongTinh()
        {
            InitializeComponent();
        }
        private List<PSDanhMucGoiDichVuChung> lstgoiXN = new List<PSDanhMucGoiDichVuChung>();
        private void LoadGoiDichVuXetNGhiem()
        {
            try
            {

                this.lstgoiXN = BioNet_Bus.GetDanhsachGoiDichVuChung();
                this.LookUpEditGoiXN.DataSource = null;
                this.LookUpEditGoiXN.DataSource = this.lstgoiXN;
            }
            catch { }
        }
        private void GCDanhSachMauDuongTinh_Click(object sender, EventArgs e)
        {

        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string MaDonVi = String.Empty;
                if (this.txtDonVi.EditValue.ToString() == "all")
                {
                    if (this.txtChiCuc.EditValue.ToString() == "all")
                    {
                        MaDonVi = "all";
                    }
                    else
                    {
                        MaDonVi = this.txtChiCuc.EditValue.ToString();
                    }
                }
                else
                {
                    MaDonVi = this.txtDonVi.EditValue.ToString();
                }
                GCDanhSachMauDuongTinh.DataSource = null;
                GCDanhSachMauDuongTinh.DataSource = BioNet_Bus.GetDanhSachDuongTinh(dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date, cbbDichVu.EditValue.ToString(), MaDonVi, txtMin.Text, txtMax.Text);
            }
            catch
            {

            }
            
        }

        private void FrmQuanLyMauDuongTinh_Load(object sender, EventArgs e)
        {
            this.LoadGoiDichVuXetNGhiem();
            cbbDichVu.Properties.DataSource = BioNet_Bus.GetDanhSachDichVu(false);
            this.txtChiCuc.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            dllNgay.tungay.Value = DateTime.Now;
            dllNgay.denngay.Value = DateTime.Now;
            this.txtChiCuc.EditValue = "all";
        }

        private void txtChiCuc_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit sear = sender as SearchLookUpEdit;
                var value = sear.EditValue.ToString();
                this.txtDonVi.Properties.DataSource = null;
                this.txtDonVi.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi(value.ToString());
                this.txtDonVi.EditValue = "all";
                if (txtDonVi.EditValue.ToString() != "all")
                {
                }
                else
                {
                }
            }
            catch { }
        }

        private void btnXuatFile_Click(object sender, EventArgs e)
        {
            if (this.GVDanhSachDuongTinh.RowCount > 0)
            {
                this.ExportDataToExcelFile();
            }
            else
            {
                XtraMessageBox.Show("Không có dữ liệu, vui lòng lấy dữ liệu lại và kiểm tra điều kiện lọc.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExportDataToExcelFile()
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "Excel File(*.xlsx)|*.xlsx";
            ofd.FileName = "QuanLyMauDuongTinh" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName.Length > 0)
                {
                    try
                    {

                        this.GVDanhSachDuongTinh.ExportToXlsx(ofd.FileName);
                    }
                    catch
                    {
                        XtraMessageBox.Show("Không thể lưu file này! Vui lòng chọn đường dẫn khác.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

    }
}