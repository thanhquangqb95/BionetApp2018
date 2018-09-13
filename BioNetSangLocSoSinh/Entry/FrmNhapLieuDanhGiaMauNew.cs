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
using BioNetModel;
using BioNetModel.Data;
using BioNetBLL;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraSplashScreen;

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmNhapLieuDanhGiaMauNew : DevExpress.XtraEditors.XtraForm
    {
        public static PsEmployeeLogin emp = new PsEmployeeLogin();
        public List<PSDanhMucDonViCoSo> lstDonVi = new List<PSDanhMucDonViCoSo>();
        public List<PSDanhMucChiCuc> lstChiCuc = new List<PSDanhMucChiCuc>();
        public List<PSDanhMucDanToc> lstDanToc = new List<PSDanhMucDanToc>();
        public List<PSDanhMucChuongTrinh> lstChuongTrinh = new List<PSDanhMucChuongTrinh>();
        public List<PSDanhMucGoiDichVuChung> lstgoiXN = new List<PSDanhMucGoiDichVuChung>();
        public List<PsDichVu> lstDichVu = new List<PsDichVu>();
        private List<PSTiepNhan> lstDSCanDanhGia = new List<PSTiepNhan>();
        public List<PSDanhMucDanhGiaChatLuongMau> lstCLMau = new List<PSDanhMucDanhGiaChatLuongMau>();
        public List<PsDanhGiaMauSoBo> lstDanhGiaSoBo = new List<PsDanhGiaMauSoBo>();
        public List<PSTiepNhan> lstTiepNhan = new List<PSTiepNhan>();
        public List<PSChiDinhDichVu> lstDaDuyet = new List<PSChiDinhDichVu>();
        private List<PSDanhMucDanhGiaChatLuongMau> sourceListDanhGiaChatLuong = new List<PSDanhMucDanhGiaChatLuongMau>();
        private List<PSChiTietDanhGiaChatLuong> lstDanhGiaChatLuong = new List<PSChiTietDanhGiaChatLuong>();
        private List<PSChiDinhTrenPhieu> listdvcanlamlai = new List<PSChiDinhTrenPhieu>();

        private List<PsDichVu> lstChiDinhDichVu = new List<PsDichVu>();
        public FrmNhapLieuDanhGiaMauNew(PsEmployeeLogin Employee)
        {
            InitializeComponent();
            emp = Employee;
        }

        private void txtNgayLayMau_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void labelControl39_Click(object sender, EventArgs e)
        {

        }

        private void FrmNhapLieuDanhGiaMauNew_Load(object sender, EventArgs e)
        {
            this.txtDenNgay.EditValue = DateTime.Now;
            this.txtTuNgay.EditValue = DateTime.Now;
            this.LoadDataDanhMuc();
            this.LoadGoiDichVuXetNGhiem();
            this.LoadDanhSachDichVu();
            this.LoadListDanhGiaSoBo();
            this.lstCLMau = BioNet_Bus.GetDanhMucDanhGiaChatLuong();
            LoadDanhSachTiepNhan();
            LoadDanhSachDaDuyet();
            LoadNew();
            this.sourceListDanhGiaChatLuong = BioNet_Bus.GetDanhMucDanhGiaChatLuong();

        }

        private void LoadDataDanhMuc()
        {
            try
            {
                this.lstChiCuc = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
                this.lstDonVi = BioNet_Bus.GetDanhSachDonViCoSo();
                this.cbbChiCucTiepNhan.Properties.DataSource = lstChiCuc;
                this.cbbChiCucTiepNhan.EditValue = "all";
                this.cbbDonViTiepNhan.EditValue = "all";
                this.cbbChiCucDaDuyet.Properties.DataSource = lstChiCuc;
                this.cbbChiCucDaDuyet.EditValue = "all";
                this.cbbDonViDaDuyet.EditValue = "all";
                
                this.LookUpEditDonVi.DataSource = lstDonVi;
                this.LookUpEditDonViDaDuyet.DataSource = lstDonVi;
                this.cbbDonViChon.Properties.DataSource = lstDonVi;
                this.lstDanToc = BioNet_Bus.GetDanhSachDanToc(-1);//get all dân tộc
                this.cbbDanToc.Properties.DataSource = lstDanToc;
                this.lstChuongTrinh = BioNet_Bus.GetDanhSachChuongTrinh(false);
                this.cbbChuongTrinh.Properties.DataSource = lstChuongTrinh;
            }
            catch (Exception ex)
            {
                DiaglogFrm.FrmNotData notData = new DiaglogFrm.FrmNotData(ex.ToString());
            }
        }

        private void LoadGoiDichVuXetNGhiem()
        {
            try
            {
                this.lstgoiXN = BioNet_Bus.GetDanhsachGoiDichVuChung();
                this.LookUpEditXetNghiem.DataSource = this.lstgoiXN;
                this.lookupGoiXN.DataSource = this.lstgoiXN;
                this.cbbGoiXNLoc.Properties.DataSource = this.lstgoiXN;
            }
            catch { }
        }

        private void LoadDanhSachDichVu()
        {
            this.lstDichVu.Clear();
            this.lstDichVu = BioNet_Bus.GetDanhSachDichVu(false);
            this.checkedListBoxXN.Items.Clear();
            foreach (PsDichVu v in this.lstDichVu)
            {
                this.checkedListBoxXN.Items.Add(v, v.TenDichVu);
            }
        }

        private void LoadListDanhGiaSoBo()
        {
            this.lstDanhGiaSoBo.Clear();
            PsDanhGiaMauSoBo dg1 = new PsDanhGiaMauSoBo();
            dg1.giaTri = false;
            dg1.maGiaTri = "islaymautruoc24h";
            dg1.noiDungChuThich = "Lấy mẫu trước 30h sau khi sinh";
            PsDanhGiaMauSoBo dg2 = new PsDanhGiaMauSoBo();
            dg2.giaTri = false;
            dg2.maGiaTri = "isSinhNon";
            dg2.noiDungChuThich = "Sinh non";
            PsDanhGiaMauSoBo dg3 = new PsDanhGiaMauSoBo();
            dg3.giaTri = false;
            dg3.maGiaTri = "isNheCan";
            dg3.noiDungChuThich = "Nhẹ cân";
            PsDanhGiaMauSoBo dg4 = new PsDanhGiaMauSoBo();
            dg4.giaTri = false;
            dg4.maGiaTri = "isGuiMauTre";
            dg4.noiDungChuThich = "Gửi mẫu trễ";
            this.lstDanhGiaSoBo.Add(dg1);
            this.lstDanhGiaSoBo.Add(dg2);
            this.lstDanhGiaSoBo.Add(dg3);
            this.lstDanhGiaSoBo.Add(dg4);
        }

        private void LoadDanhSachTiepNhan(List<PSTiepNhan> lst)
        {
            this.GCDanhSachTiepNhan.DataSource = null;
            this.GCDanhSachTiepNhan.DataSource = lst;
            this.GVDanhSachTiepNhan.Columns["MaDonVi"].Group();
            this.GVDanhSachTiepNhan.ExpandAllGroups();
        }
        private void LoadDanhSachDaDuyet(List<PSChiDinhDichVu> lst)
        {
            this.GCDanhSachDaTracking.DataSource = null;
            this.GCDanhSachDaTracking.DataSource = lst;
            this.GVDanhSachDaTracking.Columns["MaDonVi"].Group();
            this.GVDanhSachDaTracking.ExpandAllGroups();
        }

        private void LoadDanhSachTiepNhan()
        {
            this.lstTiepNhan.Clear();
            this.lstTiepNhan = BioNet_Bus.GetDanhSachPhieuChuaDanhGia("all");
            this.LoadDanhSachTiepNhan(lstTiepNhan);
        }
        private void LoadDanhSachDaDuyet()
        {
            this.lstDaDuyet.Clear();
            this.lstDaDuyet = BioNet_Bus.GetDanhSachPhieuDaDuyet("all",txtTuNgay.DateTime.Date,txtDenNgay.DateTime.Date);
            this.LoadDanhSachDaDuyet(lstDaDuyet);
        }

        private void cbbChiCuc_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit sear = sender as SearchLookUpEdit;
                var value = sear.EditValue.ToString();
                this.cbbDonViTiepNhan.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi(value.ToString());
                this.cbbDonViTiepNhan.EditValue = "all";
                if (cbbChiCucTiepNhan.EditValue.ToString() != "all")
                {
                    FilterTiepNhanTheoDonVi();
                }
                else
                {
                    GVDanhSachTiepNhan.ClearColumnsFilter();
                }
            }
            catch { }
        }
        private void FilterTiepNhanTheoDonVi()
        {
            string machicuc = cbbChiCucTiepNhan.EditValue.ToString();
            string madv = cbbDonViTiepNhan.EditValue.ToString();
            if (!machicuc.Equals("all") && madv.Equals("all"))
            {
                madv = machicuc;
            }
            string filterMaDV = "Contains([MaDonVi], '" + madv + "')";
            GVDanhSachTiepNhan.Columns["MaDonVi"].FilterInfo = new ColumnFilterInfo(filterMaDV);

        }
        private void txtMaPhieuSearch_KeyPress(object sender, KeyPressEventArgs e) //Filter ds tiếp nhận theo mã phiếu
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                if (!string.IsNullOrEmpty(txtMaPhieuSearch.Text))
                {
                    string filterMaPhieu = "[MaPhieu]='" + txtMaPhieuSearch.Text + "'";
                    GVDanhSachTiepNhan.Columns["MaPhieu"].FilterInfo = new ColumnFilterInfo(filterMaPhieu);
                }
            }
        }

        private void btnCapNhatDSTiepNhan_Click(object sender, EventArgs e)
        {
            LoadDanhSachTiepNhan();
        }

        private void cbbDonViTiepNhan_EditValueChanged(object sender, EventArgs e)
        {
            if (cbbChiCucTiepNhan.EditValue.ToString() != "all" || cbbDonViTiepNhan.EditValue.ToString() != "all")
            {
                FilterTiepNhanTheoDonVi();
            }
            else
            {
                GVDanhSachTiepNhan.ClearColumnsFilter();
            }
        }

        private void GVDanhSachTiepNhan_Click(object sender, EventArgs e)
        {

        }

        private void GVDanhSachTiepNhan_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.GVDanhSachTiepNhan.RowCount > 0)
                {
                    if (this.GVDanhSachTiepNhan.GetFocusedRow() != null)
                    {
                        string maPhieu = this.GVDanhSachTiepNhan.GetRowCellValue(this.GVDanhSachTiepNhan.FocusedRowHandle, this.col_MaPhieu_GCTiepNhan).ToString();
                        string maDonVi = this.GVDanhSachTiepNhan.GetRowCellValue(this.GVDanhSachTiepNhan.FocusedRowHandle, this.col_DonViCoSo_GCTiepNhan).ToString();
                        this.HienThiThongTinPhieu(maPhieu, maDonVi);
                    }
                }
            }
            catch { }
        }
        private void HienThiThongTinPhieu(string maPhieu, string maDonVi)
        {
            this.LoadNew();
            this.ReadOnly(true);
            PSTTPhieu data = BioNet_Bus.GetTTPhieu(maPhieu, maDonVi);
            this.LoadGoiXetNghiem(maDonVi);
            this.LoadDanhSachDichVu();
            this.btnDuyet.Enabled = true;
            this.txtMaPhieuHienThi.Text = maPhieu;
            this.txtMaPhieu.Text = maPhieu;
            this.cbbDonViChon.EditValue = maDonVi;
            //Thông tin tiếp nhận
            this.txtMaTiepNhan.Text = data.TiepNhan.MaTiepNhan;
            this.txtNVTiepNhan.Text = BioNet_Bus.GetThongTinNhanVien(data.TiepNhan.MaNVTiepNhan).EmployeeName;
            this.txtNgayTiepNhan.EditValue = data.TiepNhan.NgayTiepNhan.Value.Date;
            if (data.Phieu != null)
            {               
                if (data.ChiDinh != null)
                {
                    this.txtMaChiDinh.Text = data.ChiDinh.MaChiDinh;
                    this.txtNVChiDinh.Text = BioNet_Bus.GetThongTinNhanVien(data.ChiDinh.MaNVChiDinh).EmployeeName;
                    this.txtNgayChiDinh.EditValue = data.ChiDinh.NgayChiDinhLamViec.Value.Date;
                }
                //Thông tin đơn vị
                this.txtTrangThaiMau.EditValue = data.Phieu.TrangThaiMau != null ? data.Phieu.TrangThaiMau.ToString() : "0";
                if(int.Parse(this.txtTrangThaiMau.EditValue.ToString())>1)
                {
                    this.btnDuyet.Enabled = false;
                    this.btnDuyet.Visible = false;
                    this.btnLuu.Visible = true;
                    this.btnLuu.Enabled = true;
                    this.cbbDonViChon.Enabled = false;
                    this.radioGroupGoiXN.Enabled = false;
                }
                else
                {
                    this.btnDuyet.Enabled = true;
                    this.btnDuyet.Visible = true;
                    this.btnLuu.Visible = false;
                    this.btnLuu.Enabled = false;
                    this.cbbDonViChon.Enabled = true;
                    this.radioGroupGoiXN.Enabled = true;
                }
                if (!string.IsNullOrEmpty(data.Phieu.IDPhieuLan1))
                    this.txtMaPhieu1.Text = data.Phieu.IDPhieuLan1;
                this.txtGhiChu.Text = data.Phieu.LuuYPhieu;
                this.txtMaPhieuHienThi.Text = data.Phieu.IDPhieu;
                this.txtNgayTruyenMau.EditValue = data.Phieu.NgayTruyenMau;
                this.txtSoLuongTruyenMau.Text = data.Phieu.SLTruyenMau.ToString();
                this.cbbTTTre.EditValue = data.Phieu.TinhTrangLucLayMau.ToString();
                this.cbbViTriLayMau.EditValue = data.Phieu.IDViTriLayMau.ToString();
                this.cbbChuongTrinh.EditValue = data.Phieu.IDChuongTrinh;
                this.radioGroupGoiXN.EditValue = data.Phieu.MaGoiXN;
                this.txtGioLayMau.EditValue = Convert.ToDateTime(data.Phieu.NgayGioLayMau.Value.ToString("HH:mm"));
                this.txtNgayLayMau.EditValue = data.Phieu.NgayGioLayMau.Value.Date;
                this.txtNguoiLayMau.Text = String.IsNullOrEmpty(data.Phieu.TenNhanVienLayMau) ? this.txtNguoiLayMau.Text : data.Phieu.TenNhanVienLayMau; 
                this.txtDiaChiNoiLayMau.Text = data.Phieu.DiaChiLayMau;
                this.txtNoiLayMau.Text = data.Phieu.NoiLayMau;
                this.radioDanhGia.SelectedIndex = (data.Phieu.isKhongDat ?? false) == false ? 0 : 1;
                this.txtSDTNguoiLayMau.Text = String.IsNullOrEmpty(data.Phieu.SDTNhanVienLayMau) ? this.txtSDTNguoiLayMau.Text : data.Phieu.SDTNhanVienLayMau;

                if (data.Benhnhan != null)
                {
                    //Thông tin gia đình
                    this.txtTenMe.Text = data.Benhnhan.MotherName;
                    this.txtTenCha.Text = data.Benhnhan.FatherName;
                    this.txtSDTMe.Text = data.Benhnhan.MotherPhoneNumber;
                    this.txtSDTCha.Text = data.Benhnhan.FatherPhoneNumber;
                    this.txtNamSinhMe.EditValue = data.Benhnhan.MotherBirthday;
                    this.txtNamSinhCha.EditValue = data.Benhnhan.FatherBirthday;
                    this.txtMaBenhNhan.Text = data.Benhnhan.MaBenhNhan;
                    this.txtMaKhachHang.Text = data.Benhnhan.MaKhachHang;
                    this.txtDiaChiGiaDinh.Text = data.Benhnhan.DiaChi;
                    this.txtPara.Text = data.Benhnhan.Para;
                    //Thông tin trẻ
                    this.txtTenBenhNhan.Text = data.Benhnhan.TenBenhNhan;
                    this.txtGioiTinh.SelectedIndex = data.Benhnhan.GioiTinh ?? 2;
                    this.cboPhuongPhapSinh.SelectedIndex = data.Benhnhan.PhuongPhapSinh ?? 2;
                    this.cbbDanToc.EditValue = data.Benhnhan.DanTocID;
                    this.txtNoiSinh.Text = data.Benhnhan.NoiSinh;
                    if (data.Benhnhan.NgayGioSinh != null)
                    {
                        this.txtGioSinhBenhNhan.EditValue = Convert.ToDateTime(data.Benhnhan.NgayGioSinh.Value.ToString("HH:mm"));
                        this.txtNamSinhBenhNhan.EditValue = data.Benhnhan.NgayGioSinh.Value.Date;
                    }
                    this.txtCanNang.Text = data.Benhnhan.CanNang == null ? "0" : data.Benhnhan.CanNang.ToString();
                    this.txtTuanTuoi.Text = data.Benhnhan.TuanTuoiKhiSinh == null ? "0" : data.Benhnhan.TuanTuoiKhiSinh.ToString();
                    this.cbbDanToc.EditValue = data.Benhnhan.DanTocID == null ? 0 : data.Benhnhan.DanTocID;
                    this.cbbCheDoDD.EditValue = data.Phieu.CheDoDinhDuong.ToString();
                    if (data.ChiDinh != null)
                    {
                        this.txtGiaGoiXN.EditValue = data.ChiDinh.DonGia;
                    }
                }
            }
            else
            { 
                this.txtDiaChiNoiLayMau.Text = lstDonVi.FirstOrDefault(x => x.MaDVCS.Equals(maDonVi)).DiaChiDVCS.ToString();
                this.txtNoiLayMau.Text = cbbDonViChon.Text.ToString();              
            }
            KiemTraIsDaNhapLieu();
            ValidateNheCan();
            ValidateSinhNon();
            ValidateGuiMauTre(string.Empty, string.Empty);
            ValidateThuMauSom(string.Empty, string.Empty);

        }

        private void LoadGoiXetNghiem(string maDonVi)
        {
            var list = BioNet_Bus.GetDanhsachGoiDichVuTrungTam(maDonVi);
            this.radioGroupGoiXN.Properties.Items.Clear();
            foreach (var item in list)
            {
                if (!item.IDGoiDichVuChung.Equals("DVGXNL2"))
                {
                    this.radioGroupGoiXN.Properties.Items.Add(new RadioGroupItem(item.IDGoiDichVuChung, item.TenGoiDichVuChung));
                }
            }
        }
        private void LoadNew()
        {
            this.btnDuyet.Enabled = false;
            this.btnLuu.Enabled = false;
            this.btnHuy.Enabled = false;
            this.btnBackPhieu.Enabled = false;
            this.checkedListBoxXN.Enabled = false;
            this.txtMaTiepNhan.ResetText();
            this.txtMaChiDinh.ResetText();
            this.txtNgayTiepNhan.ResetText();
            this.txtMaBenhNhan.ResetText();
            this.txtNgayChiDinh.ResetText();
            this.txtNVChiDinh.ResetText();
            this.txtNVTiepNhan.ResetText();
            this.txtMaPhieu1.ResetText();
            this.txtNoiSinh.ResetText();
            this.txtPara.ResetText();
            this.cboPhuongPhapSinh.EditValue = "0";
            this.txtCanNang.EditValue = "0";
            this.cbbChuongTrinh.ItemIndex = 0;
            this.cbbDanToc.ItemIndex = 0;
            this.txtGioLayMau.ResetText();
            this.txtGioSinhBenhNhan.EditValue = null;
            this.txtMaPhieu1.ResetText();
            this.txtGhiChu.ResetText();
            this.txtDiaChiGiaDinh.ResetText();
            this.txtDiaChiGiaDinh.ResetText();
            this.txtCanNang.ResetText();
            this.txtGioiTinh.ResetText();
            this.txtNamSinhBenhNhan.ResetText();
            this.txtNamSinhCha.ResetText();
            this.txtNamSinhMe.ResetText();
            this.txtNgayTruyenMau.ResetText();
            this.txtNoiSinh.ResetText();
            this.txtSDTCha.ResetText();
            this.txtSDTMe.ResetText();
            this.txtSoLuongTruyenMau.ResetText();
            this.txtTenBenhNhan.ResetText();
            this.txtTenCha.ResetText();
            this.txtTenMe.ResetText();
            this.cbbCheDoDD.EditValue = "0";
            this.radioGroupGoiXN.EditValue = null;
            this.cbbTTTre.EditValue = "0";
            this.cbbViTriLayMau.EditValue = "0";
            this.txtTuanTuoi.ResetText();
            this.txtGioiTinh.ResetText();
            this.cbbDanToc.ResetText();
            this.cbbChuongTrinh.ResetText();
            this.txtGioSinhBenhNhan.ResetText();
            this.txtGioLayMau.EditValue = null;
            this.txtDiaChiGiaDinh.ResetText();
            this.txtNoiLayMau.ResetText();
            this.txtNgayLayMau.ResetText();
            this.LoadListDanhGiaSoBo();
            this.txtLuuY.ResetText();
            this.radioDanhGia.SelectedIndex = 0;
        }

        private void ReadOnly(bool isreadonly)
        {
            this.txtNoiSinh.Enabled = isreadonly;
            this.txtPara.Enabled = isreadonly;
            this.cboPhuongPhapSinh.Enabled = isreadonly;
            this.txtCanNang.Enabled = isreadonly;
            this.txtGioLayMau.Enabled = isreadonly;
            this.txtGioSinhBenhNhan.Enabled = isreadonly;
            this.txtGhiChu.Enabled = isreadonly;
            this.txtDiaChiGiaDinh.Enabled = isreadonly;
            this.txtDiaChiGiaDinh.Enabled = isreadonly;
            this.txtCanNang.Enabled = isreadonly;
            this.txtGioiTinh.Enabled = isreadonly;
            this.txtNamSinhBenhNhan.Enabled = isreadonly;
            this.txtTuanTuoi.Enabled = isreadonly;
            this.txtNamSinhCha.Enabled = isreadonly;
            this.txtNamSinhMe.Enabled = isreadonly;
            this.txtNgayTruyenMau.Enabled = isreadonly;
            this.txtNoiSinh.Enabled = isreadonly;
            this.txtSDTCha.Enabled = isreadonly;
            this.txtSDTMe.Enabled = isreadonly;
            this.txtSoLuongTruyenMau.Enabled = isreadonly;
            this.txtTenBenhNhan.Enabled = isreadonly;
            this.txtTenCha.Enabled = isreadonly;
            this.txtTenMe.Enabled = isreadonly;
            this.cbbCheDoDD.Enabled = isreadonly;
            this.radioGroupGoiXN.Enabled = isreadonly;
            this.cbbTTTre.Enabled = isreadonly;
            this.cbbViTriLayMau.Enabled = isreadonly;
            this.txtGioiTinh.Enabled = isreadonly;
            this.cbbDanToc.Enabled = isreadonly;
            this.cbbChuongTrinh.Enabled = isreadonly;
            this.txtGioSinhBenhNhan.Enabled = isreadonly;
            this.txtGioLayMau.Enabled = isreadonly;
            this.txtDiaChiGiaDinh.Enabled = isreadonly;
            this.txtDiaChiNoiLayMau.Enabled = isreadonly;
            this.txtSDTNguoiLayMau.Enabled = isreadonly;
            this.txtNguoiLayMau.Enabled = isreadonly;
            this.txtNoiLayMau.Enabled = isreadonly;
            this.cboPhuongPhapSinh.Enabled = isreadonly;
            this.txtNgayLayMau.Enabled = isreadonly;
            this.txtLuuY.Enabled = isreadonly;
            this.radioDanhGia.Enabled = isreadonly;
            this.cbbChuongTrinh.Enabled = isreadonly;
        }

        private void cbbTTTre_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {


        }
        private void KiemtraChitietDanhGiaCuaPhieu()
        {
            if (this.checkedListBoxLydoKhongDat.Items.Count > 0)
            {
                var lstDanhGia = BioNet_Bus.GetChiTietDanhGiaMạuKhongDatTrenPhieu(this.txtMaPhieuHienThi.Text.Trim(), this.txtMaTiepNhan.Text.Trim());
                if (lstDanhGia.Count > 0)
                {
                    foreach (CheckedListBoxItem item in this.checkedListBoxLydoKhongDat.Items)
                    {
                        foreach (var kd in lstDanhGia)
                        {
                            if (item.Value.ToString() == kd.IDDanhGiaChatLuongMau)
                                item.CheckState = CheckState.Checked;
                        }
                    }
                }
            }
        }
        private void radioGroupGoiXN_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                RadioGroup rd = sender as RadioGroup;
                var ts = rd.EditValue;
                if (ts.Equals("DVGXN0001"))
                {
                    if(int.Parse(txtTrangThaiMau.EditValue.ToString())<2)
                    {
                        this.checkedListBoxXN.Enabled = true;
                        this.txtMaPhieu1.Enabled = true;
                    }
                    else
                    {
                        this.checkedListBoxXN.Enabled = false;
                        this.txtMaPhieu1.Enabled = false;
                    }
                    this.lstChiDinhDichVu = BioNet_Bus.GetDanhSachDichVuTheoMaGoi(radioGroupGoiXN.EditValue.ToString(), cbbDonViChon.EditValue.ToString());
                    this.LoadDanhSachDichVu();
                    this.LoadGCChiDinhDichVu();
                    this.txtGiaGoiXN.EditValue = 0;

                }
                else
                {
                    this.txtMaPhieu1.ResetText();
                    this.checkedListBoxXN.Enabled = false;
                    this.checkedListBoxXN.DataSource = null;
                    this.lstChiDinhDichVu = BioNet_Bus.GetDanhSachDichVuTheoMaGoi(radioGroupGoiXN.EditValue.ToString(), cbbDonViChon.EditValue.ToString());
                    var list = BioNet_Bus.GetDanhsachGoiDichVuTrungTam(cbbDonViChon.EditValue.ToString());
                    this.txtGiaGoiXN.EditValue = list.FirstOrDefault(x => x.IDGoiDichVuChung == radioGroupGoiXN.Text).DonGia;            
                    this.LoadDanhSachDichVu();
                    this.LoadGCChiDinhDichVu();
                    this.txtMaPhieu1.Enabled = false;
                }
            }
            catch
            {
                this.checkedListBoxXN.DataSource = null;
                this.LoadDanhSachDichVu();
                this.txtMaPhieu1.Enabled = false;
            }
        }
        private void LoadGCChiDinhDichVu()
        {
            this.GCChiDinhDichVu.DataSource = null;
           
            if(!radioGroupGoiXN.EditValue.Equals("DVGXN0001"))
            {
                this.GCChiDinhDichVu.DataSource = this.lstChiDinhDichVu;
                foreach (CheckedListBoxItem item in this.checkedListBoxXN.Items)
                {
                    try
                    {
                        PsDichVu dichvu = item.Value as PsDichVu;
                        if (lstChiDinhDichVu.FirstOrDefault(x => x.IDDichVu.Equals(dichvu.IDDichVu)) != null)
                        {
                            item.CheckState = CheckState.Checked;
                        }
                    }
                    catch (Exception ex) { }
                }
            }
            else

            {
                List<PsDichVu> lstdv = new List<PsDichVu>();
                foreach (var lamlai in this.listdvcanlamlai)
                {
                    PsDichVu dv = lstChiDinhDichVu.FirstOrDefault(x => x.IDDichVu == lamlai.MaDichVu);
                    lstdv.Add(dv);
                }
                this.GCChiDinhDichVu.DataSource = lstdv;
                foreach (CheckedListBoxItem item in this.checkedListBoxXN.Items)
                {
                    try
                    {
                        PsDichVu dichvu = item.Value as PsDichVu;
                        if (listdvcanlamlai.FirstOrDefault(x => x.MaDichVu.Equals(dichvu.IDDichVu)) != null)
                        {
                            item.CheckState = CheckState.Checked;
                        }
                    }
                    catch (Exception ex) { }
                }
            }
           
        }

        private void cbbTTTre_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbbTTTre.EditValue.ToString() == "6")
                {
                    lblNgayTruyen.Enabled = true;
                    lblSLTruyenMau.Enabled = true;
                    lblTruyenMau.Enabled = true;
                    txtSoLuongTruyenMau.Enabled = true;
                    txtNgayTruyenMau.Enabled = true;
                }
                else
                {
                    lblNgayTruyen.Enabled = false;
                    lblSLTruyenMau.Enabled = false;
                    lblTruyenMau.Enabled = false;
                    txtSoLuongTruyenMau.Enabled = false;
                    txtNgayTruyenMau.Enabled = false;
                }
            }
            catch
            {

            }
        }

        private void radioDanhGia_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                RadioGroup rd = sender as RadioGroup;
                var value = rd.EditValue.ToString();
                if (!value.Equals("1"))
                {

                    foreach (PSDanhMucDanhGiaChatLuongMau v in this.sourceListDanhGiaChatLuong)
                    {
                        this.checkedListBoxLydoKhongDat.Items.Add(v.IDDanhGiaChatLuongMau, v.ChatLuongMau);
                    }
                    this.checkedListBoxLydoKhongDat.Enabled = true;
                    this.KiemtraChitietDanhGiaCuaPhieu();
                }
                else
                {
                    this.lstDanhGiaChatLuong.Clear();
                    this.checkedListBoxLydoKhongDat.Items.Clear();
                    this.checkedListBoxLydoKhongDat.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi khi load dữ liệu ! \r\n Lỗi chi tiết : " + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #region Duyệt phiếu
        private void btnDuyet_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.radioGroupGoiXN.EditValue != null)
                {
                    if (this.KiemTraCacTruongDuLieuTruocKhiLuu())
                    {
                        if (int.Parse(txtTrangThaiMau.EditValue.ToString()) < 2)
                        {
                            this.LuuThongTinPhieu(true);
                        }
                    }
                }
                else
                {
                    DiaglogFrm.FrmNotData notData = new DiaglogFrm.FrmNotData("Yêu cầu chọn gói xét nghiệm");
                }
            }
            catch
            {

            }
            
        }

        private bool KiemTraCacTruongDuLieuTruocKhiLuu()
        {
            bool kq = true;
            //Thông tin gia đình
            if (string.IsNullOrEmpty(this.txtTenMe.Text.Trim()))
            {
                this.txtTenMe.BackColor = Color.PapayaWhip;
                kq = false;
            }
            else
            {
                this.txtTenMe.BackColor = Color.White;
            }
            if (string.IsNullOrEmpty(this.txtGioLayMau.Text.Trim()))
            {
                this.txtGioLayMau.BackColor = Color.PapayaWhip;
                kq = false;
            }
            else
            {
                this.txtGioLayMau.BackColor = Color.White;
            }
            if (string.IsNullOrEmpty(this.txtTenBenhNhan.Text.Trim()))
            {
                this.txtTenBenhNhan.BackColor = Color.PapayaWhip;
                kq = false;
            }
            else
            {
                this.txtGioLayMau.BackColor = Color.White;
            }
            //if (string.IsNullOrEmpty(this.txtTenCha.Text.Trim()))
            //{
            //    this.txtTenCha.BackColor = Color.PapayaWhip;
            //    kq = false;
            //}
            //else
            //{
            //    this.txtTenCha.BackColor = Color.White;
            //}

            if (string.IsNullOrEmpty(this.txtNamSinhMe.Text))
            {
                this.txtNamSinhMe.BackColor = Color.PapayaWhip;
                kq = false;
            }
            else
            {
                if (int.Parse(this.txtNamSinhMe.Text.ToString()) < 1945)
                {
                    this.txtNamSinhMe.BackColor = Color.PapayaWhip;
                    kq = false;
                }
                else
                {
                    this.txtNamSinhMe.BackColor = Color.White;
                }
            }
            if(!string.IsNullOrEmpty(this.txtTenCha.Text))
            {
                if (string.IsNullOrEmpty(this.txtNamSinhCha.Text))
                {
                    this.txtNamSinhCha.BackColor = Color.PapayaWhip;
                    kq = false;
                }
                else
                {
                    if (int.Parse(this.txtNamSinhCha.Text.ToString()) < 1945)
                    {
                        this.txtNamSinhCha.BackColor = Color.PapayaWhip;
                        kq = false;
                    }
                    else
                    {
                        this.txtNamSinhMe.BackColor = Color.White;
                    }
                }
            }          

            if (string.IsNullOrEmpty(this.txtDiaChiGiaDinh.Text.Trim()))
            {
                this.txtDiaChiGiaDinh.BackColor = Color.PapayaWhip;
                kq = false;
            }
            else
            {
                this.txtDiaChiGiaDinh.BackColor = Color.White;
            }

            if (string.IsNullOrEmpty(this.txtPara.Text.Trim()))
            {
                this.txtPara.BackColor = Color.PapayaWhip;
                kq = false;
            }
            else
            {
                if (this.txtPara.Text.Length != 4)
                {
                    this.txtPara.BackColor = Color.PapayaWhip;
                    kq = false;
                }
                else
                {
                    this.txtPara.BackColor = Color.White;
                }
            }

            if (string.IsNullOrEmpty(this.txtNgayLayMau.Text))
            {
                this.txtNgayLayMau.BackColor = Color.PapayaWhip;
                kq = false;
            }
            else
            {
                try
                {
                    if (((DateTime)this.txtNgayLayMau.EditValue).Date > ((DateTime)this.txtNgayTiepNhan.EditValue).Date)
                    {
                        XtraMessageBox.Show("Ngày lấy mẫu không được lớn hơn ngày nhận mẫu!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                catch
                {
                    XtraMessageBox.Show("Vui lòng nhập đúng định dạng ngày tháng năm của trường Ngày lấy mẫu!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    kq = false;
                }
            }
            //Thông tin trẻ         
            if (!string.IsNullOrEmpty(this.txtNamSinhBenhNhan.Text) && !string.IsNullOrEmpty(this.txtNgayLayMau.Text))
            {
                try
                {
                    TimeSpan time = DateTime.ParseExact(this.txtNamSinhBenhNhan.Text + " " + this.txtGioSinhBenhNhan.Text, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture) - DateTime.ParseExact(this.txtNgayLayMau.Text + " " + this.txtGioLayMau.Text, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    if (time.Minutes > 0)
                    {
                        XtraMessageBox.Show("Ngày tháng năm sinh trẻ không thể sau ngày lấy mẫu", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        kq = false;
                    }
                }

                catch
                {
                    XtraMessageBox.Show("Vui lòng nhập đúng định dạng ngày tháng năm của trường Ngày tháng năm sinh trẻ!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    kq = false;
                }
            }

            if (this.txtGioiTinh.SelectedIndex < 0)
            {
                txtGioiTinh.BackColor = Color.PapayaWhip;
                kq = false;
            }
            else
            {
                txtGioiTinh.BackColor = Color.White;
            }


            if (string.IsNullOrEmpty(this.txtNamSinhBenhNhan.Text.Trim()))
            {
                this.txtNamSinhBenhNhan.BackColor = Color.PapayaWhip;
            }
            else
            {
                if (DateTime.Parse(this.txtNamSinhBenhNhan.Text.ToString()) > DateTime.Now)
                {
                    this.txtNamSinhBenhNhan.BackColor = Color.PapayaWhip;
                }
                else
                {
                    this.txtNamSinhBenhNhan.BackColor = Color.White;
                }

            }

            if (!string.IsNullOrEmpty(this.txtCanNang.Text))
            {
                if (int.Parse(this.txtCanNang.Text.ToString()) < 500)
                {
                    this.txtCanNang.BackColor = Color.PapayaWhip;
                    kq = false;
                }
            }
            else
            {
                this.txtCanNang.BackColor = Color.PapayaWhip;
            }

            if (!string.IsNullOrEmpty(this.txtTuanTuoi.Text))
            {
                if (int.Parse(this.txtTuanTuoi.Text.ToString()) < 21 && int.Parse(this.txtTuanTuoi.Text) >= 0)
                {
                    this.txtTuanTuoi.BackColor = Color.PapayaWhip;
                    kq = false;
                }
            }
            else
            {
                this.txtTuanTuoi.BackColor = Color.PapayaWhip;
            }           

            if (!this.txtSoLuongTruyenMau.ReadOnly && int.Parse(this.cbbTTTre.EditValue.ToString() ?? "0") > 3)
            {
                try
                {
                    int sl = int.Parse(this.txtSoLuongTruyenMau.Text);
                    if (sl > 1000 || sl < 10)
                    {
                        XtraMessageBox.Show("Vui lòng điền đúng định dạng trường số lượng truyền máu! \r\n Số lượng truyền phải nhỏ hơn 1000ml và lớn 10 ml", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        kq = false;
                    }
                }
                catch
                {
                    XtraMessageBox.Show("Vui lòng điền đúng định dạng trường số lượng truyền máu!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    kq = false;
                }
                if (string.IsNullOrEmpty(this.txtNgayTruyenMau.Text))
                {
                    XtraMessageBox.Show("Vui lòng không để trống trường Ngày truyền máu", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    kq = false;
                }
                else
                {
                    try
                    {
                        DateTime ngay = (DateTime)this.txtNgayTruyenMau.EditValue;
                        if (ngay.Date < this.txtNamSinhBenhNhan.DateTime.Date && int.Parse(this.cbbTTTre.EditValue.ToString() ?? "0") > 3)
                        {
                            XtraMessageBox.Show("Ngày truyền máu không được nhỏ hơn ngày sinh của trẻ", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            kq = false;
                        }
                    }
                    catch
                    {
                        XtraMessageBox.Show("Vui lòng kiểm tra lại ngày truyền máu và ngày sinh của trẻ", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        kq = false;
                    }
                }
            }
            if (!string.IsNullOrEmpty(this.txtPara.Text.Trim()))
            {
                if (this.txtPara.Text.Trim().Length != 4)
                {
                    XtraMessageBox.Show("Mã para phải 4 kí tự.", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    kq = false;
                }
            }
           

            return kq;
        }
        #endregion
        #region // Lưu phiếu
        private void LuuThongTinPhieu(bool isDuyet)
        {
            try
            {

                PSTTPhieu phieu = new PSTTPhieu();
                string lydokhongdat = string.Empty;
                #region TT bệnh nhân
                PSPatient benhNhan = new PSPatient();
                benhNhan.TenBenhNhan = this.ChuyenSangChuHoa(this.txtTenBenhNhan.Text.Trim());
                benhNhan.GioiTinh = this.txtGioiTinh.SelectedIndex < 0 ? 2 : this.txtGioiTinh.SelectedIndex;
                benhNhan.CanNang = int.Parse(this.txtCanNang.Text);
                benhNhan.DanTocID = this.cbbDanToc.EditValue == null ? 1 : int.Parse(this.cbbDanToc.EditValue.ToString());
                string NgaySinhBN = string.IsNullOrEmpty(txtNamSinhBenhNhan.Text) ? DateTime.Now.Date.ToString() : txtNamSinhBenhNhan.Text;
                string GioSinhBN = string.IsNullOrEmpty(txtGioSinhBenhNhan.Text) ? "00:00" : txtGioSinhBenhNhan.Text;
                benhNhan.NgayGioSinh = DateTime.ParseExact(NgaySinhBN + " " + GioSinhBN, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                benhNhan.NoiSinh = this.txtNoiSinh.Text;
                benhNhan.PhuongPhapSinh = this.cboPhuongPhapSinh.EditValue == null ? 2 : this.cboPhuongPhapSinh.SelectedIndex;
                benhNhan.QuocTichID = 1;
                benhNhan.TuanTuoiKhiSinh = int.Parse(this.txtTuanTuoi.Text);
                benhNhan.MaBenhNhan = this.txtMaBenhNhan.Text;
                if (!string.IsNullOrEmpty(txtTenCha.Text))
                {
                    benhNhan.FatherBirthday = string.IsNullOrEmpty(this.txtNamSinhCha.EditValue.ToString()) == true ? DateTime.Now : (DateTime)this.txtNamSinhCha.EditValue;
                }
                if (!string.IsNullOrEmpty(txtTenMe.Text))
                {
                    benhNhan.MotherBirthday = string.IsNullOrEmpty(this.txtNamSinhMe.EditValue.ToString()) == true ? DateTime.Now : (DateTime)this.txtNamSinhMe.EditValue;
                }
                benhNhan.FatherPhoneNumber = this.txtSDTCha.Text;
                benhNhan.MotherPhoneNumber = this.txtSDTMe.Text;
                benhNhan.FatherName = this.ChuyenSangChuHoa(this.txtTenCha.Text);
                benhNhan.MotherName = this.ChuyenSangChuHoa(this.txtTenMe.Text);
                benhNhan.DiaChi = this.txtDiaChiGiaDinh.Text;
                benhNhan.Para = this.txtPara.Text.TrimEnd();
                phieu.Benhnhan = benhNhan;
                #endregion

                #region Phiếu sàng lọc
                PSPhieuSangLoc phieuSangLoc = new PSPhieuSangLoc();
                phieuSangLoc.TrangThaiMau = byte.Parse(txtTrangThaiMau.EditValue.ToString());
                phieuSangLoc.IDNhanVienTaoPhieu = emp.EmployeeCode;
                phieuSangLoc.IDCoSo = this.cbbDonViChon.EditValue.ToString();
                phieuSangLoc.IDViTriLayMau = byte.Parse(this.cbbViTriLayMau.EditValue.ToString());
                phieuSangLoc.isKhongDat = this.radioDanhGia.EditValue.ToString() != "1" ? true : false;
                phieu.lstChiDinhDichVu = this.lstChiDinhDichVu;
                phieu.lstLyDoKhongDat = this.lstDanhGiaChatLuong;
                if (!string.IsNullOrEmpty(this.txtMaPhieu1.Text.Trim()))
                    phieuSangLoc.isLayMauLan2 = true;
                else phieuSangLoc.isLayMauLan2 = false;
                phieuSangLoc.MaBenhNhan = this.txtMaBenhNhan.Text.Trim();
                phieuSangLoc.CheDoDinhDuong = byte.Parse(this.cbbCheDoDD.EditValue.ToString());
                phieuSangLoc.IDChuongTrinh = this.cbbChuongTrinh.EditValue.ToString();
                phieuSangLoc.MaGoiXN = this.radioGroupGoiXN.EditValue.ToString();
                phieuSangLoc.IDPhieuLan1 = this.txtMaPhieu1.Text;
                phieuSangLoc.TinhTrangLucLayMau = byte.Parse(this.cbbTTTre.EditValue.ToString());
                string NgayLayMau = string.IsNullOrEmpty(txtNgayLayMau.Text) ? DateTime.Now.Date.ToString() : txtNgayLayMau.Text;
                string GioLayMau = string.IsNullOrEmpty(txtGioLayMau.Text) ? "00:00" : txtGioLayMau.Text;
                phieuSangLoc.NgayGioLayMau = DateTime.ParseExact(NgayLayMau + " " + GioLayMau, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture); ;
                phieuSangLoc.NgayNhanMau = this.txtNgayTiepNhan.DateTime;
                phieuSangLoc.IDPhieu = this.txtMaPhieu.Text;
                phieuSangLoc.LuuYPhieu = this.txtGhiChu.Text;
                if (cbbTTTre.EditValue.ToString() == "4")
                {
                    phieuSangLoc.NgayTruyenMau = string.IsNullOrEmpty(this.txtNgayTruyenMau.EditValue.ToString()) == true ? DateTime.Now : (DateTime)this.txtNgayTruyenMau.EditValue;
                    if (!string.IsNullOrEmpty(this.txtSoLuongTruyenMau.Text))
                        phieuSangLoc.SLTruyenMau = short.Parse(this.txtSoLuongTruyenMau.Text);
                }
                phieuSangLoc.SDTNhanVienLayMau = this.txtSDTNguoiLayMau.Text;
                phieuSangLoc.TenNhanVienLayMau = this.ChuyenSangChuHoa(this.txtNguoiLayMau.Text);
                phieuSangLoc.NoiLayMau = this.txtNoiLayMau.Text;
                phieuSangLoc.DiaChiLayMau = this.txtDiaChiNoiLayMau.Text;
                phieuSangLoc.Para = this.txtPara.Text.TrimEnd();
                phieuSangLoc.NgayTaoPhieu = DateTime.Now;
                // this.ValidateFrom();

                foreach (var item in this.lstDanhGiaSoBo)
                {
                    if (item.maGiaTri.Equals("islaymautruoc24h"))
                    {
                        phieuSangLoc.isTruoc24h = item.giaTri == true ? true : false;
                    }
                    if (item.maGiaTri.Equals("isSinhNon"))
                    {
                        phieuSangLoc.isSinhNon = item.giaTri == true ? true : false;
                    }
                    if (item.maGiaTri.Equals("isNheCan"))
                    {
                        phieuSangLoc.isNheCan = item.giaTri == true ? true : false;
                    }
                    if (item.maGiaTri.Equals("isGuiMauTre"))
                    {
                        phieuSangLoc.isGuiMauTre = item.giaTri == true ? true : false;
                    }
                }

                if (phieu.lstLyDoKhongDat.Count > 0)
                {
                    List<PSDanhMucDanhGiaChatLuongMau> lstTTLyDo = BioNet_Bus.GetDanhMucDanhGiaChatLuong();
                    if (lstTTLyDo.Count > 0)
                    {
                        foreach (var ld in phieu.lstLyDoKhongDat)
                        {
                            var res = lstTTLyDo.FirstOrDefault(o => o.IDDanhGiaChatLuongMau == ld.IDDanhGiaChatLuongMau);
                            if (res != null)
                            {
                                ld.TenLyDo = res.ChatLuongMau;
                                if (string.IsNullOrEmpty(lydokhongdat))
                                    lydokhongdat += res.ChatLuongMau;
                                else lydokhongdat += ". " + res.ChatLuongMau;
                            }
                        }
                    }
                }
                phieuSangLoc.LyDoKhongDat = lydokhongdat;
                phieu.Phieu = phieuSangLoc;
                #endregion
                #region Đợt chỉ định
                PSChiDinhDichVu DotchiDinh = new PSChiDinhDichVu();
                DotchiDinh.MaNVChiDinh = emp.EmployeeCode;
                DotchiDinh.MaChiDinh = this.txtMaChiDinh.Text.Trim();
                DotchiDinh.MaDonVi = this.cbbDonViChon.EditValue.ToString();
                DotchiDinh.IDGoiDichVu = this.radioGroupGoiXN.EditValue.ToString();
                DotchiDinh.MaTiepNhan = this.txtMaTiepNhan.Text.Trim();
                DotchiDinh.MaPhieu = this.txtMaPhieu.Text.Trim();
                DotchiDinh.MaPhieuLan1 = this.txtMaPhieu1.Text.Trim();
                DotchiDinh.NgayChiDinhHienTai = DateTime.Now;
                DotchiDinh.NgayChiDinhLamViec = DateTime.Now;
                DotchiDinh.NgayTiepNhan = this.txtNgayTiepNhan.DateTime;
                DotchiDinh.SoLuong = 1;
                DotchiDinh.DonGia = int.Parse(this.txtGiaGoiXN.EditValue.ToString());
                DotchiDinh.isLayMauLai = false;
                #endregion
                #region Tiếp nhận
                PSTiepNhan tiepNhan = new PSTiepNhan();
                tiepNhan.MaTiepNhan = this.txtMaTiepNhan.Text;
                tiepNhan.MaDonVi = this.cbbDonViChon.EditValue.ToString();
                tiepNhan.MaPhieu = this.txtMaPhieu.Text;
                phieu.TiepNhan = tiepNhan;
                #endregion
                phieu.ChiDinh = DotchiDinh;
                    SplashScreenManager.ShowForm(this, typeof(DiaglogFrm.WaitingfromSave), true, true, false);
                    var result = BioNet_Bus.DuyetPhieuBT(phieu);
                    SplashScreenManager.CloseForm();
                    if (result.Result)
                    {
                        BioNet_Bus.UpdateThongTinPhieuLan1(this.txtMaPhieu1.Text.Trim().ToString());
                        XtraMessageBox.Show("Đã lưu!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                        this.LoadNew();
                        this.ReadOnly(false);
                        this.LoadDanhSachTiepNhan();
                        this.LoadDanhSachDaDuyet();
                        this.btnDuyet.Enabled = false;
                        this.btnLuu.Enabled = false;
                        this.GVDanhSachTiepNhan.Focus();
                    }
                    else
                    {
                        XtraMessageBox.Show("Lỗi không thể lưu thông tin phiếu và đánh giá mẫu.\r\n Lỗi chi tiết " + result.StringError, "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
            }

            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi \r\n Lỗi chi tiết :" + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region Đánh giá phiếu
        #endregion
        #region Kiểm tra phiếu
        private int GiaTriNheCan = 2500;
        private int GiaTriSinhNon = 36;
        private void LoadThongTinLuuY()
        {
            this.txtLuuY.ResetText();
            string str = string.Empty;
            foreach (var item in this.lstDanhGiaSoBo)
            {
                if (item.giaTri)
                {
                    str += "- " + item.noiDungChuThich + "\r\n";
                }
            }
            this.txtLuuY.Text = str;
        }
        private void ValidateSinhNon()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtTuanTuoi.Text.Trim()))
                {
                    bool giatri = false;
                    int tuan = int.Parse(this.txtTuanTuoi.EditValue.ToString());
                    if(tuan<=this.GiaTriSinhNon)
                    {
                        giatri = true;
                    }
                    else
                    {
                        giatri = false;
                    }
                    var tt = this.lstDanhGiaSoBo.FirstOrDefault(p => p.maGiaTri == "isSinhNon");
                    if(tt!=null)
                    {
                        tt.giaTri = giatri;
                    }
                    else
                    {
                        PsDanhGiaMauSoBo dg1 = new PsDanhGiaMauSoBo();
                        dg1.giaTri = giatri;
                        dg1.maGiaTri = "isSinhNon";
                        dg1.noiDungChuThich = "Sinh non";
                        this.lstDanhGiaSoBo.Add(dg1);
                    }
                    this.LoadThongTinLuuY();
                }
            }
            catch { }
        }
        private void ValidateNheCan()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtCanNang.Text.Trim()))
                {
                    if (!string.IsNullOrEmpty(this.txtCanNang.Text.Trim()))
                    {
                        bool giatri = false;
                        int cannang = int.Parse(this.txtCanNang.EditValue.ToString());
                        if (cannang < this.GiaTriNheCan)
                        {
                            giatri = true;
                        }
                        else
                        {
                            giatri = false;
                        }
                        var tt = this.lstDanhGiaSoBo.FirstOrDefault(p => p.maGiaTri == "isNheCan");
                        if (tt != null)
                        {
                            tt.giaTri = giatri;
                        }
                        else
                        {
                            PsDanhGiaMauSoBo dg1 = new PsDanhGiaMauSoBo();
                            dg1.giaTri = true;
                            dg1.maGiaTri = "isNheCan";
                            dg1.noiDungChuThich = "Nhẹ cân";
                            this.lstDanhGiaSoBo.Add(dg1);
                        }
                        this.LoadThongTinLuuY();
                    }
                    this.LoadThongTinLuuY();
                }
            }
            catch { }
        }
        private void ValidateGuiMauTre(string ngaylaymau,string ngaynhanmau)
        {
            try
            {
                var dg = this.lstDanhGiaSoBo.FirstOrDefault(p => p.maGiaTri == "isGuiMauTre");
                TimeSpan time;
                if (!string.IsNullOrEmpty(ngaylaymau) && !string.IsNullOrEmpty(ngaynhanmau))
                {

                    time = DateTime.Parse(ngaynhanmau) - DateTime.Parse(ngaylaymau);
                }
                else if (!string.IsNullOrEmpty(this.txtNgayLayMau.Text) && !string.IsNullOrEmpty(this.txtGioLayMau.Text))
                {
                    DateTime ngayGioLayMau = DateTime.ParseExact(this.txtNgayLayMau.Text + " " + this.txtGioLayMau.Text, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime ngayNhanMau = DateTime.ParseExact(this.txtNgayTiepNhan.Text, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    time = ngayNhanMau - ngayGioLayMau;
                }
                else
                {
                    if (dg != null)
                    {
                        dg.giaTri = true;
                        dg.noiDungChuThich = "Chưa có dữ liệu khoảng thời gian gửi mẫu";
                    }    
                    else
                    {
                        PsDanhGiaMauSoBo dg1 = new PsDanhGiaMauSoBo();
                        dg1.giaTri = true;
                        dg1.maGiaTri = "isGuiMauTre";
                        dg1.noiDungChuThich = "Chưa có dữ liệu khoảng thời gian gửi mẫu";
                        this.lstDanhGiaSoBo.Add(dg1);
                    }
                    this.LoadThongTinLuuY();
                    return;
                }
                if (time.TotalMinutes > 5760)
                {
                   
                    if (dg != null)
                    {
                        dg.giaTri = true;
                        if (time.TotalMinutes > 7200)
                        {
                            dg.noiDungChuThich = "Gửi mẫu rất muộn";
                        }
                        else
                        {
                            dg.noiDungChuThich = "Gửi mẫu muộn";
                        }
                    } 
                    else
                    {
                        PsDanhGiaMauSoBo dg1 = new PsDanhGiaMauSoBo();
                        dg1.giaTri = true;
                        dg1.maGiaTri = "isGuiMauTre";
                        if (time.TotalMinutes > 7200)
                        {
                            dg1.noiDungChuThich = "Gửi mẫu rất muộn";
                        }
                        else
                        {
                            dg1.noiDungChuThich = "Gửi mẫu muộn";
                        }
                        this.lstDanhGiaSoBo.Add(dg1);
                    }
                }
                else
                {
                    if (dg != null)
                    {
                        dg.giaTri = false;
                    }  
                    else
                    {
                        PsDanhGiaMauSoBo dg1 = new PsDanhGiaMauSoBo();
                        dg1.giaTri = false;
                        dg1.maGiaTri = "isGuiMauTre";
                        this.lstDanhGiaSoBo.Add(dg1);
                    }
                }
                this.LoadThongTinLuuY();
            }
            catch { }
        }
        private void ValidateThuMauSom(string ngayGioSinh, string ngaylaymau)
        {
            try
            {
                var dg = this.lstDanhGiaSoBo.FirstOrDefault(p => p.maGiaTri == "islaymautruoc24h");
                TimeSpan time;
                if (!string.IsNullOrEmpty(ngaylaymau) && !string.IsNullOrEmpty(ngayGioSinh))
                {
                    time = DateTime.Parse(ngaylaymau) - DateTime.Parse(ngayGioSinh);
                }
                else if (!string.IsNullOrEmpty(this.txtNgayLayMau.Text) && !string.IsNullOrEmpty(this.txtGioLayMau.Text) && !string.IsNullOrEmpty(this.txtNamSinhBenhNhan.Text) && !string.IsNullOrEmpty(this.txtGioSinhBenhNhan.Text))
                {
                    DateTime ngayGioLayMau = DateTime.ParseExact(this.txtNgayLayMau.Text + " " + this.txtGioLayMau.Text, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime ngaySinh = DateTime.ParseExact(this.txtNamSinhBenhNhan.Text + " " + this.txtGioSinhBenhNhan.Text, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    time = ngayGioLayMau - ngaySinh;
                }
                else
                {
                    if (dg != null)
                    {
                        dg.giaTri = true;
                        dg.noiDungChuThich = "Chưa có dữ liệu khoảng thời gian thu mẫu";
                    }
                    else
                    {
                        PsDanhGiaMauSoBo dg1 = new PsDanhGiaMauSoBo();
                        dg1.giaTri = true;
                        dg1.maGiaTri = "islaymautruoc24h";
                        dg1.noiDungChuThich = "Chưa có dữ liệu khoảng thời gian thu mẫu";
                        this.lstDanhGiaSoBo.Add(dg1);
                    }
                    this.LoadThongTinLuuY();
                    return;
                }

                if (time.TotalMinutes < 1800)
                {
                    if (dg != null)
                    {
                        dg.giaTri = true;
                        dg.noiDungChuThich = "Lấy mẫu trước 30h sau khi sinh";
                    }             
                    else
                    {
                        PsDanhGiaMauSoBo dg1 = new PsDanhGiaMauSoBo();
                        dg1.giaTri = true;
                        dg1.maGiaTri = "islaymautruoc24h";
                        dg1.noiDungChuThich = "Lấy mẫu trước 30h sau khi sinh";
                        this.lstDanhGiaSoBo.Add(dg1);
                    }
                }
                else
                {
                    if (dg != null)
                    {
                        dg.giaTri = false;
                    }  
                    else
                    {
                        PsDanhGiaMauSoBo dg1 = new PsDanhGiaMauSoBo();
                        dg1.giaTri = false;
                        dg1.maGiaTri = "islaymautruoc24h";
                        this.lstDanhGiaSoBo.Add(dg1);
                    }
                }
                this.LoadThongTinLuuY();
            }
            catch { }
        }
        private bool KiemTraIsDaNhapLieu()
        {
            bool kq = true;
            if(string.IsNullOrEmpty(txtTenBenhNhan.Text))
            {
                txtTenBenhNhan.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtTenBenhNhan.BackColor = Color.White;
            }
            if (string.IsNullOrEmpty(txtTenMe.Text))
            {
                txtTenMe.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtTenMe.BackColor = Color.White;
            }
            if (string.IsNullOrEmpty(txtNamSinhMe.Text))
            {
                txtNamSinhMe.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtNamSinhMe.BackColor = Color.White;
            }
            if (string.IsNullOrEmpty(txtNoiSinh.Text))
            {
                txtNoiSinh.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtNoiSinh.BackColor = Color.White;
            }
            if (string.IsNullOrEmpty(txtSDTMe.Text))
            {
                txtSDTMe.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtSDTMe.BackColor = Color.White;
            }
            if (string.IsNullOrEmpty(txtGioiTinh.Text))
            {
                txtGioiTinh.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtGioiTinh.BackColor = Color.White;
            }
            if (string.IsNullOrEmpty(txtNgayLayMau.Text))
            {
                txtNgayLayMau.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtNgayLayMau.BackColor = Color.White;
            }
            if (string.IsNullOrEmpty(txtNamSinhBenhNhan.Text))
            {
                txtNamSinhBenhNhan.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtNamSinhBenhNhan.BackColor = Color.White;
            }

            if (string.IsNullOrEmpty(txtDiaChiGiaDinh.Text))
            {
                txtDiaChiGiaDinh.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtDiaChiGiaDinh.BackColor = Color.White;
            }

            if (string.IsNullOrEmpty(txtCanNang.Text))
            {
                txtCanNang.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtCanNang.BackColor = Color.White;
            }

            if (string.IsNullOrEmpty(txtPara.Text))
            {
                txtPara.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtPara.BackColor = Color.White;
            }

            if (string.IsNullOrEmpty(txtTuanTuoi.Text))
            {
                txtTuanTuoi.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtTuanTuoi.BackColor = Color.White;
            }
            if (string.IsNullOrEmpty(txtNoiLayMau.Text))
            {
                txtNoiLayMau.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtNoiLayMau.BackColor = Color.White;
            }
            if (string.IsNullOrEmpty(txtDiaChiNoiLayMau.Text))
            {
                txtDiaChiNoiLayMau.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtDiaChiNoiLayMau.BackColor = Color.White;
            }
            return kq;
        }
        #endregion
       
        private string ChuyenSangChuHoa(string chuoi)
        {
            if (!string.IsNullOrEmpty(chuoi))
                return chuoi.ToUpper();
            else return string.Empty;
        }

        private void txtGiaGoiXN_EditValueChanged(object sender, EventArgs e)
        {

        }

        

        private void txtCanNang_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCanNang.Text))
            {
                txtCanNang.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtCanNang.BackColor = Color.White;
            }
            ValidateNheCan();
        }

        private void txtTuanTuoi_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTuanTuoi.Text))
            {
                txtTuanTuoi.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtTuanTuoi.BackColor = Color.White;
            }
            ValidateSinhNon();
        }

        private void txtMaPhieuHienThi_Validated(object sender, EventArgs e)
        {
            var phieu = BioNet_Bus.GetThongTinTiepNhanTheoMaPhieu(txtMaPhieuHienThi.Text);
            if(phieu!=null)
            {
                HienThiThongTinPhieu(phieu.MaPhieu, phieu.MaDonVi);
            }
            else
            {
                DiaglogFrm.FrmWarning notData = new DiaglogFrm.FrmWarning("Mã phiếu không tồn tại.");
                notData.ShowDialog();
            }
        }

        private void txtMaPhieuHienThi_Enter(object sender, EventArgs e)
        {

        }

        private void txtTenMe_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenMe.Text))
            {
                txtTenMe.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtTenMe.BackColor = Color.White;
            }
        }

        private void txtPara_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPara.Text) || txtPara.Text.Length!=4)
            {
                txtPara.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtPara.BackColor = Color.White;
            }
        }

        private void txtTenBenhNhan_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenBenhNhan.Text) )
            {
                txtTenBenhNhan.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtTenBenhNhan.BackColor = Color.White;
            }
        }

        private void txtNamSinhMe_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtNamSinhMe.Text))
            {
                this.txtNamSinhMe.BackColor = Color.PapayaWhip;
            }
            else
            {
                if (int.Parse(this.txtNamSinhMe.Text.ToString()) < 1945 || int.Parse(this.txtNamSinhMe.Text)<=DateTime.Now.Year)
                {
                    this.txtNamSinhMe.BackColor = Color.PapayaWhip;
                }
                else
                {
                    this.txtNamSinhMe.BackColor = Color.White;
                }
            }
        }

        private void txtDiaChiGiaDinh_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtDiaChiGiaDinh.Text.Trim()))
            {
                this.txtDiaChiGiaDinh.BackColor = Color.PapayaWhip;
            }
            else
            {
                this.txtDiaChiGiaDinh.BackColor = Color.White;
            }
        }

        private void txtNoiLayMau_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtNoiLayMau.Text.Trim()))
            {
                this.txtNoiLayMau.BackColor = Color.PapayaWhip;
            }
            else
            {
                this.txtNoiLayMau.BackColor = Color.White;
            }
        }

        private void txtDiaChiNoiLayMau_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtNoiLayMau.Text.Trim()))
            {
                this.txtDiaChiNoiLayMau.BackColor = Color.PapayaWhip;
            }
            else
            {
                this.txtDiaChiNoiLayMau.BackColor = Color.White;
            }
        }

        private void txtNguoiLayMau_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtNoiLayMau.Text.Trim()))
            {
                this.txtNguoiLayMau.BackColor = Color.PapayaWhip;
            }
            else
            {
                this.txtNguoiLayMau.BackColor = Color.White;
            }
        }

        private void txtSDTNguoiLayMau_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtSDTNguoiLayMau.Text.Trim()))
            {
                this.txtSDTNguoiLayMau.BackColor = Color.PapayaWhip;
            }
            else
            {
                this.txtSDTNguoiLayMau.BackColor = Color.White;
            }
        }

        private void txtNgayLayMau_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtSDTNguoiLayMau.Text.Trim()))
            {
                this.txtSDTNguoiLayMau.BackColor = Color.PapayaWhip;
            }
            else
            {
                this.txtSDTNguoiLayMau.BackColor = Color.White;
            }
        }

        private void txtGioLayMau_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtSDTNguoiLayMau.Text.Trim()))
            {
                this.txtSDTNguoiLayMau.BackColor = Color.PapayaWhip;
            }
            else
            {
                this.txtSDTNguoiLayMau.BackColor = Color.White;
            }
        }

        private void cbbViTriLayMau_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.cbbViTriLayMau.Text.Trim()))
            {
                this.cbbViTriLayMau.BackColor = Color.PapayaWhip;
            }
            else
            {
                this.cbbViTriLayMau.BackColor = Color.White;
            }
        }

        private void GVDanhSachDaTracking_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.GVDanhSachDaTracking.RowCount > 0)
                {
                    if (this.GVDanhSachDaTracking.GetFocusedRow() != null)
                    {
                        string maPhieu = this.GVDanhSachDaTracking.GetRowCellValue(this.GVDanhSachDaTracking.FocusedRowHandle, this.col_maPhieu_GCDaTracking).ToString();
                        string maDonVi = this.GVDanhSachDaTracking.GetRowCellValue(this.GVDanhSachDaTracking.FocusedRowHandle, this.col_maDonVi_GCDaTracking).ToString();
                        this.HienThiThongTinPhieu(maPhieu, maDonVi);
                    }
                }
            }
            catch { }
        }

        private void txtTuNgay_EditValueChanged(object sender, EventArgs e)
        {
            LoadDanhSachDaDuyet();
        }

        private void txtDenNgay_EditValueChanged(object sender, EventArgs e)
        {
            LoadDanhSachDaDuyet();
        }

        private void cbbChiCucDaDuyet_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit sear = sender as SearchLookUpEdit;
                var value = sear.EditValue.ToString();
                this.cbbDonViDaDuyet.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi(value.ToString());
                this.cbbDonViDaDuyet.EditValue = "all";
                if (cbbChiCucDaDuyet.EditValue.ToString() != "all")
                {
                    FilterTiepNhanTheoDonViDaTiepNhan();
                }
                else
                {
                    GVDanhSachDaTracking.ClearColumnsFilter();
                }
            }
            catch { }
        }
        private void FilterTiepNhanTheoDonViDaTiepNhan()
        {
            string machicuc = cbbChiCucDaDuyet.EditValue.ToString();
            string madv = cbbDonViDaDuyet.EditValue.ToString();
            if (!machicuc.Equals("all") && madv.Equals("all"))
            {
                madv = machicuc;
            }
            string filterMaDV = "Contains([MaDonVi], '" + madv + "')";
            GVDanhSachDaTracking.Columns["MaDonVi"].FilterInfo = new ColumnFilterInfo(filterMaDV);

        }

        private void cbbGoiXNLoc_EditValueChanged(object sender, EventArgs e)
        {
            string filter = "Contains([IDGoiDichVu], '" + cbbGoiXNLoc.EditValue + "')";
            GVDanhSachDaTracking.Columns["IDGoiDichVu"].FilterInfo = new ColumnFilterInfo(filter);
        }

        private void txtMaPhieu1_Validated(object sender, EventArgs e)
        {
            this.LoadTTPhieuLan1();
        }

        private void HienThiTTPhieuLan1(PSTTPhieu ph)
        { 
            if(string.IsNullOrEmpty(txtTenBenhNhan.Text))
            {
                txtTenBenhNhan.Text = ph.Benhnhan.TenBenhNhan;
            }
            if (string.IsNullOrEmpty(txtGioiTinh.Text))
            {
                txtGioiTinh.EditValue = ph.Benhnhan.GioiTinh.ToString();
            }
            if (string.IsNullOrEmpty(txtDiaChiGiaDinh.Text))
            {
                txtDiaChiGiaDinh.Text = ph.Benhnhan.DiaChi;
            }
            if (string.IsNullOrEmpty(txtDiaChiGiaDinh.Text))
            {
                txtDiaChiGiaDinh.Text = ph.Benhnhan.DiaChi;
            }
            if (string.IsNullOrEmpty(txtNamSinhBenhNhan.EditValue.ToString()))
            {
                txtNamSinhBenhNhan.EditValue = ph.Benhnhan.NgayGioSinh.Value;
                txtGioSinhBenhNhan.EditValue = ph.Benhnhan.NgayGioSinh.Value;
            }
            if(string.IsNullOrEmpty(txtTenMe.Text))
            {
                txtTenMe.Text = ph.Benhnhan.MotherName;
                txtNamSinhMe.EditValue = ph.Benhnhan.MotherBirthday;
                txtSDTMe.Text = ph.Benhnhan.MotherPhoneNumber;
            }
            if (string.IsNullOrEmpty(txtTenCha.Text))
            {
                txtTenCha.Text = ph.Benhnhan.FatherName;
                txtNamSinhCha.EditValue = ph.Benhnhan.FatherBirthday;
                txtSDTCha.Text = ph.Benhnhan.FatherPhoneNumber;
            }
            if (string.IsNullOrEmpty(txtPara.Text))
            {
                txtPara.Text = ph.Benhnhan.Para;
            }
            cbbDanToc.EditValue= cbbDanToc.EditValue.ToString() != ph.Benhnhan.DanTocID.ToString() ? ph.Benhnhan.DanTocID:cbbDanToc.EditValue;
            cboPhuongPhapSinh.EditValue = cboPhuongPhapSinh.EditValue.ToString() != ph.Benhnhan.PhuongPhapSinh.ToString() ? ph.Benhnhan.PhuongPhapSinh : cboPhuongPhapSinh.EditValue;
            cbbCheDoDD.EditValue = cbbCheDoDD.EditValue.ToString() != ph.Phieu.CheDoDinhDuong.ToString() ? ph.Phieu.CheDoDinhDuong : cbbCheDoDD.EditValue;
            cbbTTTre.EditValue = cbbTTTre.EditValue.ToString() != ph.Phieu.TinhTrangLucLayMau.ToString() ? ph.Phieu.TinhTrangLucLayMau : cbbTTTre.EditValue;

        }

        private void txtMaPhieuDaDuyetSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                if (!string.IsNullOrEmpty(txtMaPhieuDaDuyetSearch.Text))
                {
                    string filterMaPhieu = "[MaPhieu]='" + txtMaPhieuDaDuyetSearch.Text + "'";
                    GVDanhSachDaTracking.Columns["MaPhieu"].FilterInfo = new ColumnFilterInfo(filterMaPhieu);
                }
            }
        }

        private void cbbDonViDaDuyet_EditValueChanged(object sender, EventArgs e)
        {
            if (cbbChiCucTiepNhan.EditValue.ToString() != "all" || cbbDonViTiepNhan.EditValue.ToString() != "all")
            {
                FilterTiepNhanTheoDonViDaTiepNhan();
            }
            else
            {
                GVDanhSachDaTracking.ClearColumnsFilter();
            }
        }

        private void txtMaPhieu1_EditValueChanged(object sender, EventArgs e)
        {
            this.LoadTTPhieuLan1();
        }
        private void LoadTTPhieuLan1()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtMaPhieu1.Text) && string.IsNullOrEmpty(this.txtTenBenhNhan.Text))
                {
                    var phieu = BioNet_Bus.GetTTPhieu(txtMaPhieu1.Text, this.cbbDonViChon.EditValue.ToString());
                    if (phieu.Phieu != null)
                    {
                        if (phieu.Phieu.TrangThaiMau > 5 )
                        {
                            this.listdvcanlamlai.Clear();
                            this.listdvcanlamlai = BioNet_Bus.GetDichVuCanLamLaiCuaPhieu(phieu.Phieu.IDPhieu, phieu.Phieu.IDCoSo);
                            if (listdvcanlamlai.Count < 0)
                            {
                                XtraMessageBox.Show("Phiếu " + txtMaPhieu1.Text + "được chỉ định thu lại \r\n nhưng không tìm thấy danh sách thông số nguy cơ cao cần xét nghiệm lại. \r\n Vui lòng kiểm tra lại!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
                            }
                            if(int.Parse(txtTrangThaiMau.EditValue.ToString())>2)
                            {
                                if (!BioNet_Bus.KiemTraPhieuThuMauLaiDaDuocChiDinhChua(txtMaPhieu1.Text, txtMaPhieu.Text.TrimEnd()))
                                {
                                    HienThiTTPhieuLan1(phieu);
                                }
                                else
                                {
                                    XtraMessageBox.Show("Phiếu " + txtMaPhieu1.Text + " đã được thu mẫu lại rồi", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    this.txtMaPhieu1.ResetText();
                                }
                            }
                        }
                        else 
                            XtraMessageBox.Show("Phiếu " + txtMaPhieu1.Text + " Không có được yêu cầu lấy mẫu lại. \r\n Vui lòng kiểm tra lại!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    else XtraMessageBox.Show("Không tìm thấy thông tin cũ của phiếu " + txtMaPhieu1.Text + " thuộc đơn vị " + this.cbbDonViChon.Text + ". Vui lòng kiểm tra lại!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi khi lấy thông tin phiếu cũ. Lỗi chi tiết \r\n" + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void LayDanhSachDanhGiaHangLoat()
        {
            this.lstDSCanDanhGia.Clear();
            int[] lstChecked = this.GVDanhSachTiepNhan.GetSelectedRows();
            foreach (var index in lstChecked)
            {
                if (index >= 0)
                {
                    try
                    {
                        long rowIDTiepNhan = long.Parse(this.GVDanhSachTiepNhan.GetRowCellValue(index, this.col_RowIDTiepNhan).ToString());
                        var Tn = BioNet_Bus.GetThongTinTiepNhan(rowIDTiepNhan);

                        if (Tn != null)
                            this.lstDSCanDanhGia.Add(Tn);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show("Lỗi khi lấy thông tin tiếp nhận của phiếu \r\n Lỗi chi tiết :" + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
        private void btnDuyetNhieuPhieu_Click(object sender, EventArgs e)
        {
            try
            {
                this.LayDanhSachDanhGiaHangLoat();
                if (this.lstDSCanDanhGia.Count > 0)
                {
                    List<PsPhieuLoiKhiDanhGia> listphieuloi = new List<PsPhieuLoiKhiDanhGia>();
                    DiaglogFrm.FrmDiaglogChonGoiXetNghiem frm = new DiaglogFrm.FrmDiaglogChonGoiXetNghiem();
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        string maGoiXN = frm.maGoiXn;
                        SplashScreenManager.ShowForm(this, typeof(DiaglogFrm.WaitingfromSave), true, true, false);
                        foreach (var item in this.lstDSCanDanhGia)
                        {
                            var result=this.DuyetPhieu(item.MaDonVi, item.MaPhieu,maGoiXN);
                            if (!result.Result)
                            {
                                PsPhieuLoiKhiDanhGia phieu = new PsPhieuLoiKhiDanhGia();
                                phieu.MaPhieu = item.MaPhieu;
                                phieu.ThongTinLoi = result.StringError;
                                listphieuloi.Add(phieu);
                            }
                        }
                        SplashScreenManager.CloseForm();
                        if (listphieuloi.Count < 1)
                        {
                            XtraMessageBox.Show("Lưu thành công!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            DiaglogFrm.FrmDiagLogShowPhieuLoi frmloi = new DiaglogFrm.FrmDiagLogShowPhieuLoi(listphieuloi);
                            frmloi.ShowDialog();
                        }
                        this.LoadDanhSachTiepNhan();
                    }


                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi \r\n Lỗi chi tiết :" + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }
        private PsReponse DuyetPhieu(String MaDonVi,String MaPhieu,String MaGoiXN)
        {
            PsReponse res = new PsReponse();
            try
            {
                
                PsPhieu phieu = BioNet_Bus.GetThongTinPhieu(MaPhieu, MaDonVi);
                PSTiepNhan tiepnhan = BioNet_Bus.GetThongTinTiepNhanTheoMaPhieu(MaPhieu);
                var donvi = BioNet_Bus.GetThongTinDonViCoSo(MaDonVi);
                PsChiDinhvsDanhGia DotchiDinh = new PsChiDinhvsDanhGia();
                if (phieu != null && tiepnhan != null)
                {
                    phieu.ngayGioLayMau = phieu.ngayGioLayMau;
                    phieu.isKhongDat = false;
                    phieu.lydokhongdat = String.Empty;
                    phieu.lstLyDoKhongDat = new List<PSChiTietDanhGiaChatLuong>();
                    phieu.DiaChiLayMau = donvi.DiaChiDVCS;
                    phieu.NoiLayMau = donvi.TenDVCS;
                    phieu.maChuongTrinh = phieu.maChuongTrinh ?? "CTXH0001";
                    phieu.TenNhanVienLayMau = phieu.TenNhanVienLayMau == null ? this.ChuyenSangChuHoa(this.txtNguoiLayMau.Text) : phieu.TenNhanVienLayMau;
                    phieu.SoDTNhanVienLayMau = phieu.SoDTNhanVienLayMau ?? txtSDTNguoiLayMau.Text.ToString();
                    phieu.LuuYPhieu = "";
                    phieu.maGoiXetNghiem = string.IsNullOrEmpty(phieu.maGoiXetNghiem)!=true ? phieu.maGoiXetNghiem : MaGoiXN;
                    phieu.lstChiDinhDichVu = BioNet_Bus.GetDanhSachDichVuTheoMaGoi(phieu.maGoiXetNghiem, MaDonVi);
                    if (phieu.BenhNhan.CanNang != null)
                    {
                        phieu.isNheCan = phieu.BenhNhan.CanNang < GiaTriNheCan ? true : false;
                    }
                    if (phieu.BenhNhan.TuanTuoiKhiSinh != null)
                    {
                        phieu.isSinhNon = phieu.BenhNhan.TuanTuoiKhiSinh < GiaTriSinhNon ? true : false;
                    }
                    TimeSpan TimeLayMau = phieu.ngayGioLayMau - phieu.BenhNhan.NgayGioSinh ?? new TimeSpan();
                    if (TimeLayMau.TotalMinutes < 5760)
                    {
                        phieu.isTruoc24h = true;
                    }
                    else
                    {
                        phieu.isTruoc24h = false;
                    }
                    TimeSpan TimeNhanMau = phieu.ngayNhanMau - phieu.ngayGioLayMau;
                    if (TimeNhanMau.TotalMinutes > 7200)
                    {
                        phieu.isGuiMauTre = true;
                    }
                    else
                    {
                        phieu.isGuiMauTre = false;
                    }
                    phieu.BenhNhan.DanTocID = phieu.BenhNhan.DanTocID ?? 1;
                    phieu.BenhNhan.QuocTichID = phieu.BenhNhan.QuocTichID ?? 1;
                    phieu.BenhNhan.PhuongPhapSinh = phieu.BenhNhan.PhuongPhapSinh ?? 1;
                    DotchiDinh.MaNVChiDinh =emp.EmployeeCode;
                    DotchiDinh.MaDonVi = phieu.maDonViCoSo;
                    DotchiDinh.MaGoiDichVu = phieu.maGoiXetNghiem;
                    DotchiDinh.Phieu = phieu;
                    DotchiDinh.MaTiepNhan = tiepnhan.MaTiepNhan;
                    DotchiDinh.MaPhieu = phieu.maPhieu;
                    DotchiDinh.MaPhieuLan1 = phieu.maPhieuLan1;
                    DotchiDinh.NgayChiDinhHienTai = DateTime.Now;
                    DotchiDinh.NgayChiDinhLamViec = DateTime.Now;
                    DotchiDinh.NgayTiepNhan = tiepnhan.NgayTiepNhan ?? DateTime.Now;
                    DotchiDinh.SoLuong = 1;
                    var list = BioNet_Bus.GetDanhsachGoiDichVuTrungTam(MaDonVi);
                    var dg = list.FirstOrDefault(x => x.IDGoiDichVuChung == phieu.maGoiXetNghiem).DonGia;
                    DotchiDinh.DonGia = dg.Value;
                    DotchiDinh.lstDichVu = phieu.lstChiDinhDichVu;

                }
                else if (phieu == null && tiepnhan != null)
                {

                    PsPhieu phieuN = new PsPhieu();

                    PSPatient benhNhan = new PSPatient();
                    phieuN.isKhongDat = false;
                    phieuN.lydokhongdat = String.Empty;
                    phieuN.lstLyDoKhongDat = new List<PSChiTietDanhGiaChatLuong>();
                    phieuN.BenhNhan = benhNhan;
                    phieuN.BenhNhan.NgayGioSinh = DateTime.Now;
                    phieuN.maNVTaoPhieu = emp.EmployeeCode;
                    phieuN.maDonViCoSo = MaDonVi;
                    phieuN.DiaChiLayMau = donvi.DiaChiDVCS;
                    phieuN.NoiLayMau = donvi.TenDVCS;
                    phieuN.maGoiXetNghiem = MaGoiXN;
                    phieuN.lstChiDinhDichVu = BioNet_Bus.GetDanhSachDichVuTheoMaGoi(phieuN.maGoiXetNghiem, MaDonVi);
                    phieuN.ngayGioLayMau = DateTime.Now;
                    phieuN.ngayNhanMau = DateTime.Now;
                    phieuN.maPhieu = MaPhieu;
                    phieuN.BenhNhan.CanNang = 0;
                    phieuN.BenhNhan.PhuongPhapSinh = 0;
                    phieuN.BenhNhan.GioiTinh = 0;
                    phieuN.BenhNhan.TuanTuoiKhiSinh = 0;
                    phieuN.idViTriLayMau = 0;
                    phieuN.BenhNhan.QuocTichID = 1;
                    phieuN.maTinhTrangLucLayMau = 0;
                    phieuN.maCheDoDinhDuong = 0;
                    phieuN.BenhNhan.DanTocID = 1;
                    phieuN.paRa = "0000";
                    phieuN.BenhNhan.Para = "0000";
                    phieuN.isNheCan = true;
                    phieuN.isSinhNon = true;
                    phieuN.isTruoc24h = true;
                    phieuN.isGuiMauTre = false;
                    phieuN.maChuongTrinh = "CTXH0001";
                    phieuN.SoDTNhanVienLayMau = this.txtSDTNguoiLayMau.Text;
                    phieuN.TenNhanVienLayMau = this.ChuyenSangChuHoa(this.txtNguoiLayMau.Text);
                    phieuN.ngayTaoPhieu = DateTime.Now;
                    DotchiDinh.MaNVChiDinh = emp.EmployeeCode;
                    DotchiDinh.MaDonVi = phieuN.maDonViCoSo;
                    DotchiDinh.MaGoiDichVu = phieuN.maGoiXetNghiem;
                    DotchiDinh.Phieu = phieuN;
                    DotchiDinh.MaTiepNhan = tiepnhan.MaTiepNhan;
                    DotchiDinh.MaPhieu = phieuN.maPhieu;
                    DotchiDinh.NgayChiDinhHienTai = DateTime.Now;
                    DotchiDinh.NgayChiDinhLamViec = DateTime.Now;
                    DotchiDinh.NgayTiepNhan = tiepnhan.NgayTiepNhan ?? DateTime.Now;
                    DotchiDinh.SoLuong = 1;
                    DotchiDinh.lstDichVu = phieuN.lstChiDinhDichVu;
                    var list = BioNet_Bus.GetDanhsachGoiDichVuTrungTam(MaDonVi);
                    var dg=list.FirstOrDefault(x => x.IDGoiDichVuChung == phieuN.maGoiXetNghiem).DonGia;
                    DotchiDinh.DonGia = dg.Value;
                    
                }
                var result = BioNet_Bus.InsertDotChiDinhDichVu(DotchiDinh);

                if (result.Result)
                {
                    res.Result = true;
                }
                else
                {
                    res.Result = false;
                }

            }
            catch
            {
                res.Result = false;
            }
            return res;
        }

        private void btnCapNhatDanhSachDaDuyet_Click(object sender, EventArgs e)
        {
            LoadDanhSachDaDuyet();
        }

        private void txtGioiTinh_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.radioGroupGoiXN.EditValue != null)
                {
                    if (this.KiemTraCacTruongDuLieuTruocKhiLuu())
                    {
                            this.LuuThongTinPhieu(true);
                    }
                }
                else
                {
                    DiaglogFrm.FrmNotData notData = new DiaglogFrm.FrmNotData("Yêu cầu chọn gói xét nghiệm");
                }
            }
            catch
            {

            }
        }
    }
}
