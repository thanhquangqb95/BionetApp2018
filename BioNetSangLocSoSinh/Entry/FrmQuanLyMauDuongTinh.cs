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
using BioNetModel;
using BioNetSangLocSoSinh.Reports;
using DevExpress.XtraSplashScreen;
using BioNetSangLocSoSinh.DiaglogFrm;

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmQuanLyMauDuongTinh : DevExpress.XtraEditors.XtraForm
    {
        public FrmQuanLyMauDuongTinh(PsEmployeeLogin emp)
        {
            InitializeComponent();
            Emp = emp;
        }
        private PsEmployeeLogin Emp = new PsEmployeeLogin();
        private List<PSDanhMucGoiDichVuChung> lstgoiXN = new List<PSDanhMucGoiDichVuChung>();
        private List<PsDanhSachMauDuongTinh> lstMauDT = new List<PsDanhSachMauDuongTinh>();
        private string TenDV = string.Empty;
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
                        TenDV = "Trung tâm Sàng lọc Bionet";
                    }
                    else
                    {
                        MaDonVi = this.txtChiCuc.EditValue.ToString();
                        TenDV = txtChiCuc.DisplayRectangle.ToString();
                    }
                }
                else
                {
                    MaDonVi = this.txtDonVi.EditValue.ToString();
                    TenDV = txtDonVi.DisplayRectangle.ToString();
                }
                if(cbbDichVu.EditValue==null)
                {
                    XtraMessageBox.Show("Yêu cầu chọn dịch vụ thống kê.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    
                    if (string.IsNullOrEmpty(txtMin.Text) && string.IsNullOrEmpty(txtMax.Text))
                    {
                        XtraMessageBox.Show("Yêu cầu chọn khoảng giá trị.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        SplashScreenManager.ShowForm(typeof(WaitingLoadData), true, false);
                        DateTime d1 = DateTime.Now;
                        lstMauDT = new List<PsDanhSachMauDuongTinh>();
                        string kq = cbbKetQua.EditValue.ToString();
                        if (cbbKetQua.EditValue.ToString().Equals("True"))
                        {
                            lstMauDT = BioNet_Bus.GetDanhSachDuongTinh(dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date, cbbDichVu.EditValue.ToString(), MaDonVi, txtMin.Text, txtMax.Text);
                        }
                        else
                        {
                            lstMauDT = BioNet_Bus.GetDanhSachDuongTinhNew(dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date, cbbDichVu.EditValue.ToString(), MaDonVi, txtMin.Text, txtMax.Text);
                        }
                        
                        GCDanhSachMauDuongTinh.DataSource = null;
                        GCDanhSachMauDuongTinh.DataSource = lstMauDT;
                        DateTime d2 = DateTime.Now;
                        TimeSpan kt = d2 - d1;
                        txtDate.Text = string.Format("{0:00}:{1:00}:{2:00}", kt.Hours, kt.Minutes, kt.Seconds);
                        SplashScreenManager.CloseForm();
                    }
                }                              
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
            cbbKetQua.EditValue = false;
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
            ofd.FileName = "QuanLyMauDuongTinh" + DateTime.Now.ToString("yyyy.MM.dd.HH.mm") + ".xlsx";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName.Length > 0)
                {
                    try
                    {
                        rptQuanLyMauDuongTinh dt = new rptQuanLyMauDuongTinh();
                        dt.DataSource = lstMauDT;
                        dt.Parameters["Thoigianxuatds"].Value = DateTime.Now;
                        if (cbbKetQua.EditValue.ToString().Equals("True"))
                        {
                            dt.Parameters["TieuDe"].Value = "DANH SÁCH MẪU DƯƠNG TINH ĐÃ ĐẦY ĐỦ KẾT QUẢ";
                        }
                        else
                        {
                            dt.Parameters["TieuDe"].Value = "DANH SÁCH MẪU DƯƠNG TINH CHƯA ĐẦY ĐỦ KQ";
                        }
                           
                        dt.Parameters["Thoigiancapma"].Value = dllNgay.tungay.Value.ToString("dd/MM/yyyy") + " đến "+ dllNgay.denngay.Value.ToString("dd/MM/yyyy");
                        dt.Parameters["TenNV"].Value = Emp.EmployeeName;
                        dt.ExportToXlsx(ofd.FileName);
                    }
                    catch
                    {
                        XtraMessageBox.Show("Không thể lưu file này! Vui lòng chọn đường dẫn khác.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cbbDichVu.EditValue = null;
            txtMin.Text = string.Empty;
            txtMax.Text = string.Empty;
            txtChiCuc.EditValue = "all";
            GCDanhSachMauDuongTinh.DataSource = null;
        }
    }
}