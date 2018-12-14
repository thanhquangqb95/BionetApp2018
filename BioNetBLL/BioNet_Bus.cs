using BioNetDAL;
using BioNetModel.Data;
using System;
using System.Collections.Generic;
using BioNetModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using BioNetDAL.WebReference;

namespace BioNetBLL
{
    public class BioNet_Bus
    {
        #region SET
        public static PsReponse HuyMauPhieu(string maPhieu, string maTiepnhan, string maDonVi, string maNV, string lydoXoa)
        {
            var db = new DataObjects();
            return db.HuyMauPhieu(maPhieu, maTiepnhan, maDonVi, maNV, lydoXoa);
        }

        public static PsReponse UpdateDanhMucGhiChu(PSDanhMucGhiChu ghichu)
        {
            var db = new DataObjects();
            return db.UpdateDanhMucGhiChu(ghichu);
        }
        public static PsReponse UpdateThongTinTrungTam(PSThongTinTrungTam trungtam)
        {
            PsReponse resukt = new PsReponse();
            var db = new DataObjects();
            try
            {
                return db.UpdateThongTinTrungTam(trungtam);
            }
            catch (Exception ex)
            {
                resukt.Result = false;
                resukt.StringError = ex.ToString();
            }
            return resukt;
        }
        public static PsReponse InsertTiepNhan(PSTiepNhan dotTiepNhan)
        {
            var db = new DataObjects();
            return db.InsertTiepNhan(dotTiepNhan);
        }
        public static bool KiemTraMauDaLamLaiXetNghiemLan2DaVaoQuyTrinhXetNghieHayChua(string maPhieu, string maDonvi)
        {
            var db = new DataObjects();
            return db.KiemTraMauDaLamLaiXetNghiemLan2DaVaoQuyTrinhXetNghieHayChua(maPhieu, maDonvi);
        }

        public static PsReponse CapNhatLisence(string key)
        {
            var db = new DataObjects();
            return db.CapNhatLisence(key);
        }

        public static int KiemTraTrangThaiPhieu(string maPhieu, string maDonVi)
        {
            var db = new DataObjects();
            return db.KiemTraTrangThaiPhieu(maPhieu, maDonVi);
        }
        public static bool KiemTraMauDaLamLaiXetNghiemLan2(string maPhieu)
        {
            var db = new DataObjects();
            return db.KiemTraMauDaLamLaiXetNghiemLan2(maPhieu);
        }
        public static bool KiemTraBenhNhanNguyCoCaoDaVaoDotChanDoanChua(string maTiepNhan)
        {
            var db = new DataObjects();
            return db.KiemTraBenhNhanNguyCoCaoDaVaoDotChanDoanChua(maTiepNhan);
        }
        public static PsReponse LuuDuyetTraKetQuaXetNghiem(TraKetQua_XetNghiem ketQua)
        {
            var db = new DataObjects();
            return db.UpdateTraKetQua(ketQua, true);
        }
        public static PsReponse LuuTraKetQuaXetNghiem(TraKetQua_XetNghiem ketQua)
        {
            var db = new DataObjects();
            return db.UpdateTraKetQua(ketQua, false);
        }
        public static bool UpdateKetQuaLamLaiXetNghiemLan2(string maPhieu, string maTiepNhan, string maDonVi)
        {
            var db = new DataObjects();
            return db.UpdatePhieuTraKetQuaChoXNLan2(maPhieu, maTiepNhan, maDonVi);
        }

        public static List<PSXN_TraKetQua> GetDanhSachPDFChuaDongBo()
        {
            var db = new DataObjects();
            return db.GetDanhSachPDFChuaDongBo();
        }
        public static PsReponse UpdateDanhSachPDFChuaDongBo(List<string> Maphieu)
        {
            var db = new DataObjects();
            return db.UpdateDanhSachPDFChuaDongBo(Maphieu);
        }
        public static PsReponse BenhNhanNguyCoCao(long rowID)
        {
            var db = new DataObjects();
            return db.UpdateTrangThaiBenhNhanNguyCoCao(true, rowID);
        }
        public static PsReponse BenhNhanNguyCoGia(long rowID)
        {
            var db = new DataObjects();
            return db.UpdateTrangThaiBenhNhanNguyCoCao(false, rowID);
        }
        public static PsReponse UpdatePhieuThucHienThuMauLai(string maPhieu, string maDonvi)
        {
            var db = new DataObjects();
            return db.UpdatePhieuThucHienThuMauLai(maPhieu, maDonvi);
        }
        public static PsReponse InsertDotChanDoan(PSDotChuanDoan dotChanDoan)
        {
            var db = new DataObjects();
            return db.InsertDotChanDoan(dotChanDoan);
        }
        public static List<PSDanhMucThongSoXN> GetThongSoXN()
        {
            var db = new DataObjects();
            return db.GetDanhMucThongSoXN();
        }

        public static PsReponse DuyetNhanh(string maTiepNhan, string maPhieu)
        {
            var db = new DataObjects();
            return db.DuyetNhanh(maTiepNhan, maPhieu);
        }
        public static PsReponse KoThuLaiMau(string maPhieu, string maTiepNhan)
        {
            var db = new DataObjects();
            return db.KoThuLaiMau(maPhieu, maTiepNhan);
        }
        public static bool InsertLamLaiXetNghiem(string maPhieu, string maTiepNhan, string maNVChiDinh, List<PsDichVu> lstDVLamLai)
        {
            var db = new DataObjects();
            try
            {
                PSChiDinhDichVu cd = new PSChiDinhDichVu();

                var result = db.GetThongTinChiDinh(maPhieu, maTiepNhan);
                if (result != null)
                {
                    cd.IDGoiDichVu = "DVGXNL2";
                    cd.isLayMauLai = false;
                    cd.MaDonVi = result.MaDonVi;
                    cd.MaPhieu = maPhieu;
                    cd.MaTiepNhan = maTiepNhan;
                    cd.NgayChiDinhHienTai = DateTime.Now;
                    cd.NgayChiDinhLamViec = DateTime.Now;
                    cd.NgayTiepNhan = result.NgayTiepNhan;
                    cd.SoLuong = 1;
                    cd.TrangThai = 1;
                    cd.MaChiDinh = string.Empty;//
                    cd.MaNVChiDinh = maNVChiDinh;
                    cd.isDongBo = false;
                    cd.isXoa = false;
                }
                else
                {
                    return false;
                }
                string maCD = db.InsertDotChiDinhDichVu(cd, true);
                if (!string.IsNullOrEmpty(maCD))
                {
                    foreach (var item in lstDVLamLai)
                    {
                        PSChiDinhDichVuChiTiet dv = new PSChiDinhDichVuChiTiet();
                        dv.GiaTien = item.GiaDichVu;
                        dv.isXetNghiemLan2 = false;
                        dv.MaChiDinh = maCD;
                        dv.MaDichVu = item.IDDichVu;
                        dv.MaDonVi = cd.MaDonVi;
                        dv.MaGoiDichVu = "DVGXNL2";
                        dv.MaPhieu = maPhieu;
                        dv.SoLuong = 1;
                        dv.isXetNghiemLan2 = true;
                        dv.isXoa = false;
                        dv.isDongBo = false;
                        db.InsertDotChiDinhDichVuChiTiet(dv);
                    }
                    return true;
                }
                return false;

            }
            catch { return false; }
        }
        public static PsReponse UpdateGhiChuXetNghiem(string maKQ, string ghiChu)
        {
            var db = new DataObjects();
            return db.UpdateGhiChuXetNghiem(maKQ, ghiChu);
        }
        public static string GetGhiChuXetNghiem(string maKQ)
        {
            var db = new DataObjects();
            return db.GetGhiChuXetNghiem(maKQ);
        }
        //public static bool InsertListChiDinhHangLoat(List<PSTiepNhan> lstTiepNhan, string MaNVChiDinh)
        //{
        //    var db = new DataObjects();
        //    return db.InsertChiDinhHangLoat(lstTiepNhan, MaNVChiDinh);
        //}
        public static PsReponse InsertChiDinhTheoDanhSachHangLoat(PSTiepNhan dotTiepNhan, string MaNVChiDinh, string MaGoiXN)
        {
            var db = new DataObjects();
            return db.InsertChiDinhTheoDanhSachHangLoat(dotTiepNhan, MaNVChiDinh, MaGoiXN);
        }
        public static PsReponse UpdateThongTinPhieu_FrmNhapLieu(PsPhieu Phieu)
        {
            var db = new DataObjects();
            PsReponse result = new PsReponse();
            try
            {
                if (Phieu != null)
                {
                    if (Phieu.BenhNhan != null)
                    {
                        PSPatient patient = new PSPatient();
                        patient.MaBenhNhan = Phieu.BenhNhan.MaBenhNhan;
                        patient.FatherName = Phieu.BenhNhan.FatherName;
                        patient.FatherPhoneNumber = Phieu.BenhNhan.FatherPhoneNumber;
                        patient.FatherBirthday = Phieu.BenhNhan.FatherBirthday;
                        patient.MotherBirthday = Phieu.BenhNhan.MotherBirthday;
                        patient.MotherPhoneNumber = Phieu.BenhNhan.MotherPhoneNumber;
                        patient.MotherName = Phieu.BenhNhan.MotherName;
                        patient.MaKhachHang = Phieu.BenhNhan.MaKhachHang;
                        patient.DiaChi = Phieu.BenhNhan.DiaChi;
                        patient.TenBenhNhan = Phieu.BenhNhan.TenBenhNhan;
                        patient.QuocTichID = Phieu.BenhNhan.QuocTichID;
                        patient.TuanTuoiKhiSinh = Phieu.BenhNhan.TuanTuoiKhiSinh;
                        patient.PhuongPhapSinh = Phieu.BenhNhan.PhuongPhapSinh;
                        patient.NoiSinh = Phieu.BenhNhan.NoiSinh;
                        patient.NgayGioSinh = Phieu.BenhNhan.NgayGioSinh;
                        patient.GioiTinh = Phieu.BenhNhan.GioiTinh;
                        patient.DanTocID = Phieu.BenhNhan.DanTocID;
                        patient.CanNang = Phieu.BenhNhan.CanNang;
                        patient.Para = string.IsNullOrEmpty(Phieu.BenhNhan.Para) == true ? "0000" : Phieu.BenhNhan.Para;
                        string mbn = db.InsertBenhNhan(patient, Phieu.maDonViCoSo);
                        if (!string.IsNullOrEmpty(mbn))
                        {
                            PSPhieuSangLoc p = new PSPhieuSangLoc();
                            p.CheDoDinhDuong = Phieu.maCheDoDinhDuong;
                            p.IDChuongTrinh = Phieu.maChuongTrinh;
                            p.IDCoSo = Phieu.maDonViCoSo;
                            p.IDNhanVienLayMau = Phieu.maNVLayMau;
                            //p.IDNhanVienTaoPhieu = Phieu.maNVTaoPhieu;
                            p.IDPhieu = Phieu.maPhieu;
                            p.IDPhieuLan1 = Phieu.maPhieuLan1;
                            p.IDViTriLayMau = Phieu.idViTriLayMau;
                            p.isGuiMauTre = Phieu.isGuiMauTre;
                            p.isHuyMau = false;
                            p.isKhongDat = Phieu.isKhongDat;
                            p.isLayMauLan2 = Phieu.isLayMauLan2;
                            p.isNheCan = Phieu.isNheCan;
                            p.isSinhNon = Phieu.isSinhNon;
                            p.isTruoc24h = Phieu.isTruoc24h;
                            p.MaBenhNhan = mbn;
                            p.MaGoiXN = Phieu.maGoiXetNghiem;
                            //   p.MaXetNghiem = Phieu.maXetNghiem;
                            p.NgayGioLayMau = Phieu.ngayGioLayMau;
                            //  p.NgayNhanMau = Phieu.ngayNhanMau;
                            p.NgayTaoPhieu = Phieu.ngayTaoPhieu;
                            p.NgayTruyenMau = Phieu.ngayTruyenMau;
                            p.NoiLayMau = Phieu.NoiLayMau;
                            p.DiaChiLayMau = Phieu.DiaChiLayMau;
                            p.Para = Phieu.paRa;
                            p.SDTNhanVienLayMau = Phieu.SoDTNhanVienLayMau;
                            p.SLTruyenMau = Phieu.soLuongTruyenMau;
                            p.TenNhanVienLayMau = Phieu.TenNhanVienLayMau;
                            p.TinhTrangLucLayMau = Phieu.maTinhTrangLucLayMau;
                            p.TrangThaiMau = Phieu.trangThaiMau;
                            p.TrangThaiPhieu = true;
                            p.LyDoKhongDat = Phieu.lydokhongdat;
                            var res = db.InsertPhieu(p);
                            if (res.Result)
                            {
                                result.Result = true;
                            }
                            else
                            {
                                result.Result = false;
                                result.StringError = "Không thể cập nhật thông tin phiếu! \r\n Lỗi chi tiết :" + res.StringError;
                            }
                        }
                        else
                        {
                            result.Result = false;
                            result.StringError = "Không thể cập nhật thông tin bệnh nhân!";
                        }
                    }
                    else
                    {
                        result.Result = false;
                        result.StringError = "Dữ liệu đầu vào của bệnh nhân rỗng!";
                    }
                }
                else
                {
                    result.Result = false;
                    result.StringError = "Dữ liệu đầu vào của phiếu rỗng!";
                }
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.StringError = ex.ToString();
            }
            return result;
        }

        public static PsReponse InsertDotChiDinhDichVu(PsChiDinhvsDanhGia dg)
        {
            var db = new DataObjects();
            return db.InsertDotChiDinhDichVu(dg);
        }
        public static PsReponse UpdateThongTinPhieuLan1(string maphieu1)
        {
            var db = new DataObjects();
            return db.UpdateThongTinPhieuLan1(maphieu1);
        }
        public static PsReponse InsertMauDaDanhMaXN(PSXN_KetQua rowKQ)
        {
            var db = new DataObjects();
            return db.InsertKetQuaXN(rowKQ);
        }
        public static PsReponse HuyChiDinhDichVu(string maCD, string maNV, string lydoHuy, string maPhieu, string maTiepNhan)
        {
            var db = new DataObjects();
            return db.HuyChiDinhDichVu(maCD, maNV, lydoHuy, maPhieu, maTiepNhan);
        }
        public static PsReponse HuyPhieuDaTiepNhan(string maPhieu)
        {
            var db = new DataObjects();
            return db.HuyPhieuDaTiepNhan(maPhieu);
        }
        public static PsReponse LuuKetQuaXN(KetQua_XetNghiem KQ)
        {
            var db = new DataObjects();
            return db.LuuKetQuaXN(KQ);
        }

        public static PsReponse LuuKetQuaSuaXN(KetQua_XetNghiem KQ)
        {
            var db = new DataObjects();
            return db.LuuKetQuaSuaXN(KQ);
        }
        public static PsReponse HuyDuyetKQ(string maTiepNhan, string maDonVi, string maPhieu)
        {
            var db = new DataObjects();
            return db.UpdateDuyetKetQua(false, maTiepNhan, maDonVi, maPhieu);
        }

        #endregion SET
        #region GET
        public static List<PSDanhMucGhiChu> GetListCauHinhGhiChu()
        {
            var db = new DataObjects();
            return db.GetListCauHinhGhiChu();
        }
        public static PSThongTinTrungTam GetThongTinTrungTam()
        {
            var db = new DataObjects();
            return db.GetThongTinTrungTam();
        }
        public static PSDanhMucChiCuc GetThongTinChiCuc(string MaChiCuc)
        {
            var db = new DataObjects();
            return db.GetThongTinChiCuc(MaChiCuc);
        }
        public static List<PsTinhTrangPhieu> GetTinhTrangPhieu(DateTime startdate, DateTime enddate, string maDonVi)
        {
            var db = new DataObjects();
            return db.GetTinhTrangPhieu(startdate, enddate, maDonVi);
        }
        public static List<PsTinhTrangPhieu> GetTTPhieuCanSuaLoi(DateTime startdate, DateTime enddate, string maDonVi, string maChiCuc, string maPhieu)
        {
            var db = new DataObjects();
            return db.GetTTPhieuCanSuaLoi(startdate, enddate, maDonVi, maChiCuc, maPhieu);
        }
        public static PsReponse UpdatePhieuSuaLoi(List<string> maPhieu)
        {
            var db = new DataObjects();
            return db.UpdatePhieuSuaLoi(maPhieu);
        }
        public static PsReponse RestoreCTChatLuongMau()
        {
            var db = new DataObjects();
            return db.RestoreCTChatLuongMau();
        }
        public static List<PsCTDanhGiaChatLuong> GetCTChatLuogMau(string MaPhieu, string IDCoSo, string IDChiCuc, DateTime NgayBD, DateTime NgayKT)
        {
            var db = new DataObjects();
            return db.GetCTChatLuogMau(MaPhieu, IDCoSo, IDChiCuc, NgayBD, NgayKT);
        }
        public static PsReponse CapNhatGuiMail(List<String> MaPhieu, string MaDonVi, string EmailTT,string EmailDV, string NV)
        {
            var db = new DataObjects();
            return db.CapNhatGuiMail(MaPhieu, MaDonVi,EmailTT,EmailDV,NV);
        }
        public static List<PsTinhTrangPhieu> GetTinhTrangPhieuMail(DateTime startdate, DateTime enddate, string maDonVi, string maChiCuc)
        {
            var db = new DataObjects();
            return db.GetTinhTrangPhieuMail(startdate, enddate, maDonVi, maChiCuc);
        }
        public static int GetSLChuaDuocGuiMail()
        {
            var db = new DataObjects();
            return db.GetSLChuaDuocGuiMail();
        }
        public static PSTKKQPhieuMail GetThongKePhieuMail(string[] maphieu)
        {
            var db = new DataObjects();
            return db.GetThongKePhieuMail(maphieu);
        }
        public static List<PSDanhSachGuiSMS> GetDanhSachGuiSMS(DateTime TuNgay, DateTime DenNgay, int TrangThaiPhieu, int nguyco, string MaDV, string NoiDung, string DoiTuong, bool KieuKiTu, int TrangThaiGui,
            int SDT, string NoiDungGui, string MauGui, string HinhThuc, bool isSDT1)
        {
            var db = new DataObjects();
            return db.GetDanhSachGuiSMS(TuNgay, DenNgay, TrangThaiPhieu, nguyco, MaDV, NoiDung, DoiTuong, KieuKiTu, TrangThaiGui, SDT, NoiDungGui, MauGui, HinhThuc, isSDT1);
        }
        public static string GetLogSMS(string MaPhieu, string MaKhachHang, string SDT, string DV)
        {
            var db = new DataObjects();
            return db.GetLogSMS(MaPhieu, MaKhachHang,SDT,DV);
        }
            public static PsReponse InsertMauSMS(PSDanhMucMauSMS sms)
        {
            var db = new DataObjects();
            return db.InsertMauSMS(sms);
        }
        public static List<PSDanhMucMauSMS> LoadMauSMS(string HinhThucGuiTN, string DoituongNhanTinNhan, string Noidunggui)
        {
            var db = new DataObjects();
            return db.LoadMauSMS(HinhThucGuiTN, DoituongNhanTinNhan, Noidunggui);
        }
        public static List<PSDotChuanDoan> GetDanhSachDotChanDoanCuaBenhNhan(string MaBenhNhan)
        {
            var db = new DataObjects();
            return db.GetDanhSachDotChanDoanCuaBenhNhan(MaBenhNhan);
        }
        public static PSDotChuanDoan GetThongTinDotChanDoan(long rowID)
        {
            var db = new DataObjects();
            return db.GetThongTinDotChanDoan(rowID);
        }
        public static bool KiemTraKetQuaDaDuocDuyetHayChua(string maPhieu, string maTiepNhan)
        {
            var db = new DataObjects();
            return db.KiemTraPhieuDaDuyetHayChua(maPhieu, maTiepNhan);
        }
        public static List<PSBenhNhanNguyCoCao> GetDanhSachBenhNhanNguyCoCao(string madonvi, DateTime tungay, DateTime denngay)
        {
            var db = new DataObjects();
            if (madonvi.Equals("all") || string.IsNullOrEmpty(madonvi))
                madonvi = string.Empty;
            return db.GetDanhSachBenhNhanNguyCoCao(madonvi, true, tungay, denngay);
        }
        public static List<PSBenhNhanNguyCoCao> GetDanhSachBenhNhanNguyCoGia(string madonvi, DateTime tungay, DateTime denngay)
        {
            var db = new DataObjects();
            if (madonvi.Equals("all") || string.IsNullOrEmpty(madonvi))
                madonvi = string.Empty;
            return db.GetDanhSachBenhNhanNguyCoCao(madonvi, false, tungay, denngay);
        }
        public static string GetMaXNBangGhi()
        {
            var db = new DataObjects();
            var res = db.GetDuLieuXNBangGhi();
            if (string.IsNullOrEmpty(res))
            {
                return (SoBanDau() + "0000");
            }
            return res;
        }
        public static string GetMaXNTrongBangGhi()
        {
            var db = new DataObjects();
            var que = "MAXN" + SoBanDau();
            var res = db.GetDuLieuBangGhi(que);
            if (string.IsNullOrEmpty(res))
            {
                return (SoBanDau() + "0000");
            }
            else { return res; }
        }
        public static string GetMaXetNghiemTrongDB()
        {
            var db = new DataObjects();
            var res = db.GetDuLieuXNBangGhi();
            if (string.IsNullOrEmpty(res))
            {
                return (SoBanDau() + "0000");
            }
            else { return res; }
        }

        public static PsReponse UpdateMaXetNghiemTrongDB(string MaBD)
        {
            var db = new DataObjects();
            PsReponse res = db.UpdateDuLieuBangGhi(MaBD);
            return res;
        }

        public static PsReponse KiemTraMaXN(string MaXN)
        {
            var db = new DataObjects();
            PsReponse res = new PsReponse();
            res = db.KTMaXN(MaXN);
            return res;
        }
        public static string GetMaPNTrongBangGhi()
        {
            var db = new DataObjects();
            var que = "PN" + SoBanDau();
            var res = db.GetDuLieuBangGhi(que);
            if (string.IsNullOrEmpty(res))
            {
                return (SoBanDau() + "00000");
            }
            else { return res; }
        }

        public static PsReponse KiemTraThongTinPhieuDaDuocTiepNhan(string maPhieu)
        {
            var db = new DataObjects();
            PsReponse res = new PsReponse();
            return res = db.KiemTraThongTinPhieuDaDuocTiepNhan(maPhieu);
        }
        public static PSXN_TraKetQua GetThongTinKetQuaXN(string maPhieu, string maTiepNhan)
        {
            var db = new DataObjects();
            return db.GetThongTinTraKetQua(maPhieu, maTiepNhan);

        }
        public static string GetGhiChuPhongXetNghiem(string maKQ)
        {
            var db = new DataObjects();
            return db.GetGhiChuPhongXetNghiem(maKQ);
        }
        public static rptChiTietTrungTam GetBaoCaoTrungTamTongHopChiTietTheoDonVi(DateTime tuNgay, DateTime denNgay)
        {
            var db = new DataObjects();
            return db.GetBaoCaoTrungTamTongHopChiTietTheoDonVi(tuNgay, denNgay);
        }
        public static rptChiTietTrungTam GetBaoCaoTrungTamTongHopChiTietTheoChiCuc(DateTime tuNgay, DateTime denNgay)
        {
            var db = new DataObjects();
            return db.GetBaoCaoTrungTamTongHopChiTietTheoChiCuc(tuNgay, denNgay);
        }

        public static rptBaoCaoTongHop GetBaoCaoTongHopTrungTam(DateTime tuNgay, DateTime denNgay)
        {
            var db = new DataObjects();
            return db.GetBaoCaoTongHopTrungTam(tuNgay, denNgay);
        }
        public static TTPhieuCB GetBaoCaoThongTinPhieuTheoTime(string MaDonVi, DateTime NgayBD, DateTime NgayKT)
        {
            var db = new DataObjects();
            return db.GetBaoCaoThongTinPhieuTheoTime(MaDonVi, NgayBD, NgayKT);
        }
        public static PsReponse DeletePhieu(List<String> MaPhieu, bool XoaPhieu, string MaNV, string LydoXoa)
        {
            var db = new DataObjects();
            return db.DeletePhieu(MaPhieu, XoaPhieu, MaNV, LydoXoa);
        }
        public static rptBaoCaoTongHop GetBaoCaoTongHopChiCuc(DateTime tuNgay, DateTime denNgay, string maChiCuc)
        {
            var db = new DataObjects();
            return db.GetBaoCaoTongHopChiCuc(tuNgay, denNgay, maChiCuc);
        }
        public static List<PsBaoCaoTaiChinh> GetBaoCaoTaiChinh(string MaDonVi, DateTime NgayBD, DateTime NgayKT)
        {
            var db = new DataObjects();
            return db.GetBaoCaoTaiChinh(MaDonVi, NgayBD, NgayKT);
        }
        public static PsRptTraKetQuaSangLoc GetDuLieuInKetQuaSangLoc(string maPhieu, string maTiepNhan, string maNV, string maDonVi)
        {
            PsRptTraKetQuaSangLoc rptKQ = new PsRptTraKetQuaSangLoc();
            List<PsRPTTraKetQuaSangLocChiTiet> lstrptKQCT = new List<PsRPTTraKetQuaSangLocChiTiet>();
            var db = new DataObjects();
            var phieu = GetThongTinPhieu(maPhieu, maDonVi);
            var ttKQ = db.GetThongTinTraKetQua(maPhieu, maTiepNhan);
            var ttKQCT = db.GetThongTinTraKetQuaChiTiet(maPhieu, maTiepNhan);
            var Nv = db.GetThongTinNhanVien(maNV);
            var DtTTam = db.GetThongTinTrungTam();
            var TTDonvi = db.GetThongTinDonViCoSo(maDonVi);
            PsThongTinTrungTam TrungTam = new PsThongTinTrungTam();
            PsThongTinDonVi DonVi = new PsThongTinDonVi();
            if (ttKQ != null)
            {
                rptKQ.NgayNhanMau = (ttKQ.NgayTiepNhan ?? DateTime.Now).ToString("dd/MM/yyyy");
                rptKQ.MaXetNghiem = ttKQ.MaXetNghiem;
                rptKQ.NgayXetNghiem = (ttKQ.NgayLamXetNghiem ?? DateTime.Now).ToString("dd/MM/yyyy");
                rptKQ.MaPhieu = ttKQ.MaPhieu;
                rptKQ.MaDonVi = "Mã đơn vị: " + ttKQ.IDCoSo;
                rptKQ.KetLuanBinhThuong = ttKQ.KetLuanTongQuat.Split('.')[0];
                try { rptKQ.KetLuanNguyCoCao = ttKQ.KetLuanTongQuat.Split('.')[1]; } catch { rptKQ.KetLuanNguyCoCao = string.Empty; }
                rptKQ.GhiChu = ttKQ.GhiChu;
                rptKQ.Ngay = ttKQ.NgayTraKQ.Value.Day.ToString().PadLeft(2, '0');
                rptKQ.Thang = ttKQ.NgayTraKQ.Value.Month.ToString().PadLeft(2, '0');
                rptKQ.Nam = ttKQ.NgayTraKQ.Value.Year.ToString();
                rptKQ.NgayCoKQ = (ttKQ.NgayCoKQ ?? DateTime.Now).ToString("dd/MM/yyyy");
                rptKQ.NgayTraKQ = ttKQ.isTraKQ == true ? (ttKQ.NgayTraKQ ?? DateTime.Now).ToString("dd/MM/yyyy") : string.Empty;
            }
            else return null;
            if (phieu != null)
            {
                if (phieu.BenhNhan != null)
                {
                    rptKQ.TenCha = phieu.BenhNhan.FatherName;
                    rptKQ.TenMe = phieu.BenhNhan.MotherName;
                    rptKQ.DienThoaiMe = phieu.BenhNhan.MotherPhoneNumber;
                    rptKQ.DienThoaiCha = phieu.BenhNhan.FatherPhoneNumber;
                    rptKQ.DiaChiTre = "Địa chỉ gia đình: " + phieu.BenhNhan.DiaChi;
                    rptKQ.MaKhachHang = phieu.BenhNhan.MaKhachHang;
                    rptKQ.TenTre = phieu.BenhNhan.TenBenhNhan;
                    rptKQ.NgaySinh = (phieu.BenhNhan.NgayGioSinh ?? DateTime.Now).ToString("dd/MM/yyyy");
                    // rptKQ.GioiTinh = phieu.BenhNhan.GioiTinh == true ? "Nam" : "Nữ";
                    if (phieu.BenhNhan.GioiTinh == 0)
                    {
                        rptKQ.GioiTinh = "Nam";
                    }
                    else if (phieu.BenhNhan.GioiTinh == 1)
                        rptKQ.GioiTinh = "Nữ";
                    else rptKQ.GioiTinh = "N/a";
                    rptKQ.CanNang = phieu.BenhNhan.CanNang.ToString();
                    rptKQ.TuoiThai = phieu.BenhNhan.TuanTuoiKhiSinh.ToString();
                    rptKQ.NgayThuMau = phieu.ngayGioLayMau.ToString("dd/MM/yyyy");

                }
                if (phieu.DonVi != null)
                {
                    rptKQ.TenDonVi = "Tên đơn vị: " + phieu.DonVi.TenDVCS;
                    rptKQ.DiaChiDonVi = "Địa chỉ đơn vị: " + phieu.DonVi.DiaChiDVCS;
                }
                if (ttKQCT != null)
                {
                    foreach (var ct in ttKQCT)
                    {
                        PsRPTTraKetQuaSangLocChiTiet c = new PsRPTTraKetQuaSangLocChiTiet();
                        c.DonViDo = ct.DonViTinh.TrimEnd();
                        c.GiaTri = ct.GiaTriCuoi;
                        c.GiaTriTrungBinh = ct.GiaTriTrungBinh;
                        c.KetLuan = ct.KetLuan;
                        c.MaThongSo = ct.IDThongSoXN;
                        c.isNguyCoCao = ct.isNguyCo;
                        c.MaDiChVu = ct.MaDichVu;
                        try
                        {
                            c.TenDichVu = string.IsNullOrEmpty(db.GetTenDichVuCuaKyThuat(ct.IDKyThuat)) == true ? ct.TenKyThuat : db.GetTenDichVuCuaKyThuat(ct.IDKyThuat);
                        }
                        catch { c.TenDichVu = ct.TenKyThuat; }

                        c.TenThongSo = ct.TenThongSo;
                        lstrptKQCT.Add(c);
                    }
                }
                rptKQ.chitietKetQua = lstrptKQCT;

                rptKQ.ThongTinNhanVien = Nv;
                try
                {
                    TrungTam.DiaChi = DtTTam.Diachi;
                    TrungTam.DienThoai = DtTTam.DienThoai;
                    TrungTam.MaTrungTam = DtTTam.MaTrungTam;
                    TrungTam.MaVietTat = DtTTam.MaVietTat;
                    TrungTam.TenTrungTam = DtTTam.TenTrungTam;
                    if (DtTTam.Header.Length > 0)
                    {
                        try
                        {
                            byte[] b = DtTTam.Header.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.Hearder = img;
                        }
                        catch { }
                    }
                    if (DtTTam.ChuKiTT.Length > 0)
                    {
                        try
                        {
                            byte[] b = DtTTam.ChuKiTT.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.ChuKiTT = img;
                        }
                        catch { }
                    }
                    if (DtTTam.ChuKiXN.Length > 0)
                    {
                        try
                        {
                            byte[] b = DtTTam.ChuKiXN.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.ChuKiXN = img;
                        }
                        catch { }
                    }
                    if (DtTTam.Logo.Length > 0)
                    {

                        try
                        {
                            byte[] b = DtTTam.Logo.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.Logo = img;
                        }
                        catch { }
                    }
                }
                catch { }
                try
                {
                    DonVi.DiaChi = TTDonvi.DiaChiDVCS;
                    DonVi.isLocked = TTDonvi.isLocked ?? false;
                    DonVi.KieuTraKetQua = TTDonvi.KieuTraKetQua ?? 1;
                    DonVi.MaChiCuc = TTDonvi.MaChiCuc;
                    DonVi.MaDonVi = TTDonvi.MaDVCS;
                    DonVi.SoDt = TTDonvi.SDTCS;
                    DonVi.Stt = TTDonvi.Stt ?? 0;
                    DonVi.TenDonVi = TTDonvi.TenDVCS.ToUpper();
                    rptKQ.TenBacSi = TTDonvi.TenBacSiDaiDien;
                    try
                    {
                        if (TTDonvi.Logo.Length > 0)
                        {
                            byte[] b = TTDonvi.Logo.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            DonVi.LogoDonVi = img;
                        }
                    }
                    catch { }
                    try
                    {
                        if (TTDonvi.HeaderReport.Length > 0)
                        {
                            byte[] b2 = TTDonvi.HeaderReport.ToArray();
                            MemoryStream ms2 = new MemoryStream(b2);
                            Image img2 = Image.FromStream(ms2);
                            DonVi.Header = img2;
                        }
                    }
                    catch { }
                    try
                    {
                        if (TTDonvi.ChuKiDonVi.Length > 0)
                        {
                            byte[] b2 = TTDonvi.ChuKiDonVi.ToArray();
                            MemoryStream ms = new MemoryStream(b2);
                            Image img = Image.FromStream(ms);
                            DonVi.ChuKiDonVI = img;
                        }
                    }
                    catch { }


                }
                catch { }
                rptKQ.ThongTinDonVi = DonVi;
                rptKQ.ThongTinTrungTam = TrungTam;
            }
            else return null;
            return rptKQ;
        }
        public static PsRptViewTT GetDuLieuViewTT(string maPhieu)
        {
            PsRptViewTT rptKQ = new PsRptViewTT();
            List<PsRPTTraKetQuaSangLocChiTiet> lstrptKQCT = new List<PsRPTTraKetQuaSangLocChiTiet>();
            var db = new DataObjects();
            var ttphieu = GetThongTinTiepNhanTheoMaPhieu(maPhieu);
            PsThongTinTrungTam TrungTam = new PsThongTinTrungTam();
            PsThongTinDonVi DonVi = new PsThongTinDonVi();

            if (ttphieu != null)
            {
                var phieu = GetThongTinPhieu(maPhieu, ttphieu.MaDonVi);
                var ttKQ = db.GetThongTinTraKetQua(maPhieu, ttphieu.MaTiepNhan);
                var ttKQCT = db.GetThongTinTraKetQuaChiTiet(maPhieu, ttphieu.MaTiepNhan);
                var Nv = db.GetThongTinNhanVien(ttphieu.MaNVTiepNhan);
                var TTDonvi = db.GetThongTinDonViCoSo(ttphieu.MaDonVi);
                if (phieu != null)
                {
                    if (ttKQ != null)
                    {

                        rptKQ.NgayXetNghiem = (ttKQ.NgayLamXetNghiem ?? DateTime.Now).ToString("dd/MM/yyyy");
                        rptKQ.KetLuanBinhThuong = ttKQ.KetLuanTongQuat.Split('.')[0];
                        try { rptKQ.KetLuanNguyCoCao = ttKQ.KetLuanTongQuat.Split('.')[1]; } catch { rptKQ.KetLuanNguyCoCao = string.Empty; }
                        rptKQ.GhiChu = ttKQ.GhiChu;
                        rptKQ.NgayCoKQ = (ttKQ.NgayCoKQ ?? DateTime.Now).ToString("dd/MM/yyyy");
                    }
                    if (phieu.BenhNhan != null)
                    {
                        rptKQ.MaPhieu = phieu.maPhieu;
                        rptKQ.MaXetNghiem = phieu.maXetNghiem;
                        rptKQ.NgayNhanMau = phieu.ngayNhanMau.ToString("dd/MM/yyyy");
                        rptKQ.TenCha = phieu.BenhNhan.FatherName;
                        rptKQ.TenMe = phieu.BenhNhan.MotherName;
                        rptKQ.DienThoaiMe = phieu.BenhNhan.MotherPhoneNumber;
                        rptKQ.DienThoaiCha = phieu.BenhNhan.FatherPhoneNumber;
                        if (!string.IsNullOrEmpty(phieu.BenhNhan.FatherBirthday.ToString()))
                        {
                            rptKQ.NSCha = phieu.BenhNhan.FatherBirthday.Value.Year.ToString();
                        }
                        if (!string.IsNullOrEmpty(phieu.BenhNhan.MotherBirthday.ToString()))
                        {
                            rptKQ.NSMe = phieu.BenhNhan.MotherBirthday.Value.Year.ToString();
                        }
                        rptKQ.DiaChiTre = "Địa chỉ gia đình: " + phieu.BenhNhan.DiaChi;
                        rptKQ.MaKhachHang = phieu.BenhNhan.MaKhachHang;
                        rptKQ.TenTre = phieu.BenhNhan.TenBenhNhan;
                        rptKQ.NgaySinh = (phieu.BenhNhan.NgayGioSinh ?? DateTime.Now).ToString("dd/MM/yyyy");
                        if (phieu.BenhNhan.GioiTinh == 0)
                        {
                            rptKQ.GioiTinh = "Nam";
                        }
                        else if (phieu.BenhNhan.GioiTinh == 1)
                            rptKQ.GioiTinh = "Nữ";
                        else rptKQ.GioiTinh = "N/a";
                        rptKQ.CanNang = phieu.BenhNhan.CanNang.ToString();
                        rptKQ.TuoiThai = phieu.BenhNhan.TuanTuoiKhiSinh.ToString();
                        rptKQ.NgayThuMau = phieu.ngayGioLayMau.ToString("dd/MM/yyyy");
                        switch (phieu.BenhNhan.PhuongPhapSinh)
                        {
                            case 0:
                                rptKQ.PPSinh = "Sinh thường";
                                break;
                            case 1:
                                rptKQ.PPSinh = "Sinh mổ";
                                break;
                            case 2:
                                rptKQ.PPSinh = "Không xác định";
                                break;
                            default:
                                rptKQ.PPSinh = "Chưa có dữ liệu";
                                break;
                        }
                        rptKQ.CTSangLoc = db.GetCTrinh(phieu.maChuongTrinh);
                        rptKQ.LyDoKoDat = phieu.lydokhongdat == null ? string.Empty : phieu.lydokhongdat;
                        rptKQ.Para = phieu.paRa;
                        rptKQ.GoiXN = db.GetGoiXN(phieu.maGoiXetNghiem);
                        rptKQ.NguoiLayMau = phieu.tenNVLayMau == null ? string.Empty : phieu.tenNVLayMau;
                        switch (phieu.isKhongDat)
                        {
                            case false:
                                rptKQ.CLuongMau = "Đạt";
                                break;
                            case true:
                                rptKQ.CLuongMau = "Không đạt";
                                break;
                            default:
                                rptKQ.CLuongMau = "Chưa có dữ liệu";
                                break;
                        }
                    }
                    if (phieu.DonVi != null)
                    {
                        rptKQ.MaDonVi = "Mã đơn vị: " + phieu.DonVi.MaDVCS;
                        rptKQ.TenDonVi = "Tên đơn vị: " + phieu.DonVi.TenDVCS;
                        rptKQ.DiaChiDonVi = "Địa chỉ đơn vị: " + phieu.DonVi.DiaChiDVCS;
                    }
                    if (ttKQCT != null)
                    {
                        foreach (var ct in ttKQCT)
                        {
                            PsRPTTraKetQuaSangLocChiTiet c = new PsRPTTraKetQuaSangLocChiTiet();
                            c.DonViDo = ct.DonViTinh;
                            c.GiaTri = ct.GiaTriCuoi;
                            c.GiaTriTrungBinh = ct.GiaTriTrungBinh;
                            c.KetLuan = ct.KetLuan;
                            c.MaThongSo = ct.IDThongSoXN;
                            c.isNguyCoCao = ct.isNguyCo;
                            try
                            {
                                c.TenDichVu = string.IsNullOrEmpty(db.GetTenDichVuCuaKyThuat(ct.IDKyThuat)) == true ? ct.TenKyThuat : db.GetTenDichVuCuaKyThuat(ct.IDKyThuat);
                            }
                            catch { c.TenDichVu = ct.TenKyThuat; }

                            c.TenThongSo = ct.TenThongSo;
                            lstrptKQCT.Add(c);
                        }
                    }
                    rptKQ.chitietKetQua = lstrptKQCT;
                }

            }
            else return null;
            return rptKQ;
        }
        public static string GetPatientTheoMaPhieu(string maPhieu)
        {
            string MaBN = string.Empty;
            try
            {
                PSTiepNhan tn = GetThongTinTiepNhanTheoMaPhieu(maPhieu);
                if (tn != null)
                {
                    PsPhieu ph = GetThongTinPhieu(tn.MaPhieu, tn.MaDonVi);
                    if (ph != null)
                    {
                        MaBN = ph.maBenhNhan;
                    }
                }
            }
            catch
            {
            }           
            return MaBN;
        }
        public static List<PSXN_TraKetQua> GetDanhSachChoTraKetQua(DateTime tuNgay, DateTime denNgay, string maDonVi)
        {
            var db = new DataObjects();
            if (maDonVi.Equals("all")) maDonVi = string.Empty;
            return db.GetDanhSachChoTraKetQua(tuNgay, denNgay, maDonVi, false);
        }

        public static List<PSXN_TraKetQua> GetDanhSachDaDuyetTraKetQua(DateTime tuNgay, DateTime denNgay, string maDonVi)
        {
            var db = new DataObjects();
            if (maDonVi.Equals("all")) maDonVi = string.Empty;
            return db.GetDanhSachChoTraKetQua(tuNgay, denNgay, maDonVi, true);

        }
        public static List<PSXN_TTTraKQ> GetDanhSachChoTraKetQuaAll(DateTime tuNgay, DateTime denNgay, string maDonVi)
        {
            var db = new DataObjects();
            if (maDonVi.Equals("all")) maDonVi = string.Empty;
            return db.GetDanhSachChoTraKetQuaAll(tuNgay, denNgay, maDonVi, false);
        }
        public static List<PSXN_TTTraKQ> GetDanhSachChoTraKetQuaAll()
        {
            var db = new DataObjects();
            return db.GetDanhSachChoTraKetQuaAll();
        }
        public static List<View_TraKQ> GetDanhSachChoTraKetQuaAllNew()
        {
            var db = new DataObjects();
            return db.GetDanhSachChoTraKetQuaAllNew();
        }
        public static List<PsKetQua_ChiTiet> GetDanhSachKetQuaChiTiet(string maKQ, string maPhieu, string MaXN)
        {
            var db = new DataObjects();
            List<PsKetQua_ChiTiet> lst = new List<PsKetQua_ChiTiet>();
            var results = db.GetDanhSachChiTietKetQua(maKQ, MaXN);
            if (results.Count > 0)
            {
                int gioitinh = 1;
                var Phieu = db.GetThongTinPhieu(maPhieu);
                try
                {
                    if (Phieu != null)
                    {
                        PSPatient BenhNhan = GetThongTinBenhNhan(Phieu.MaBenhNhan);
                        gioitinh = BenhNhan.GioiTinh ?? 1;
                    }
                }
                catch { }
                foreach (var result in results)
                {
                    PsKetQua_ChiTiet kq = new PsKetQua_ChiTiet();
                    kq.DonViTinh = result.DonViTinh;
                    kq.MaDichVu = result.MaDichVu;
                    kq.MaKQ = result.MaKQ;
                    kq.isNguyCoCao = result.isNguyCo ?? false;
                    kq.GiaTri = result.GiaTri;
                    kq.MaKyThuat = result.MaKyThuat;
                    kq.MaThongSo = result.MaThongSoXN;
                    kq.MaXN = result.MaXetNghiem;
                    kq.TenKyThuat = result.TenKyThuat;
                    kq.TenThongSo = result.TenThongSo;
                    if (gioitinh == 1)
                    {
                        kq.GiaTriMax = result.GiaTriMaxNam ?? 0;
                        kq.GiaTriMin = result.GiaTriMinNam ?? 0;
                        kq.GiaTriTrungBinh = result.GiaTriTrungBinhNam;
                    }
                    else
                    {
                        kq.GiaTriMax = result.GiaTriMaxNu ?? 0;
                        kq.GiaTriMin = result.GiaTriMinNu ?? 0;
                        kq.GiaTriTrungBinh = result.GiaTriTrungBinhNu;
                    }
                    lst.Add(kq);
                }
            }
            return lst;
        }
        public static PSXN_KetQua GetPhieuXNKetQua(string MaPhieu, string MaXN, string MaGoiXN)
        {
            var db = new DataObjects();
            return db.GetPhieuXNKetQua(MaPhieu, MaXN, MaGoiXN);
        }
        public static List<PsRptDanhSachDaCapMaXetNghiem> GetDanhSachDaCapMaXetNghiem(DateTime tuNgay, DateTime denNgay, string maDonVi)
        {
            List<PsRptDanhSachDaCapMaXetNghiem> lst = new List<PsRptDanhSachDaCapMaXetNghiem>();
            var db = new DataObjects();
            if (maDonVi.Equals("all"))
                maDonVi = string.Empty;
            var res = db.GetDanhSachChoNhanKetQua(tuNgay, denNgay, maDonVi, false);
            if (res.Count > 0)
            {
                foreach (var item in res)
                {
                    var dovi = db.GetThongTinDonViCoSo(item.MaDonVi);
                    if (dovi != null)
                    {
                        PsRptDanhSachDaCapMaXetNghiem cm = new PsRptDanhSachDaCapMaXetNghiem();
                        cm.MaDonVi = item.MaDonVi;
                        cm.MaPhieu = item.MaPhieu;
                        cm.MaXetNghiem = item.MaXetNghiem;

                        var goixn = db.GetDanhMucGoiXetNghiemChung(item.MaGoiXN);
                        if (goixn != null)
                        {
                            foreach (var goi in goixn)
                            {
                                cm.MaGoiXetNghiem = goi.IDGoiDichVuChung;
                                cm.TenGoiXetNghiem = goi.TenGoiDichVuChung;
                                cm.TenGoiXetNghiemKhongDau = Bodautiengviet(goi.TenGoiDichVuChung);
                            }
                        }
                        else
                        {
                            cm.MaGoiXetNghiem = "XNLan2";
                            cm.TenGoiXetNghiem = "XNL L2";
                            cm.TenGoiXetNghiemKhongDau = "XNL L2";
                        }
                        cm.TenDonVi = dovi.TenDVCS;

                        PSEmployee nv = GetThongTinNhanVien(item.IDNhanVienCapMa);
                        if (nv != null)
                        {
                            cm.NVCapMa = nv.EmployeeName;
                        }

                        string GhiChu = string.Empty;
                        try
                        {

                            var phieu = db.GetThongTinPhieu(item.MaPhieu);
                            if (phieu != null)
                            {
                                if (phieu.isGuiMauTre ?? false)
                                {
                                    GhiChu = "Gửi mẫu trễ";
                                }
                                if (phieu.isNheCan ?? false)
                                {
                                    if (!string.IsNullOrEmpty(GhiChu))
                                        GhiChu += ", trẻ nhẹ cân";
                                    else GhiChu = "Trẻ nhẹ cân";
                                }
                                if (phieu.isSinhNon ?? false)
                                {
                                    if (!string.IsNullOrEmpty(GhiChu))
                                        GhiChu += ", trẻ sinh non";
                                    else GhiChu = "Trẻ sinh non";
                                }
                                if (phieu.isTruoc24h ?? false)
                                {
                                    if (!string.IsNullOrEmpty(GhiChu))
                                        GhiChu += ", mẫu lấy trước 30h sau khi sinh";
                                    else GhiChu = "Mẫu lấy trước 30h sau khi sinh";
                                }
                                if (!string.IsNullOrEmpty(phieu.LyDoKhongDat))
                                {
                                    if (!string.IsNullOrEmpty(GhiChu))
                                        GhiChu += "." + phieu.LyDoKhongDat;
                                    else GhiChu = phieu.LyDoKhongDat;
                                }
                                if (!string.IsNullOrEmpty(phieu.LuuYPhieu))
                                {
                                    if (!string.IsNullOrEmpty(GhiChu))
                                        GhiChu += ". Ghi Chú:" + phieu.LuuYPhieu;
                                    else GhiChu = "Ghi Chú:" + phieu.LuuYPhieu;
                                }
                            }

                        }
                        catch { }
                        cm.GhiChu = GhiChu;
                        lst.Add(cm);
                    }
                }
            }
            return lst;
        }
        public static List<pro_ReportGanViTriMayXNResult> GanViTriMayXNResult(DateTime tungay, DateTime denngay)
        {
            List<pro_ReportGanViTriMayXNResult> lst = new List<pro_ReportGanViTriMayXNResult>();
            try
            {
                var db = new DataObjects();
                lst = db.GanViTriMayXNResult(tungay, denngay);
            }
            catch
            {

            }
            return lst;
        }
        public static string Bodautiengviet(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public static List<PSXN_KetQua> GetDanhSachChoKetQuaXN(DateTime tuNgay, DateTime denNgay, string maDonVi)
        {
            var db = new DataObjects();
            if (maDonVi.Equals("all"))
                maDonVi = string.Empty;
            return db.GetDanhSachChoNhanKetQua(tuNgay, denNgay, maDonVi, false);
        }
        public static List<PSXN_KetQua> GetDanhSachDaCoKetQuaXN(DateTime tuNgay, DateTime denNgay, string maDonVi)
        {
            var db = new DataObjects();
            return db.GetDanhSachChoNhanKetQua(tuNgay, denNgay, maDonVi, true);
        }
        public static PSChiDinhDichVu GetThongTinhChiDinh(string maphieu, string maTiepNhan)
        {
            var db = new DataObjects();
            return db.GetThongTinChiDinh(maphieu, maTiepNhan);
        }
        public static PSChiDinhDichVu GetThongTinhChiDinh(string maCD)
        {
            var db = new DataObjects();
            return db.GetThongTinChiDinh(maCD);
        }
        public static bool KiemTraPhieuThuMauLaiDaDuocChiDinhChua(string maphieulan1, string maphieulamlai)
        {
            var db = new DataObjects();
            return db.KiemTraPhieuThuMauLaiDaDuocChiDinhChua(maphieulan1, maphieulamlai);
        }
        public static List<PSChiDinhTrenPhieu> GetChiDinhDichVuChiTiet(string maCD)
        {
            List<PSChiDinhTrenPhieu> lstdv = new List<PSChiDinhTrenPhieu>();
            var db = new DataObjects();
            //  var lst db.GetThongTinChiDinhDichVuChiTiet(maCD);
            var lstcd = db.GetThongTinChiDinhDichVuChiTiet(maCD);
            if (lstcd.Count > 0)
            {
                foreach (var item in lstcd)
                {
                    PSChiDinhTrenPhieu dv = new PSChiDinhTrenPhieu();
                    dv.MaDichVu = item.MaDichVu;
                    dv.MaPhieu = item.MaPhieu;
                    dv.MaDonVi = item.MaDonVi;
                    lstdv.Add(dv);
                }
            }
            return lstdv;
        }
        public static List<PSXN_TraKQ_ChiTiet> GetDanhSachTraKetQuaChiTiet(string maTiepNhan)
        {
            var db = new DataObjects();
            return db.GetDanhSachTraKetQuaChiTiet(maTiepNhan);
        }
        public static string GetMaPhieu(string maTiepNhan)
        {
            var db = new DataObjects();
            return db.GetMaPhieu(maTiepNhan);
        }

        public static string GetMaPhieu1TheoMaBN(string maTiepNhan)
        {
            var db = new DataObjects();
            return db.GetMaPhieu1TheoMaBN(maTiepNhan);
        }
        public static string GetMaPhieu2TheoMaBN(string maTiepNhan)
        {
            var db = new DataObjects();
            return db.GetMaPhieu2TheoMaBN(maTiepNhan);
        }
        public static List<PSXN_TraKQ_ChiTiet> GetDanhSachTraKQChiTiet(string maTiepNhan, string maPhieu)
        {
            var db = new DataObjects();
            return db.GetDanhSachTraKetQuaChiTiet(maTiepNhan, maPhieu);
        }
        public static List<PSXN_TraKQ_ChiTiet> GetDanhSachTraKetQuaChiTietPhieuCu(string maPhieu)
        {
            var db = new DataObjects();
            return db.GetDanhSachTraKetQuaChiTietPhieuCu(maPhieu);
        }
        public static bool XacThucNhanVien(string maNV, string pass)
        {
            var db = new DataObjects();
            return db.XacThucNhanVien(maNV, pass);
        }
        public static List<PSChiDinhDichVu> GetDanhSachChuaCapMa()
        {
            List<PSChiDinhDichVu> lst = new List<PSChiDinhDichVu>();
            var db = new DataObjects();
            lst = db.GetDanhSachChuaCapMaXN(1);
            return lst;
        }
        public static List<PSChiDinhDichVu> GetDanhSachDaCapMa()
        {
            List<PSChiDinhDichVu> lst = new List<PSChiDinhDichVu>();
            var db = new DataObjects();
            lst = db.GetDanhSachChuaCapMaXN(2);
            return lst;
        }
        public static List<PSXN_KetQua> GetDanhSachChuaCoKQ()
        {
            List<PSXN_KetQua> lst = new List<PSXN_KetQua>();
            var db = new DataObjects();
            lst = db.GetDSPhongXN(false);
            return lst;
        }
        public static List<PSGanViTriXNKQ> GetDSChuaGanXN()
        {
            List<PSGanViTriXNKQ> lst = new List<PSGanViTriXNKQ>();
            var db = new DataObjects();
            return lst = db.GetDSChuaGanXN();
        }
        public static List<PSXN_KetQua> GetDanhSachCoKQ()
        {
            List<PSXN_KetQua> lst = new List<PSXN_KetQua>();
            var db = new DataObjects();
            lst = db.GetDSPhongXN(true);
            return lst;
        }
        public static List<PSChiDinhDichVu> GetDanhSachChiDinhChuaDuocCapMa(string maDonVi, DateTime tuNgay, DateTime denNgay)
        {
            List<PSChiDinhDichVu> lst = new List<PSChiDinhDichVu>();
            var db = new DataObjects();
            if (maDonVi.Equals("all"))
                maDonVi = string.Empty;
            var results = db.GetDanhSachDichVuDaChiDinh(1, maDonVi, tuNgay, denNgay, true);
            if (results != null)
                lst = results;
            return lst;
        }
        public static List<PSChiDinhDichVu> GetDanhSachDaDuocChiDinh(string maDonVi, DateTime tuNgay, DateTime denNgay)
        {
            List<PSChiDinhDichVu> lst = new List<PSChiDinhDichVu>();
            var db = new DataObjects();
            if (maDonVi.Equals("all"))
                maDonVi = string.Empty;
            var results = db.GetDanhSachDichVuDaChiDinh(-1, maDonVi, tuNgay, denNgay, false);
            if (results != null)
                lst = results;
            return lst;
        }
        public static PSCMGanViTriChung GetPhieuChuaCoKQ(string maphieu,string IDMayDucLo)
        {
            PSCMGanViTriChung lst = new PSCMGanViTriChung();
            var db = new DataObjects();
            lst = db.GetPhieuChuaCoKQ(maphieu,IDMayDucLo);
            return lst;
        }

        public static string GetMaRowIDLanDucLo(string MaMayDL)
        {
            var db = new DataObjects();
            return db.GetMaRowIDLanDucLo(MaMayDL);
        }
        public static PsReponse SuaDanhSachGanXNLuu(PSCMGanViTriChung pm,string STT,string IDLanDucLo)
        {
            var db = new DataObjects();
            return db.SuaDanhSachGanXNLuu(pm,STT,IDLanDucLo);
        }
        public static PsReponse SuaDanhSachGanXNLuuNew(PSCMGanViTriChung pm, string MayDucLo, string IDLanDucLo, string MaNV)
        {
            var db = new DataObjects();
            return db.SuaDanhSachGanXNLuuNew(pm, MayDucLo, IDLanDucLo, MaNV);
        }
        public static PsReponse DoiViTriGanXN(PSCMGanViTriChung pm1, PSCMGanViTriChung pm2, string IDMayXN)
        {
            var db = new DataObjects();
            return db.DoiViTriGanXN(pm1, pm2, IDMayXN);
        }
        public static PsReponse ThemGhiChuGanVT(PSCMGanViTriChung pm)
        {
            var db = new DataObjects();
            return db.ThemGhiChuGanVT(pm);
        }
            public static bool GetMaPhieuDaCoDSCapVT(string MaPhieu)
        {
            var db = new DataObjects();
            return db.GetMaPhieuDaCoDSCapVT(MaPhieu);
        }
        public static PsReponse DeleteDanhSachGanXNLuu(PSCMGanViTriChung pm)
        {
            var db = new DataObjects();
            return db.DeleteDanhSachGanXNLuu(pm);
        }
        public static PsReponse DeleteDanhSachGanXNLuuMax(PSCMGanViTriChung pm)
        {
            var db = new DataObjects();
            return db.DeleteDanhSachGanXNLuu(pm);
        }
        public static PsReponse DeleteCTGanXNLuu(PSCM_GanViTriCT gan)
        {
            var db = new DataObjects();
            return db.DeleteCTGanXNLuu(gan);
        }
        public static PsReponse DeleteDanhSachAllGanXNLuu(string IDLanGanXN, string IDNhanVien)
        {
            var db = new DataObjects();
            return db.DeleteDanhSachAllGanXNLuu(IDLanGanXN, IDNhanVien);
        }
        public static List<PSDanhMucGhiChuXN> GetDanhMucGhiXN()
        {
            var db = new DataObjects();
            return db.GetDanhMucGhiXN();
        }

        public static bool UpdateDanhMucGC(PSDanhMucGhiChuXN ghichu)
        {
            var db = new DataObjects();
            return db.UpdateDanhMucGC(ghichu);
        }
        public static bool AddNewDanhMucGC(PSDanhMucGhiChuXN ghichu)
        {
            var db = new DataObjects();
            return db.AddNewDanhMucGC(ghichu);
        }
        public static bool DeleteDanhMucGC(PSDanhMucGhiChuXN ghichu)
        {
            var db = new DataObjects();
            return db.DeleteDanhMucGC(ghichu);
        }
        public static PsReponse DuyetCapMaGanMayXN(string IDLanGanXN, string IDNhanVien)
        {
            var db = new DataObjects();
            return db.DuyetCapMaGanMayXN(IDLanGanXN, IDNhanVien);
        }
        public static List<PSCMGanViTriChung> GetDanhSachGanXNLuuNew(string MayDucLo, string IDLanDucLo)
        {
            var db = new DataObjects();
            return db.GetDanhSachGanXNLuuNew(MayDucLo,IDLanDucLo);
        }
            public static PsReponse SaveGanXN(PSCMGanViTriChung pm)
        {
            var db = new DataObjects();
            return db.SaveGanXN(pm);
        }
        public static List<PSDanhMucMayDucLo> GetDanhSachMayDucLo(bool isSuDung)
        {
            var db = new DataObjects();
            return db.GetDanhSachMayDucLo(isSuDung);
        }
        public static List<PSDanhMucMayXN> GetDSMayXN()
        {
            var db = new DataObjects();
            List<PSDanhMucMayXN> lst = db.GetDSMayXN();
            return lst;
        }
        public static List<PSDanhMucMayXN> GetDSMayXNTheoMayDucLo(String IDMayDucLo)
        {
            var db = new DataObjects();
            return db.GetDSMayXNTheoMayDucLo(IDMayDucLo);
        }
        public static PSDanhMucMayDucLo GetTTMayDucLo(string IDMayDucLo)
        {
            var db = new DataObjects();
            return db.GetTTMayDucLo(IDMayDucLo);
        }

        public static List<PSMapsViTriMayXN> GetDSMapViTriMayXN(string IDMayXN)
        {
            var db = new DataObjects();
            List<PSMapsViTriMayXN> lst = db.GetDSMapViTriMayXN(IDMayXN);
            return lst;
        }
        public static bool UpdateMaXNVaoBangGhi(string giaTri)
        {
            var db = new DataObjects();
            string maDuLieu = "MAXN" + SoBanDau();
            return db.UpdateBangGhi(maDuLieu, giaTri);
        }
        public static List<PSChiTietDanhGiaChatLuong> GetChiTietDanhGiaMạuKhongDatTrenPhieu(string maPhieu, string maTiepNhan)
        {
            var db = new DataObjects();
            return db.GetChiTietDanhGiaMạuKhongDatTrenPhieu(maPhieu, maTiepNhan);
        }
        public static List<PSDanhMucDanhGiaChatLuongMau> GetDanhMucDanhGiaChatLuong()
        {
            var db = new DataObjects();
            return db.GetDanhMucDanhGiaChatLuongMau();
        }

        public static PsDichVu GetThongTinDichVu(string maDichVu, string maDonVi)
        {
            var db = new DataObjects();
            var ttdv = db.GetDichVuTheoDonVi(maDichVu, maDonVi);
            if (ttdv != null)
            {
                PsDichVu dvu = new PsDichVu();
                dvu.IDDichVu = ttdv.IDDichVu;
                dvu.GiaDichVu = ttdv.GiaDichVu ?? 0;
                dvu.isChecked = false;
                dvu.isLocked = ttdv.isLocked ?? false;
                dvu.MaNhom = ttdv.MaNhom.ToString();
                dvu.TenDichVu = ttdv.TenDichVu;
                dvu.TenHienThi = ttdv.TenHienThiDichVu;
                return dvu;
            }
            else return null;

        }
        public static List<PsDichVu> GetDanhSachDichVuTheoMaGoi(string maGoi, string maDonVi)
        {
            List<PsDichVu> lst = new List<PsDichVu>();
            var db = new DataObjects();
            var lstmadichvu = db.GetDichVuTheoMaGoi(maGoi);
            var lstdichvu = db.GetDanhSachDichVu(false);
            if (lstmadichvu.Count > 0)
            {
                foreach (var dv in lstmadichvu)
                {

                    var term = lstdichvu.SingleOrDefault(x => x.IDDichVu == dv.IDDichVu);

                    PsDichVu dvu = new PsDichVu();
                    dvu.IDDichVu = dv.IDDichVu;
                    dvu.GiaDichVu = term.GiaDichVu ?? 0;
                    dvu.isChecked = false;
                    dvu.isLocked = term.isLocked ?? false;
                    dvu.MaNhom = term.MaNhom.ToString();
                    dvu.TenDichVu = dv.IDDichVu;
                    dvu.TenHienThi = term.TenHienThiDichVu;
                    lst.Add(dvu);
                }
            }
            return lst;
        }
        public static PSTTPhieu GetTTPhieu(string maGoi, string maDonVi)
        {
            PSTTPhieu lst = new PSTTPhieu();
            var db = new DataObjects();
            lst = db.GetTTPhieu(maGoi, maDonVi);
            return lst;


        }
        public static List<PSDanhMucDonViCoSo> GetDanhSachDonVi_Searchlookup()
        {
            var db = new DataObjects();
            return db.GetDanhSachDonVi();
        }
        public static List<PSDanhMucDonViCoSo> GetDanhSachDonViCoSo()
        {
            var db = new DataObjects();
            return db.GetDonViCoSo(null);
        }
        public static PSDanhMucDonViCoSo GetThongTinDonViCoSo(string maDonViCoSo)
        {
            PSDanhMucDonViCoSo donvi = new PSDanhMucDonViCoSo();
            var db = new DataObjects();
            return db.GetThongTinDonViCoSo(maDonViCoSo);
        }
        public static string GetThongTinMailDonViCoSo(string maDonViCoSo)
        {

            var db = new DataObjects();
            return db.GetThongTinMailDonViCoSo(maDonViCoSo);
        }
        public static string GetThongTinMaTiepNhan(string maphieu, string maDonViCoSo)
        {

            var db = new DataObjects();
            return db.GetThongTinMaTiepNhan(maphieu, maDonViCoSo);
        }


        public static List<PSTiepNhan> GetDanhSachPhieuDaTiepNhan(string maDonvi, DateTime tuNgay, DateTime denNgay)
        {
            List<PSTiepNhan> lst = new List<PSTiepNhan>();
            var db = new DataObjects();
            if (maDonvi.Equals("ALL"))
            {
                var results = db.GetDanhSachPhieuDaTiepNhan(null, tuNgay, denNgay);
                if (results != null)
                {
                    return results;
                }
            }
            else
            {
                var results = db.GetDanhSachPhieuDaTiepNhan(maDonvi, tuNgay, denNgay);
                if (results != null)
                {
                    return results;
                }
            }
            return lst;

        }
        public static List<PSTiepNhan> GetDanhSachPhieuChuaDanhGia(string maDonvi)
        {
            List<PSTiepNhan> lst = new List<PSTiepNhan>();
            var db = new DataObjects();
            if (maDonvi.Equals("all"))
            {
                lst = db.GetDanhSachPhieuDaTiepNhan(null, false);
            }
            else
            {
                lst = db.GetDanhSachPhieuDaTiepNhan(maDonvi, false);
            }
            return lst;

        }

        public static List<PSTiepNhan> GetDanhSachPhieuDaDanhGia(string maDonvi, DateTime tuNgay, DateTime denNgay)
        {
            List<PSTiepNhan> lst = new List<PSTiepNhan>();
            var db = new DataObjects();
            if (maDonvi.Equals("all"))
            {
                var results = db.GetDanhSachPhieuDaTiepNhan(null, true);
                if (results != null)
                {
                    foreach (var result in results)
                    {
                        lst.Add(result);
                    }
                }
            }
            else
            {
                var results = db.GetDanhSachPhieuDaTiepNhan(maDonvi, true);
                if (results != null)
                {
                    foreach (var result in results)
                    {
                        lst.Add(result);
                    }
                }
            }
            return lst;

        }
        public static List<PSChiDinhDichVu> GetDanhSachPhieuDaDuyet(string maDonvi, DateTime tuNgay, DateTime denNgay)
        {
            List<PSChiDinhDichVu> lst = new List<PSChiDinhDichVu>();
            var db = new DataObjects();
            if (maDonvi.Equals("all"))
            {
                lst = db.GetDanhSachPhieuDaDuyet(null, tuNgay, denNgay);
            }
            else
            {
                lst = db.GetDanhSachPhieuDaDuyet(maDonvi, tuNgay, denNgay);
            }
            return lst;
        }
        public static List<PSChiDinhDichVu> GetDanhSachPhieuDaDuyet(string maDonvi)
        {
            List<PSChiDinhDichVu> lst = new List<PSChiDinhDichVu>();
            var db = new DataObjects();
            if (maDonvi.Equals("all"))
            {
                lst = db.GetDanhSachPhieuDaDuyet(null);
            }
            else
            {
                lst = db.GetDanhSachPhieuDaDuyet(maDonvi);
            }
            return lst;
        }

        public static string GetMaXN(string maTiepNhan)
        {
            var db = new DataObjects();
            return db.GetMaXN(maTiepNhan);
        }
        public static PSDanhMucGhiChu GetThongTinHienThiGhiChu(string maGhiChu)
        {
            var db = new DataObjects();
            return db.GetThongTinHienThiGhiChu(maGhiChu);
        }
        public static bool KiemTraChoPhepThuMauLan2()
        {
            var db = new DataObjects();
            return db.KiemTraChoPhepThuMauLan2();
        }
        public static bool KiemTraGioiHan()
        {
            var db = new DataObjects();
            return db.KiemTraGioiHan();
        }
        public static bool KiemTraChoPhepLamLaiXetNghiemLan2()
        {
            var db = new DataObjects();
            return db.KiemTraChoPhepLamLaiXetNghiemLan2();
        }

        public static List<PsPhieu> GetDanhSachPhieuChoTiepNhan()
        {
            List<PsPhieu> lst = new List<PsPhieu>();
            var db = new DataObjects();
            var result = db.GetPhieuSangLoc(false, 0);
            if (result.Count > 0)
            {
                foreach (var phieu in result)
                {
                    var ph = GetThongTinPhieu(phieu.IDPhieu, phieu.IDCoSo);
                    if (ph != null)
                    {
                        lst.Add(ph);
                    }
                }
            }
            return lst;
        }
        public static List<PsHoanDongBo> GetFullPhieuSangLoc(DateTime NgayBD, DateTime NgayKT, string maDonVi, string maChiCuc, bool dongbo, bool kq)
        {
            List<PsHoanDongBo> lst = new List<PsHoanDongBo>();
            var db = new DataObjects();
            string donvi = "";
            try
            {
                if (maDonVi != null && !maDonVi.Equals("all"))
                {
                    donvi = maDonVi;

                }
                else
                {
                    if (maChiCuc == null || maChiCuc.Equals("all"))
                    {
                        donvi = "all";
                    }
                    else
                    {
                        donvi = maChiCuc;
                    }
                }
                lst = db.GetFullPhieuSangLoc(NgayBD, NgayKT, donvi, dongbo, kq);
            }
            catch
            {

            }

            return lst;
        }

        public static PsReponse HoanDongBo(string IDPhieu, string IDCoSo, string MaBenhNhan)
        {
            PsReponse lst = new PsReponse();
            var db = new DataObjects();

            lst = db.HoanDongBo(IDPhieu, IDCoSo, MaBenhNhan);
            return lst;
        }
        public static PsReponse HoanDongBoPhieu(string IDPhieu)
        {
            PsReponse lst = new PsReponse();
            var db = new DataObjects();
            lst = db.HoanDongBoPhieu(IDPhieu);
            return lst;
        }
        public static PsReponse DelPhieuTiepNhan(string MaPhieu, string MaBenhNhan)
        {
            var db = new DataObjects();
            var result = db.DelPhieuTiepNhan(MaPhieu, MaBenhNhan);
            return result;
        }
        public static List<PSDanhMucGoiDichVuTheoDonVi> GetDanhsachGoiDichVuTrungTam(string maDonvi)
        {
            List<PSDanhMucGoiDichVuTheoDonVi> lst = new List<PSDanhMucGoiDichVuTheoDonVi>();
            var db = new DataObjects();
            return db.GetDanhMucGoiXetNghiemTrungTam(maDonvi);
        }

        public static List<PSDanhMucGoiDichVuChung> GetDanhsachGoiDichVuChung()
        {
            List<PSDanhMucGoiDichVuChung> lst = new List<PSDanhMucGoiDichVuChung>();
            var db = new DataObjects();
            var results = db.GetDanhMucGoiXetNghiemChung(null);
            try
            {
                foreach (var result in results)
                {
                    lst.Add(result);
                }
            }
            catch { }
            return lst;
        }
        public static List<PsDichVu> GetDanhSachDichVu(bool isLocked)
        {
            List<PsDichVu> lst = new List<PsDichVu>();
            var db = new DataObjects();
            var results = db.GetDanhSachDichVu(isLocked);
            try
            {
                foreach (var result in results)
                {
                    PsDichVu item = new PsDichVu();
                    item.RowID = result.RowIDDichVu;
                    item.IDDichVu = result.IDDichVu;
                    item.TenDichVu = result.TenDichVu;
                    item.MaNhom = result.MaNhom.ToString();
                    item.isLocked = result.isLocked ?? false;
                    item.isChecked = false;
                    lst.Add(item);
                }
            }
            catch { }
            return lst;
        }
        public static List<PsMapMayDichVu> GetMapMayDichVus(string IDMayXN)
        {
            var db = new DataObjects();
            return db.GetMapMayDichVus(IDMayXN);
        }
        //public static rptBaoCaoTongHop GetBaoCaoTongHopTrungTam(DateTime tuNgay,DateTime denNgay)
        //{
        //    rptBaoCaoTongHop bc = new rptBaoCaoTongHop();
        //    var db = new DataObjects();
        //    var kq = db.GetDanhSachKetQuaDaDuyet(tuNgay, denNgay);
        //    if(kq.Count>0)
        //    {
        //        var lstPhieu = kq.GroupBy(p => p.MaGoiXN);
        //        var goiXNTrungTam = db.GetDanhMucGoiXetNghiemChung(string.Empty);
        //    }
        //    return bc;
        //}
        public static PSPatient GetThongTinBenhNhan(string maSoBenhNhan)
        {
            var db = new DataObjects();
            PSPatient BN = new PSPatient();
            var result = db.GetThongTinDuLieuBenhNhan(maSoBenhNhan);
            if (result != null)
                return result;
            else return BN;
        }
        public static PSEmployee GetThongTinNhanVien(string maNV)
        {
            var db = new DataObjects();
            PSEmployee BN = new PSEmployee();
            var result = db.GetThongTinDuLieuNhanVien(maNV);
            if (result != null)
                return result;
            else return BN;
        }

        public static PsPhieu GetThongTinPhieu(string maPhieu, string maDonvi)
        {
            var db = new DataObjects();
            var ph = db.GetPhieuSangLoc(maPhieu, maDonvi);
            PsPhieu phieu = new PsPhieu();
            if (ph != null)
            {
                phieu.idViTriLayMau = ph.IDViTriLayMau ?? 0;
                phieu.isGuiMauTre = ph.isGuiMauTre ?? false;
                phieu.isKhongDat = ph.isKhongDat ?? false;
                phieu.isLayMauLan2 = ph.isLayMauLan2 ?? false;
                phieu.isNheCan = ph.isNheCan ?? false;
                phieu.isSinhNon = ph.isSinhNon ?? false;
                phieu.isTruoc24h = ph.isTruoc24h ?? false;
                phieu.maBenhNhan = ph.MaBenhNhan;
                phieu.maCheDoDinhDuong = ph.CheDoDinhDuong ?? 0;
                phieu.maChuongTrinh = ph.IDChuongTrinh;
                phieu.maDonViCoSo = ph.IDCoSo;
                phieu.maGoiXetNghiem = ph.MaGoiXN;
                phieu.maNVTaoPhieu = ph.IDNhanVienTaoPhieu;
                phieu.maNVLayMau = ph.IDNhanVienLayMau;
                phieu.maPhieu = ph.IDPhieu;
                phieu.maPhieuLan1 = ph.IDPhieuLan1;
                phieu.maTinhTrangLucLayMau = ph.TinhTrangLucLayMau ?? 0;
                phieu.maXetNghiem = ph.MaXetNghiem;
                phieu.ngayGioLayMau = ph.NgayGioLayMau ?? DateTime.Now;
                phieu.ngayNhanMau = ph.NgayNhanMau ?? DateTime.Now;
                phieu.ngayTaoPhieu = ph.NgayTaoPhieu ?? DateTime.Now;
                phieu.ngayTruyenMau = ph.NgayTruyenMau ?? DateTime.Now;
                phieu.paRa = ph.Para;
                phieu.soLuongTruyenMau = ph.SLTruyenMau;
                phieu.trangThaiMau = ph.TrangThaiMau ?? 0;
                phieu.trangThaiPhieu = ph.TrangThaiPhieu ?? true;
                phieu.rowIDPhieu = ph.RowIDPhieu;
                phieu.tenNVLayMau = ph.TenNhanVienLayMau;
                phieu.TenNhanVienLayMau = ph.TenNhanVienLayMau;
                phieu.SoDTNhanVienLayMau = ph.SDTNhanVienLayMau;
                phieu.NoiLayMau = ph.NoiLayMau;
                phieu.DiaChiLayMau = ph.DiaChiLayMau;
                phieu.LuuYPhieu = ph.LuuYPhieu;
                phieu.lydokhongdat = ph.LyDoKhongDat;
                if (!string.IsNullOrEmpty(ph.IDCoSo))
                {
                    phieu.DonVi = BioNet_Bus.GetThongTinDonViCoSo(ph.IDCoSo);
                }
                if (!string.IsNullOrEmpty(ph.MaBenhNhan))
                {
                    var res = GetThongTinBenhNhan(ph.MaBenhNhan);
                    if (res != null)
                    {
                        phieu.BenhNhan = res;
                    }
                }
                //var listChiDinh = db.GetChiDinhTrenPhieu(maPhieu);
                // List<PSChiDinhTrenPhieu> lst = new List<PSChiDinhTrenPhieu>();
                //if (listChiDinh!=null)
                //{
                //    foreach(var chiDinh in listChiDinh)
                //    {
                //        PSChiDinhTrenPhieu cd = new PSChiDinhTrenPhieu();
                //        cd.RowID = chiDinh.RowID;
                //        cd.MaPhieu = chiDinh.MaPhieu;
                //        cd.MaDonVi = chiDinh.MaDonVi;
                //        cd.MaDichVu = chiDinh.MaDichVu;
                //        lst.Add(cd);
                //    }
                //}
                // phieu.lstChiDinh = lst;
                return phieu;
            }
            else
                return null;
        }

        public static string GetMaPhieuLan1(string maPhieulan2, string maDonVi)
        {
            var db = new DataObjects();
            return db.GetThongTinPhieuLan1(maPhieulan2, maDonVi);
        }
        public static List<PSChiDinhTrenPhieu> GetDichVuCanLamLaiCuaPhieu(string maPhieu, string madonVi)
        {
            var db = new DataObjects();
            return db.GetDichVuCanLamLaiCuaPhieu(maPhieu, madonVi);
        }
        public static List<PSDanhMucDanToc> GetDanhSachDanToc(int maQuocGia)
        {
            var db = new DataObjects();
            var result = db.GetDanhSachDanToc(maQuocGia);
            if (result != null)
            {
                return result;
            }
            else
                return null;
        }
        public static List<PSDanhMucChuongTrinh> GetDanhSachChuongTrinh(bool isLocked)
        {
            List<PSDanhMucChuongTrinh> lst = new List<PSDanhMucChuongTrinh>();
            var db = new DataObjects();
            var results = db.GetDanhSachChuongTrinh(isLocked);
            if (results != null)
            {
                return results;
            }
            else return lst;
        }
        public static PSTiepNhan GetThongTinTiepNhan(long rowID)
        {
            var db = new DataObjects();
            return db.GetThongTinTiepNhan(rowID);
        }
        public static PSTiepNhan GetThongTinTiepNhanTheoMaPhieu(string maPhieu)
        {
            var db = new DataObjects();
            return db.GetThongTinTiepNhanTheoMaPhieu(maPhieu);
        }
        public static List<PsBaoCaoTuyChon> GetBaoCaoTuyChonTrangThai(DateTime NgayBD, DateTime NgayKT, string MaDonVi, int TrangThai)
        {
            var db = new DataObjects();
            if (MaDonVi == "all")
            {
                return db.GetBaoCaoTuyChonTrangThai(NgayBD, NgayKT, TrangThai);
            }
            else
            {
                return db.GetBaoCaoTuyChonTrangThai(NgayBD, NgayKT, TrangThai);
            }
        }
        public static List<PsBaoCaoTuyChon> GetBaoCaoTuyChon(DateTime NgayBD, DateTime NgayKT, string MaDonVi)
        {
            var db = new DataObjects();
            if (MaDonVi == "all")
            {
                return db.GetBaoCaoTuyChon(NgayBD, NgayKT);
            }
            else
            {
                return db.GetBaoCaoTuyChon(NgayBD, NgayKT, MaDonVi);
            }

        }
        public static List<pro_Report_BaoCaoTuyChonResult> GetBaoCaoTuyChonNew(DateTime NgayBD, DateTime NgayKT, string MaDonVi)
        {
            var db = new DataObjects();
            return db.GetBaoCaoTuyChonNew(NgayBD, NgayKT, MaDonVi);

        }
        //public static rptChiTietTrungTam GetBaoCaoTrungTamCoBan(DateTime tuNgay, DateTime denNgay)
        //{

        //}
        public static List<ClassDieuKienLocBaoCao.ChanDoan> GetDieuKienLocBaoCao_ChanDoan()
        {
            List<ClassDieuKienLocBaoCao.ChanDoan> lst = new List<ClassDieuKienLocBaoCao.ChanDoan>();
            ClassDieuKienLocBaoCao.ChanDoan cd1 = new ClassDieuKienLocBaoCao.ChanDoan();
            cd1.idChanDoan = 0;
            cd1.ChanDoanBenh = "Tất cả chẩn đoán";
            ClassDieuKienLocBaoCao.ChanDoan cd2 = new ClassDieuKienLocBaoCao.ChanDoan();
            cd2.idChanDoan = 1;
            cd2.ChanDoanBenh = "Âm tính";
            ClassDieuKienLocBaoCao.ChanDoan cd3 = new ClassDieuKienLocBaoCao.ChanDoan();
            cd3.idChanDoan = 2;
            cd3.ChanDoanBenh = "Dương tính";
            ClassDieuKienLocBaoCao.ChanDoan cd4 = new ClassDieuKienLocBaoCao.ChanDoan();
            cd4.idChanDoan = 3;
            cd4.ChanDoanBenh = "Dương tính giả";
            ClassDieuKienLocBaoCao.ChanDoan cd5 = new ClassDieuKienLocBaoCao.ChanDoan();
            cd5.idChanDoan = 4;
            cd5.ChanDoanBenh = "Dương tính thật";
            lst.Add(cd1); lst.Add(cd2); lst.Add(cd3); lst.Add(cd4); lst.Add(cd5);
            return lst;
        }
        public static List<ClassDieuKienLocBaoCao.ChatLuongMau> GetDieuKienLocBaoCao_ChatLuongMau()
        {
            List<ClassDieuKienLocBaoCao.ChatLuongMau> lst = new List<ClassDieuKienLocBaoCao.ChatLuongMau>();
            ClassDieuKienLocBaoCao.ChatLuongMau cl1 = new ClassDieuKienLocBaoCao.ChatLuongMau();
            cl1.idCLM = 3;
            cl1.CLM = "Tất cả";
            ClassDieuKienLocBaoCao.ChatLuongMau cl2 = new ClassDieuKienLocBaoCao.ChatLuongMau();
            cl2.idCLM = 1;
            cl2.CLM = "Đạt";
            ClassDieuKienLocBaoCao.ChatLuongMau cl3 = new ClassDieuKienLocBaoCao.ChatLuongMau();
            cl3.idCLM = 0;
            cl3.CLM = "Không đạt";
            lst.Add(cl1); lst.Add(cl2); lst.Add(cl3);
            return lst;
        }
        public static List<ClassDieuKienLocBaoCao.GioiTinh> GetDieuKienLocBaoCao_GioiTinh()
        {
            List<ClassDieuKienLocBaoCao.GioiTinh> lst = new List<ClassDieuKienLocBaoCao.GioiTinh>();
            ClassDieuKienLocBaoCao.GioiTinh cl1 = new ClassDieuKienLocBaoCao.GioiTinh();
            cl1.idGT = 3;
            cl1.NameGioiTinh = "Tất cả";
            ClassDieuKienLocBaoCao.GioiTinh cl2 = new ClassDieuKienLocBaoCao.GioiTinh();
            cl2.idGT = 0;
            cl2.NameGioiTinh = "Nam";
            ClassDieuKienLocBaoCao.GioiTinh cl3 = new ClassDieuKienLocBaoCao.GioiTinh();
            cl3.idGT = 1;
            cl3.NameGioiTinh = "Nữ";
            ClassDieuKienLocBaoCao.GioiTinh cl4 = new ClassDieuKienLocBaoCao.GioiTinh();
            cl4.idGT = 2;
            cl4.NameGioiTinh = "N/A";
            lst.Add(cl1); lst.Add(cl2); lst.Add(cl3); lst.Add(cl4);
            return lst;
        }
        public static List<PSDanhMucGoiDichVuChung> GetDieuKienLocBaoCao_GoiXN()
        {
            List<PSDanhMucGoiDichVuChung> lst = new List<PSDanhMucGoiDichVuChung>();
            PSDanhMucGoiDichVuChung goi = new PSDanhMucGoiDichVuChung();
            goi.IDGoiDichVuChung = "all";
            goi.TenGoiDichVuChung = "Tất cả";
            lst.Add(goi);
            var db = new DataObjects();
            var lstgoi = db.GetDanhMucGoiXetNghiemChung(string.Empty);
            if (lstgoi.Count > 0)
            {
                foreach (var goixn in lstgoi)
                {
                    if (!goixn.IDGoiDichVuChung.Equals("DVGXN0001") && !goixn.IDGoiDichVuChung.Equals("DVGXNL2"))
                        lst.Add(goixn);
                }
            }
            return lst;
        }
        public static List<PSDanhMucChuongTrinh> GetDieuKienLocBaoCao_ChuongTrinh()
        {
            List<PSDanhMucChuongTrinh> lst = new List<PSDanhMucChuongTrinh>();
            PSDanhMucChuongTrinh ctr = new PSDanhMucChuongTrinh();
            ctr.IDChuongTrinh = "all";
            ctr.TenChuongTrinh = "Tất cả";
            lst.Add(ctr);
            var db = new DataObjects();
            var lstct = db.GetDanhSachChuongTrinh(false);
            foreach (var ct in lstct)
            {
                lst.Add(ct);
            }
            return lst;
        }
        public static List<PSDanhMucDichVu> GetDieuKienLocBaoCao_Benh()
        {
            List<PSDanhMucDichVu> lst = new List<PSDanhMucDichVu>();
            PSDanhMucDichVu dv = new PSDanhMucDichVu();
            dv.IDDichVu = "all";
            dv.TenHienThiDichVu = "Tất cả";
            lst.Add(dv);
            var db = new DataObjects();
            var lstdv = db.GetDanhSachDichVu(false);
            if (lstdv.Count > 0)
                foreach (var dvv in lstdv)
                { lst.Add(dvv); }
            return lst;
        }
        public static List<PSDanhMucDonViCoSo> GetDieuKienLocBaoCao_DonVi(string maChiCuc)
        {
            List<PSDanhMucDonViCoSo> lstcc = new List<PSDanhMucDonViCoSo>();
            PSDanhMucDonViCoSo dv = new PSDanhMucDonViCoSo();
            dv.MaDVCS = "all";
            dv.TenDVCS = "Tất cả";
            lstcc.Add(dv);
            var db = new DataObjects();
            var listcc = db.GetDanhSachDonVi(maChiCuc);
            lstcc.AddRange(listcc);
            return lstcc;
        }
        public static List<PSDanhMucDonViCoSo> GetDanhSachDonViSMS(string maChiCuc)
        {
            List<PSDanhMucDonViCoSo> lstcc = new List<PSDanhMucDonViCoSo>();
            PSDanhMucDonViCoSo dv = new PSDanhMucDonViCoSo();
            dv.MaDVCS = "all";
            dv.TenDVCS = "Tất cả";
            lstcc.Add(dv);
            var db = new DataObjects();
            var listcc = db.GetDanhSachDonViSMS(maChiCuc);
            lstcc.AddRange(listcc);
            return lstcc;
        }

        public static List<PSDanhMucChiCuc> GetDieuKienLocBaoCao_ChiCuc()
        {
            List<PSDanhMucChiCuc> lstcc = new List<PSDanhMucChiCuc>();
            PSDanhMucChiCuc cc = new PSDanhMucChiCuc();
            cc.MaChiCuc = "all";
            cc.TenChiCuc = "Tất cả";
            lstcc.Add(cc);
            var db = new DataObjects();
            var listcc = db.GetDanhMucChiCuc();
            foreach (var ccc in listcc)
            {
                lstcc.Add(ccc);
            }
            return lstcc;
        }
        public static List<PSDanhMucDanToc> GetDieuKienLocBaoCao_DanToc()
        {
            List<PSDanhMucDanToc> lst = new List<PSDanhMucDanToc>();
            PSDanhMucDanToc dt = new PSDanhMucDanToc();
            dt.IDDanToc = 0;
            dt.TenDanToc = "Tất cả";
            lst.Add(dt);
            var db = new DataObjects();
            var lstdt = db.GetDanhSachDanToc(1);
            if (lstdt.Count > 0)
                foreach (var dtt in lstdt)
                {
                    lst.Add(dtt);
                }
            return lst;
        }
        public static string GetTenDanToc(int maDanToc)
        {
            var db = new DataObjects();
            return db.GetTenDanToc(maDanToc);
        }
            public static List<ClassDieuKienLocBaoCao.LoaiMau> GetDieuKienLocBaoCao_LoaiMau()
        {
            List<ClassDieuKienLocBaoCao.LoaiMau> lst = new List<ClassDieuKienLocBaoCao.LoaiMau>();
            ClassDieuKienLocBaoCao.LoaiMau cl1 = new ClassDieuKienLocBaoCao.LoaiMau();
            cl1.idLoai = 3;
            cl1.NameLoai = "Tất cả";
            ClassDieuKienLocBaoCao.LoaiMau cl2 = new ClassDieuKienLocBaoCao.LoaiMau();
            cl2.idLoai = 0;
            cl2.NameLoai = "Mẫu mới";
            ClassDieuKienLocBaoCao.LoaiMau cl3 = new ClassDieuKienLocBaoCao.LoaiMau();
            cl3.idLoai = 1;
            cl3.NameLoai = "Mẫu lấy lại";

            lst.Add(cl1); lst.Add(cl2); lst.Add(cl3);
            return lst;
        }
        public static List<ClassDieuKienLocBaoCao.NguyCo> GetDieuKienLocBaoCao_NguyCo()
        {
            List<ClassDieuKienLocBaoCao.NguyCo> lst = new List<ClassDieuKienLocBaoCao.NguyCo>();
            ClassDieuKienLocBaoCao.NguyCo nc1 = new ClassDieuKienLocBaoCao.NguyCo();
            nc1.idNC = 0;
            nc1.TenNguyCo = "Tất cả";
            ClassDieuKienLocBaoCao.NguyCo nc2 = new ClassDieuKienLocBaoCao.NguyCo();
            nc2.idNC = 1;
            nc2.TenNguyCo = "Nguy cơ cao";
            ClassDieuKienLocBaoCao.NguyCo nc3 = new ClassDieuKienLocBaoCao.NguyCo();
            nc3.idNC = 2;
            nc3.TenNguyCo = "Nguy cơ thấp";
            ClassDieuKienLocBaoCao.NguyCo nc4 = new ClassDieuKienLocBaoCao.NguyCo();
            nc4.idNC = 3;
            nc4.TenNguyCo = "Nghi ngờ";
            lst.Add(nc1); lst.Add(nc2); lst.Add(nc3); lst.Add(nc4);
            return lst;

        }
        public static List<ClassDieuKienLocBaoCao.PPSinh> GetDieuKienLocBaoCao_PhuongPhapSinh()
        {
            List<ClassDieuKienLocBaoCao.PPSinh> lst = new List<ClassDieuKienLocBaoCao.PPSinh>();
            ClassDieuKienLocBaoCao.PPSinh cl1 = new ClassDieuKienLocBaoCao.PPSinh();
            cl1.idPPS = 0;
            cl1.PhuongPhapSinh = "Tất cả";
            ClassDieuKienLocBaoCao.PPSinh cl2 = new ClassDieuKienLocBaoCao.PPSinh();
            cl2.idPPS = 1;
            cl2.PhuongPhapSinh = "Sinh thường";
            ClassDieuKienLocBaoCao.PPSinh cl3 = new ClassDieuKienLocBaoCao.PPSinh();
            cl3.idPPS = 2;
            cl3.PhuongPhapSinh = "Sinh mổ";
            ClassDieuKienLocBaoCao.PPSinh cl4 = new ClassDieuKienLocBaoCao.PPSinh();
            cl4.idPPS = 3;
            cl4.PhuongPhapSinh = "Không xác định";
            lst.Add(cl1); lst.Add(cl2); lst.Add(cl3); lst.Add(cl4);
            return lst;
        }
        public static List<ClassDieuKienLocBaoCao.ViTriLayMau> GetDieuKienLocBaoCao_ViTriLayMau()
        {
            List<ClassDieuKienLocBaoCao.ViTriLayMau> lst = new List<ClassDieuKienLocBaoCao.ViTriLayMau>();
            ClassDieuKienLocBaoCao.ViTriLayMau cl1 = new ClassDieuKienLocBaoCao.ViTriLayMau();
            cl1.idViTri = 0;
            cl1.TenViTriLayMau = "Tất cả";
            ClassDieuKienLocBaoCao.ViTriLayMau cl2 = new ClassDieuKienLocBaoCao.ViTriLayMau();
            cl2.idViTri = 1;
            cl2.TenViTriLayMau = "Gót chân";
            ClassDieuKienLocBaoCao.ViTriLayMau cl3 = new ClassDieuKienLocBaoCao.ViTriLayMau();
            cl3.idViTri = 2;
            cl3.TenViTriLayMau = "Tĩnh mạch";
            ClassDieuKienLocBaoCao.ViTriLayMau cl4 = new ClassDieuKienLocBaoCao.ViTriLayMau();
            cl4.idViTri = 3;
            cl4.TenViTriLayMau = "Khác";
            lst.Add(cl1); lst.Add(cl2); lst.Add(cl3); lst.Add(cl4);
            return lst;
        }


        #endregion GET
        #region Hàm Xư Lý 
        //public static DataTable ReadExcelContents(string fileName)
        //{
        //    try
        //    {
        //        OleDbConnection connection = new OleDbConnection();

        //        connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName); //Excel 97-2003, .xls
        //        string excelQuery = @"Select [CODE],[CONC] 
        //           FROM [Sheet1$]";

        //        //string excelQuery = @"Select * FROM [Sheet1$]";        
        //        connection.Open();
        //        OleDbCommand cmd = new OleDbCommand(excelQuery, connection);
        //        OleDbDataAdapter adapter = new OleDbDataAdapter();
        //        adapter.SelectCommand = cmd;
        //        DataSet ds = new DataSet();
        //        adapter.Fill(ds);
        //        DataTable dt = ds.Tables[0];
        //        connection.Close();
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        private static string SoBanDau()
        {
            try
            {
                var db = new DataObjects();
                var dat = db.GetDateTimeServer();
                string s1 = (dat.Year.ToString()).Trim().Substring(DateTime.Now.Year.ToString().Trim().ToString().Length - 2);
                string s2 = (dat.Month.ToString()).PadLeft(2, '0');
                return s1 + s2;
            }
            catch
            {
                string s1 = (DateTime.Now.Year.ToString()).Trim().Substring(DateTime.Now.Year.ToString().Trim().ToString().Length - 2);
                string s2 = (DateTime.Now.Month.ToString()).PadLeft(2, '0');
                return s1 + s2;
            }
        }
        public static DateTime GetDateTime()
        {
            var db = new DataObjects();
            return db.GetDateTimeServer();
        }
        public DataSet CreateDataSet<T>(List<T> list)
        {
            //list is nothing or has nothing, return nothing (or add exception handling)
            if (list == null || list.Count == 0) { return null; }

            //get the type of the first obj in the list
            var obj = list[0].GetType();

            //now grab all properties
            var properties = obj.GetProperties();

            //make sure the obj has properties, return nothing (or add exception handling)
            if (properties.Length == 0) { return null; }

            //it does so create the dataset and table
            var dataSet = new DataSet();
            var dataTable = new DataTable();

            //now build the columns from the properties
            var columns = new DataColumn[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                columns[i] = new DataColumn(properties[i].Name, properties[i].PropertyType);
            }

            //add columns to table
            dataTable.Columns.AddRange(columns);

            //now add the list values to the table
            foreach (var item in list)
            {
                //create a new row from table
                var dataRow = dataTable.NewRow();

                //now we have to iterate thru each property of the item and retrieve it's value for the corresponding row's cell
                var itemProperties = item.GetType().GetProperties();

                for (int i = 0; i < itemProperties.Length; i++)
                {
                    dataRow[i] = itemProperties[i].GetValue(item, null);
                }

                //now add the populated row to the table
                dataTable.Rows.Add(dataRow);
            }

            //add table to dataset
            dataSet.Tables.Add(dataTable);

            //return dataset
            return dataSet;
        }
        #endregion Hàm Convert
        #region Version Multi Language

        public static List<PSMenuItem> GetMenuItem(string IDForm)
        {
            List<PSMenuItem> psmenu = new List<PSMenuItem>();
            var db = new DataObjects();
            try
            {
                psmenu = db.GetMenuItem(IDForm);
            }
            catch
            {

            }
            return psmenu;
        }

        public static void AddMenuItem(List<PSMenuItem> lst, long? IDForm)
        {
            var db = new DataObjects();
            try
            {
                db.AddMenuItem(lst, IDForm);
            }
            catch
            {

            }
        }
        public static void SetLanguage(int lang)
        {
            var db = new DataObjects();
            try
            {
                db.SetLanguage(lang);
            }
            catch
            {

            }
        }

        public static void AddMenuForm(PSMenuForm fo)
        {
            var db = new DataObjects();
            try
            {
                db.AddMenuForm(fo);
            }
            catch
            {
            }
        }

        public static long? GetMenuIDForm(string formName)
        {
            var db = new DataObjects();
            return db.GetMenuIDForm(formName);
        }

        public static PSMenuTrans TransItem(long? IDForm, string NameItem)
        {
            var db = new DataObjects();
            PSMenuTrans kq = new PSMenuTrans();
            try
            {
                kq = db.TransItem(IDForm, NameItem);
            }
            catch
            {
            }
            return kq;
        }
        public static List<PSMenuTrans> Trans(long? IDForm)
        {
            var db = new DataObjects();
            List<PSMenuTrans> kq = new List<PSMenuTrans>();
            try
            {
                kq = db.Trans(IDForm);
            }
            catch
            {
            }
            return kq;
        }
        public static List<PSMenuTrans> TransAll(long? IDForm)
        {
            var db = new DataObjects();
            List<PSMenuTrans> kq = new List<PSMenuTrans>();
            try
            {
                kq = db.TransAll(IDForm);
            }
            catch
            {
            }
            return kq;
        }

        public static List<PSMenuForm> GetMenuForm()
        {
            var db = new DataObjects();
            List<PSMenuForm> kq = new List<PSMenuForm>();
            try
            {
                kq = db.GetMenuForm();
            }
            catch
            {

            }
            return kq;
        }

        #endregion
        #region Duyệt phiếu
        public static PsReponse DuyetPhieuBT(PSTTPhieu phieu)
        {
            var db = new BioData();
            return db.DuyetPhieuBT(phieu);
        }
        #endregion
        #region Sửa lỗi
        public static PsReponse UpdateMaKhachHang()
        {
            var db = new DataObjects();
            return db.UpdateMaKhachHang();
        }
        #endregion
        public static PsReponseSMS SMS(string content, string sdt, bool codau)
        {
            PsReponseSMS reponse = new PsReponseSMS();
            BioNetDAL.WebReference.CcApi api = new BioNetDAL.WebReference.CcApi();
            api.Timeout = 6000;
            string iscodau;
            if (codau)
            {
                iscodau = "1";
            }
            else
            {
                iscodau = "0";
            }

            result r = api.wsCpMt("smsbrand_bionet", "123456a@", "BIONET", "1", sdt, sdt, "BIONET", "bulksms", content, iscodau);
            if (r.result1 == 1)
            {
                reponse.Result = true;

            }
            else
            {
                reponse.Result = false;
                reponse.StringError = r.message;
            }
            return reponse;
        }
        public static PsReponse InsertSMSNumber(PSDanhSachGuiSMS sms, PsReponseSMS kq, string MaNV)
        {
            var db = new DataObjects();
            return db.InsertSMSNumber(sms, kq, MaNV);
        }
        public static List<PsDanhSachMauDuongTinh> GetDanhSachDuongTinh(DateTime NgayBD, DateTime NgayKT, string TenDichVu, string DonVi, string Min, string Max)
        {
            var db = new DataObjects();
            return db.GetDanhSachDuongTinh(NgayBD, NgayKT, TenDichVu, DonVi, Min, Max);
        }
        public static List<PsDanhSachMauDuongTinh> GetDanhSachDuongTinhNew(DateTime NgayBD, DateTime NgayKT, string TenDichVu, string DonVi, string Min, string Max)
        {
            var db = new DataObjects();
            return db.GetDanhSachDuongTinhNew(NgayBD, NgayKT, TenDichVu, DonVi, Min, Max);
        }
        public static List<PSBaoCaoTuyChonDichVu> LoadDSBaoCaoTuyChonDichVu(DateTime NgayBD, DateTime NgayKT, string MaDV, string MaDichVu)
        {
            var db = new DataObjects();
            return db.LoadDSBaoCaoTuyChonDichVu(NgayBD, NgayKT, MaDV, MaDichVu);
        }
        public static List<pro_ThongKeTheoDichVuResult> LoadDSBaoCaoTuyChonDichVuNew(DateTime NgayBD, DateTime NgayKT, string MaDichVu, string MaDV)
        {
            var db = new DataObjects();
            return db.LoadDSBaoCaoTuyChonDichVuNew(NgayBD, NgayKT, MaDichVu, MaDV);
        }
        public static List<PSBaoCaoTuyChonDonVi> LoadDSThongKeDonVi(DateTime NgayBD, DateTime NgayKT, string MaDV)
        {
            var db = new DataObjects();
            return db.LoadDSThongKeDonVi(NgayBD, NgayKT, MaDV);
        }
        public static List<pro_Report_BaoCaoTongHopTheoBenhNhanResult> LoadDSThongKeTheoBenhNhan(DateTime NgayBD, DateTime NgayKT, string MaDV)
        {
            var db = new DataObjects();
            return db.LoadDSThongKeTheoBenhNhan(NgayBD, NgayKT, MaDV);
        }
        public static List<PSThongKeTheoDonVi> LoadDSThongKeDonViCT(List<PSBaoCaoTuyChonDonVi> lst)
        {
            var db = new DataObjects();
            return db.LoadDSThongKeDonViCT(lst);
        }
        public static PSThongKeTheoDonVi LoadDSThongKeChiCucTNew(List<pro_Report_BaoCaoTongHopTheoBenhNhanResult> lst, string ChiCuc, int STT)
        {
            var db = new DataObjects();
            return db.LoadDSThongKeChiCucTNew(lst,ChiCuc,STT);
        }
            public static List<PSThongKeTheoDonVi> LoadDSThongKeDonViCTNew(List<pro_Report_BaoCaoTongHopTheoBenhNhanResult> lst)
        {
            var db = new DataObjects();
            return db.LoadDSThongKeDonViCTNew(lst);
        }
        public static PSThongKeTheoDonVi LoadDSThongKeDVNew(List<pro_Report_BaoCaoTongHopTheoBenhNhanResult> lst, PSThongKeTheoDonVi dv,int phieu)
        {
            var db = new DataObjects();
            return db.LoadDSThongKeDVNew(lst, dv, phieu);
        }
        public static PSThongKeTheoDonVi LoadDSThongKeDV(List<PSBaoCaoTuyChonDonVi> lst, PSThongKeTheoDonVi dv,int phieu)
        {
            var db = new DataObjects();
            return db.LoadDSThongKeDV(lst, dv,phieu);
        }
        public static PSThongKePDFXetNghiem GetBaoCaoDonViXNCoBan(List<pro_Report_BaoCaoTongHopTheoBenhNhanResult> lst, string MaDVCS, DateTime NgayBD, DateTime NgayKT)
        {
            var db = new DataObjects();
            return db.GetBaoCaoDonViXNCoBan(lst, MaDVCS, NgayBD, NgayKT);
        }
        #region Save Tree Report
        public static string GetFileReport(string Type, string EndFile)
        {
            var db = new BioData();
            return db.GetFileReport(Type, EndFile);
        }
        public static string SaveFileReport(string Folder,string Type, string Unit,DateTime dateBD, DateTime dateKT,string EndFile)
        {
            var db = new BioData();
            return db.SaveFileReport(Folder,Type, Unit, dateBD, dateKT,EndFile);
        }
        public static string SaveFileTemp( string Group, string Unit, DateTime dateBD, DateTime dateKT, string EndFile)
        {
            var db = new BioData();
            return db.SaveFileTemp( Group, Unit, dateBD, dateKT, EndFile);
        }
        public static string GetFileGanViTri(string Group, string Machine, string STT)
        {
            var db = new BioData();
            return db.GetFileGanViTri(Group, Machine,STT);
        }
        public static string GetFileGanViTriTemp(string Machine, string STT, string IDMayXN)
        {
            var db = new BioData();
            return db.GetFileGanViTriTemp( Machine, STT,IDMayXN);
        }
        public static PSDanhMucMayXN GetTTMayXN(string IDMayXN)
        {
            var db = new BioData();
            return db.GetTTMayXN(IDMayXN);
        }

        public static string convertToUnSign(string s)
        {
            var db = new BioData();
            return db.convertToUnSign(s);
        }
       
            #endregion
        }
    }
