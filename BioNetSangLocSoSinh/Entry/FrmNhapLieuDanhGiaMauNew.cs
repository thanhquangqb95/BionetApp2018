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
using DevExpress.XtraGrid.Views.Grid;
using System.Text.RegularExpressions;

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
        // public List<PSTiepNhan> lstTiepNhan = new List<PSTiepNhan>();
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

        #region Load thông tin
        private void FrmNhapLieuDanhGiaMauNew_Load(object sender, EventArgs e)
        {
            this.txtDenNgay.EditValue = DateTime.Now;
            this.txtTuNgay.EditValue = DateTime.Now;
            this.LoadDataDanhMuc();
            this.LoadGoiDichVuXetNGhiem();
            this.LoadDanhSachDichVu();
            this.LoadListDanhGiaSoBo();
            this.lstCLMau = BioNet_Bus.GetDanhMucDanhGiaChatLuong();
            this.LoadDanhSachTiepNhan();
            this.LoadDanhSachDaDuyet();
            this.LoadNew();
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

        private void LoadDanhSachTiepNhan()
        {
            this.GCDanhSachTiepNhan.DataSource = null;
            this.GCDanhSachTiepNhan.DataSource = BioNet_Bus.GetDanhSachPhieuDaTiepNhanNew("all");
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

        private void LoadDanhSachDaDuyet()
        {
            this.lstDaDuyet.Clear();
            this.lstDaDuyet = BioNet_Bus.GetDanhSachPhieuDaDuyet("all");
            this.LoadDanhSachDaDuyet(lstDaDuyet);
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

        private void LoadGCChiDinhDichVu()
        {
            this.GCChiDinhDichVu.DataSource = null;

            if (!radioGroupGoiXN.EditValue.Equals("DVGXN0001"))
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
        #endregion

        #region Filter
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
            try
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
            catch
            {}
        }

        private void txtMaPhieuSearch_KeyPress(object sender, KeyPressEventArgs e) //Filter ds tiếp nhận theo mã phiếu
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                if (!string.IsNullOrEmpty(txtMaPhieuSearch.Text))
                {
                    string filterMaPhieu = "[MaPhieu]='" + txtMaPhieuSearch.Text + "'";
                    this.GVDanhSachTiepNhan.Columns["MaPhieu"].FilterInfo = new ColumnFilterInfo(filterMaPhieu);
                    this.GVDanhSachTiepNhan.Columns["MaDonVi"].Group();
                    this.GVDanhSachTiepNhan.ExpandAllGroups();
                }
            }
        }
        #endregion

        #region Hiện thị phiếu
        private void txtMaPhieuHienThi_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Enter))
                {
                    var phieu = BioNet_Bus.GetThongTinTiepNhanTheoMaPhieu(txtMaPhieuHienThi.Text);
                    if (phieu != null)
                    {
                        HienThiThongTinPhieu(phieu.MaPhieu, phieu.MaDonVi);
                    }
                    else
                    {
                        this.LoadNew();
                        this.ReadOnly(true);
                        DiaglogFrm.FrmWarning notData = new DiaglogFrm.FrmWarning("Mã phiếu không tồn tại.");
                        notData.ShowDialog();
                    }
                    txtMaPhieuHienThi.ResetText();
                    txtMaPhieuHienThi.Focus();
                }
            }
            catch
            { }
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
            catch (Exception Ẽ) { }
        }

        private void HienThiThongTinPhieu(string maPhieu, string maDonVi)
        {
            this.LoadNew();
            this.ReadOnly(true);
            PSTTPhieu data = BioNet_Bus.GetTTPhieu(maPhieu, maDonVi);
            this.LoadGoiXetNghiem(maDonVi);
            this.LoadDanhSachDichVu();
            this.txtMaPhieu.Text = maPhieu;
            this.cbbDonViChon.EditValue = maDonVi;

            //Thông tin tiếp nhận
            if (data.TiepNhan != null)
            {
                this.txtMaTiepNhan.Text = data.TiepNhan.MaTiepNhan;
                this.txtNVTiepNhan.Text = BioNet_Bus.GetThongTinNhanVien(data.TiepNhan.MaNVTiepNhan).EmployeeName;
                this.txtNgayTiepNhan.EditValue = data.TiepNhan.NgayTiepNhan;
            }
            //thông tin chỉ định
            if (data.ChiDinh != null)
            {
                this.txtMaChiDinh.Text = data.ChiDinh.MaChiDinh;
                this.txtNVChiDinh.Text = BioNet_Bus.GetThongTinNhanVien(data.ChiDinh.MaNVChiDinh).EmployeeName;
                this.txtNgayChiDinh.EditValue = data.ChiDinh.NgayChiDinhLamViec;
                this.txtGiaGoiXN.EditValue = data.ChiDinh.DonGia;
            }

            if (data.Phieu != null)
            {
                this.txtTrangThaiMau.EditValue = data.Phieu.TrangThaiMau != null ? data.Phieu.TrangThaiMau.ToString() : "1";
                if (!string.IsNullOrEmpty(data.Phieu.IDPhieuLan1))
                {
                    this.txtMaPhieu1.Text = data.Phieu.IDPhieuLan1;
                }
                //Thông tin đơn vị
                this.txtGhiChu.Text = data.Phieu.LuuYPhieu;
                this.txtNgayTruyenMau.EditValue = data.Phieu.NgayTruyenMau;
                this.txtSoLuongTruyenMau.Text = data.Phieu.SLTruyenMau.ToString();
                this.cbbTTTre.EditValue = data.Phieu.TinhTrangLucLayMau.ToString();
                this.cbbViTriLayMau.EditValue = data.Phieu.IDViTriLayMau.ToString();
                this.cbbChuongTrinh.EditValue = data.Phieu.IDChuongTrinh;
                this.radioGroupGoiXN.EditValue = data.Phieu.MaGoiXN;
                this.txtNgayLayMau.EditValue = data.Phieu.NgayGioLayMau;
                this.txtNguoiLayMau.Text = String.IsNullOrEmpty(data.Phieu.TenNhanVienLayMau) ? this.txtNguoiLayMau.Text : data.Phieu.TenNhanVienLayMau;
                if (string.IsNullOrEmpty(data.Phieu.DiaChiLayMau))
                {
                    this.txtDiaChiNoiLayMau.Text = lstDonVi.FirstOrDefault(x => x.MaDVCS.Equals(maDonVi)).DiaChiDVCS.ToString();
                }
                else
                {
                    this.txtDiaChiNoiLayMau.Text = data.Phieu.DiaChiLayMau;
                }
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
                    this.txtNamSinhBenhNhan.EditValue = data.Benhnhan.NgayGioSinh;
                    this.txtGioiTinh.EditValue = data.Benhnhan.GioiTinh == null ? "2" : data.Benhnhan.GioiTinh.ToString();
                    this.txtCanNang.Text = data.Benhnhan.CanNang == null ? null : data.Benhnhan.CanNang.ToString();
                    this.txtTuanTuoi.Text = data.Benhnhan.TuanTuoiKhiSinh == null ? null : data.Benhnhan.TuanTuoiKhiSinh.ToString();
                    this.cbbDanToc.EditValue = data.Benhnhan.DanTocID == null ? 1 : data.Benhnhan.DanTocID;
                    this.cboPhuongPhapSinh.SelectedIndex = data.Benhnhan.PhuongPhapSinh ?? 2;
                    this.cbbDanToc.EditValue = data.Benhnhan.DanTocID;
                    this.cbbCheDoDD.EditValue = data.Phieu.CheDoDinhDuong == null ? "0" : data.Phieu.CheDoDinhDuong.ToString();
                    if (string.IsNullOrEmpty(data.Benhnhan.NoiSinh))
                    {
                        this.txtNoiSinh.Text = lstDonVi.FirstOrDefault(x => x.MaDVCS.Equals(maDonVi)).TenDVCS.ToString();
                    }
                    else
                    {
                        this.txtNoiSinh.Text = data.Benhnhan.NoiSinh;
                    }
                }
            }
            else
            {
                this.txtTrangThaiMau.EditValue = "1";
                this.txtDiaChiNoiLayMau.Text = lstDonVi.FirstOrDefault(x => x.MaDVCS.Equals(maDonVi)).DiaChiDVCS.ToString();
                this.txtNoiLayMau.Text = cbbDonViChon.Text.ToString();

            }
            if (int.Parse(this.txtTrangThaiMau.EditValue.ToString()) > 1)
            {
                //Phiếu đã duyệt
                this.btnDuyet.Visible = false;
                this.btnLuu.Visible = true;
                this.cbbDonViChon.Enabled = false;
                this.radioGroupGoiXN.Enabled = false;
            }
            else
            {
                //phiếu chưa duyệt
                this.btnDuyet.Visible = true;
                this.btnLuu.Visible = false;
                this.cbbDonViChon.Enabled = true;
                this.radioGroupGoiXN.Enabled = true;
            }
            KiemTraIsDaNhapLieu();
            ValidateNheCan();
            ValidateSinhNon();
            ValidatePara();
            ValidateGuiMauTre();
            ValidateThuMauSom();
        }

        #endregion

        #region Edit Nội dung
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

        private void btnCapNhatDSTiepNhan_Click(object sender, EventArgs e)
        {
            LoadDanhSachTiepNhan();
        }

        private void LoadNew()
        {
            this.lstDanhGiaSoBo=new List<PsDanhGiaMauSoBo>();
            this.btnHuy.Visible = false;
            this.btnLuu.Visible = false;
            this.btnDuyet.Visible = false;
            //Thông tin phiếu
            this.txtMaPhieuHienThi.ResetText();
            this.txtMaPhieu.ResetText();
            this.cbbDonViChon.EditValue = null;
            this.cbbChuongTrinh.EditValue = null;
            this.txtNgayTiepNhan.EditValue = null;
            this.txtMaTiepNhan.ResetText();
            this.txtNVTiepNhan.ResetText();
            this.txtNgayChiDinh.EditValue = null;
            this.txtMaChiDinh.ResetText();
            this.txtNVChiDinh.ResetText();
            this.txtMaKhachHang.ResetText();
            this.txtMaBenhNhan.ResetText();
            //Thông tin gia đình
            this.txtTenMe.ResetText();
            this.txtNamSinhMe.EditValue = null;
            this.txtSDTMe.ResetText();
            this.txtTenCha.ResetText();
            this.txtNamSinhCha.EditValue = null;
            this.txtSDTCha.ResetText();
            this.txtDiaChiGiaDinh.ResetText();
            this.txtPara.EditValue = null;
            this.cboPhuongPhapSinh.EditValue = "0";
            //Thông tin trẻ
            this.txtTenBenhNhan.ResetText();
            this.txtGioiTinh.EditValue = "2";
            this.txtNamSinhBenhNhan.EditValue = null;
            this.txtTuanTuoi.EditValue = null;
            this.txtCanNang.EditValue = null;
            this.cbbDanToc.EditValue = 1;
            this.txtNoiSinh.ResetText();
            this.cbbCheDoDD.EditValue = "1";
            this.cbbTTTre.EditValue = "0";
            this.txtSoLuongTruyenMau.ResetText();
            this.txtNgayTruyenMau.EditValue = null;
            // Thông tin gói
            this.txtNoiLayMau.ResetText();
            this.txtDiaChiNoiLayMau.ResetText();
            this.txtNgayLayMau.EditValue = null;
            this.cbbViTriLayMau.EditValue = "0";
            this.txtMaPhieu1.ResetText();
            this.radioGroupGoiXN.EditValue = null;
            this.radioDanhGia.SelectedIndex = 0;
            this.txtLuuY.ResetText();
            this.txtGhiChu.ResetText();
        }

        private void ReadOnly(bool isreadonly)
        {
            //Thông tin phiếu
            this.txtMaPhieuHienThi.Enabled = isreadonly;
            this.cbbDonViChon.Enabled = isreadonly;
            this.cbbChuongTrinh.Enabled = isreadonly;
            //Thông tin gia đình
            this.txtTenMe.Enabled = isreadonly;
            this.txtNamSinhMe.Enabled = isreadonly;
            this.txtSDTMe.Enabled = isreadonly;
            this.txtTenCha.Enabled = isreadonly;
            this.txtNamSinhCha.Enabled = isreadonly;
            this.txtSDTCha.Enabled = isreadonly;
            this.txtDiaChiGiaDinh.Enabled = isreadonly;
            this.txtPara.Enabled = isreadonly;
            this.cboPhuongPhapSinh.Enabled = isreadonly;
            //Thông tin trẻ
            this.txtTenBenhNhan.Enabled = isreadonly;
            this.txtGioiTinh.Enabled = isreadonly;
            this.txtNamSinhBenhNhan.Enabled = isreadonly;
            this.txtTuanTuoi.Enabled = isreadonly;
            this.txtCanNang.Enabled = isreadonly;
            this.cbbDanToc.Enabled = isreadonly;
            this.txtNoiSinh.Enabled = isreadonly;
            this.cbbCheDoDD.Enabled = isreadonly;
            this.cbbTTTre.Enabled = isreadonly;
            this.txtSoLuongTruyenMau.Enabled = isreadonly;
            this.txtNgayTruyenMau.Enabled = isreadonly;
            // Thông tin gói
            this.txtNoiLayMau.Enabled = isreadonly;
            this.txtDiaChiNoiLayMau.Enabled = isreadonly;
            this.txtNguoiLayMau.Enabled = isreadonly;
            this.txtSDTNguoiLayMau.Enabled = isreadonly;
            this.txtNgayLayMau.Enabled = isreadonly;
            this.cbbViTriLayMau.Enabled = isreadonly;
            
            this.radioDanhGia.Enabled = isreadonly;
            this.txtLuuY.Enabled = isreadonly;
            this.txtGhiChu.Enabled = isreadonly;
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
                if(ts==null)
                {
                    this.txtMaPhieu1.Enabled = false;
                    this.txtMaPhieu1.ResetText();
                }
                else
                {
                    if (ts.Equals("DVGXN0001"))
                    {
                        if (int.Parse(txtTrangThaiMau.EditValue.ToString()) < 2)
                        {
                            this.checkedListBoxXN.Enabled = true;
                            this.txtMaPhieu1.Enabled = true;
                            this.checkedListBoxXN.Enabled = false;
                            this.checkedListBoxXN.DataSource = null;
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
                        this.txtMaPhieu1.Enabled = false;
                        this.txtMaPhieu1.ResetText();
                        this.checkedListBoxXN.Enabled = false;
                        this.checkedListBoxXN.DataSource = null;
                        this.lstChiDinhDichVu = BioNet_Bus.GetDanhSachDichVuTheoMaGoi(radioGroupGoiXN.EditValue.ToString(), cbbDonViChon.EditValue.ToString());
                        var list = BioNet_Bus.GetDanhsachGoiDichVuTrungTam(cbbDonViChon.EditValue.ToString());
                        this.txtGiaGoiXN.EditValue = list.FirstOrDefault(x => x.IDGoiDichVuChung == radioGroupGoiXN.Text).DonGia;
                        this.LoadDanhSachDichVu();
                        this.LoadGCChiDinhDichVu();
                    }
                }
                
            }
            catch
            {
                this.checkedListBoxXN.DataSource = null;
                this.txtMaPhieu1.Enabled = false;
                this.LoadDanhSachDichVu();
                
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
        #endregion

        #region Duyệt phiếu
        private void btnDuyet_Click(object sender, EventArgs e)
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
            if (string.IsNullOrEmpty(this.txtTenBenhNhan.Text.Trim()))
            {
                this.txtTenBenhNhan.BackColor = Color.PapayaWhip;
                kq = false;
            }
            else
            {
                this.txtTenBenhNhan.BackColor = Color.White;
            }

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
            if (!string.IsNullOrEmpty(this.txtTenCha.Text))
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
                        this.txtNamSinhCha.BackColor = Color.White;
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
                    if (((DateTime)this.txtNgayLayMau.EditValue) > ((DateTime)this.txtNgayTiepNhan.EditValue))
                    {
                        XtraMessageBox.Show("Ngày lấy mẫu không được lớn hơn ngày nhận mẫu!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        kq = false;
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
                    TimeSpan time = DateTime.ParseExact(this.txtNamSinhBenhNhan.Text, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture) - DateTime.ParseExact(this.txtNgayLayMau.Text, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
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

            if (this.txtGioiTinh.EditValue == null)
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
                if (IsNumber(this.txtCanNang.Text))
                {
                    if (int.Parse(this.txtCanNang.Text.ToString()) < 500)
                    {
                        this.txtCanNang.BackColor = Color.PapayaWhip;
                        kq = false;
                    }
                }

            }
            else
            {
                this.txtCanNang.BackColor = Color.PapayaWhip;
            }

            if (!string.IsNullOrEmpty(this.txtTuanTuoi.Text))
            {
                if (IsNumber(this.txtTuanTuoi.Text))
                {
                    if (int.Parse(this.txtTuanTuoi.Text.ToString()) < 21 && int.Parse(this.txtTuanTuoi.Text) >= 0)
                    {
                        this.txtTuanTuoi.BackColor = Color.PapayaWhip;
                        kq = false;
                    }
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

        #region Lưu phiếu
        private void LuuThongTinPhieu(bool isDuyet)
        {
            try
            {

                PSTTPhieu phieu = new PSTTPhieu();
                string lydokhongdat = string.Empty;
                #region TT bệnh nhân
                PSPatient benhNhan = new PSPatient();
                benhNhan.TenBenhNhan = this.ChuyenSangChuHoa(this.txtTenBenhNhan.Text.Trim());
                benhNhan.GioiTinh = string.IsNullOrEmpty(this.txtGioiTinh.EditValue.ToString()) != true ? int.Parse(this.txtGioiTinh.EditValue.ToString()) : 2;
                if (IsNumber(this.txtCanNang.Text))
                {
                    benhNhan.CanNang = int.Parse(this.txtCanNang.EditValue.ToString());
                }
                else
                {
                    benhNhan.CanNang = null;
                }
                if (IsNumber(this.txtTuanTuoi.Text))
                {
                    benhNhan.TuanTuoiKhiSinh = int.Parse(this.txtTuanTuoi.EditValue.ToString());
                }
                else
                {
                    benhNhan.TuanTuoiKhiSinh = null;
                }
                benhNhan.DanTocID = this.cbbDanToc.EditValue == null ? 1 : int.Parse(this.cbbDanToc.EditValue.ToString());
                string NgaySinhBN = string.IsNullOrEmpty(txtNamSinhBenhNhan.Text) ? DateTime.Now.Date.ToString() : txtNamSinhBenhNhan.Text;
                benhNhan.NgayGioSinh = DateTime.ParseExact(NgaySinhBN, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                benhNhan.NoiSinh = this.txtNoiSinh.Text;
                benhNhan.PhuongPhapSinh = this.cboPhuongPhapSinh.EditValue == null ? 2 : this.cboPhuongPhapSinh.SelectedIndex;
                benhNhan.QuocTichID = 1;
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
                benhNhan.MaKhachHang = this.txtMaKhachHang.Text.Trim();
                phieu.Benhnhan = benhNhan;
                phieu.MaBenhNhan = benhNhan.MaBenhNhan;
                phieu.MaNV = emp.EmployeeCode;
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
                phieuSangLoc.IDChuongTrinh = string.IsNullOrEmpty(this.cbbChuongTrinh.EditValue.ToString()) == true ? string.Empty : this.cbbChuongTrinh.EditValue.ToString();
                phieuSangLoc.MaGoiXN = this.radioGroupGoiXN.EditValue.ToString();
                phieuSangLoc.IDPhieuLan1 = this.txtMaPhieu1.Text;
                phieuSangLoc.TinhTrangLucLayMau = byte.Parse(this.cbbTTTre.EditValue.ToString());
                string NgayLayMau = string.IsNullOrEmpty(txtNgayLayMau.Text) ? DateTime.Now.Date.ToString() : txtNgayLayMau.Text;
                phieuSangLoc.NgayGioLayMau = DateTime.ParseExact(NgayLayMau, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
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
                phieuSangLoc.TenNhanVienLayMau = this.ChuyenSangChuHoa(this.txtNguoiLayMau.Text.Trim());
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
                                if (string.IsNullOrEmpty(lydokhongdat))
                                    lydokhongdat += res.ChatLuongMau;
                                else lydokhongdat += ". " + res.ChatLuongMau;
                            }
                        }
                    }
                }
                phieuSangLoc.LyDoKhongDat = lydokhongdat;
                phieu.Phieu = phieuSangLoc;
                phieu.MaPhieu = phieuSangLoc.IDPhieu;
                phieu.MaPhieu1 = phieuSangLoc.IDPhieuLan1;
                phieu.MaGoiXN = phieuSangLoc.MaGoiXN;
                phieu.MaDonVi = phieuSangLoc.IDCoSo;
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
                phieu.MaChiDinh = DotchiDinh.MaChiDinh;
                phieu.DonGiaGoi = DotchiDinh.DonGia;
                #endregion
                #region Tiếp nhận
                PSTiepNhan tiepNhan = new PSTiepNhan();
                tiepNhan.MaTiepNhan = this.txtMaTiepNhan.Text;
                tiepNhan.MaDonVi = this.cbbDonViChon.EditValue.ToString();
                tiepNhan.MaPhieu = this.txtMaPhieu.Text;
                phieu.TiepNhan = tiepNhan;
                phieu.MaTiepNhan = tiepNhan.MaTiepNhan;
                #endregion
                phieu.ChiDinh = DotchiDinh;
                SplashScreenManager.ShowForm(this, typeof(DiaglogFrm.WaitingfromSave), true, true, false);
                var result = BioNet_Bus.DuyetPhieuThuCong(phieu);
                SplashScreenManager.CloseForm();
                if (result.Result)
                {
                    BioNet_Bus.UpdateThongTinPhieuLan1(this.txtMaPhieu1.Text.Trim().ToString());
                    XtraMessageBox.Show("Đã lưu!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                    this.LoadNew();
                    this.ReadOnly(false);
                    this.LoadDanhSachTiepNhan();
                    this.LoadDanhSachDaDuyet();
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
                PsDanhGiaMauSoBo tt = this.lstDanhGiaSoBo.FirstOrDefault(p => p.maGiaTri == "isSinhNon");
                if (txtTuanTuoi.EditValue == null)
                {
                    if (tt != null)
                    {
                        tt.giaTri = true;
                        tt.noiDungChuThich = "Tuần tuổi thai không xác định";
                    }
                    else
                    {
                        tt = new PsDanhGiaMauSoBo();
                        tt.giaTri = true;
                        tt.maGiaTri = "isSinhNon";
                        tt.noiDungChuThich = "Tuần tuổi thai không xác định";
                        this.lstDanhGiaSoBo.Add(tt);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.txtTuanTuoi.Text.Trim()))
                    {
                        bool giatri = false;
                        int tuan = int.Parse(this.txtTuanTuoi.EditValue.ToString());
                        if (tuan <= this.GiaTriSinhNon)
                        {
                            giatri = true;
                        }
                        else
                        {
                            giatri = false;
                        }
                        
                        if (tt != null)
                        {
                            tt.giaTri = giatri;
                        }
                        else
                        {
                            tt = new PsDanhGiaMauSoBo();
                            tt.giaTri = giatri;
                            tt.maGiaTri = "isSinhNon";
                            tt.noiDungChuThich = "Sinh non";
                            this.lstDanhGiaSoBo.Add(tt);
                        }

                    }
                }
            
                this.LoadThongTinLuuY();
            }
            catch { }
        }
        private void ValidatePara()
        {
            try
            {
                PsDanhGiaMauSoBo tt = this.lstDanhGiaSoBo.FirstOrDefault(p => p.maGiaTri == "isPara");
                if (txtPara.EditValue == null)
                {
                    if (tt != null)
                    {
                        tt.giaTri = true;
                    }
                    else
                    {
                        tt = new PsDanhGiaMauSoBo();
                        tt.giaTri = true;
                        tt.maGiaTri = "isPara";
                        tt.noiDungChuThich = "Para không xác định";
                        this.lstDanhGiaSoBo.Add(tt);
                    }
                }
                else
                {
                    if (tt != null)
                    {
                        tt.giaTri = false;
                    }
                    else
                    {
                        tt = new PsDanhGiaMauSoBo();
                        tt.giaTri = false;
                        tt.maGiaTri = "isPara";
                        tt.noiDungChuThich = "Para không xác định";
                        this.lstDanhGiaSoBo.Add(tt);
                    }
                }
                this.LoadThongTinLuuY();
            }
            catch { }
        }
        private void ValidateNheCan()
        {
            try
            {
                var tt = this.lstDanhGiaSoBo.FirstOrDefault(p => p.maGiaTri == "isNheCan");
                if (txtCanNang.EditValue==null)
                {
                    if (tt != null)
                    {
                        tt.giaTri = true;
                        tt.noiDungChuThich = "Cân nặng không xác định";
                    }
                    else
                    {
                        tt = new PsDanhGiaMauSoBo();
                        tt.giaTri = true;
                        tt.maGiaTri = "isNheCan";
                        tt.noiDungChuThich = "Cân nặng không xác định";
                        this.lstDanhGiaSoBo.Add(tt);
                    }
                }
                else
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
                        
                        if (tt != null)
                        {
                            tt.giaTri = giatri;
                        }
                        else
                        {
                            tt = new PsDanhGiaMauSoBo();
                            tt.giaTri = giatri;
                            tt.maGiaTri = "isNheCan";
                            tt.noiDungChuThich = "Nhẹ cân";
                            this.lstDanhGiaSoBo.Add(tt);
                        }
                    }
                }
                this.LoadThongTinLuuY();

            }
            catch { }
        }
        private void ValidateGuiMauTre()
        {
            try
            {
                var dg = this.lstDanhGiaSoBo.FirstOrDefault(p => p.maGiaTri == "isGuiMauTre");
                TimeSpan time;
                if (!string.IsNullOrEmpty(txtNgayTiepNhan.Text) && !string.IsNullOrEmpty(txtNgayLayMau.Text))
                {

                    time = DateTime.Parse(txtNgayTiepNhan.Text) - DateTime.Parse(txtNgayLayMau.Text);
                }
                else if (!string.IsNullOrEmpty(this.txtNgayLayMau.Text))
                {
                    DateTime ngayGioLayMau = DateTime.ParseExact(this.txtNgayLayMau.Text, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
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
        private void ValidateThuMauSom()
        {
            try
            {
                var dg = this.lstDanhGiaSoBo.FirstOrDefault(p => p.maGiaTri == "islaymautruoc24h");
                TimeSpan time;
                if (!string.IsNullOrEmpty(this.txtNgayLayMau.Text) && !string.IsNullOrEmpty(this.txtNamSinhBenhNhan.Text))
                {
                    time = DateTime.Parse(this.txtNgayLayMau.Text) - DateTime.Parse(this.txtNamSinhBenhNhan.Text);
                }
                else if (!string.IsNullOrEmpty(this.txtNgayLayMau.Text) && !string.IsNullOrEmpty(this.txtNamSinhBenhNhan.Text))
                {
                    DateTime ngayGioLayMau = DateTime.ParseExact(this.txtNgayLayMau.Text, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime ngaySinh = DateTime.ParseExact(this.txtNamSinhBenhNhan.Text, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
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
            if (string.IsNullOrEmpty(txtTenBenhNhan.Text))
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

        #region Kiểm lỗi
        private void cbbDonViChon_EditValueChanged(object sender, EventArgs e)
        {
         
        }

        #endregion
        private string ChuyenSangChuHoa(string chuoi)
        {
            if (!string.IsNullOrEmpty(chuoi))
                return chuoi.ToUpper();
            else return string.Empty;
        }

        private void txtCanNang_Validated(object sender, EventArgs e)
        {
            if ( txtCanNang.EditValue == null)
            {
                txtCanNang.BackColor = Color.PapayaWhip;
                
            }
            else
            {
                if(string.IsNullOrEmpty(txtCanNang.EditValue.ToString()) || txtCanNang.EditValue.ToString() == "0")
                {
                    txtCanNang.BackColor = Color.PapayaWhip;
                    txtCanNang.EditValue = null;
                }
                else
                {
                    txtCanNang.BackColor = Color.White;
                }

            }
            ValidateNheCan();
        }

        private void txtTuanTuoi_Validated(object sender, EventArgs e)
        {
            if (txtTuanTuoi.EditValue == null)
            {
                txtTuanTuoi.BackColor = Color.PapayaWhip;

            }
            else
            {
                if (string.IsNullOrEmpty(txtTuanTuoi.EditValue.ToString()) ||txtTuanTuoi.EditValue.ToString()=="0")
                {
                    txtTuanTuoi.BackColor = Color.PapayaWhip;
                    txtTuanTuoi.EditValue = null;
                }
                else
                {
                    txtTuanTuoi.BackColor = Color.White;
                }

            }
            ValidateSinhNon();
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

        private void HienThiTTPhieuLan1(PSTTPhieu ph)
        {
            if (string.IsNullOrEmpty(txtTenBenhNhan.Text))
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
            if (string.IsNullOrEmpty(txtNamSinhBenhNhan.EditValue.ToString()))
            {
                txtNamSinhBenhNhan.EditValue = ph.Benhnhan.NgayGioSinh.Value;
            }
            if (string.IsNullOrEmpty(txtTenMe.Text))
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
            cbbDanToc.EditValue = cbbDanToc.EditValue.ToString() != ph.Benhnhan.DanTocID.ToString() ? ph.Benhnhan.DanTocID : cbbDanToc.EditValue;
            cboPhuongPhapSinh.EditValue = cboPhuongPhapSinh.EditValue.ToString() != ph.Benhnhan.PhuongPhapSinh.ToString() ? ph.Benhnhan.PhuongPhapSinh : cboPhuongPhapSinh.EditValue;
            cbbCheDoDD.EditValue = cbbCheDoDD.EditValue.ToString() != ph.Phieu.CheDoDinhDuong.ToString() ? ph.Phieu.CheDoDinhDuong : cbbCheDoDD.EditValue;
            cbbTTTre.EditValue = cbbTTTre.EditValue.ToString() != ph.Phieu.TinhTrangLucLayMau.ToString() ? ph.Phieu.TinhTrangLucLayMau : cbbTTTre.EditValue;
            txtPara.Text = ph.Phieu.Para;
            txtNoiLayMau.Text = ph.Phieu.NoiLayMau;
            txtCanNang.EditValue = ph.Benhnhan.CanNang;
            txtTuanTuoi.EditValue = ph.Benhnhan.TuanTuoiKhiSinh;
            txtNoiSinh.Text = ph.Benhnhan.NoiSinh;
            txtDiaChiNoiLayMau.Text = ph.Phieu.DiaChiLayMau;
            txtMaBenhNhan.Text = ph.Phieu.MaBenhNhan;
            btnChiTietKQ1.Visible = true;


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
                        if (phieu.Phieu.TrangThaiMau > 5)
                        {
                            this.listdvcanlamlai.Clear();
                            this.listdvcanlamlai = BioNet_Bus.GetDichVuCanLamLaiCuaPhieu(phieu.Phieu.IDPhieu, phieu.Phieu.IDCoSo);
                            if (listdvcanlamlai.Count < 0)
                            {
                                XtraMessageBox.Show("Phiếu " + txtMaPhieu1.Text + "được chỉ định thu lại \r\n nhưng không tìm thấy danh sách thông số nguy cơ cao cần xét nghiệm lại. \r\n Vui lòng kiểm tra lại!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
                            }
                            if (int.Parse(txtTrangThaiMau.EditValue.ToString()) < 2)
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
                            var result = this.DuyetPhieu(item.MaDonVi, item.MaPhieu, maGoiXN);
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
        private PsReponse DuyetPhieu(String MaDonVi, String MaPhieu, String MaGoiXN)
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
                    phieu.maGoiXetNghiem = string.IsNullOrEmpty(phieu.maGoiXetNghiem) != true ? phieu.maGoiXetNghiem : MaGoiXN;
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
                    DotchiDinh.MaNVChiDinh = emp.EmployeeCode;
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
                    var dg = list.FirstOrDefault(x => x.IDGoiDichVuChung == phieuN.maGoiXetNghiem).DonGia;
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
     
        private void txtMaPhieu1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                this.LoadTTPhieuLan1();
            }
        }

        private void GVDanhSachDaTracking_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {
                GridView View = sender as GridView;
                if (e.RowHandle >= 0)
                {
                    bool isDaNhapLieu = View.GetRowCellDisplayText(e.RowHandle, col_isDaNhapLieu_GCTracking) == null ? false : (bool)View.GetRowCellValue(e.RowHandle, this.col_isDaNhapLieu_GCTracking);


                    if (!isDaNhapLieu)
                    {
                        e.Appearance.BackColor = Color.Chocolate;
                        e.Appearance.BackColor2 = Color.BurlyWood;
                    }
                    else
                    {
                        e.Appearance.BackColor = Color.Aqua;
                        e.Appearance.BackColor2 = Color.AliceBlue;
                    }

                }
            }
            catch { }
        }
        #region tab

        private void txtTenMe_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void txtNamSinhMe_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void txtNamSinhMe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter || e.KeyData == Keys.Tab)
            {
                this.txtTenCha.Focus();
            }
        }
        private void txtTenCha_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter || e.KeyData == Keys.Tab)
            {
                txtTenCha.Text = txtTenCha.Text.ToUpper();
                this.txtNamSinhCha.Focus();
            }
        }

        public static string VietHoaChuCaiDau(string s)
        {
            if (String.IsNullOrEmpty(s))
                return s;
            string result = "";
            string[] words = s.Split(' ');
            foreach (string word in words)
            {
                if (word.Trim() != "")
                {
                    if (word.Length > 1)
                        result += word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower() + " ";
                    else
                        result += word.ToUpper() + " ";
                }
            }
            return result.Trim();
        }





        #endregion

     

        private void GVDanhSachTiepNhan_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {

            try
            {
                GridView View = sender as GridView;
                if (e.RowHandle >= 0)
                {
                    bool isDaNhapLieu = View.GetRowCellDisplayText(e.RowHandle, View.Columns["isDaNhapLieu"]) == null ? false : (bool)View.GetRowCellValue(e.RowHandle, this.col_isDaNhapLieu);
                    if (!isDaNhapLieu)
                    {
                        e.Appearance.BackColor = Color.Salmon;
                        e.Appearance.BackColor2 = Color.SeaShell;
                    }
                    else
                    {
                        e.Appearance.BackColor = Color.Aqua;
                        e.Appearance.BackColor2 = Color.AliceBlue;
                    }
                }
            }
            catch
            {

            }
        }
        private bool IsNumber(string pText)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(pText);
        }

        private void checkedListBoxLydoKhongDat_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                CheckedListBoxControl obj = sender as CheckedListBoxControl;
                string check = obj.GetItemCheckState(e.Index).ToString();
                if (check.Equals("Checked"))
                {
                    string value = obj.GetItemValue(e.Index).ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        var psCTDanhGia = sourceListDanhGiaChatLuong.FirstOrDefault(x => x.IDDanhGiaChatLuongMau.Equals(value));
                        PSChiTietDanhGiaChatLuong dg = new PSChiTietDanhGiaChatLuong();
                        dg.IDPhieu = this.txtMaPhieu.Text;
                        dg.IDDanhGiaChatLuongMau = value;
                        dg.MaTiepNhan = this.txtMaTiepNhan.Text.Trim();
                        dg.TenLyDo = psCTDanhGia.ChatLuongMau;
                        dg.Stt = psCTDanhGia.STT;
                        dg.RowIDChatLuongMau = psCTDanhGia.RowIDChatLuongMau;
                        this.ThayDoiThongTinChiTietKhongDat(dg, true);
                    }
                }
                else
                {
                    string value = obj.GetItemValue(e.Index).ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        var psCTDanhGia = sourceListDanhGiaChatLuong.FirstOrDefault(x => x.IDDanhGiaChatLuongMau.Equals(value));
                        PSChiTietDanhGiaChatLuong dg = new PSChiTietDanhGiaChatLuong();
                        dg.IDPhieu = this.txtMaPhieu.Text;
                        dg.IDDanhGiaChatLuongMau = value;
                        dg.MaTiepNhan = this.txtMaTiepNhan.Text.Trim();
                        dg.TenLyDo = psCTDanhGia.ChatLuongMau;
                        dg.Stt = psCTDanhGia.STT;
                        dg.RowIDChatLuongMau = psCTDanhGia.RowIDChatLuongMau;
                        this.ThayDoiThongTinChiTietKhongDat(dg, false);
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        private void ThayDoiThongTinChiTietKhongDat(PSChiTietDanhGiaChatLuong LydoKhongDat, bool isThem)
        {
            if (isThem)
            {
                if (this.lstDanhGiaChatLuong.Count == 0)
                {
                    this.lstDanhGiaChatLuong.Add(LydoKhongDat);
                }
                else
                {
                    if (this.lstDanhGiaChatLuong.FindAll(p => p.IDDanhGiaChatLuongMau == LydoKhongDat.IDDanhGiaChatLuongMau).Count < 1)
                    {
                        this.lstDanhGiaChatLuong.Add(LydoKhongDat);
                    }
                }
            }
            else
            {
                if (this.lstDanhGiaChatLuong.Count > 0)
                {
                    var ld = this.lstDanhGiaChatLuong.FirstOrDefault(p => p.IDDanhGiaChatLuongMau == LydoKhongDat.IDDanhGiaChatLuongMau);
                    if (ld != null)
                    {
                        this.lstDanhGiaChatLuong.Remove(ld);
                    }
                }

            }
        }
        #region Các trường lỗi
        //Tên mẹ
        private void txtTenMe_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenMe.Text))
            {
                txtTenMe.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtTenMe.BackColor = Color.White;
                txtTenMe.Text = txtTenMe.Text.ToUpper();
            }
        }
        //tên cha
        private void txtTenCha_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTenCha.Text))
            {
                txtTenCha.Text = txtTenCha.Text.ToUpper();
            }
        }
        //Dịa chỉ gia đình
        private void txtDiaChiGiaDinh_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtDiaChiGiaDinh.Text.Trim()))
            {
                this.txtDiaChiGiaDinh.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtDiaChiGiaDinh.Text = VietHoaChuCaiDau(txtDiaChiGiaDinh.Text.Trim());
                this.txtDiaChiGiaDinh.BackColor = Color.White;
            }
        }
        //Sdt mẹ
        private void txtSDTMe_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSDTMe.Text))
            {
                txtSDTMe.BackColor = Color.PapayaWhip;
            }
            else
            {
                if (IsNumber(txtSDTMe.Text))
                {
                    txtSDTMe.BackColor = Color.White;
                }
                else
                {
                    txtSDTMe.BackColor = Color.PapayaWhip;
                }  
            }
        }
        private void txtSDTCha_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSDTCha.Text))
            {
                if (IsNumber(txtSDTCha.Text))
                {
                    txtSDTCha.BackColor = Color.White;
                }
                else
                {
                    txtSDTCha.BackColor = Color.PapayaWhip;
                }
            }
        }
        //Ten Bệnh nhân
        private void txtTenBenhNhan_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenBenhNhan.Text))
            {
                txtTenBenhNhan.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtTenBenhNhan.BackColor = Color.White;
                txtTenBenhNhan.Text = txtTenBenhNhan.Text.ToUpper();
            }
        }

        //Noi SInh
        private void txtNoiSinh_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNoiSinh.Text))
            {
                txtNoiSinh.BackColor = Color.PapayaWhip;
            }
            else
            {
                txtNoiSinh.BackColor = Color.White;
                txtNoiSinh.Text = VietHoaChuCaiDau(txtNoiSinh.Text.Trim());
            }
        }

        //Tình trạng trẻ
        private void cbbTTTre_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbbTTTre.EditValue.ToString() == "4")
                {
                    lblNgayTruyen.Visible = true;
                    lblSLTruyenMau.Visible = true;
                    txtSoLuongTruyenMau.Visible = true;
                    txtNgayTruyenMau.Visible = true;
                }
                else
                {
                    lblNgayTruyen.Visible = false;
                    lblSLTruyenMau.Visible = false;
                    txtSoLuongTruyenMau.Visible = false;
                    txtNgayTruyenMau.Visible = false;
                }
            }
            catch
            {
            }
        }

        private void txtSDTMe_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSDTMe.Text))
            {
            }
            else
            {
                if (string.IsNullOrEmpty(txtTenCha.Text) && string.IsNullOrEmpty(txtSDTCha.Text))
                {
                    txtSDTCha.Text = txtSDTMe.Text;
                }
            }
        }
        //Năm sinh mẹ
        private void txtNamSinhMe_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNamSinhMe.Text))
            {
                txtNamSinhMe.BackColor = Color.PapayaWhip;
            }
            else
            {
                if((DateTime.Now.Year-int.Parse(txtNamSinhMe.Text))<100)
                {
                    txtNamSinhMe.BackColor = Color.White;
                }
                else
                {
                    txtNamSinhMe.BackColor= Color.PapayaWhip;
                }
            }
        }

        private void txtNamSinhCha_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNamSinhMe.Text))
            {
            }
            else
            {
                if ((DateTime.Now.Year - int.Parse(txtNamSinhMe.Text)) < 100)
                {
                    txtNamSinhMe.BackColor = Color.White;
                }
                else
                {
                    txtNamSinhMe.BackColor = Color.PapayaWhip;
                }
            }
        }
        //Para
        private void txtPara_EditValueChanged(object sender, EventArgs e)
        {
           if (txtPara.EditValue == null)
            {
                txtPara.BackColor = Color.PapayaWhip;
            }
            else
            {
                if(txtPara.EditValue.ToString().Length!= 4) 
                {
                    if(txtPara.EditValue.ToString().Length == 0)
                    {
                        txtPara.BackColor = Color.White;
                        txtPara.EditValue = null;
                    }
                    else
                    {
                        txtPara.BackColor = Color.PapayaWhip;
                    }
                }
                else
                {
                    txtPara.BackColor = Color.White;
                }
            }
            ValidatePara();
        }

        //txtNgayLayMau
        private void txtNgayLayMau_EditValueChanged(object sender, EventArgs e)
        {
            if (txtNgayLayMau.Text==null || txtNgayLayMau.Text.Trim()== "")
            {
                txtNgayLayMau.BackColor = Color.PapayaWhip;
            }
            else
            {
                try
                {
                    if (DateTime.Parse(txtNgayLayMau.EditValue.ToString()) > DateTime.Parse(txtNgayTiepNhan.EditValue.ToString()))
                    {
                        txtNgayLayMau.BackColor = Color.PapayaWhip;
                    }
                    else
                    {
                        txtNgayLayMau.BackColor = Color.White;
                    }  
                }
                catch{} 
            }
        }

        private void txtNamSinhBenhNhan_Validated(object sender, EventArgs e)
        {
            if (txtNamSinhBenhNhan.Text == null || txtNamSinhBenhNhan.Text.Trim() == "")
            {
                txtNamSinhBenhNhan.BackColor = Color.PapayaWhip;
            }
            else
            {
                try
                {
                    if (DateTime.Parse(txtNamSinhBenhNhan.EditValue.ToString()) > DateTime.Parse(txtNgayTiepNhan.EditValue.ToString()))
                    {
                        txtNamSinhBenhNhan.BackColor = Color.PapayaWhip;
                    }
                    else
                    {
                        txtNamSinhBenhNhan.BackColor = Color.White;
                    }
                }
                catch
                { }
            }
        }

        private void txtSDTNguoiLayMau_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSDTNguoiLayMau.Text))
            {
                if (IsNumber(txtSDTNguoiLayMau.Text))
                {
                    txtSDTNguoiLayMau.BackColor = Color.White;
                }
                else
                {
                    txtSDTNguoiLayMau.BackColor = Color.PapayaWhip;
                }
            }
            else
            {
                txtSDTNguoiLayMau.BackColor = Color.PapayaWhip;
            }
        }

        private void radioGroupGoiXN_Validated(object sender, EventArgs e)
        {
            try
            {
                RadioGroup rd = sender as RadioGroup;
                var ts = rd.EditValue;
                if (ts == null)
                {
                    this.txtMaPhieu1.Enabled = false;
                    this.txtMaPhieu1.ResetText();
                }
                else
                {
                    if (ts.Equals("DVGXN0001"))
                    {
                        if (int.Parse(txtTrangThaiMau.EditValue.ToString()) < 2)
                        {
                            this.checkedListBoxXN.Enabled = true;
                            this.txtMaPhieu1.Enabled = true;
                            this.checkedListBoxXN.Enabled = false;
                            this.checkedListBoxXN.DataSource = null;
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
                        this.txtMaPhieu1.Enabled = false;
                        this.txtMaPhieu1.ResetText();
                        this.checkedListBoxXN.Enabled = false;
                        this.checkedListBoxXN.DataSource = null;
                        this.lstChiDinhDichVu = BioNet_Bus.GetDanhSachDichVuTheoMaGoi(radioGroupGoiXN.EditValue.ToString(), cbbDonViChon.EditValue.ToString());
                        var list = BioNet_Bus.GetDanhsachGoiDichVuTrungTam(cbbDonViChon.EditValue.ToString());
                        this.txtGiaGoiXN.EditValue = list.FirstOrDefault(x => x.IDGoiDichVuChung == radioGroupGoiXN.Text).DonGia;
                        this.LoadDanhSachDichVu();
                        this.LoadGCChiDinhDichVu();
                    }
                }

            }
            catch
            {
                this.checkedListBoxXN.DataSource = null;
                this.txtMaPhieu1.Enabled = false;
                this.LoadDanhSachDichVu();
            }
        }
        private void cbbDonViChon_EditValueChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtMaPhieu.Text))
                {
                    var donvi = BioNet_Bus.GetThongTinDonViCoSo(cbbDonViChon.EditValue.ToString());
                    if (donvi != null)
                    {
                        this.txtNoiSinh.Text = donvi.TenDVCS;
                        this.txtNoiLayMau.Text = donvi.TenDVCS;
                        this.txtDiaChiNoiLayMau.Text = donvi.DiaChiDVCS;
                        if (cbbGoiXNLoc.EditValue != null)
                        {
                            var goi = BioNet_Bus.GetDanhsachGoiDichVuTrungTam(cbbDonViChon.EditValue.ToString());
                            if (goi != null)
                            {
                                this.txtGiaGoiXN.EditValue = goi.FirstOrDefault(x => x.IDGoiDichVuChung.Equals(cbbGoiXNLoc.EditValue)).DonGia;
                            }
                            else
                            {
                                this.txtGiaGoiXN.EditValue = 0;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        #endregion

        private void txtMaTiepNhan_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txtMaBenhNhan_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void panelControl4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
    

