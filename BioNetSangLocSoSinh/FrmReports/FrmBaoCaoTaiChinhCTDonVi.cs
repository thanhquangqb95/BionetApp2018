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
using BioNetSangLocSoSinh.Reports.RepostsBaoCao;
using BioNetModel;
using System.IO;
using BioNetSangLocSoSinh.Reports;
using DevExpress.XtraSplashScreen;
using BioNetSangLocSoSinh.DiaglogFrm;
using BioNetModel.Data;

namespace BioNetSangLocSoSinh.FrmReports
{
    public partial class FrmBaoCaoTaiChinhCTDonVi : DevExpress.XtraEditors.XtraForm
    {
        public FrmBaoCaoTaiChinhCTDonVi(string MaNhanVien)
        {
            MaNV = MaNhanVien;
            InitializeComponent();
        } List<PsBaoCaoTaiChinh> kq = new List<PsBaoCaoTaiChinh>();
        public string MaNV;
        public string MaDonVi = String.Empty;
        public string TenDonVi = String.Empty;
        DateTime T1TuNgay = new DateTime();
        DateTime T1DenNgay = new DateTime();
        DateTime T2TuNgay = new DateTime();
        DateTime T2DenNgay = new DateTime();
        DateTime T3TuNgay = new DateTime();
        DateTime T3DenNgay = new DateTime();
        DateTime T4TuNgay = new DateTime();
        DateTime T4DenNgay = new DateTime();
        DateTime T5TuNgay = new DateTime();
        DateTime T5DenNgay = new DateTime();

        private void btnLayDuLieu_Click(object sender, EventArgs e)
        {
            LoadDuLieu();
        }
        private void LoadDuLieu()
        {

            
            if (this.txtDonVi.EditValue.ToString() == "all")
            {
                if (this.txtChiCuc.EditValue.ToString() == "all")
                {
                    this.lblTenDonVi.Text = "Thống kê toàn bộ trung tâm";
                    TenDonVi = "Trung tâm Bionet Tổng hợp";
                    MaDonVi = "all";
                }
                else
                {
                    this.lblTenDonVi.Text = "Thống kê chi cục " + this.txtChiCuc.Text.ToString();
                    TenDonVi ="Chi cục "+txtChiCuc.Text.ToString();
                    MaDonVi = this.txtChiCuc.EditValue.ToString();
                }
            }
            else
            {
                this.lblTenDonVi.Text = "Thống kê đơn vị " + this.txtDonVi.Text.ToString();

                TenDonVi = "Đơn vị " + txtDonVi.Text.ToString();
                MaDonVi = this.txtDonVi.EditValue.ToString();
            }
            SplashScreenManager.ShowForm(this, typeof(WaitingLoadData), true, true, false);
            kq = BioNetBLL.BioNet_Bus.GetBaoCaoTaiChinh(MaDonVi, this.dllNgay.tungay.Value.Date, this.dllNgay.denngay.Value.Date);
            SplashScreenManager.CloseForm();
            this.GCBaoCaoTaiChinh.DataSource = kq;
            

        }

        private void FrmBaoCaoTaiChinhCTDonVi_Load(object sender, EventArgs e)
        {
            this.txtChiCuc.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            this.txtDonVi.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi("all");
            this.txtDonVi.EditValue = "all";
            this.txtChiCuc.EditValue = "all";
            this.repositoryItemGridLookUpEditMaGoi.DataSource = BioNet_Bus.GetDanhsachGoiDichVuChung(); ;
            this.repositoryItemGridLookUpEditMaGoi.DisplayMember = "TenGoiDichVuChung";
            this.repositoryItemGridLookUpEditMaGoi.ValueMember = "IDGoiDichVuChung";
            this.LoadDuLieu();
            AddItemForm();
        }

        private void txtChiCuc_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit sear = sender as SearchLookUpEdit;
                var value = sear.EditValue.ToString();
                this.txtDonVi.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi(value.ToString());
                this.txtDonVi.EditValue = "all";
            }
            catch { }
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            rptBaoCaoTaiChinhDonViChiTiet bc = XuatBaoCaoCT();

        }
        private rptBaoCaoTaiChinhDonViChiTiet XuatBaoCaoCT()
        {
            string Tong;
            var tt = BioNet_Bus.GetThongTinTrungTam();
            byte[] b = tt.Header.ToArray();
            MemoryStream ms = new MemoryStream(b);
            Image img = Image.FromStream(ms);
            rptBaoCaoTaiChinhDonViChiTiet bc = new rptBaoCaoTaiChinhDonViChiTiet(img);
            try
            {
                var nv = BioNetBLL.BioNet_Bus.GetThongTinNhanVien(MaNV);
                bc.Parameters["TenNV"].Value = nv.EmployeeName;
                bc.DataSource = kq;
                bc.Parameters["MaDV"].Value = MaDonVi.Contains("all") ? "1B1" : MaDonVi;
                bc.Parameters["TenDV"].Value = TenDonVi;
                Tong = kq.Sum(x => x.DonGia).ToString();
                bc.Parameters["TongTien"].Value = String.Format(Tong, "{0:0,0}");
                bc.Parameters["TongSoMau"].Value = kq.Count();
                string tungay = this.dllNgay.tungay.Value.Date.ToShortDateString();
                string dennay = this.dllNgay.denngay.Value.Date.ToShortDateString();
                bc.Parameters["ThoiGian"].Value = tungay + " đến " + dennay;
                bc.Parameters["benh2"].Value = kq.Where(x => x.MaGoiXN.Contains("DVGXN0002")).Count();
                bc.Parameters["benh3"].Value = kq.Where(x => x.MaGoiXN.Contains("DVGXN0003")).Count();
                bc.Parameters["benh5"].Value = kq.Where(x => x.MaGoiXN.Contains("DVGXN0004")).Count();
                bc.Parameters["MauThuLai"].Value = kq.Where(x => x.MaGoiXN.Contains("DVGXN0001")).Count();
                bc.Parameters["Ngay"].Value = DateTime.Now.Day;
                bc.Parameters["Thang"].Value = DateTime.Now.Month;
                bc.Parameters["Nam"].Value = DateTime.Now.Year;
                Reports.frmDanhSachDaCapMa rpt = new Reports.frmDanhSachDaCapMa(bc);
                rpt.ShowDialog();
            }
            catch
            {

            }
            return bc;
          
        }
        private rptBaoCaoTaiChinhDonViChiTiet XuatBaoCaoCTDV(string madv,string tenDV)
        {
            string Tong;
            var tt = BioNet_Bus.GetThongTinTrungTam();
            byte[] b = tt.Header.ToArray();
            MemoryStream ms = new MemoryStream(b);
            Image img = Image.FromStream(ms);
            rptBaoCaoTaiChinhDonViChiTiet bc = new rptBaoCaoTaiChinhDonViChiTiet(img);
            try
            {
                var kqdv = kq.Where(x => x.MaDVCS.Contains(madv));
                if (kqdv.Count() !=0)
                {
                    var nv = BioNetBLL.BioNet_Bus.GetThongTinNhanVien(MaNV);
                    bc.Parameters["TenNV"].Value = nv.EmployeeName;
                    bc.DataSource = kqdv;
                    bc.Parameters["MaDV"].Value = madv.Contains("all") ? "1B1" : madv;
                    bc.Parameters["TenDV"].Value = tenDV;
                    Tong = kqdv.Sum(x => x.DonGia).ToString();
                    bc.Parameters["TongTien"].Value = String.Format(Tong, "{0:0,0}");
                    bc.Parameters["TongSoMau"].Value = kqdv.Count();
                    string tungay = this.dllNgay.tungay.Value.Date.ToShortDateString();
                    string dennay = this.dllNgay.denngay.Value.Date.ToShortDateString();
                    bc.Parameters["ThoiGian"].Value = tungay + " đến " + dennay;
                    bc.Parameters["benh2"].Value = kqdv.Where(x => x.MaGoiXN.Contains("DVGXN0002")).Count();
                    bc.Parameters["benh3"].Value = kqdv.Where(x => x.MaGoiXN.Contains("DVGXN0003")).Count();
                    bc.Parameters["benh5"].Value = kqdv.Where(x => x.MaGoiXN.Contains("DVGXN0004")).Count();
                    bc.Parameters["MauThuLai"].Value = kqdv.Where(x => x.MaGoiXN.Contains("DVGXN0001")).Count();
                    bc.Parameters["Ngay"].Value = DateTime.Now.Day;
                    bc.Parameters["Thang"].Value = DateTime.Now.Month;
                    bc.Parameters["Nam"].Value = DateTime.Now.Year;
                }
                else
                {
                    bc = null;
                }
                
            }
            catch
            {

            }
            return bc;

        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
             T1TuNgay = new DateTime(this.dllNgay.tungay.Value.Year,this.dllNgay.tungay.Value.Month, 1);
             T1DenNgay = new DateTime();
           
            switch (T1TuNgay.DayOfWeek.ToString())
            {
                case "Monday":
                    T1DenNgay = T1TuNgay.AddDays(6);
                    break;
                case "Tuesday":
                    T1DenNgay = T1TuNgay.AddDays(5);
                    break;
                case "Wednesday":
                    T1DenNgay = T1TuNgay.AddDays(4);
                    break;
                case "Thursday":
                    T1DenNgay = T1TuNgay.AddDays(3);
                    break;
                case "Friday":
                    T1DenNgay = T1TuNgay.AddDays(2);
                    break;
                case "Saturday":
                    T1DenNgay = T1TuNgay.AddDays(1);
                    break;
                case "Sunday":
                    T1DenNgay = T1TuNgay.AddDays(0);
                    break;
            }
             T2TuNgay = T1DenNgay.AddDays(1);
             T2DenNgay = T2TuNgay.AddDays(6);
             T3TuNgay = T2DenNgay.AddDays(1);
             T3DenNgay = T3TuNgay.AddDays(6);
             T4TuNgay = T3DenNgay.AddDays(1);
             T4DenNgay = T4TuNgay.AddDays(6);
             T5TuNgay = T4DenNgay.AddDays(1);
             T5DenNgay = T5TuNgay.AddMonths(1);
            T5DenNgay = T5DenNgay.AddDays(-(T5DenNgay.Day));
            if(T1TuNgay== this.dllNgay.tungay.Value.Date && T5DenNgay == this.dllNgay.denngay.Value.Date)
            {
                panelControl3.Visible = true;
                lblBCThang.Text = "Báo cáo tháng " + this.dllNgay.tungay.Value.Month + " năm " + this.dllNgay.tungay.Value.Year;
                dateT1TuNgay.EditValue = T1TuNgay;
                dateT2TuNgay.EditValue = T2TuNgay;
                dateT3TuNgay.EditValue = T3TuNgay;
                dateT4TuNgay.EditValue = T4TuNgay;
                dateT5TuNgay.EditValue = T5TuNgay;
                dateT1DenNgay.EditValue = T1DenNgay;
                dateT2DenNgay.EditValue = T2DenNgay;
                dateT3DenNgay.EditValue = T3DenNgay;
                dateT4DenNgay.EditValue = T4DenNgay;
                dateT5DenNgay.EditValue = T5DenNgay;
            }
            else
            {
                MessageBox.Show("Yêu cầu chọn đúng tháng cần lấy dữ liệu", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK);
            }
      

        }
        private List<PsRptBaoCaoTaiChinhCT> GetBCChiTiet(List<PsBaoCaoTaiChinh> bc)
        {

            List<PsRptBaoCaoTaiChinhCT> tc = new List<PsRptBaoCaoTaiChinhCT>();
            try
            {
                 // var IDCoSo = bc.Select(g => g.MaDVCS).Distinct().ToList();
                    var temp = (from a in bc select new { a.TenDVCS, a.MaDVCS }).ToList();

                    var IDCoSo = temp.Distinct().ToList();
                    int i = 1;
                    foreach (var k in IDCoSo)
                    {

                     PsRptBaoCaoTaiChinhCT ct = new PsRptBaoCaoTaiChinhCT();
                     T1TuNgay= dateT1TuNgay.DateTime;
                     T2TuNgay = dateT2TuNgay.DateTime;
                     T3TuNgay = dateT3TuNgay.DateTime;
                     T4TuNgay= dateT4TuNgay.DateTime;
                     T5TuNgay= dateT5TuNgay.DateTime;
                     T1DenNgay= dateT1DenNgay.DateTime;
                     T2DenNgay = dateT2DenNgay.DateTime;
                     T3DenNgay= dateT3DenNgay.DateTime;
                     T4DenNgay = dateT4DenNgay.DateTime;
                     T5DenNgay = dateT5DenNgay.DateTime;
                     ct.MaDVCS = k.MaDVCS;
                        ct.TenDVCS = k.TenDVCS;
                        var goi = BioNet_Bus.GetDanhsachGoiDichVuTrungTam(k.MaDVCS);
                        var phieu = bc.Where(g => g.MaDVCS.Contains(ct.MaDVCS)).ToList();

                        ct.STT = i++;
                        string b2 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0002")).Select(x => x.DonGia).FirstOrDefault().ToString();
                        string b3 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0003")).Select(x => x.DonGia).FirstOrDefault().ToString();
                        string b5 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0004")).Select(x => x.DonGia).FirstOrDefault().ToString();

                        ct.Gia2Benh = Convert.ToInt32(goi.Where(x => x.IDGoiDichVuChung.Contains("DVGXN0002")).Select(x => x.DonGia).FirstOrDefault().ToString());
                        ct.Gia3Benh = Convert.ToInt32(goi.Where(x => x.IDGoiDichVuChung.Contains("DVGXN0003")).Select(x => x.DonGia).FirstOrDefault().ToString());
                        ct.Gia5Benh = Convert.ToInt32(goi.Where(x => x.IDGoiDichVuChung.Contains("DVGXN0004")).Select(x => x.DonGia).FirstOrDefault().ToString());

                        ct.SL2Benh = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0002") && x.NgayCoKQ.Value.Date >= T1TuNgay.Date && x.NgayCoKQ.Value.Date <= T5DenNgay.Date).ToList().Count().ToString();
                        ct.SL2BenhT1 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0002") && x.NgayCoKQ.Value.Date >= T1TuNgay.Date && x.NgayCoKQ.Value.Date <= T1DenNgay.Date).ToList().Count().ToString();
                        ct.SL2BenhT2 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0002") && x.NgayCoKQ.Value.Date >= T2TuNgay.Date && x.NgayCoKQ.Value.Date <= T2DenNgay.Date).ToList().Count().ToString();
                        ct.SL2BenhT3 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0002") && x.NgayCoKQ.Value.Date >= T3TuNgay.Date && x.NgayCoKQ.Value.Date <= T3DenNgay.Date).ToList().Count().ToString();
                        ct.SL2BenhT4 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0002") && x.NgayCoKQ.Value.Date >= T4TuNgay.Date && x.NgayCoKQ.Value.Date <= T4DenNgay.Date).ToList().Count().ToString();
                        ct.SL2BenhT5 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0002") && x.NgayCoKQ.Value.Date >= T5TuNgay.Date && x.NgayCoKQ.Value.Date <= T5DenNgay.Date).ToList().Count().ToString();

                        ct.SL3Benh = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0003") && x.NgayCoKQ.Value.Date >= T1TuNgay.Date && x.NgayCoKQ.Value.Date <= T5DenNgay.Date).ToList().Count().ToString();
                        ct.SL3BenhT1 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0003") && x.NgayCoKQ.Value.Date >= T1TuNgay.Date && x.NgayCoKQ.Value.Date <= T1DenNgay.Date).ToList().Count().ToString();
                        ct.SL3BenhT2 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0003") && x.NgayCoKQ.Value.Date >= T2TuNgay.Date && x.NgayCoKQ.Value.Date <= T2DenNgay.Date).ToList().Count().ToString();
                        ct.SL3BenhT3 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0003") && x.NgayCoKQ.Value.Date >= T3TuNgay.Date && x.NgayCoKQ.Value.Date <= T3DenNgay.Date).ToList().Count().ToString();
                        ct.SL3BenhT4 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0003") && x.NgayCoKQ.Value.Date >= T4TuNgay.Date && x.NgayCoKQ.Value.Date <= T4DenNgay.Date).ToList().Count().ToString();
                        ct.SL3BenhT5 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0003") && x.NgayCoKQ.Value.Date >= T5TuNgay.Date && x.NgayCoKQ.Value.Date <= T5DenNgay.Date).ToList().Count().ToString();

                        ct.SL5Benh = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0004") && x.NgayCoKQ.Value.Date >= T1TuNgay.Date && x.NgayCoKQ.Value.Date <= T5DenNgay.Date).ToList().Count().ToString();
                        ct.SL5BenhT1 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0004") && x.NgayCoKQ.Value.Date >= T1TuNgay.Date && x.NgayCoKQ.Value.Date <= T1DenNgay.Date).ToList().Count().ToString();
                        ct.SL5BenhT2 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0004") && x.NgayCoKQ.Value.Date >= T2TuNgay.Date && x.NgayCoKQ.Value.Date <= T2DenNgay.Date).ToList().Count().ToString();
                        ct.SL5BenhT3 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0004") && x.NgayCoKQ.Value.Date >= T3TuNgay.Date && x.NgayCoKQ.Value.Date <= T3DenNgay.Date).ToList().Count().ToString();
                        ct.SL5BenhT4 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0004") && x.NgayCoKQ.Value.Date >= T4TuNgay.Date && x.NgayCoKQ.Value.Date <= T4DenNgay.Date).ToList().Count().ToString();
                        ct.SL5BenhT5 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0004") && x.NgayCoKQ.Value.Date >= T5TuNgay.Date && x.NgayCoKQ.Value.Date <= T5DenNgay.Date).ToList().Count().ToString();

                        ct.SLThuMauLai = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0001") && x.NgayCoKQ.Value.Date >= T1TuNgay.Date && x.NgayCoKQ.Value.Date <= T5DenNgay.Date).ToList().Count().ToString();
                        ct.SLThuMauLaiT1 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0001") && x.NgayCoKQ.Value.Date >= T1TuNgay.Date && x.NgayCoKQ.Value.Date <= T1DenNgay.Date).ToList().Count().ToString();
                        ct.SLThuMauLaiT2 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0001") && x.NgayCoKQ.Value.Date >= T2TuNgay.Date && x.NgayCoKQ.Value.Date <= T2DenNgay.Date).ToList().Count().ToString();
                        ct.SLThuMauLaiT3 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0001") && x.NgayCoKQ.Value.Date >= T3TuNgay.Date && x.NgayCoKQ.Value.Date <= T3DenNgay.Date).ToList().Count().ToString();
                        ct.SLThuMauLaiT4 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0001") && x.NgayCoKQ.Value.Date >= T4TuNgay.Date && x.NgayCoKQ.Value.Date <= T4DenNgay.Date).ToList().Count().ToString();
                        ct.SLThuMauLaiT5 = phieu.Where(x => x.MaGoiXN.Contains("DVGXN0001") && x.NgayCoKQ.Value.Date >= T5TuNgay.Date && x.NgayCoKQ.Value.Date <= T5DenNgay.Date).ToList().Count().ToString();

                        ct.TongTienT1 = ((Convert.ToInt32(ct.SL2BenhT1) * ct.Gia2Benh) +
                                        (Convert.ToInt32(ct.SL3BenhT1) * ct.Gia3Benh) +
                                        (Convert.ToInt32(ct.SL5BenhT1) * ct.Gia5Benh));
                        ct.TongTienT2 = ((Convert.ToInt32(ct.SL2BenhT2) * ct.Gia2Benh) +
                                        (Convert.ToInt32(ct.SL3BenhT2) * ct.Gia3Benh) +
                                        (Convert.ToInt32(ct.SL5BenhT2) * ct.Gia5Benh));
                        ct.TongTienT3 = ((Convert.ToInt32(ct.SL2BenhT3) * ct.Gia2Benh) +
                                        (Convert.ToInt32(ct.SL3BenhT3) * ct.Gia3Benh) +
                                        (Convert.ToInt32(ct.SL5BenhT3) * ct.Gia5Benh));
                        ct.TongTienT4 = ((Convert.ToInt32(ct.SL2BenhT4) * ct.Gia2Benh) +
                                        (Convert.ToInt32(ct.SL3BenhT4) * ct.Gia3Benh) +
                                        (Convert.ToInt32(ct.SL5BenhT4) * ct.Gia5Benh));
                        ct.TongTienT5 = ((Convert.ToInt32(ct.SL2BenhT5) * ct.Gia2Benh) +
                                        (Convert.ToInt32(ct.SL3BenhT5) * ct.Gia3Benh) +
                                        (Convert.ToInt32(ct.SL5BenhT5) * ct.Gia5Benh));
                        ct.TongTienDV = ct.TongTienT1 + ct.TongTienT2 + ct.TongTienT3 + ct.TongTienT4 + ct.TongTienT5;
                        tc.Add(ct);
                    }
                
            }
            catch
            {

            }
            return tc;

        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            List<PsRptBaoCaoTaiChinhCT> tc = GetBCChiTiet(kq);
            rptBaoCaoTaiChinhTongHop rpt = new rptBaoCaoTaiChinhTongHop();
            rpt.DataSource = tc;
            rpt.Parameters["BaoCao"].Value = "(Báo cáo tháng " + this.dllNgay.tungay.Value.Month + "/" + this.dllNgay.tungay.Value.Year + ")";
            rpt.Parameters["T1TuNgay"].Value = dateT1TuNgay.DateTime.ToShortDateString();
            rpt.Parameters["T1DenNgay"].Value = dateT1DenNgay.DateTime.ToShortDateString();
            rpt.Parameters["T2TuNgay"].Value = dateT2TuNgay.DateTime.ToShortDateString();
            rpt.Parameters["T2DenNgay"].Value = dateT2TuNgay.DateTime.ToShortDateString();
            rpt.Parameters["T3TuNgay"].Value = dateT3TuNgay.DateTime.ToShortDateString();
            rpt.Parameters["T3DenNgay"].Value = dateT3TuNgay.DateTime.ToShortDateString();
            rpt.Parameters["T4TuNgay"].Value = dateT4TuNgay.DateTime.ToShortDateString();
            rpt.Parameters["T4DenNgay"].Value = dateT4TuNgay.DateTime.ToShortDateString();
            rpt.Parameters["T5TuNgay"].Value = dateT5TuNgay.DateTime.ToShortDateString();
            rpt.Parameters["T5DenNgay"].Value = dateT5TuNgay.DateTime.ToShortDateString();
            rpt.Parameters["TimeThang"].Value = "Tổng tháng " + this.dllNgay.tungay.Value.Month;
            Reports.frmDanhSachDaCapMa bc = new Reports.frmDanhSachDaCapMa(rpt);
            bc.ShowDialog();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            panelControl3.Visible = false;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

            //List<string> DSDV = new List<string>();
            SplashScreenManager.ShowForm(this, typeof(WaitingLoadData), true, true, false);
            if (MaDonVi.Length<8)
            {
                var DSDV = BioNet_Bus.GetDieuKienLocBaoCao_DonVi(MaDonVi).ToList();
                foreach (var madvcs in DSDV)
                {
                    if(!madvcs.MaDVCS.Contains("all"))
                    {
                        rptBaoCaoTaiChinhDonViChiTiet rpt = XuatBaoCaoCTDV(madvcs.MaDVCS, madvcs.TenDVCS);
                        if(rpt!=null)
                        {
                            frmReportCTTaiChinh.SaveFilePDF(rpt, madvcs.TenDVCS, this.dllNgay.tungay.Value.Month.ToString(), this.dllNgay.tungay.Value.Year.ToString());
                        }
                        
                    }                 
                }
            }
            else
            {
                rptBaoCaoTaiChinhDonViChiTiet rpt = XuatBaoCaoCTDV(MaDonVi, TenDonVi);
                if (rpt != null)
                {
                    frmReportCTTaiChinh.SaveFilePDF(rpt, TenDonVi, this.dllNgay.tungay.Value.Month.ToString(), this.dllNgay.tungay.Value.Year.ToString());
                }

            }
            SplashScreenManager.CloseForm();

        }

        private void btnGuiMail_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn là sẽ gửi mail", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string Error = null;
                SplashScreenManager.ShowForm(this, typeof(WaitingformMail), true, true, false);
                if (MaDonVi.Length < 8)
                {
                    var DSDV = BioNet_Bus.GetDieuKienLocBaoCao_DonVi(MaDonVi).ToList();
                    var dvMail = DSDV.Where(x => x.isGuiMailTC == true);
                    foreach (var madvcs in dvMail)
                    {
                        if (!madvcs.MaDVCS.Contains("all"))
                        {
                            rptBaoCaoTaiChinhDonViChiTiet rpt = XuatBaoCaoCTDV(madvcs.MaDVCS, madvcs.TenDVCS);
                            if(rpt!=null)
                            {
                                frmReportCTTaiChinh.SaveFilePDF(rpt, madvcs.TenDVCS, this.dllNgay.tungay.Value.Month.ToString(), this.dllNgay.tungay.Value.Year.ToString());
                                string paththumuc = Application.StartupPath + "\\BaoCaoCTTaiChinhDonVi\\" + madvcs.TenDVCS + @"\" + "BaoCaoTaiChinh_" + madvcs.TenDVCS + "_Thang" + this.dllNgay.tungay.Value.Month.ToString() + "Nam" + this.dllNgay.tungay.Value.Year.ToString() + ".pdf";
                                int kq=GuiMail(madvcs.MaDVCS, paththumuc);
                                if (kq == 1)
                                {
                                    break;
                                }
                                else if (kq == 2)
                                {
                                    Error = Error + "\r\n" + "Lỗi: Email của đơn vị " + madvcs.TenDVCS + " không tồn tại.";
                                    break;
                                }
                                else if (kq == 5)
                                {
                                    Error = Error + "\r\n" + "Lỗi: Gửi Email cho đơn vị " + madvcs.TenDVCS + " bị lỗi.";
                                    break;
                                }
                            }                          
                        }
                    }
                }
                else
                {
                    var DSDV = BioNet_Bus.GetThongTinDonViCoSo(MaDonVi);
                   if(DSDV.isGuiMailTC==true)
                    {
                        rptBaoCaoTaiChinhDonViChiTiet rpt = XuatBaoCaoCTDV(MaDonVi, TenDonVi);
                        if (rpt != null)
                        {
                            frmReportCTTaiChinh.SaveFilePDF(rpt, TenDonVi, this.dllNgay.tungay.Value.Month.ToString(), this.dllNgay.tungay.Value.Year.ToString());
                            string paththumuc = Application.StartupPath + "\\BaoCaoCTTaiChinhDonVi\\" + TenDonVi + @"\" + "BaoCaoTaiChinh_" + TenDonVi + "_Thang" + this.dllNgay.tungay.Value.Month.ToString() + "Nam" + this.dllNgay.tungay.Value.Year.ToString() + ".pdf";
                            int kq = (GuiMail(MaDonVi, paththumuc));
                            if (kq == 1)
                            {
                            }
                            else if (kq == 2)
                            {
                                Error = Error + "\r\n" + "Lỗi: Email của đơn vị " + TenDonVi + " không tồn tại.";
                            }
                            else if (kq == 5)
                            {
                                Error = Error + "\r\n" + "Lỗi: Gửi Email cho đơn vị " + TenDonVi + " bị lỗi.";
                            }
                        }
                    }
                   
                }
            
            SplashScreenManager.CloseForm();
                if (String.IsNullOrEmpty(Error))
                {
                    XtraMessageBox.Show("Danh sách gửi mail tài chính lỗi: \r\n" + Error, "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("Danh sách gửi mail tài chính lỗi: \r\n" + Error, "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
           
        }
        private int GuiMail(string MaDVCS,string pathbc)
        {
            var donvi = BioNet_Bus.GetThongTinDonViCoSo(MaDVCS);
            var tt = BioNet_Bus.GetThongTinTrungTam();
            if (tt != null)
            {
                string fromAddress = tt.EmailTC;
                string passEMail =tt.PassEmailTC;
                string tieude = "Bionet gửi công nợ XN SLSS  của BV" + donvi.TenDVCS + " " + DateTime.Now.ToShortDateString();
                string noidung = "<p style='font-size:12.8px;margin:6pt 0in;text-align:justify'><font face='times new roman,serif' size='4' color='black'>" + "Kính gửi:<b> " + donvi.TenDVCS + "</b>,</font></p>" +"<p style='font-size:12.8px;margin:6pt 0in;text-align:justify '><font face='times new roman,serif' size='4' color='black'>" + "<br></br>" + "Công ty Bionet Việt Nam gửi anh/chị công nợ XN SLSS tháng " + this.dllNgay.tungay.Value.Month.ToString()+" năm "+this.dllNgay.tungay.Value.Year.ToString() + " tính từ ngày " + this.dllNgay.tungay.Value.ToShortDateString() + " đến ngày " + this.dllNgay.denngay.Value.ToShortDateString() + "." + "</font></p>" +
                   "<p style='font-size:12.8px;margin:6pt 0in;text-align:justify'><font face='times new roman,serif' size = '4' color='black'>" + "Anh/Chị vui lòng kiểm tra file đính kèm để biết thêm thông tin chi tiết!" + "</font></p>" +
                  "<p style='font-size:12.8px;margin:6pt 0in;text-align:justify'><font face='times new roman,serif' size = '4' color='black'>" + "Mọi thắc mắc về công nợ, anh / chị vui lòng liên hệ Ms Hồng phòng kế toán. SĐT: 024.6674.7102 / Mail: hong_ketoan@bionet.vn" + "</font></p>" + "<p style='font-size:12.8px;margin:6pt 0in;text-align:justify'><font face='times new roman,serif' size = '4' color='black'>" + "<br></br>" + "Trân trọng." + "</font></p>" +
                  "<p>Phòng Kế Toán - Công ty Bionet Việt Nam</p>"
                  +"<br></br>" +
                   "<p style='font-size:12.8px;margin:6pt 0in;text-align:justify;font-style:italic'><font face='times new roman,serif' size = '4' color='black'>" + "Lưu ý: Đây là email tự động được gửi từ phần mềm SLSS của Trung tâm XN SLSS Bionet Việt Nam, vui lòng không trả lời email này." + "</font></p>";
                string madvcs = MaDVCS;
                // string MailKH = BioNet_Bus.GetThongTinMailDonViCoSo(madvcs);
                string MailKH = donvi.EmailTC;
                int kq = DataSync.BioNetSync.GuiMail.Send_Email_With_Attachment(MailKH, fromAddress, passEMail, pathbc, tieude, noidung);
                //File.Create(pathzip).Close();
                return kq;
            }
            else
            {
                return 5;
            }

        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            List<PsRptBaoCaoHoaDon> bc = new List<PsRptBaoCaoHoaDon>();

            
            var temp = (from a in kq select new { a.TenDVCS, a.MaDVCS,a.DiaChiDVCS }).ToList();
            var IDCoSo = temp.Distinct().ToList();
            foreach(var dv in IDCoSo)
            {
                PsRptBaoCaoHoaDon ct = new PsRptBaoCaoHoaDon();
                ct.TenDVCS = dv.TenDVCS;
                ct.MaDVCS = dv.MaDVCS;
                ct.DiaChiDVCS = dv.DiaChiDVCS;
                var phieu = kq.Where(g => g.MaDVCS.Contains(ct.MaDVCS)).ToList();
                var goi = (from a in phieu select new { a.MaGoiXN, a.TenGoiDichVuChung }).ToList();
                var ctgoi = goi.Distinct().ToList();
                List<PsRptBaoCaoHoaDonCT> listct = new List<PsRptBaoCaoHoaDonCT>();
                foreach (var g in ctgoi)
                {
                    PsRptBaoCaoHoaDonCT bcct = new PsRptBaoCaoHoaDonCT();
                    
                    switch (g.MaGoiXN)
                    {
                        case "DVGXN0002":
                            {
                                bcct.TenGoiDV = "Gói XN cơ bản 02 bệnh";
                                break;
                            }
                        case "DVGXN0003":
                            {
                                bcct.TenGoiDV = "Gói XN cơ bản 03 bệnh";
                                break;
                            }
                        case "DVGXN0004":
                            {
                                bcct.TenGoiDV = "Gói XN cơ bản 05 bệnh";
                                break;
                            }
                        default:
                            {
                                bcct.TenGoiDV = "Gói XN lại";
                                break;
                            }
                    }

                    bcct.IDGoiDV = g.MaGoiXN;
                    bcct.SL = phieu.Where(x => x.MaGoiXN==g.MaGoiXN).ToList().Count();
                    bcct.DonGia = Convert.ToInt32(phieu.Where(y => y.MaGoiXN == g.MaGoiXN).Select(x => x.DonGia).FirstOrDefault());
                    bcct.TongTien = bcct.SL * bcct.DonGia;
                    listct.Add(bcct);
                }
                ct.PsRptBaoCaoHoaDonCT = listct;
                bc.Add(ct);
            }
            rptBaoCaoHoaDon hd = new rptBaoCaoHoaDon();
            hd.DataSource = bc;
            hd.Parameters["BCthang"].Value = "Xét nghiệm SLSS tháng " + this.dllNgay.tungay.Value.Month + "/" + this.dllNgay.tungay.Value.Year + " gồm:";
            Reports.frmDanhSachDaCapMa phd = new Reports.frmDanhSachDaCapMa(hd);
            phd.ShowDialog();


        }
        private void AddItemForm()
        {
            PSMenuForm fo = new PSMenuForm
            {
                NameForm = this.Name,
                Capiton = this.Text,
            };
            BioNet_Bus.AddMenuForm(fo);
            long? idfo = BioNet_Bus.GetMenuIDForm(this.Name);
            CustomLayouts.TransLanguage.AddItemCT(this.Controls, idfo);
        }
    }
}