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
using DevExpress.XtraSplashScreen;
using BioNetSangLocSoSinh.DiaglogFrm;
using BioNetModel.Data;

namespace BioNetSangLocSoSinh.FrmReports
{
    public partial class FrmBaoCaoTheoPattent : DevExpress.XtraEditors.XtraForm
    {
        public FrmBaoCaoTheoPattent()
        {
            InitializeComponent();
        }
        public class CLPPSinh
        {
            public string TenPPSinh { get; set; }
            public string PPSinh { get; set; }
        }

        public class CLViTriLayMau
        {
            public string TenViTriLayMau { get; set; }
            public byte? IDViTriLayMau { get; set; }
        }

        public class CLTinhTrangTre
        {
            public string TenTrinhTrangTre { get; set; }
            public string IDTrinhTrangTre { get; set; }
        }

        public class CLCheDoDD
        {
            public string CheDoDinhDuong { get; set; }
            public string IDCheDoDinhDuong { get; set; }
        }
        public class CLGioiTinh
        {
            public string TenGioiTinh { get; set; }
            public string GioiTinh { get; set; }
        }
        int gio = 0, phut = 0, giay = 0;
        private List<PSDanhMucGoiDichVuChung> lstgoiXN = new List<PSDanhMucGoiDichVuChung>();
        private void btnThongke_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (!string.IsNullOrEmpty(cbbDichVu.EditValue.ToString()))
                {
                    SplashScreenManager.ShowForm(typeof(WaitingLoadData), true, false);
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
                   
                    DateTime TIme1 = DateTime.Now;
                    GCDanhSachMauDuongTinh.DataSource = null;
                    GCDanhSachMauDuongTinh.DataSource = BioNet_Bus.LoadDSBaoCaoTuyChonDichVuNew(dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date, cbbDichVu.EditValue.ToString(), MaDonVi);
                    DateTime TIme2 = DateTime.Now;
                    TimeSpan kt = TIme2 - TIme1;
                    txtTime.Text = string.Format("{0:00}:{1:00}:{2:00}", kt.Hours, kt.Minutes, kt.Seconds);
                    SplashScreenManager.CloseForm();
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn dịch vụ cần thống kê","BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
               

            }
            catch(Exception ex)
            {

            }
        }

        private void FrmBaoCaoTheoPattent_Load(object sender, EventArgs e)
        {
           this.LoadGoiDichVuXetNGhiem();
            this.cbbDichVu.Properties.DataSource = BioNet_Bus.GetDanhSachDichVu(false);
            this.txtChiCuc.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            dllNgay.tungay.Value = DateTime.Now;
            dllNgay.denngay.Value = DateTime.Now;
            this.txtChiCuc.EditValue = "all";
            LoadDuLieuDieuKienLoc();
        }
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
        private void LoadDuLieuDieuKienLoc()
        {
            this.LookupEditTenDV.DataSource = BioNet_Bus.GetDanhSachDonViCoSo();
            this.LookupEditTenVietTat.DataSource = BioNet_Bus.GetDanhSachDonViCoSo();
            this.LookupEditTenChiCuc.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            List<CLPPSinh> CLPPSinhs = new List<CLPPSinh>();
            CLPPSinhs.Add(new CLPPSinh() { PPSinh = "0", TenPPSinh = "Sinh thường" });
            CLPPSinhs.Add(new CLPPSinh() { PPSinh = "1", TenPPSinh = "Sinh mổ" });
            CLPPSinhs.Add(new CLPPSinh() { PPSinh = "2", TenPPSinh = "N/A" });

            List<CLViTriLayMau> CLViTriLayMaus = new List<CLViTriLayMau>();
            CLViTriLayMaus.Add(new CLViTriLayMau() { IDViTriLayMau = 0, TenViTriLayMau = "Gót chân" });
            CLViTriLayMaus.Add(new CLViTriLayMau() { IDViTriLayMau = 1, TenViTriLayMau = "Tĩnh mạch" });
            CLViTriLayMaus.Add(new CLViTriLayMau() { IDViTriLayMau = 2, TenViTriLayMau = "Khác" });

            List<CLTinhTrangTre> CLTinhTrangTres = new List<CLTinhTrangTre>();
            CLTinhTrangTres.Add(new CLTinhTrangTre() { IDTrinhTrangTre = "0", TenTrinhTrangTre = "Bình thường" });
            CLTinhTrangTres.Add(new CLTinhTrangTre() { IDTrinhTrangTre = "1", TenTrinhTrangTre = "Bị bệnh" });
            CLTinhTrangTres.Add(new CLTinhTrangTre() { IDTrinhTrangTre = "2", TenTrinhTrangTre = "Dùng steroid" });
            CLTinhTrangTres.Add(new CLTinhTrangTre() { IDTrinhTrangTre = "3", TenTrinhTrangTre = "Dùng kháng sinh" });
            CLTinhTrangTres.Add(new CLTinhTrangTre() { IDTrinhTrangTre = "4", TenTrinhTrangTre = "Truyền máu" });


            List<CLCheDoDD> CLCheDoDDs = new List<CLCheDoDD>();
            CLCheDoDDs.Add(new CLCheDoDD() { IDCheDoDinhDuong = "0", CheDoDinhDuong = "Bú mẹ" });
            CLCheDoDDs.Add(new CLCheDoDD() { IDCheDoDinhDuong = "1", CheDoDinhDuong = "Bú bình" });
            CLCheDoDDs.Add(new CLCheDoDD() { IDCheDoDinhDuong = "2", CheDoDinhDuong = "Tĩnh mạch" });

            List<CLGioiTinh> CLGioiTinhs = new List<CLGioiTinh>();
            CLGioiTinhs.Add(new CLGioiTinh() { GioiTinh = "0", TenGioiTinh = "Nam" });
            CLGioiTinhs.Add(new CLGioiTinh() { GioiTinh = "1", TenGioiTinh = "Nữ" });
            CLGioiTinhs.Add(new CLGioiTinh() { GioiTinh = "2", TenGioiTinh = "N/A" });
          

            this.LookupPPSinh.DataSource = CLPPSinhs;
            this.LookupPPSinh.DisplayMember = "TenPPSinh";
            this.LookupPPSinh.ValueMember = "PPSinh";

            
            this.LookupViTriLayMau.DataSource = CLViTriLayMaus;
            this.LookupViTriLayMau.DisplayMember = "TenViTriLayMau";
            this.LookupViTriLayMau.ValueMember = "IDViTriLayMau";

            this.LookupDanToc.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DanToc();
            this.LookupDanToc.DisplayMember = "TenDanToc";
            this.LookupDanToc.ValueMember = "IDDanToc";

            this.LookupTTTre.DataSource = CLTinhTrangTres;
            this.LookupTTTre.DisplayMember = "TenTrinhTrangTre";
            this.LookupTTTre.ValueMember = "IDTrinhTrangTre";

            this.LookupCDDinhDuong.DataSource = CLCheDoDDs;
            this.LookupCDDinhDuong.DisplayMember = "CheDoDinhDuong";
            this.LookupCDDinhDuong.ValueMember = "IDCheDoDinhDuong";

            this.LookupGT.DataSource = CLGioiTinhs;
            this.LookupGT.DisplayMember = "TenGioiTinh";
            this.LookupGT.ValueMember = "GioiTinh";

            this.LookupChuongTrinh.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChuongTrinh(); ;
            this.LookupChuongTrinh.DisplayMember = "TenChuongTrinh";
            this.LookupChuongTrinh.ValueMember = "IDChuongTrinh";
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
            ofd.FileName = "BaoCaoPattent" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName.Length > 0)
                {
                    try
                    {

                        this.GVDanhSachDuongTinh.ExportToXlsx(ofd.FileName);
                        XtraMessageBox.Show("Lưu file thành cộng.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        System.Diagnostics.Process.Start(ofd.FileName);
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
            
        }

        private void GCDanhSachMauDuongTinh_Click(object sender, EventArgs e)
        {

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            giay++;
            if (giay == 60)
            {
                phut++;
                giay = 0;
            }
            if (phut == 60)
            {
                gio++;
                phut = 0;
            }
            
        }
        
    }
}