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
using DevExpress.XtraGrid.Views.Grid;

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
                this.lstDanToc = BioNet_Bus.GetDanhSachDanToc(-1);
                this.repositoryItemLookUpDonVi_GCChuaTraKQ.DataSource = lstDonVi;
                this.repositoryItemLookUpDonVi_GCDaTraKQ.DataSource = lstDonVi;
                this.LookUpGoiXN.DataSource = lstgoiXN;
                this.LookUpGoiXN_ChuaKQ.DataSource = lstgoiXN;
                this.cbbGoiXN.Properties.DataSource = lstgoiXN;
                this.cbbDanToc.Properties.DataSource = lstDanToc;
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
      

        private void groupControl3_Paint(object sender, PaintEventArgs e)
        {

        }
        #region Hiện thị kết quả
        private void GVChuaKQ_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.GVChuaKQ.RowCount > 0)
                {
                    if (this.GVChuaKQ.GetFocusedRow() != null)
                    {
                        string maPhieu = this.GVChuaKQ.GetRowCellValue(this.GVChuaKQ.FocusedRowHandle, this.col_MaPhieu_GCChuaCoKQ).ToString();
                        string maDonVi = this.GVChuaKQ.GetRowCellValue(this.GVChuaKQ.FocusedRowHandle, this.col_MaDonVi_GCChuaCoKQ).ToString();
                        string maTiepNhan = this.GVChuaKQ.GetRowCellValue(this.GVChuaKQ.FocusedRowHandle, this.col_MaTiepNhan_GCChuaTraKQ).ToString();
                        string maXetNghiem = this.GVChuaKQ.GetRowCellValue(this.GVChuaKQ.FocusedRowHandle, this.col_maXn_GCChuaKQ).ToString();
                        this.HienThiThongTinPhieu(maPhieu, maDonVi,maTiepNhan,maXetNghiem);
                    }
                }
            }
            catch { }
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
                            this.cbbGioiTinh.SelectedIndex = phieu.BenhNhan.GioiTinh ?? 2;
                            this.txtNoiSinh.Text = phieu.BenhNhan.NoiSinh;
                            this.cbbCheDoDD.EditValue = phieu.maCheDoDinhDuong.ToString();
                            this.txtPara.Text = phieu.paRa;
                            this.cbbPhuongPhapSinh.EditValue = phieu.BenhNhan.PhuongPhapSinh.ToString();
                            this.cbbTTTre.EditValue = phieu.maTinhTrangLucLayMau.ToString();
                            this.cbbDanToc.EditValue = phieu.BenhNhan.DanTocID;
                        }
                    }
                    catch { }
                    this.txtTenDVCS.Text = phieu.DonVi.TenDVCS;
                    this.txtNoiLayMau.Text = phieu.NoiLayMau;
                    this.lstChiTietKQ.Clear();
                    this.lstChiTietKQ = BioNet_Bus.GetDanhSachTraKQChiTiet(maTiepNhan, maPhieu);
                    this.txtMaPhieu.Text = phieu.maPhieu;
                    this.txtNgayLayMau.EditValue = phieu.ngayGioLayMau.Date;
                    this.txtNgayNhanMau.EditValue = phieu.ngayNhanMau;
                    this.txtDiaChiNoiLayMau.Text = phieu.DiaChiLayMau;
                    this.txtNguoiLayMau.Text = phieu.TenNhanVienLayMau;
                    this.txtSDTNguoiLayMau.Text = phieu.SoDTNhanVienLayMau;
                    this.cbbViTriLayMau.EditValue = phieu.idViTriLayMau.ToString();
                    this.cbbGoiXN.EditValue = phieu.maGoiXetNghiem;
                    // this.cboTinhTrang.SelectedIndex = phieu.maTinhTrangLucLayMau;
                    this.txtMaPhieuLan1.Text = phieu.maPhieuLan1;
                    this.txtSDTNguoiLayMau.Text = phieu.DonVi.SDTCS;
                    var kq= BioNet_Bus.GetDuLieuInKetQuaSangLoc(phieu.maPhieu, maTiepNhan,emp.EmployeeCode, maDonVi);
                    if(kq!=null)
                    {
                        this.txtNgayCoKQ.EditValue = kq.NgayCoKQ;
                        this.txtNgayTraKQ.EditValue = kq.NgayTraKQ;
                        this.txtNgayXN.EditValue = kq.NgayXetNghiem;
                    }
                    if (phieu.isLayMauLan2 && !string.IsNullOrEmpty(phieu.maPhieuLan1))
                    {
                        var result = BioNet_Bus.GetDanhSachTraKetQuaChiTietPhieuCu(phieu.maPhieuLan1);
                        if (result.Count > 0)
                        {
                            this.lstChiTietKQCu = result;
                            this.TabThongTinChiTietKetQuaCu.PageVisible = true;
                            this.LoadGCKetQuaChiTietCu();
                        }
                        else this.TabThongTinChiTietKetQuaCu.PageVisible = true;
                    }
                    else
                    {
                        this.TabThongTinChiTietKetQuaCu.PageVisible = false;
                    }

                    this.loadGCKetQuaChiTiet();
                    this.HienThiNoiDungLuuYTrenPhieu(phieu.isTruoc24h ?? false, phieu.isSinhNon ?? false, phieu.isNheCan ?? false, phieu.isGuiMauTre ?? false, maPhieu, maTiepNhan, TTTraKQ.GhiChuPhongXetNghiem, TTTraKQ.isDaDuyetKQ, phieu.LuuYPhieu);

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
                    this.CheckLaiKetLuan();
                }

                btnDuyetKQ.Enabled = !TTTraKQ.isDaDuyetKQ ?? false;
            }

        }
        private void HienThiNoiDungLuuYTrenPhieu(bool isLayMauTruoc24h, bool isTreSinhNon, bool isTreNheCan, bool isGuiMauTre, string maPhieu, string maTiepNhan, string GhiChuXN, bool? isDuyetKQ, string LuuYPhieu)
        {
            isDuyetKQ = isDuyetKQ ?? false;
            string thongtinluuY = string.Empty;
            string strDanhGia = string.Empty;
            bool XNl2 = false;
            bool ThuMau = false;
            string mXN = this.txtMaXetNghiem.Text.Trim();
            if (!string.IsNullOrEmpty(mXN))
            {
                if (mXN.Length > 3)
                {
                    if (mXN.Substring(mXN.Length - 2, 2) == "L2")
                    {
                        XNl2 = true;
                    }
                    else XNl2 = false;
                }
                else
                    XNl2 = false;
            }
            else
                XNl2 = false;

            if (string.IsNullOrEmpty(this.txtMaPhieuLan1.Text.Trim()))
                ThuMau = false;
            else ThuMau = true;
            if (isDuyetKQ == false)
            {
                if (XNl2 && ThuMau)
                {
                    thongtinluuY += "Mẫu thu lại đã làm xét nghiệm lần 2 \r\n";
                }
                else if (!ThuMau && XNl2)
                    thongtinluuY += "Mẫu đã làm xét nghiệm lần 2\r\n";
                else if (ThuMau && !XNl2) thongtinluuY += " Mẫu thu lại đã làm xét nghiệm lần 1 \r\n";
            }
            else
            {
                if (ThuMau)
                {
                    thongtinluuY += "Mẫu thu lại";
                }
            }
            if (isLayMauTruoc24h)
            {
                thongtinluuY += "- Thu mẫu sớm \r\n";
            }
            if (isTreNheCan)
            {
                thongtinluuY += "- Trẻ nhẹ cân \r\n";
            }
            if (isTreSinhNon)
            {
                thongtinluuY += "- Trẻ sinh non \r\n";
            }
            if (isGuiMauTre)
            {
                thongtinluuY += "- Gửi mẫu trễ \r\n";
            }
            try
            {
                List<PSChiTietDanhGiaChatLuong> lstDanhGia = BioNet_Bus.GetChiTietDanhGiaMạuKhongDatTrenPhieu(maPhieu, maTiepNhan);
                if (lstDanhGia.Count > 0)
                {
                    strDanhGia = "- Mẫu không đạt : \r\n\t";
                    foreach (var ld in lstDanhGia)
                    {
                        strDanhGia += "+ " + ld.TenLyDo + "\r\n\t";
                    }
                }
            }
            catch { }
            thongtinluuY += strDanhGia;
            if (!string.IsNullOrEmpty(GhiChuXN))
            {
                thongtinluuY += "\r\n" + GhiChuXN;
            }

            this.txtLuuY.Text = thongtinluuY + "\r\n Ghi Chú:" + LuuYPhieu;
        }
        private void CheckLaiKetLuan()
        {
            bool ln1 = false;
            for (int i = 0; i < this.GVChiTietKQ.RowCount; i++)
            {
                var MaThongSo = this.GVChiTietKQ.GetRowCellValue(i, this.col_MaThongSo) == null ? string.Empty : this.GVChiTietKQ.GetRowCellValue(i, this.col_MaThongSo).ToString();
                var tenThongSo = this.GVChiTietKQ.GetRowCellValue(i, this.col_TenThongSo) == null ? string.Empty : this.GVChiTietKQ.GetRowCellValue(i, this.col_TenThongSo).ToString();
                bool nguyCo = (bool)this.GVChiTietKQ.GetRowCellValue(i, this.col_isNguyCoCao);
                bool isThuLai = this.GVChiTietKQ.GetRowCellValue(i, this.col_isMauXNLai) == null ? false : (bool)this.GVChiTietKQ.GetRowCellValue(i, this.col_isMauXNLai);
                var result = this.lstKetLuan.FirstOrDefault(p => p.maThongSo == MaThongSo);

                if (result != null)
                {
                    result.isNguyCo = nguyCo;
                    result.isThuLai = isThuLai;

                }
                else
                {
                    KetLuan kl = new KetLuan();
                    kl.maThongSo = MaThongSo;
                    kl.tenThongSo = tenThongSo;
                    kl.isNguyCo = nguyCo;
                    kl.isThuLai = isThuLai;
                    this.lstKetLuan.Add(kl);
                }
                if (isThuLai)
                {
                    ln1 = true;
                }

            }
            if (ln1)
            {
                this.col_KQ2.OptionsColumn.AllowEdit = true;
                this.col_KQ1.OptionsColumn.AllowEdit = false;
            }
            else
            {
                this.col_KQ1.OptionsColumn.AllowEdit = true;
                this.col_KQ2.OptionsColumn.AllowEdit = false;
            }
            this.col_KQCuoi.OptionsColumn.AllowEdit = true;
            var th = HienThiKetLuanvaGhiChuAuto(this.txtMaPhieuLan1.Text.TrimEnd());
            this.txtKLBinhThuong.Text = th.KetLuanBT;
            this.txtKLBatThuong.Text = th.KetLuanNguyCoCao;
            this.txtGhiChu.Text = th.GhiChu;
        }
        private PSKetLuanPhieu HienThiKetLuanvaGhiChuAuto(string MaPhieuLan1)
        {
            PSKetLuanPhieu pkl = new PSKetLuanPhieu();
            try
            {
                this.txtKLBatThuong.ResetText();
                this.txtKLBinhThuong.ResetText();
                this.txtGhiChu.ResetText();
                this.lstKetLuan.OrderBy(p => p.isNguyCo).ThenBy(p => p.isThuLai).ToList();
                List<string> lstbt = new List<string>();
                List<string> lstnghingo = new List<string>();
                List<string> lstnguycocao = new List<string>();
                string strbt = string.Empty;
                string strnghingo = string.Empty;
                string strnguycocao = string.Empty;
                string strKLbt = string.Empty;
                string strKLNguyCo = string.Empty;
                string strGhiChu = string.Empty;
                foreach (var kl in this.lstKetLuan)
                {
                    if (BioNet_Bus.KiemTraChoPhepLamLaiXetNghiemLan2())
                    {
                        if (kl.isNguyCo && kl.isThuLai)
                        {
                            lstnguycocao.Add(kl.tenThongSo);
                        }
                        else if (kl.isNguyCo && !kl.isThuLai)
                        {
                            lstnghingo.Add(kl.tenThongSo);
                        }
                        else { lstbt.Add(kl.tenThongSo); }
                    }
                    else
                    {
                        if (kl.isNguyCo)
                        {
                            lstnguycocao.Add(kl.tenThongSo);
                        }
                        else { lstbt.Add(kl.tenThongSo); }
                    }

                }
                foreach (var item in lstbt)
                {
                    if (string.IsNullOrEmpty(strbt))
                    { strbt = item; }
                    else { strbt += ", " + item; }
                }
                foreach (var item in lstnghingo)
                {
                    if (string.IsNullOrEmpty(strnghingo))
                    { strnghingo = item; }
                    else { strnghingo += ", " + item; }
                }
                foreach (var item in lstnguycocao)
                {
                    if (string.IsNullOrEmpty(strnguycocao))
                    { strnguycocao = item; }
                    else { strnguycocao += ", " + item; }
                }

                if (!string.IsNullOrEmpty(strbt)) //Kết luận bình thường
                {
                    var KL = BioNet_Bus.GetThongTinHienThiGhiChu("KL1");
                    if (KL.isNoiDungDatTruoc ?? true)
                    {
                        if (string.IsNullOrEmpty(KL.ThongTinHienThiGhiChu))
                            strKLbt = "Trong giới hạn bình thường với chỉ số :" + strbt;
                        else
                            strKLbt = KL.ThongTinHienThiGhiChu + strbt;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(KL.ThongTinHienThiGhiChu))
                            strKLbt = strbt + " : Trong giới hạn bình thường";
                        else strKLbt = strbt + KL.ThongTinHienThiGhiChu;
                    }
                }

                if (!string.IsNullOrEmpty(strnghingo))//Kết luận nghi nhờ
                {

                    var KL = BioNet_Bus.GetThongTinHienThiGhiChu("KL3");
                    if (KL.isNoiDungDatTruoc ?? true)
                    {
                        if (string.IsNullOrEmpty(KL.ThongTinHienThiGhiChu))
                            strKLNguyCo = "Mẫu nghi ngờ :" + strnghingo;
                        else strKLNguyCo = KL.ThongTinHienThiGhiChu + strnghingo;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(KL.ThongTinHienThiGhiChu))
                            strKLNguyCo = strnghingo + " : Nghi ngờ";
                        else strKLNguyCo = strnghingo + KL.ThongTinHienThiGhiChu;
                    }
                    var ghichu = BioNet_Bus.GetThongTinHienThiGhiChu("NN1");
                    if (ghichu.isNoiDungDatTruoc ?? true)
                    {
                        string noidung = ghichu.ThongTinHienThiGhiChu;
                        if (string.IsNullOrEmpty(ghichu.ThongTinHienThiGhiChu))
                            noidung = "Lấy mẫu xét nghiệm lần 2 cho :";
                        strGhiChu = noidung + strnghingo;
                    }
                    else
                    {
                        string noidung = ghichu.ThongTinHienThiGhiChu;
                        if (string.IsNullOrEmpty(ghichu.ThongTinHienThiGhiChu))
                            noidung = " : Cần làm xét nghiệm lần 2.";
                        strGhiChu = strnghingo + noidung;
                    }


                }

                if (!string.IsNullOrEmpty(strnguycocao))//Kết luận nguy cơ cao
                {

                    if (string.IsNullOrEmpty(strKLNguyCo))
                    {
                        var KL = BioNet_Bus.GetThongTinHienThiGhiChu("KL2");// Nếu nguy cơ cao
                        if (KL.isNoiDungDatTruoc ?? true)
                        {
                            if (string.IsNullOrEmpty(KL.ThongTinHienThiGhiChu))
                                strKLNguyCo = "Mẫu nguy cơ cao gồm :" + strnguycocao;
                            else strKLNguyCo = KL.ThongTinHienThiGhiChu + strnguycocao;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(KL.ThongTinHienThiGhiChu))
                                strKLNguyCo = strnguycocao + ": Nguy cơ cao.";
                            else strKLNguyCo = strnguycocao + KL.ThongTinHienThiGhiChu;
                        }
                    }
                    else
                    {
                        strKLNguyCo += ";" + strnguycocao + ": Nguy cơ cao";
                    }

                    if (string.IsNullOrEmpty(MaPhieuLan1))//nguy cơ cao phiếu mới
                    {
                        var ghichu = BioNet_Bus.GetThongTinHienThiGhiChu("NC1");
                        if (ghichu.isNoiDungDatTruoc ?? true)
                        {
                            string noidung = ghichu.ThongTinHienThiGhiChu;
                            if (string.IsNullOrEmpty(noidung))
                                noidung = " Đề nghị thu mẫu lại để thực hiện xét nghiệm : ";
                            strGhiChu = noidung + strnguycocao;
                        }
                        else
                        {
                            string noidung = ghichu.ThongTinHienThiGhiChu;
                            if (string.IsNullOrEmpty(noidung))
                                noidung = " : Cần thu mẫu lại để thực hiện xét nghiệm. ";
                            strGhiChu = strnguycocao + noidung;
                        }
                    }
                    else // Nguy cơ cao phiếu thu lại
                    {
                        var ghichu = BioNet_Bus.GetThongTinHienThiGhiChu("NC2");
                        if (ghichu.isNoiDungDatTruoc ?? true)
                        {
                            string noidung = ghichu.ThongTinHienThiGhiChu;
                            if (string.IsNullOrEmpty(noidung))
                                noidung = " Yêu cầu thực xét nghiệm chẩn đoán tại bệnh viện chuyên khoa đối với thông số : ";
                            strGhiChu = noidung + strnguycocao;
                        }
                        else
                        {
                            string noidung = ghichu.ThongTinHienThiGhiChu;
                            if (string.IsNullOrEmpty(noidung))
                                noidung = " : Cần được thực hiện xét nghiệm chẩn đoán tại bệnh viện chuyên khoa .";
                            strGhiChu = strnguycocao + noidung;
                        }
                    }
                }
                pkl.KetLuanBT = strKLbt;
                pkl.KetLuanNguyCoCao = strKLNguyCo;
                pkl.GhiChu = strGhiChu;

            }
            catch { }
            return pkl;
        }
        private void LoadGCKetQuaChiTietCu()
        {
            this.GCChiTietKQCu.DataSource = null;
            this.GCChiTietKQCu.DataSource = this.lstChiTietKQCu;
        }

        private void loadGCKetQuaChiTiet()
        {
            this.GCChiTietKetQua.DataSource = null;
            this.GCChiTietKetQua.DataSource = this.lstChiTietKQ;
        }
        #endregion

        private void txtMaXetNghiem_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txtMaPhieuLan1_DoubleClick(object sender, EventArgs e)
        {
            ViewTTCTPhieu(txtMaPhieuLan1.Text);
        }
        private void ViewTTCTPhieu(string MaPhieu)
        {
            try
            {
                Reports.rptPhieuViewTT datarp = new Reports.rptPhieuViewTT();
                if (!string.IsNullOrEmpty(MaPhieu))
                {
                    PsRptViewTT kq = BioNet_Bus.GetDuLieuViewTT(MaPhieu);
                    if (kq == null)
                    {
                        MessageBox.Show("Mã phiếu không tồn tại", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK);
                    }
                    else
                    {
                        datarp.DataSource = kq.chitietKetQua;
                        datarp.Parameters["TenTre"].Value = kq.TenTre;
                        datarp.Parameters["CanNang"].Value = kq.CanNang;
                        datarp.Parameters["NgaySinh"].Value = kq.NgaySinh;
                        datarp.Parameters["GioiTinh"].Value = kq.GioiTinh;
                        datarp.Parameters["CanNang"].Value = kq.CanNang;
                        datarp.Parameters["TuoiThai"].Value = kq.TuoiThai;
                        datarp.Parameters["TenMe"].Value = kq.TenMe;
                        datarp.Parameters["DienThoaiMe"].Value = kq.DienThoaiMe;
                        datarp.Parameters["TenCha"].Value = kq.TenCha;
                        datarp.Parameters["DienThoaiCha"].Value = kq.DienThoaiCha;
                        datarp.Parameters["TenDonVi"].Value = kq.TenDonVi;
                        datarp.Parameters["MaDonVi"].Value = kq.MaDonVi;
                        datarp.Parameters["DiaChiDonVi"].Value = kq.DiaChiDonVi;
                        datarp.Parameters["DiaChiTre"].Value = kq.DiaChiTre;
                        datarp.Parameters["MaPhieu"].Value = kq.MaPhieu;
                        datarp.Parameters["MaKhachHang"].Value = kq.MaKhachHang;
                        datarp.Parameters["MaXetNghiem"].Value = kq.MaXetNghiem;
                        datarp.Parameters["NgayThuMau"].Value = kq.NgayThuMau;
                        datarp.Parameters["NgayNhanMau"].Value = kq.NgayNhanMau;
                        datarp.Parameters["NgayXetNghiem"].Value = kq.NgayXetNghiem;
                        datarp.Parameters["NgayCoKQ"].Value = kq.NgayCoKQ;
                        datarp.Parameters["PPSinh"].Value = kq.PPSinh;
                        datarp.Parameters["Para"].Value = kq.Para;
                        datarp.Parameters["GoiXN"].Value = kq.GoiXN;
                        datarp.Parameters["CTSangLoc"].Value = kq.CTSangLoc;
                        datarp.Parameters["NguoiLayMau"].Value = kq.NguoiLayMau;
                        datarp.Parameters["LyDoKoDat"].Value = kq.LyDoKoDat;
                        datarp.Parameters["CLuongMau"].Value = kq.CLuongMau;
                        datarp.Parameters["GhiChu"].Value = kq.GhiChu;
                        datarp.Parameters["KetLuanNguyCoCao"].Value = kq.KetLuanNguyCoCao;
                        datarp.Parameters["KetLuanBinhThuong"].Value = kq.KetLuanBinhThuong;
                        datarp.Parameters["NSCha"].Value = "NS Cha: " + kq.NSCha;
                        datarp.Parameters["NSMe"].Value = "NS Mẹ: " + kq.NSMe;
                        datarp.CreateDocument(true);
                        Reports.frmDanhSachDaCapMa myForm = new Reports.frmDanhSachDaCapMa(datarp);
                        myForm.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Nhập mã phiếu cần tra cứu", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK);

                }
            }
            catch
            {
                MessageBox.Show("Lỗi hiện thị thông tin phiếu", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GVChiTietKQ_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            var rowHandle = e.RowHandle;

            if (rowHandle >= 0)
            {
                try
                {
                    var valueGT1 = view.GetRowCellValue(rowHandle, this.col_KQ1);
                    var valueGT2 = view.GetRowCellValue(rowHandle, this.col_KQ2) == null ? string.Empty : view.GetRowCellValue(rowHandle, this.col_KQ2).ToString();
                    var valueGTCuoi = view.GetRowCellValue(rowHandle, this.col_KQCuoi) == null ? string.Empty : view.GetRowCellValue(rowHandle, this.col_KQCuoi);
                    bool nguyCo = view.GetRowCellValue(rowHandle, this.col_isNguyCoCao) == null ? false : (bool)view.GetRowCellValue(rowHandle, this.col_isNguyCoCao);
                    var ketLuan = view.GetRowCellValue(rowHandle, this.col_KetLuan) == null ? string.Empty : view.GetRowCellValue(rowHandle, this.col_KetLuan).ToString();

                    if (string.IsNullOrEmpty(valueGTCuoi.ToString().Trim()))
                    {
                        if (string.IsNullOrEmpty(valueGT2.ToString()) && string.IsNullOrEmpty(valueGTCuoi.ToString())) valueGTCuoi = valueGT1;
                        else valueGTCuoi = (float.Parse(valueGT1.ToString()) + float.Parse(valueGT2.ToString())) / 2;
                        view.SetRowCellValue(rowHandle, this.col_KQCuoi, valueGTCuoi.ToString());
                    }
                    if (nguyCo)
                    {
                        e.Appearance.BackColor = Color.Salmon;
                        e.Appearance.BackColor2 = Color.SeaShell;
                    }
                    else
                    {
                        e.Appearance.BackColor = Color.White;
                    }
                }
                catch { }
            }
        }

        private void btnInKetQua_Click(object sender, EventArgs e)
        {
            PsRptTraKetQuaSangLoc data = new PsRptTraKetQuaSangLoc();
            try
            {
                var donvi = BioNet_Bus.GetThongTinDonViCoSo(this.txtMaDonVi.Text.Trim());
                if (donvi != null)
                {
                    var kieuTraKQ = donvi.KieuTraKetQua ?? 1;
                    if (kieuTraKQ == 1) // Cần sửa chỗ này, cho chọn động loat report theo cấu hình của đơn vị
                    {
                        data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(txtMaPhieu.Text.Trim(), txtMaTiepNhan.Text.Trim(), "MaBsi", this.txtMaDonVi.Text.Trim());
                        Reports.rptPhieuTraKetQua datarp = new Reports.rptPhieuTraKetQua();
                        // Reports.rptPhieuTraKetQuaNew datarp = new Reports.rptPhieuTraKetQuaNew();
                        // Reports.rptPhieuTraKetQua_TheoDonVi datarp = new Reports.rptPhieuTraKetQua_TheoDonVi();
                        datarp.DataSource = data;
                        string name = data.MaPhieu.ToString();
                        string madvcs = data.ThongTinDonVi.MaDonVi.ToString();
                        Reports.frmReportEditGeneral rept = new Reports.frmReportEditGeneral(datarp, "PhieuTraKetQua", name, madvcs);
                        rept.ShowDialog();
                    }
                    else
                    if (kieuTraKQ == 2)
                    {
                        data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(txtMaPhieu.Text.Trim(), txtMaTiepNhan.Text.Trim(), "MaBsi", this.txtMaDonVi.Text.Trim());
                        Reports.rptPhieuTraKetQua_TheoTT2 datarp = new Reports.rptPhieuTraKetQua_TheoTT2();
                        datarp.DataSource = data;
                        string madvcs = data.ThongTinDonVi.MaDonVi.ToString();
                        string name = data.MaPhieu.ToString();
                        Reports.frmReportEditGeneral rept = new Reports.frmReportEditGeneral(datarp, "PhieuTraKetQua", name, madvcs);
                        rept.ShowDialog();
                    }
                    else if (kieuTraKQ == 3)
                    {
                        data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(txtMaPhieu.Text.Trim(), txtMaTiepNhan.Text.Trim(), "MaBsi", this.txtMaDonVi.Text.Trim());
                        Reports.rptPhieuTraKetQua_TheoDonVi datarp = new Reports.rptPhieuTraKetQua_TheoDonVi();
                        datarp.DataSource = data;
                        string madvcs = data.ThongTinDonVi.MaDonVi.ToString();
                        string name = data.MaPhieu.ToString();
                        Reports.frmReportEditGeneral rept = new Reports.frmReportEditGeneral(datarp, "PhieuTraKetQua", name, madvcs);
                        rept.ShowDialog();
                    }
                    else if (kieuTraKQ == 4)
                    {
                        data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(txtMaPhieu.Text.Trim(), txtMaTiepNhan.Text.Trim(), "MaBsi", this.txtMaDonVi.Text.Trim());
                        Reports.rptPhieuTraKetQua_TheoDonVi2 datarp = new Reports.rptPhieuTraKetQua_TheoDonVi2();
                        datarp.DataSource = data;
                        string madvcs = data.ThongTinDonVi.MaDonVi.ToString();
                        string name = data.MaPhieu.ToString();
                        Reports.frmReportEditGeneral rept = new Reports.frmReportEditGeneral(datarp, "PhieuTraKetQua", name, madvcs);
                        rept.ShowDialog();
                    }
                    else if (kieuTraKQ == 5)
                    {
                        data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(txtMaPhieu.Text.Trim(), txtMaTiepNhan.Text.Trim(), "MaBsi", this.txtMaDonVi.Text.Trim());
                        Reports.rptPhieuTraKetQua_TheoDonVi3 datarp = new Reports.rptPhieuTraKetQua_TheoDonVi3();
                        datarp.DataSource = data;
                        string madvcs = data.ThongTinDonVi.MaDonVi.ToString();
                        string name = data.MaPhieu.ToString();
                        Reports.frmReportEditGeneral rept = new Reports.frmReportEditGeneral(datarp, "PhieuTraKetQua", name, madvcs);
                        rept.ShowDialog();
                    }
                    else
                    {
                        try
                        {
                            data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(txtMaPhieu.Text.Trim(), txtMaTiepNhan.Text.Trim(), "MaBsi", this.txtMaDonVi.Text.Trim());
                            Reports.rptPhieuTraKetQua datarp = new Reports.rptPhieuTraKetQua();
                            datarp.DataSource = data;
                            string name = data.MaPhieu.ToString();
                            string madvcs = data.ThongTinDonVi.MaDonVi.ToString();
                            Reports.frmReportEditGeneral rept = new Reports.frmReportEditGeneral(datarp, "PhieuTraKetQua", name, madvcs);
                            rept.ShowDialog();
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                }
                else
                {
                    data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(txtMaPhieu.Text.Trim(), txtMaTiepNhan.Text.Trim(), "MaBsi", this.txtMaDonVi.Text.Trim());
                    Reports.rptPhieuTraKetQua_TheoDonVi2 rp = new Reports.rptPhieuTraKetQua_TheoDonVi2();
                    rp.DataSource = data;

                    string name = data.MaPhieu.ToString();
                    string madvcs = data.ThongTinDonVi.MaDonVi.ToString();
                    Reports.frmReportEditGeneral rpt = new Reports.frmReportEditGeneral(rp, "PhieuTraKetQua", name, madvcs);
                    rpt.ShowDialog();
                }
            }
            catch (Exception ex) { XtraMessageBox.Show("Lỗi phát sinh khi lấy dữ liệu in \r\n Lỗi chi tiết :" + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void btnDuyetPhieu_Click(object sender, EventArgs e)
        {

        }
    }
}