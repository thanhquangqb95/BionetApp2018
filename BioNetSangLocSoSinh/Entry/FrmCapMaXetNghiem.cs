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
using BioNetModel;
using BioNetModel.Data;

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmCapMaXetNghiem : DevExpress.XtraEditors.XtraForm
    {
        public FrmCapMaXetNghiem(PsEmployeeLogin nv)
        {
            emp = nv;
            InitializeComponent();
        }
        public static PsEmployeeLogin emp = new PsEmployeeLogin();
        private string MaNhanVien;
        private List<string> lstCDCanDanhMa = new List<string>();
        protected List<PSChiDinhDichVuNew> lstCho = new List<PSChiDinhDichVuNew>();
        private List<PSChiDinhDichVu> lstCanDanhMa = new List<PSChiDinhDichVu>();
        private List<PSChiDinhDichVu> lstCanDanhMaFull = new List<PSChiDinhDichVu>();
        private List<PsDanhSachCapMa> lstCapMaTheoDonVi = new List<PsDanhSachCapMa>();
        private List<string> lstDonViCanCapMa = new List<string>();
        private List<PSDanhMucDonViCoSo> lstDonVi = new List<PSDanhMucDonViCoSo>();
        private List<PSDanhMucGoiDichVuChung> lstgoiXN = new List<PSDanhMucGoiDichVuChung>();
        private bool isCapMaXNTheoMaPhieu = true;
        private long maKT;
        private string maGoiXNFocusHandle;
        private string maPhieuFocusHandle;
        private string maDonviFocusHandle;
        private string maChiDinhFocusHandle;
        private string maTiepNhanFocusHandle;
        private DateTime ngayChiDinhFocusHandle;
        private DateTime ngayTiepNhanFocusHandle;
        private List<PSXN_KetQuaNew> lstDanhSachDuyet = new List<PSXN_KetQuaNew>();
        private void FrmCapMaXetNghiem_Load(object sender, EventArgs e)
        {
            LoadFrm();
        }
        private void LoadFrm()
        {
            var tttrungtam = BioNet_Bus.GetThongTinTrungTam();
            if (tttrungtam.isCapMaXNTheoMaPhieu ?? true)
            {
                isCapMaXNTheoMaPhieu = true;
            }
            else
            {
                isCapMaXNTheoMaPhieu = false;
            }
            this.LoadDanhMuc();
            this.LoadGoiDichVuXetNGhiem();
            this.txtTuNgay_ChuaCapMa.EditValue = DateTime.Now;
            this.txtDenNgay_ChuaCapMa.EditValue = DateTime.Now;
            this.LoadListDSCho();
            //AddItemForm();
        }
        #region Load Thông tin
        private void LoadDanhMuc()
        {
            this.cbbChiCuc_ChuaCapMa.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            this.cbbChiCuc_ChuaCapMa.EditValue = "all";
            this.lstDonVi = BioNet_Bus.GetDanhSachDonViCoSo();
            this.repositoryItemLookUpDonVi_GCDaCapMau.DataSource = this.lstDonVi;
            this.repositoryItemLookUpDonVu_GCDanhSachCho.DataSource = this.lstDonVi;
        }

        private void LoadGCDanhSachCho(List<PSChiDinhDichVuNew> lst)
        {
            this.GCDanhSachCho.DataSource = null;
            this.GCDanhSachCho.DataSource = lst;
            this.GVDanhSachCho.Columns["MaDonVi"].Group();
            this.GVDanhSachCho.ExpandAllGroups();
        }

        private void LoadListDSCho()
        {
            if (this.KiemTraDieuKienLamMoiDanhSach())
            {
                this.lstCho.Clear();
                this.lstCho = BioNet_Bus.GetDanhSachChuaCapMaXNNew(1);
                List<PSChiDinhDichVuNew> lst = BioNet_Bus.GetDanhSachChuaCapMaXNNew(1);
                this.LoadGCDanhSachCho(lst);
            }
            else
            {
                XtraMessageBox.Show("Đưa danh sách đã cấp mã vào phòng xét nghiệm hoặc hủy danh sách đã cấp mã và làm lại từ đầu", "BioNet - Chương trình sàng lọc sơ sinh!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool KiemTraDieuKienLamMoiDanhSach()
        {
            if (this.GVDanhSachDaCapMa.RowCount > 0)
                return false; // nếu danh sách đã cấp mã mà chưa đưa vào xn thì ko đc làm mới ds
            else return true;
        }

        private void LoadGoiDichVuXetNGhiem()
        {
            try
            {
                this.lstgoiXN = BioNet_Bus.GetDanhsachGoiDichVuChung();
                PSDanhMucGoiDichVuChung goi = new PSDanhMucGoiDichVuChung();
                goi.IDGoiDichVuChung = "DVGXNL2";
                goi.TenGoiDichVuChung = "Xét Nghiệm Lần 2";
                goi.Stt = 6;
                goi.DonGia = 0;
                goi.ChietKhau = 0;
                this.lstgoiXN.Add(goi);
                PSDanhMucGoiDichVuChung goi1 = new PSDanhMucGoiDichVuChung();
                goi.IDGoiDichVuChung = "ALL";
                goi.TenGoiDichVuChung = "Tất cả";
                goi.Stt = 7;
                goi.DonGia = 0;
                goi.ChietKhau = 0;
                this.lstgoiXN.Add(goi1);
                this.LookUpGoiXN.DataSource = this.lstgoiXN;
                this.LookUpEditGoiXN.DataSource = this.lstgoiXN;
                this.cbbGoiXNLocChuaCapma.Properties.DataSource = this.lstgoiXN;
                this.cbbGoiXNLocChuaCapma.EditValue = "ALL";
            }
            catch { }
        }

        #endregion

        #region Thêm danh sách cấp mã
        private void btnDuaVaoDanhSachCapMa_Click(object sender, EventArgs e)
        {
            if (this.KiemTraDieuKienLamMoiDanhSach())
            {
                if (this.GVDanhSachCho.SelectedRowsCount > 0)
                {
                    int[] lstChecked = this.GVDanhSachCho.GetSelectedRows();
                    foreach (var index in lstChecked)
                    {
                        if (index >= 0)
                        {
                            string maChiDinh = this.GVDanhSachCho.GetRowCellValue(index, this.col_MaChiDinh_GCDanhSachCho).ToString();
                            var cd = BioNet_Bus.GetThongTinChiDinhNew(maChiDinh);
                            if (cd != null)
                            {
                                PSXN_KetQuaNew kqnew = new PSXN_KetQuaNew();
                                kqnew.MaChiDinh = this.GVDanhSachCho.GetRowCellValue(index, this.col_MaChiDinh_GCDanhSachCho).ToString();
                                kqnew.MaDonVi = this.GVDanhSachCho.GetRowCellValue(index, this.col_maDonVi__GCDanhSachCho).ToString();
                                kqnew.MaPhieu = this.GVDanhSachCho.GetRowCellValue(index, this.col_maPhieu_GCDanhSachCho).ToString();
                                kqnew.MaTiepNhan = this.GVDanhSachCho.GetRowCellValue(index, this.col_MaTiepNhan_GCDanhSachCho).ToString();
                                kqnew.MaGoiXN = this.GVDanhSachCho.GetRowCellValue(index, this.col_MaGoiXN).ToString();
                                kqnew.IDNhanVienCapMa = emp.EmployeeCode;
                                if(!kqnew.MaGoiXN.Equals("DVGXN0001"))
                                {
                                    kqnew.MaXetNghiem = kqnew.MaPhieu;
                                }
                                else
                                {
                                    var listCD=BioNet_Bus.GetDanhSachChiDinhNewPhieu(kqnew.MaPhieu, kqnew.MaDonVi);
                                    kqnew.MaXetNghiem = kqnew.MaPhieu + "L" +(listCD.Count()+1);
                                }
                                lstDanhSachDuyet.Add(kqnew);
                                var rowremove = lstCho.FirstOrDefault(x => x.MaChiDinh.Equals(kqnew.MaChiDinh) && x.MaPhieu.Equals(kqnew.MaPhieu));
                                lstCho.Remove(rowremove);
                            }
                        }
                    }
                    this.LookUpMaXetNghiem.DataSource = null;
                    this.LookUpMaXetNghiem.DataSource = this.lstDanhSachDuyet;
                    GCDanhSachDaCapMa.DataSource = lstDanhSachDuyet.OrderBy(x=>x.MaDonVi).ThenBy(x=>x.MaPhieu);
                    LoadGCDanhSachCho(lstCho);
                }

            }
            else
            {
                XtraMessageBox.Show("Đưa danh sách đã cấp mã vào phòng xét nghiệm hoặc hủy danh sách đã cấp mã và làm lại từ đầu", "BioNet - Chương trình sàng lọc sơ sinh!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        private void btnRefesh_Click(object sender, EventArgs e)
        {

        }
        #region Duyệt danh sách
        private void btnDuaVaoXN_Click(object sender, EventArgs e)
        {
            LuuDanhSachDaCapMa();
        }
        private void LuuDanhSachDaCapMa()
        {
            bool ok = true;
            int max = lstDanhSachDuyet.Count();
            int giatridem;
            string dem = null;
            do
            {
                try
                {
                    giatridem = Int32.Parse(lstDanhSachDuyet[max - 1].MaXetNghiem.ToString().Trim());
                    dem = giatridem.ToString();
                }
                catch
                {
                    max = max - 1;
                    if (max < 1)
                        break;
                }
            }
            while (String.IsNullOrEmpty(dem));

            List<PsPhieuLoiKhiDanhGia> listphieuloi = new List<PsPhieuLoiKhiDanhGia>();
            foreach (var item in this.lstDanhSachDuyet)
            {

                var res = BioNet_Bus.InsertKetQuaXNNew(item);
                if (!res.Result)
                {
                    PsPhieuLoiKhiDanhGia phieu = new PsPhieuLoiKhiDanhGia();
                    phieu.MaPhieu = item.MaPhieu;
                    phieu.ThongTinLoi = res.StringError;
                    listphieuloi.Add(phieu);
                }

            }
            if (listphieuloi.Count < 1)
            {
                XtraMessageBox.Show("Lưu danh sách phiếu thành công", "BioNet - Chương trình sàng lọc sơ sinh!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.lstCanDanhMa.Clear();
                this.LoadListDSCho();
            }
            else
            {
                DiaglogFrm.FrmDiagLogShowPhieuLoi frmloi = new DiaglogFrm.FrmDiagLogShowPhieuLoi(listphieuloi);
                frmloi.ShowDialog();
            }
        }
        #endregion
    }
}