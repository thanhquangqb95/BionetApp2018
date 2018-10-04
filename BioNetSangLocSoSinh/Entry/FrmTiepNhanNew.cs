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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using System.Threading;

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
        public List<PsPhieu> lstPhieuChoHTMD = new List<PsPhieu>();
        public List<PSTiepNhan> lstDaTiepNhan = new List<PSTiepNhan>();
        private List<PsDichVu> lstDichVu = new List<PsDichVu>();
        public FrmTiepNhanNew(PsEmployeeLogin employee)
        {
            InitializeComponent();
            emp = employee;
        }

        private void FrmTiepNhanNew_Load(object sender, EventArgs e)
        {
            this.LoadDataDanhMuc();
            this.GetDanhSachChoTrenHT();
            this.GetDanhSachDaTiepNhan();
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
                this.txtTuNgay_ChuaKQ.EditValue = DateTime.Now;
                this.txtDenNgay_ChuaKQ.EditValue = DateTime.Now;
                this.lookupDVCSChoTrenHT.DataSource = lstDonVi;
                this.lstDanToc = BioNet_Bus.GetDanhSachDanToc(-1);//get all dân tộc
                this.lstChuongTrinh = BioNet_Bus.GetDanhSachChuongTrinh(false);
                this.lookUpDanToc.Properties.DataSource = this.lstDanToc;
            }
            catch (Exception ex)
            {
                DiaglogFrm.FrmNotData notData = new DiaglogFrm.FrmNotData(ex.ToString());
            }
        }

        private void GetDanhSachChoTrenHT()
        {
            this.lstPhieuChoHT = BioNet_Bus.GetDanhSachPhieuChoTiepNhan();
            this.lstPhieuChoHTMD = BioNet_Bus.GetDanhSachPhieuChoTiepNhan();
            LoadDanhSachChoTrenHeThong();
        }

        private void GetDanhSachDaTiepNhan()
        {
            this.lstDaTiepNhan = BioNet_Bus.GetDanhSachPhieuChuaDanhGia("all");
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
                if (cbbChiCucDaTiepNhan.EditValue.ToString() != "all")
                {
                    FilterTiepNhanTheoDonViChuaCoKQ();
                }
                else
                {
                    GVDaTiepNhan.ClearColumnsFilter();
                }
            }
            catch { }
        }

       
        private void FilterTiepNhanTheoDonViChuaCoKQ()
        {
            string machicuc = cbbChiCucDaTiepNhan.EditValue.ToString();
            string madv = cbbDonViDaTiepNhan.EditValue.ToString();
            if (!machicuc.Equals("all") && madv.Equals("all"))
            {
                madv = machicuc;
            }
            if (!madv.Equals("all"))
            {
                string filterMaDV = "Contains([MaDonVi], '" + madv + "')";
                GVDaTiepNhan.Columns["MaDonVi"].FilterInfo = new ColumnFilterInfo(filterMaDV);
            }
            else
            {
                GVDaTiepNhan.Columns["MaDonVi"].ClearFilter();
            }
            this.GVDaTiepNhan.ExpandAllGroups();
            this.GVDaTiepNhan.OptionsDetail.ShowDetailTabs = false;
        }
        #region Tiếp nhận phiếu
        private void txtMaPhieuTiepNhan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                string maDonVi = this.cbbDonViChoDuyet.EditValue.ToString();
                if (!string.IsNullOrEmpty(txtMaPhieuTiepNhan.Text.Trim()))
                {
                    var ph = lstChoTiepNhan.FirstOrDefault(x => x.MaPhieu.Equals(txtMaPhieuTiepNhan.Text.Trim()));
                    if (!maDonVi.Equals("all")|| ph!=null)
                    {
                        this.CheckPhieuTiepNhan(txtMaPhieuTiepNhan.Text.Trim(), maDonVi);
                        this.txtMaPhieuTiepNhan.ResetText();
                        this.txtMaPhieuTiepNhan.Focus();
                    }
                    else
                    {
                        DiaglogFrm.FrmWarning notData = new DiaglogFrm.FrmWarning("Yêu cầu chọn đơn vị.");
                        notData.ShowDialog();
                    }
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
                List<PSTiepNhan> lst = lstChoTiepNhan.OrderBy(x => x.MaPhieu).ToList();
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
                this.GetDanhSachDaTiepNhan();
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

        private void txtHuyVeDanhSachCho_Click(object sender, EventArgs e)
        {
            PsReponse res = new PsReponse();
            try
            {
                int[] lstChecked = this.GVDanhSachTiepNhan.GetSelectedRows();
                if (XtraMessageBox.Show("Bạn chắn chắn muốn hủy danh sách phiếu chờ  khỏi danh sách tiếp nhận chứ?", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    List<PSTiepNhan> lstTemp = new List<PSTiepNhan>();
                    foreach (var index in lstChecked)
                    {
                        if (index >= 0)
                        {
                            string maPhieuXoa = this.GVDanhSachTiepNhan.GetRowCellValue(index, this.col_MaPhieu_GCTiepNhan).ToString();
                            var ph = this.lstChoTiepNhan.FirstOrDefault(p => p.MaPhieu == maPhieuXoa);
                            if (ph != null)
                            {
                                lstTemp.Add(ph);
                                 var phTontai = this.lstPhieuChoHTMD.FirstOrDefault(p => p.maPhieu == maPhieuXoa);
                                if (phTontai != null)
                                {
                                    this.lstPhieuChoHT.Add(phTontai);     
                                }
                                
                            }
                        }
                    }
                    if(lstTemp.Count()>0)
                    {
                        foreach(var lst in lstTemp)
                        {
                            lstChoTiepNhan.Remove(lst);
                        }
                    }
                    this.LoadDanhSachChoTrenHeThong();
                    this.LoadDanhSachChoDuyetTiepNhan();
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi xóa phiếu khỏi danh sách tiếp nhận " + ex, "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
                            this.CheckPhieuTiepNhan(maPhieu, maDonVi);
                        }
                    }
                }
            }
            catch { }
        }

        #region //hiện thị phiếu
        private void HienThiThongTinPhieu(string maPhieu, string maDonvi)
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
                            this.txtNamSinhMe.Text = phieu.BenhNhan.MotherBirthday!=null?phieu.BenhNhan.MotherBirthday.Value.Year.ToString():null;
                            if(!string.IsNullOrEmpty(phieu.BenhNhan.FatherName))
                            {
                                this.txtNamSinhCha.Text = phieu.BenhNhan.FatherBirthday!=null?phieu.BenhNhan.FatherBirthday.Value.Year.ToString():null;
                                this.txtSDTCha.Text = phieu.BenhNhan.FatherPhoneNumber;
                            }
                           
                            this.txtTuanTuoi.Text = phieu.BenhNhan.TuanTuoiKhiSinh.ToString();
                            this.txtTenBenhNhan.Text = phieu.BenhNhan.TenBenhNhan;
                            this.txtCanNang.Text = phieu.BenhNhan.CanNang.ToString();
                            this.txtGioiTinh.SelectedIndex = phieu.BenhNhan.GioiTinh ?? 2;
                            this.cboPhuongPhapSinh.SelectedIndex = Convert.ToInt16(phieu.BenhNhan.PhuongPhapSinh);
                            this.lookUpDanToc.EditValue = phieu.BenhNhan.DanTocID;
                            this.txtNoiSinh.Text = phieu.BenhNhan.NoiSinh;
                            if (phieu.BenhNhan.NgayGioSinh!=null)
                            {
                                this.txtGioSinhBenhNhan.EditValue = phieu.BenhNhan.NgayGioSinh.Value.TimeOfDay;
                                this.txtNamSinhBenhNhan.EditValue = phieu.BenhNhan.NgayGioSinh.Value.ToShortDateString();
                            }
                        }
                    }
                    catch { }
                    this.txtMaPhieu.Text = phieu.maPhieu;
                    this.txtDiaChiNoiLayMau.Text = phieu.DiaChiLayMau;
                    this.txtNgayTruyenMau.EditValue = phieu.ngayTruyenMau;
                    this.txtSoLuongTruyenMau.Text = phieu.soLuongTruyenMau.ToString();
                    this.cbbCheDoDD.EditValue = phieu.maCheDoDinhDuong.ToString();
                    this.cbbTTTre.EditValue = phieu.maTinhTrangLucLayMau.ToString();
                    this.cbbViTriLayMau.EditValue = phieu.idViTriLayMau.ToString();
                    this.txtChuongTrinh.EditValue = lstChuongTrinh.FirstOrDefault(x => x.IDChuongTrinh == phieu.maChuongTrinh).TenChuongTrinh;
                    var tendv = lstDonVi.FirstOrDefault(x => x.MaDVCS.Equals(maDonvi));
                    if(tendv!=null)
                    {
                        this.txtDonVi.EditValue = tendv.TenDVCS;
                    }
                    this.radioGroupGoiXN.EditValue = phieu.maGoiXetNghiem;
                    if(phieu.ngayGioLayMau!=null)
                    {
                        this.txtGioLayMau.EditValue = phieu.ngayGioLayMau.TimeOfDay;
                        this.txtNgayLayMau.EditValue = phieu.ngayGioLayMau.Date;
                    }
                    this.txtNguoiLayMau.Text = phieu.tenNVLayMau;
                    this.txtDiaChiDonVi.Text = phieu.DonVi.DiaChiDVCS;
                    this.txtNoiLayMau.Text = phieu.NoiLayMau;
                    this.txtSDTNguoiLayMau.Text = phieu.SoDTNhanVienLayMau;
                    this.txtPara.Text = phieu.paRa;
                    this.LoadDanhSachDichVu();
                    if (phieu.maGoiXetNghiem.Equals("DVGXN0001"))
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
                        List<PsDichVu> lstChiDinhDichVu = BioNet_Bus.GetDanhSachDichVuTheoMaGoi(phieu.maGoiXetNghiem, phieu.maDonViCoSo);
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
        private void GVPhieuCho_Click(object sender, EventArgs e)
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
        private void cbbTTTre_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if(cbbTTTre.EditValue.ToString()=="4")
                {
                    txtNgayTruyenMau.Visible = true;
                    txtSoLuongTruyenMau.Visible = true;
                    lblNgayTruyen.Visible = true;
                    lblSoLuong.Visible = true;
                    lblTruyenMau.Visible = true;
                }
                else
                {
                    txtNgayTruyenMau.Visible = false;
                    txtSoLuongTruyenMau.Visible = false;
                    lblNgayTruyen.Visible = false;
                    lblSoLuong.Visible = false;
                    lblTruyenMau.Visible = false;
                }
            }
            catch
            {
            }
        }

        private void GVDaTiepNhan_Click(object sender, EventArgs e)
        {

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
            catch { }
        }

        private void btnRefesh_Click(object sender, EventArgs e)
        {
            this.GetDanhSachDaTiepNhan();
        }

        private void GVPhieuCho_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                GridView View = sender as GridView;
                if (e.RowHandle >= 0)
                {
                    string RowPatientID = View.GetRowCellDisplayText(e.RowHandle, View.Columns["maBenhNhan"]);
                    string NgayTaoPhieu = View.GetRowCellDisplayText(e.RowHandle, col_NgayTaoPhieu_GCPhieuCho);
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
                    if (TimeS.Days > 7)
                    {
                        e.Appearance.BackColor = Color.Goldenrod;
                        e.Appearance.BackColor2 = Color.Gold;
                    }
                }
            }
            catch
            {
            }
        }

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
            catch { }
        }


        private void GVDaTiepNhan_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                GridView View = sender as GridView;
                if (e.RowHandle >= 0)
                {
                    string GoiXN = View.GetRowCellDisplayText(e.RowHandle, col_GoiXN);
                    string NgayTaoPhieu = View.GetRowCellDisplayText(e.RowHandle, col_NgayTaoPhieu_GCPhieuCho);
                    var TimeS = DateTime.Now.Date - Convert.ToDateTime(NgayTaoPhieu).Date;
                    if (string.IsNullOrEmpty(GoiXN))
                    {
                        e.Appearance.BackColor = Color.Goldenrod;
                        e.Appearance.BackColor2 = Color.Gold;
                    }
                    else
                    {
                        e.Appearance.BackColor = Color.Aqua;
                        e.Appearance.BackColor2 = Color.AliceBlue;
                    }
                    if (TimeS.Days > 7)
                    {
                        e.Appearance.BackColor = Color.Goldenrod;
                        e.Appearance.BackColor2 = Color.Gold;
                    }
                }
            }
            catch
            {

            }
        }

        private void txtMaPhieuSearch_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtMaPhieuSearch.EditValue.ToString()))
                {
                    string filterMaPhieu = "Contains([MaPhieu], '" + txtMaPhieuSearch.EditValue + "')";
                    GVDaTiepNhan.Columns["MaPhieu"].FilterInfo = new ColumnFilterInfo(filterMaPhieu);
                }
                else
                {
                    GVDaTiepNhan.Columns["MaPhieu"].ClearFilter();
                }

            }
            catch
            {
                GVDaTiepNhan.Columns["MaPhieu"].FilterInfo = new ColumnFilterInfo();
            }
            this.GVDaTiepNhan.ExpandAllGroups();
        }

        private void cbbDonViDaTiepNhan_EditValueChanged(object sender, EventArgs e)
        {
            FilterTiepNhanTheoDonViChuaCoKQ();
        }

        private void btnHuyDuyetTiepNhan_Click(object sender, EventArgs e)
        {
            PsReponse res = new PsReponse();
            res.Result = true;
            try
            {
                int[] lstChecked = this.GVDaTiepNhan.GetSelectedRows();
                if (XtraMessageBox.Show("Bạn chắn chắn muốn hủy danh sách phiếu chờ  khỏi danh sách tiếp nhận chứ?", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    List<PSTiepNhan> lstTemp = new List<PSTiepNhan>();
                    foreach (var index in lstChecked)
                    {
                        if (index >= 0)
                        {
                            string maPhieuXoa = this.GVDaTiepNhan.GetRowCellValue(index, this.col_MaPhieuDatiepNhan).ToString();
                           var kq= BioNet_Bus.HuyPhieuDaTiepNhan(maPhieuXoa);
                            if(!kq.Result)
                            {
                                res.Result = false;
                            }
                        }
                    }
                    if (lstTemp.Count() > 0)
                    {
                        foreach (var lst in lstTemp)
                        {
                            lstChoTiepNhan.Remove(lst);
                        }
                    }
                    if(!res.Result)
                    {
                        XtraMessageBox.Show("Lỗi hủy phiếu tiết nhận.", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    this.GetDanhSachDaTiepNhan();
                    this.GetDanhSachChoTrenHT();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi xóa phiếu khỏi danh sách tiếp nhận " + ex, "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GVDanhSachTiepNhan_Click(object sender, EventArgs e)
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

        private void btnXoaPhieuCho_Click(object sender, EventArgs e)
        {
            
        }

        private void btnHuyPhieuCho_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (XtraMessageBox.Show("Bạn chắn chắn muốn hủy mã phiếu ra khỏi danh sách tiếp nhận chứ ?", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (this.GVPhieuCho.RowCount > 0)
                    {
                        if (this.GVPhieuCho.GetFocusedRow() != null)
                        {
                            string maPhieuXoa = this.GVPhieuCho.GetRowCellValue(this.GVPhieuCho.FocusedRowHandle, this.col_IDPhieu_GCPhieuCho).ToString();
                            string maBenhNhanXoa = this.GVPhieuCho.GetRowCellValue(this.GVPhieuCho.FocusedRowHandle, this.col_MaBenhNhan_GCPhieuCho).ToString();
                            string maDVXoa = this.GVPhieuCho.GetRowCellValue(this.GVPhieuCho.FocusedRowHandle, this.col_IDCoSo_GCPhieuCho).ToString();
                            DiaglogFrm.FrmYeuCauMatKhauXacThuc frm = new DiaglogFrm.FrmYeuCauMatKhauXacThuc(maPhieuXoa, maDVXoa, emp.EmployeeCode);
                            frm.ShowDialog();
                            if (frm.DialogResult == DialogResult.OK)
                            {
                                this.GetDanhSachChoTrenHT();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi hủy phiếu tiếp nhận " + ex, "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GVPhieuCho_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            bool enable = false;
            if (e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell)
            {
                e.Allow = false;
                enable = true;
                popupGCPhieuCho.ItemLinks[0].Visible = enable;
                popupGCPhieuCho.ShowPopup(GCPhieuCho.PointToScreen(e.Point));
            }
            else
            {
                e.Allow = false;
                enable = false;
                popupGCPhieuCho.ItemLinks[0].Visible = enable;
                popupGCPhieuCho.ShowPopup(GCPhieuCho.PointToScreen(e.Point));
            }
        }

        private void txtLayDanhSachCho_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstChoTiepNhan.Count()==0)
                {
                    this.GetDanhSachChoTrenHT();
                }
                else
                {
                    XtraMessageBox.Show("Danh sách chờ tiếp nhận còn phiếu - Không thể cập nhật danh sách chờ hệ thống", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }             
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show("Lỗi danh sách chờ "+ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
        }
    }   
}