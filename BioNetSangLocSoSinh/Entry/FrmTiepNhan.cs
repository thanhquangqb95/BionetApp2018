﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BioNetModel.Data;
using BioNetModel;
using BioNetBLL;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils.Menu;
using DevExpress.XtraSplashScreen;
using System.Threading;
using DevExpress.XtraLayout;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.BandedGrid;

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmTiepNhan : DevExpress.XtraEditors.XtraForm
    {
        public FrmTiepNhan(string MaNhanVien)
        {
            InitializeComponent();
            this.MaNhanVienDangNhap = MaNhanVien;
        }
        private List<PSDanhMucDonViCoSo> lstDVCS = new List<PSDanhMucDonViCoSo>(); //danh sách đơn vị cơ sở
        private List<PSDanhMucChiCuc> lstChiCuc = new List<PSDanhMucChiCuc>();
        private List<PsPhieu> lstPhieu = new List<PsPhieu>(); //danh sách phiếu hiển thị trên gridview
        private List<PsPhieu> lstPhieuTaiDVCS = new List<PsPhieu>();//danh sách phiếu đã gửi của đơn vị cơ sở - dùng để đối chiếu với lstPhieu
        private List<PSTiepNhan> lstTiepNhan = new List<PSTiepNhan>(); //danh sach phieu được nhân viên tiếp nhận nhưng chưa lưu
        private string maPhieuFocusHandle = string.Empty;
        private string maDonviFocusHandle = string.Empty;
        private List<PsDichVu> lstDichVu = new List<PsDichVu>();
        private List<PSTiepNhan> lstDaTiepNhan = new List<PSTiepNhan>();
        
        private string MaNhanVienDangNhap = "MaNVTiepNhan";
        //Form Load
        private void FrmLoad_From()
        {
            this.lstChiCuc.Clear();
            this.lstDichVu.Clear();
            this.lstDVCS.Clear();
            this.lstPhieu.Clear();
            this.lstPhieuTaiDVCS.Clear();
            this.LoadDanhSachChuongTrinhSangLoc();
            this.LoadLookupDanToc();
            this.LoadsearchLookUpChiCucPhieu();
            this.LoadRepositoryLookupDonViCoSo();
            this.txtTuNgay_ChuaKQ.EditValue = DateTime.Now;
            this.txtDenNgay_ChuaKQ.EditValue = DateTime.Now;
            this.ReadOnly(true);
            this.txtMaPhieuMoi.Focus();
            AddItemForm();
        }

        private void FrmTiepNhan_Load(object sender, EventArgs e)
        {
            FrmLoad_From();                       
        }
    
        
        //Load dữ liệu cơ sở
        private void LoadGCPhieuCho()
        {
            this.GCPhieuCho.DataSource = null;
            if (this.lstPhieu.Count > 0)
            {
                this.GCPhieuCho.DataSource = this.lstPhieu;
                this.GVPhieuCho.Columns["maDonViCoSo"].Group();
                this.GVPhieuCho.ExpandAllGroups();
                this.GVPhieuCho.Columns["maPhieu"].Caption = "Mã phiếu";
                this.GVPhieuCho.OptionsDetail.ShowDetailTabs = false;
            }
        }
        private void LoadDanhSachTiepNhan()
        {
            this.GCDanhSachTiepNhan.DataSource = null;
            if(this.lstTiepNhan.Count>0)
            {
                this.GCDanhSachTiepNhan.DataSource = this.lstTiepNhan;
                this.GVDanhSachTiepNhan.Columns["MaDonVi"].Group();
                this.GVDanhSachTiepNhan.ExpandAllGroups();
            }                   

        }
        private void LoadDanhSachChuongTrinhSangLoc()
        {
            this.lookupChuongTrinh.Properties.DataSource = BioNet_Bus.GetDanhSachChuongTrinh(false);
        }
        private void LoadLookupDanToc()
        {
            this.lookUpDanToc.Properties.DataSource = BioNet_Bus.GetDanhSachDanToc(-1);
        }
             
        private void LoadDanhSachDaTiepNhan()
        {
            this.lstDaTiepNhan.Clear();
            this.lstDaTiepNhan = BioNet_Bus.GetDanhSachPhieuChuaDanhGia("all");
            this.LoadGCDaTiepNhan();
        }
     
        private void LoadRepositoryLookupDonViCoSo()
        {
            try
            {
                this.lstDVCS.Clear();
                this.lstDVCS = BioNet_Bus.GetDanhSachDonViCoSo();
                this.repositoryItemLookUpEdit1.DataSource = this.lstDVCS;
                this.repositoryLookupDonviCoSo.DataSource = this.lstDVCS;
                this.lookupDonVi.Properties.DataSource = this.lstDVCS;
                this.repositoryLookUpDonViGCTiepNhan.DataSource = this.lstDVCS;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi khi lấy danh sách đơn vị cơ sở \r\n Lỗi chi tiết :" + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void LoadNew()
        {
            this.txtCanNang.Reset();
            this.txtGioiTinh.ResetText();
            this.txtGioSinhBenhNhan.ResetText();
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
            this.RadioCheDoDD.EditValue = "0";
            this.radioGroupGoiXN.EditValue = null;
            this.radioGroupTinhTrangTre.EditValue = "0";
            this.radioGroupViTriLayMau.EditValue = "0";
            this.txtCanNang.ResetText();
            this.txtGioiTinh.ResetText();
            this.lookUpDanToc.ResetText();
            this.lookupChuongTrinh.ResetText();
            this.txtGioSinhBenhNhan.ResetText();
            this.txtGioLayMau.ResetText();
            this.txtDiaChiDonVi.ResetText();
            this.txtNguoiLayMau.ResetText();
            this.txtNoiLayMau.ResetText();
            this.txtSDTNguoiLayMau.ResetText();
            this.txtNgayLayMau.ResetText();
            this.txtGioSinhBenhNhan.ResetText();
            this.txtGioLayMau.ResetText();
            this.cboPhuongPhapSinh.ResetText();
            this.checkedListBoxXN.Enabled = false;
        }
        private void ReadOnly(bool isReadOnly)
        {
            this.txtCanNang.ReadOnly = isReadOnly;
            this.txtDiaChiDonVi.ReadOnly = isReadOnly;
            this.txtGioiTinh.ReadOnly = isReadOnly;
            this.txtGioLayMau.ReadOnly = isReadOnly;
            this.txtGioSinhBenhNhan.ReadOnly = isReadOnly;
            this.txtMaPhieuMoi.ReadOnly = !isReadOnly;
            this.txtNamSinhBenhNhan.ReadOnly = isReadOnly;
            this.txtNamSinhCha.ReadOnly = isReadOnly;
            this.txtNamSinhMe.ReadOnly = isReadOnly;
            this.txtNgayLayMau.ReadOnly = isReadOnly;
            this.txtNgayTruyenMau.ReadOnly = isReadOnly;
            this.txtNguoiLayMau.ReadOnly = isReadOnly;
            this.txtNoiLayMau.ReadOnly = isReadOnly;
            this.txtNoiSinh.ReadOnly = isReadOnly;
            this.txtSDTCha.ReadOnly = isReadOnly;
            this.txtSDTMe.ReadOnly = isReadOnly;
            this.txtSDTNguoiLayMau.ReadOnly = isReadOnly;
            this.txtSoLuongTruyenMau.ReadOnly = isReadOnly;
            this.txtTenBenhNhan.ReadOnly = isReadOnly;
            this.txtPara.ReadOnly = isReadOnly;
            this.txtTenCha.ReadOnly = isReadOnly;
            this.txtTenMe.ReadOnly = isReadOnly;
            this.RadioCheDoDD.ReadOnly = isReadOnly;
            this.radioGroupGoiXN.ReadOnly = isReadOnly;
            this.radioGroupTinhTrangTre.ReadOnly = isReadOnly;
            this.radioGroupViTriLayMau.ReadOnly = isReadOnly;
            this.cboPhuongPhapSinh.ReadOnly = isReadOnly;
            this.lookupChuongTrinh.ReadOnly = isReadOnly;
            this.lookUpDanToc.ReadOnly = isReadOnly;
            this.lookupDonVi.ReadOnly = isReadOnly;


        }
   
        private void LoadDanhSachDichVu()
        {
            this.lstDichVu.Clear();
            this.lstDichVu = BioNet_Bus.GetDanhSachDichVu(false);
        }
        private void searchLookUpChiCuc_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit sear = sender as SearchLookUpEdit;
                var value = sear.EditValue.ToString();
                this.searchLookUpDonViCoSoTiepNhan.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi(value.ToString());
                this.searchLookUpDonViCoSoTiepNhan.EditValue = "all";
            }
            catch { }
        }
      
        private void LoadsearchLookUpChiCucPhieu()
        {
            try
            {
                lstChiCuc.Clear();
                this.lstChiCuc= BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
                this.searchLookUpChiCucPhieu.Properties.DataSource = lstChiCuc;
                this.searchLookUpChiCuc.Properties.DataSource = lstChiCuc;
                this.searchLookUpChiCucPhieu.EditValue = "all";
                this.searchLookUpDonViCoSo.EditValue = "all";
                this.searchLookUpChiCuc.EditValue = "all";
                this.searchLookUpDonViCoSoTiepNhan.EditValue = "all";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi khi load danh sách chi cục \r\n Lỗi chi tiết :" + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.searchLookUpChiCuc.Focus();
                this.searchLookUpChiCucPhieu.Focus();
            }
        }
        private void searchLookUpChiCucPhieu_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit sear = sender as SearchLookUpEdit;
                var value = sear.EditValue.ToString();
                this.searchLookUpDonViCoSo.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi(value.ToString());
                this.searchLookUpDonViCoSo.EditValue = "all";
            }
            catch { }
        }
        private void searchLookUpDonViCoSo_TextChanged(object sender, EventArgs e)
        {
            this.txtMaPhieuMoi.ResetText();
            this.txtMaPhieuMoi.Focus();
        }

        private void searchLookUpDonViCoSo_EditValueChanged(object sender, EventArgs e)
        {
            if (this.txtTuNgay_ChuaKQ.EditValue != null && this.txtDenNgay_ChuaKQ.EditValue != null)
            {
                this.LoadDanhSachDaTiepNhan();
                this.LoadDuLieuDanhSachPhieu();
            }
        }

        
        private void LoadListTiepNhan()
        {
            this.GCDanhSachTiepNhan.DataSource = null;
            this.GCDanhSachTiepNhan.DataSource = this.lstTiepNhan;

        }

        //Load dữ liệu phiếu
        private void LoadDuLieuDanhSachPhieu() //lấy dữ liệu gán vào 2 list :  lstPhieu và lstPhieuTaiDVCS
        {
            this.lstPhieu.Clear();
            this.lstPhieuTaiDVCS.Clear();
            this.lstPhieu = BioNet_Bus.GetDanhSachPhieuChoTiepNhan();
            this.lstPhieuTaiDVCS = BioNet_Bus.GetDanhSachPhieuChoTiepNhan();
            this.LoadGCPhieuCho();
        }
        private void LuuDotTiepNhan()
        {
            SplashScreenManager.ShowForm(this, typeof(DiaglogFrm.WaitingfromSave), true, true, false);
            var lst = this.lstTiepNhan.OrderByDescending(p => p.MaDonVi).ToList();
            if (lst.Count > 0)
            {
                bool isOK = true;
                foreach (var tiepnhan in lst)
                {
                    var result = BioNet_Bus.InsertTiepNhan(tiepnhan);
                    if (!result.Result)
                    {
                        XtraMessageBox.Show("Lưu không thành công phiếu " + tiepnhan.MaPhieu + "\r\n Lý do : " + result.StringError, "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        isOK = false;
                    }
                    else
                    {
                        var res = this.lstTiepNhan.FirstOrDefault(p => p.MaPhieu == tiepnhan.MaPhieu);
                        this.lstTiepNhan.Remove(res);
                    }
                }
                
                this.LoadDanhSachTiepNhan();
                this.LoadDanhSachDaTiepNhan();
                SplashScreenManager.CloseForm();
                if (isOK)
                    XtraMessageBox.Show("Đã lưu thành công!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void LoadGCDaTiepNhan()
        {
            this.GCDaTiepNhan.DataSource = null;
            this.GCDaTiepNhan.DataSource = this.lstDaTiepNhan;
            if (this.lstDaTiepNhan.Count > 0)
                this.GVDaTiepNhan.Columns["MaDonVi"].Group();
            //   this.GVDaTiepNhan.ExpandAllGroups();
        }
        private void HienThiThongTinPhieu(string maPhieu, string maDonvi)
        {
            PsPhieu phieu = BioNet_Bus.GetThongTinPhieu(maPhieu, maDonvi);
            this.lookupDonVi.EditValue = maDonvi;
            this.LoadGoiXetNghiem(maDonvi);
            this.LoadNew();
            if (phieu != null)
            {
                try
                {
                    try
                    {
                        if (phieu.BenhNhan != null)
                        {
                            this.txtTenMe.Text = phieu.BenhNhan.MotherName;
                            this.txtTenCha.Text = phieu.BenhNhan.FatherName;
                            this.txtSDTMe.Text = phieu.BenhNhan.MotherPhoneNumber;
                            this.txtSDTCha.Text = phieu.BenhNhan.FatherPhoneNumber;
                            this.txtNamSinhMe.EditValue = phieu.BenhNhan.MotherBirthday;
                            this.txtNamSinhCha.EditValue = phieu.BenhNhan.FatherBirthday;

                            this.txtTenBenhNhan.Text = phieu.BenhNhan.TenBenhNhan;
                            this.txtCanNang.Text = phieu.BenhNhan.CanNang.ToString();
                            //this.txtGioiTinh.EditValue = phieu.BenhNhan.GioiTinh??2;
                            this.txtGioiTinh.SelectedIndex = phieu.BenhNhan.GioiTinh ?? 2;
                         
                            this.cboPhuongPhapSinh.SelectedIndex = Convert.ToInt16(phieu.BenhNhan.PhuongPhapSinh);
                            this.lookUpDanToc.EditValue = phieu.BenhNhan.DanTocID;
                            this.txtNoiSinh.Text = phieu.BenhNhan.NoiSinh;
                            this.txtGioSinhBenhNhan.EditValue = phieu.BenhNhan.NgayGioSinh.Value.TimeOfDay;
                            this.txtNamSinhBenhNhan.EditValue = phieu.BenhNhan.NgayGioSinh.Value.Date;
                        }
                    }
                    catch { }
                    this.barCodePhieu.Text = phieu.maPhieu;
                    this.txtNgayTruyenMau.EditValue = phieu.ngayTruyenMau;
                    this.txtSoLuongTruyenMau.Text = phieu.soLuongTruyenMau.ToString();
                    this.RadioCheDoDD.EditValue = phieu.maCheDoDinhDuong.ToString();
                    this.radioGroupTinhTrangTre.EditValue = phieu.maTinhTrangLucLayMau.ToString();
                    this.radioGroupViTriLayMau.EditValue = phieu.idViTriLayMau.ToString();
                    this.lookupChuongTrinh.EditValue = phieu.maChuongTrinh;
                    this.lookupDonVi.EditValue = phieu.maDonViCoSo;
                    this.radioGroupGoiXN.EditValue = phieu.maGoiXetNghiem;
                    this.txtGioLayMau.EditValue = phieu.ngayGioLayMau.TimeOfDay;
                    this.txtNgayLayMau.EditValue = phieu.ngayGioLayMau.Date;
                    this.txtNguoiLayMau.Text = phieu.tenNVLayMau;
                    this.txtDiaChiDonVi.Text = phieu.DonVi.DiaChiDVCS;
                    this.txtNoiLayMau.Text = phieu.NoiLayMau;
                    this.txtSDTNguoiLayMau.Text = phieu.SoDTNhanVienLayMau;
                    this.txtPara.Text = phieu.paRa;
                    this.LoadDanhSachDichVu();
                    if (phieu.maGoiXetNghiem.Equals("DVGXN0001"))
                    {

                        foreach (var item in this.lstDichVu)
                        {
                            foreach (var dv in phieu.lstChiDinh)
                            {

                                if (item.IDDichVu == dv.MaDichVu)
                                {
                                    item.isChecked = true;
                                }
                            }
                        }
                        this.checkedListBoxXN.DataSource = null;
                        this.checkedListBoxXN.DataSource = this.lstDichVu;
                    }
                    else
                    {
                        this.checkedListBoxXN.DataSource = null;
                    }         
                }
                catch (Exception ex)
                {

                }

            }
        }
        private void LoadGoiXetNghiem(string maDonVi)
        {
            var list = BioNet_Bus.GetDanhsachGoiDichVuTrungTam(maDonVi);
            this.radioGroupGoiXN.Properties.Items.Clear();
            foreach (var item in list)
            {
                this.radioGroupGoiXN.Properties.Items.Add(new DevExpress.XtraEditors.Controls.RadioGroupItem(item.IDGoiDichVuChung, item.TenGoiDichVuChung));

            }
        }
        private void CheckPhieuTiepNhan(string maPhieu, string maDonVi) //kiểm tra mã phiếu đã nằm trong lstTiepNhan hay chưa?
        {
            
            try
            {
                if(!string.IsNullOrEmpty(maPhieu))
                {
                    PsReponse isTonTaiTrongDB = BioNet_Bus.KiemTraThongTinPhieuDaDuocTiepNhan(maPhieu);
                    if (!isTonTaiTrongDB.Result)
                    {
                        XtraMessageBox.Show(isTonTaiTrongDB.StringError, "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        var ph = this.lstPhieu.FirstOrDefault(p => p.maPhieu == maPhieu);
                        if (ph != null) //thêm phiếu mới vào lstTiepNhan, mã đơn vị = mã đơn vị được người dùng chọn trên combobox
                        {

                            PSTiepNhan tNhan = new PSTiepNhan();
                            tNhan.MaDonVi = ph.maDonViCoSo;
                            tNhan.MaNVTiepNhan = this.MaNhanVienDangNhap;///"Gán mã user đăng nhập vô đây"
                            tNhan.MaPhieu = ph.maPhieu;
                            tNhan.NgayTiepNhan = DateTime.Now;
                            tNhan.isDaDanhGia = false;
                            tNhan.RowIDTiepNhan = 0;
                            tNhan.isDaNhapLieu = true;
                            ThemMoiPhieuVaoDanhSachTiepNhan(tNhan);
                            this.lstPhieu.Remove(ph);
                            this.LoadGCPhieuCho();
                            return;
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(maDonVi) || maDonVi == "all")
                            {
                                XtraMessageBox.Show("Bạn chưa chọn đơn vị để tiếp nhận phiếu!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                this.searchLookUpDonViCoSoTiepNhan.Focus();
                                this.txtMaPhieuMoi.ResetText();
                                return;
                            }
                            else
                            {
                                PSTiepNhan tNhan = new PSTiepNhan();
                                tNhan.MaDonVi = maDonVi;
                                tNhan.MaNVTiepNhan = this.MaNhanVienDangNhap;
                                tNhan.MaPhieu = maPhieu;
                                tNhan.isDaNhapLieu = false;
                                tNhan.isDaDanhGia = false;
                                tNhan.NgayTiepNhan = DateTime.Now;
                                tNhan.RowIDTiepNhan = 0;
                                ThemMoiPhieuVaoDanhSachTiepNhan(tNhan);
                            }

                        }
                    }    
                }
                else
                {
                    XtraMessageBox.Show("Bạn phải nhập mã phiếu!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show("Tiếp nhận phiếu bị lỗi - Chi tiết lỗi: " + ex, "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtMaPhieuMoi.ResetText();
                return;
            }                     
        }
        private void CheckPhieu(string maPhieu, string maDonVi) //kiểm tra mã phiếu đã nằm trong lstTiepNhan hay chưa?
        {
            try
            {
                if (this.searchLookUpDonViCoSoTiepNhan.EditValue == null && !(string.IsNullOrEmpty(maDonVi)))
                    this.searchLookUpDonViCoSoTiepNhan.EditValue = maDonVi;
            }
            catch (Exception ex) { }
            PsReponse isTonTaiTrongDB = BioNet_Bus.KiemTraThongTinPhieuDaDuocTiepNhan(maPhieu);
            if (!isTonTaiTrongDB.Result)
            {
                XtraMessageBox.Show(isTonTaiTrongDB.StringError, "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var ph = this.lstPhieu.FirstOrDefault(p => p.maPhieu == maPhieu);
            if (ph != null) //lấy dữ liệu từ lstPhieu qua nếu mã thẻ có tồn tại trong lstPhieu
            {
                PSTiepNhan tNhan = new PSTiepNhan();
                if (!(string.IsNullOrEmpty(maDonVi)))
                {
                    if (!maDonVi.Equals(ph.maDonViCoSo))
                    {
                        if (XtraMessageBox.Show("Thông tin phiếu nhập mới tồn tại trong danh sách chờ nhưng khác đơn vị \r\n Bạn bạn đã chọn sai đơn vị đúng không?", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            tNhan.MaDonVi = ph.maDonViCoSo;
                        else
                            tNhan.MaDonVi = maDonVi;
                    }
                    else tNhan.MaDonVi = ph.maDonViCoSo;

                }
                else { tNhan.MaDonVi = ph.maDonViCoSo; }
                tNhan.MaNVTiepNhan = this.MaNhanVienDangNhap;///"Gán mã user đăng nhập vô đây"
                tNhan.MaPhieu = ph.maPhieu;
                tNhan.NgayTiepNhan = DateTime.Now; //lấy theo phiên đăng nhập
                tNhan.isDaDanhGia = false;
                tNhan.RowIDTiepNhan = 0;
                tNhan.isDaNhapLieu = true;
                ThemMoiPhieuVaoDanhSachTiepNhan(tNhan);
                this.lstPhieu.Remove(ph);
                this.LoadGCPhieuCho();
            }
            else //thêm phiếu mới vào lstTiepNhan, mã đơn vị = mã đơn vị được người dùng chọn trên combobox
            {
                PSTiepNhan tNhan = new PSTiepNhan();
                if (!(string.IsNullOrEmpty(maDonVi)))
                    tNhan.MaDonVi = maDonVi;
                else tNhan.MaDonVi = searchLookUpDonViCoSoTiepNhan.EditValue.ToString();
                tNhan.MaNVTiepNhan = this.MaNhanVienDangNhap;///"Gán mã user đăng nhập vô đây"
                tNhan.MaPhieu = maPhieu;
                tNhan.isDaNhapLieu = false;
                tNhan.isDaDanhGia = false;
                tNhan.NgayTiepNhan = BioNet_Bus.GetDateTime(); //lấy theo phiên đăng nhập
                tNhan.RowIDTiepNhan = 0;
                ThemMoiPhieuVaoDanhSachTiepNhan(tNhan);
            }
        }
        private void ThemMoiPhieuVaoDanhSachTiepNhan(PSTiepNhan phieu)
        {
            if (phieu != null)
            {
                if (this.lstTiepNhan.FindAll(p => p.MaPhieu == phieu.MaPhieu).Count() > 0)
                {
                    XtraMessageBox.Show("Phiếu này đã được nhập rồi,vui lòng chọn phiếu mới", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.txtMaPhieuMoi.ResetText();
                }
                else
                {
                    this.lstTiepNhan.Add(phieu);
                    this.LoadDanhSachTiepNhan();
                    this.txtMaPhieuMoi.ResetText();
                    this.txtMaPhieuMoi.Focus();
                }
            }
        }
        private void radioGroupTinhTrangTre_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                RadioGroup rd = sender as RadioGroup;
                var ts = rd.EditValue;
                if (int.Parse(ts.ToString()) == 4)
                {
                    this.txtSoLuongTruyenMau.Enabled = true;
                    this.txtNgayTruyenMau.Enabled = true;
                }
                else
                {
                    this.txtSoLuongTruyenMau.Enabled = false;
                    this.txtNgayTruyenMau.Enabled = false;
                }
            }
            catch
            {
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
                    this.checkedListBoxXN.Enabled = true;
                }
                else
                    this.checkedListBoxXN.Enabled = false;
            }
            catch { }
        }


        //GVPhieuCho

            
        private void GVPhieuCho_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (this.GVPhieuCho.RowCount > 0)
                {
                    if (this.GVPhieuCho.GetFocusedRow() != null)
                    {
                        string maPhieu = this.GVPhieuCho.GetRowCellValue(this.GVPhieuCho.FocusedRowHandle, this.col_IDPhieu_GCPhieuCho).ToString();
                        string maDonVi = this.GVPhieuCho.GetRowCellValue(this.GVPhieuCho.FocusedRowHandle, this.col_IDCoSo_GCPhieuCho).ToString();
                        this.HienThiThongTinPhieu(maPhieu, maDonVi);
                    }
                }
            }
            catch { }
        }

        private void GVPhieuCho_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.GVPhieuCho.RowCount > 0)
                {
                    if (this.GVPhieuCho.GetFocusedRow() != null)
                    {
                        string maPhieu = this.GVPhieuCho.GetRowCellValue(this.GVPhieuCho.FocusedRowHandle, this.col_IDPhieu_GCPhieuCho).ToString();
                        string maDonVi = this.GVPhieuCho.GetRowCellValue(this.GVPhieuCho.FocusedRowHandle, this.col_IDCoSo_GCPhieuCho).ToString();
                        this.HienThiThongTinPhieu(maPhieu, maDonVi);
                        if (!string.IsNullOrEmpty(maPhieu))
                        {
                            this.CheckPhieuTiepNhan(maPhieu,maDonVi);
                        }
                    }
                }
            }
            catch { }
        }

        private void GCPhieuCho_ViewRegistered(object sender, DevExpress.XtraGrid.ViewOperationEventArgs e)
        {
            if (!e.View.IsDetailView) return;
            GridView dtview = e.View as GridView;
            dtview.Columns["rowID"].Visible = false;
            dtview.Columns["maPhieu"].Visible = false;
            dtview.Columns["maDichVu"].Caption = "Dịch vụ";
        }

        //private void GVPhieuCho_RowClick(object sender, RowClickEventArgs e)
        //{
        //    try
        //    {
        //        if (this.GVPhieuCho.RowCount > 0)
        //        {
        //            if (this.GVPhieuCho.GetFocusedRow() != null)
        //            {
        //                string maPhieu = this.GVPhieuCho.GetRowCellValue(this.GVPhieuCho.FocusedRowHandle, this.col_IDPhieu_GCPhieuCho).ToString();
        //                string maDonVi = this.GVPhieuCho.GetRowCellValue(this.GVPhieuCho.FocusedRowHandle, this.col_IDCoSo_GCPhieuCho).ToString();
        //                this.HienThiThongTinPhieu(maPhieu);
        //            }
        //        }
        //    }
        //    catch { }
        //}
        private void GVPhieuCho_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {
                GridView View = sender as GridView;
                if (e.RowHandle >= 0)
                {
                    string RowPatientID = View.GetRowCellDisplayText(e.RowHandle, View.Columns["maBenhNhan"]);
                    string NgayTaoPhieu =View.GetRowCellDisplayText(e.RowHandle, col_NgayTaoPhieu_GCPhieuCho);
                    var TimeS = DateTime.Now.Date - Convert.ToDateTime(NgayTaoPhieu).Date;
                    if (string.IsNullOrEmpty(RowPatientID))
                    {
                        e.Appearance.BackColor = Color.Goldenrod;
                        e.Appearance.BackColor2 = Color.Gold;
                    }
                    else
                    {
                        e.Appearance.BackColor = Color.Aqua;
                        e.Appearance.BackColor2 = Color.AliceBlue;
                    }
                    if(TimeS.Days>7)
                    {
                        e.Appearance.BackColor = Color.Goldenrod;
                        e.Appearance.BackColor2 = Color.Gold;
                    }
                }
            }
            catch { }
        }

        //GVPhieuNhan
        private void GVDanhSachTiepNhan_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            bool enable = false;
            if (e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell)
            {
                e.Allow = false;
                enable = true;
                PopupMenuRowGV.ItemLinks[0].Visible = enable;
                //PopupMenuRowGV.ItemLinks[2].Visible = enable;
                PopupMenuRowGV.ShowPopup(GCDanhSachTiepNhan.PointToScreen(e.Point));
            }
            else
            {
                e.Allow = false;
                enable = false;
                PopupMenuRowGV.ItemLinks[0].Visible = enable;
                //PopupMenuRowGV.ItemLinks[2].Visible = enable;
                PopupMenuRowGV.ShowPopup(GCDanhSachTiepNhan.PointToScreen(e.Point));
            }

        }
        private void GVDanhSachTiepNhan_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.GVDanhSachTiepNhan.RowCount > 0)
                {
                    if (this.GVDanhSachTiepNhan.GetFocusedRow() != null)
                    {
                        string maPhieu = this.GVDanhSachTiepNhan.GetRowCellValue(this.GVDanhSachTiepNhan.FocusedRowHandle, this.col_MaPhieu_GCTiepNhan ).ToString();
                        string maDonVi = this.GVDanhSachTiepNhan.GetRowCellValue(this.GVDanhSachTiepNhan.FocusedRowHandle, this.col_DonViCoSo_GCTiepNhan).ToString();
                        this.HienThiThongTinPhieu(maPhieu, maDonVi);
                        if (!string.IsNullOrEmpty(maPhieu))
                        {
                            this.CheckPhieuTiepNhan(maPhieu, maDonVi);
                        }
                    }
                }
            }
            catch { }
        }
        private void GVDanhSachTiepNhan_MouseDown(object sender, MouseEventArgs e)
        {

            string maPhieu = string.Empty;
            string maDonVi = string.Empty;
            GridView view = sender as GridView;
            GridViewInfo viewInfo = view.GetViewInfo() as GridViewInfo;
            GridHitInfo hitInfo = view.CalcHitInfo(e.Location);
            if (hitInfo.InRow)
            {
                if (hitInfo.Column == this.col_MaPhieu_GCTiepNhan)
                {
                    int a = 2;
                }
                try
                {
                    var tempMaPhieu = view.GetRowCellValue(hitInfo.RowHandle, view.Columns["MaPhieu"]).ToString();
                    if (tempMaPhieu != null)
                        maPhieu = tempMaPhieu;
                    var tempDonVi = view.GetRowCellValue(hitInfo.RowHandle, view.Columns["MaDonVi"]).ToString();
                    if (tempDonVi != null)
                        maDonVi = tempDonVi;
                }
                catch { }
            }
            if (!string.IsNullOrEmpty(maPhieu) && !string.IsNullOrEmpty(maDonVi))
            {
                this.maPhieuFocusHandle = maPhieu;
                this.maDonviFocusHandle = maDonVi;
            }
        }

        private void GVDaTiepNhan_RowCellClick(object sender, RowCellClickEventArgs e)
        {


            this.LoadDanhSachChuongTrinhSangLoc();
            this.LoadDanhSachDichVu();
            this.LoadLookupDanToc();



            try
            {
                if (this.GVDaTiepNhan.RowCount > 0)
                {
                    if (this.GVDaTiepNhan.GetFocusedRow() != null)
                    {
                        string maPhieu = this.GVDaTiepNhan.GetRowCellValue(this.GVDaTiepNhan.FocusedRowHandle, this.col_MaPhieuDatiepNhan).ToString();
                        string maDonVi = this.GVDaTiepNhan.GetRowCellValue(this.GVDaTiepNhan.FocusedRowHandle, this.col_donViCoSoDaTiepNhan).ToString();
                        this.HienThiThongTinPhieu(maPhieu, maDonVi);
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void GVDanhSachTiepNhan_RowStyle(object sender, RowStyleEventArgs e)
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
            catch { }
        }

        // Button sự kiện
        private void txtMaPhieuMoi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                string maDonVi = this.searchLookUpDonViCoSoTiepNhan.EditValue.ToString();
                this.CheckPhieuTiepNhan(txtMaPhieuMoi.Text.Trim(), maDonVi);

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.GVDanhSachTiepNhan.RowCount > 0)
                this.LuuDotTiepNhan();
        }

        private void btnRefesh_Click(object sender, EventArgs e)
        {
            this.LoadDanhSachDaTiepNhan();
            this.LoadDuLieuDanhSachPhieu();
        }
        private void ItemDelete_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ItemEdit_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnTiepNhan_Click(object sender, EventArgs e)
        {
            string maDonVi = this.searchLookUpDonViCoSoTiepNhan.EditValue.ToString();
            this.CheckPhieuTiepNhan(txtMaPhieuMoi.Text.Trim(), maDonVi);
        }
        private void btnDelBarPopupMenuRowGV_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PsReponse res = new PsReponse();
            try
            {
                if (this.GVDanhSachTiepNhan.GetFocusedRow() != null)
                {
                    string maPhieuXoa = this.GVDanhSachTiepNhan.GetRowCellValue(this.GVDanhSachTiepNhan.FocusedRowHandle, this.col_MaPhieu_GCTiepNhan).ToString();
                    if (XtraMessageBox.Show("Bạn chắn chắn muốn xóa mã phiếu : " + maPhieuXoa + " ra khỏi danh sách tiếp nhận chứ ?", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        var ph = this.lstTiepNhan.FirstOrDefault(p => p.MaPhieu == maPhieuXoa);
                        if (ph != null)
                        {
                            this.lstTiepNhan.Remove(ph);
                            var phTontai = this.lstPhieuTaiDVCS.FirstOrDefault(p => p.maPhieu == maPhieuXoa);
                            if (phTontai != null)
                            {
                                this.lstPhieu.Add(phTontai);
                                LoadGCPhieuCho();
                            }
                            this.LoadDanhSachTiepNhan();
                        }
                    }
                }
                
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show("Lỗi xóa phiếu khỏi danh sách tiếp nhận " + ex, "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }         
        }
      

        private void btnXoa_Click(object sender, EventArgs e)
        {

        }

        private void GVPhieuCho_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            bool enable = false;
            if (e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell)
            {
                e.Allow = false;
                enable = true;
                popupMenuRowChoHeThong.ItemLinks[0].Visible = enable;
                popupMenuRowChoHeThong.ShowPopup(GCPhieuCho.PointToScreen(e.Point));
            }
            else
            {
                e.Allow = false;
                enable = false;
                popupMenuRowChoHeThong.ItemLinks[0].Visible = enable;
                popupMenuRowChoHeThong.ShowPopup(GCPhieuCho.PointToScreen(e.Point));
            }
        }

        private void btnXoaPhieu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PsReponse res = new PsReponse();
            try
            {
                if (XtraMessageBox.Show("Bạn chắn chắn muốn xóa mã phiếu ra khỏi danh sách tiếp nhận chứ ?", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {                    
                    if (this.GVPhieuCho.RowCount > 0)
                     {
                       if (this.GVPhieuCho.GetFocusedRow() != null)
                            {
                            string maPhieuXoa = this.GVPhieuCho.GetRowCellValue(this.GVPhieuCho.FocusedRowHandle, this.col_IDPhieu_GCPhieuCho).ToString();
                            string maBenhNhanXoa = this.GVPhieuCho.GetRowCellValue(this.GVPhieuCho.FocusedRowHandle, this.col_RowIDBenhNhan_GCPhieuCho).ToString();
                            string maDVXoa = this.GVPhieuCho.GetRowCellValue(this.GVPhieuCho.FocusedRowHandle, this.col_IDCoSo_GCPhieuCho).ToString();
                            // res = BioNet_Bus.DelPhieuTiepNhan(maPhieuXoa, maBenhNhanXoa);
                            DiaglogFrm.FrmYeuCauMatKhauXacThuc frm = new DiaglogFrm.FrmYeuCauMatKhauXacThuc(maPhieuXoa,maDVXoa , MaNhanVienDangNhap);
                            frm.ShowDialog();
                            if (frm.DialogResult == DialogResult.OK)
                            {

                                this.LoadDanhSachDaTiepNhan();
                                this.LoadDuLieuDanhSachPhieu();
                            }
                        }
                     }
                  
                }
            }
            catch { }
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
            CustomLayouts.TransLanguage.Trans(this.Controls, idfo);
        }
    }
}