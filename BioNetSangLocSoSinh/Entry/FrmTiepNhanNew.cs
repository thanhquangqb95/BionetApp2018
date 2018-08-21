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
using DevExpress.XtraSplashScreen;
using DevExpress.XtraEditors.Controls;

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmTiepNhanNew : DevExpress.XtraEditors.XtraForm
    {
        public static PsEmployeeLogin emp = new PsEmployeeLogin();
        public List<PSDanhMucDonViCoSo> lstDonVi = new List<PSDanhMucDonViCoSo>();
        public List<PSDanhMucChiCuc> lstChiCuc = new List<PSDanhMucChiCuc>();
        public List<PSDanhMucDanToc> lstDanToc = new List<PSDanhMucDanToc>();
        public List<PSDanhMucChuongTrinh> lstChuongTrinh = new List<PSDanhMucChuongTrinh>();

        public List<PSTiepNhan> lstChoTiepNhan = new List<PSTiepNhan>();
        public List<PsPhieu> lstPhieuChoHT = new List<PsPhieu>();
        public List<PSTiepNhan> lstDaTiepNhan = new List<PSTiepNhan>();
        private List<PsDichVu> lstDichVu = new List<PsDichVu>();
        public FrmTiepNhanNew(PsEmployeeLogin employee)
        {
            InitializeComponent();
            emp = employee;
        }

        private void FrmTiepNhanNew_Load(object sender, EventArgs e)
        {
            LoadDataDanhMuc();
            GetDanhSachChoTrenHT();
            GetDanhSachDaTiepNhan();
        }

        private void LoadDataDanhMuc()
        {
            try
            {
                this.lstChiCuc = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
                this.lstDonVi = BioNet_Bus.GetDanhSachDonViCoSo();
                this.cbbChiCucChoDuyet.Properties.DataSource = lstChiCuc;
                this.cbbChiCucChoDuyet.EditValue = "all";
                this.cbbChiCucDaTiepNhan.Properties.DataSource = lstChiCuc;
                this.cbbChiCucDaTiepNhan.EditValue = "all";
                this.lookupDVCS.DataSource = lstDonVi;
                this.lookupDVCSDuyetTiepNhan.DataSource = lstDonVi;
                this.lookupDVCSChoTrenHT.DataSource = lstDonVi;
                this.lstDanToc = BioNet_Bus.GetDanhSachDanToc(-1);//get all dân tộc
                this.lstChuongTrinh = BioNet_Bus.GetDanhSachChuongTrinh(false);
            }
            catch (Exception ex)
            {
                DiaglogFrm.FrmNotData notData = new DiaglogFrm.FrmNotData(ex.ToString());
            }
        }

        private void GetDanhSachChoTrenHT()
        {
            this.lstPhieuChoHT = BioNet_Bus.GetDanhSachPhieuChoTiepNhan();
            LoadDanhSachChoTrenHeThong();
        }

        private void GetDanhSachDaTiepNhan()
        {
            string machicuc = cbbChiCucDaTiepNhan.EditValue.ToString();
            string madv = cbbDonViDaTiepNhan.EditValue.ToString();
            if (!machicuc.Equals("all") && madv.Equals("all"))
            {
                madv = machicuc;
            }
            this.lstDaTiepNhan = BioNet_Bus.GetDanhSachPhieuChuaDanhGia(madv);
            LoadDanhSachDaTiepNhan();
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

        private void LoadDanhSachChoTrenHeThong()
        {
            this.GCPhieuCho.DataSource = null;
            this.GCPhieuCho.DataSource = this.lstPhieuChoHT;
            this.GVPhieuCho.Columns["maDonViCoSo"].Group();
            this.GVPhieuCho.ExpandAllGroups();
            this.GVPhieuCho.OptionsDetail.ShowDetailTabs = false;
        }

        private void LoadDanhSachChoDuyetTiepNhan()
        {
            this.GCDanhSachTiepNhan.DataSource = null;
            this.GCDanhSachTiepNhan.DataSource = this.lstChoTiepNhan;
            this.GVDanhSachTiepNhan.Columns["MaDonVi"].Group();
            this.GVDanhSachTiepNhan.ExpandAllGroups();
            this.GVDanhSachTiepNhan.OptionsDetail.ShowDetailTabs = false;
        }

        private void LoadDanhSachDaTiepNhan()
        {
            this.GCDaTiepNhan.DataSource = null;
            this.GCDaTiepNhan.DataSource = this.lstDaTiepNhan;
            this.GVDaTiepNhan.Columns["MaDonVi"].Group();
            this.GVDaTiepNhan.ExpandAllGroups();
            this.GVDaTiepNhan.OptionsDetail.ShowDetailTabs = false;
        }

        private void cbbChiCucChoDuyet_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit sear = sender as SearchLookUpEdit;
                var value = sear.EditValue.ToString();
                this.cbbDonViChoDuyet.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi(value.ToString());
                this.cbbDonViChoDuyet.EditValue = "all";
            }
            catch { }
        }

        private void cbbChiCucDaTiepNhan_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit sear = sender as SearchLookUpEdit;
                var value = sear.EditValue.ToString();
                this.cbbDonViDaTiepNhan.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi(value.ToString());
                this.cbbDonViDaTiepNhan.EditValue = "all";
            }
            catch { }
        }

        private void cbbDonViChoDuyet_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cbbDonViChoDuyet.EditValue.Equals("all"))
                {
                }
                else
                {
                    this.txtMaPhieuTiepNhan.Enabled = true;
                }
            }
            catch
            { }
        }

        #region Tiếp nhận phiếu
        private void txtMaPhieuTiepNhan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                string maDonVi = this.cbbDonViChoDuyet.EditValue.ToString();
                if (!string.IsNullOrEmpty(txtMaPhieuTiepNhan.Text.Trim()))
                {
                    this.CheckPhieuTiepNhan(txtMaPhieuTiepNhan.Text.Trim(), maDonVi);
                    this.txtMaPhieuTiepNhan.Reset();
                    this.txtMaPhieuTiepNhan.Focus();
                }
                else
                {
                    DiaglogFrm.FrmWarning notData = new DiaglogFrm.FrmWarning("Yêu cầu nhập mã phiếu tiếp nhận.");
                    notData.ShowDialog();
                }

            }
        }

        private void CheckPhieuTiepNhan(string maPhieu, string maDonVi) //kiểm tra mã phiếu đã nằm trong lstTiepNhan hay chưa?
        {
            try
            {
                PsReponse isTonTaiTrongDB = BioNet_Bus.KiemTraThongTinPhieuDaDuocTiepNhan(maPhieu);
                if (!isTonTaiTrongDB.Result)
                {
                    DiaglogFrm.FrmWarning notData = new DiaglogFrm.FrmWarning("Mã phiếu đã được tiếp nhận.");
                    notData.ShowDialog();
                    return;
                }
                else
                {
                    if (this.lstChoTiepNhan.FindAll(p => p.MaPhieu == maPhieu).Count() > 0)
                    {
                        DiaglogFrm.FrmWarning notData = new DiaglogFrm.FrmWarning("Phiếu đã có trong danh sách chờ duyệt tiếp nhận.");
                        notData.ShowDialog();
                        this.txtMaPhieuTiepNhan.ResetText();
                        return;
                    }
                    else
                    {
                        var ph = this.lstPhieuChoHT.FirstOrDefault(p => p.maPhieu == maPhieu);
                        PSTiepNhan tNhan = new PSTiepNhan();
                        if (ph != null) //thêm phiếu có cùng mã phiếu với phiếu chờ hệ thống
                        {
                            tNhan.MaDonVi = ph.maDonViCoSo;
                            tNhan.MaNVTiepNhan = emp.EmployeeCode;///"Gán mã user đăng nhập vô đây"
                            tNhan.MaPhieu = ph.maPhieu;
                            tNhan.NgayTiepNhan = DateTime.Now;
                            tNhan.isDaDanhGia = false;
                            tNhan.isDaNhapLieu = true;
                            tNhan.RowIDTiepNhan = 0;
                            this.lstPhieuChoHT.Remove(ph);
                            this.LoadDanhSachChoTrenHeThong();
                        }
                        else
                        {
                            tNhan.MaDonVi = maDonVi;
                            tNhan.MaNVTiepNhan = emp.EmployeeCode;
                            tNhan.MaPhieu = maPhieu;
                            tNhan.isDaNhapLieu = false;
                            tNhan.isDaDanhGia = false;
                            tNhan.RowIDTiepNhan = 0;
                            tNhan.NgayTiepNhan = DateTime.Now;
                        }
                        this.lstChoTiepNhan.Add(tNhan);
                        this.LoadDanhSachChoDuyetTiepNhan();
                    }
                }
            }
            catch (Exception ex)
            {
                DiaglogFrm.FrmWarning notData = new DiaglogFrm.FrmWarning("Tiếp nhận phiếu bị lỗi. " + ex.ToString());
                notData.ShowDialog();
                this.txtMaPhieuTiepNhan.ResetText();
                return;
            }
        }

        private void txtDuyetDanhSachTiepNhan_Click(object sender, EventArgs e)
        {
            
            if (lstChoTiepNhan.Count > 0)
            {
                SplashScreenManager.ShowForm(this, typeof(DiaglogFrm.WaitingfromSave), true, true, false);
                bool isOK = true;
                String Error = string.Empty;
                List<PSTiepNhan> lst = lstChoTiepNhan.OrderBy(x=>x.MaPhieu).ToList();
                foreach (var tiepnhan in lst)
                {
                    var result = BioNet_Bus.InsertTiepNhan(tiepnhan);
                    if (!result.Result)
                    {
                        Error = Error + " " + result.StringError;
                        isOK = false;
                    }
                    else
                    {
                        this.lstChoTiepNhan.Remove(tiepnhan);
                    }
                }

                this.LoadDanhSachChoDuyetTiepNhan();
                this.LoadDanhSachDaTiepNhan();
                SplashScreenManager.CloseForm();
                if (isOK)
                {
                    DiaglogFrm.FrmOK notData = new DiaglogFrm.FrmOK("Tiếp nhận phiếu thành công.");
                    notData.ShowDialog();
                }
                else
                {
                    DiaglogFrm.FrmWarning notData = new DiaglogFrm.FrmWarning("Duyệt phiếu tiếp nhận bị lỗi." + Error.ToString());
                    notData.ShowDialog();
                }
            }

        }

        #endregion

        private void GVPhieuCho_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {

        }

        private void txtHuyVeDanhSachCho_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.GVPhieuCho.RowCount > 0)
                {
                    if (this.GVPhieuCho.GetFocusedRow() != null)
                    {
                        string maPhieu = this.GVPhieuCho.GetRowCellValue(this.GVPhieuCho.FocusedRowHandle, this.col_IDPhieu_GCPhieuCho).ToString();
                        string maDonVi = this.GVPhieuCho.GetRowCellValue(this.GVPhieuCho.FocusedRowHandle, this.col_IDCoSo_GCPhieuCho).ToString();
                        string tenDonVi = this.GVPhieuCho.GetRowCellDisplayText(this.GVPhieuCho.FocusedRowHandle, this.col_IDCoSo_GCPhieuCho).ToString();
                        this.HienThiThongTinPhieu(maPhieu, maDonVi, tenDonVi);
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
                        string tenDonVi = this.GVPhieuCho.GetRowCellDisplayText(this.GVPhieuCho.FocusedRowHandle, this.col_IDCoSo_GCPhieuCho).ToString();
                        this.HienThiThongTinPhieu(maPhieu, maDonVi, tenDonVi);
                        if (!string.IsNullOrEmpty(maPhieu))
                        {
                            this.CheckPhieuTiepNhan(maPhieu, maDonVi);
                        }
                    }
                }
            }
            catch { }
        }
        private void GVDaTiepNhan_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.GVDaTiepNhan.RowCount > 0)
                {
                    if (this.GVDaTiepNhan.GetFocusedRow() != null)
                    {
                        string maPhieu = this.GVDaTiepNhan.GetRowCellValue(this.GVDaTiepNhan.FocusedRowHandle, this.col_MaPhieuDatiepNhan).ToString();
                        string maDonVi = this.GVDaTiepNhan.GetRowCellValue(this.GVDaTiepNhan.FocusedRowHandle, this.col_donViCoSoDaTiepNhan).ToString();
                        string tenDonVi = this.GVDaTiepNhan.GetRowCellDisplayText(this.GVDaTiepNhan.FocusedRowHandle, this.col_donViCoSoDaTiepNhan).ToString();
                        this.HienThiThongTinPhieu(maPhieu, maDonVi, tenDonVi);
                    }
                }
            }
            catch { }
        }

        #region //hiện thị phiếu
        private void HienThiThongTinPhieu(string maPhieu, string maDonvi, string tenDonVi)
        {
            PsPhieu phieu = BioNet_Bus.GetThongTinPhieu(maPhieu, maDonvi);
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
                            this.txtNamSinhMe.Text = phieu.BenhNhan.MotherBirthday.Value.Year.ToString();
                            this.txtNamSinhCha.Text = phieu.BenhNhan.FatherBirthday.Value.Year.ToString();
                            this.txtTuanTuoi.Text = phieu.BenhNhan.TuanTuoiKhiSinh.ToString();
                            this.txtTenBenhNhan.Text = phieu.BenhNhan.TenBenhNhan;
                            this.txtCanNang.Text = phieu.BenhNhan.CanNang.ToString();
                            this.txtGioiTinh.SelectedIndex = phieu.BenhNhan.GioiTinh ?? 2;
                            this.cboPhuongPhapSinh.SelectedIndex = Convert.ToInt16(phieu.BenhNhan.PhuongPhapSinh);
                            this.lookUpDanToc.EditValue = phieu.BenhNhan.DanTocID;
                            this.txtNoiSinh.Text = phieu.BenhNhan.NoiSinh;
                            this.txtGioSinhBenhNhan.EditValue = phieu.BenhNhan.NgayGioSinh.Value.TimeOfDay;
                            this.txtNamSinhBenhNhan.EditValue = phieu.BenhNhan.NgayGioSinh.Value.ToShortDateString();
                        }
                    }
                    catch { }
                    this.barCodePhieu.Text = phieu.maPhieu;
                    this.txtNgayTruyenMau.EditValue = phieu.ngayTruyenMau;
                    this.txtSoLuongTruyenMau.Text = phieu.soLuongTruyenMau.ToString();
                    this.cbbCheDoDD.EditValue = phieu.maCheDoDinhDuong.ToString();
                    this.cbbTTTre.EditValue = phieu.maTinhTrangLucLayMau.ToString();
                    this.cbbViTriLayMau.EditValue = phieu.idViTriLayMau.ToString();
                    this.txtChuongTrinh.EditValue = lstChuongTrinh.FirstOrDefault(x => x.IDChuongTrinh == phieu.maChuongTrinh).TenChuongTrinh;
                    this.txtDonVi.EditValue = tenDonVi;
                    this.radioGroupGoiXN.EditValue = phieu.maGoiXetNghiem;
                    this.txtGioLayMau.EditValue = phieu.ngayGioLayMau.TimeOfDay;
                    this.txtNgayLayMau.EditValue = phieu.ngayGioLayMau.Date;
                    this.txtNguoiLayMau.Text = phieu.tenNVLayMau;
                    this.txtDiaChiDonVi.Text = phieu.DonVi.DiaChiDVCS;
                    this.txtNoiLayMau.Text = phieu.NoiLayMau;
                    this.txtSDTNguoiLayMau.Text = phieu.SoDTNhanVienLayMau;
                    this.txtPara.Text = phieu.paRa;
                    this.LoadDanhSachDichVu();
                    if(phieu.maGoiXetNghiem.Equals("DVGXN0001"))
                    {
                        List<PSChiDinhTrenPhieu> listdvcanlamlai = BioNet_Bus.GetDichVuCanLamLaiCuaPhieu(phieu.maPhieu, phieu.maDonViCoSo);
                        List<PsDichVu> lstdv = new List<PsDichVu>();
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
                    else
                    {
                        List <PsDichVu> lstChiDinhDichVu = BioNet_Bus.GetDanhSachDichVuTheoMaGoi(phieu.maGoiXetNghiem, phieu.maDonViCoSo);
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
                                     
                }
                catch
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
            this.cbbCheDoDD.EditValue = "0";
            this.radioGroupGoiXN.EditValue = null;
            this.cbbTTTre.EditValue = "0";
            this.cbbViTriLayMau.EditValue = "0";
            this.txtCanNang.ResetText();
            this.txtGioiTinh.ResetText();
            this.lookUpDanToc.ResetText();
            this.txtChuongTrinh.ResetText();
            this.txtDonVi.ResetText();
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

       
        #endregion

    }   
}