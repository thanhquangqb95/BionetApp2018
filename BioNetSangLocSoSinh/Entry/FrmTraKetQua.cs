﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using BioNetModel;
using BioNetModel.Data;
using BioNetBLL;
using DevExpress.XtraGrid.Views.Grid;
using System.Diagnostics;
using BioNetSangLocSoSinh.Reports;
using System.IO;
using System.IO.Compression;
using DevExpress.Pdf;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DevExpress.XtraSplashScreen;

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmTraKetQua : DevExpress.XtraEditors.XtraForm
    {
        string pathtxt = Application.StartupPath + @"\DSPhieuChuaDongBo\dsPhieuChuaDongBo.txt";
        string path = Application.StartupPath + "\\DSPhieuChuaDongBo\\";
        bool isDaNhapLieu = true;
        public FrmTraKetQua(string maNvDangNhap)
        {
            InitializeComponent();
            this.MaNV = maNvDangNhap;
        }
        private bool Slide = false;
        private List<PSXN_TraKetQua> lstDaDuyetTraKetQua = new List<PSXN_TraKetQua>();
        //private List<PSXN_TraKetQua> lstChoTraKetQua = new List<PSXN_TraKetQua>();
        private List<PSXN_TTTraKQ> lstChoTraKetQua = new List<PSXN_TTTraKQ>();
        private List<PSXN_TTTraKQ> lstChoTraKetQuaXNLan2 = new List<PSXN_TTTraKQ>();
        private List<PSXN_TraKQ_ChiTiet> lstChiTietKQ = new List<PSXN_TraKQ_ChiTiet>();
        private List<PSXN_TraKQ_ChiTiet> lstChiTietKQCu = new List<PSXN_TraKQ_ChiTiet>();
        private List<KetLuan> lstKetLuan = new List<KetLuan>();
        private string MaNV = string.Empty;
        private bool isOK = true;
        private List<PSDanhMucGoiDichVuChung> lstgoiXN = new List<PSDanhMucGoiDichVuChung>();
        private void LoadGoiDichVuXetNGhiem()
        {
            try
            {
                this.lstgoiXN = BioNet_Bus.GetDanhsachGoiDichVuChung();
                this.LookUpGoiXN.DataSource = this.lstgoiXN;
                this.LookUpGoiXN_ChuaKQ.DataSource = this.lstgoiXN;
            }
            catch { }
        }
        private void FrmTraKetQua_Load(object sender, EventArgs e)
        {

            this.LoadFrm();
        }
        private void LoadFrm()
        {
            this.LoadsearchLookUpChiCuc();
            this.LoadRepositoryLookupDonViCoSo();
            // this.LoadSearchLookupDonVi();
            this.LoadGoiDichVuXetNGhiem();
            //this.searchLookUpDonViCoSo.EditValue = "ALL";
            this.txtTuNgay_ChuaKQ.EditValue = DateTime.Now;
            this.txtDenNgay_ChuaKQ.EditValue = DateTime.Now;
            this.LoadDanhSachChoTraKetQua();

        }

        private void LoadSearchLookupDonVi()
        {
            this.searchLookUpDonViCoSo.Properties.DataSource = null;
            this.searchLookUpDonViCoSo.Properties.DataSource = BioNet_Bus.GetDanhSachDonVi_Searchlookup();
        }
        private void LoadDanhSachChoTraKetQua()
        {
            DateTime tu = this.txtTuNgay_ChuaKQ.EditValue == null ? DateTime.Now.Date : (DateTime)this.txtTuNgay_ChuaKQ.EditValue;
            DateTime den = this.txtDenNgay_ChuaKQ.EditValue == null ? DateTime.Now.Date : (DateTime)this.txtDenNgay_ChuaKQ.EditValue;
            string machicuc = this.searchLookUpChiCuc.EditValue == null ? string.Empty : this.searchLookUpChiCuc.EditValue.ToString();
            string madv = this.searchLookUpDonViCoSo.EditValue == null ? string.Empty : this.searchLookUpDonViCoSo.EditValue.ToString();
            if (!machicuc.Equals("all") && madv.Equals("all"))
            {
                madv = machicuc;
            }
            this.lstChoTraKetQua.Clear();
            this.lstChoTraKetQua = BioNet_Bus.GetDanhSachChoTraKetQuaAll(tu, den, madv);
            this.LoadGCChuaCOKQ();
        }

        private void LoadDanhSachDaTraKetQua()
        {
            DateTime tu = this.txtTuNgay_ChuaKQ.EditValue == null ? DateTime.Now.Date : (DateTime)this.txtTuNgay_ChuaKQ.EditValue;
            DateTime den = this.txtDenNgay_ChuaKQ.EditValue == null ? DateTime.Now.Date : (DateTime)this.txtDenNgay_ChuaKQ.EditValue;
            string machicuc = this.searchLookUpChiCuc.EditValue == null ? string.Empty : this.searchLookUpChiCuc.EditValue.ToString();
            string madv = this.searchLookUpDonViCoSo.EditValue == null ? string.Empty : this.searchLookUpDonViCoSo.EditValue.ToString();
            if (!machicuc.Equals("all") && madv.Equals("all"))
            {
                madv = machicuc;
            }

            this.lstDaDuyetTraKetQua.Clear();
            this.lstDaDuyetTraKetQua = BioNet_Bus.GetDanhSachDaDuyetTraKetQua(tu, den, madv);
            this.LoadGCDaDuyetKQ();
        }
        private void LoadGCChuaCOKQ()
        {
            this.GCChuaKQ.DataSource = null;
            this.GCChuaKQ.DataSource = this.lstChoTraKetQua;
            this.GVChuaKQ.ExpandAllGroups();
        }

        private void LoadGCDaDuyetKQ()
        {
            this.GCDaTraKetQua.DataSource = null;
            this.GCDaTraKetQua.DataSource = this.lstDaDuyetTraKetQua;
            this.GVDaTraKetQua.ExpandAllGroups();
        }

        private void LoadNew()
        {
            this.txtMaPhieuLan1.ResetText();
            this.txtKLBinhThuong.ResetText();
            this.txtKLBatThuong.ResetText();
            this.txtGhiChu.ResetText();
            this.txtMaPhieu.ResetText();
            this.txtCanNang.ResetText();
            this.txtGioiTinh.ResetText();
            // this.txtCanNang.ResetText();
            // this.txtGioiTinh.ResetText();
            // this.txtGioSinhBenhNhan.ResetText();
            this.txtNamSinhBenhNhan.ResetText();
            // this.txtNamSinhCha.ResetText();
            // this.txtNamSinhMe.ResetText();
            // this.txtNgayTruyenMau.ResetText();
            // this.txtNoiSinh.ResetText();
            // this.txtSDTCha.ResetText();
            // this.txtSDTMe.ResetText();
            // this.txtSoLuongTruyenMau.ResetText();
            this.txtTenBenhNhan.ResetText();
            // this.txtTenCha.ResetText();
            this.txtTenMe.ResetText();
            //  this.RadioCheDoDD.EditValue = "0";
            // this.radioGroupGoiXN.EditValue = null;
            //this.radioGroupTinhTrangTre.EditValue = "0";
            //.radioGroupViTriLayMau.EditValue = "0";
            this.txtCanNang.ResetText();
            //this.txtGioiTinh.ResetText();
            //this.lookUpDanToc.ResetText();
            //this.lookupChuongTrinh.ResetText();
            //this.txtGioSinhBenhNhan.ResetText();
            //this.txtGioLayMau.ResetText();
            //this.txtDiaChiDonVi.ResetText();
            //this.txtNguoiLayMau.ResetText();
            //this.txtNoiLayMau.ResetText();
            //this.txtSDTNguoiLayMau.ResetText();
            this.txtNgayLayMau.ResetText();
            //this.txtGioSinhBenhNhan.ResetText();
            //this.txtGioLayMau.ResetText();
            //this.cboPhuongPhapSinh.ResetText();
            //this.checkedListBoxXN.Enabled = false;
            //this.LoadListDanhGiaSoBo();
            this.txtKLBinhThuong.ResetText();
            this.txtKLBatThuong.ResetText();
            this.txtLuuY.ResetText();
            this.txtGhiChu.ResetText();


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
        private void HienThiThongTinPhieu(string maPhieu, string maDonVi, string maTiepNhan, string maXetNghiem)
        {
            PsPhieu phieu = BioNet_Bus.GetThongTinPhieu(maPhieu, maDonVi);

            this.lstKetLuan.Clear();
            this.LoadNew();
            PSXN_TraKetQua TTTraKQ = BioNet_Bus.GetThongTinKetQuaXN(maPhieu, maTiepNhan);
            this.txtMadonVi.Text = maDonVi;
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
                            this.txtSDTBenhNhan.Text = phieu.BenhNhan.MotherPhoneNumber;
                            this.txtDiaChiBenhNhan.Text = phieu.BenhNhan.DiaChi;
                            this.txtTenBenhNhan.Text = phieu.BenhNhan.TenBenhNhan;
                            this.txtCanNang.Text = phieu.BenhNhan.CanNang.ToString();
                            this.txtNamSinhBenhNhan.EditValue = phieu.BenhNhan.NgayGioSinh.Value.Date;
                            this.txtTuanTuoi.Text = phieu.BenhNhan.TuanTuoiKhiSinh.ToString();
                            this.txtMaTiepNhan.Text = maTiepNhan;
                            this.txtGioiTinh.SelectedIndex = phieu.BenhNhan.GioiTinh ?? 2;
                        }
                    }
                    catch { }
                    this.lstChiTietKQ.Clear();
                    this.lstChiTietKQ = BioNet_Bus.GetDanhSachTraKQChiTiet(maTiepNhan, maPhieu);
                    this.txtMaPhieu.Text = phieu.maPhieu;
                    this.txtNgayLayMau.EditValue = phieu.ngayGioLayMau.Date;
                    this.txtNgayNhanMau.EditValue = phieu.ngayNhanMau;
                    this.txtTenDonVi.Text = phieu.DonVi.TenDVCS;
                    this.cboTinhTrang.SelectedIndex = phieu.maTinhTrangLucLayMau;
                    this.txtMaPhieuLan1.Text = phieu.maPhieuLan1;
                    this.txtSDTDonVi.Text = phieu.DonVi.SDTCS;
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
        private void LoadGCKetQuaChiTietCu()
        {
            //this.GCChiTietKQCu.DataSource = null;
            this.GCChiTietKQCu.DataSource = this.lstChiTietKQCu;
        }
        private void loadGCKetQuaChiTiet()
        {
            //this.GCChiTietKetQua.DataSource = null;
            this.GCChiTietKetQua.DataSource = this.lstChiTietKQ;
        }

        private void LoadRepositoryLookupDonViCoSo()
        {
            try
            {
                this.repositoryItemLookUpDonVi_GCDaTraKQ.DataSource = null;
                this.repositoryItemLookUpDonVi_GCChuaTraKQ.DataSource = null;
                this.repositoryItemLookUpDonVi_GCChuaTraKQ.DataSource = BioNet_Bus.GetDanhSachDonViCoSo();
                this.repositoryItemLookUpDonVi_GCDaTraKQ.DataSource = BioNet_Bus.GetDanhSachDonViCoSo();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi khi lấy danh sách đơn vị cơ sở \r\n Lỗi chi tiết :" + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void ThuMauLai()
        {
            if (BioNet_Bus.KiemTraMauDaLamLaiXetNghiemLan2(this.txtMaPhieu.Text.Trim()))
            {
                var res = BioNet_Bus.UpdatePhieuThucHienThuMauLai(this.txtMaPhieu.Text.Trim(), this.txtMadonVi.EditValue.ToString());
                if (!res.Result)
                {
                    MessageBox.Show(res.StringError, "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void LamLaiXN()
        {
            List<PsDichVu> lstdvlamlai = new List<PsDichVu>();
            if (GVChiTietKQ.RowCount > 0)
            {
                for (int i = 0; i < GVChiTietKQ.RowCount; i++)
                {
                    try
                    {
                        string maDV = this.GVChiTietKQ.GetRowCellValue(i, this.col_MaDichVu).ToString();
                        bool isnguyco = this.GVChiTietKQ.GetRowCellValue(i, this.col_isNguyCoCao) == null ? false : (bool)this.GVChiTietKQ.GetRowCellValue(i, this.col_isNguyCoCao);
                        string KQ2 = this.GVChiTietKQ.GetRowCellValue(i, this.col_KQ2) == null ? string.Empty : this.GVChiTietKQ.GetRowCellValue(i, this.col_KQ2).ToString();
                        if (isnguyco && !string.IsNullOrEmpty(maDV))
                        {
                            PsDichVu dv = BioNet_Bus.GetThongTinDichVu(maDV, this.txtMadonVi.Text.Trim());
                            if (dv != null && string.IsNullOrEmpty(KQ2))
                            {
                                lstdvlamlai.Add(dv);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(KQ2))
                                    XtraMessageBox.Show("Mã dịch vụ " + maDV + " Đã làm lại xét nghiệm rồi.", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                else
                                    XtraMessageBox.Show("Mã dịch vụ " + maDV + " không đủ thông tin để thực hiện làm lại xét nghiệm.", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show("Lỗi khi lấy danh sách dịch vụ để làm xét nghiệm lại \r\n Lỗi chi tiết :" + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            if (lstdvlamlai.Count > 0)
            {
                if (BioNet_Bus.InsertLamLaiXetNghiem(this.txtMaPhieu.Text.Trim(), this.txtMaTiepNhan.Text.Trim(), this.MaNV, lstdvlamlai))
                {
                    BioNet_Bus.UpdateKetQuaLamLaiXetNghiemLan2(this.txtMaPhieu.Text.Trim(), this.txtMaTiepNhan.Text.Trim(), this.txtMadonVi.Text.Trim());
                    XtraMessageBox.Show("Chỉ định làm lại xét nghiệm thành công.", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    XtraMessageBox.Show("Chỉ định làm lại xét nghiệm KHÔNG thành công.", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void GVChuaKQ_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (e.Column == this.col_MaPhieu_GCChuaCoKQ)
                    {

                        if (this.GVChuaKQ.RowCount > 0)
                        {

                            if (this.GVChuaKQ.GetFocusedRow() != null)
                            {
                                string maPhieu = this.GVChuaKQ.GetRowCellValue(this.GVChuaKQ.FocusedRowHandle, this.col_MaPhieu_GCChuaCoKQ).ToString();
                                string maDonVi = this.GVChuaKQ.GetRowCellValue(this.GVChuaKQ.FocusedRowHandle, this.col_MaDonVi_GCChuaCoKQ).ToString();
                                string maTiepNhan = this.GVChuaKQ.GetRowCellValue(this.GVChuaKQ.FocusedRowHandle, this.col_MaTiepNhan_GCChuaTraKQ).ToString();
                                isDaNhapLieu = (bool)(this.GVChuaKQ.GetRowCellValue(this.GVChuaKQ.FocusedRowHandle, this.col_isDaNhapLieu) ?? false);
                                string maXetNghiem = this.GVChuaKQ.GetRowCellValue(this.GVChuaKQ.FocusedRowHandle, this.col_maXn_GCChuaKQ).ToString();
                                this.btnLuuNhieuPDF.Enabled = false;
                                this.btnDuyetKQ.Enabled = true;
                                this.btnDuyetNhieuPhieu.Enabled = true;
                                this.btnHuyMau.Enabled = true;
                                this.btnInKQ.Enabled = false;
                                
                                this.HienThiThongTinPhieu(maPhieu, maDonVi, maTiepNhan, maXetNghiem);
                                //this.HienThiKetLuanvaGhiChuAuto();
                            }
                        }
                    }
                }
            }
            catch { }
        }
        private bool KiemTraTruocKhiSuaKetQua()
        {
            try
            {
                return BioNet_Bus.KiemTraBenhNhanNguyCoCaoDaVaoDotChanDoanChua(this.txtMaTiepNhan.Text.Trim());
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show("Lỗi khi kiểm tra quyền được sửa kết quả! /r/n Lỗi chi tiết : " + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
        }
        private void LuuKetQua(bool isDuyet)
        {

            if (!isDaNhapLieu)
            {
                XtraMessageBox.Show("Phiếu :" + this.txtMaPhieu.Text + " Chưa có đầy đủ thông tin trẻ. Vui lòng nhập liệu trước khi duyệt kết quả!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            else
            {
                bool _isLamLai = false;
                bool _isNguyCo = false;
                TraKetQua_XetNghiem KQ = new TraKetQua_XetNghiem();
                List<PSXN_TraKQ_ChiTiet> lstKQCT = new List<PSXN_TraKQ_ChiTiet>();
                KQ.ghiChu = this.txtGhiChu.Text.Trim();
                KQ.ketLuan = this.txtKLBinhThuong.Text.Trim() + "." + this.txtKLBatThuong.Text.Trim();
                KQ.maDonVi = this.txtMadonVi.Text;
                KQ.maPhieu = this.txtMaPhieu.Text;
                KQ.maTiepNhan = this.txtMaTiepNhan.Text;
                KQ.ngayDuyetKQ = DateTime.Now;
                KQ.userTraKQ = this.MaNV;
                KQ.MaPhieuLan1 = this.txtMaPhieuLan1.Text.Trim();
                #region Kết quả chi tiết
                if (this.GVChiTietKQ.RowCount > 0)
                {
                    for (int i = 0; i < this.GVChiTietKQ.RowCount; i++)
                    {
                        PSXN_TraKQ_ChiTiet cT = new PSXN_TraKQ_ChiTiet();
                        var IDThongso = this.GVChiTietKQ.GetRowCellValue(i, this.col_MaThongSo) == null ? string.Empty : this.GVChiTietKQ.GetRowCellValue(i, this.col_MaThongSo);
                        var valueGTCuoi = this.GVChiTietKQ.GetRowCellValue(i, this.col_KQCuoi) == null ? string.Empty : this.GVChiTietKQ.GetRowCellValue(i, this.col_KQCuoi);
                        var valueGT1 = this.GVChiTietKQ.GetRowCellValue(i, this.col_KQ1) == null ? string.Empty : this.GVChiTietKQ.GetRowCellValue(i, this.col_KQ1);
                        cT.GiaTri1 = valueGT1.ToString();
                        var valueGT2 = this.GVChiTietKQ.GetRowCellValue(i, this.col_KQ2) == null ? string.Empty : this.GVChiTietKQ.GetRowCellValue(i, this.col_KQ2);
                        cT.GiaTri2 = valueGT2.ToString();
                        bool nguyCo = this.GVChiTietKQ.GetRowCellValue(i, this.col_isNguyCoCao) == null ? false : (bool)this.GVChiTietKQ.GetRowCellValue(i, this.col_isNguyCoCao);
                        bool xnLai = this.GVChiTietKQ.GetRowCellValue(i, this.col_isMauXNLai) == null ? false : (bool)this.GVChiTietKQ.GetRowCellValue(i, this.col_isMauXNLai);
                        var ketLuan = this.GVChiTietKQ.GetRowCellValue(i, this.col_KetLuan) == null ? string.Empty : this.GVChiTietKQ.GetRowCellValue(i, this.col_KetLuan).ToString();                      
                        cT.GiaTriCuoi = valueGTCuoi.ToString();                                               
                        cT.MaPhieu = this.txtMaPhieu.Text;
                        cT.IDThongSoXN = IDThongso.ToString();
                        cT.MaTiepNhan = this.txtMaTiepNhan.Text;
                        cT.KetLuan = ketLuan;
                        cT.isMauXNLai = xnLai;
                        cT.isNguyCo = nguyCo;
                        lstKQCT.Add(cT);
                        if (nguyCo) _isNguyCo = true;
                        if (xnLai) _isLamLai = true;
                    }
                }
                #endregion
                KQ.chiTietKQ = lstKQCT;
                KQ.isNguyCo = _isNguyCo;
                KQ.isDaDuyet = true;
                KQ.ngayTraKQ = DateTime.Now;
                if (_isNguyCo && _isLamLai || !_isNguyCo)
                {
                    KQ.isTraKQ = true;
                }
                else
                {
                    KQ.isTraKQ = false;
                }
                PsReponse luuOK = new PsReponse();
                if (KQ.isTraKQ)
                {
                    luuOK = BioNet_Bus.LuuDuyetTraKetQuaXetNghiem(KQ);
                }
                else luuOK = BioNet_Bus.LuuTraKetQuaXetNghiem(KQ);
                #region Yêu cầu xét nghiệm lần 2 hoặc thu mẫu lại
                if (luuOK.Result)
                {
                    if (KQ.isTraKQ)
                    {
                        if (string.IsNullOrEmpty(KQ.MaPhieuLan1))
                        {
                            if (_isLamLai && _isNguyCo)
                            {
                                if (BioNet_Bus.KiemTraChoPhepThuMauLan2())
                                {
                                    this.ThuMauLai();
                                }
                               
                            }
                        }
                        this.btnDuyetKQ.Enabled = false;
                        this.btnInKQ.Enabled = true;
                        LuuPDF(KQ.maPhieu, KQ.maDonVi, KQ.maTiepNhan);
                        XtraMessageBox.Show("Đã trả kết quả của phiếu :" + this.txtMaPhieu.Text, "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        XtraMessageBox.Show("Đã duyệt kết quả hiện tại của phiếu :" + this.txtMaPhieu.Text, "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (_isNguyCo && !_isLamLai)
                        {
                            if (BioNet_Bus.KiemTraChoPhepLamLaiXetNghiemLan2())
                            {
                                if (!BioNet_Bus.KiemTraMauDaLamLaiXetNghiemLan2(this.txtMaPhieu.Text.Trim()))
                                {
                                    this.LamLaiXN();
                                }
                            }
                            else
                            {
                                BioNet_Bus.KoThuLaiMau(this.txtMaPhieu.Text.Trim(), this.txtMaTiepNhan.Text.Trim());
                            }
                        }                       
                    }
                    this.LoadDanhSachChoTraKetQua();

                }
                else
                {
                    XtraMessageBox.Show("Không thể lưu kết quả của phiếu " + this.txtMaPhieu.Text + " vì :" + luuOK.StringError.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                #endregion                
            }
        }
        private void KiemtraLaiGiaTriNguyCoTrenGirdview()
        {
            for (int i = 0; i < this.GVChiTietKQ.RowCount; i++)
            {
                var MaThongSo = this.GVChiTietKQ.GetRowCellValue(i, this.col_MaThongSo) == null ? string.Empty : this.GVChiTietKQ.GetRowCellValue(i, this.col_MaThongSo).ToString();
                var tenThongSo = this.GVChiTietKQ.GetRowCellValue(i, this.col_TenThongSo) == null ? string.Empty : this.GVChiTietKQ.GetRowCellValue(i, this.col_TenThongSo).ToString();
                var valueMin = this.GVChiTietKQ.GetRowCellValue(i, this.col_GiaTriMin) == null ? 0 : this.GVChiTietKQ.GetRowCellValue(i, this.col_GiaTriMin);
                var valueMax = this.GVChiTietKQ.GetRowCellValue(i, this.col_GiaTriMax) == null ? 0 : this.GVChiTietKQ.GetRowCellValue(i, this.col_GiaTriMax);
                var valueGT1 = this.GVChiTietKQ.GetRowCellValue(i, this.col_KQ1) == null ? string.Empty : this.GVChiTietKQ.GetRowCellValue(i, this.col_KQ1).ToString();
                var valueGT2 = this.GVChiTietKQ.GetRowCellValue(i, this.col_KQ2) == null ? string.Empty : this.GVChiTietKQ.GetRowCellValue(i, this.col_KQ2).ToString();
                var valueGTCuoi = this.GVChiTietKQ.GetRowCellValue(i, this.col_KQCuoi) == null ? string.Empty : this.GVChiTietKQ.GetRowCellValue(i, this.col_KQCuoi);
                bool nguyCo = this.GVChiTietKQ.GetRowCellValue(i, this.col_isNguyCoCao) == null ? false : (bool)this.GVChiTietKQ.GetRowCellValue(i, this.col_isNguyCoCao);
                var ketLuan = this.GVChiTietKQ.GetRowCellValue(i, this.col_KetLuan) == null ? string.Empty : this.GVChiTietKQ.GetRowCellValue(i, this.col_KetLuan).ToString();
                bool isThuLai = this.GVChiTietKQ.GetRowCellValue(i, this.col_isMauXNLai) == null ? false : (bool)this.GVChiTietKQ.GetRowCellValue(i, this.col_isMauXNLai);
                if (nguyCo)
                {
                    if (isThuLai)
                    {
                        var result = this.lstKetLuan.FirstOrDefault(p => p.maThongSo == MaThongSo);
                        if (result != null)
                        {
                            result.isNguyCo = true;
                            result.isThuLai = true;
                        }
                        else
                        {
                            KetLuan kl = new KetLuan();
                            kl.maThongSo = MaThongSo;
                            kl.tenThongSo = tenThongSo;
                            kl.isNguyCo = true;
                            kl.isThuLai = true;
                            this.lstKetLuan.Add(kl);
                        }
                    }
                    else
                    {

                        var result = this.lstKetLuan.FirstOrDefault(p => p.maThongSo == MaThongSo);
                        if (result != null)
                        {
                            result.isNguyCo = true;
                            result.isThuLai = false;
                        }
                        else
                        {
                            KetLuan kl = new KetLuan();
                            kl.maThongSo = MaThongSo;
                            kl.tenThongSo = tenThongSo;
                            kl.isNguyCo = true;
                            kl.isThuLai = false;
                            this.lstKetLuan.Add(kl);
                        }
                    }

                }
                else
                {

                    var result = this.lstKetLuan.FirstOrDefault(p => p.maThongSo == MaThongSo);
                    if (result != null)
                    {
                        result.isNguyCo = false;
                        result.isThuLai = false;
                    }
                    else
                    {
                        KetLuan kl = new KetLuan();
                        kl.maThongSo = MaThongSo;
                        kl.tenThongSo = tenThongSo;
                        kl.isNguyCo = false;
                        kl.isThuLai = false;
                        this.lstKetLuan.Add(kl);
                    }
                }

            }
            var th = HienThiKetLuanvaGhiChuAuto(this.txtMaPhieuLan1.Text.TrimEnd());
            this.txtKLBinhThuong.Text = th.KetLuanBT;
            this.txtKLBatThuong.Text = th.KetLuanNguyCoCao;
            this.txtGhiChu.Text = th.GhiChu;
        }
        private void GVChiTietKQ_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            var columnHandle = e.Column.ColumnHandle;
            var rowHandle = e.RowHandle;
            var MaThongSo = view.GetRowCellValue(rowHandle, this.col_MaThongSo) == null ? string.Empty : view.GetRowCellValue(rowHandle, this.col_MaThongSo).ToString();
            var tenThongSo = view.GetRowCellValue(rowHandle, this.col_TenThongSo) == null ? string.Empty : view.GetRowCellValue(rowHandle, this.col_TenThongSo).ToString();
            var valueMin = view.GetRowCellValue(rowHandle, this.col_GiaTriMin) == null ? 0 : view.GetRowCellValue(rowHandle, this.col_GiaTriMin);
            var valueMax = view.GetRowCellValue(rowHandle, this.col_GiaTriMax) == null ? 0 : view.GetRowCellValue(rowHandle, this.col_GiaTriMax);
            var valueGT1 = view.GetRowCellValue(rowHandle, this.col_KQ1) == null ? string.Empty : view.GetRowCellValue(rowHandle, this.col_KQ1).ToString();
            var valueGT2 = view.GetRowCellValue(rowHandle, this.col_KQ2) == null ? string.Empty : view.GetRowCellValue(rowHandle, this.col_KQ2).ToString();
            var valueGTCuoi = view.GetRowCellValue(rowHandle, this.col_KQCuoi) == null ? string.Empty : view.GetRowCellValue(rowHandle, this.col_KQCuoi);
            bool nguyCo = view.GetRowCellValue(rowHandle, this.col_isNguyCoCao) == null ? false : (bool)view.GetRowCellValue(rowHandle, this.col_isNguyCoCao);
            var ketLuan = view.GetRowCellValue(rowHandle, this.col_KetLuan) == null ? string.Empty : view.GetRowCellValue(rowHandle, this.col_KetLuan).ToString();
            bool isThuLai = view.GetRowCellValue(rowHandle, this.col_isMauXNLai) == null ? false : (bool)view.GetRowCellValue(rowHandle, this.col_isMauXNLai);
            if (columnHandle == this.col_KQ1.ColumnHandle)
            {
                if (!string.IsNullOrEmpty(valueGT1.Trim()))
                {
                    if (string.IsNullOrEmpty(valueGT2))
                    {
                        valueGTCuoi = valueGT1;
                    }

                    else
                    {
                        valueGTCuoi = (float.Parse(valueGT1.ToString()) + float.Parse(valueGT2.ToString())) / 2;
                    }
                    view.SetRowCellValue(rowHandle, this.col_KQCuoi, valueGTCuoi.ToString());
                    this.isOK = true;
                }
                else
                {
                    this.isOK = false;
                    XtraMessageBox.Show(" Vui lòng nhập đúng giá trị!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            if (columnHandle == this.col_KQ2.ColumnHandle)
            {
                if (!string.IsNullOrEmpty(valueGT2))
                {
                    if (string.IsNullOrEmpty(valueGT1))
                    {
                        XtraMessageBox.Show(" Kết quả lần 1 chưa có, vui lòng nhập kết quả lần 1 trước!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        view.SetRowCellValue(rowHandle, this.col_KQ2, "");
                    }
                    else valueGTCuoi = (float.Parse(valueGT1.ToString()) + float.Parse(valueGT2.ToString())) / 2;
                }
                else valueGTCuoi = float.Parse(valueGT1.ToString());
                view.SetRowCellValue(rowHandle, this.col_KQCuoi, valueGTCuoi.ToString());

            }
            if (columnHandle == this.col_KQCuoi.ColumnHandle)
            {
                if (!string.IsNullOrEmpty(valueGTCuoi.ToString()))
                {
                    if (string.IsNullOrEmpty(valueGT1))
                    {
                        XtraMessageBox.Show(" Kết quả lần 1 chưa có, vui lòng nhập kết quả lần 1 trước!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        view.SetRowCellValue(rowHandle, this.col_KQCuoi, "");
                    }
                    else
                    {
                        try
                        {
                            if (float.Parse(valueGTCuoi.ToString()) >= float.Parse(valueMin.ToString()) && float.Parse(valueGTCuoi.ToString()) < float.Parse(valueMax.ToString()))
                                view.SetRowCellValue(rowHandle, this.col_isNguyCoCao, false);

                            else view.SetRowCellValue(rowHandle, this.col_isNguyCoCao, true);
                        }
                        catch
                        {
                            XtraMessageBox.Show("Vui lòng nhập đúng giá trị", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            if (columnHandle == this.col_isNguyCoCao.ColumnHandle)
            {
                if (nguyCo)
                {
                    if (isThuLai)
                    {
                        view.SetRowCellValue(rowHandle, this.col_KetLuan, "Nguy cơ cao");
                        var result = this.lstKetLuan.FirstOrDefault(p => p.maThongSo == MaThongSo);
                        if (result != null)
                        {
                            result.isNguyCo = true;
                            result.isThuLai = true;
                        }
                        else
                        {
                            KetLuan kl = new KetLuan();
                            kl.maThongSo = MaThongSo;
                            kl.tenThongSo = tenThongSo;
                            kl.isNguyCo = true;
                            kl.isThuLai = true;
                            this.lstKetLuan.Add(kl);
                        }
                    }
                    else
                    {
                        view.SetRowCellValue(rowHandle, this.col_KetLuan, "Nghi ngờ");
                        var result = this.lstKetLuan.FirstOrDefault(p => p.maThongSo == MaThongSo);
                        if (result != null)
                        {
                            result.isNguyCo = true;
                            result.isThuLai = false;
                        }
                        else
                        {
                            KetLuan kl = new KetLuan();
                            kl.maThongSo = MaThongSo;
                            kl.tenThongSo = tenThongSo;
                            kl.isNguyCo = true;
                            kl.isThuLai = false;
                            this.lstKetLuan.Add(kl);
                        }
                    }

                }
                else
                {
                    view.SetRowCellValue(rowHandle, this.col_KetLuan, "Nguy cơ thấp");
                    var result = this.lstKetLuan.FirstOrDefault(p => p.maThongSo == MaThongSo);
                    if (result != null)
                    {
                        result.isNguyCo = false;
                        result.isThuLai = false;
                    }
                    else
                    {
                        KetLuan kl = new KetLuan();
                        kl.maThongSo = MaThongSo;
                        kl.tenThongSo = tenThongSo;
                        kl.isNguyCo = false;
                        kl.isThuLai = false;
                        this.lstKetLuan.Add(kl);
                    }
                }
                // this.KiemtraLaiGiaTriNguyCoTrenGirdview();
                this.CheckLaiKetLuan();
            }
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

        private void btnLuu_Click(object sender, EventArgs e)
        {
            this.LuuKetQua(false);
        }
        private void PrintKQ(string maDonVi, string maPhieu, string MaTiepNhan)
        {

            PsRptTraKetQuaSangLoc data = new PsRptTraKetQuaSangLoc();
            try
            {
                var donvi = BioNet_Bus.GetThongTinDonViCoSo(maDonVi.Trim());
                if (donvi != null)
                {
                    var kieuTraKQ = donvi.KieuTraKetQua ?? 1;
                    if (kieuTraKQ == 1)
                    {
                        data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(maPhieu, MaTiepNhan, "MaBsi", maDonVi);
                        Reports.rptPhieuTraKetQua datarp = new Reports.rptPhieuTraKetQua();
                        datarp.DataSource = data;
                        datarp.Print();
                    }
                    else
                    if (kieuTraKQ == 2)
                    {
                        data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(maPhieu, MaTiepNhan, "MaBsi", maDonVi);
                        Reports.rptPhieuTraKetQua_TheoTT2 datarp = new Reports.rptPhieuTraKetQua_TheoTT2();
                        datarp.DataSource = data;
                        datarp.Print();
                    }
                    else
                    if (kieuTraKQ == 5)
                    {
                        data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(maPhieu, MaTiepNhan, "MaBsi", maDonVi);
                        Reports.rptPhieuTraKetQua_TheoDonVi3 datarp = new Reports.rptPhieuTraKetQua_TheoDonVi3();
                        datarp.DataSource = data;
                        datarp.Print();
                    }
                    else
                    {
                        data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(maPhieu, MaTiepNhan, "MaBsi", maDonVi);
                        Reports.rptPhieuTraKetQua_TheoDonVi datarp = new Reports.rptPhieuTraKetQua_TheoDonVi();
                        datarp.DataSource = data;
                        datarp.Print();
                    }
                }
                else
                {
                    data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(maPhieu, MaTiepNhan, "MaBsi", maDonVi);
                    Reports.rptPhieuTraKetQua_TheoDonVi2 rp = new Reports.rptPhieuTraKetQua_TheoDonVi2();
                    rp.DataSource = data;
                    rp.Print();
                }
            }
            catch (Exception ex) { XtraMessageBox.Show("Lỗi phát sinh khi lấy dữ liệu in \r\n Lỗi chi tiết :" + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

        }
        private void btnInKQ_Click(object sender, EventArgs e)
        {
            //LuuPDF();
            PsRptTraKetQuaSangLoc data = new PsRptTraKetQuaSangLoc();
            try
            {
                var donvi = BioNet_Bus.GetThongTinDonViCoSo(this.txtMadonVi.Text.Trim());
                if (donvi != null)
                {
                    var kieuTraKQ = donvi.KieuTraKetQua ?? 1;
                    if (kieuTraKQ == 1) // Cần sửa chỗ này, cho chọn động loat report theo cấu hình của đơn vị
                    {
                        data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(txtMaPhieu.Text.Trim(), txtMaTiepNhan.Text.Trim(), "MaBsi", this.txtMadonVi.Text.Trim());
                        Reports.rptPhieuTraKetQua datarp = new Reports.rptPhieuTraKetQua();
                        datarp.DataSource = data;
                        string name = data.MaPhieu.ToString();
                        string madvcs = data.ThongTinDonVi.MaDonVi.ToString();
                        Reports.frmReportEditGeneral rept = new Reports.frmReportEditGeneral(datarp, "PhieuTraKetQua", name, madvcs);
                        rept.ShowDialog();
                    }
                    else
                    if (kieuTraKQ == 2)
                    {
                        data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(txtMaPhieu.Text.Trim(), txtMaTiepNhan.Text.Trim(), "MaBsi", this.txtMadonVi.Text.Trim());
                        Reports.rptPhieuTraKetQua_TheoTT2 datarp = new Reports.rptPhieuTraKetQua_TheoTT2();
                        datarp.DataSource = data;
                        string madvcs = data.ThongTinDonVi.MaDonVi.ToString();
                        string name = data.MaPhieu.ToString();
                        Reports.frmReportEditGeneral rept = new Reports.frmReportEditGeneral(datarp, "PhieuTraKetQua", name, madvcs);
                        rept.ShowDialog();
                    }
                    else if (kieuTraKQ == 3)
                    {
                        data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(txtMaPhieu.Text.Trim(), txtMaTiepNhan.Text.Trim(), "MaBsi", this.txtMadonVi.Text.Trim());
                        Reports.rptPhieuTraKetQua_TheoDonVi datarp = new Reports.rptPhieuTraKetQua_TheoDonVi();
                        datarp.DataSource = data;
                        string madvcs = data.ThongTinDonVi.MaDonVi.ToString();
                        string name = data.MaPhieu.ToString();
                        Reports.frmReportEditGeneral rept = new Reports.frmReportEditGeneral(datarp, "PhieuTraKetQua", name, madvcs);
                        rept.ShowDialog();
                    }
                    else if (kieuTraKQ == 4)
                    {
                        data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(txtMaPhieu.Text.Trim(), txtMaTiepNhan.Text.Trim(), "MaBsi", this.txtMadonVi.Text.Trim());
                        Reports.rptPhieuTraKetQua_TheoDonVi2 datarp = new Reports.rptPhieuTraKetQua_TheoDonVi2();
                        datarp.DataSource = data;
                        string madvcs = data.ThongTinDonVi.MaDonVi.ToString();
                        string name = data.MaPhieu.ToString();
                        Reports.frmReportEditGeneral rept = new Reports.frmReportEditGeneral(datarp, "PhieuTraKetQua", name, madvcs);
                        rept.ShowDialog();
                    }
                    else if (kieuTraKQ == 5)
                    {
                        data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(txtMaPhieu.Text.Trim(), txtMaTiepNhan.Text.Trim(), "MaBsi", this.txtMadonVi.Text.Trim());
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
                            data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(txtMaPhieu.Text.Trim(), txtMaTiepNhan.Text.Trim(), "MaBsi", this.txtMadonVi.Text.Trim());
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
                    data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(txtMaPhieu.Text.Trim(), txtMaTiepNhan.Text.Trim(), "MaBsi", this.txtMadonVi.Text.Trim());
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
        private void GVDaTraKetQua_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (e.Column == this.col_MaPhieu_GCDaTraKQ)
                {
                    if (this.GVDaTraKetQua.RowCount > 0)
                    {

                        if (this.GVDaTraKetQua.GetFocusedRow() != null)
                        {
                            this.col_KQ2.OptionsColumn.AllowEdit = false;
                            this.col_KQ1.OptionsColumn.AllowEdit = false;
                            this.col_KQCuoi.OptionsColumn.AllowEdit = false;
                            string maPhieu = this.GVDaTraKetQua.GetRowCellValue(this.GVDaTraKetQua.FocusedRowHandle, this.col_MaPhieu_GCDaTraKQ).ToString();
                            string maDonVi = this.GVDaTraKetQua.GetRowCellValue(this.GVDaTraKetQua.FocusedRowHandle, this.col_MaDonVi_DaTraKQ).ToString();
                            string maTiepNhan = string.Empty;
                            try { maTiepNhan = this.GVDaTraKetQua.GetRowCellValue(this.GVDaTraKetQua.FocusedRowHandle, this.col_MaTiepNhan_DaTraKQ).ToString(); }
                            catch { };
                            string maXetNghiem = string.Empty;
                            try
                            {
                                maXetNghiem = this.GVDaTraKetQua.GetRowCellValue(this.GVDaTraKetQua.FocusedRowHandle, this.col_MaXN_GCDaTraKQ).ToString();
                            }
                            catch { };
                            bool isTraKQ = (bool)(this.GVDaTraKetQua.GetRowCellValue(this.GVDaTraKetQua.FocusedRowHandle, this.col_isDaTraKQ_GCTraKQ) ?? false);
                            if (!isTraKQ)
                            {
                                
                                this.btnInKQ.Enabled = false;
                            }
                            else
                                this.btnInKQ.Enabled = true;
                            this.btnDuyetKQ.Enabled = false;
                            this.btnDuyetNhieuPhieu.Enabled = false;
                            this.btnLuuNhieuPDF.Enabled = true;
                            //  this.btnHuyDuyet.Enabled = true;
                            this.btnHuyMau.Enabled = false;                           
                            this.HienThiThongTinPhieu(maPhieu, maDonVi, maTiepNhan, maXetNghiem);
                        }
                    }
                }
            }
            catch { }
        }

        private void btnLamLaiXN_Click(object sender, EventArgs e)
        {
            this.LamLaiXN();
        }

        private void txtTuNgay_ChuaKQ_EditValueChanged(object sender, EventArgs e)
        {
            if (txtDenNgay_ChuaKQ.EditValue != null)
            {
                this.LoadDanhSachChoTraKetQua();
                this.LoadDanhSachDaTraKetQua();
            }
        }

        private void txtDenNgay_ChuaKQ_EditValueChanged(object sender, EventArgs e)
        {
            {
                this.LoadDanhSachChoTraKetQua();
                this.LoadDanhSachDaTraKetQua();
            }
        }

        private void searchLookUpDonViCoSo_EditValueChanged(object sender, EventArgs e)
        {
            if (this.txtTuNgay_ChuaKQ.EditValue != null && this.txtDenNgay_ChuaKQ.EditValue != null)
            {
                this.LoadDanhSachChoTraKetQua();
                this.LoadDanhSachDaTraKetQua();
            }
        }

        private void btnRefesh_Click(object sender, EventArgs e)
        {
            this.LoadDanhSachChoTraKetQua();
            this.LoadDanhSachDaTraKetQua();
        }

        private void btnDuyet_Click(object sender, EventArgs e)
        {
            this.LuuKetQua(true);
        }

        private void GVChiTietKQ_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void GCChiTietKetQua_EditorKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
        private bool KiemTraDieuKienTruocKhiLuu()
        {

            bool result = true;
            if (string.IsNullOrEmpty(this.txtTenMe.Text)) return false;
            if (string.IsNullOrEmpty(this.txtTenBenhNhan.Text)) return false;
            if (string.IsNullOrEmpty(this.txtTenDonVi.Text)) return false;
            if (string.IsNullOrEmpty(this.txtTuanTuoi.Text)) return false;
            // if (string.IsNullOrEmpty(this.txtNguoiLayMau.Text)) return false;
            return result;

        }
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                DevExpress.XtraTab.XtraTabControl tab = sender as DevExpress.XtraTab.XtraTabControl;
                if (tab.SelectedTabPageIndex == 1)
                {
                    this.LoadDanhSachDaTraKetQua();
                    this.btnInKQ.Enabled = true;
                    this.btnLuuNhieuPDF.Enabled = true;
                    this.btnDuyetNhieuPhieu.Enabled = false;
                }
                else
                {
                    this.btnInKQ.Enabled = false;
                    this.btnHuyMau.Enabled = true;
                    this.btnLuuNhieuPDF.Enabled = false;
                    this.btnDuyetNhieuPhieu.Enabled = true;
                    this.LoadDanhSachChoTraKetQua();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!this.KiemTraTruocKhiSuaKetQua())
            {
                this.col_KQCuoi.OptionsColumn.AllowEdit = true;
            }
            else
            {
                XtraMessageBox.Show("Bệnh nhân đã vào đợt chẩn đoán nên không được sửa!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.col_KQCuoi.OptionsColumn.AllowEdit = false;
            }
        }
        private void btnHuyDuyet_Click(object sender, EventArgs e)
        {
            var res = BioNet_Bus.HuyDuyetKQ(this.txtMaTiepNhan.Text, this.txtMadonVi.Text, this.txtMaPhieu.Text);
            if (res.Result)
            {
                XtraMessageBox.Show("Đã hủy duyệt phiếu, bạn có thể sửa kết quả và lưu lại", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                XtraMessageBox.Show(res.StringError, "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnDuyetKQ_EnabledChanged(object sender, EventArgs e)
        {
            try
            {
                SimpleButton btn = sender as SimpleButton;
                if (btn.Enabled)
                {
                    // btnHuyDuyet.Enabled = false;
                    btnHuyMau.Enabled = false;
                    btnInKQ.Enabled = false;
                }
                else
                {
                    //  btnHuyDuyet.Enabled = true;
                    btnHuyMau.Enabled = true;
                    btnInKQ.Enabled = true;
                }
            }
            catch { }
        }
        private void btnHuyMau_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtMaPhieu.Text.Trim()) && string.IsNullOrEmpty(this.txtMaTiepNhan.Text.Trim()) && string.IsNullOrEmpty(this.txtMadonVi.Text.Trim()))
            {
                XtraMessageBox.Show("Vui lòng chọn một mã phiếu trước khi thực hiện thao tác này", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(this.MaNV))
            {
                XtraMessageBox.Show("Không tìm thấy mã tài khoản của phiên đăng nhập này! \r\n Vui lòng đăng xuất và đăng nhập lại trước khi thực thiện thao tác này", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DiaglogFrm.FrmYeuCauMatKhauXacThuc frm = new DiaglogFrm.FrmYeuCauMatKhauXacThuc(this.txtMaPhieu.Text.Trim(),  this.txtMadonVi.Text.Trim(), this.MaNV);
            frm.ShowDialog();
            if (frm.DialogResult == DialogResult.OK)
            {
                this.LoadFrm();
            }

        }
        private void GVChuaKQ_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            try
            {
                GridView viewer = sender as GridView;
                int indexrow = e.ControllerRow;
                if (e.Action == CollectionChangeAction.Add || e.Action == CollectionChangeAction.Refresh)
                {
                    if (indexrow >= 0)
                    {
                        try
                        {
                            bool isNC = (bool)(viewer.GetRowCellValue(indexrow, this.col_isDaNhapLieu) ?? false);
                           
                            if (!isNC)
                            {
                                viewer.UnselectRow(indexrow);
                            }

                        }
                        catch (Exception ex) { }
                    }
                    else
                    {
                        if (indexrow == -2147483648)
                        {
                            
                            int[] lstChecked = viewer.GetSelectedRows();
                            foreach (var index in lstChecked)
                            {
                                if (index >= 0)
                                {
                                    try
                                    {
                                        bool isNC = (bool)(viewer.GetRowCellValue(index, this.col_isDaNhapLieu) ?? false);
                                        
                                        if (!isNC)
                                        {
                                            viewer.UnselectRow(index);                                          
                                        }

                                    }
                                    catch (Exception ex) { }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }


        }
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
        private void GVDaTraKetQua_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            try
            {
                GridView viewer = sender as GridView;
                int indexrow = e.ControllerRow;
                if (e.Action == CollectionChangeAction.Add || e.Action == CollectionChangeAction.Refresh)
                {
                    if (indexrow >= 0)
                    {
                        try
                        {
                            //    string MaTiepNhan = viewer.GetRowCellValue(indexrow, this.col_MaTiepNhan_DaTraKQ).ToString();
                            bool isDuyet = (bool)(viewer.GetRowCellValue(indexrow, this.col_isDaTraKQ_GCTraKQ) ?? false);
                            if (!isDuyet)
                            {
                                viewer.UnselectRow(indexrow);
                                this.btnInKQ.Enabled = false;
                            }
                        }
                        catch (Exception ex) { }
                    }
                    else
                    {
                        if (indexrow == -2147483648)
                        {
                            int[] lstChecked = viewer.GetSelectedRows();
                            foreach (var index in lstChecked)
                            {
                                if (index >= 0)
                                {
                                    try
                                    {
                                        // string MaTiepNhan = viewer.GetRowCellValue(index, this.col_MaTiepNhan_DaTraKQ).ToString();
                                        bool isDuyet = (bool)(viewer.GetRowCellValue(index, this.col_isDaTraKQ_GCTraKQ) ?? false);
                                        if (!isDuyet)
                                        {
                                            viewer.UnselectRow(index);
                                            this.btnInKQ.Enabled = false;
                                        }                                    }
                                    catch (Exception ex) { }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void GVChuaKQ_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            bool enable = false;
            if (e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell)
            {
                enable = true;
                popupMenuGVChuaKetQua.ItemLinks[0].Visible = enable;
                popupMenuGVChuaKetQua.ShowPopup(GCChuaKQ.PointToScreen(e.Point));
            }
            else
            {
                enable = false;
                popupMenuGVChuaKetQua.ItemLinks[0].Visible = enable;
                popupMenuGVChuaKetQua.ShowPopup(GCChuaKQ.PointToScreen(e.Point));
            }
        }
        private List<PSXN_TraKetQua> LayDanhSachDuyetHangLoat()
        {
            List<PSXN_TraKetQua> lst = new List<PSXN_TraKetQua>();
            try
            {
                int[] lstChecked = this.GVChuaKQ.GetSelectedRows();
                if (lstChecked.Count() > 0)
                    foreach (var index in lstChecked)
                    {
                        if (index >= 0)
                        {
                            try
                            {
                                PSXN_TraKetQua kq = new PSXN_TraKetQua();
                                string MaTiepNhan = this.GVChuaKQ.GetRowCellValue(index, this.col_MaTiepNhan_GCChuaTraKQ).ToString();
                                string MaPhieu = this.GVChuaKQ.GetRowCellValue(index, this.col_MaPhieu_GCChuaCoKQ).ToString();
                                string MaDonvi = this.GVChuaKQ.GetRowCellValue(index, this.col_MaDonVi_GCChuaCoKQ).ToString();
                                
                                kq.MaTiepNhan = MaTiepNhan;
                                kq.MaPhieu = MaPhieu;
                                kq.IDCoSo = MaDonvi;
                                kq.MaPhieuCu = BioNetBLL.BioNet_Bus.GetMaPhieuLan1(MaPhieu, MaDonvi);
                                lst.Add(kq);
                            }
                            catch (Exception ex) { }
                        }
                    }
            }
            catch { }
            return lst;
        }

        private List<PSXN_TraKetQua> LayDanhSachInHangLoat()
        {
            List<PSXN_TraKetQua> lst = new List<PSXN_TraKetQua>();
            try
            {
                int[] lstChecked = this.GVDaTraKetQua.GetSelectedRows();
                if (lstChecked.Count() > 0)
                    foreach (var index in lstChecked)
                    {
                        if (index >= 0)
                        {
                            try
                            {
                                PSXN_TraKetQua kq = new PSXN_TraKetQua();
                                string MaDonVi = this.GVDaTraKetQua.GetRowCellValue(index, this.col_MaDonVi_DaTraKQ).ToString();
                                string MaTiepNhan = this.GVDaTraKetQua.GetRowCellValue(index, this.col_MaTiepNhan_DaTraKQ).ToString();
                                string MaPhieu = this.GVDaTraKetQua.GetRowCellValue(index, this.col_MaPhieu_GCDaTraKQ).ToString();
                                kq.MaTiepNhan = MaTiepNhan;
                                kq.MaPhieu = MaPhieu;
                                kq.IDCoSo = MaDonVi;
                                lst.Add(kq);
                            }
                            catch (Exception ex) { }
                        }
                    }
            }
            catch { }
            return lst;
        }

        private void btnDuyetHangLoat_PopupMenu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (this.GVChuaKQ.RowCount > 0)
                {
                    List<PSXN_TraKetQua> lst = this.LayDanhSachDuyetHangLoat();
                    if (lst.Count > 0)
                    {
                        List<PsPhieuLoiKhiDanhGia> listphieuloi = new List<PsPhieuLoiKhiDanhGia>();
                        foreach (var kq in lst)
                        {
                            var result = BioNet_Bus.DuyetNhanh(kq.MaTiepNhan, kq.MaPhieu);
                            if (!result.Result)
                            {

                                PsPhieuLoiKhiDanhGia phieu = new PsPhieuLoiKhiDanhGia();
                                phieu.MaPhieu = kq.MaPhieu;
                                phieu.ThongTinLoi = result.StringError;
                                listphieuloi.Add(phieu);
                            }
                        }
                        if (listphieuloi.Count > 0)
                        {
                            DiaglogFrm.FrmDiagLogShowPhieuLoi frmloi = new DiaglogFrm.FrmDiagLogShowPhieuLoi(listphieuloi);
                            frmloi.ShowDialog();
                        }
                        else
                        {
                            XtraMessageBox.Show("Đã duyệt các phiếu thành công!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        this.LoadDanhSachChoTraKetQua();
                        this.LoadDanhSachDaTraKetQua();
                    }
                }
                else XtraMessageBox.Show("Vui lòng tick chọn các phiếu cần duyệt", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch { }

        }

        private void GVDaTraKetQua_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            bool enable = false;
            if (e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell)
            {
                enable = true;
                popupMenuGVDaCoKetQua.ItemLinks[0].Visible = enable;
                popupMenuGVDaCoKetQua.ShowPopup(GCChuaKQ.PointToScreen(e.Point));
            }
            else
            {
                enable = false;
                popupMenuGVDaCoKetQua.ItemLinks[0].Visible = enable;
                popupMenuGVDaCoKetQua.ShowPopup(GCChuaKQ.PointToScreen(e.Point));
            }
        }

        private void btnInNhieuPhieu__PopupMenu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (this.GVDaTraKetQua.RowCount > 0)
                {
                    List<PSXN_TraKetQua> lst = this.LayDanhSachInHangLoat();
                    if (lst.Count > 0)
                    {
                        foreach (var kq in lst)
                        {
                            this.PrintKQ(kq.IDCoSo, kq.MaPhieu, kq.MaTiepNhan);
                        }

                    }
                }
                else XtraMessageBox.Show("Vui lòng tick chọn các phiếu cần in", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch { }

        }

        //Lưu File PDF
        public static void LuuPDF(string MaPhieu, string MaDonVi,string MaTiepNhan)
        {
            PsRptTraKetQuaSangLoc data = new PsRptTraKetQuaSangLoc();
            try
            {
                var donvi = BioNet_Bus.GetThongTinDonViCoSo(MaDonVi);
                if (donvi != null)
                {
                    var kieuTraKQ = donvi.KieuTraKetQua ?? 1;
                    data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(MaPhieu, MaTiepNhan, "MaBsi", MaDonVi);
                    if (kieuTraKQ == 1) // Cần sửa chỗ này, cho chọn động loat report theo cấu hình của đơn vị
                    {
                        Reports.rptPhieuTraKetQua datarp = new Reports.rptPhieuTraKetQua();
                        frmReportEditGeneral.FileLuuPDF(datarp, data);
                    }
                    else
                    if (kieuTraKQ == 2)
                    {
                        Reports.rptPhieuTraKetQua_TheoTT2 datarp = new Reports.rptPhieuTraKetQua_TheoTT2();
                        frmReportEditGeneral.FileLuuPDF(datarp, data);
                    }
                    else
                    if (kieuTraKQ == 3)
                    {
                        Reports.rptPhieuTraKetQua_TheoDonVi datarp = new Reports.rptPhieuTraKetQua_TheoDonVi();
                        frmReportEditGeneral.FileLuuPDF(datarp, data);
                    }
                    else
                    if (kieuTraKQ == 4)
                    {
                        Reports.rptPhieuTraKetQua_TheoDonVi2 datarp = new Reports.rptPhieuTraKetQua_TheoDonVi2();
                        frmReportEditGeneral.FileLuuPDF(datarp, data);
                    }
                    else
                    if (kieuTraKQ == 5)
                    {
                        Reports.rptPhieuTraKetQua_TheoDonVi3 datarp = new Reports.rptPhieuTraKetQua_TheoDonVi3();
                        frmReportEditGeneral.FileLuuPDF(datarp, data);
                    }
                    else
                    {
                        Reports.rptPhieuTraKetQua datarp = new Reports.rptPhieuTraKetQua();
                        frmReportEditGeneral.FileLuuPDF(datarp, data);
                    }
                }
                else
                {
                    Reports.rptPhieuTraKetQua datarp = new Reports.rptPhieuTraKetQua();
                    frmReportEditGeneral.FileLuuPDF(datarp, data);
                }
            }
            catch (Exception ex) { XtraMessageBox.Show("Lỗi phát sinh khi lấy dữ liệu in \r\n Lỗi chi tiết :" + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

        }

        //Lưu file PDF vào thư mục   
        private void btnLuuNhieuPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.GVDaTraKetQua.RowCount > 0)
                {
                    List<PSXN_TraKetQua> lst = this.LayDanhSachInHangLoat();
                    if (lst.Count > 0)
                    {
                        bool loi = false;
                        foreach (var kq in lst)
                        {
                            try
                            {
                                LuuPDF(kq.MaPhieu, kq.IDCoSo, kq.MaTiepNhan);
                            }
                            catch (Exception ex)
                            {
                                XtraMessageBox.Show("Lỗi phát sinh khi lấy dữ liệu in \r\n Lỗi chi tiết :" + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                loi = true;
                                break;
                            }
                        }
                        if (loi == false)
                        {
                            MessageBox.Show("Lưu thành công: " + lst.Count + "phiếu ", "Thông Báo", MessageBoxButtons.OK);
                        }

                    }
                    else XtraMessageBox.Show("Vui lòng tick chọn các phiếu cần in", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else XtraMessageBox.Show("Vui lòng tick chọn các phiếu cần in", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi" + ex, "Thông Báo", MessageBoxButtons.OK);
            }
        }

  
        private void searchLookUpChiCuc_EditValueChanged(object sender, EventArgs e)
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

        private void LoadsearchLookUpChiCuc()
        {
            try
            {
                this.searchLookUpChiCuc.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
                this.searchLookUpChiCuc.EditValue = "all";
                this.searchLookUpDonViCoSo.EditValue = "all";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi khi load danh sách chi cục \r\n Lỗi chi tiết :" + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.searchLookUpChiCuc.Focus();
            }
        }

        private void labelControl37_Click(object sender, EventArgs e)
        {

        }

        private void GVChuaKQ_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                GridView View = sender as GridView;
                if (e.RowHandle >= 0)
                {
                    bool danhaplieu = View.GetRowCellDisplayText(e.RowHandle, col_isDaNhapLieu) == null ? false : (bool)View.GetRowCellValue(e.RowHandle, this.col_isDaNhapLieu);
                    if (!danhaplieu)
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

        private void GVDaTraKetQua_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                GridView View = sender as GridView;
                if (e.RowHandle >= 0)
                {
                    bool nguyngocao = View.GetRowCellDisplayText(e.RowHandle, col_NguyCoCao) == null ? false : (bool)View.GetRowCellValue(e.RowHandle, this.col_NguyCoCao);


                    if (nguyngocao)
                    {
                        e.Appearance.BackColor = Color.DarkOrange;
                        e.Appearance.BackColor2 = Color.Orange;
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

        private void btnDuyetNhieuPhieu_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.GVChuaKQ.RowCount > 0)
                {
                    
                    List<PSXN_TraKetQua> lst = this.LayDanhSachDuyetHangLoat();
                    List<PSXN_TraKetQua> lst2 = this.LayDanhSachDuyetHangLoat();
                    if (lst.Count > 0)
                    {
                        List<PsPhieuLoiKhiDanhGia> listphieuloi = new List<PsPhieuLoiKhiDanhGia>();
                        SplashScreenManager.ShowForm(this, typeof(DiaglogFrm.Waitingfrom), true, true, false);
                        foreach (var kq in lst)
                        {
                            lstKetLuan = new List<KetLuan>();
                            bool _isLamLai = false;
                            bool _isNguyCo = false;
                            TraKetQua_XetNghiem KQ = new TraKetQua_XetNghiem();
                            PSKetLuanPhieu klp = new PSKetLuanPhieu();
                            this.lstChiTietKQ = BioNet_Bus.GetDanhSachTraKQChiTiet(kq.MaTiepNhan, kq.MaPhieu);
                            KQ.maPhieu = kq.MaPhieu;
                            KQ.maDonVi = kq.IDCoSo;
                            KQ.MaPhieuLan1 = kq.MaPhieuCu;
                            KQ.maTiepNhan = kq.MaTiepNhan;
                            KQ.ngayDuyetKQ = DateTime.Now;
                            KQ.userTraKQ = this.MaNV;
                            KQ.ngayTraKQ = DateTime.Now;
                            if (this.lstChiTietKQ.Count > 0)
                            {
                                foreach (var ls in lstChiTietKQ)
                                {
                                    KetLuan kl = new KetLuan();
                                    ls.isDaDuyetKQ = true;
                                    if (ls.isMauXNLai == true) _isLamLai = true;
                                    kl.isNguyCo = ls.isNguyCo;
                                    kl.isThuLai = ls.isMauXNLai ?? false;
                                    kl.maThongSo = ls.IDThongSoXN;
                                    kl.tenThongSo = ls.TenThongSo;
                                    lstKetLuan.Add(kl);
                                    if (ls.isNguyCo) _isNguyCo = true;
                                }
                            }
                            klp = HienThiKetLuanvaGhiChuAuto(kq.MaPhieuCu);
                            KQ.isNguyCo = _isNguyCo;
                            KQ.ghiChu = klp.GhiChu;
                            KQ.ketLuan = klp.KetLuanBT + "." + klp.KetLuanNguyCoCao;
                            KQ.chiTietKQ = lstChiTietKQ;
                            if (_isLamLai && _isNguyCo || !_isNguyCo)
                            {
                                KQ.isTraKQ = true;
                            }  
                            else KQ.isTraKQ = false;
                            PsReponse luuOK = new PsReponse();
                          
                            
                                if (KQ.isTraKQ)
                            {
                                luuOK = BioNet_Bus.LuuDuyetTraKetQuaXetNghiem(KQ);
                            }
                            else luuOK = BioNet_Bus.LuuTraKetQuaXetNghiem(KQ);
                            #region Yêu cầu xét nghiệm lần 2 hoặc thu mẫu lại
                            if (luuOK.Result)
                            {
                                if (KQ.isTraKQ)
                                {
                                    if(string.IsNullOrEmpty(KQ.MaPhieuLan1))
                                    {
                                        if (_isLamLai && _isNguyCo)
                                        {
                                            if (BioNet_Bus.KiemTraChoPhepThuMauLan2())
                                            {
                                                this.ThuMauLaiXNDuyetNhanh(kq.MaPhieu, kq.IDCoSo);
                                            }
                                        }
                                    }                                   
                                }
                                else
                                {
                                    if (_isNguyCo && !_isLamLai)
                                    {
                                        if (BioNet_Bus.KiemTraChoPhepLamLaiXetNghiemLan2())
                                        {
                                            if (!BioNet_Bus.KiemTraMauDaLamLaiXetNghiemLan2(kq.MaPhieu))
                                            {
                                                this.LamLaiXNDuyetNhanh(kq.MaPhieu, kq.MaTiepNhan);
                                            }
                                        }
                                        else
                                        {
                                            BioNet_Bus.KoThuLaiMau(kq.MaPhieu, kq.MaTiepNhan);
                                        }
                                      
                                    }
                                }                                
                            }
                            else
                            {
                                PsPhieuLoiKhiDanhGia phieu = new PsPhieuLoiKhiDanhGia();
                                phieu.MaPhieu = kq.MaPhieu;
                                phieu.ThongTinLoi = luuOK.StringError;
                                listphieuloi.Add(phieu);
                                lst2.Remove(kq);
                            }
                            #endregion
                        }
                        InGopPhieuPDF(lst2);
                        SplashScreenManager.CloseForm();
                        if (listphieuloi.Count > 0)
                        {
                            DiaglogFrm.FrmDiagLogShowPhieuLoi frmloi = new DiaglogFrm.FrmDiagLogShowPhieuLoi(listphieuloi);
                            frmloi.ShowDialog();
                        }
                        else
                        {
                            XtraMessageBox.Show("Đã duyệt các phiếu thành công!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }                        
                    }
                    else XtraMessageBox.Show("Vui lòng tick chọn các phiếu trả kết quả", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else XtraMessageBox.Show("Vui lòng tick chọn các phiếu trả kết quả", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch { }
            this.LoadDanhSachChoTraKetQua();
            this.LoadDanhSachDaTraKetQua();
        }
        private void InGopPhieuPDF(List<PSXN_TraKetQua> listPhieu) //In phiếu PDF tổng hợp
        {
            try
            {
                if (this.GVChuaKQ.RowCount > 0)
                {
                    List<PSXN_TraKetQua> lst = listPhieu;
                    if (lst.Count > 0)
                    {
                        string pathDir = Application.StartupPath + "\\DSPhieuDuyetNhanh\\";
                        string pathpdf = string.Empty;
                        List<String> linkphieunhanh = new List<string>();
                        if (Directory.Exists(pathDir))
                        {
                            DirectoryInfo dirInfo = new DirectoryInfo(pathDir);
                            FileInfo[] childFiles = dirInfo.GetFiles();
                            pathpdf = pathDir + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + "." + (childFiles.Count() + 1).ToString() + ".pdf";
                        }
                        else
                        {
                            Directory.CreateDirectory(pathDir);
                            pathpdf = pathDir + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + ".1" + ".pdf";
                        }
                        foreach (var kq in lst)
                        {
                            LuuPDF(kq.MaPhieu, kq.IDCoSo, kq.MaTiepNhan);
                            string path = Application.StartupPath + "\\PhieuKetQua\\" + kq.IDCoSo + @"\" + kq.MaPhieu + ".pdf";
                            if (File.Exists(path))
                            {
                                linkphieunhanh.Add(path);
                            }

                        }
                        CombineMultiplePDFs(linkphieunhanh, pathpdf);
                        XtraMessageBox.Show("Xuất file PDF tổng hợp thành công ", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Xuất file PDF tổng hợp bị lỗi " + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private PsReponse LamLaiXNDuyetNhanh(string MaPhieu, string MaTiepNhan)
        {
            PsReponse res = new PsReponse();
            List<PsDichVu> lstdvlamlai = new List<PsDichVu>();
            PSXN_TraKetQua TTTraKQ = BioNet_Bus.GetThongTinKetQuaXN(MaPhieu, MaTiepNhan);
            List<PSXN_TraKQ_ChiTiet> CTTraKQ = BioNet_Bus.GetDanhSachTraKQChiTiet(MaTiepNhan, MaPhieu);
            try
            {
                if (TTTraKQ != null && CTTraKQ != null)
                {
                    if (CTTraKQ.Count > 0)
                    {
                        foreach (var ct in CTTraKQ)
                        {
                            PsDichVu dv = BioNet_Bus.GetThongTinDichVu(ct.MaDichVu, TTTraKQ.IDCoSo);
                            if (ct.isNguyCo && !String.IsNullOrEmpty(ct.MaDichVu))
                            {
                                if (dv != null && string.IsNullOrEmpty(ct.GiaTri2))
                                {
                                    lstdvlamlai.Add(dv);
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(ct.GiaTri2))
                                        res.Result = false;
                                    else
                                        res.Result = false;
                                }
                            }
                        }
                    }
                    if (lstdvlamlai.Count > 0)
                    {
                        if (BioNet_Bus.InsertLamLaiXetNghiem(TTTraKQ.MaPhieu, TTTraKQ.MaTiepNhan, this.MaNV, lstdvlamlai))
                        {
                            BioNet_Bus.UpdateKetQuaLamLaiXetNghiemLan2(TTTraKQ.MaPhieu, TTTraKQ.MaTiepNhan, TTTraKQ.IDCoSo);
                            res.Result = true;
                        }
                        else
                        {
                            res.Result = false;
                        }
                    }
                }
            }
            catch
            {
                res.Result = false;

            }
            return res;
        }

        private PsReponse ThuMauLaiXNDuyetNhanh(string MaPhieu, string MaDonVi)
        {
            PsReponse res = new PsReponse();
            try
            {
                if (BioNet_Bus.KiemTraMauDaLamLaiXetNghiemLan2(MaPhieu))
                {
                    res = BioNet_Bus.UpdatePhieuThucHienThuMauLai(MaPhieu, MaDonVi);
                }
                else
                    res.Result = false;
            }
            catch
            {

            }
            return res;
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
                    if(BioNet_Bus.KiemTraChoPhepLamLaiXetNghiemLan2())
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
                        if (kl.isNguyCo )
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

        public static void CombineMultiplePDFs(List<string> fileNames, string outFile)
        {
            Document document = new Document();
            PdfCopy writer = new PdfCopy(document, new FileStream(outFile, FileMode.Create));
            if (writer == null)
            {
                return;
            }
            document.Open();
            foreach (string fileName in fileNames)
            {

                PdfReader reader = new PdfReader(fileName);
                reader.ConsolidateNamedDestinations();


                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                    writer.AddPage(page);
                }

                PRAcroForm form = reader.AcroForm;
                if (form != null)
                {
                    writer.CopyDocumentFields(reader);
                }

                reader.Close();
            }
            writer.Close();
            document.Close();
        }

    }

}