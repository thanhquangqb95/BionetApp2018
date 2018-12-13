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
using DevExpress.XtraGrid.Views.BandedGrid;

namespace BioNetSangLocSoSinh.FrmReports
{
    public partial class FrmBaoCaoTuyChonNew : DevExpress.XtraEditors.XtraForm
    {
        public FrmBaoCaoTuyChonNew()
        {
            InitializeComponent();
        }

        private void btnLayDuLieu_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.dllNgay.tungay.Text) && !string.IsNullOrEmpty(this.dllNgay.denngay.Text))
            {
               
                string MaDonVi = String.Empty;

                if (this.txtDonVi.EditValue.ToString() == "all")
                {
                    if (this.txtChiCuc.EditValue.ToString() == "all")
                    {
                        this.lblTenDonVi.Text = "Thông kê toàn bộ trung tâm";
                        MaDonVi = "all";
                    }
                    else
                    {
                        this.lblTenDonVi.Text = "Thông kê chi cục " + this.txtChiCuc.Text.ToString();
                        MaDonVi = this.txtChiCuc.EditValue.ToString();
                    }
                }
                else
                {
                    this.lblTenDonVi.Text = "Thông kê đơn vị " + this.txtDonVi.Text.ToString();
                    MaDonVi = this.txtDonVi.EditValue.ToString();
                }
                SplashScreenManager.ShowForm(typeof(WaitingLoadData), true, false);
                var kq = BioNet_Bus.GetBaoCaoTuyChonNew(dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date, MaDonVi);
                SplashScreenManager.CloseForm();
                this.GCBaoCaoTuyChon.DataSource = kq;
            }
            else
            {
                XtraMessageBox.Show("Vui lòng chọn khoảng thời gian trước khi lấy dữ liệu báo cáo", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (!string.IsNullOrEmpty(dllNgay.tungay.Text))
                    dllNgay.tungay.Focus();
                else this.dllNgay.tungay.Focus();

            }
        }

        private void FrmBaoCaoTuyChonNew_Load(object sender, EventArgs e)
        {
            this.dllNgay.tungay.Value = DateTime.Now.Date;
            this.dllNgay.denngay.Value = DateTime.Now.Date;
            this.txtChiCuc.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            this.txtDonVi.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi("all");
            this.txtDonVi.EditValue = "all";
            this.txtChiCuc.EditValue = "all";
            this.LoadDuLieuDieuKienLoc();
            if (this.txtDonVi.EditValue.ToString() == "all")
            {
                if (this.txtChiCuc.EditValue.ToString() == "all")
                {
                    this.lblTenDonVi.Text = "Thông kê toàn bộ trung tâm";
                }
                else
                {
                    this.lblTenDonVi.Text = "Thông kê chi cục " + this.txtChiCuc.Text.ToString();
                }
            }
            else
            {
                this.lblTenDonVi.Text = "Thông kê đơn vị " + this.txtDonVi.Text.ToString();
            }
        }
        public class CLPPSinh
        {
            public string TenPPSinh { get; set; }
            public int? PPSinh { get; set; }
        }

        public class CLViTriLayMau
        {
            public string TenViTriLayMau { get; set; }
            public int? IDViTriLayMau { get; set; }
        }

        public class CLTinhTrangTre
        {
            public string TenTrinhTrangTre { get; set; }
            public int? IDTrinhTrangTre { get; set; }
        }

        public class CLCheDoDD
        {
            public string CheDoDinhDuong { get; set; }
            public int? IDCheDoDinhDuong { get; set; }
        }
        public class CLGioiTinh
        {
            public string TenGioiTinh { get; set; }
            public int? GioiTinh { get; set; }
        }
        private void LoadDuLieuDieuKienLoc()
        {
            var DMThongSo = BioNet_Bus.GetThongSoXN();
            if (DMThongSo != null)
            {
                foreach (var ctdm in DMThongSo)
                {

                    GridBand band = new GridBand();
                    band.Name = ctdm.TenThongSo;
                    band.Caption = ctdm.TenThongSo;
                    band.RowCount = 1;
                    band.Visible = true;
                    gridBand3.Children.Add(band);

                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col4 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                    col4.Name = ctdm.TenThongSo + "L2";
                    col4.FieldName = ctdm.TenThongSo +"_isNguyCo";
                    col4.Caption = "KL Cuối";
                    col4.OptionsColumn.AllowEdit = false;
                    col4.Visible = true;
                    band.Columns.Add(col4);

                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                    col3.Name = ctdm.TenThongSo + "L2";
                    col3.FieldName = ctdm.TenThongSo + "_KQCuoi";
                    col3.Caption = "KQ Cuối";
                    col3.OptionsColumn.AllowEdit = false;
                    col3.Visible = true;
                    band.Columns.Add(col3);

                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                    col2.Name = ctdm.TenThongSo + "L2";
                    col2.FieldName = ctdm.TenThongSo + "_KQ2";
                    col2.Caption = "KQ L2";
                    col2.OptionsColumn.AllowEdit = false;
                    col2.Visible = true;
                    band.Columns.Add(col2);

                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                    col1.Name = ctdm.TenThongSo + "L1";
                    col1.FieldName = ctdm.TenThongSo+"_KQ1";
                    col1.Caption = "KQ L1";
                    col1.OptionsColumn.AllowEdit = false;
                    col1.Visible = true;
                    band.Columns.Add(col1);

                }

            }
            List<CLPPSinh> CLPPSinhs = new List<CLPPSinh>();
            CLPPSinhs.Add(new CLPPSinh() { PPSinh = 0, TenPPSinh = "Sinh thường" });
            CLPPSinhs.Add(new CLPPSinh() { PPSinh = 1, TenPPSinh = "Sinh mổ" });
            CLPPSinhs.Add(new CLPPSinh() { PPSinh = 2, TenPPSinh = "N/A" });
            CLPPSinhs.Add(new CLPPSinh() { PPSinh = null, TenPPSinh = "" });

            List<CLViTriLayMau> CLViTriLayMaus = new List<CLViTriLayMau>();
            CLViTriLayMaus.Add(new CLViTriLayMau() { IDViTriLayMau = 0, TenViTriLayMau = "Gót chân" });
            CLViTriLayMaus.Add(new CLViTriLayMau() { IDViTriLayMau = 1, TenViTriLayMau = "Tĩnh mạch" });
            CLViTriLayMaus.Add(new CLViTriLayMau() { IDViTriLayMau = 2, TenViTriLayMau = "Khác" });
            CLViTriLayMaus.Add(new CLViTriLayMau() { IDViTriLayMau = null, TenViTriLayMau = "" });

            List<CLTinhTrangTre> CLTinhTrangTres = new List<CLTinhTrangTre>();
            CLTinhTrangTres.Add(new CLTinhTrangTre() { IDTrinhTrangTre = 0, TenTrinhTrangTre = "Bình thường" });
            CLTinhTrangTres.Add(new CLTinhTrangTre() { IDTrinhTrangTre = 1, TenTrinhTrangTre = "Bị bệnh" });
            CLTinhTrangTres.Add(new CLTinhTrangTre() { IDTrinhTrangTre = 2, TenTrinhTrangTre = "Dùng steroid" });
            CLTinhTrangTres.Add(new CLTinhTrangTre() { IDTrinhTrangTre = 3, TenTrinhTrangTre = "Dùng kháng sinh" });
            CLTinhTrangTres.Add(new CLTinhTrangTre() { IDTrinhTrangTre = 4, TenTrinhTrangTre = "Truyền máu" });
            CLTinhTrangTres.Add(new CLTinhTrangTre() { IDTrinhTrangTre = null, TenTrinhTrangTre = "" });


            List<CLCheDoDD> CLCheDoDDs = new List<CLCheDoDD>();
            CLCheDoDDs.Add(new CLCheDoDD() { IDCheDoDinhDuong = 0, CheDoDinhDuong = "Bú mẹ" });
            CLCheDoDDs.Add(new CLCheDoDD() { IDCheDoDinhDuong = 1, CheDoDinhDuong = "Bú bình" });
            CLCheDoDDs.Add(new CLCheDoDD() { IDCheDoDinhDuong = 2, CheDoDinhDuong = "Tĩnh mạch" });
            CLCheDoDDs.Add(new CLCheDoDD() { IDCheDoDinhDuong = null, CheDoDinhDuong = "" });

            List<CLGioiTinh> CLGioiTinhs = new List<CLGioiTinh>();
            CLGioiTinhs.Add(new CLGioiTinh() { GioiTinh = 0, TenGioiTinh = "Nam" });
            CLGioiTinhs.Add(new CLGioiTinh() { GioiTinh = 1, TenGioiTinh = "Nam" });
            CLGioiTinhs.Add(new CLGioiTinh() { GioiTinh = 2, TenGioiTinh = "N/A" });
            CLGioiTinhs.Add(new CLGioiTinh() { GioiTinh = null, TenGioiTinh = "N/A" });



            this.repositoryItemGridLookUpEditDonVi.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi("all");
            this.repositoryItemGridLookUpEditDonVi.DisplayMember = "TenDVCS";
            this.repositoryItemGridLookUpEditDonVi.ValueMember = "MaDVCS";

            this.repositoryItemGridLookUpEditPPSinh.DataSource = CLPPSinhs;
            this.repositoryItemGridLookUpEditPPSinh.DisplayMember = "TenPPSinh";
            this.repositoryItemGridLookUpEditPPSinh.ValueMember = "PPSinh";

            this.repositoryItemGridLookUpEditViTriLayMau.DataSource = CLViTriLayMaus;
            this.repositoryItemGridLookUpEditViTriLayMau.DisplayMember = "TenViTriLayMau";
            this.repositoryItemGridLookUpEditViTriLayMau.ValueMember = "IDViTriLayMau";

            this.repositoryItemGridLookUpEditDanToc.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DanToc();
            this.repositoryItemGridLookUpEditDanToc.DisplayMember = "TenDanToc";
            this.repositoryItemGridLookUpEditDanToc.ValueMember = "IDDanToc";

            this.repositoryItemGridLookUpEditTinhTrangTre.DataSource = CLTinhTrangTres;
            this.repositoryItemGridLookUpEditTinhTrangTre.DisplayMember = "TenTrinhTrangTre";
            this.repositoryItemGridLookUpEditTinhTrangTre.ValueMember = "IDTrinhTrangTre";

            this.repositoryItemGridLookUpEditCheDoDinhDuong.DataSource = CLCheDoDDs;
            this.repositoryItemGridLookUpEditCheDoDinhDuong.DisplayMember = "CheDoDinhDuong";
            this.repositoryItemGridLookUpEditCheDoDinhDuong.ValueMember = "IDCheDoDinhDuong";
            

            this.repositoryItemGridLookUpEditGoiXN.DataSource = BioNet_Bus.GetDanhsachGoiDichVuChung(); ;
            this.repositoryItemGridLookUpEditGoiXN.DisplayMember = "TenGoiDichVuChung";
            this.repositoryItemGridLookUpEditGoiXN.ValueMember = "IDGoiDichVuChung";

            this.repositoryItemGridLookUpEditChuongTrinh.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChuongTrinh(); ;
            this.repositoryItemGridLookUpEditChuongTrinh.DisplayMember = "TenChuongTrinh";
            this.repositoryItemGridLookUpEditChuongTrinh.ValueMember = "IDChuongTrinh";

            this.repositoryItemGridLookUpEditGioiTinh.DataSource = CLGioiTinhs ;
            this.repositoryItemGridLookUpEditGioiTinh.DisplayMember = "TenGioiTinh";
            this.repositoryItemGridLookUpEditGioiTinh.ValueMember = "GioiTinh";

            this.txtChiCuc.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            this.txtDonVi.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi("all");
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {

            if (this.GVBaoCaoTuyChon.RowCount > 0)
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
            ofd.FileName = "BaoCaoTuyChon" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName.Length > 0)
                {
                    try
                    {

                        this.GVBaoCaoTuyChon.ExportToXlsx(ofd.FileName);
                    }
                    catch
                    {
                        XtraMessageBox.Show("Không thể lưu file này! Vui lòng chọn đường dẫn khác.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
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
    }
}