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

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmTraKetQuaNew : DevExpress.XtraEditors.XtraForm
    {
        public static PsEmployeeLogin emp = new PsEmployeeLogin();
        public List<PSDanhMucDonViCoSo> lstDonVi = new List<PSDanhMucDonViCoSo>();
        public List<PSDanhMucChiCuc> lstChiCuc = new List<PSDanhMucChiCuc>();
        public List<PSDanhMucDanToc> lstDanToc = new List<PSDanhMucDanToc>();
        public List<PSDanhMucChuongTrinh> lstChuongTrinh = new List<PSDanhMucChuongTrinh>();
        public List<PSDanhMucGoiDichVuChung> lstgoiXN = new List<PSDanhMucGoiDichVuChung>();
        private List<PSXN_TraKetQua> lstDaDuyetTraKetQua = new List<PSXN_TraKetQua>();
        //private List<PSXN_TraKetQua> lstChoTraKetQua = new List<PSXN_TraKetQua>();
        private List<PSXN_TTTraKQ> lstChoTraKetQua = new List<PSXN_TTTraKQ>();
        private List<PSXN_TTTraKQ> lstChoTraKetQuaXNLan2 = new List<PSXN_TTTraKQ>();
        private List<PSXN_TraKQ_ChiTiet> lstChiTietKQ = new List<PSXN_TraKQ_ChiTiet>();
        private List<PSXN_TraKQ_ChiTiet> lstChiTietKQCu = new List<PSXN_TraKQ_ChiTiet>();
        private List<KetLuan> lstKetLuan = new List<KetLuan>();
        public FrmTraKetQuaNew(PsEmployeeLogin Employee)
        {
            InitializeComponent();
            emp = Employee;
        }

        private void FrmTraKetQuaNew_Load(object sender, EventArgs e)
        {
            this.LoadFrm();
            this.LoadDanhMuc();
        }
        #region  Load Danh mục

        private void LoadFrm()
        {
            LoadDanhMuc();
            LoadGCChuaCOKQ();
        }
        private void LoadDanhMuc()
        {
            try
            {
                this.lstgoiXN = BioNet_Bus.GetDanhsachGoiDichVuChung();
                this.cbbGoiXNLoc_ChuaCoKQ.Properties.DataSource = this.lstgoiXN;
                this.cbbGoiLocXN_CoKQ.Properties.DataSource = this.lstgoiXN;
                this.lstChiCuc.Clear();
                this.lstChiCuc = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
                this.cbbChiCuc_CoKQ.Properties.DataSource = this.lstChiCuc;
                this.cbbChiCuc_ChuaCoKQ.Properties.DataSource = this.lstChiCuc;
                this.cbbChiCuc_ChuaCoKQ.EditValue = "all";
                this.lstDonVi.Clear();
                this.lstDonVi = BioNet_Bus.GetDanhSachDonViCoSo();
                this.repositoryItemLookUpDonVi_GCChuaTraKQ.DataSource = lstDonVi;
                this.repositoryItemLookUpDonVi_GCDaTraKQ.DataSource = lstDonVi;
                this.LookUpGoiXN.DataSource = lstgoiXN;
                this.LookUpGoiXN_ChuaKQ.DataSource = lstgoiXN;
            }
            catch (Exception ex)
            {
               
            }
        }
        private void LoadGCChuaCOKQ()
        {
            this.lstChoTraKetQua.Clear();
            this.lstChoTraKetQua = BioNet_Bus.GetDanhSachChoTraKetQuaAll();
            this.GCChuaKQ.DataSource = null;
            this.GCChuaKQ.DataSource = this.lstChoTraKetQua;
            this.GVChuaKQ.ExpandAllGroups();
        }

        private void LoadDanhSachDaTraKetQua()
        {
            
        }


        #endregion

        #region Lọc DS

        #endregion

        #region L
        #endregion
        private void txtTenCha_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void GVChuaKQ_DoubleClick(object sender, EventArgs e)
        {

        }
        private void HienThiThongTinPhieu(string maPhieu, string maDonVi, string maTiepNhan, string maXetNghiem)
        {
            PsPhieu phieu = BioNet_Bus.GetThongTinPhieu(maPhieu, maDonVi);

            this.lstKetLuan.Clear();
            PSXN_TraKetQua TTTraKQ = BioNet_Bus.GetThongTinKetQuaXN(maPhieu, maTiepNhan);
            this.txtMaDonVi.Text = maDonVi;
            this.txtMaPhieu.Text = maPhieu;
            this.txtMaXetNghiem.Text = maXetNghiem;
            if (phieu != null)
            {
                try
                {
                    try
                    {
                        if (phieu.BenhNhan != null)
                        {
                            this.txtTenMe.Text = phieu.BenhNhan.MotherName;
                            this.txtSDTMe.Text = phieu.BenhNhan.MotherPhoneNumber;
                            this.txtTenCha.Text = phieu.BenhNhan.FatherName;
                            this.txtSDTCha.Text = phieu.BenhNhan.FatherPhoneNumber;
                            this.txtDiaChiGiaDinh.Text = phieu.BenhNhan.DiaChi;
                            this.txtTenTre.Text = phieu.BenhNhan.TenBenhNhan;
                            this.txtCanNang.Text = phieu.BenhNhan.CanNang.ToString();
                            this.txtNgaySinh.EditValue = phieu.BenhNhan.NgayGioSinh.Value.Date;
                            this.txtTuanTuoi.Text = phieu.BenhNhan.TuanTuoiKhiSinh.ToString();
                            this.txtMaTiepNhan.Text = maTiepNhan;
                            this.cbbGioiTinh.SelectedIndex = phieu.BenhNhan.GioiTinh ?? 2;
                        }
                    }
                    catch { }
                    this.lstChiTietKQ.Clear();
                    this.lstChiTietKQ = BioNet_Bus.GetDanhSachTraKQChiTiet(maTiepNhan, maPhieu);
                    this.txtMaPhieu.Text = phieu.maPhieu;
                    this.txtNgayLayMau.EditValue = phieu.ngayGioLayMau.Date;
                    this.txtNgayNhanMau.EditValue = phieu.ngayNhanMau;
                    this.txtDiaChiNoiLayMau.Text = phieu.DonVi.TenDVCS;
                    // this.cboTinhTrang.SelectedIndex = phieu.maTinhTrangLucLayMau;
                    this.txtMaPhieuLan1.Text = phieu.maPhieuLan1;
                    this.txtSDTNguoiLayMau.Text = phieu.DonVi.SDTCS;
                    if (phieu.isLayMauLan2 && !string.IsNullOrEmpty(phieu.maPhieuLan1))
                    {
                        var result = BioNet_Bus.GetDanhSachTraKetQuaChiTietPhieuCu(phieu.maPhieuLan1);
                        if (result.Count > 0)
                        {
                            this.lstChiTietKQCu = result;
                            this.TabThongTinChiTietKetQuaCu.PageVisible = true;
                            //this.LoadGCKetQuaChiTietCu();
                        }
                        else this.TabThongTinChiTietKetQuaCu.PageVisible = true;
                    }
                    else
                    {
                        this.TabThongTinChiTietKetQuaCu.PageVisible = false;
                    }

                   // this.loadGCKetQuaChiTiet();
                    //this.HienThiNoiDungLuuYTrenPhieu(phieu.isTruoc24h ?? false, phieu.isSinhNon ?? false, phieu.isNheCan ?? false, phieu.isGuiMauTre ?? false, maPhieu, maTiepNhan, TTTraKQ.GhiChuPhongXetNghiem, TTTraKQ.isDaDuyetKQ, phieu.LuuYPhieu);

                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("Lỗi khi lấy thông tin trả kết quả! \r\n Lỗi chi tiết :" + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            if (TTTraKQ != null)
            {
                if (!string.IsNullOrEmpty(TTTraKQ.KetLuanTongQuat.Trim()))
                {
                    try
                    {
                        try { this.txtKLBatThuong.Text = TTTraKQ.KetLuanTongQuat.Split('.')[1]; }
                        catch { this.txtKLBatThuong.Text = string.Empty; }
                        try
                        {
                            this.txtKLBinhThuong.Text = TTTraKQ.KetLuanTongQuat.Split('.')[0];
                        }
                        catch { this.txtKLBinhThuong.Text = string.Empty; }
                        this.txtGhiChu.Text = TTTraKQ.GhiChu;
                        //  this.btnLuu
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show("Lỗi khi lấy thông tin trả kết quả! \r\n Lỗi chi tiết :" + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    //this.CheckLaiKetLuan();
                }

                btnDuyetKQ.Enabled = !TTTraKQ.isDaDuyetKQ ?? false;
            }

        }

        private void groupControl3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}