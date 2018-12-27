﻿using System;
using System.Collections.Generic;
using System.Linq;
using BioNetModel.Data;
using BioNetModel;
using System.Data;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Transactions;

namespace BioNetDAL
{
    public class BioData
    {
        public static BioNetDBContextDataContext db = null;
        public BioData()
        {
            try
            {
                string str = calltringconect();
                db = new BioNetDBContextDataContext(str);
                db.CommandTimeout = 5000;
            }
            catch (Exception ex) { }
        }
        private ServerInfo serverInfo = new ServerInfo();

        public SqlConnection sqlConnection;
        private string calltringconect()
        {
            try
            {
                string connectionstring;
                string pathFileName = (Application.StartupPath) + "\\xml\\configiBionet.xml";
                ServerInfo server = this.LoadFileXml(pathFileName);
                if (server != null && server.Encrypt == "true")
                {
                    this.serverInfo.Encrypt = server.Encrypt;
                    this.serverInfo.ServerName = this.DecryptString(server.ServerName, true);
                    this.serverInfo.Database = this.DecryptString(server.Database, true);
                    this.serverInfo.UserName = this.DecryptString(server.UserName, true);
                    this.serverInfo.Password = this.DecryptString(server.Password, true);
                }
                else
                {
                    this.serverInfo.Encrypt = "false";
                    this.serverInfo.ServerName = "powersoft.vn";
                    this.serverInfo.Database = "Bio";
                    this.serverInfo.UserName = "sa";
                    this.serverInfo.Password = "****";
                }
                connectionstring = "Data Source=" + this.serverInfo.ServerName + ";Initial Catalog=" + this.serverInfo.Database + ";User Id=" + this.serverInfo.UserName + ";Password=" + this.serverInfo.Password + ";";
                return connectionstring;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public ServerInfo LoadFileXml(string FileName)
        {
            using (var stream = System.IO.File.OpenRead(FileName))
            {
                var serializer = new XmlSerializer(typeof(ServerInfo));
                return serializer.Deserialize(stream) as ServerInfo;
            }
        }
        public string DecryptString(string toDecrypt, bool useHashing)
        {
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
                if (useHashing)
                {
                    var hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes("2$Powersoft.vn"));
                }
                else keyArray = Encoding.UTF8.GetBytes("2$Powersoft.vn");
                var tdes = new TripleDESCryptoServiceProvider
                {
                    Key = keyArray,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Encoding.UTF8.GetString(resultArray);
            }
            catch { return toDecrypt; }
        }
        #region Tạo mã
        public string GetNewMaKhachHang(string maDonVi, string chuoiNamThang) //Tạp mã khách hàng
        {

            int numID = 0;
            string maKH = string.Empty;
            string startStr = maDonVi + chuoiNamThang;
            try
            {
                var res = db.PSPatients.Where(p => p.MaKhachHang.StartsWith(maDonVi + chuoiNamThang) && p.isXoa == false).ToList();
                if (res != null)
                {
                    if (res.Count > 0)
                    {
                        var idRow = res.Max(p => p.RowIDBenhNhan);
                        numID = int.Parse(res.FirstOrDefault(p => p.RowIDBenhNhan == idRow).MaKhachHang.Substring(12, 4)) + 1;
                        if (numID <= 9)
                            maKH = startStr + "000" + numID;
                        else if (numID > 9 && numID <= 99)
                            maKH = startStr + "00" + numID;
                        else if (numID > 99 && numID <= 999)
                            maKH = startStr + "0" + numID;
                        else maKH = startStr + numID;
                    }
                    else maKH = startStr + "0001";
                }
                else maKH = startStr + "0001";
            }
            catch
            {
                maKH = startStr + "0001";
            }
            return maKH;
        }
        private static string SoBanDau() //Số bắt đầu
        {
            try
            {
                var dat = db.ExecuteQuery<DateTime>("Select GETDATE()").FirstOrDefault();
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
        private string GetID() //Láy mã GUILD
        {
            var maTT = db.PSThongTinTrungTams.FirstOrDefault().MaVietTat;
            if (!string.IsNullOrEmpty(maTT))
            {
                return maTT + Guid.NewGuid().ToString();
            }
            else { return "00" + Guid.NewGuid().ToString(); }
        }
        #endregion

        #region Đánh giá và duyệt phiếu
        // Duyệt nhanh phiếu
        public PsReponse DuyetNhanh(string maTiepNhan, string maPhieu) // Duyệt nhanh phiếu
        {
            PsReponse result = new PsReponse();
            try
            {
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();
                var kq = db.PSXN_TraKetQuas.FirstOrDefault(p => p.isXoa == false && p.MaPhieu == maPhieu && p.MaTiepNhan == maTiepNhan);
                if (kq != null)
                {
                    kq.isDaDuyetKQ = true;
                    kq.isTraKQ = true;
                    kq.isDongBo = false;
                    kq.NgayTraKQ = DateTime.Now;
                }
                db.SubmitChanges();
                var lstkq = db.PSXN_TraKQ_ChiTiets.Where(p => p.isXoa == false && p.MaPhieu == maPhieu && p.MaTiepNhan == maTiepNhan).ToList();
                if (lstkq.Count > 0)
                {
                    foreach (var item in lstkq)
                    {
                        item.isDaDuyetKQ = true;
                        item.isDongBo = false;
                        db.SubmitChanges();
                    }
                }
                var phieu = db.PSPhieuSangLocs.FirstOrDefault(p => p.IDPhieu == maPhieu && p.isXoa == false);
                if (phieu != null)
                {

                    phieu.TrangThaiMau = 4;

                    db.SubmitChanges();
                }
                result.Result = true;
                db.Transaction.Commit();
                db.Connection.Close();
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                result.Result = false;
                if (string.IsNullOrEmpty(result.StringError))
                    result.StringError = ex.ToString();
            }
            return result;
        }
        public PsReponse InsertChiDinhTheoDanhSachHangLoat(PSTiepNhan dotTiepNhan, string maNVChiDinh, string maGoiXn)// Duyệt chị định hàng loạt
        {
            PsReponse result = new PsReponse();
            result.Result = true;
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            bool isNhapLieu = false;
            try
            {
                if (string.IsNullOrEmpty(dotTiepNhan.MaPhieu))
                {
                    result.Result = false;
                    result.StringError = "Không có thông tin mã phiếu!";
                    throw new Exception("Không có thông tin mã phiếu!");
                }
                else
                {
                    var phieu = db.PSPhieuSangLocs.FirstOrDefault(p => p.isXoa == false && p.IDPhieu == dotTiepNhan.MaPhieu && p.IDCoSo == dotTiepNhan.MaDonVi);
                    string _maGoiXN = maGoiXn;

                    if (phieu != null)
                    {
                        if (!string.IsNullOrEmpty(phieu.MaBenhNhan))
                        {
                            var bn = db.PSPatients.FirstOrDefault(p => p.isXoa == false && p.MaBenhNhan == phieu.MaBenhNhan);
                            if (bn != null)
                            {
                                if (!string.IsNullOrEmpty(bn.MotherName) && !string.IsNullOrEmpty(bn.MotherPhoneNumber) && !string.IsNullOrEmpty(bn.TenBenhNhan) && !string.IsNullOrEmpty(bn.DiaChi) && phieu.NgayGioLayMau != null)
                                    isNhapLieu = true;
                            }
                        }
                        if (string.IsNullOrEmpty(phieu.MaGoiXN))
                        {
                            phieu.MaGoiXN = maGoiXn;
                            phieu.isXoa = false;
                            phieu.isDongBo = false;
                            phieu.TrangThaiPhieu = true;
                            if (phieu.TrangThaiMau < 2)
                                phieu.TrangThaiMau = 2;
                            phieu.isXNLan2 = phieu.isXNLan2 ?? false;
                            phieu.isLayMauLan2 = phieu.isLayMauLan2 ?? false;

                            db.SubmitChanges();
                        }
                        else _maGoiXN = phieu.MaGoiXN;
                    }
                    else
                    {
                        PSPhieuSangLoc phSangLoc = new PSPhieuSangLoc();
                        phSangLoc.IDCoSo = dotTiepNhan.MaDonVi;
                        phSangLoc.IDPhieu = dotTiepNhan.MaPhieu;
                        phSangLoc.MaGoiXN = _maGoiXN;
                        phSangLoc.isDongBo = false;
                        phSangLoc.isXoa = false;
                        phSangLoc.TrangThaiPhieu = true;
                        phSangLoc.TrangThaiMau = 2;
                        phSangLoc.isXNLan2 = false;
                        phSangLoc.isLayMauLan2 = false;
                        phSangLoc.IDNhanVienTaoPhieu = maNVChiDinh;
                        phSangLoc.isKhongDat = false;
                        db.PSPhieuSangLocs.InsertOnSubmit(phSangLoc);
                        db.SubmitChanges();
                    }
                    var dongia = db.PSDanhMucGoiDichVuTheoDonVis.FirstOrDefault(p => p.IDGoiDichVuChung == _maGoiXN && p.MaDVCS == dotTiepNhan.MaDonVi).DonGia ?? 0;


                    PSChiDinhDichVu cd = new PSChiDinhDichVu();
                    cd.IDGoiDichVu = _maGoiXN;
                    cd.MaNVChiDinh = maNVChiDinh;
                    cd.MaChiDinh = cd.MaChiDinh = "CD" + GetID();
                    cd.isLayMauLai = false;
                    cd.MaNVChiDinh = maNVChiDinh;
                    cd.DonGia = dongia;
                    cd.MaDonVi = dotTiepNhan.MaDonVi;
                    cd.MaPhieu = dotTiepNhan.MaPhieu;
                    cd.MaTiepNhan = dotTiepNhan.MaTiepNhan;
                    cd.NgayChiDinhHienTai = DateTime.Now;
                    cd.NgayChiDinhLamViec = DateTime.Now;
                    cd.isDaNhapLieu = isNhapLieu;
                    cd.NgayTiepNhan = dotTiepNhan.NgayTiepNhan;
                    cd.SoLuong = 1;
                    cd.TrangThai = 1;
                    cd.isDongBo = false;
                    cd.isXoa = false;
                    db.PSChiDinhDichVus.InsertOnSubmit(cd);
                    db.SubmitChanges();
                    var listChiTietCD = db.PSChiTietGoiDichVuChungs.Where(p => p.IDGoiDichVuChung == _maGoiXN).ToList();

                    if (listChiTietCD.Count > 0)
                    {
                        foreach (var item in listChiTietCD)
                        {
                            try
                            {
                                var ttdv = db.PSDanhMucDichVus.FirstOrDefault(x => x.IDDichVu == phieu.IDPhieu) ;
                                PSChiDinhDichVuChiTiet chitiet = new PSChiDinhDichVuChiTiet();
                                chitiet.GiaTien = ttdv.GiaDichVu;
                                chitiet.isDongBo = false;
                                chitiet.isXetNghiemLan2 = false;
                                chitiet.isXoa = false;
                                chitiet.MaChiDinh = cd.MaChiDinh;
                                chitiet.MaDichVu = ttdv.IDDichVu;
                                chitiet.MaDonVi = cd.MaDonVi;
                                chitiet.MaGoiDichVu = maGoiXn;
                                chitiet.MaPhieu = cd.MaPhieu;
                                chitiet.SoLuong = 1;
                                db.PSChiDinhDichVuChiTiets.InsertOnSubmit(chitiet);
                                db.SubmitChanges();
                            }
                            catch
                            {
                                result.Result = false;
                                result.StringError = "Chưa khai báo dịch vụ  cho gói" + _maGoiXN + " Mã dịch vụ chưa khai báo :" + item.IDDichVu;
                                throw new Exception("Chưa khai báo dịch vụ  cho gói!");
                            }
                        }

                    }
                    else
                    {
                        result.Result = false;
                        result.StringError = "Chưa khai báo gói dịch vụ chung";
                        throw new Exception("Chưa khai báo gói dịch vụ chung");
                    }
                    var _Tiepnhan = db.PSTiepNhans.FirstOrDefault(p => p.isXoa == false && p.MaTiepNhan == dotTiepNhan.MaTiepNhan);
                    if (_Tiepnhan != null)
                    {
                        _Tiepnhan.isDongBo = false;
                        _Tiepnhan.isDaDanhGia = true;
                        _Tiepnhan.isDaNhapLieu = isNhapLieu;
                        db.SubmitChanges();
                    }
                }

                db.Transaction.Commit();
                db.Connection.Close();

            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                result.Result = false;
                if (string.IsNullOrEmpty(result.StringError))
                    result.StringError = ex.ToString();

            }
            finally
            {

            }
            return result;
        }
        //Duyệt phiếu
        public PsReponse DuyetPhieuThuCong(PSTTPhieu ttphieu)
        {
            PsReponse reponse = new PsReponse();
            try
            {
                #region Phiếu sàng lọc
                PSPhieuSangLoc ph = db.PSPhieuSangLocs.FirstOrDefault(p => p.isXoa != true && p.IDPhieu.Equals(ttphieu.MaPhieu) && p.IDCoSo.Equals(ttphieu.MaDonVi));
                if (ph == null)
                {
                    ph = ttphieu.Phieu;
                    ph.NgayTaoPhieu = DateTime.Now;
                    ph.IDNhanVienTaoPhieu = ttphieu.MaNV;
                    if (string.IsNullOrEmpty(ph.IDPhieuLan1))
                    {
                        ph.isLayMauLan2 = true;
                    }
                    else
                    {
                        ph.isLayMauLan2 = false;
                    }
                    ph.TrangThaiPhieu = true;
                    ph.MaBenhNhan = GetID();
                    ttphieu.MaBenhNhan = ph.MaBenhNhan;
                    ph.TrangThaiMau = 2;
                    ph.isDongBo = false;
                    ph.isXoa = false;
                    db.PSPhieuSangLocs.InsertOnSubmit(ph);
                    db.SubmitChanges();
                }
                else
                {
                    ph.isNheCan = ttphieu.Phieu.isNheCan;
                    ph.isSinhNon = ttphieu.Phieu.isSinhNon;
                    ph.isTruoc24h = ttphieu.Phieu.isTruoc24h;
                    ph.MaXetNghiem = ttphieu.Phieu.MaXetNghiem;
                    ph.NgayGioLayMau = ttphieu.Phieu.NgayGioLayMau;
                    ph.NgayNhanMau = ttphieu.Phieu.NgayNhanMau;
                    ph.NgayTruyenMau = ttphieu.Phieu.NgayTruyenMau;
                    ph.NoiLayMau = ttphieu.Phieu.NoiLayMau;
                    ph.DiaChiLayMau = ttphieu.Phieu.DiaChiLayMau;
                    ph.Para = ttphieu.Phieu.Para;
                    ph.SDTNhanVienLayMau = ttphieu.Phieu.SDTNhanVienLayMau;
                    ph.SLTruyenMau = ttphieu.Phieu.SLTruyenMau;
                    ph.TenNhanVienLayMau = ttphieu.Phieu.TenNhanVienLayMau;
                    ph.TinhTrangLucLayMau = ttphieu.Phieu.TinhTrangLucLayMau;
                    ph.TrangThaiPhieu = true;
                    if (string.IsNullOrEmpty(ph.MaBenhNhan))
                    {
                        ph.MaBenhNhan = GetID();
                    }
                    else
                    {
                        ph.MaBenhNhan = ttphieu.Phieu.MaBenhNhan;
                    }
                    ttphieu.MaBenhNhan = ph.MaBenhNhan;
                    if (ph.TrangThaiMau < 2)
                    {
                        ph.IDPhieuLan1 = ttphieu.Phieu.IDPhieuLan1;
                        if (!string.IsNullOrEmpty(ph.IDPhieuLan1))
                        {
                            ph.isLayMauLan2 = true;
                        }
                        else ph.isLayMauLan2 = false;
                        ph.MaGoiXN = ttphieu.Phieu.MaGoiXN;
                        ph.TrangThaiMau = 2;
                    }
                    ph.isXoa = false;
                    ph.isDongBo = false;
                    db.SubmitChanges();
                }
                #endregion

                #region Pattent
                PSPatient bn = new PSPatient();
                if (!string.IsNullOrEmpty(ttphieu.MaBenhNhan))
                {
                    bn = db.PSPatients.FirstOrDefault(p => p.MaBenhNhan.Equals(ttphieu.MaBenhNhan) && p.isXoa != true);
                    if (bn != null)
                    {
                        bn.FatherName = ttphieu.Benhnhan.FatherName;
                        bn.MotherName = ttphieu.Benhnhan.MotherName;
                        bn.FatherPhoneNumber = ttphieu.Benhnhan.FatherPhoneNumber;
                        bn.FatherBirthday = ttphieu.Benhnhan.FatherBirthday;
                        bn.MotherBirthday = ttphieu.Benhnhan.MotherBirthday;
                        bn.MotherPhoneNumber = ttphieu.Benhnhan.MotherPhoneNumber;
                        if (string.IsNullOrEmpty(bn.MaKhachHang))
                        {
                            bn.MaKhachHang = GetNewMaKhachHang(ttphieu.Phieu.IDCoSo, SoBanDau());
                        }
                        bn.DiaChi = ttphieu.Benhnhan.DiaChi;
                        bn.TenBenhNhan = ttphieu.Benhnhan.TenBenhNhan;
                        bn.QuocTichID = ttphieu.Benhnhan.QuocTichID;
                        bn.TuanTuoiKhiSinh = ttphieu.Benhnhan.TuanTuoiKhiSinh;
                        bn.PhuongPhapSinh = ttphieu.Benhnhan.PhuongPhapSinh;
                        bn.NoiSinh = ttphieu.Benhnhan.NoiSinh;
                        bn.NgayGioSinh = ttphieu.Benhnhan.NgayGioSinh;
                        bn.GioiTinh = ttphieu.Benhnhan.GioiTinh;
                        bn.DanTocID = ttphieu.Benhnhan.DanTocID;
                        bn.CanNang = ttphieu.Benhnhan.CanNang;
                        bn.Para = ttphieu.Benhnhan.Para;
                        bn.isXoa = false;
                        bn.isDongBo = false;
                        db.SubmitChanges();
                    }
                    else
                    {
                        bn = ttphieu.Benhnhan;
                        bn.MaBenhNhan = ttphieu.MaBenhNhan;
                        if (string.IsNullOrEmpty(bn.MaKhachHang))
                        {
                            bn.MaKhachHang = GetNewMaKhachHang(ttphieu.Phieu.IDCoSo, SoBanDau());
                        }
                        bn.isDongBo = false;
                        bn.isXoa = false;
                        db.PSPatients.InsertOnSubmit(bn);
                        db.SubmitChanges();
                    }
                }
                else
                {
                    reponse.Result = false;
                    reponse.StringError = "Lỗi không có mã bệnh nhân";
                    return reponse;
                }
                #endregion

                ttphieu.isDaNhapLieu = KiemTraNhapLieu(bn, ph);
                #region Chỉ định phiếu
                PSChiDinhDichVu cd = new PSChiDinhDichVu();
                if (!string.IsNullOrEmpty(ttphieu.MaChiDinh))
                {
                    cd = db.PSChiDinhDichVus.FirstOrDefault(p => p.MaChiDinh.Equals(ttphieu.MaChiDinh) && p.MaDonVi.Equals(ttphieu.MaDonVi)
                     && p.MaPhieu.Equals(ttphieu.MaPhieu) && p.MaTiepNhan.Equals(ttphieu.MaTiepNhan));
                    if (cd == null)
                    {
                        cd.IDGoiDichVu = ttphieu.MaGoiXN;
                        cd.isLayMauLai = false;
                        cd.MaChiDinh = "CD" + GetID();
                        ttphieu.MaChiDinh = cd.MaChiDinh;
                        cd.isDaNhapLieu = ttphieu.isDaNhapLieu;
                        cd.MaDonVi = ttphieu.MaDonVi;
                        cd.MaNVChiDinh = ttphieu.MaNV;
                        cd.IDNhanVienChiDinh = ttphieu.MaNV;
                        cd.MaTiepNhan = ttphieu.MaTiepNhan;
                        cd.MaPhieu = ttphieu.MaPhieu;
                        cd.MaPhieuLan1 = ttphieu.MaPhieu1;
                        cd.NgayChiDinhHienTai = DateTime.Now;
                        cd.NgayChiDinhLamViec = DateTime.Now;
                        cd.SoLuong = 1;
                        cd.TrangThai = 1;
                        cd.NgayTiepNhan = ttphieu.TiepNhan.NgayTiepNhan;
                        cd.DonGia = ttphieu.DonGiaGoi;
                        cd.isXoa = false;
                        cd.isDongBo = false;
                        db.PSChiDinhDichVus.InsertOnSubmit(cd);
                        db.SubmitChanges();
                    }
                    else
                    {
                        cd.TrangThai = 1;
                        cd.isDongBo = false;
                        cd.isXoa = false;
                        cd.isDaNhapLieu = ttphieu.isDaNhapLieu;
                        db.SubmitChanges();
                    }
                }
                else
                {
                    cd.IDGoiDichVu = ttphieu.MaGoiXN;
                    cd.isLayMauLai = false;
                    cd.MaChiDinh = "CD" + GetID();
                    ttphieu.MaChiDinh = cd.MaChiDinh;
                    cd.isDaNhapLieu = ttphieu.isDaNhapLieu;
                    cd.MaDonVi = ttphieu.MaDonVi;
                    cd.MaNVChiDinh = ttphieu.MaNV;
                    cd.IDNhanVienChiDinh = ttphieu.MaNV;
                    cd.MaTiepNhan = ttphieu.MaTiepNhan;
                    cd.MaPhieu = ttphieu.MaPhieu;
                    cd.MaPhieuLan1 = ttphieu.MaPhieu1;
                    cd.NgayChiDinhHienTai = DateTime.Now;
                    cd.NgayChiDinhLamViec = DateTime.Now;
                    cd.SoLuong = 1;
                    cd.TrangThai = 1;
                    cd.NgayTiepNhan = ttphieu.TiepNhan.NgayTiepNhan;
                    cd.DonGia = ttphieu.DonGiaGoi;
                    cd.isXoa = false;
                    cd.isDongBo = false;
                    db.PSChiDinhDichVus.InsertOnSubmit(cd);
                    db.SubmitChanges();
                }

                foreach (var ctdv in ttphieu.lstChiDinhDichVu)
                {
                    var kq = db.PSChiDinhDichVuChiTiets.FirstOrDefault(x => x.MaChiDinh.Equals(ttphieu.MaChiDinh) && x.MaChiDinh.Equals(ctdv.IDDichVu));
                    if (kq == null)
                    {
                        PSChiDinhDichVuChiTiet CTiet = new PSChiDinhDichVuChiTiet();
                        CTiet.MaChiDinh = ttphieu.MaChiDinh;
                        CTiet.MaDichVu = ctdv.IDDichVu;
                        CTiet.MaPhieu = ttphieu.MaPhieu;
                        CTiet.MaGoiDichVu = ttphieu.MaGoiXN;
                        CTiet.MaDonVi = ttphieu.MaDonVi;
                        CTiet.isXetNghiemLan2 = false;
                        CTiet.isDongBo = false;
                        CTiet.isXoa = false;
                        CTiet.SoLuong = 1;
                        CTiet.GiaTien = ctdv.GiaDichVu;
                        db.PSChiDinhDichVuChiTiets.InsertOnSubmit(CTiet);
                        db.SubmitChanges();
                    }
                    else
                    {
                        kq.MaChiDinh = ttphieu.MaChiDinh;
                        kq.MaDichVu = ctdv.IDDichVu;
                        kq.MaPhieu = ttphieu.MaPhieu;
                        kq.MaGoiDichVu = ttphieu.MaGoiXN;
                        kq.MaDonVi = ttphieu.MaDonVi;
                        kq.isXetNghiemLan2 = false;
                        kq.isDongBo = false;
                        kq.isXoa = false;
                        kq.SoLuong = 1;
                        kq.GiaTien = ctdv.GiaDichVu;
                        db.SubmitChanges();
                    }
                }
                #endregion

                #region Chi tiết đánh giá chất lượng
                var CTDanhGIa = db.PSChiTietDanhGiaChatLuongs.Where(p => p.MaTiepNhan.Equals(ttphieu.MaTiepNhan) && p.IDPhieu.Equals(ttphieu.MaPhieu)).ToList();
                db.PSChiTietDanhGiaChatLuongs.DeleteAllOnSubmit(CTDanhGIa);
                db.SubmitChanges();
                if (ttphieu.lstLyDoKhongDat.Count > 0)
                {
                    foreach (var ld in ttphieu.lstLyDoKhongDat)
                    {
                        ld.isDongBo = false;
                        ld.isXoa = false;
                        ld.MaTiepNhan = ttphieu.MaTiepNhan;
                        db.PSChiTietDanhGiaChatLuongs.InsertOnSubmit(ld);
                        db.SubmitChanges();
                    }
                }
                #endregion

                #region Phiếu tiếp nhận
                PSTiepNhan TiepNhan = db.PSTiepNhans.FirstOrDefault(p => p.isXoa != true && p.MaPhieu.Equals(ttphieu.MaPhieu) && p.MaDonVi.Equals(ttphieu.MaDonVi));
                if (TiepNhan != null)
                {
                    TiepNhan.isDongBo = false;
                    TiepNhan.isDaDanhGia = true;
                    TiepNhan.isDaNhapLieu = ttphieu.isDaNhapLieu;
                    db.SubmitChanges();
                }
                #endregion
                reponse.Result = true;
            }
            catch (Exception ex)
            {
                reponse.Result = false;
                reponse.StringError = ex.ToString();
            }
            return reponse;
        }
        public bool KiemTraNhapLieu(PSPatient pa,PSPhieuSangLoc ph)
        {
            bool isDaNhapLieu = true;
            if(string.IsNullOrEmpty(pa.TenBenhNhan))
            {
                isDaNhapLieu = false;
                return isDaNhapLieu;
            }
            if (string.IsNullOrEmpty(pa.NgayGioSinh.Value.ToString()))
            {
                isDaNhapLieu = false;
                return isDaNhapLieu;
            }
            if (string.IsNullOrEmpty(pa.MotherName))
            {
                isDaNhapLieu = false;
                return isDaNhapLieu;
            }
            if (string.IsNullOrEmpty(pa.MotherBirthday.Value.ToString()))
            {
                isDaNhapLieu = false;
                return isDaNhapLieu;
            }
            if (string.IsNullOrEmpty(pa.DiaChi))
            {
                isDaNhapLieu = false;
                return isDaNhapLieu;
            }
            if (string.IsNullOrEmpty(pa.NoiSinh))
            {
                isDaNhapLieu = false;
                return isDaNhapLieu;
            }
            if (string.IsNullOrEmpty(pa.TuanTuoiKhiSinh.ToString()))
            {
                isDaNhapLieu = false;
                return isDaNhapLieu;
            }
            if (string.IsNullOrEmpty(ph.NgayGioLayMau.ToString()))
            {
                isDaNhapLieu = false;
                return isDaNhapLieu;
            }
            if (string.IsNullOrEmpty(ph.TenNhanVienLayMau))
            {
                isDaNhapLieu = false;
                return isDaNhapLieu;
            }
            if (string.IsNullOrEmpty(ph.SDTNhanVienLayMau))
            {
                isDaNhapLieu = false;
                return isDaNhapLieu;
            }
            if (string.IsNullOrEmpty(ph.MaGoiXN))
            {
                isDaNhapLieu = false;
                return isDaNhapLieu;
            }
            return isDaNhapLieu;
        }
            public PsReponse DuyetPhieuBT(PSTTPhieu ttphieu)
        {
            PsReponse reponse = new PsReponse();
            try
            {
                string MaBN=string.Empty;
                string MaCD = string.Empty;
                string MaDV = ttphieu.Phieu.IDCoSo;
                string MaPhieu = ttphieu.Phieu.IDPhieu;
                string MaTiepNhan = ttphieu.TiepNhan.MaTiepNhan;
                string MaGoiXN = ttphieu.Phieu.MaGoiXN;
                bool isNhapLieu = false;
                if (!string.IsNullOrEmpty(ttphieu.Benhnhan.MotherName) && !string.IsNullOrEmpty(ttphieu.Benhnhan.MotherPhoneNumber) && !string.IsNullOrEmpty(ttphieu.Benhnhan.TenBenhNhan) && !string.IsNullOrEmpty(ttphieu.Benhnhan.DiaChi) && ttphieu.Phieu.NgayGioLayMau != null)
                    isNhapLieu = true;
                bool isMaulaylai = false;
                var ph = db.PSPhieuSangLocs.FirstOrDefault(p => p.isXoa == false && p.IDPhieu == ttphieu.Phieu.IDPhieu);
                if (ph != null)
                {
                    ph.isNheCan = ttphieu.Phieu.isNheCan;
                    ph.isSinhNon = ttphieu.Phieu.isSinhNon;
                    ph.isTruoc24h = ttphieu.Phieu.isTruoc24h;
                    ph.MaXetNghiem = ttphieu.Phieu.MaXetNghiem;
                    ph.NgayGioLayMau = ttphieu.Phieu.NgayGioLayMau;
                    ph.NgayNhanMau = ttphieu.Phieu.NgayNhanMau;
                    ph.NgayTruyenMau = ttphieu.Phieu.NgayTruyenMau;
                    ph.NoiLayMau = ttphieu.Phieu.NoiLayMau;
                    ph.DiaChiLayMau = ttphieu.Phieu.DiaChiLayMau;
                    ph.Para = ttphieu.Phieu.Para;
                    ph.SDTNhanVienLayMau = ttphieu.Phieu.SDTNhanVienLayMau;
                    ph.SLTruyenMau = ttphieu.Phieu.SLTruyenMau;
                    ph.TenNhanVienLayMau = ttphieu.Phieu.TenNhanVienLayMau;
                    ph.TinhTrangLucLayMau = ttphieu.Phieu.TinhTrangLucLayMau;
                    if(string.IsNullOrEmpty(ph.MaBenhNhan))
                    {
                        ph.MaBenhNhan = GetID();
                    }
                    else
                    {
                        ph.MaBenhNhan = ttphieu.Phieu.MaBenhNhan;
                    }
                    
                    MaBN = ph.MaBenhNhan;
                    if (ph.TrangThaiMau < 2)
                    {
                        ph.IDPhieuLan1 = ttphieu.Phieu.IDPhieuLan1;
                        if (!string.IsNullOrEmpty(ph.IDPhieuLan1))
                        {
                            ph.isLayMauLan2 = true;
                        }
                        else ph.isLayMauLan2 = false;
                        ph.MaGoiXN = ttphieu.Phieu.MaGoiXN;
                        ph.TrangThaiMau = 2; 
                    }
                    ph.isXoa = false;
                    ph.isDongBo = false;
                    ph.TrangThaiPhieu = true;
                    db.SubmitChanges();
                }
                else
                {
                    PSPhieuSangLoc p = new PSPhieuSangLoc();
                    p = ttphieu.Phieu;
                    if (string.IsNullOrEmpty(p.IDPhieuLan1))
                    {
                        p.isLayMauLan2 = true;
                    }
                    else
                    {
                        p.isLayMauLan2 = false;
                    }
                    p.TrangThaiPhieu = true;
                    p.MaBenhNhan = GetID();
                    MaBN = p.MaBenhNhan;
                    p.TrangThaiMau = 2;
                    p.isDongBo = false;
                    p.isXoa = false;
                    db.PSPhieuSangLocs.InsertOnSubmit(p);
                    db.SubmitChanges();
                }

                if (!string.IsNullOrEmpty(MaBN))
                {
                    var bn = db.PSPatients.FirstOrDefault(p => p.MaBenhNhan == MaBN && p.isXoa != true);
                    if (bn != null)
                    {
                       
                        bn.FatherName = ttphieu.Benhnhan.FatherName;
                        bn.MotherName = ttphieu.Benhnhan.MotherName;
                        bn.FatherPhoneNumber = ttphieu.Benhnhan.FatherPhoneNumber;
                        bn.FatherBirthday = ttphieu.Benhnhan.FatherBirthday;
                        bn.MotherBirthday = ttphieu.Benhnhan.MotherBirthday;
                        bn.MotherPhoneNumber = ttphieu.Benhnhan.MotherPhoneNumber;
                        if (string.IsNullOrEmpty(bn.MaKhachHang))
                            bn.MaKhachHang = GetNewMaKhachHang(ttphieu.Phieu.IDCoSo, SoBanDau());
                        bn.DiaChi = ttphieu.Benhnhan.DiaChi;
                        bn.TenBenhNhan = ttphieu.Benhnhan.TenBenhNhan;
                        bn.QuocTichID = ttphieu.Benhnhan.QuocTichID;
                        bn.TuanTuoiKhiSinh = ttphieu.Benhnhan.TuanTuoiKhiSinh;
                        bn.PhuongPhapSinh = ttphieu.Benhnhan.PhuongPhapSinh;
                        bn.NoiSinh = ttphieu.Benhnhan.NoiSinh;
                        bn.NgayGioSinh = ttphieu.Benhnhan.NgayGioSinh;
                        bn.GioiTinh = ttphieu.Benhnhan.GioiTinh;
                        bn.DanTocID = ttphieu.Benhnhan.DanTocID;
                        bn.CanNang = ttphieu.Benhnhan.CanNang;
                        bn.Para = ttphieu.Benhnhan.Para;
                        bn.isXoa = false;
                        bn.isDongBo = false;
                        db.SubmitChanges();
                    }
                    else
                    {
                        PSPatient bnh = new PSPatient();
                        bnh = ttphieu.Benhnhan;
                        bnh.MaBenhNhan = MaBN;
                        if (string.IsNullOrEmpty(bnh.MaKhachHang))
                            bnh.MaKhachHang = GetNewMaKhachHang(ttphieu.Phieu.IDCoSo, SoBanDau());
                        bnh.isDongBo = false;
                        bnh.isXoa = false;
                        db.PSPatients.InsertOnSubmit(bnh);
                        db.SubmitChanges();
                    }
                }
                else
                {
                    reponse.Result = false;
                    reponse.StringError = "Lỗi không có mã bệnh nhân";
                    return reponse;
                    //PSPatient bnh = new PSPatient();
                    //bnh = ttphieu.Benhnhan;
                    //bnh.MaBenhNhan = MaBN;
                    //bnh.MaKhachHang = GetNewMaKhachHang(ttphieu.Phieu.IDCoSo, SoBanDau());
                    //bnh.isDongBo = false;
                    //bnh.isXoa = false;
                    //db.PSPatients.InsertOnSubmit(bnh);
                    //db.SubmitChanges();
                }

                if (!string.IsNullOrEmpty(ttphieu.ChiDinh.MaChiDinh))
                {
                    var dvv = db.PSChiDinhDichVus.FirstOrDefault(p => p.MaChiDinh == ttphieu.ChiDinh.MaChiDinh);
                    if (dvv == null)
                    {
                        PSChiDinhDichVu cd = new PSChiDinhDichVu();
                        cd.IDGoiDichVu = ttphieu.ChiDinh.IDGoiDichVu;
                        cd.IDNhanVienChiDinh = ttphieu.ChiDinh.IDNhanVienChiDinh;
                        cd.isLayMauLai = ttphieu.ChiDinh.isLayMauLai;
                        cd.MaChiDinh = ttphieu.ChiDinh.MaChiDinh;
                        cd.MaDonVi = ttphieu.ChiDinh.MaDonVi;
                        cd.MaNVChiDinh = ttphieu.ChiDinh.MaNVChiDinh;
                        cd.MaTiepNhan = ttphieu.ChiDinh.MaTiepNhan;
                        cd.MaPhieu = ttphieu.ChiDinh.MaPhieu;
                        cd.MaPhieuLan1 = ttphieu.ChiDinh.MaPhieuLan1;
                        cd.NgayChiDinhHienTai = ttphieu.ChiDinh.NgayChiDinhHienTai;
                        cd.NgayChiDinhLamViec = ttphieu.ChiDinh.NgayChiDinhLamViec;
                        cd.SoLuong = ttphieu.ChiDinh.SoLuong;
                        cd.NgayTiepNhan = ttphieu.ChiDinh.NgayTiepNhan;
                        cd.DonGia = ttphieu.ChiDinh.DonGia;
                        cd.TrangThai = ttphieu.ChiDinh.TrangThai==null?1: ttphieu.ChiDinh.TrangThai;
                        cd.isDongBo = false;
                        cd.isXoa = false;
                        cd.isDaNhapLieu = isNhapLieu;
                        MaCD = cd.MaChiDinh;
                        db.PSChiDinhDichVus.InsertOnSubmit(cd);
                        db.SubmitChanges();
                    }
                    else
                    {
                        dvv.TrangThai = ttphieu.ChiDinh.TrangThai == null ? 1 : ttphieu.ChiDinh.TrangThai;
                        dvv.isDongBo = false;
                        dvv.isXoa = false;
                        dvv.isDaNhapLieu = isNhapLieu;
                        MaCD = dvv.MaChiDinh;
                        db.SubmitChanges();
                    }
                }
                else
                {
                    PSChiDinhDichVu cd = new PSChiDinhDichVu();
                    cd.IDGoiDichVu = ttphieu.ChiDinh.IDGoiDichVu;
                    cd.IDNhanVienChiDinh = ttphieu.ChiDinh.MaNVChiDinh;
                    cd.isLayMauLai = ttphieu.ChiDinh.isLayMauLai;
                    if (cd.isLayMauLai==true)
                    {
                        cd.MaChiDinh = "XN" + GetID();
                        isMaulaylai = true;
                    }  
                    else
                    {
                        cd.MaChiDinh = "CD" + GetID();
                    }
                    cd.isDaNhapLieu = isNhapLieu;
                    cd.MaDonVi = ttphieu.ChiDinh.MaDonVi;
                    cd.MaNVChiDinh = ttphieu.ChiDinh.MaNVChiDinh;
                    cd.MaTiepNhan = ttphieu.ChiDinh.MaTiepNhan;
                    cd.MaPhieu = ttphieu.ChiDinh.MaPhieu;
                    cd.MaPhieuLan1 = ttphieu.ChiDinh.MaPhieuLan1;
                    cd.NgayChiDinhHienTai = ttphieu.ChiDinh.NgayChiDinhHienTai;
                    cd.NgayChiDinhLamViec = ttphieu.ChiDinh.NgayChiDinhLamViec;
                    cd.SoLuong = ttphieu.ChiDinh.SoLuong;
                    cd.TrangThai =1;
                    cd.NgayTiepNhan = ttphieu.ChiDinh.NgayTiepNhan;
                    cd.DonGia = ttphieu.ChiDinh.DonGia;
                    cd.isXoa = false;
                    cd.isDongBo = false;
                    db.PSChiDinhDichVus.InsertOnSubmit(cd);
                    db.SubmitChanges();
                    MaCD = cd.MaChiDinh;
                }         
                    foreach(var ctdv in  ttphieu.lstChiDinhDichVu)
                    {
                    var kq = db.PSChiDinhDichVuChiTiets.FirstOrDefault(x => x.MaChiDinh.Equals(MaCD) && x.MaChiDinh.Equals(ctdv.IDDichVu));
                    if(kq==null)
                    {
                        PSChiDinhDichVuChiTiet CTiet = new PSChiDinhDichVuChiTiet();
                        CTiet.MaChiDinh = MaCD;
                        CTiet.MaDichVu = ctdv.IDDichVu;
                        CTiet.MaPhieu = ttphieu.Phieu.IDPhieu;
                        CTiet.MaGoiDichVu = ttphieu.Phieu.MaGoiXN;
                        CTiet.MaDonVi = ttphieu.Phieu.IDCoSo;
                        CTiet.isXetNghiemLan2 = isMaulaylai;
                        CTiet.isDongBo = false;
                        CTiet.isXoa = false;
                        CTiet.SoLuong = 1;
                        CTiet.GiaTien = ctdv.GiaDichVu;
                    }
                        
                    }
                var lstld = db.PSChiTietDanhGiaChatLuongs.Where(p => p.MaTiepNhan == MaTiepNhan && p.IDPhieu == MaPhieu).ToList();
                    db.PSChiTietDanhGiaChatLuongs.DeleteAllOnSubmit(lstld);
                    db.SubmitChanges();
                    if (ttphieu.lstLyDoKhongDat.Count > 0)
                    {
                        foreach (var ld in ttphieu.lstLyDoKhongDat)
                        {
                            ld.isDongBo = false;
                            ld.isXoa = false;
                            db.PSChiTietDanhGiaChatLuongs.InsertOnSubmit(ld);
                            db.SubmitChanges();
                        }
                    }

                var results = db.PSTiepNhans.Where(p => p.isXoa == false && p.MaPhieu == ttphieu.Phieu.IDPhieu && p.MaDonVi == ttphieu.Phieu.IDCoSo).ToList();
                if (results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        result.isDongBo = false;
                        result.isDaDanhGia = true;
                        result.isDaNhapLieu = isNhapLieu;
                        db.SubmitChanges();
                    }
                }
                reponse.Result = true;
            }
            catch
            {
                reponse.Result = false;
            }
            return reponse;
        }
        public string InsertDotChiDinhDichVu(PSChiDinhDichVu dv, bool isLamLaiXN)
        {
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                string maCD = string.Empty;

                if (!string.IsNullOrEmpty(dv.MaChiDinh))
                {
                    var dvv = db.PSChiDinhDichVus.FirstOrDefault(p => p.MaChiDinh == dv.MaChiDinh);
                    if (dvv != null)
                    {
                        dvv.IDGoiDichVu = dv.IDGoiDichVu;
                        //  dvv.IDNhanVienChiDinh = dv.IDNhanVienChiDinh;
                        dvv.isLayMauLai = dv.isLayMauLai;
                        dvv.MaChiDinh = dv.MaChiDinh;
                        dvv.MaDonVi = dv.MaDonVi;
                        //dvv.MaNVChiDinh = dv.MaNVChiDinh;
                        dvv.MaPhieu = dv.MaPhieu;
                        // dvv.MaTiepNhan = dv.MaTiepNhan;
                        dvv.MaPhieuLan1 = dv.MaPhieuLan1;
                        dvv.NgayChiDinhHienTai = dv.NgayChiDinhHienTai;
                        dvv.NgayChiDinhLamViec = dv.NgayChiDinhLamViec;
                        dvv.NgayTiepNhan = dv.NgayTiepNhan;
                        dvv.SoLuong = dv.SoLuong;
                        dvv.isXoa = false;
                        dvv.isDongBo = false;
                        dvv.TrangThai = dv.TrangThai;
                        db.SubmitChanges();
                        maCD = dvv.MaChiDinh;
                    }
                    else
                    {
                        PSChiDinhDichVu cd = new PSChiDinhDichVu();
                        cd.IDGoiDichVu = dv.IDGoiDichVu;
                        cd.IDNhanVienChiDinh = dv.IDNhanVienChiDinh;
                        cd.isLayMauLai = dv.isLayMauLai;
                        cd.MaChiDinh = dv.MaChiDinh;
                        cd.MaDonVi = dv.MaDonVi;
                        cd.MaNVChiDinh = dv.MaNVChiDinh;
                        cd.MaTiepNhan = dv.MaTiepNhan;
                        cd.MaPhieu = dv.MaPhieu;
                        cd.MaPhieuLan1 = dv.MaPhieuLan1;
                        cd.NgayChiDinhHienTai = dv.NgayChiDinhHienTai;
                        cd.NgayChiDinhLamViec = dv.NgayChiDinhLamViec;
                        cd.SoLuong = dv.SoLuong;
                        cd.NgayTiepNhan = dv.NgayTiepNhan;
                        cd.TrangThai = dv.TrangThai;
                        cd.isDongBo = false;
                        cd.isXoa = false;
                        db.PSChiDinhDichVus.InsertOnSubmit(cd);
                        db.SubmitChanges();
                        maCD = cd.MaChiDinh;
                    }

                }
                else
                {
                    PSChiDinhDichVu cd = new PSChiDinhDichVu();
                    cd.IDGoiDichVu = dv.IDGoiDichVu;
                    cd.IDNhanVienChiDinh = dv.MaNVChiDinh;
                    cd.isLayMauLai = dv.isLayMauLai;
                    if (isLamLaiXN)
                        cd.MaChiDinh = "XN" + GetID();
                    else
                        cd.MaChiDinh = "CD" + GetID();
                    cd.MaDonVi = dv.MaDonVi;
                    cd.MaNVChiDinh = dv.MaNVChiDinh;
                    cd.MaTiepNhan = dv.MaTiepNhan;
                    cd.MaPhieu = dv.MaPhieu;
                    cd.MaPhieuLan1 = dv.MaPhieuLan1;
                    cd.NgayChiDinhHienTai = dv.NgayChiDinhHienTai;
                    cd.NgayChiDinhLamViec = dv.NgayChiDinhLamViec;
                    cd.SoLuong = dv.SoLuong;
                    cd.TrangThai = dv.TrangThai;
                    cd.NgayTiepNhan = dv.NgayTiepNhan;
                    cd.isXoa = false;
                    cd.isDongBo = false;
                    db.PSChiDinhDichVus.InsertOnSubmit(cd);
                    db.SubmitChanges();
                    maCD = cd.MaChiDinh;
                }
                var results = db.PSTiepNhans.Where(p => p.isXoa == false && p.MaPhieu == dv.MaPhieu && p.MaDonVi == dv.MaDonVi).ToList();
                if (results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        result.isDongBo = false;
                        result.isDaDanhGia = true;
                        db.SubmitChanges();
                    }
                }
                db.Transaction.Commit();
                db.Connection.Close();
                return maCD;
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                return string.Empty;
            }
        }
        public bool InsertDotChiDinhDichVuChiTiet(PSChiDinhDichVuChiTiet ct)
        {
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                if (!string.IsNullOrEmpty(ct.RowIDDichVuChiTiet.ToString()))
                {
                    if (ct.RowIDDichVuChiTiet > 0)
                    {
                        var row = db.PSChiDinhDichVuChiTiets.FirstOrDefault(p => p.RowIDDichVuChiTiet == ct.RowIDDichVuChiTiet);
                        if (row != null)
                        {
                            row.GiaTien = ct.GiaTien;
                            row.isXetNghiemLan2 = ct.isXetNghiemLan2;
                            row.MaChiDinh = ct.MaChiDinh;
                            row.MaDichVu = ct.MaDichVu;
                            row.MaDonVi = ct.MaDonVi;
                            row.MaGoiDichVu = ct.MaGoiDichVu;
                            row.MaPhieu = ct.MaPhieu;
                            row.SoLuong = ct.SoLuong;
                            row.isDongBo = false;
                            row.isXoa = false;
                            db.SubmitChanges();
                        }
                        {
                            PSChiDinhDichVuChiTiet ctt = new PSChiDinhDichVuChiTiet();
                            ctt.GiaTien = ct.GiaTien;
                            ctt.isXetNghiemLan2 = ct.isXetNghiemLan2;
                            ctt.MaChiDinh = ct.MaChiDinh;
                            ctt.MaDichVu = ct.MaDichVu;
                            ctt.MaDonVi = ct.MaDonVi;
                            ctt.MaGoiDichVu = ct.MaGoiDichVu;
                            ctt.MaPhieu = ct.MaPhieu;
                            ctt.SoLuong = ct.SoLuong;
                            ctt.isXoa = false;
                            ctt.isDongBo = false;
                            db.PSChiDinhDichVuChiTiets.InsertOnSubmit(ctt);
                            db.SubmitChanges();
                        }
                    }
                    else
                    {
                        PSChiDinhDichVuChiTiet ctt = new PSChiDinhDichVuChiTiet();
                        ctt.GiaTien = ct.GiaTien;
                        ctt.isXetNghiemLan2 = ct.isXetNghiemLan2;
                        ctt.MaChiDinh = ct.MaChiDinh;
                        ctt.MaDichVu = ct.MaDichVu;
                        ctt.MaDonVi = ct.MaDonVi;
                        ctt.MaGoiDichVu = ct.MaGoiDichVu;
                        ctt.MaPhieu = ct.MaPhieu;
                        ctt.SoLuong = ct.SoLuong;
                        ctt.isXoa = false;
                        ctt.isDongBo = false;
                        db.PSChiDinhDichVuChiTiets.InsertOnSubmit(ctt);
                        db.SubmitChanges();
                    }
                }

                db.Transaction.Commit();
                db.Connection.Close();
                return true;
            }
            catch
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                return false;
            }
        }
        public bool InsertLamLaiXetNghiem(string maPhieu, string maTiepNhan, string maNVChiDinh, List<PsDichVu> lstDVLamLai)
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
        public PSDanhMucMayXN GetTTMayXN(string IDMayXN)
        {
            return  db.PSDanhMucMayXNs.FirstOrDefault(x => x.IDMayXN.Equals(IDMayXN));
        }
        public PSDanhMucMayDucLo GetTTMayDucLo(string IDMayDucLo)
        {
            return db.PSDanhMucMayDucLos.FirstOrDefault(x => x.IDMayDucLo.Equals(IDMayDucLo));
        }
        #endregion
        public string convertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        #region Save Tree Report
        public string GetFileReport(string Type, string EndFile)
        {
            string LinkCenter = Application.StartupPath + "\\BaoCao";
            try
            {
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
                LinkCenter = LinkCenter + "\\" + Type;
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
                LinkCenter = LinkCenter + "\\" + EndFile;
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
                LinkCenter = LinkCenter + "\\" + DateTime.Now.ToString("yyyy.MM.dd");
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
                DirectoryInfo dirInfo = new DirectoryInfo(LinkCenter);
                DirectoryInfo[] childFolder = dirInfo.GetDirectories();
                LinkCenter = LinkCenter + "\\Serial" + (childFolder.ToList().Count() + 1);
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
            }
            catch
            {
            }
            return LinkCenter;
        }
        public string SaveFileReport(string Folder,string Type, string Unit,DateTime dateBD,DateTime dateKT,string EndFile)
        {
            string LinkCenter = Folder;
            try
            {
                Unit = convertToUnSign(Unit);
                Unit = Unit.Replace(" ", "");
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
                if (!dateBD.ToString().Equals(dateKT.ToString()))
                {
                    LinkCenter = LinkCenter + "\\BaoCao" + Type + "_" + Unit + "_" + dateBD.ToString("yyyy.MM.dd") + "_" + dateKT.ToString("yyyy.MM.dd")+EndFile;
                }
                else
                {
                    LinkCenter = LinkCenter + "\\BaoCao" + Type + "_" + Unit + "_" + dateBD.ToString("yyyy.MM.dd")+ EndFile;
                }
                if (File.Exists(LinkCenter))
                {
                    File.Delete(LinkCenter);
                }
            }
            catch
            {
            }
            return LinkCenter;
        }
        public string SaveFileTemp(string Group, string Unit, DateTime dateBD, DateTime dateKT, string EndFile)
        {
            string LinkCenter = Application.StartupPath + "\\BaoCao";
            try
            {
                Unit = convertToUnSign(Unit);
                Unit = Unit.Replace(" ", "");
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
                LinkCenter = LinkCenter + "\\Temp"  ;
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
                LinkCenter = LinkCenter + "\\" + Group;
                string LinkTT = LinkCenter;
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
                LinkCenter = LinkCenter + "\\" + DateTime.Now.ToString("yyyy.MM.dd");
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
                if (!dateBD.ToString().Equals(dateBD.ToString()))
                {
                    LinkCenter = LinkCenter + "\\Temp_" + Unit + "_" + dateBD.ToString("yyyy.MM.dd") + "_" + dateKT.ToString("yyyy.MM.dd.HH.mm") + EndFile;
                }
                else
                {
                    LinkCenter = LinkCenter + "\\Temp_" + Unit + "_" + DateTime.Now.ToString("yyyy.MM.dd.HH.mm") + EndFile;
                }
                if (File.Exists(LinkCenter))
                {
                    File.Delete(LinkTT);
                }
            }
            catch
            {
            }
            return LinkCenter;
        }
        public string GetFileGanViTri(string Group,string Machine,string STT)
        {
            string LinkCenter = Application.StartupPath + "\\SoDoGanViTriMayXN";
            try
            {
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
                LinkCenter = LinkCenter + "\\" + DateTime.Now.ToString("yyyy.MM.dd");
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
                string TenMayDL = convertToUnSign(GetTTMayDucLo(Machine).TenMayDucLo);
                TenMayDL = TenMayDL.Replace(" ", "");
                LinkCenter = LinkCenter + "\\" + TenMayDL;
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
                LinkCenter = LinkCenter + "\\" + Group;
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
                LinkCenter = LinkCenter + "\\Serial" + STT;
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
            }
            catch
            {
            }
            return LinkCenter;
        }
        public string GetFileGanViTriTemp(string Machine, string STT,string IDMayXN)
        {
            string LinkCenter = Application.StartupPath + "\\TestChart";
            try
            {
               
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
                LinkCenter = LinkCenter + "\\Temp";
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
                LinkCenter = LinkCenter + "\\" + DateTime.Now.ToString("yyyy.MM.dd");
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
                string TenMayDL = convertToUnSign(GetTTMayDucLo(Machine).TenMayDucLo);
                TenMayDL = TenMayDL.Replace(" ", "");
                LinkCenter = LinkCenter + "\\" + TenMayDL;
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
                string TenMay = convertToUnSign(GetTTMayXN(IDMayXN).TenMayXN);
                TenMay = TenMay.Replace(" ", "");
                LinkCenter = LinkCenter + "\\" + TenMay;
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
                LinkCenter = LinkCenter + "\\Serial" + STT;
                if (!File.Exists(LinkCenter))
                {
                    Directory.CreateDirectory(LinkCenter);
                }
            }
            catch
            {
            }
            return LinkCenter;
        }
      
        #endregion

    }
}
