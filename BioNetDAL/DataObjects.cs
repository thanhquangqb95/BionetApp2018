using System;
using System.Collections.Generic;
using System.Linq;
using BioNetModel.Data;
using BioNetModel;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace BioNetDAL
{
    public class DataObjects
    {

        public static BioNetDBContextDataContext db = null;
        public DataObjects()
        {
            try
            {
                string str = calltringconect();
                db = new BioNetDBContextDataContext(str);
                db.CommandTimeout = 5000;
                //db = new BioNetDBContextDataContext(DataContext.connectionString);

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
                //this.connectionstring = string.Empty;
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


        #region GET

        public PsReponse DeletePhieu(List<string> MaPhieu, bool Xoa, string maNV, string lydoXoa)
        {
            PsReponse res = new PsReponse();
            try
            {
                if (Xoa)
                {
                    foreach (var maphieu in MaPhieu)
                    {
                        //Xóa phiếu                     
                        var phieu1 = db.PSPhieuSangLocs.FirstOrDefault(x => x.IDPhieu == maphieu && x.isXoa == true);
                        if (phieu1 != null)
                        {
                            if (phieu1.TrangThaiMau == 7)
                            {
                                var phieu2 = db.PSPhieuSangLocs.FirstOrDefault(x => x.IDPhieuLan1 == maphieu && x.isXoa != true);
                                if (phieu2 != null)
                                {
                                    db.pro_DeletePhieu(phieu2.IDPhieu, false);
                                }
                            }

                        }
                        db.pro_DeletePhieu(maphieu, true);

                    }
                }
                else
                {
                    foreach (var maphieu in MaPhieu)
                    {
                        var ttphieu = GetThongTinPhieu(maphieu);

                        if (ttphieu.TrangThaiMau != 7)
                        {
                            //Cập nhật is Xóa =1
                            if (!string.IsNullOrEmpty(ttphieu.IDPhieuLan1))
                            {
                                var phieu1 = db.PSPhieuSangLocs.FirstOrDefault(x => x.IDPhieu == ttphieu.IDPhieuLan1);
                                phieu1.TrangThaiMau = 6;
                                phieu1.isDongBo = false;
                                db.SubmitChanges();
                            }
                            db.pro_DeletePhieu(maphieu, false);
                            PSChiTietXoaPhieu phieuxoa = new PSChiTietXoaPhieu();
                            phieuxoa.MaNV = maNV;
                            phieuxoa.DateDelete = DateTime.Now;
                            phieuxoa.MaPhieu = maphieu;
                            phieuxoa.LydoXoa = lydoXoa;
                            db.PSChiTietXoaPhieus.InsertOnSubmit(phieuxoa);
                            db.SubmitChanges();
                        }
                        else
                        {
                            res.Result = false;
                            res.StringError = " Phiếu " + maphieu + " đã được thu mẫu lần 2 nên không thể xóa \r\n";
                        }

                    }
                }
                if (string.IsNullOrEmpty(res.StringError))
                {
                    res.Result = true;
                }
                else
                {
                    res.Result = false;
                    res.StringError = "Chi tiết lỗi hủy phiếu:\r\n " + res.StringError;
                }
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.StringError = "Lỗi xóa phiếu - " + ex.ToString() + ".\r\n";
            }
            return res;

        }
        public TTPhieuCB GetBaoCaoThongTinPhieuTheoTime(string MaDonVi, DateTime NgayBD, DateTime NgayKT)
        {
            TTPhieuCB pcb = new TTPhieuCB();
            PsBaoCaoCoBanCT pcbct = new PsBaoCaoCoBanCT();
            List<rptBaoCaoSLPhieu> rpt = new List<rptBaoCaoSLPhieu>();
            List<PSPhieuSangLoc> phieusangloc = new List<PSPhieuSangLoc>();
            List<PSPatient> patient = new List<PSPatient>();
            List<PSXN_TraKetQua> traKQ = new List<PSXN_TraKetQua>();
            List<PSXN_TraKQ_ChiTiet> traKQCT = new List<PSXN_TraKQ_ChiTiet>();
            try
            {
                var TkSlPhieu = (from p in db.pro_Report_ThongKeSLPhieu(MaDonVi, NgayKT)
                                 select new
                                 {
                                     thang = p.thang,
                                     slphieu = p.SLphieu
                                 }).ToList();

                if (MaDonVi == "all")
                {
                    phieusangloc = db.PSPhieuSangLocs.Where(x => x.isXoa != true && x.TrangThaiMau > 3 && x.TrangThaiMau != 5 && x.NgayCoKQ.Value.Date <= NgayKT.Date && x.NgayCoKQ.Value.Date >= NgayBD.Date).ToList();
                    patient = (from pa in db.PSPatients
                               from ph in db.PSPhieuSangLocs
                               where (ph.isXoa != true && pa.isXoa != true && ph.TrangThaiMau > 3 && ph.TrangThaiMau != 5 && ph.NgayCoKQ != null && pa.MaBenhNhan == ph.MaBenhNhan && ph.NgayCoKQ.Value.Date <= NgayKT.Date && ph.NgayCoKQ.Value.Date >= NgayBD.Date)
                               select pa).ToList();
                    traKQ = (from pa in db.PSXN_TraKetQuas
                             from ph in db.PSPhieuSangLocs
                             where (ph.isXoa != true && pa.isXoa != true && pa.isTraKQ == true && ph.TrangThaiMau > 3 && ph.TrangThaiMau != 5 && pa.isDaDuyetKQ == true && pa.MaPhieu == ph.IDPhieu && ph.NgayCoKQ.Value.Date <= NgayKT.Date && ph.NgayCoKQ.Value.Date >= NgayBD.Date)
                             select pa).ToList();
                    traKQCT = (from pa in db.PSXN_TraKetQuas
                               from ph in db.PSPhieuSangLocs
                               from ct in db.PSXN_TraKQ_ChiTiets
                               where (ph.isXoa != true && pa.isXoa != true && pa.isTraKQ == true && ph.TrangThaiMau > 3 && ph.TrangThaiMau != 5 && pa.isDaDuyetKQ == true && pa.MaPhieu == ph.IDPhieu && ph.NgayCoKQ.Value.Date <= NgayKT.Date && ph.NgayCoKQ.Value.Date >= NgayBD.Date && pa.MaPhieu == ct.MaPhieu)
                               select ct).ToList();
                    patient = patient.GroupBy(x => x.MaBenhNhan).Select(g => g.First()).ToList();
                }
                else
                {
                    phieusangloc = db.PSPhieuSangLocs.Where(x => x.isXoa != true && x.TrangThaiMau > 3 && x.TrangThaiMau != 5 && x.NgayCoKQ.Value.Date <= NgayKT.Date && x.NgayCoKQ.Value.Date >= NgayBD.Date && x.IDCoSo.Contains(MaDonVi)).ToList();
                    patient = (from pa in db.PSPatients
                               from ph in db.PSPhieuSangLocs
                               where (ph.isXoa != true && pa.isXoa != true && ph.TrangThaiMau > 3 && ph.TrangThaiMau != 5 && ph.NgayCoKQ != null && pa.MaBenhNhan == ph.MaBenhNhan && ph.IDCoSo.Contains(MaDonVi) && ph.NgayCoKQ.Value.Date <= NgayKT.Date && ph.NgayCoKQ.Value.Date >= NgayBD.Date)
                               select pa).ToList();
                    traKQ = (from pa in db.PSXN_TraKetQuas
                             from ph in db.PSPhieuSangLocs
                             where (ph.isXoa != true && pa.isXoa != true && pa.isTraKQ == true && ph.TrangThaiMau > 3 && ph.TrangThaiMau != 5 && pa.isDaDuyetKQ == true && pa.MaPhieu == ph.IDPhieu && ph.IDCoSo.Contains(MaDonVi) && ph.NgayCoKQ.Value.Date <= NgayKT.Date && ph.NgayCoKQ.Value.Date >= NgayBD.Date)
                             select pa).ToList();
                    traKQCT = (from pa in db.PSXN_TraKetQuas
                               from ph in db.PSPhieuSangLocs
                               from ct in db.PSXN_TraKQ_ChiTiets
                               where (ph.isXoa != true && pa.isXoa != true && pa.isTraKQ == true && ph.TrangThaiMau > 3 && ph.TrangThaiMau != 5 && pa.isDaDuyetKQ == true && pa.MaPhieu == ph.IDPhieu && ph.IDCoSo.Contains(MaDonVi) && ph.NgayCoKQ.Value.Date <= NgayKT.Date && ph.NgayCoKQ.Value.Date >= NgayBD.Date && pa.MaPhieu == ct.MaPhieu)
                               select ct).ToList();
                    patient = patient.GroupBy(x => x.MaBenhNhan).Select(g => g.First()).ToList();

                }

                pcb = GetBaoCaoThongTinCoBanPhieu(phieusangloc, patient, traKQ);
                pcbct = GetBaoCaoCoBanCT(phieusangloc, traKQCT, traKQ);
                if (TkSlPhieu != null)
                {
                    foreach (var tkslphieu in TkSlPhieu)
                    {
                        rptBaoCaoSLPhieu tkphieu = new rptBaoCaoSLPhieu();
                        tkphieu.Thang = tkslphieu.thang ?? 0;
                        tkphieu.SLphieu = tkslphieu.slphieu ?? 0;
                        rpt.Add(tkphieu);
                    }
                    pcb.slphieu = rpt;
                    pcb.baocaocobanct = pcbct;
                }
            }
            catch (Exception ex)
            {

            }
            return pcb;
        }
        public TTPhieuCB GetBaoCaoThongTinCoBanPhieu(List<PSPhieuSangLoc> phieusangloc, List<PSPatient> patient, List<PSXN_TraKetQua> traKQ)
        {
            TTPhieuCB pcb = new TTPhieuCB();
            List<PsThongKe> tkbenh = new List<PsThongKe>();
            List<PsThongKe> tkClMau = new List<PsThongKe>();
            List<PsThongKe> tkChuongTrinh = new List<PsThongKe>();
            try
            {
                pcb.TongSoPhieu = phieusangloc.Count();
                pcb.PhieuThuLai = phieusangloc.Where(x => x.isLayMauLan2 == true).Count();
                pcb.PhieuThuMoi = phieusangloc.Where(x => x.isLayMauLan2 == false).Count();
                pcb.PPSinhMo = patient.Where(x => x.PhuongPhapSinh == 0).Count();
                pcb.PPSinhThuong = patient.Where(x => x.PhuongPhapSinh == 1).Count();
                pcb.PPSinhKhac = patient.Where(x => x.PhuongPhapSinh == 2).Count();
                pcb.Nam = patient.Where(x => x.GioiTinh == 0).Count();
                pcb.Nu = patient.Where(x => x.GioiTinh == 1).Count();
                pcb.GTKhac = patient.Where(x => x.GioiTinh == 2).Count();
                pcb.MauDat = phieusangloc.Where(x => x.isKhongDat != true).Count();
                pcb.MauKoDat = phieusangloc.Where(x => x.isKhongDat == true).Count();
                pcb.TyleSinhConThu3 = 0;
                pcb.TuoiMeDuoi18 = 0;
                pcb.TuoiMeTu18den35 = 0;
                pcb.TuoiMeTren35 = 0;
                foreach (var pat in patient)
                {
                    try
                    {
                        string para = pat.Para.ToString().Substring(3);
                        if (Int32.Parse(para) > 2)
                        {
                            pcb.TyleSinhConThu3 = pcb.TyleSinhConThu3 + 1;
                        }
                        int tuoi = Int32.Parse((pat.NgayGioSinh.Value.Year - pat.MotherBirthday.Value.Year).ToString());
                        if (tuoi < 18)
                        {
                            pcb.TuoiMeDuoi18 = pcb.TuoiMeDuoi18 + 1;
                        }
                        else if (tuoi <= 35 && tuoi > 18)
                        {
                            pcb.TuoiMeTu18den35 = pcb.TuoiMeTu18den35 + 1;
                        }
                        else if (tuoi > 35)
                        {
                            pcb.TuoiMeTren35 = pcb.TuoiMeTren35 + 1;
                        }
                    }
                    catch
                    {

                    }

                }


                List<PSPhieuSangLoc> PhieuCanThuLai = (from ph in phieusangloc
                                                       where (ph.isNguyCoCao == true && ph.isLayMauLan2 != true && ph.TrangThaiMau > 3)
                                                       select ph).ToList();
                pcb.SoMauCanThuLai = PhieuCanThuLai.Count();
                pcb.SoMauDaThuLai = (from ph in phieusangloc
                                     from ph1 in PhieuCanThuLai
                                     where (ph.isLayMauLan2 == true && ph.IDPhieuLan1 == ph1.IDPhieu)
                                     select ph).Count();
                pcb.SoMauChuaThuLai = pcb.SoMauCanThuLai - pcb.SoMauDaThuLai;
                var dsbenh = db.PSDanhMucGoiDichVuChungs.ToList();
                foreach (var b in dsbenh)
                {
                    PsThongKe tkctbenh = new PsThongKe();
                    var dsphieu = phieusangloc.Where(x => x.MaGoiXN == b.IDGoiDichVuChung && x.isXoa != true && x.TrangThaiMau > 1).Count();
                    tkctbenh.TenThongKe = b.TenGoiDichVuChung;
                    tkctbenh.SoLuong = dsphieu;
                    tkbenh.Add(tkctbenh);
                }
                pcb.thongkebenh = tkbenh;

                var dskdat = db.PSDanhMucDanhGiaChatLuongMaus.ToList();
                foreach (var TenDG in dskdat)
                {
                    PsThongKe tkdanhgia = new PsThongKe();
                    var dsdanhgia = db.PSChiTietDanhGiaChatLuongs.Where(x => x.isXoa != true).ToList();
                    var dsphieu = ((from c in dsdanhgia join p in phieusangloc on c.IDPhieu equals p.IDPhieu where c.IDDanhGiaChatLuongMau == TenDG.IDDanhGiaChatLuongMau select c).ToList()).Count();
                    tkdanhgia.TenThongKe = TenDG.ChatLuongMau;
                    tkdanhgia.SoLuong = dsphieu;
                    tkClMau.Add(tkdanhgia);
                }
                pcb.thongkeDGMau = tkClMau;

                var dsChuongTrinh = db.PSDanhMucChuongTrinhs.ToList();
                foreach (var ct in dsChuongTrinh)
                {
                    PsThongKe tkCT = new PsThongKe();
                    var dsphieu = phieusangloc.Where(x => x.IDChuongTrinh == ct.IDChuongTrinh && x.isXoa != true).Count();
                    tkCT.TenThongKe = ct.TenChuongTrinh;
                    tkCT.SoLuong = dsphieu;
                    tkChuongTrinh.Add(tkCT);
                }
                pcb.thongkeCTrinh = tkChuongTrinh;
            }
            catch (Exception ex)
            {

            }
            return pcb;
        }
        public PsBaoCaoCoBanCT GetBaoCaoCoBanCT(List<PSPhieuSangLoc> phieusangloc, List<PSXN_TraKQ_ChiTiet> traKQCT, List<PSXN_TraKetQua> traKQ)
        {
            PsBaoCaoCoBanCT bc = new PsBaoCaoCoBanCT();

            List<PSPhieuSangLoc> PhieuCanThuLai = (from ph in phieusangloc
                                                   where (ph.isNguyCoCao == true && ph.isLayMauLan2 != true && ph.TrangThaiMau > 3)
                                                   select ph).ToList();
            List<PSPhieuSangLoc> phNguyCoCao1 = (from ph in phieusangloc
                                                 where (ph.isNguyCoCao == true && ph.isLayMauLan2 != true && ph.TrangThaiMau > 3)
                                                 select ph).ToList();
            List<PSPhieuSangLoc> phNguyCoCao2 = (from ph in phieusangloc
                                                 where (ph.isLayMauLan2 == true)
                                                 select ph).ToList();
            List<PSPhieuSangLoc> phNguyCoCao1ChuaThuMau = (from ph in PhieuCanThuLai
                                                           where !(from nc in phNguyCoCao2
                                                                   select nc.IDPhieuLan1)
                                                                  .Contains(ph.IDPhieu)
                                                           select ph).ToList();
            //List<PSPhieuSangLoc> phNguyCoCao2 = (from ph in phieusangloc
            //                                     from ph1 in PhieuCanThuLai
            //                                     where ( ph.isLayMauLan2 == true && ph.IDPhieuLan1==ph1.IDPhieu )
            //                                     select ph).ToList();
            // List<PSPhieuSangLoc> phNguyCoCao1 = PhieuCanThuLai.Where(p => !phNguyCoCao2.Any(p2 => p2.IDPhieuLan1 == p.IDPhieu)).ToList();

            List<PSXN_TraKQ_ChiTiet> TraKQCao = (from ct in traKQCT
                                                 where ct.isNguyCo == true
                                                 select ct).ToList();


            //Đếm NCC Lần 1
            bc.NCC1G6PD = (from ph in phNguyCoCao1ChuaThuMau
                           from ct in traKQCT
                           where ct.TenKyThuat.Equals("G6PD") && ct.isNguyCo == true && ph.isLayMauLan2 != true && ph.IDPhieu == ct.MaPhieu
                           select ph).Count();
            bc.NCC1CH = (from ph in phNguyCoCao1ChuaThuMau
                         from ct in traKQCT
                         where ct.TenKyThuat.Equals("CH") && ct.isNguyCo == true && ph.IDPhieu == ct.MaPhieu && ph.isLayMauLan2 != true
                         select ph).Count();
            bc.NCC1CAH = (from ph in phNguyCoCao1ChuaThuMau
                          from ct in traKQCT
                          where ct.TenKyThuat.Equals("CAH") && ct.isNguyCo == true && ph.IDPhieu == ct.MaPhieu && ph.isLayMauLan2 != true
                          select ph).Count();
            bc.NCC1GAL = (from ph in phNguyCoCao1ChuaThuMau
                          from ct in traKQCT
                          where ct.TenKyThuat.Equals("GAL") && ct.isNguyCo == true && ph.IDPhieu == ct.MaPhieu && ph.isLayMauLan2 != true
                          select ph).Count();
            bc.NCC1PKU = (from ph in phNguyCoCao1ChuaThuMau
                          from ct in traKQCT
                          where ct.TenKyThuat.Equals("PKU") && ct.isNguyCo == true && ph.IDPhieu == ct.MaPhieu && ph.isLayMauLan2 != true
                          select ph).Count();
            //Đếm NCC lần 2
            bc.NCC2G6PD = (from ph in phNguyCoCao2
                           from ct in traKQCT
                           where ct.TenKyThuat.Equals("G6PD") && ct.isNguyCo == true && ph.IDPhieu == ct.MaPhieu && ph.isLayMauLan2 == true
                           select ph).Count();
            bc.NCC2CH = (from ph in phNguyCoCao2
                         from ct in traKQCT
                         where ct.TenKyThuat.Equals("CH") && ct.isNguyCo == true && ph.IDPhieu == ct.MaPhieu && ph.isLayMauLan2 == true
                         select ph).Count();
            bc.NCC2CAH = (from ph in phNguyCoCao2
                          from ct in traKQCT
                          where ct.TenKyThuat.Equals("CAH") && ct.isNguyCo == true && ph.IDPhieu == ct.MaPhieu && ph.isLayMauLan2 == true
                          select ph).Count();
            bc.NCC2GAL = (from ph in phNguyCoCao2
                          from ct in traKQCT
                          where ct.TenKyThuat.Equals("GAL") && ct.isNguyCo == true && ph.IDPhieu == ct.MaPhieu && ph.isLayMauLan2 == true
                          select ph).Count();
            bc.NCC2PKU = (from ph in phNguyCoCao2
                          from ct in traKQCT
                          where ct.TenKyThuat.Equals("PKU") && ct.isNguyCo == true && ph.IDPhieu == ct.MaPhieu && ph.isLayMauLan2 == true
                          select ph).Count();
            //Đếm NCT lần 2
            bc.NCT2G6PD = (from ph in phNguyCoCao2
                           from ct in traKQCT
                           where ct.TenKyThuat.Equals("G6PD") && ct.isNguyCo != true && ph.IDPhieu == ct.MaPhieu && ph.isLayMauLan2 == true
                           select ph).Count();
            bc.NCT2CH = (from ph in phNguyCoCao2
                         from ct in traKQCT
                         where ct.TenKyThuat.Equals("CH") && ct.isNguyCo != true && ph.IDPhieu == ct.MaPhieu && ph.isLayMauLan2 == true
                         select ph).Count();
            bc.NCT2CAH = (from ph in phNguyCoCao2
                          from ct in traKQCT
                          where ct.TenKyThuat.Equals("CAH") && ct.isNguyCo != true && ph.IDPhieu == ct.MaPhieu && ph.isLayMauLan2 == true
                          select ph).Count();
            bc.NCT2GAL = (from ph in phNguyCoCao2
                          from ct in traKQCT
                          where ct.TenKyThuat.Equals("GAL") && ct.isNguyCo != true && ph.IDPhieu == ct.MaPhieu && ph.isLayMauLan2 == true
                          select ph).Count();
            bc.NCT2PKU = (from ph in phNguyCoCao2
                          from ct in traKQCT
                          where ct.TenKyThuat.Equals("PKU") && ct.isNguyCo != true && ph.IDPhieu == ct.MaPhieu && ph.isLayMauLan2 == true
                          select ph).Count();
            return bc;
        }

        public List<PsBaoCaoTaiChinh> GetBaoCaoTaiChinh(string MaDonVi, DateTime NgayBD, DateTime NgayKT)
        {
            List<PsBaoCaoTaiChinh> lst = new List<PsBaoCaoTaiChinh>();
            try
            {
                var data = db.pro_ReportBaoCaoTaiChinh(MaDonVi, NgayBD, NgayKT).ToList();

                foreach (var da in data)
                {

                    PsBaoCaoTaiChinh tc = new PsBaoCaoTaiChinh();
                    ConvertObjectToObject(da, tc);
                    lst.Add(tc);
                }

            }
            catch
            {

            }
            return lst;
        }
        public rptBaoCaoTongHop GetBaoCaoTongHopTrungTam(DateTime tuNgay, DateTime denNgay)
        {
            rptBaoCaoTongHop rpt = new rptBaoCaoTongHop();
            try
            {
                PsThongTinTrungTam TrungTam = new PsThongTinTrungTam();
                PsThongTinDonVi donvi = new PsThongTinDonVi();
                var ttam = db.PSThongTinTrungTams.FirstOrDefault();
                try
                {
                    TrungTam.DiaChi = ttam.Diachi;
                    TrungTam.DienThoai = ttam.DienThoai;
                    TrungTam.MaTrungTam = ttam.MaTrungTam;
                    TrungTam.MaVietTat = ttam.MaVietTat;
                    TrungTam.TenTrungTam = ttam.TenTrungTam;
                    if (ttam.ChuKiTT.Length > 0)
                    {
                        try
                        {
                            byte[] b = ttam.ChuKiTT.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.ChuKiTT = img;
                        }
                        catch { }
                    }
                    if (ttam.ChuKiXN.Length > 0)
                    {
                        try
                        {
                            byte[] b = ttam.ChuKiXN.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.ChuKiXN = img;
                        }
                        catch { }
                    }
                    if (ttam.Header.Length > 0)
                    {
                        try
                        {
                            byte[] b = ttam.Header.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.Hearder = img;
                        }
                        catch { }
                    }
                    if (ttam.Logo.Length > 0)
                    {

                        try
                        {
                            byte[] b = ttam.Logo.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.Logo = img;
                        }
                        catch { }
                    }
                    donvi.TenDonVi = ttam.TenTrungTam;
                    donvi.DiaChi = ttam.Diachi;
                }
                catch { }

                var datatonghop = db.pro_Report_TrungTamCoBan(tuNgay, denNgay).FirstOrDefault();
                if (datatonghop != null)
                {

                    rptBaoCaoTongHop.CAH cah = new rptBaoCaoTongHop.CAH();
                    rptBaoCaoTongHop.CH ch = new rptBaoCaoTongHop.CH();
                    rptBaoCaoTongHop.ChatLuongMau chatluong = new rptBaoCaoTongHop.ChatLuongMau();
                    rptBaoCaoTongHop.ChuongTrinh ctr = new rptBaoCaoTongHop.ChuongTrinh();
                    rptBaoCaoTongHop.G6PD g6pd = new rptBaoCaoTongHop.G6PD();
                    rptBaoCaoTongHop.GAL gal = new rptBaoCaoTongHop.GAL();
                    rptBaoCaoTongHop.GioiTinh gt = new rptBaoCaoTongHop.GioiTinh();
                    rptBaoCaoTongHop.GoiBenh gb = new rptBaoCaoTongHop.GoiBenh();
                    rptBaoCaoTongHop.PhuongPhapSinh pps = new rptBaoCaoTongHop.PhuongPhapSinh();
                    rptBaoCaoTongHop.PKU pku = new rptBaoCaoTongHop.PKU();
                    cah.CAHBinhThuong = datatonghop.CAHBinhThuong ?? 0;
                    cah.CAHBinhThuong_Tong = datatonghop.CAHBinhThuong_Tong;
                    cah.CAHNguyCo = datatonghop.CAHNguyCo ?? 0;
                    cah.CAHNguyCo_Tong = datatonghop.CAHNguyCo_Tong;
                    cah.CAHTong = datatonghop.CAHTong ?? 0;
                    ch.CHBinhThuong = datatonghop.CHBinhThuong ?? 0;
                    ch.CHBinhThuong_Tong = datatonghop.CHBinhThuong_Tong;
                    ch.CHNguyCo = datatonghop.CHNguyCo ?? 0;
                    ch.CHNguyCo_Tong = datatonghop.CHNguyCo_Tong;
                    ch.CHTong = datatonghop.CHTong ?? 0;
                    chatluong.Dat = datatonghop.Dat ?? 0;
                    chatluong.KhongDat = datatonghop.KhongDat ?? 0;
                    ctr.QuocGia = datatonghop.QuocGia ?? 0;
                    ctr.XaHoiHoa = datatonghop.XaHoi ?? 0;
                    g6pd.G6PDBinhThuong = datatonghop.G6PDBinhThuong ?? 0;
                    g6pd.G6PDBinhThuong_Tong = datatonghop.G6PDBinhThuong_Tong;
                    g6pd.G6PDNguyCo = datatonghop.G6PDNguyCo ?? 0;
                    g6pd.G6PDNguyCo_Tong = datatonghop.G6PDNguyCo_Tong;
                    g6pd.G6PDTong = datatonghop.G6PDTong ?? 0;
                    gal.GALBinhThuong = datatonghop.GALBinhThuong ?? 0;
                    gal.GALBinhThuong_Tong = datatonghop.GALBinhThuong_Tong;
                    gal.GALNguyCo = datatonghop.GALNguyCo ?? 0;
                    gal.GALNguyCo_Tong = datatonghop.GALNguyCo_Tong;
                    gal.GALTong = datatonghop.GALTong ?? 0;
                    gt.GTNa = datatonghop.KhongXacDinh ?? 0;
                    gt.GTNam = datatonghop.Nam ?? 0;
                    gt.GTNu = datatonghop.Nu ?? 0;
                    gb.sl2Benh = datatonghop.SL2Benh ?? 0;
                    gb.sl3Benh = datatonghop.SL3Benh ?? 0;
                    gb.sl5Benh = datatonghop.SL5Benh ?? 0;
                    gb.slThuLai = datatonghop.SLThuLai ?? 0;
                    pps.SinhMo = datatonghop.SinhMo ?? 0;
                    pps.SinhNa = datatonghop.KhongXacDinhSinh ?? 0;
                    pps.SinhThuong = datatonghop.SinhThuong ?? 0;
                    pku.PKUBinhThuong = datatonghop.PKUBinhThuong ?? 0;
                    pku.PKUNguyCo = datatonghop.PKUNguyCo ?? 0;
                    pku.PKUTong = datatonghop.PKUTong ?? 0;
                    pku.PUKBinhThuong_Tong = datatonghop.PKUBinhThuong_Tong;
                    pku.PUKNguyCo_Tong = datatonghop.PKUNguyCo_Tong;
                    rpt.cAH = cah;
                    rpt.cH = ch;
                    rpt.chatLuongMau = chatluong;
                    rpt.chuongTrinh = ctr;
                    rpt.g6PD = g6pd;
                    rpt.gAL = gal;
                    rpt.gioiTinh = gt;
                    rpt.goiBenh = gb;
                    rpt.phuongPhapSinh = pps;
                    rpt.pKU = pku;
                    rpt.SoLuongMau = datatonghop.SoPhieu ?? 0;
                    rpt.TrungTam = TrungTam;
                    rpt.Donvi = donvi;
                    rpt.tuNgay = tuNgay;
                    rpt.denNgay = denNgay;
                }
            }
            catch { }
            return rpt;
        }
        public rptBaoCaoTongHop GetBaoCaoTongHopChiCuc(DateTime tuNgay, DateTime denNgay, string maChiCuc)
        {
            rptBaoCaoTongHop rpt = new rptBaoCaoTongHop();
            try
            {
                PsThongTinTrungTam TrungTam = new PsThongTinTrungTam();
                PsThongTinDonVi DonVi = new PsThongTinDonVi();
                var ttam = db.PSThongTinTrungTams.FirstOrDefault();
                var chicuc = db.PSDanhMucChiCucs.FirstOrDefault(p => p.MaChiCuc == maChiCuc);
                try
                {

                    TrungTam.DiaChi = ttam.Diachi;
                    TrungTam.DienThoai = ttam.DienThoai;
                    TrungTam.MaTrungTam = ttam.MaTrungTam;
                    TrungTam.TenTrungTam = ttam.TenTrungTam;
                    TrungTam.MaVietTat = ttam.MaVietTat;
                    if (ttam.ChuKiTT.Length > 0)
                    {
                        try
                        {
                            byte[] b = ttam.ChuKiTT.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.ChuKiTT = img;
                        }
                        catch { }
                    }
                    if (ttam.ChuKiXN.Length > 0)
                    {
                        try
                        {
                            byte[] b = ttam.ChuKiXN.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.ChuKiXN = img;
                        }
                        catch { }
                    }
                    if (ttam.Header.Length > 0)
                    {
                        try
                        {
                            byte[] b = ttam.Header.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.Hearder = img;
                        }
                        catch { }
                    }
                    if (ttam.Logo.Length > 0)
                    {

                        try
                        {
                            byte[] b = ttam.Logo.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.Logo = img;
                        }
                        catch { }
                    }
                    DonVi.TenDonVi = chicuc.TenChiCuc;
                    DonVi.DiaChi = chicuc.DiaChiChiCuc;
                }
                catch { }

                var datatonghop = db.pro_Report_ChiCucCoBan(tuNgay, denNgay, maChiCuc).FirstOrDefault();
                if (datatonghop != null)
                {

                    rptBaoCaoTongHop.CAH cah = new rptBaoCaoTongHop.CAH();
                    rptBaoCaoTongHop.CH ch = new rptBaoCaoTongHop.CH();
                    rptBaoCaoTongHop.ChatLuongMau chatluong = new rptBaoCaoTongHop.ChatLuongMau();
                    rptBaoCaoTongHop.ChuongTrinh ctr = new rptBaoCaoTongHop.ChuongTrinh();
                    rptBaoCaoTongHop.G6PD g6pd = new rptBaoCaoTongHop.G6PD();
                    rptBaoCaoTongHop.GAL gal = new rptBaoCaoTongHop.GAL();
                    rptBaoCaoTongHop.GioiTinh gt = new rptBaoCaoTongHop.GioiTinh();
                    rptBaoCaoTongHop.GoiBenh gb = new rptBaoCaoTongHop.GoiBenh();
                    rptBaoCaoTongHop.PhuongPhapSinh pps = new rptBaoCaoTongHop.PhuongPhapSinh();
                    rptBaoCaoTongHop.PKU pku = new rptBaoCaoTongHop.PKU();
                    cah.CAHBinhThuong = datatonghop.CAHBinhThuong ?? 0;
                    cah.CAHBinhThuong_Tong = datatonghop.CAHBinhThuong_Tong;
                    cah.CAHNguyCo = datatonghop.CAHNguyCo ?? 0;
                    cah.CAHNguyCo_Tong = datatonghop.CAHNguyCo_Tong;
                    cah.CAHTong = datatonghop.CAHTong ?? 0;
                    ch.CHBinhThuong = datatonghop.CHBinhThuong ?? 0;
                    ch.CHBinhThuong_Tong = datatonghop.CHBinhThuong_Tong;
                    ch.CHNguyCo = datatonghop.CHNguyCo ?? 0;
                    ch.CHNguyCo_Tong = datatonghop.CHNguyCo_Tong;
                    ch.CHTong = datatonghop.CHTong ?? 0;
                    chatluong.Dat = datatonghop.Dat ?? 0;
                    chatluong.KhongDat = datatonghop.KhongDat ?? 0;
                    ctr.QuocGia = datatonghop.QuocGia ?? 0;
                    ctr.XaHoiHoa = datatonghop.XaHoi ?? 0;
                    g6pd.G6PDBinhThuong = datatonghop.G6PDBinhThuong ?? 0;
                    g6pd.G6PDBinhThuong_Tong = datatonghop.G6PDBinhThuong_Tong;
                    g6pd.G6PDNguyCo = datatonghop.G6PDNguyCo ?? 0;
                    g6pd.G6PDNguyCo_Tong = datatonghop.G6PDNguyCo_Tong;
                    g6pd.G6PDTong = datatonghop.G6PDTong ?? 0;
                    gal.GALBinhThuong = datatonghop.GALBinhThuong ?? 0;
                    gal.GALBinhThuong_Tong = datatonghop.GALBinhThuong_Tong;
                    gal.GALNguyCo = datatonghop.GALNguyCo ?? 0;
                    gal.GALNguyCo_Tong = datatonghop.GALNguyCo_Tong;
                    gal.GALTong = datatonghop.GALTong ?? 0;
                    gt.GTNa = datatonghop.KhongXacDinh ?? 0;
                    gt.GTNam = datatonghop.Nam ?? 0;
                    gt.GTNu = datatonghop.Nu ?? 0;
                    gb.sl2Benh = datatonghop.SL2Benh ?? 0;
                    gb.sl3Benh = datatonghop.SL3Benh ?? 0;
                    gb.sl5Benh = datatonghop.SL5Benh ?? 0;
                    gb.slThuLai = datatonghop.SLThuLai ?? 0;
                    pps.SinhMo = datatonghop.SinhMo ?? 0;
                    pps.SinhNa = datatonghop.KhongXacDinhSinh ?? 0;
                    pps.SinhThuong = datatonghop.SinhThuong ?? 0;
                    pku.PKUBinhThuong = datatonghop.PKUBinhThuong ?? 0;
                    pku.PKUNguyCo = datatonghop.PKUNguyCo ?? 0;
                    pku.PKUTong = datatonghop.PKUTong ?? 0;
                    pku.PUKBinhThuong_Tong = datatonghop.PKUBinhThuong_Tong;
                    pku.PUKNguyCo_Tong = datatonghop.PKUNguyCo_Tong;
                    rpt.cAH = cah;
                    rpt.cH = ch;
                    rpt.chatLuongMau = chatluong;
                    rpt.chuongTrinh = ctr;
                    rpt.g6PD = g6pd;
                    rpt.gAL = gal;
                    rpt.gioiTinh = gt;
                    rpt.goiBenh = gb;
                    rpt.phuongPhapSinh = pps;
                    rpt.pKU = pku;
                    rpt.SoLuongMau = datatonghop.SoPhieu ?? 0;
                    rpt.TrungTam = TrungTam;
                    rpt.Donvi = DonVi;
                    rpt.tuNgay = tuNgay;
                    rpt.denNgay = denNgay;
                }
            }
            catch (Exception ex) { }
            return rpt;
        }

        public PsReponse CapNhatLisence(string key)
        {
            PsReponse res = new PsReponse();

            try
            {
                var tt = db.PSThongTinTrungTams.FirstOrDefault();
                if (tt != null)
                {
                    tt.LicenseKey = key;
                    tt.isDongBo = false;
                    db.SubmitChanges();
                    res.Result = true;
                }
                else
                {
                    res.Result = false;
                    res.StringError = "Chưa có thông tin trung tâm, vui lòng đăng nhập lại!";
                }
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.StringError = ex.ToString();
            }
            return res;
        }


        public List<PSDanhMucThongSoXN> GetDanhMucThongSoXN()
        {
            List<PSDanhMucThongSoXN> lst = new List<PSDanhMucThongSoXN>();
            try
            {
                var nccs = db.PSDanhMucThongSoXNs.ToList();
                if (nccs.Count > 0)
                    return nccs;
            }
            catch { }
            return lst;
        }
        public rptChiTietTrungTam GetBaoCaoChiCucTongHopChiTietTheoDonVi(DateTime tuNgay, DateTime denNgay, string maChiCuc)
        {
            rptChiTietTrungTam rpt = new rptChiTietTrungTam();
            try
            {
                PsThongTinTrungTam TrungTam = new PsThongTinTrungTam();
                List<rptChiTietTrungTam_ChiTiet> lstdata = new List<rptChiTietTrungTam_ChiTiet>();
                var data = db.pro_Report_ChiCucChiTietTheoTungDonVi(tuNgay, denNgay, maChiCuc).ToList();
                if (data != null)
                {
                    int stt = 0;
                    foreach (var rd in data)
                    {
                        rptChiTietTrungTam_ChiTiet dt = new rptChiTietTrungTam_ChiTiet();
                        dt.CAH = rd.CAHNguyCo ?? 0 + rd.CAHBinhThuong ?? 0;
                        dt.CAHBinhThuong = rd.CAHBinhThuong ?? 0;
                        dt.CAHBinhThuong_Tong = rd.CAHBinhThuong_Tong;
                        dt.CAHNguyCo = rd.CAHNguyCo ?? 0;
                        dt.CAHNguyCo_Tong = rd.CAHNguyCo_Tong;
                        dt.CH = rd.CHBinhThuong ?? 0 + rd.CHNguyCo ?? 0;
                        dt.CHBinhThuong = rd.CHBinhThuong ?? 0;
                        dt.CHBinhThuong_Tong = rd.CHBinhThuong_Tong;
                        dt.CHNguyCo = rd.CHNguyCo ?? 0;
                        dt.CHNguyCo_Tong = rd.CHNguyCo_Tong;
                        dt.Dat = rd.Dat ?? 0;
                        dt.G6PD = rd.G6PDBinhThuong ?? 0 + rd.G6PDNguyCo ?? 0;
                        dt.G6PDBinhThuong = rd.G6PDBinhThuong ?? 0;
                        dt.G6PDBinhThuong_Tong = rd.G6PDBinhThuong_Tong;
                        dt.G6PDNguyCo = rd.G6PDNguyCo ?? 0;
                        dt.G6PDNguyCo_Tong = rd.G6PDNguyCo_Tong;
                        dt.GAL = rd.GALBinhThuong ?? 0 + rd.GALNguyCo ?? 0;
                        dt.GALBinhThuong = rd.GALBinhThuong ?? 0;
                        dt.GALBinhThuong_Tong = rd.GALBinhThuong_Tong;
                        dt.GALNguyCo = rd.GALNguyCo ?? 0;
                        dt.GALNguyCo_Tong = rd.GALNguyCo_Tong;
                        dt.GTNa = rd.KhongXacDinh ?? 0;
                        dt.GTNam = rd.Nam ?? 0;
                        dt.GTNu = rd.Nu ?? 0;
                        dt.KhongDat = rd.KhongDat ?? 0;
                        dt.MaChiCuc = rd.MaChiCuc;
                        dt.TenChiCuc = rd.ChiCuc;
                        dt.MaDonVi = rd.MaDonVi;
                        dt.TenDonVi = rd.TenDonVi;
                        dt.XaHoiHoa = rd.XaHoi ?? 0;
                        dt.PKU = rd.PKUBinhThuong ?? 0 + rd.PKUNguyCo ?? 0;
                        dt.PKUNguyCo = rd.PKUNguyCo ?? 0;
                        dt.PUKBinhThuong_Tong = rd.PKUBinhThuong_Tong;
                        dt.PKUBinhThuong = rd.PKUBinhThuong ?? 0;
                        dt.PUKNguyCo_Tong = rd.PKUNguyCo_Tong;
                        dt.QuocGia = rd.QuocGia ?? 0;
                        dt.SinhMo = rd.SinhMo ?? 0;
                        dt.SinhNa = rd.KhongXacDinhSinh ?? 0;
                        dt.SinhThuong = rd.SinhThuong ?? 0;
                        dt.sl2Benh = rd.SL2Benh ?? 0;
                        dt.sl3Benh = rd.SL3Benh ?? 0;
                        dt.sl5Benh = rd.SL5Benh ?? 0;
                        dt.slMauThuLai = rd.SLThuLai ?? 0;
                        dt.SoLuongMau = rd.SoPhieu ?? 0;
                        dt.Stt = (stt + 1).ToString();
                        lstdata.Add(dt);
                    }
                }
                var ttam = db.PSThongTinTrungTams.FirstOrDefault();
                try
                {
                    TrungTam.DiaChi = ttam.Diachi;
                    TrungTam.DienThoai = ttam.DienThoai;
                    TrungTam.MaTrungTam = ttam.MaTrungTam;
                    TrungTam.MaVietTat = ttam.MaVietTat;
                    TrungTam.TenTrungTam = ttam.TenTrungTam;
                    if (ttam.ChuKiTT.Length > 0)
                    {
                        try
                        {
                            byte[] b = ttam.ChuKiTT.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.ChuKiTT = img;
                        }
                        catch { }
                    }
                    if (ttam.ChuKiXN.Length > 0)
                    {
                        try
                        {
                            byte[] b = ttam.ChuKiXN.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.ChuKiXN = img;
                        }
                        catch { }
                    }
                    if (ttam.Header.Length > 0)
                    {
                        try
                        {
                            byte[] b = ttam.Header.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.Hearder = img;
                        }
                        catch { }
                    }
                    if (ttam.Logo.Length > 0)
                    {

                        try
                        {
                            byte[] b = ttam.Logo.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.Logo = img;
                        }
                        catch { }
                    }
                }
                catch { }
                rpt.TuNgay = tuNgay;
                rpt.DenNgay = denNgay;
                rpt.ThongTinTrungTam = TrungTam;
                rpt.ChiTietCacChiCuc = lstdata;
                return rpt;
            }
            catch
            {
                return rpt;
            }
        }
        public rptChiTietTrungTam GetBaoCaoTrungTamTongHopChiTietTheoDonVi(DateTime tuNgay, DateTime denNgay)
        {
            rptChiTietTrungTam rpt = new rptChiTietTrungTam();
            try
            {
                PsThongTinTrungTam TrungTam = new PsThongTinTrungTam();
                List<rptChiTietTrungTam_ChiTiet> lstdata = new List<rptChiTietTrungTam_ChiTiet>();
                var data = db.pro_Report_TrungTamChiTietTheoTungDonVi(tuNgay, denNgay).ToList();
                if (data != null)
                {
                    int stt = 0;
                    foreach (var rd in data)
                    {
                        rptChiTietTrungTam_ChiTiet dt = new rptChiTietTrungTam_ChiTiet();
                        dt.CAH = rd.CAHNguyCo ?? 0 + rd.CAHBinhThuong ?? 0;
                        dt.CAHBinhThuong = rd.CAHBinhThuong ?? 0;
                        dt.CAHBinhThuong_Tong = rd.CAHBinhThuong_Tong;
                        dt.CAHNguyCo = rd.CAHNguyCo ?? 0;
                        dt.CAHNguyCo_Tong = rd.CAHNguyCo_Tong;
                        dt.CH = rd.CHBinhThuong ?? 0 + rd.CHNguyCo ?? 0;
                        dt.CHBinhThuong = rd.CHBinhThuong ?? 0;
                        dt.CHBinhThuong_Tong = rd.CHBinhThuong_Tong;
                        dt.CHNguyCo = rd.CHNguyCo ?? 0;
                        dt.CHNguyCo_Tong = rd.CHNguyCo_Tong;
                        dt.Dat = rd.Dat ?? 0;
                        dt.G6PD = rd.G6PDBinhThuong ?? 0 + rd.G6PDNguyCo ?? 0;
                        dt.G6PDBinhThuong = rd.G6PDBinhThuong ?? 0;
                        dt.G6PDBinhThuong_Tong = rd.G6PDBinhThuong_Tong;
                        dt.G6PDNguyCo = rd.G6PDNguyCo ?? 0;
                        dt.G6PDNguyCo_Tong = rd.G6PDNguyCo_Tong;
                        dt.GAL = rd.GALBinhThuong ?? 0 + rd.GALNguyCo ?? 0;
                        dt.GALBinhThuong = rd.GALBinhThuong ?? 0;
                        dt.GALBinhThuong_Tong = rd.GALBinhThuong_Tong;
                        dt.GALNguyCo = rd.GALNguyCo ?? 0;
                        dt.GALNguyCo_Tong = rd.GALNguyCo_Tong;
                        dt.GTNa = rd.KhongXacDinh ?? 0;
                        dt.GTNam = rd.Nam ?? 0;
                        dt.GTNu = rd.Nu ?? 0;
                        dt.KhongDat = rd.KhongDat ?? 0;
                        dt.MaChiCuc = rd.MaChiCuc;
                        dt.TenChiCuc = rd.ChiCuc;
                        dt.MaDonVi = rd.MaDonVi;
                        dt.TenDonVi = rd.TenDonVi;
                        dt.XaHoiHoa = rd.XaHoi ?? 0;
                        dt.PKU = rd.PKUBinhThuong ?? 0 + rd.PKUNguyCo ?? 0;
                        dt.PKUNguyCo = rd.PKUNguyCo ?? 0;
                        dt.PUKBinhThuong_Tong = rd.PKUBinhThuong_Tong;
                        dt.PKUBinhThuong = rd.PKUBinhThuong ?? 0;
                        dt.PUKNguyCo_Tong = rd.PKUNguyCo_Tong;
                        dt.QuocGia = rd.QuocGia ?? 0;
                        dt.SinhMo = rd.SinhMo ?? 0;
                        dt.SinhNa = rd.KhongXacDinhSinh ?? 0;
                        dt.SinhThuong = rd.SinhThuong ?? 0;
                        dt.sl2Benh = rd.SL2Benh ?? 0;
                        dt.sl3Benh = rd.SL3Benh ?? 0;
                        dt.sl5Benh = rd.SL5Benh ?? 0;
                        dt.slMauThuLai = rd.SLThuLai ?? 0;
                        dt.SoLuongMau = rd.SoPhieu ?? 0;
                        dt.Stt = (stt + 1).ToString();
                        lstdata.Add(dt);
                    }
                }
                var ttam = db.PSThongTinTrungTams.FirstOrDefault();
                try
                {
                    TrungTam.DiaChi = ttam.Diachi;
                    TrungTam.DienThoai = ttam.DienThoai;
                    TrungTam.MaTrungTam = ttam.MaTrungTam;
                    TrungTam.MaVietTat = ttam.MaVietTat;
                    TrungTam.TenTrungTam = ttam.TenTrungTam;
                    if (ttam.ChuKiTT.Length > 0)
                    {
                        try
                        {
                            byte[] b = ttam.ChuKiTT.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.ChuKiTT = img;
                        }
                        catch { }
                    }
                    if (ttam.ChuKiXN.Length > 0)
                    {
                        try
                        {
                            byte[] b = ttam.ChuKiXN.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.ChuKiXN = img;
                        }
                        catch { }
                    }
                    if (ttam.Header.Length > 0)
                    {
                        try
                        {
                            byte[] b = ttam.Header.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.Hearder = img;
                        }
                        catch { }
                    }
                    if (ttam.Logo.Length > 0)
                    {

                        try
                        {
                            byte[] b = ttam.Logo.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.Logo = img;
                        }
                        catch { }
                    }
                }
                catch { }
                rpt.TuNgay = tuNgay;
                rpt.DenNgay = denNgay;
                rpt.ThongTinTrungTam = TrungTam;
                rpt.ChiTietCacChiCuc = lstdata;
                return rpt;
            }
            catch
            {
                return rpt;
            }
        }
        public PsReponse DelPhieuTiepNhan(string maPhieu, string maBenhNhan)
        {
            PsReponse res = new PsReponse();
            try
            {
                var phieu = db.PSPhieuSangLocs.FirstOrDefault(x => x.IDPhieu == maPhieu);
                if (string.IsNullOrEmpty(phieu.IDPhieuLan1))
                {
                    var patient = db.PSPatients.FirstOrDefault(x => x.MaBenhNhan == maBenhNhan);
                    db.PSPatients.DeleteOnSubmit(patient);
                }
                else
                {
                    var phieu1 = db.PSPhieuSangLocs.FirstOrDefault(x => x.IDPhieu == phieu.IDPhieuLan1);
                    phieu1.TrangThaiMau = 6;
                    phieu1.isDongBo = false;
                    db.SubmitChanges();
                }
                db.PSPhieuSangLocs.DeleteOnSubmit(phieu);
                db.SubmitChanges();
                res.Result = true;
            }
            catch
            {
                res.Result = false;

            }
            return res;
        }
        public rptChiTietTrungTam GetBaoCaoTrungTamTongHopChiTietTheoChiCuc(DateTime tuNgay, DateTime denNgay)
        {
            rptChiTietTrungTam rpt = new rptChiTietTrungTam();
            try
            {
                PsThongTinTrungTam TrungTam = new PsThongTinTrungTam();
                List<rptChiTietTrungTam_ChiTiet> lstdata = new List<rptChiTietTrungTam_ChiTiet>();
                var data = db.pro_Report_TrungTamChiTietTheoTungChiCuc(tuNgay, denNgay).ToList();
                if (data != null)
                {
                    int stt = 0;
                    foreach (var rd in data)
                    {
                        rptChiTietTrungTam_ChiTiet dt = new rptChiTietTrungTam_ChiTiet();
                        dt.CAH = rd.CAHNguyCo ?? 0 + rd.CAHBinhThuong ?? 0;
                        dt.CAHBinhThuong = rd.CAHBinhThuong ?? 0;
                        dt.CAHBinhThuong_Tong = rd.CAHBinhThuong_Tong;
                        dt.CAHNguyCo = rd.CAHNguyCo ?? 0;
                        dt.CAHNguyCo_Tong = rd.CAHNguyCo_Tong;
                        dt.CH = rd.CHBinhThuong ?? 0 + rd.CHNguyCo ?? 0;
                        dt.CHBinhThuong = rd.CHBinhThuong ?? 0;
                        dt.CHBinhThuong_Tong = rd.CHBinhThuong_Tong;
                        dt.CHNguyCo = rd.CHNguyCo ?? 0;
                        dt.CHNguyCo_Tong = rd.CHNguyCo_Tong;
                        dt.Dat = rd.Dat ?? 0;
                        dt.G6PD = rd.G6PDBinhThuong ?? 0 + rd.G6PDNguyCo ?? 0;
                        dt.G6PDBinhThuong = rd.G6PDBinhThuong ?? 0;
                        dt.G6PDBinhThuong_Tong = rd.G6PDBinhThuong_Tong;
                        dt.G6PDNguyCo = rd.G6PDNguyCo ?? 0;
                        dt.G6PDNguyCo_Tong = rd.G6PDNguyCo_Tong;
                        dt.GAL = rd.GALBinhThuong ?? 0 + rd.GALNguyCo ?? 0;
                        dt.GALBinhThuong = rd.GALBinhThuong ?? 0;
                        dt.GALBinhThuong_Tong = rd.GALBinhThuong_Tong;
                        dt.GALNguyCo = rd.GALNguyCo ?? 0;
                        dt.GALNguyCo_Tong = rd.GALNguyCo_Tong;
                        dt.GTNa = rd.KhongXacDinh ?? 0;
                        dt.GTNam = rd.Nam ?? 0;
                        dt.GTNu = rd.Nu ?? 0;
                        dt.KhongDat = rd.KhongDat ?? 0;
                        dt.MaChiCuc = rd.MaChiCuc;
                        dt.TenChiCuc = rd.ChiCuc;
                        dt.XaHoiHoa = rd.XaHoi ?? 0;
                        dt.PKU = rd.PKUBinhThuong ?? 0 + rd.PKUNguyCo ?? 0;
                        dt.PKUNguyCo = rd.PKUNguyCo ?? 0;
                        dt.PUKBinhThuong_Tong = rd.PKUBinhThuong_Tong;
                        dt.PKUBinhThuong = rd.PKUBinhThuong ?? 0;
                        dt.PUKNguyCo_Tong = rd.PKUNguyCo_Tong;
                        dt.QuocGia = rd.QuocGia ?? 0;
                        dt.SinhMo = rd.SinhMo ?? 0;
                        dt.SinhNa = rd.KhongXacDinhSinh ?? 0;
                        dt.SinhThuong = rd.SinhThuong ?? 0;
                        dt.sl2Benh = rd.SL2Benh ?? 0;
                        dt.sl3Benh = rd.SL3Benh ?? 0;
                        dt.sl5Benh = rd.SL5Benh ?? 0;
                        dt.slMauThuLai = rd.SLThuLai ?? 0;
                        dt.SoLuongMau = rd.SoPhieu ?? 0;
                        dt.Stt = (stt + 1).ToString();
                        lstdata.Add(dt);
                    }
                }
                var ttam = db.PSThongTinTrungTams.FirstOrDefault();
                try
                {
                    TrungTam.DiaChi = ttam.Diachi;
                    TrungTam.DienThoai = ttam.DienThoai;
                    TrungTam.MaTrungTam = ttam.MaTrungTam;
                    TrungTam.MaVietTat = ttam.MaVietTat;
                    TrungTam.TenTrungTam = ttam.TenTrungTam;
                    if (ttam.ChuKiTT.Length > 0)
                    {
                        try
                        {
                            byte[] b = ttam.ChuKiTT.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.ChuKiTT = img;
                        }
                        catch { }
                    }
                    if (ttam.ChuKiXN.Length > 0)
                    {
                        try
                        {
                            byte[] b = ttam.ChuKiXN.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.ChuKiXN = img;
                        }
                        catch { }
                    }
                    if (ttam.Header.Length > 0)
                    {
                        try
                        {
                            byte[] b = ttam.Header.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.Hearder = img;
                        }
                        catch { }
                    }
                    if (ttam.Logo.Length > 0)
                    {

                        try
                        {
                            byte[] b = ttam.Logo.ToArray();
                            MemoryStream ms = new MemoryStream(b);
                            Image img = Image.FromStream(ms);
                            TrungTam.Logo = img;
                        }
                        catch { }
                    }
                }
                catch { }
                rpt.TuNgay = tuNgay;
                rpt.DenNgay = denNgay;
                rpt.ThongTinTrungTam = TrungTam;
                rpt.ChiTietCacChiCuc = lstdata;
                return rpt;
            }
            catch (Exception ex)
            {
                return rpt;
            }
        }
        public DataTable GetBaoCaoTrungTamTongHopCoBan(DateTime tuNgay, DateTime denNgay)
        {
            try
            {
                var data = db.pro_Report_TrungTamCoBan(tuNgay, denNgay).ToList();
                return ToDataTable(data);
            }
            catch
            {
                return new DataTable();
            }
        }
        public bool KiemTraChoPhepThuMauLan2()
        {
            try
            {
                return db.PSThongTinTrungTams.FirstOrDefault().isChoThuLaiMauLan2 ?? false;
            }
            catch { return false; }
        }
        public bool KiemTraChoPhepLamLaiXetNghiemLan2()
        {
            try
            {
                return db.PSThongTinTrungTams.FirstOrDefault().isChoXNLan2 ?? false;
            }
            catch { return false; }
        }
        public string GetGhiChuPhongXetNghiem(string maKQ)
        {
            string res = "Ghi chú xét nghiệm : \r\n";
            try
            {
                if (!string.IsNullOrEmpty(maKQ))
                {
                    var KQ = db.PSXN_KetQuas.FirstOrDefault(p => p.MaKetQua == maKQ && p.isXoa == false);
                    if (KQ != null)
                    {
                        if (!string.IsNullOrEmpty(KQ.GhiChu))
                            res = KQ.GhiChu;
                    }
                }
                return res;
            }
            catch { return null; }

        }
        public PSThongTinTrungTam GetThongTinTrungTam()
        {
            PSThongTinTrungTam tt = new PSThongTinTrungTam();
            try
            {
                tt = db.PSThongTinTrungTams.FirstOrDefault();
            }
            catch (Exception ex) { }
            return tt;
        }
        public PSEmployee GetThongTinNhanVien(string maNV)
        {
            PSEmployee NV = new PSEmployee();
            try
            {
                NV = db.PSEmployees.FirstOrDefault(p => p.EmployeeCode == maNV);
            }
            catch { }
            return NV;
        }

        public bool KiemTraMauDaLamLaiXetNghiemLan2DaVaoQuyTrinhXetNghieHayChua(string maPhieu, string maDonvi)
        {
            try
            {
                var res = db.PSXN_KetQuas.Where(p => p.MaPhieu == maPhieu && p.MaDonVi == maDonvi && p.isXoa == false).ToList();
                if (res.Count > 1) return true;
                else return false;
            }
            catch { return false; }
        }
        public int KiemTraTrangThaiPhieu(string maPhieu, string maDonVi)
        {
            try
            {
                var phieu = db.PSPhieuSangLocs.FirstOrDefault(p => p.IDPhieu == maPhieu && p.IDCoSo == maDonVi && p.isXoa == false);
                if (phieu != null)
                {
                    return phieu.TrangThaiMau ?? 0;
                }
                else { return 0; }
            }
            catch { return 0; }
        }
        public bool KiemTraMauDaLamLaiXetNghiemLan2(string maPhieu)
        {
            try
            {
                var res = db.PSChiDinhDichVus.Where(p => p.MaPhieu == maPhieu && p.isXoa == false).ToList();
                if (res.Count > 1) return true;
                else return false;
            }
            catch { return false; }
        }
        public string GetThongTinMaTiepNhan(string maPhieu, string maDonVi)
        {

            try
            {
                var phieu = db.PSXN_TraKetQuas.FirstOrDefault(p => p.MaPhieu == maPhieu && p.IDCoSo == maDonVi && p.isXoa == false);
                if (phieu != null)
                {
                    return phieu.MaTiepNhan;
                }
                else { return null; }
            }
            catch { return null; }
        }
        public PSDanhMucGhiChu GetThongTinHienThiGhiChu(string maGhiChu)
        {
            PSDanhMucGhiChu gc = new PSDanhMucGhiChu();
            try
            {
                if (!string.IsNullOrEmpty(maGhiChu))
                {
                    var res = db.PSDanhMucGhiChus.FirstOrDefault(p => p.MaGhiChu == maGhiChu);
                    if (res != null)
                        return res;
                    else return gc;
                }
                else
                {
                    return gc;
                }
            }
            catch
            {
                return gc;
            }
        }

        public List<PsTinhTrangPhieu> GetTinhTrangPhieu(DateTime startdate, DateTime enddate, string maDonVi)
        {
            string donvi = "";
            List<PsTinhTrangPhieu> lst = new List<PsTinhTrangPhieu>();
            try
            {

                if (maDonVi != null && !maDonVi.Equals("all"))
                {
                    donvi = maDonVi;
                    //return db.PSPhieuSangLocs.Where(p => p.NgayNhanMau.Value.Date >= startdate.Date && p.NgayNhanMau.Value.Date <= enddate.Date && p.IDCoSo == maDonVi).ToList();
                }
                else
                {
                    maDonVi = "";
                }

                var data = db.pro_Report_TrungTamTinhTrangMau(startdate, enddate, donvi).ToList();



                if (data.Count > 0)
                {
                    foreach (var _data in data)
                    {
                        PsTinhTrangPhieu tt = new PsTinhTrangPhieu();
                        tt.DiaChi = _data.DiaChi;
                        tt.IDPhieu = _data.IDPhieu;
                        tt.MaBenhNhan = _data.MaBenhNhan;
                        tt.MaDonVi = _data.MaDVCS;
                        tt.MaKhachHang = _data.MaKhachHang;
                        tt.NamSinhCha = _data.FatherBirthday ?? DateTime.Now;
                        tt.NamSinhMe = _data.MotherBirthday ?? DateTime.Now;
                        tt.NgayNhanMau = _data.NgayNhanMau ?? DateTime.Now;
                        tt.SdtCha = _data.FatherPhoneNumber;
                        tt.SdtMe = _data.MotherPhoneNumber;
                        tt.TenMe = _data.MotherName;
                        tt.TenBenhNhan = _data.TenBenhNhan;
                        tt.TenDonVi = _data.TenDVCS;
                        tt.TinhTrangMau_Text = _data.TrangThaiMau_Text;
                        tt.TinhTrangMau = _data.TrangThaiMau ?? 0;
                        lst.Add(tt);
                    }
                }
            }
            catch { }
            return lst;
        }
        public List<PsTinhTrangPhieu> GetTTPhieuCanSuaLoi(DateTime startdate, DateTime enddate, string maDonVi, string maChiCuc, string maphieu)
        {
            string donvi = "";
            List<PsTinhTrangPhieu> lst = new List<PsTinhTrangPhieu>();
            try
            {

                if (!string.IsNullOrEmpty(maphieu))
                {
                    var da = (from l in db.PSPhieuSangLocs
                              join p in db.PSPatients on l.MaBenhNhan equals p.MaBenhNhan
                              join dv in db.PSDanhMucDonViCoSos on l.IDCoSo equals dv.MaDVCS
                              join cc in db.PSDanhMucChiCucs on dv.MaChiCuc equals cc.MaChiCuc
                              where l.IDPhieu == maphieu && (l.TrangThaiMau == 7 || l.TrangThaiMau == 4 || l.TrangThaiMau == 6)
                              select new { l, p, dv, cc }).SingleOrDefault();

                    if (da != null)
                    {

                        PsTinhTrangPhieu tt = new PsTinhTrangPhieu();
                        tt.DiaChi = da.p.DiaChi;
                        tt.IDPhieu = da.l.IDPhieu;
                        tt.MaBenhNhan = da.p.MaBenhNhan;
                        tt.MaDonVi = da.l.IDCoSo;
                        tt.MaKhachHang = da.p.MaKhachHang;
                        tt.NamSinhCha = da.p.FatherBirthday ?? DateTime.Now;
                        tt.NamSinhMe = da.p.MotherBirthday ?? DateTime.Now;
                        tt.NgayNhanMau = da.l.NgayNhanMau ?? DateTime.Now;
                        tt.SdtCha = da.p.FatherPhoneNumber;
                        tt.SdtMe = da.p.MotherPhoneNumber;
                        tt.TenMe = da.p.MotherName;
                        tt.TenBenhNhan = da.p.TenBenhNhan;
                        tt.TenDonVi = da.dv.TenDVCS;
                        tt.TinhTrangMau = da.l.TrangThaiMau ?? 0;
                        tt.MaChiCuc = da.cc.MaChiCuc;
                        tt.TenChiCuc = da.cc.TenChiCuc;
                        switch (tt.TinhTrangMau)
                        {
                            case 1:
                                tt.TinhTrangMau_Text = "Đã tiếp nhận";
                                break;
                            case 2:
                                tt.TinhTrangMau_Text = "Đã đánh giá";
                                break;
                            case 3:
                                tt.TinhTrangMau_Text = "Đã vào phòng xét nghiệm";
                                break;
                            case 4:
                                tt.TinhTrangMau_Text = "Đã có kết quả";
                                break;
                            case 5:
                                tt.TinhTrangMau_Text = "Đang xét nghiệm lại lần 2";
                                break;
                            case 6:
                                tt.TinhTrangMau_Text = "Đang chờ thu mẫu lần 2";
                                break;
                            case 7:
                                tt.TinhTrangMau_Text = "Đã có kết quả - Đã thu lại mẫu";
                                break;
                            default:
                                tt.TinhTrangMau_Text = "Không xác định";
                                break;
                        }
                        lst.Add(tt);
                    }
                }
                else
                {
                    if (maDonVi != null && !maDonVi.Equals("all"))
                    {
                        donvi = maDonVi;
                        //return db.PSPhieuSangLocs.Where(p => p.NgayNhanMau.Value.Date >= startdate.Date && p.NgayNhanMau.Value.Date <= enddate.Date && p.IDCoSo == maDonVi).ToList();
                    }
                    else
                    {
                        maDonVi = maChiCuc;
                    }

                    var data = db.pro_Report_TrungTamTinhTrangMauMail(startdate, enddate, donvi, maChiCuc).ToList();
                    if (data.Count > 0)
                    {
                        foreach (var _data in data)
                        {
                            if (_data.TrangThaiMau == 7 || _data.TrangThaiMau == 4 || _data.TrangThaiMau == 6)
                            {
                                PsTinhTrangPhieu tt = new PsTinhTrangPhieu();
                                tt.DiaChi = _data.DiaChi;
                                tt.IDPhieu = _data.IDPhieu;
                                tt.MaBenhNhan = _data.MaBenhNhan;
                                tt.MaDonVi = _data.MaDVCS;
                                tt.MaKhachHang = _data.MaKhachHang;
                                tt.NamSinhCha = _data.FatherBirthday ?? DateTime.Now;
                                tt.NamSinhMe = _data.MotherBirthday ?? DateTime.Now;
                                tt.NgayNhanMau = _data.NgayNhanMau ?? DateTime.Now;
                                tt.SdtCha = _data.FatherPhoneNumber;
                                tt.SdtMe = _data.MotherPhoneNumber;
                                tt.TenMe = _data.MotherName;
                                tt.TenBenhNhan = _data.TenBenhNhan;
                                tt.TenDonVi = _data.TenDVCS;
                                tt.MaChiCuc = _data.MaChiCuc;
                                tt.TenChiCuc = _data.TenChiCuc;
                                tt.TinhTrangMau_Text = _data.TrangThaiMau_Text;
                                tt.TinhTrangMau = _data.TrangThaiMau ?? 0;
                                lst.Add(tt);
                            }

                        }
                    }
                }

            }
            catch { }
            return lst;
        }
        public PsReponse UpdatePhieuSuaLoi(List<string> maphieu)
        {
            PsReponse res = new PsReponse();
            try
            {
                foreach (var phieu in maphieu)
                {

                    // db.pro_DeletePhieuLoi(phieu);
                    var kq = db.PSXN_TraKetQuas.FirstOrDefault(x => x.MaPhieu == phieu && x.isXoa != true);
                    kq.isTraKQ = false;
                    kq.isDaDuyetKQ = false;
                    kq.isDongBo = false;
                    kq.isDongBoPDF = false;
                    kq.KetLuanTongQuat = string.Empty;
                    kq.GhiChu = string.Empty;
                    kq.isDaGuiMail = false;
                    var ctkq = db.PSXN_TraKQ_ChiTiets.Where(x => x.MaPhieu == phieu && x.isXoa != true);
                    foreach (var ct in ctkq)
                    {
                        ct.isDongBo = false;
                    }
                    var phieuc = db.PSPhieuSangLocs.FirstOrDefault(x => x.IDPhieu == phieu && x.isXoa != true);
                    if (phieuc.isXNLan2 != true)
                    {
                        phieuc.TrangThaiMau = 3;
                    }
                    else
                    {
                        phieuc.TrangThaiMau = 5;
                    }
                    phieuc.isDongBo = false;
                    db.SubmitChanges();
                    res.Result = true;
                }

            }
            catch
            {
                res.Result = false;
            }
            return res;
        }
        public PsReponse RestoreCTChatLuongMau()
        {
            PsReponse res = new PsReponse();
            try
            {
                var cl = db.PSDanhMucDanhGiaChatLuongMaus.ToList();
                foreach (var ctcl in cl)
                {
                    var kp = db.PSPhieuSangLocs.Where(d => !db.PSChiTietDanhGiaChatLuongs.Any(c => c.IDPhieu == d.IDPhieu && c.IDDanhGiaChatLuongMau == ctcl.IDDanhGiaChatLuongMau) && d.isKhongDat == true && d.isXoa != true && d.LyDoKhongDat.Contains(ctcl.ChatLuongMau)).ToList();
                    foreach (var ctkp in kp)
                    {
                        var matiepnhan = db.PSTiepNhans.FirstOrDefault(x => x.MaPhieu == ctkp.IDPhieu && x.MaDonVi == ctkp.IDCoSo).MaTiepNhan;
                        PSChiTietDanhGiaChatLuong ctdg = new PSChiTietDanhGiaChatLuong();
                        ctdg.IDPhieu = ctkp.IDPhieu;
                        ctdg.MaTiepNhan = matiepnhan;
                        ctdg.IDDanhGiaChatLuongMau = ctcl.IDDanhGiaChatLuongMau;
                        ctdg.TenLyDo = ctcl.ChatLuongMau;
                        ctdg.isDongBo = false;
                        ctdg.isXoa = false;
                        db.PSChiTietDanhGiaChatLuongs.InsertOnSubmit(ctdg);
                        db.SubmitChanges();
                    }
                    res.Result = true;
                }
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.StringError = ex.ToString();
            }
            return res;
        }
        public List<PsCTDanhGiaChatLuong> GetCTChatLuogMau(string MaPhieu, string IDCoSo, string IDChiCuc, DateTime NgayBD, DateTime NgayKT)
        {
            List<PsCTDanhGiaChatLuong> list = new List<PsCTDanhGiaChatLuong>();
            db.CommandTimeout = 2000;
            try
            {
                var phieu = (from ph in db.PSPhieuSangLocs
                             join ct in db.PSChiDinhDichVus on ph.IDPhieu equals ct.MaPhieu
                             join dv in db.PSDanhMucDonViCoSos on ph.IDCoSo equals dv.MaDVCS
                             where ph.isKhongDat == true && ph.TrangThaiMau > 1 && ct.IDGoiDichVu != "DVGXNL2" && ct.isXoa != true && ct.isDaNhapLieu == true && ph.isXoa != true
                             && ph.NgayNhanMau.Value.Date >= NgayBD && ph.NgayNhanMau.Value.Date <= NgayKT
                             select new { ph, ct, dv.TenDVCS, dv.MaChiCuc }).ToList();
                if (phieu != null)
                {
                    foreach (var ph in phieu)
                    {
                        PsCTDanhGiaChatLuong ct = new PsCTDanhGiaChatLuong();
                        ct.IDPhieu = ph.ph.IDPhieu;
                        ct.MaTiepNhan = ph.ct.MaTiepNhan;
                        ct.LyDoKhongDat = ph.ph.LyDoKhongDat;
                        ct.TenDonVi = ph.TenDVCS;
                        ct.NgayTiepNhan = ph.ph.NgayNhanMau;
                        ct.IDChiCuc = ph.MaChiCuc;
                        ct.IDCoSo = ph.ph.IDCoSo;
                        list.Add(ct);
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return list;

        }
        public PSTKKQPhieuMail GetThongKePhieuMail(string[] maphieu)
        {
            PSTKKQPhieuMail tt = new PSTKKQPhieuMail();
            try
            {
                List<PSPhieuSangLoc> phieu = new List<PSPhieuSangLoc>();
                List<PSChiTietDanhGiaChatLuong> ct = new List<PSChiTietDanhGiaChatLuong>();
                List<PSXN_TraKQ_ChiTiet> trakqlan1 = new List<PSXN_TraKQ_ChiTiet>();
                List<PSXN_TraKQ_ChiTiet> trakqlan2 = new List<PSXN_TraKQ_ChiTiet>();
                foreach (string MaPhieu in maphieu)
                {
                    var kqmau = db.PSPhieuSangLocs.FirstOrDefault(x => x.IDPhieu == MaPhieu && x.isXoa != true);
                    if (kqmau != null)
                    {
                        var patt = db.PSChiTietDanhGiaChatLuongs.Where(x => x.IDPhieu == kqmau.IDPhieu && x.isXoa != true).ToList();
                        ct.AddRange(patt);
                        if (string.IsNullOrEmpty(kqmau.IDPhieuLan1))
                        {
                            var ctkq = db.PSXN_TraKQ_ChiTiets.Where(x => x.MaPhieu == kqmau.IDPhieu && x.isXoa != true).ToList();
                            trakqlan1.AddRange(ctkq);
                        }
                        else
                        {
                            var ctkq2 = db.PSXN_TraKQ_ChiTiets.Where(x => x.MaPhieu == kqmau.IDPhieu && x.isXoa != true).ToList();
                            trakqlan2.AddRange(ctkq2);
                        }
                    }
                    phieu.Add(kqmau);
                }
                tt.MauMoi = phieu.Where(x => x.isLayMauLan2 != true).ToList().Count();
                tt.MauXNL = phieu.Where(x => x.isLayMauLan2 == true).ToList().Count();
                tt.MauDat = phieu.Where(x => x.isKhongDat != true).ToList().Count();
                tt.MauKDat = phieu.Where(x => x.isKhongDat == true).ToList().Count();
                tt.ThuSom = ct.Where(x => x.IDDanhGiaChatLuongMau.Trim() == "MS").ToList().Count();
                tt.GuiCham = ct.Where(x => x.IDDanhGiaChatLuongMau.Trim() == "MM").ToList().Count();
                tt.MauChong = ct.Where(x => x.IDDanhGiaChatLuongMau.Trim() == "CL").ToList().Count();
                tt.MauChuaKho = ct.Where(x => x.IDDanhGiaChatLuongMau.Trim() == "CK").ToList().Count();
                tt.MauCoVongHuyetThanh = ct.Where(x => x.IDDanhGiaChatLuongMau.Trim() == "HT").ToList().Count();
                tt.Mauit = ct.Where(x => x.IDDanhGiaChatLuongMau.Trim() == "MI").ToList().Count();
                tt.MauKdeu = ct.Where(x => x.IDDanhGiaChatLuongMau.Trim() == "KT").ToList().Count();
                tt.SinhNonNheCan = ct.Where(x => x.IDDanhGiaChatLuongMau.Trim() == "NC").ToList().Count();
                tt.LyDoKhac = ct.ToList().Count() - tt.Mauit - tt.MauKdeu - tt.MauChong - tt.SinhNonNheCan - tt.ThuSom - tt.GuiCham;

                tt.benh2 = phieu.Where(x => x.MaGoiXN == "DVGXN0002").ToList().Count();
                tt.benh3 = phieu.Where(x => x.MaGoiXN == "DVGXN0003").ToList().Count();
                tt.benh5 = phieu.Where(x => x.MaGoiXN == "DVGXN0004").ToList().Count();
                tt.G6PD = trakqlan1.Where(x => x.IDThongSoXN == "G6PD" && x.isNguyCo == false).ToList().Count();
                tt.PKU = trakqlan1.Where(x => x.IDThongSoXN == "PKU" && x.isNguyCo == false).ToList().Count();
                tt.GAL = trakqlan1.Where(x => x.IDThongSoXN == "GAL" && x.isNguyCo == false).ToList().Count();
                tt.CAH = trakqlan1.Where(x => x.IDThongSoXN == "CAH" && x.isNguyCo == false).ToList().Count();
                tt.CH = trakqlan1.Where(x => x.IDThongSoXN == "CH" && x.isNguyCo == false).ToList().Count();
                tt.G6PD2 = trakqlan1.Where(x => x.IDThongSoXN == "G6PD" && x.isNguyCo == true).ToList().Count();
                tt.PKU2 = trakqlan1.Where(x => x.IDThongSoXN == "PKU" && x.isNguyCo == true).ToList().Count();
                tt.GAL2 = trakqlan1.Where(x => x.IDThongSoXN == "GAL" && x.isNguyCo == true).ToList().Count();
                tt.CAH2 = trakqlan1.Where(x => x.IDThongSoXN == "CAH" && x.isNguyCo == true).ToList().Count();
                tt.CH2 = trakqlan1.Where(x => x.IDThongSoXN == "CH" && x.isNguyCo == true).ToList().Count();
                tt.G6PD3 = trakqlan2.Where(x => x.IDThongSoXN == "G6PD").ToList().Count();
                tt.PKU3 = trakqlan2.Where(x => x.IDThongSoXN == "PKU").ToList().Count();
                tt.GAL3 = trakqlan2.Where(x => x.IDThongSoXN == "GAL").ToList().Count();
                tt.CAH3 = trakqlan2.Where(x => x.IDThongSoXN == "CAH").ToList().Count();
                tt.CH3 = trakqlan2.Where(x => x.IDThongSoXN == "CH").ToList().Count();
                tt.G6PD4 = trakqlan2.Where(x => x.IDThongSoXN == "G6PD" && x.isNguyCo == false).ToList().Count();
                tt.PKU4 = trakqlan2.Where(x => x.IDThongSoXN == "PKU" && x.isNguyCo == false).ToList().Count();
                tt.GAL4 = trakqlan2.Where(x => x.IDThongSoXN == "GAL" && x.isNguyCo == false).ToList().Count();
                tt.CAH4 = trakqlan2.Where(x => x.IDThongSoXN == "CAH" && x.isNguyCo == false).ToList().Count();
                tt.CH4 = trakqlan2.Where(x => x.IDThongSoXN == "CH" && x.isNguyCo == false).ToList().Count();
                tt.G6PD5 = trakqlan2.Where(x => x.IDThongSoXN == "G6PD" && x.isNguyCo == true).ToList().Count();
                tt.PKU5 = trakqlan2.Where(x => x.IDThongSoXN == "PKU" && x.isNguyCo == true).ToList().Count();
                tt.GAL5 = trakqlan2.Where(x => x.IDThongSoXN == "GAL" && x.isNguyCo == true).ToList().Count();
                tt.CAH5 = trakqlan2.Where(x => x.IDThongSoXN == "CAH" && x.isNguyCo == true).ToList().Count();
                tt.CH5 = trakqlan2.Where(x => x.IDThongSoXN == "CH" && x.isNguyCo == true).ToList().Count();
                tt.NguyCoThap = trakqlan1.Where(x => x.isNguyCo == false).ToList().Count();
                tt.NguyCoCao = trakqlan1.Where(x => x.isNguyCo == true).ToList().Count();
                tt.NguyCoThap2 = trakqlan2.Where(x => x.isNguyCo == false).ToList().Count();
                tt.NguyCoCao2 = trakqlan2.Where(x => x.isNguyCo == true).ToList().Count();



            }
            catch { }
            return tt;
        }
        public List<PSDanhSachGuiSMS> GetDanhSachGuiSMS(DateTime TuNgay, DateTime DenNgay, int TrangThaiPhieu, int nguyco, string MaDV, string NoiDung, string DoiTuong, bool KieuKiTu, int TrangThaiGui, 
            int SDT,string NoiDungGUi,string MauGui,string HinhThuc,bool SDT1)
        {
            List<PSDanhSachGuiSMS> lst = new List<PSDanhSachGuiSMS>();
            try
            {
                List<PSPhieuSangLoc> phieu = new List<PSPhieuSangLoc>();
                switch (TrangThaiPhieu)
                {
                    case 1:
                        {
                            phieu = db.PSPhieuSangLocs.Where(p => p.TrangThaiMau <= 2 &&
                            p.NgayNhanMau.Value.Date <= DenNgay && p.NgayNhanMau.Value.Date >= TuNgay.Date && p.isXoa != true).ToList();
                            break;
                        }
                    case 2:
                        {
                            phieu = phieu.Where(p => (p.TrangThaiMau == 3 || p.TrangThaiMau == 5) &&
                            p.NgayNhanMau.Value.Date <= DenNgay && p.NgayNhanMau.Value.Date >= TuNgay.Date && p.isXoa != true).ToList();
                            break;
                        }
                    case 3:
                        {
                            if (nguyco == 1)
                            {
                                phieu = db.PSPhieuSangLocs.Where(p => p.TrangThaiMau >= 4 && p.TrangThaiMau != 5 &&
                               p.NgayCoKQ.Value.Date <= DenNgay && p.NgayCoKQ.Value.Date >= TuNgay.Date && p.isXoa != true && p.isNguyCoCao == true).ToList();
                            }
                            else if (nguyco == 0)
                            {
                                phieu = phieu = db.PSPhieuSangLocs.Where(p => p.TrangThaiMau >= 4 && p.TrangThaiMau != 5 &&
                              p.NgayCoKQ.Value.Date <= DenNgay && p.NgayCoKQ.Value.Date >= TuNgay.Date && p.isXoa != true && p.isNguyCoCao != true).ToList();
                            }
                            else
                            {
                                phieu = db.PSPhieuSangLocs.Where(p => p.TrangThaiMau >= 4 && p.TrangThaiMau != 5 &&
                               p.NgayCoKQ.Value.Date <= DenNgay && p.NgayCoKQ.Value.Date >= TuNgay.Date && p.isXoa != true).ToList();
                            }
                            break;
                        }
                    case 4:
                        {
                            if (nguyco == 1)
                            {
                                phieu = db.PSPhieuSangLocs.Where(p => p.TrangThaiMau >= 4 && p.TrangThaiMau != 5 && p.isLayMauLan2 != true &&
                               p.NgayCoKQ.Value.Date <= DenNgay && p.NgayCoKQ.Value.Date >= TuNgay.Date && p.isXoa != true && p.isNguyCoCao == true).ToList();
                            }
                            else if (nguyco == 0)
                            {
                                phieu = phieu = db.PSPhieuSangLocs.Where(p => p.TrangThaiMau >= 4 && p.TrangThaiMau != 5 && p.isLayMauLan2 != true &&
                              p.NgayCoKQ.Value.Date <= DenNgay && p.NgayCoKQ.Value.Date >= TuNgay.Date && p.isXoa != true && p.isNguyCoCao != true).ToList();
                            }
                            else
                            {
                                phieu = phieu = db.PSPhieuSangLocs.Where(p => p.TrangThaiMau >= 4 && p.TrangThaiMau != 5 && p.isLayMauLan2 != true &&
                              p.NgayCoKQ.Value.Date <= DenNgay && p.NgayCoKQ.Value.Date >= TuNgay.Date && p.isXoa != true).ToList();
                            }
                            break;
                        }
                    case 6:
                        {
                            if (nguyco == 1)
                            {
                                phieu = db.PSPhieuSangLocs.Where(p => p.TrangThaiMau >= 4 && p.TrangThaiMau != 5 && p.isLayMauLan2 == true &&
                               p.NgayCoKQ.Value.Date <= DenNgay && p.NgayCoKQ.Value.Date >= TuNgay.Date && p.isXoa != true && p.isNguyCoCao == true).ToList();
                            }
                            else if (nguyco == 0)
                            {
                                phieu = phieu = db.PSPhieuSangLocs.Where(p => p.TrangThaiMau >= 4 && p.TrangThaiMau != 5 && p.isLayMauLan2 == true &&
                              p.NgayCoKQ.Value.Date <= DenNgay && p.NgayCoKQ.Value.Date >= TuNgay.Date && p.isXoa != true && p.isNguyCoCao != true).ToList();
                            }
                            else
                            {
                                phieu = phieu = db.PSPhieuSangLocs.Where(p => p.TrangThaiMau >= 4 && p.TrangThaiMau != 5 && p.isLayMauLan2 == true &&
                              p.NgayCoKQ.Value.Date <= DenNgay && p.NgayCoKQ.Value.Date >= TuNgay.Date && p.isXoa != true).ToList();
                            }
                            break;
                        }
                    case 0:
                        {
                            phieu = db.PSPhieuSangLocs.Where(p =>
                            p.NgayNhanMau.Value.Date <= DenNgay && p.NgayNhanMau.Value.Date >= TuNgay.Date && p.isXoa != true).ToList();
                            break;
                        }
                }
                if (phieu.Count() > 0)
                {
                    if (!MaDV.Equals("all"))
                    {
                        phieu = phieu.Where(p => p.IDCoSo.Equals(MaDV)).ToList();
                    }
                    var dsdv = GetDanhSachDonViSMS(MaDV);
                    if (dsdv.Count > 0)
                    {
                        List<PSPhieuSangLoc> phieu1 = new List<PSPhieuSangLoc>();
                        foreach (var dv in dsdv)
                        {
                            var phieudv = phieu.Where(p => p.IDCoSo.Equals(dv.MaDVCS)).ToList();
                            phieu1.AddRange(phieudv);
                        }
                        phieu = null;
                        phieu = phieu1;
                    }
                    else
                    {
                        phieu = null;
                    }

                    foreach (var ph in phieu)
                    {
                        PSPatient pa = db.PSPatients.FirstOrDefault(x => x.MaBenhNhan == ph.MaBenhNhan && x.isXoa != true);
                        if (pa != null)
                        {
                            PSDanhSachGuiSMS dsms = new PSDanhSachGuiSMS();
                            dsms.MaPhieu = ph.IDPhieu;
                            dsms.TenTre = pa.TenBenhNhan;
                            dsms.MaKhachHang = pa.MaKhachHang;
                            var dsdaGuiSMS = db.PSSMSCs.FirstOrDefault(x => x.MaKhachHang.Equals(pa.MaKhachHang) && x.GroupSMS == HinhThuc && x.NoiDungGui.Equals(NoiDungGUi) && x.IDPhieu==ph.IDPhieu);
                            if (dsdaGuiSMS != null)
                            {
                                if (dsdaGuiSMS.isDaGui == true)
                                {
                                    dsms.isDaGui = true;
                                }
                                else if (dsdaGuiSMS.isDaGui == false)
                                {
                                    dsms.isDaGui = false;
                                }
                            }
                            if (!string.IsNullOrEmpty(pa.MotherPhoneNumber) && SDT1==false)
                            {
                                dsms.TenNguoiNhan = "chị " + pa.MotherName;
                                if (pa.MotherPhoneNumber.StartsWith("0") == true)
                                {
                                    dsms.SDTNguoiNhan = "84" + pa.MotherPhoneNumber.Substring(1);
                                }
                            }
                            else if (!string.IsNullOrEmpty(pa.FatherPhoneNumber) && SDT1==true)
                            {
                                dsms.TenNguoiNhan = "anh " + pa.FatherName;
                                if (pa.FatherPhoneNumber.StartsWith("0") == true)
                                {
                                    dsms.SDTNguoiNhan = "84" + pa.FatherPhoneNumber.Substring(1);
                                }
                            }
                            dsms.NoiDungTinNhan = NoiDungVietTatSMS(NoiDung, ph.IDPhieu, pa.TenBenhNhan, dsms.TenNguoiNhan, KieuKiTu, int.Parse(ph.TrangThaiMau.ToString()), pa.NgayGioSinh.Value);
                            dsms.HinhThucGui = HinhThuc;
                            dsms.NoiDungGui = NoiDungGUi;
                            dsms.MauGui = string.IsNullOrEmpty(MauGui)?0:int.Parse(MauGui);
                            dsms.SoKiTu = dsms.NoiDungTinNhan.Length;
                            lst.Add(dsms);
                        }
                    }
                    if (SDT == 1)
                    {
                        lst = lst.Where(x => x.SDTNguoiNhan != null).ToList();
                    }
                    else if (SDT == 0)
                    {
                        lst = lst.Where(x => x.SDTNguoiNhan == null).ToList();
                    }
                    switch (TrangThaiGui)
                    {
                        case 1:
                            {
                                lst = lst.Where(x => x.isDaGui == null).ToList();
                                break;
                            }
                        case 2:
                            {
                                lst = lst.Where(x => x.isDaGui == false).ToList();
                                break;
                            }
                        case 3:
                            {
                                lst = lst.Where(x => x.isDaGui == true).ToList();
                                break;
                            }
                    }
                }
            }
            catch
            {}
            return lst;
        }
        public string GetTenTrangThaiPhieu(int TrangThaiPhieu)
        {
            string TenTrangThai = "không xác định";
            if (TrangThaiPhieu <= 2)
            {
                TenTrangThai = "tiếp nhận và đánh giá mẫu";
            }
            else if (TrangThaiPhieu == 3 || TrangThaiPhieu == 5)
            {
                TenTrangThai = "đang xét nghiệm";
            }
            else if (TrangThaiPhieu == 4)
            {
                TenTrangThai = "đã có kết quả";
            }
            else if (TrangThaiPhieu == 6)
            {
                TenTrangThai = "đã có kết quả- cần thu lại mẫu";
            }
            else if (TrangThaiPhieu == 7)
            {
                TenTrangThai = "đã có kết quả- đã thu lại mẫu";
            }
            return TenTrangThai;
        }
        public string[] GetKetQua(string MaPhieu)
        {
            string[] lst = new string[2];
            string KQ = "chưa có kết quả";
            string KQNCC = "Nguy cơ cao(";
            string KQNCT = "Nguy cơ thấp(";
            string KetLuan = "";
            bool ncc = false;
            var XNKQ = db.PSXN_TraKetQuas.FirstOrDefault(x => x.MaPhieu == MaPhieu && x.isXoa != true);
            if (XNKQ != null)
            {
                switch (XNKQ.isNguyCoCao)
                {
                    case true:
                        {
                            KetLuan = "nguy cơ cao";
                            break;
                        }
                    default:
                        {
                            KetLuan = "nguy cơ thấp";
                            break;
                        }
                }
                foreach (var ctkq in XNKQ.PSXN_TraKQ_ChiTiets)
                {
                    string ten = GetTenDichVuVietTatCuaKyThuat(ctkq.IDKyThuat);
                    if (ctkq.isNguyCo)
                    {
                        if (KQNCC.Equals("Nguy cơ cao("))
                        {
                            KQNCC = KQNCC + ten;
                        }
                        else
                        {
                            KQNCC = KQNCC + "," + ten;
                        }
                        ncc = true;

                    }
                    else
                    {
                        if (KQNCT.Equals("Nguy cơ thấp("))
                        {
                            KQNCT = KQNCT + ten;
                        }
                        else
                        {
                            KQNCT = KQNCT + "," + ten;
                        }
                    }
                }
                if (ncc)
                {
                    KQ = KQNCT + ")," + KQNCC + ")";
                }
                else
                {
                    KQ = KQNCT + ")";
                }
            }

            lst[0] = KQ;
            lst[1] = KetLuan;
            return lst;
        }
        public PsReponse InsertMauSMS(PSDanhMucMauSMS sms)
        {
            PsReponse res = new PsReponse();
            try
            {
                if (sms.RowIDMauSMS == 0)
                {
                    db.PSDanhMucMauSMS.InsertOnSubmit(sms);
                    db.SubmitChanges();
                }
                else
                {
                    var sm = db.PSDanhMucMauSMS.FirstOrDefault(x => x.RowIDMauSMS == sms.RowIDMauSMS);
                    sm.MauNoiDungGui = sms.MauNoiDungGui;
                    db.SubmitChanges();
                }
                res.Result = true;
            }
            catch
            {

            }
            return res;
        }
        public List<PSDanhMucMauSMS> LoadMauSMS(string HinhThucGuiTN, string DoituongNhanTinNhan, string Noidunggui)
        {
            List<PSDanhMucMauSMS> danhmucsms = new List<PSDanhMucMauSMS>();
            try
            {
                danhmucsms = db.PSDanhMucMauSMS.OrderBy(y=>y.STT).Where(x => x.HinhThucGuiTN == HinhThucGuiTN && x.DoiTuongNhanTN == DoituongNhanTinNhan && x.NoidungGui == Noidunggui).ToList();
            }
            catch
            {
            }
            return danhmucsms;
        }
        public string NoiDungVietTatSMS(string NoiDung, string maPhieu, string TenTre, string TenNguoiNhan, bool isCoDau, int Trangthaiphieu, DateTime ngaysinh)
        {
            string tam = string.Empty;
            try
            {
                string Trangthai = GetTenTrangThaiPhieu(Trangthaiphieu);
                string[] KQKL = new string[2];
                KQKL = GetKetQua(maPhieu);
                tam = NoiDung.Replace("#maphieu", maPhieu);
                tam = tam.Replace("#tentre", TenTre);
                tam = tam.Replace("#tennguoinhan", TenNguoiNhan);
                tam = tam.Replace("#trangthaiphieu", Trangthai);
                tam = tam.Replace("#ketqua", KQKL[0]);
                tam = tam.Replace("#ketluan", KQKL[1]);
                tam = tam.Replace("#ngaysinh", ngaysinh.ToShortDateString());
                if (!isCoDau)
                {
                    Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
                    tam = tam.Normalize(NormalizationForm.FormD);
                    tam = regex.Replace(tam, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
                    return tam;
                }
                else
                {
                    return tam;
                }
            }
            catch
            {

            }
            return tam;
        }

        public PsReponse InsertSMSNumber(PSDanhSachGuiSMS sms, PsReponseSMS kq, string MaNV)
        {
            PsReponse psReponse = new PsReponse();
            try
            {
                var smsC = db.PSSMSCs.FirstOrDefault(x => x.MaKhachHang.Equals(sms.MaKhachHang) && x.NoiDungGui.Equals(sms.NoiDungGui) && x.GroupSMS.Equals(sms.HinhThucGui) && x.IDPhieu==sms.MaPhieu);
                if(smsC!=null)
                {
                    PSSMSLog log = new PSSMSLog();
                    log.RowIDNumber = smsC.RowIDNumber;
                    log.SMSContent = sms.NoiDungTinNhan;
                    log.TimeSend = DateTime.Now;
                    log.IDNhanVienSend = MaNV;
                    log.NumberMobile = sms.SDTNguoiNhan;
                    log.SMSError = kq.StringError;
                    log.isResult = kq.Result;
                    log.IDMauSend = sms.MauGui;
                    log.RowIDSMSlog = 0;
                    db.PSSMSLogs.InsertOnSubmit(log);
                    db.SubmitChanges();
                }
                else
                {
                    PSSMSC pSSMS = new PSSMSC();
                    pSSMS.MaKhachHang = sms.MaKhachHang;
                    pSSMS.NoiDungGui = sms.NoiDungGui;
                    pSSMS.GroupSMS = sms.HinhThucGui;
                    pSSMS.isDaGui = kq.Result;
                    pSSMS.RowIDNumber = 0;
                    pSSMS.IDPhieu = sms.MaPhieu;
                    db.PSSMSCs.InsertOnSubmit(pSSMS);
                    db.SubmitChanges();
                    var t = db.PSSMSCs.FirstOrDefault(x => x.MaKhachHang.Equals(sms.MaKhachHang) && x.NoiDungGui.Equals(sms.NoiDungGui) && x.GroupSMS.Equals(sms.HinhThucGui));
                    if (t != null)
                    {
                        PSSMSLog log = new PSSMSLog();
                        log.RowIDNumber = t.RowIDNumber;
                        log.SMSContent = sms.NoiDungTinNhan;
                        log.TimeSend = DateTime.Now;
                        log.IDNhanVienSend = MaNV;
                        log.NumberMobile = sms.SDTNguoiNhan;
                        log.SMSError = kq.StringError;
                        log.isResult = kq.Result;
                        log.IDMauSend = sms.MauGui;                        
                        log.RowIDSMSlog = 0;
                        db.PSSMSLogs.InsertOnSubmit(log);
                        db.SubmitChanges();
                    }
                }                                       
            }
            catch
            {
            }
            return psReponse;
        }
        public bool KiemTraGioiHan()
        {
            try
            {
                var res = db.PSTiepNhans.ToList();
                if (res.Count > 1000)
                    return true;
                else return false;
            }
            catch { return false; }
        }
        public List<PsTinhTrangPhieu> GetTinhTrangPhieuMail(DateTime startdate, DateTime enddate, string maDonVi, string maChiCuc)
        {
            string donvi = "";
            List<PsTinhTrangPhieu> lst = new List<PsTinhTrangPhieu>();
            try
            {

                if (maDonVi != null && !maDonVi.Equals("all"))
                {
                    donvi = maDonVi;

                }
                else
                {
                    maDonVi = "";
                    if (maChiCuc == null || maChiCuc.Equals("all"))
                    {
                        maChiCuc = "";
                    }
                }

                var data = db.pro_Report_TrungTamTinhTrangMauMail(startdate, enddate, donvi, maChiCuc).ToList();
                if (data.Count > 0)
                {
                    foreach (var _data in data)
                    {
                        PsTinhTrangPhieu tt = new PsTinhTrangPhieu();
                        tt.DiaChi = _data.DiaChi;
                        tt.IDPhieu = _data.IDPhieu;
                        tt.MaBenhNhan = _data.MaBenhNhan;
                        tt.MaDonVi = _data.MaDVCS;
                        tt.MaKhachHang = _data.MaKhachHang;
                        tt.NamSinhCha = _data.FatherBirthday ?? DateTime.Now;
                        tt.NamSinhMe = _data.MotherBirthday ?? DateTime.Now;
                        tt.NgayNhanMau = _data.NgayNhanMau ?? DateTime.Now;
                        tt.SdtCha = _data.FatherPhoneNumber;
                        tt.SdtMe = _data.MotherPhoneNumber;
                        tt.TenMe = _data.MotherName;
                        tt.TenBenhNhan = _data.TenBenhNhan;
                        tt.TenDonVi = _data.TenDVCS;
                        tt.TinhTrangMau_Text = _data.TrangThaiMau_Text;
                        tt.TinhTrangMau = _data.TrangThaiMau ?? 0;
                        tt.Chon = 0;
                        tt.TenChiCuc = _data.TenChiCuc;
                        tt.Email = _data.Email;
                        tt.isNguyCoCao = _data.isNguyCoCao;
                        bool? mail = db.PSXN_TraKetQuas.FirstOrDefault(y => y.MaPhieu == _data.IDPhieu && y.isXoa != true).isDaGuiMail;
                        tt.isDaGuiMail = mail ?? false;
                        lst.Add(tt);
                    }
                }
            }
            catch { }
            return lst;
        }

        public PsReponse CapNhatGuiMail(List<String> MaPhieu)
        {
            PsReponse res = new PsReponse();
            try
            {
                var data = db.PSXN_TraKetQuas.Where(s => (MaPhieu).Contains(s.MaPhieu)).ToList();
                data.ToList().ForEach(c => c.isDaGuiMail = true);
                db.SubmitChanges();
                db.Transaction.Commit();
                db.Connection.Close();
                res.Result = true;
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.StringError = ex.ToString();
            }
            return res;
        }

        public bool KiemTraBenhNhanNguyCoCaoDaVaoDotChanDoanChua(string maTiepNhan)
        {
            try
            {
                var res = db.PSDotChuanDoans.FirstOrDefault(p => p.MaBenhNhan == db.PSBenhNhanNguyCoCaos.FirstOrDefault(o => o.MaTiepNhan == maTiepNhan && p.isXoa == false).MaBenhNhan);
                if (res != null) return true;
                else return false;
            }
            catch { return false; }
        }
        public bool KiemTraPhieuDaDuyetHayChua(string maPhieu, string maTiepNhan)
        {
            try
            {
                var res = db.PSXN_TraKetQuas.FirstOrDefault(p => p.MaPhieu == maPhieu && p.MaTiepNhan == maTiepNhan && p.isXoa == false);
                if (res != null)
                {
                    bool isDuyet = res.isDaDuyetKQ ?? false;
                    if (isDuyet)
                    {
                        return true;
                    }
                    else return false;
                }
                return false;
            }
            catch
            {
                return true;
            }
        }
        public List<PSBenhNhanNguyCoCao> GetDanhSachBenhNhanNguyCoCao(string maDonvi, bool nguyco, DateTime tuNgay, DateTime denNgay)
        {
            List<PSBenhNhanNguyCoCao> lst = new List<PSBenhNhanNguyCoCao>();
            try
            {
                if (string.IsNullOrEmpty(maDonvi))
                {
                    var res = db.PSBenhNhanNguyCoCaos.Where(p => p.isNguyCoCao == nguyco && p.NgayTiepNhan.Value.Date <= denNgay && p.NgayTiepNhan.Value.Date >= tuNgay.Date && p.isXoa == false).ToList();
                    if (res.Count > 0) return res;
                    else return lst;
                }
                else
                {
                    var res = db.PSBenhNhanNguyCoCaos.Where(p => p.isNguyCoCao == nguyco && p.NgayTiepNhan.Value.Date <= denNgay && p.NgayTiepNhan.Value.Date >= tuNgay.Date && p.isXoa == false && p.MaDonVi.Contains(maDonvi)).ToList();
                    if (res.Count > 0) return res;
                    else return lst;
                }
            }
            catch { return lst; }
        }
        public string GetNewMaKhachHang(string maDonVi, string chuoiNamThang)
        {

            int numID = 0;
            string maKH = string.Empty;
            string startStr = maDonVi + chuoiNamThang;
            try
            {
                var res = db.PSPatients.Where(p => p.MaKhachHang.StartsWith(maDonVi + chuoiNamThang) && p.isXoa !=true).ToList();
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
        public string GetMaXN(string maTiepNhan)
        {
            try
            {
                var res = db.PSXN_KetQuas.FirstOrDefault(p => p.MaTiepNhan == maTiepNhan && p.isXoa == false).MaXetNghiem.ToString();
                if (res == null) return string.Empty;
                else return res;
            }
            catch
            {
                return string.Empty;
            }
        }
        public string GetDuLieuBangGhi(string MaDuLieu)
        {
            try
            {
                var res = db.BangGhiDuLieus.FirstOrDefault(p => p.MaDuLieu == MaDuLieu);
                if (res != null)
                    return res.DuLieu;
                else return string.Empty;
            }
            catch { return string.Empty; }
        }
        public PsReponse UpdateDuLieuBangGhi(string MaBatDau)
        {
            PsReponse res = new PsReponse();
            try
            {
                var data = db.BangGhiDuLieus.OrderByDescending(x => x.MaDuLieu).FirstOrDefault();

                if (data != null)
                {
                    data.DuLieu = MaBatDau;
                    db.SubmitChanges();
                    res.Result = true;
                }
                else
                {
                    BangGhiDuLieu N = new BangGhiDuLieu();

                    var dat = GetDateTimeServer();
                    string s1 = (dat.Year.ToString()).Trim().Substring(DateTime.Now.Year.ToString().Trim().ToString().Length - 2);
                    string s2 = data.DuLieu.Substring(6, 2);
                    N.MaDuLieu = "MAXN" + s1 + s2;
                    N.DuLieu = MaBatDau;
                    db.BangGhiDuLieus.InsertOnSubmit(N);
                    db.SubmitChanges();
                    res.Result = true;
                }
            }
            catch { res.Result = false; }
            return res;
        }

        public string GetDuLieuXNBangGhi()
        {
            try
            {
                var res = db.BangGhiDuLieus.OrderByDescending(x => x.MaDuLieu).FirstOrDefault();
                if (res != null)
                    return res.DuLieu;
                else return string.Empty;
            }
            catch { return string.Empty; }
        }
        public PsReponse KTMaXN(string MaXN)
        {
            PsReponse res = new PsReponse();
            try
            {
                var kq = db.PSXN_KetQuas.Where(x => x.MaXetNghiem == MaXN).ToList();
                if (kq.Count > 0)
                {
                    res.Result = false;
                    res.StringError = "Mã Xét Nghiệm Đã Tồn Tại - Yêu Cầu Nhập Mã Khác";
                }
                else
                {
                    res.Result = true;
                }
            }
            catch
            {
                res.Result = false;
            }
            return res;
        }
        public PSTiepNhan GetThongTinTiepNhan(long rowID)
        {
            try
            {
                return db.PSTiepNhans.FirstOrDefault(p => p.RowIDTiepNhan == rowID);
            }
            catch { return null; }
        }
        public PSTiepNhan GetThongTinTiepNhanTheoMaPhieu(string maPhieu)
        {
            try
            {
                return db.PSTiepNhans.FirstOrDefault(p => p.MaPhieu == maPhieu && p.isXoa != true);
            }
            catch { return null; }
        }

        public PsReponse KiemTraThongTinPhieuDaDuocTiepNhan(string maPhieu)
        {
            PsReponse res = new PsReponse();
            try
            {
                var result = db.PSTiepNhans.FirstOrDefault(p => p.MaPhieu == maPhieu);
                var result2 = db.PSPhieuSangLocs.FirstOrDefault(p => p.IDPhieu == maPhieu && p.isXoa == true);
                if (result == null && result2 == null)
                {
                    res.Result = true;

                }
                else
                {
                    res.Result = false;
                    if (result != null)
                    {
                        if (result.isXoa == true)
                        {
                            res.StringError = "Mã phiếu không thể tiếp nhận do mã phiếu đã bị hủy và chưa đồng bộ xóa trên hệ thống";
                        }
                        else
                        {
                            res.StringError = "Phiếu này đã được nhập rồi,vui lòng chọn phiếu mới";
                        }
                    }
                    else
                    {

                        res.StringError = "Mã phiếu không thể tiếp nhận do mã phiếu đã bị hủy và chưa đồng bộ xóa trên hệ thống";
                    }

                }

            }
            catch (Exception ex)
            {
                res.Result = false;
                res.StringError = "Tiếp nhận phiếu bị lỗi" + ex;
            }
            return res;
        }
        public PSChiDinhDichVu GetThongTinChiDinh(string maPhieu, string maTiepNhan)
        {
            try
            {
                if (!string.IsNullOrEmpty(maPhieu) && !string.IsNullOrEmpty(maTiepNhan))
                {
                    var result = db.PSChiDinhDichVus.FirstOrDefault(p => p.MaPhieu == maPhieu && p.MaTiepNhan == maTiepNhan && p.isXoa == false);
                    if (result != null) return result;
                    else return null;
                }
                else return null;
            }
            catch { return null; }
        }
        public PSDanhMucDonViCoSo GetThongTinDonViCoSo(string maDovi)
        {
            PSDanhMucDonViCoSo donvi = db.PSDanhMucDonViCoSos.FirstOrDefault(p => p.MaDVCS == maDovi);
            return donvi;
        }
        public string GetCTrinh(string MaCTrinh)
        {
            string tenCT = db.PSDanhMucChuongTrinhs.FirstOrDefault(p => p.IDChuongTrinh == MaCTrinh).TenChuongTrinh;
            return tenCT;
        }
        public string GetGoiXN(string MaGoiXN)
        {
            string TenGoiXN = db.PSDanhMucGoiDichVuChungs.FirstOrDefault(p => p.IDGoiDichVuChung == MaGoiXN).TenGoiDichVuChung;
            return TenGoiXN;
        }

        public string GetThongTinMailDonViCoSo(string maDovi)
        {
            PSDanhMucDonViCoSo donvi = db.PSDanhMucDonViCoSos.FirstOrDefault(p => p.MaDVCS == maDovi);
            return donvi.Email;
        }
        public List<PSChiDinhDichVuChiTiet> GetThongTinChiDinhDichVuChiTiet(string maCD)
        {
            List<PSChiDinhDichVuChiTiet> lst = new List<PSChiDinhDichVuChiTiet>();
            try
            {
                if (string.IsNullOrEmpty(maCD))
                    return lst;
                else
                {
                    var res = db.PSChiDinhDichVuChiTiets.Where(p => p.MaChiDinh == maCD).ToList();
                    if (res.Count > 0)
                    {
                        return res;
                    }
                    else return lst;
                }
            }
            catch { return lst; }
        }
        public PSChiDinhDichVu GetThongTinChiDinh(string maCD)
        {
            try
            {
                if (!string.IsNullOrEmpty(maCD))
                {
                    var result = db.PSChiDinhDichVus.FirstOrDefault(p => p.MaChiDinh == maCD && p.isXoa == false);
                    if (result != null) return result;
                    else return null;
                }
                else return null;
            }
            catch { return null; }
        }
        public List<PSChiDinhDichVu> GetDanhSachChuaCapMaXN(int trangthai)
        {
            List<PSChiDinhDichVu> lst = new List<PSChiDinhDichVu>();
            try
            {
                lst = db.PSChiDinhDichVus.Where(p => p.isXoa != true && p.TrangThai == trangthai).OrderBy(x => x.MaPhieu).ToList();
            }
            catch
            {
            }
            return lst;
        }
        public List<PSXN_KetQua> GetDanhSachChuaGanViTriXN(int trangthai)
        {
            List<PSXN_KetQua> lst = new List<PSXN_KetQua>();
            try
            {
                lst = db.PSXN_KetQuas.Where(p => p.isXoa != true && p.isCoKQ != true).OrderBy(x => x.MaPhieu).ToList();
            }
            catch
            {
            }
            return lst;
        }
        public List<PSXN_KetQua> GetDSPhongXN(bool isDaCoKQ)
        {
            List<PSXN_KetQua> lst = new List<PSXN_KetQua>();
            try
            {
                lst = db.PSXN_KetQuas.Where(p => p.isXoa != true && p.isCoKQ == isDaCoKQ).OrderBy(x => x.MaPhieu).ToList();
            }
            catch
            {
            }
            return lst;
        }
        public List<PSGanViTriXNKQ> GetDSChuaGanXN()
        {
            List<PSGanViTriXNKQ> lst = new List<PSGanViTriXNKQ>();
            try
            {
                var kq = db.PSXN_KetQuas.Where(p => p.isXoa != true && p.isCoKQ != true).OrderBy(x => x.MaPhieu).ToList();
                // var gan = (from ls in db.PSCM_GanViTris
                // join kq in db.PSXN_KetQuas on ls.MaXetNghiem equals kq.MaXetNghiem
                //  where ls.MaPhieu == kq.MaPhieu && kq.isCoKQ != true && kq.MaGoiXN == ls.MaGoiXN && ls.isDaDuyet!=true
                //  select new { kq }).ToList();

                foreach (var c in kq)
                {
                    PSGanViTriXNKQ gan = new PSGanViTriXNKQ();
                    gan.RowIDXN_TraKetQua = c.RowIDKetQua;
                    gan.MaDonVi = c.MaDonVi;
                    gan.MaChiDinh = c.MaChiDinh;
                    gan.MaGoiXN = c.MaGoiXN;
                    gan.MaKetQua = c.MaKetQua;
                    gan.MaPhieu = c.MaPhieu;
                    gan.MaXetNghiem = c.MaXetNghiem;
                    gan.NgayChiDinh = c.NgayChiDinh;
                    gan.NgayTiepNhan = c.NgayTiepNhan;
                    var duyet = db.PSCM_GanViTris.FirstOrDefault(x => x.MaPhieu.Equals(gan.MaPhieu) && x.MaGoiXN.Equals(gan.MaGoiXN) && x.MaXetNghiem.Equals(gan.MaXetNghiem));
                    if (duyet != null)
                    {
                        gan.isDaDuyet = Boolean.Parse(duyet.isDaDuyet.ToString());
                    }

                    lst.Add(gan);
                }
            }
            catch
            {
            }
            return lst;
        }
        public PSCMGanViTriChung GetPhieuChuaCoKQ(string maphieu)
        {
            PSCMGanViTriChung vt = new PSCMGanViTriChung();
            try
            {
                PSXN_KetQua kq = db.PSXN_KetQuas.FirstOrDefault(p => p.isXoa != true && p.isCoKQ != true && p.MaPhieu == maphieu);
                if (kq == null)
                {

                }
                else
                {
                    vt.MaPhieu = kq.MaPhieu;
                    vt.MaGoiXN = kq.MaGoiXN;
                    vt.MaXetNghiem = kq.MaXetNghiem;

                    var idmay = (from ct in kq.PSXN_KetQua_ChiTiets
                                 join dv in db.PSMapsMayXN_DichVus on ct.MaDichVu equals dv.IDDichVu
                                 join may in db.PSDanhMucMayXNs on dv.IDMayXN equals may.IDMayXN
                                 select new { may }).ToList().Distinct();
                    List<PSDanhMucMayXN> m = new List<PSDanhMucMayXN>();
                    foreach (var mayct in idmay)
                    {
                        m.Add(mayct.may);
                    }
                    vt.may = m;

                }
            }
            catch
            {

            }
            return vt;

        }
        public string GetMaRowIDLanGanXN()
        {
            string ma = string.Empty;
            try
            {
                if (db.PSCM_GanViTris.ToList().Count > 0)
                {
                    var makq = (from vt in db.PSCM_GanViTris
                                where vt.isDaDuyet != true
                                select new { vt.IDLanGanXN }).OrderByDescending(x => x.IDLanGanXN).FirstOrDefault();
                    if (makq != null)
                    {
                        ma = makq.IDLanGanXN.ToString();
                    }
                    else
                    {
                        var madcc = (from vt in db.PSCM_GanViTris
                                     where vt.isDaDuyet == true
                                     select new { vt.IDLanGanXN }).OrderByDescending(x => x.IDLanGanXN).FirstOrDefault();
                        if (madcc == null)
                        {
                            ma = "1";
                        }
                        else
                        {
                            ma = (int.Parse(madcc.IDLanGanXN.ToString()) + 1).ToString();
                        }
                    }
                }
                else
                {
                    ma = "1";
                }

            }
            catch
            {
                ma = "1";
            }
            return ma;

        }
        public PsReponse SaveGanXN(PSCMGanViTriChung pm)
        {
            PsReponse reponse = new PsReponse();
            PSCM_GanViTri gan = new PSCM_GanViTri();
            gan.IDLanGanXN = pm.IDLanGanXN;
            gan.isDaDuyet = false;
            gan.MaGoiXN = pm.MaGoiXN;
            gan.MaXetNghiem = pm.MaXetNghiem;
            gan.STT_bang = pm.STT_bang;
            gan.GhiChuChung = pm.GhiChuChung;
            gan.MaPhieu = pm.MaPhieu;
            db.PSCM_GanViTris.InsertOnSubmit(gan);
            db.SubmitChanges();
            //PSCM_GanViTri viTri = db.PSCM_GanViTris.FirstOrDefault(x => x.MaPhieu == pm.MaPhieu && x.MaXetNghiem == pm.MaXetNghiem && x.IDLanGanXN==pm.IDLanGanXN
            //&& x.STT_bang == pm.STT_bang && x.MaGoiXN == pm.MaGoiXN && x.isDaDuyet != true);
            PSCM_GanViTri viTri = db.PSCM_GanViTris.FirstOrDefault(x => x.IDLanGanXN.Equals(pm.IDLanGanXN)
            && x.STT_bang == pm.STT_bang && x.isDaDuyet != true);
            if (viTri != null)
            {
                if (pm.MAYXN01 != null)
                {
                    PSCM_GanViTriCT ganct = new PSCM_GanViTriCT();
                    ganct.IDLanGanXN = viTri.IDLanGanXN;
                    ganct.IDRowGanXN = viTri.IDRowGanXN;
                    ganct.isDaDuyet = false;
                    ganct.IDMayXN = "MAYXN01";
                    ganct.STT = pm.MAYXN01.STT;
                    ganct.STTDia = pm.MAYXN01.STTDia;
                    ganct.STTVT = pm.MAYXN01.STTVT;
                    ganct.isTest = pm.MAYXN01.isTest;
                    ganct.GhiChuCT = pm.MAYXN01.GhiChuCT;
                    ganct.ViTri = pm.MAYXN01.ViTri;
                    db.PSCM_GanViTriCTs.InsertOnSubmit(ganct);
                    db.SubmitChanges();
                }
                if (pm.MAYXN02 != null)
                {
                    PSCM_GanViTriCT ganct = new PSCM_GanViTriCT();
                    ganct.IDLanGanXN = viTri.IDLanGanXN;
                    ganct.IDRowGanXN = viTri.IDRowGanXN;
                    ganct.isDaDuyet = false;
                    ganct.IDMayXN = "MAYXN02";
                    ganct.STT = pm.MAYXN02.STT;
                    ganct.STTDia = pm.MAYXN02.STTDia;
                    ganct.STTVT = pm.MAYXN02.STTVT;
                    ganct.isTest = pm.MAYXN02.isTest;
                    ganct.GhiChuCT = pm.MAYXN02.GhiChuCT;
                    ganct.ViTri = pm.MAYXN02.ViTri;
                    db.PSCM_GanViTriCTs.InsertOnSubmit(ganct);
                    db.SubmitChanges();
                }
            }
            return reponse;
        }
        public PsReponse SuaDanhSachGanXNLuu(PSCMGanViTriChung pm)
        {
            PsReponse gvc = new PsReponse();
            try
            {
                PSCM_GanViTri viTri = db.PSCM_GanViTris.FirstOrDefault(x => x.IDLanGanXN.Equals(pm.IDLanGanXN) && x.IDRowGanXN == pm.IDRowGanXN
                && x.isDaDuyet != true);
                if (viTri != null)
                {
                    viTri.MaGoiXN = pm.MaGoiXN;
                    viTri.MaPhieu = pm.MaPhieu;
                    viTri.MaXetNghiem = pm.MaXetNghiem;
                    viTri.STT_bang = pm.STT_bang;
                    viTri.isDaDuyet = false;
                    viTri.GhiChuChung = pm.GhiChuChung;
                    if (viTri.PSCM_GanViTriCTs.Count() > 0)
                    {
                        foreach (var vtc in viTri.PSCM_GanViTriCTs)
                        {
                            switch (vtc.IDMayXN)
                            {
                                case "MAYXN01":
                                    {
                                        int co = db.PSCM_GanViTriCTs.Where(x => x.IDLanGanXN.Equals(pm.IDLanGanXN) && x.isDaDuyet != true && x.IDMayXN.Equals("MAYXN01")).ToList().Count();
                                        if (pm.MAYXN01.isTest == true && pm.MAYXN01.STT >= co)
                                        {
                                            db.PSCM_GanViTriCTs.DeleteOnSubmit(vtc);
                                            db.PSCM_GanViTris.DeleteOnSubmit(viTri);
                                            var ganvt = db.PSCM_GanViTris.Where(x => x.STT_bang >= viTri.STT_bang).ToList();
                                            foreach (var g in ganvt)
                                            {
                                                g.STT_bang = g.STT_bang - 1;
                                            }
                                        }
                                        else
                                        {
                                            vtc.GhiChuCT = pm.MAYXN01.GhiChuCT;
                                            vtc.isTest = pm.MAYXN01.isTest;
                                            vtc.STT = pm.MAYXN01.STT;
                                            vtc.STTDia = pm.MAYXN01.STTDia;
                                            vtc.STTVT = pm.MAYXN01.STTVT;
                                            vtc.ViTri = pm.MAYXN01.ViTri;
                                            db.SubmitChanges();
                                        }
                                        break;
                                    }
                                case "MAYXN02":
                                    {
                                        int co = db.PSCM_GanViTriCTs.Where(x => x.IDLanGanXN.Equals(pm.IDLanGanXN) && x.isDaDuyet != true && x.IDMayXN.Equals("MAYXN02")).ToList().Count();
                                        if (pm.MAYXN02.isTest == true && pm.MAYXN02.STT >= co)
                                        {
                                            db.PSCM_GanViTriCTs.DeleteOnSubmit(vtc);
                                            db.PSCM_GanViTris.DeleteOnSubmit(viTri);
                                            var ganvt = db.PSCM_GanViTris.Where(x => x.STT_bang >= viTri.STT_bang).ToList();
                                            foreach (var g in ganvt)
                                            {
                                                g.STT_bang = g.STT_bang - 1;
                                            }
                                        }
                                        else
                                        {
                                            vtc.GhiChuCT = pm.MAYXN02.GhiChuCT;
                                            vtc.isTest = pm.MAYXN02.isTest;
                                            vtc.STT = pm.MAYXN02.STT;
                                            vtc.STTDia = pm.MAYXN02.STTDia;
                                            vtc.STTVT = pm.MAYXN02.STTVT;
                                            vtc.ViTri = pm.MAYXN02.ViTri;
                                            db.SubmitChanges();
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                    db.SubmitChanges();
                    gvc.Result = true;
                }
            }
            catch (Exception ex)
            {
                gvc.Result = false;
                gvc.StringError = ex.ToString();
            }
            return gvc;
        }
        public bool GetMaPhieuDaCoDSCapVT(string MaPhieu)
        {
            try
            {
                PSCM_GanViTri viTri = db.PSCM_GanViTris.FirstOrDefault(x => x.MaPhieu.Equals(MaPhieu)
               && x.isDaDuyet != true);
                if (viTri == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public PsReponse DeleteDanhSachGanXNLuu(PSCMGanViTriChung pm)
        {
            PsReponse gvc = new PsReponse();
            try
            {

                PSCM_GanViTri viTri = db.PSCM_GanViTris.FirstOrDefault(x => x.IDLanGanXN.Equals(pm.IDLanGanXN) && x.IDRowGanXN == pm.IDRowGanXN
                && x.isDaDuyet != true);
                var ds = db.PSCM_GanViTris.Where(x => x.isDaDuyet != true && x.IDLanGanXN.Equals(pm.IDLanGanXN)).ToList();
                if (viTri != null && ds.Count > 0)
                {
                    long? gt = viTri.STT_bang;
                    if (ds.Count <= gt)
                    {
                        if (viTri.PSCM_GanViTriCTs.Count() > 0)
                        {
                            foreach (var vtc in viTri.PSCM_GanViTriCTs)
                            {
                                db.PSCM_GanViTriCTs.DeleteOnSubmit(vtc);
                            }

                        }
                        db.PSCM_GanViTris.DeleteOnSubmit(viTri);
                        db.SubmitChanges();
                    }
                    else
                    {
                        viTri.MaGoiXN = null;
                        viTri.MaPhieu = null;
                        viTri.MaXetNghiem = null;
                        viTri.GhiChuChung = null;
                        if (viTri.PSCM_GanViTriCTs.Count > 0)
                        {
                            foreach (var c in viTri.PSCM_GanViTriCTs)
                            {
                                if (pm.may.Count == 1)
                                {
                                    if (pm.MAYXN01 != null)
                                    {
                                        var ct = db.PSCM_GanViTriCTs.Where(x => x.isDaDuyet != true && x.IDLanGanXN.Equals(pm.IDLanGanXN) && x.IDMayXN.Equals("MAYXN01")).ToList();
                                        if (ct.Count() <= c.STT)
                                        {
                                            db.PSCM_GanViTriCTs.DeleteOnSubmit(c);
                                            db.PSCM_GanViTris.DeleteOnSubmit(viTri);
                                            db.SubmitChanges();
                                            break;
                                        }
                                    }
                                    else if (pm.MAYXN02 != null)
                                    {
                                        var ct = db.PSCM_GanViTriCTs.Where(x => x.isDaDuyet != true && x.IDLanGanXN.Equals(pm.IDLanGanXN) && x.IDMayXN.Equals("MAYXN02")).ToList();
                                        if (ct.Count() <= c.STT)
                                        {
                                            db.PSCM_GanViTriCTs.DeleteOnSubmit(c);
                                            db.PSCM_GanViTris.DeleteOnSubmit(viTri);
                                            db.SubmitChanges();
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    c.GhiChuCT = null;
                                    db.SubmitChanges();
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gvc.Result = false;
                gvc.StringError = ex.ToString();
            }
            return gvc;
        }
        public PsReponse DeleteCTGanXNLuu(PSCM_GanViTriCT gan)
        {
            PsReponse rep = new PsReponse();
            try
            {
                var kq = db.PSCM_GanViTriCTs.FirstOrDefault(x => x.IDLanGanXN == gan.IDLanGanXN && x.IDMayXN == gan.IDMayXN && x.STTDia == gan.STTDia && x.STTVT == gan.STTVT);
                if (kq != null)
                {
                    db.PSCM_GanViTriCTs.DeleteOnSubmit(kq);
                    db.SubmitChanges();
                }
            }
            catch
            {

            }
            return rep;
        }
        public PsReponse DeleteDanhSachAllGanXNLuu(List<PSCMGanViTriChung> pm)
        {
            PsReponse gvc = new PsReponse();
            try
            {
                foreach (var gc in pm)
                {
                    PSCM_GanViTri viTri = db.PSCM_GanViTris.FirstOrDefault(x => x.IDLanGanXN.Equals(gc.IDLanGanXN) && x.IDRowGanXN == gc.IDRowGanXN
                && x.isDaDuyet != true);
                    var ds = db.PSCM_GanViTris.Where(x => x.isDaDuyet != true && x.IDLanGanXN.Equals(gc.IDLanGanXN)).ToList();
                    if (viTri != null)
                    {
                        if (viTri.PSCM_GanViTriCTs.Count() > 0)
                        {
                            foreach (var vtc in viTri.PSCM_GanViTriCTs)
                            {
                                db.PSCM_GanViTriCTs.DeleteOnSubmit(vtc);
                            }
                        }
                        db.PSCM_GanViTris.DeleteOnSubmit(viTri);
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                gvc.Result = false;
                gvc.StringError = ex.ToString();
            }
            return gvc;
        }
        public PsReponse DuyetCapMaGanMayXN(List<PSCMGanViTriChung> gvc)
        {
            PsReponse reponse = new PsReponse();
            try
            {
                foreach (var gv in gvc)
                {
                    var cm = db.PSCM_GanViTris.FirstOrDefault(x => x.IDLanGanXN == gv.IDLanGanXN && x.IDRowGanXN == gv.IDRowGanXN && x.isDaDuyet != true);
                    if (cm != null)
                    {
                        cm.isDaDuyet = true;
                        var kq = db.PSXN_KetQuas.FirstOrDefault(x => x.MaPhieu == cm.MaPhieu && x.MaXetNghiem == cm.MaXetNghiem && x.isCoKQ != true && x.isXoa != true);
                        if (kq != null)
                        {
                            string viettatchung = GetViettatGhiChuXN(cm.GhiChuChung, "MaChung");

                            foreach (var cmct in cm.PSCM_GanViTriCTs)
                            {
                                string viettatMay1 = GetViettatGhiChuXN(cmct.GhiChuCT, cmct.IDMayXN);
                                if (!string.IsNullOrEmpty(viettatMay1))
                                {
                                    viettatchung = viettatchung + "\n" + viettatMay1;
                                }
                            }
                            kq.GhiChu = viettatchung;
                            db.SubmitChanges();
                        }
                        db.SubmitChanges();
                    }
                }
            }
            catch
            {

            }
            return reponse;
        }
        public String GetViettatGhiChuXN(string viettat, string MaGC)
        {
            string noidung = string.Empty;
            if (!string.IsNullOrEmpty(viettat))
            {
                try
                {
                    List<string> gcchung = viettat.Split('-').ToList();
                    if (gcchung.Count > 0)
                    {
                        foreach (var gcc in gcchung)
                        {
                            var dmgc = db.PSDanhMucGhiChuXNs.FirstOrDefault(x => x.VietTatGhiChu.Equals(gcc.Trim().ToUpper()) && x.isSuDung != false);
                            string gc1 = string.Empty;
                            if (dmgc != null)
                            {
                                gc1 = dmgc.NoiDungGhiChuTruoc;
                            }
                            if (string.IsNullOrEmpty(gc1))
                            {
                                gc1 = gcc;
                            }
                            if (string.IsNullOrEmpty(noidung))
                            {
                                switch (MaGC)
                                {
                                    case "MaChung":
                                        {
                                            noidung = "+ Ghi chú chung: " + gc1 + " ";
                                            break;
                                        }
                                    case "MAYXN01":
                                        {
                                            noidung = " + Ghi chú máy 3 bệnh: " + gc1 + " ";
                                            break;
                                        }
                                    case "MAYXN02":
                                        {
                                            noidung = " + Ghi chú máy 2 bệnh: " + gc1 + " ";
                                            break;
                                        }
                                }

                            }
                            else
                            {
                                noidung = noidung + gc1 + " ";
                            }
                        }
                    }
                    else
                    {
                        switch (MaGC)
                        {
                            case "MaChung":
                                {
                                    noidung = "+ Ghi chú chung: " + viettat + " ";
                                    break;
                                }
                            case "MAYXN01":
                                {
                                    noidung = " + Ghi chú máy 3 bệnh: " + viettat + " ";
                                    break;
                                }
                            case "MAYXN02":
                                {
                                    noidung = " + Ghi chú máy 2 bệnh: " + viettat + " ";
                                    break;
                                }
                        }
                    }
                }
                catch
                {
                    switch (MaGC)
                    {
                        case "MaChung":
                            {
                                noidung = "+ Ghi chú chung: " + viettat + " ";
                                break;
                            }
                        case "MAYXN01":
                            {
                                noidung = " + Ghi chú máy 3 bệnh: " + viettat + " ";
                                break;
                            }
                        case "MAYXN02":
                            {
                                noidung = " + Ghi chú máy 2 bệnh: " + viettat + " ";
                                break;
                            }
                    }
                }
            }
            else
            {

            }
            return noidung;
        }
        public List<PSDanhMucGhiChuXN> GetDanhMucGhiXN()
        {
            List<PSDanhMucGhiChuXN> dm = new List<PSDanhMucGhiChuXN>();
            dm = db.PSDanhMucGhiChuXNs.ToList();
            return dm;
        }
        public Boolean UpdateDanhMucGC(PSDanhMucGhiChuXN ghichu)
        {
            PsReponse reponse = new PsReponse();
            try
            {
                PSDanhMucGhiChuXN gc = db.PSDanhMucGhiChuXNs.FirstOrDefault(x => x.IDRowGhiChuXN == ghichu.IDRowGhiChuXN);

                if (gc != null)
                {
                    gc.isSuDung = ghichu.isSuDung;
                    gc.NoiDungGhiChuTruoc = ghichu.NoiDungGhiChuTruoc;
                    gc.VietTatGhiChu = ghichu.VietTatGhiChu;
                    db.SubmitChanges();
                    reponse.Result = true;
                }
                else
                {
                    reponse.Result = false;
                }
            }
            catch
            {
                reponse.Result = false;
            }
            return reponse.Result;
        }
        public Boolean AddNewDanhMucGC(PSDanhMucGhiChuXN ghichu)
        {
            PsReponse reponse = new PsReponse();
            try
            {
                db.PSDanhMucGhiChuXNs.InsertOnSubmit(ghichu);
                db.SubmitChanges();
                reponse.Result = true;
            }
            catch
            {
                reponse.Result = false;
            }
            return reponse.Result;
        }
        public Boolean DeleteDanhMucGC(PSDanhMucGhiChuXN ghichu)
        {
            PsReponse reponse = new PsReponse();
            try
            {
                db.PSDanhMucGhiChuXNs.DeleteOnSubmit(ghichu);
                db.SubmitChanges();
                reponse.Result = true;
            }
            catch
            {
                reponse.Result = false;
            }
            return reponse.Result;
        }
        public List<PSCMGanViTriChung> GetDanhSachGanXNLuu()
        {
            List<PSCMGanViTriChung> gvc = new List<PSCMGanViTriChung>();
            try
            {
                List<PSCM_GanViTri> lst = db.PSCM_GanViTris.Where(x => x.isDaDuyet != true).OrderBy(x => x.STT_bang).ToList();
                foreach (var ls in lst)
                {
                    PSCMGanViTriChung gv = new PSCMGanViTriChung();
                    gv.IDLanGanXN = ls.IDLanGanXN;
                    gv.MaGoiXN = ls.MaGoiXN;
                    gv.MaPhieu = ls.MaPhieu;
                    gv.MaXetNghiem = ls.MaXetNghiem;
                    gv.GhiChuChung = ls.GhiChuChung;
                    gv.IDRowGanXN = ls.IDRowGanXN;
                    gv.STT_bang = ls.STT_bang;

                    if (ls.PSCM_GanViTriCTs.Count() > 0)
                    {
                        gv.may = new List<PSDanhMucMayXN>();
                        foreach (var ctg in ls.PSCM_GanViTriCTs)
                        {
                            switch (ctg.IDMayXN)
                            {
                                case "MAYXN01":
                                    {
                                        ViTriXN vt = new ViTriXN();
                                        vt.GhiChuCT = ctg.GhiChuCT;
                                        vt.isTest = ctg.isTest;
                                        vt.STT = ctg.STT;
                                        vt.STTDia = ctg.STTDia;
                                        vt.STTVT = ctg.STTVT;
                                        vt.ViTri = ctg.ViTri;
                                        gv.MAYXN01 = vt;
                                        PSDanhMucMayXN mayxn = new PSDanhMucMayXN();
                                        mayxn.IDMayXN = ctg.IDMayXN;
                                        mayxn.isSuDung = true;
                                        mayxn.TenMayXN = "Máy 3 bệnh";
                                        gv.may.Add(mayxn);
                                        break;
                                    }
                                case "MAYXN02":
                                    {
                                        ViTriXN vt = new ViTriXN();
                                        vt.GhiChuCT = ctg.GhiChuCT;
                                        vt.isTest = ctg.isTest;
                                        vt.STT = ctg.STT;
                                        vt.STTDia = ctg.STTDia;
                                        vt.STTVT = ctg.STTVT;
                                        vt.ViTri = ctg.ViTri;
                                        gv.MAYXN02 = vt;
                                        PSDanhMucMayXN mayxn = new PSDanhMucMayXN();
                                        mayxn.IDMayXN = ctg.IDMayXN;
                                        mayxn.isSuDung = true;
                                        mayxn.TenMayXN = "Máy 2 bệnh";
                                        gv.may.Add(mayxn);
                                        break;
                                    }
                            }
                        }
                    }
                    gvc.Add(gv);
                }
            }
            catch
            { }
            return gvc;
        }

        public List<PSDanhMucMayXN> GetDSMayXN()
        {
            List<PSDanhMucMayXN> xn = db.PSDanhMucMayXNs.Where(x => x.isSuDung == true).ToList();
            return xn;
        }
        public List<PSMapsViTriMayXN> GetDSMapViTriMayXN(string IDMayXN)
        {
            List<PSMapsViTriMayXN> xn = db.PSMapsViTriMayXNs.Where(x => x.IDMayXN == IDMayXN).ToList();
            return xn;
        }
        public PsReponse SaveDSMapViTri(PSCMGanViTriChung gvt)
        {
            PsReponse reponse = new PsReponse();
            try
            {
                PSCM_GanViTri gan = db.PSCM_GanViTris.FirstOrDefault(x => x.MaGoiXN == gvt.MaGoiXN && x.MaPhieu == gvt.MaPhieu
                && x.MaXetNghiem == gvt.MaXetNghiem && x.IDLanGanXN == gvt.IDLanGanXN && x.STT_bang == gvt.STT_bang);
            }
            catch
            {

            }
            return reponse;
        }
        public List<PSChiDinhDichVu> GetDanhSachDichVuDaChiDinh(int Trangthai, string maDonVi, DateTime tuNgay, DateTime denNgay, bool isCanCapMa)
        {
            if (!isCanCapMa)
            {
                if (Trangthai < 0)//get tất cả
                {

                    if (string.IsNullOrEmpty(maDonVi))
                    {
                        var results = db.PSChiDinhDichVus.OrderBy(p => p.MaPhieu).Where(p => p.isXoa == false && p.IDGoiDichVu != "DVGXNL2" && p.NgayChiDinhLamViec.Value.Date <= denNgay && p.NgayChiDinhLamViec.Value.Date >= tuNgay.Date).ToList();
                        if (results.Count > 0) return results;
                        else return null;
                    }
                    else
                    {
                        var results = db.PSChiDinhDichVus.OrderBy(p => p.MaPhieu).Where(p => p.isXoa == false && p.IDGoiDichVu != "DVGXNL2" && p.NgayChiDinhLamViec.Value.Date <= denNgay && p.NgayChiDinhLamViec.Value.Date >= tuNgay.Date && p.MaDonVi.Contains(maDonVi)).ToList();
                        if (results.Count > 0) return results;
                        else return null;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(maDonVi))
                    {
                        var results = db.PSChiDinhDichVus.OrderBy(p => p.MaPhieu).Where(p => p.isXoa == false && p.IDGoiDichVu != "DVGXNL2" && p.TrangThai == Trangthai && p.NgayChiDinhLamViec.Value.Date <= denNgay && p.NgayChiDinhLamViec.Value.Date >= tuNgay.Date).ToList();
                        if (results.Count > 0) return results;
                        else return null;
                    }
                    else
                    {
                        var results = db.PSChiDinhDichVus.OrderBy(p => p.MaPhieu).Where(p => p.isXoa == false && p.IDGoiDichVu != "DVGXNL2" && p.TrangThai == Trangthai && p.NgayChiDinhLamViec.Value.Date <= denNgay && p.NgayChiDinhLamViec.Value.Date >= tuNgay.Date && p.MaDonVi.Contains(maDonVi)).ToList();
                        if (results.Count > 0) return results;
                        else return null;
                    }
                }
            }
            else
            {
                if (Trangthai < 0)//get tất cả
                {
                    if (string.IsNullOrEmpty(maDonVi))
                    {
                        var results = db.PSChiDinhDichVus.OrderBy(p => p.MaPhieu).Where(p => p.isXoa == false && p.NgayChiDinhLamViec.Value.Date <= denNgay && p.NgayChiDinhLamViec.Value.Date >= tuNgay.Date).ToList();
                        if (results.Count > 0) return results;
                        else return null;
                    }
                    else
                    {
                        var results = db.PSChiDinhDichVus.OrderBy(p => p.MaPhieu).Where(p => p.isXoa == false && p.NgayChiDinhLamViec.Value.Date <= denNgay && p.NgayChiDinhLamViec.Value.Date >= tuNgay.Date && p.MaDonVi.Contains(maDonVi)).ToList();
                        if (results.Count > 0) return results;
                        else return null;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(maDonVi))
                    {
                        var results = db.PSChiDinhDichVus.OrderBy(p => p.MaPhieu).Where(p => p.isXoa == false && p.TrangThai == Trangthai && p.NgayChiDinhLamViec.Value.Date <= denNgay && p.NgayChiDinhLamViec.Value.Date >= tuNgay.Date).ToList();
                        if (results.Count > 0) return results;
                        else return null;
                    }
                    else
                    {
                        var results = db.PSChiDinhDichVus.OrderBy(p => p.MaPhieu).Where(p => p.isXoa == false && p.TrangThai == Trangthai && p.NgayChiDinhLamViec.Value.Date <= denNgay && p.NgayChiDinhLamViec.Value.Date >= tuNgay.Date && p.MaDonVi.Contains(maDonVi)).ToList();
                        if (results.Count > 0) return results;
                        else return null;
                    }
                }
            }
        }

        public List<PSDanhMucDanhGiaChatLuongMau> GetDanhMucDanhGiaChatLuongMau()
        {
            var lst = db.PSDanhMucDanhGiaChatLuongMaus.OrderBy(x => x.STT).ToList();
            if (lst.Count > 0)
            {
                return lst;
            }
            else return null;
        }
        public List<PSDanhMucDonViCoSo> GetDanhSachDonVi(string maChiCuc)
        {
            List<PSDanhMucDonViCoSo> lst = new List<PSDanhMucDonViCoSo>();
            try
            {
                if (!string.IsNullOrEmpty(maChiCuc) & !maChiCuc.Equals("0") & !maChiCuc.ToLower().Equals("all"))
                {
                    var results = db.PSDanhMucDonViCoSos.Where(p => p.isLocked == false && p.MaChiCuc == maChiCuc).ToList();
                    if (results.Count > 0) return results;
                }
                else
                {
                    var results = db.PSDanhMucDonViCoSos.Where(p => p.isLocked == false).ToList();
                    if (results.Count > 0) return results;
                }
            }

            catch (Exception ex) { }
            return lst;
        }
        public List<PSDanhMucDonViCoSo> GetDanhSachDonViSMS(string maChiCuc)
        {
            List<PSDanhMucDonViCoSo> lst = new List<PSDanhMucDonViCoSo>();
            try
            {
                if (!string.IsNullOrEmpty(maChiCuc) & !maChiCuc.Equals("0") & !maChiCuc.ToLower().Equals("all"))
                {
                    var results = db.PSDanhMucDonViCoSos.Where(p => p.isLocked == false && p.MaChiCuc == maChiCuc && p.isChoPhepGuiSMS == true).ToList();
                    if (results.Count > 0) return results;
                }
                else
                {
                    var results = db.PSDanhMucDonViCoSos.Where(p => p.isLocked == false && p.isChoPhepGuiSMS == true).ToList();
                    if (results.Count > 0) return results;
                }
            }

            catch (Exception ex) { }
            return lst;
        }
        public List<PSDanhMucDonViCoSo> GetDanhSachDonVi()
        {
            List<PSDanhMucDonViCoSo> lst = new List<PSDanhMucDonViCoSo>();
            try
            {
                var results = db.PSDanhMucDonViCoSos.Where(p => p.isLocked == false).ToList();
                PSDanhMucDonViCoSo dv = new PSDanhMucDonViCoSo();
                dv.MaDVCS = "ALL";
                dv.TenDVCS = "Tất cả";
                lst.Add(dv);
                foreach (var donvi in results)
                {
                    lst.Add(donvi);
                }
            }

            catch (Exception ex) { }
            return lst;
        }
        public List<PSDanhMucDonViCoSo> GetDonViCoSo(string maDonViCoSo)
        {
            List<PSDanhMucDonViCoSo> lst = new List<PSDanhMucDonViCoSo>();
            try
            {
                if (!string.IsNullOrEmpty(maDonViCoSo))
                {
                    var result = db.PSDanhMucDonViCoSos.FirstOrDefault(p => p.MaDVCS == maDonViCoSo);
                    if (result != null)
                    {
                        lst.Add(result);
                    }
                }
                else
                {
                    var results = db.PSDanhMucDonViCoSos.OrderBy(p => p.Stt).Where(p => p.isLocked == false).ToList();

                    foreach (var donvi in results)
                    {
                        lst.Add(donvi);
                    }
                }
            }
            catch (Exception ex) { }
            return lst;
        }
        public List<PSPhieuSangLoc> GetPhieuSangLoc(bool tinhTrangPhieu, byte tinhTrangMau)
        {
            List<PSPhieuSangLoc> lst = new List<PSPhieuSangLoc>();
            try
            {
                if (tinhTrangMau >= 0) // Lấy danh sách các phiếu đã đc cơ sở gửi và chọn điều kiện mẫu đã được :0- Chưa nhận,1- đã nhận,2 - Đã đánh giá;3-đang làm XN,4-Đã có kết quả, 5 XN lại ; 6 thu mẫu lại
                {
                    lst = db.PSPhieuSangLocs.OrderBy(p => p.IDPhieu).Where(p => p.TrangThaiPhieu == tinhTrangPhieu && p.TrangThaiMau == tinhTrangMau && p.isXoa != true).ToList();

                }
                else
                {
                    lst = db.PSPhieuSangLocs.OrderBy(p => p.IDPhieu).Where(p => p.TrangThaiPhieu == tinhTrangPhieu && p.isXoa != true).ToList();

                }
            }
            catch (Exception ex) { }
            return lst;
        }


        public List<PSTiepNhan> GetDanhSachPhieuDaTiepNhan(string maDonVi, bool isDaDanhGia)
        {
            List<PSTiepNhan> lst = new List<PSTiepNhan>();
            try
            {
                if (string.IsNullOrEmpty(maDonVi))
                {
                    lst = db.PSTiepNhans.OrderBy(p => p.MaPhieu).Where(p => p.isXoa != true && p.isDaDanhGia == isDaDanhGia).ToList();
                }
                else
                {
                    lst = db.PSTiepNhans.OrderBy(p => p.MaPhieu).Where(p => p.isXoa != true && p.isDaDanhGia == isDaDanhGia && p.MaDonVi.Contains(maDonVi)).ToList();
                }
            }
            catch
            {

            }
            return lst;
        }
        public List<PSChiDinhDichVu> GetDanhSachPhieuDaDuyet(string maDonVi, DateTime tuNgay, DateTime denNgay)
        {
            List<PSChiDinhDichVu> lst = new List<PSChiDinhDichVu>();
            try
            {
                if (string.IsNullOrEmpty(maDonVi))
                {
                    lst = (from cd in db.PSChiDinhDichVus
                           join ph in db.PSPhieuSangLocs on cd.MaPhieu equals ph.IDPhieu
                           where cd.isXoa != true && cd.isLayMauLai != true && cd.IDGoiDichVu != "DVGXNL2" && cd.NgayChiDinhLamViec.Value.Date >= tuNgay.Date && cd.NgayChiDinhLamViec.Value.Date <= denNgay.Date
                           && ph.isXoa != true && (ph.TrangThaiMau < 4 || ph.TrangThaiMau == 5)
                           select cd).ToList();
                }
                else
                {
                    lst = (from cd in db.PSChiDinhDichVus
                           join ph in db.PSPhieuSangLocs on cd.MaPhieu equals ph.IDPhieu
                           where cd.isXoa != true && cd.isLayMauLai != true && cd.IDGoiDichVu != "DVGXNL2" && cd.NgayChiDinhLamViec.Value.Date >= tuNgay.Date && cd.NgayChiDinhLamViec.Value.Date <= denNgay.Date
                           && ph.isXoa != true && cd.MaDonVi == maDonVi && (ph.TrangThaiMau < 4 || ph.TrangThaiMau == 5)
                           select cd).ToList();

                }
            }
            catch
            {

            }
            return lst;
        }

        public List<PSTiepNhan> GetDanhSachPhieuDaTiepNhan(string maDonVi, DateTime tuNgay, DateTime denNgay)
        {
            List<PSTiepNhan> lst = new List<PSTiepNhan>();

            if (string.IsNullOrEmpty(maDonVi))
            {
                var results = db.PSTiepNhans.OrderBy(p => p.MaPhieu).Where(p => p.isXoa == false && p.isDaDanhGia == true && p.NgayTiepNhan.Value.Date >= tuNgay.Date && p.NgayTiepNhan.Value.Date <= denNgay.Date).ToList();
                if (results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        lst.Add(result);
                    }
                    return lst;
                }
                else return null;
            }
            else
            {
                var results = db.PSTiepNhans.OrderBy(p => p.MaPhieu).Where(p => p.isXoa == false && p.isDaDanhGia == true && p.MaDonVi == maDonVi && p.NgayTiepNhan.Value.Date >= tuNgay.Date && p.NgayTiepNhan.Value.Date <= denNgay.Date).ToList();
                if (results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        lst.Add(result);
                    }
                    return lst;
                }
                else return null;
            }
        }
        public PSPhieuSangLoc GetPhieuSangLoc(string maPhieu, string maDonVi)
        {
            try
            {
                var result = db.PSPhieuSangLocs.FirstOrDefault(p => p.IDPhieu == maPhieu && p.IDCoSo == maDonVi && p.isXoa == false);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex) { return null; }

        }
        public List<PsHoanDongBo> GetFullPhieuSangLoc(DateTime NgayBD, DateTime NgayKT, string madv, bool dongbo, bool kq)
        {
            List<PsHoanDongBo> lst = new List<PsHoanDongBo>();
            List<PSPhieuSangLoc> lph = new List<PSPhieuSangLoc>();
            try
            {
                if (kq == true)
                {
                    if (madv.Equals("all"))
                    {
                        lph = db.PSPhieuSangLocs.OrderBy(p => p.IDPhieu).Where(p => p.isDongBo == dongbo && p.isXoa != true && p.NgayTaoPhieu.Value.Date >= NgayBD.Date && p.NgayTaoPhieu.Value.Date <= NgayKT.Date && p.TrangThaiMau >= 4 && p.TrangThaiMau != 5).ToList();
                    }
                    else
                    {
                        lph = db.PSPhieuSangLocs.OrderBy(p => p.IDPhieu).Where(p => p.IDCoSo.Contains(madv) && p.isDongBo == dongbo && p.isXoa != true && p.NgayTaoPhieu.Value.Date >= NgayBD.Date && p.NgayTaoPhieu.Value.Date <= NgayKT.Date && p.TrangThaiMau >= 4 && p.TrangThaiMau != 5).ToList();
                    }
                }
                else
                {
                    if (madv.Equals("all"))
                    {
                        lph = db.PSPhieuSangLocs.OrderBy(p => p.IDPhieu).Where(p => p.isDongBo == dongbo && p.isXoa != true && p.NgayTaoPhieu.Value.Date >= NgayBD.Date && p.NgayTaoPhieu.Value.Date <= NgayKT.Date).ToList();
                    }
                    else
                    {
                        lph = db.PSPhieuSangLocs.OrderBy(p => p.IDPhieu).Where(p => p.IDCoSo.Contains(madv) && p.isDongBo == dongbo && p.isXoa != true && p.NgayTaoPhieu.Value.Date >= NgayBD.Date && p.NgayTaoPhieu.Value.Date <= NgayKT.Date).ToList();
                    }
                }

                foreach (var lp in lph)
                {
                    PsHoanDongBo db = new PsHoanDongBo();
                    db.IDCoSo = lp.IDCoSo;
                    db.IDPhieu = lp.IDPhieu;
                    db.MaBenhNhan = lp.MaBenhNhan;
                    lst.Add(db);
                }

            }
            catch (Exception ex) { }
            return lst;
        }
        public PsReponse HoanDongBo(string IDPhieu, string IDCoSo, string MaBN)
        {
            PsReponse lst = new PsReponse();
            try
            {
                db.pro_HoanDongBoPhieu(IDPhieu, MaBN);
                lst.Result = true;
            }
            catch
            {
                lst.Result = false;
            }
            return lst;
        }
        public PsReponse HoanDongBoPhieu(string IDPhieu)
        {
            PsReponse lst = new PsReponse();
            try
            {
                db.pro_HoanDongBoPhieuLe(IDPhieu);
                lst.Result = true;
            }
            catch
            {
                lst.Result = false;
            }
            return lst;
        }
        public PSTTPhieu GetTTPhieu(string maPhieu, string maDonVi)
        {
            PSTTPhieu ttPhieu = new PSTTPhieu();
            try
            {
                ttPhieu.Phieu = db.PSPhieuSangLocs.FirstOrDefault(p => p.IDPhieu == maPhieu && p.IDCoSo == maDonVi && p.isXoa != true);
                ttPhieu.TiepNhan = db.PSTiepNhans.FirstOrDefault(p => p.MaPhieu == maPhieu && p.MaDonVi == maDonVi && p.isXoa != true);
                if (ttPhieu.TiepNhan != null)
                {
                    ttPhieu.ChiDinh = db.PSChiDinhDichVus.FirstOrDefault(p => p.MaPhieu == maPhieu && p.MaDonVi == maDonVi && p.MaTiepNhan == ttPhieu.TiepNhan.MaTiepNhan && p.isXoa != true);
                }
                if (ttPhieu.Phieu != null)
                {
                    ttPhieu.Benhnhan = db.PSPatients.FirstOrDefault(p => p.MaBenhNhan == ttPhieu.Phieu.MaBenhNhan && p.isXoa != true);
                }
                ttPhieu.lstLyDoKhongDat = db.PSChiTietDanhGiaChatLuongs.Where(p => p.IDPhieu == maPhieu && p.isXoa != true).ToList();
                var data = (from goi in db.PSDanhMucGoiDichVuTheoDonVis
                            join ct in db.PSChiTietGoiDichVuChungs on goi.IDGoiDichVuChung equals ct.IDGoiDichVuChung
                            join dv in db.PSDanhMucDichVus on ct.IDDichVu equals dv.IDDichVu
                            where goi.MaDVCS.Equals(maDonVi)
                            select new { goi.DonGia, dv.MaNhom, dv.TenHienThiDichVu, dv.TenDichVu, dv.IDDichVu }).ToList();
                if (data.Count() > 0)
                {
                    foreach (var da in data)
                    {
                        PsDichVu dvu = new PsDichVu();
                        dvu.GiaDichVu = da.DonGia == null ? 0 : da.DonGia.Value;
                        dvu.IDDichVu = da.IDDichVu;
                        dvu.isChecked = false;
                        dvu.isLocked = false;
                        dvu.MaNhom = da.MaNhom.ToString();
                        dvu.TenDichVu = da.TenDichVu;
                        dvu.TenHienThi = da.TenHienThiDichVu;
                        ttPhieu.lstChiDinhDichVu.Add(dvu);

                    }
                }
            }
            catch { }
            return ttPhieu;
        }
        public PSPatient GetThongTinDuLieuBenhNhan(string maBenhNhan)
        {
            try
            {
                var result = db.PSPatients.FirstOrDefault(p => p.MaBenhNhan == maBenhNhan && p.isXoa == false);
                if (result != null)
                    return result;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
        public PSEmployee GetThongTinDuLieuNhanVien(string maNV)
        {
            try
            {
                var result = db.PSEmployees.FirstOrDefault(p => p.EmployeeCode == maNV);
                if (result != null)
                    return result;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
        //public PsPerson GetThongTinConNguoi(string maThongTin)
        //{
        //    try
        //    {
        //        var result = db.PsPersons.FirstOrDefault(p => p.MaThongTin == maThongTin);
        //        if (result != null)
        //            return result;
        //        else
        //            return null;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        public List<PSDanhMucChiCuc> GetDanhMucChiCuc()
        {
            List<PSDanhMucChiCuc> lst = new List<PSDanhMucChiCuc>();
            try
            {
                var cc = db.PSDanhMucChiCucs.ToList();
                if (cc.Count > 0)
                    lst = cc;
            }
            catch
            {
            }
            return lst;
        }
        public List<PSDanhMucDanToc> GetDanhSachDanToc(int maQuocGia)
        {
            try
            {
                if (maQuocGia < 0)
                    return db.PSDanhMucDanTocs.ToList();
                else
                    return db.PSDanhMucDanTocs.Where(p => p.IDQuocGia == maQuocGia).ToList();
            }
            catch { return null; }
        }
        public string GetThongTinPhieuLan1(string maPhieuLan2, string maDonVi)
        {
            try
            {
                return db.PSPhieuSangLocs.FirstOrDefault(x => x.IDPhieu == maPhieuLan2 && x.IDCoSo == maDonVi).IDPhieuLan1;
            }
            catch { return null; }
        }
        public List<PSChiDinhTrenPhieu> GetDichVuCanLamLaiCuaPhieu(string maPhieuCu, string maDonVi)
        {
            List<PSChiDinhTrenPhieu> lst = new List<PSChiDinhTrenPhieu>();
            try
            {
                string maTN = db.PSTiepNhans.FirstOrDefault(p => p.MaPhieu == maPhieuCu && p.MaDonVi == maDonVi && p.isXoa == false).MaTiepNhan;
                if (string.IsNullOrEmpty(maTN))
                    return lst;
                else
                {
                    var lstnguyco = db.PSXN_TraKQ_ChiTiets.Where(p => p.MaTiepNhan == maTN && p.MaPhieu == maPhieuCu && p.isNguyCo == true && p.isXoa == false).ToList();
                    if (lstnguyco.Count > 0)
                    {
                        foreach (var dichvu in lstnguyco)
                        {
                            PSChiDinhTrenPhieu dv = new PSChiDinhTrenPhieu();
                            dv.MaDichVu = dichvu.MaDichVu;
                            dv.MaPhieu = maPhieuCu;
                            lst.Add(dv);
                        }
                    }

                }
            }
            catch
            {
            }
            return lst;
        }

        public bool XacThucNhanVien(string maNV, string pass)
        {
            try
            {
                if (!string.IsNullOrEmpty(maNV) && !string.IsNullOrEmpty(pass))
                {
                    var res = db.PSEmployees.FirstOrDefault(p => p.EmployeeCode == maNV && p.Password == pass);
                    if (res != null)
                        return true;
                    else return false;
                }
                else return false;
            }
            catch { return false; }
        }

        public List<PSChiTietGoiDichVuChung> GetDichVuTheoMaGoi(string maGoiDichVi)
        {
            List<PSChiTietGoiDichVuChung> lst = new List<PSChiTietGoiDichVuChung>();
            try
            {
                if (maGoiDichVi.Equals("DVGXN0001"))
                {
                    var dv = db.PSDanhMucDichVus.ToList();
                    int i = 1;
                    foreach (var ctdv in dv)
                    {
                        PSChiTietGoiDichVuChung goi = new PSChiTietGoiDichVuChung();
                        goi.IDDichVu = ctdv.IDDichVu;
                        goi.IDGoiDichVuChung = "DVGXN0001";
                        goi.RowIDChiTietGoiDichVuChung = i++;
                        lst.Add(goi);
                    }
                }
                else
                {
                    lst = db.PSChiTietGoiDichVuChungs.Where(p => p.IDGoiDichVuChung == maGoiDichVi).ToList();
                }
            }
            catch
            {
            }
            return lst;
        }
        public PSDanhMucDichVu GetDichVuTheoDonVi(string maDichVu, string maDonVi)
        {
            var result = db.PSDanhMucDichVus.Where(x => x.IDDichVu == maDichVu).FirstOrDefault();
            if (result != null)
                return result;
            else return null;
        }

        public List<PSDanhMucGoiDichVuTheoDonVi> GetDanhMucGoiXetNghiemTrungTam(string maDonVi)
        {
            List<PSDanhMucGoiDichVuTheoDonVi> lst = new List<PSDanhMucGoiDichVuTheoDonVi>();
            if (!string.IsNullOrEmpty(maDonVi))
            {

                // var results = db.PSDanhMucGoiDichVuTheoDonVis.Where(p => p.MaDVCS == maDonVi).ToList();
                var results = (from p in db.PSDanhMucGoiDichVuTheoDonVis
                               join x in db.PSDanhMucGoiDichVuChungs on p.IDGoiDichVuChung equals x.IDGoiDichVuChung
                               where p.MaDVCS == maDonVi
                               orderby x.Stt
                               select p).ToList();
                if (results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        lst.Add(result);
                    }
                    return lst;

                }
            }
            return lst;
        }

        public List<PSDanhMucGhiChu> GetListCauHinhGhiChu()
        {
            List<PSDanhMucGhiChu> lst = new List<PSDanhMucGhiChu>();
            var res = db.PSDanhMucGhiChus.ToList();
            if (res.Count > 0)
                return res;
            else return lst;
        }

        public List<PSDanhMucGoiDichVuChung> GetDanhMucGoiXetNghiemChung(string idGoiDichVuChung)
        {
            List<PSDanhMucGoiDichVuChung> lst = new List<PSDanhMucGoiDichVuChung>();
            if (!string.IsNullOrEmpty(idGoiDichVuChung))
            {
                var result = db.PSDanhMucGoiDichVuChungs.FirstOrDefault(p => p.IDGoiDichVuChung == idGoiDichVuChung);
                if (result != null) { lst.Add(result); return lst; }
                else return null;
            }
            else
            {
                var results = db.PSDanhMucGoiDichVuChungs.ToList();
                if (results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        lst.Add(result);
                    }
                    return lst;
                }
                else return null;
            }
        }
        public List<PSDanhMucDichVu> GetDanhSachDichVu(bool isLocked)
        {
            List<PSDanhMucDichVu> lst = new List<PSDanhMucDichVu>();
            try
            {
                if (!isLocked)
                {
                    var results = db.PSDanhMucDichVus.Where(p => p.isLocked != true).ToList();
                    if (results.Count > 0)
                    {
                        foreach (var result in results)
                        {
                            lst.Add(result);
                        }
                    }
                    else return null;
                }
                else
                {
                    var results = db.PSDanhMucDichVus.ToList();
                    if (results.Count > 0)
                    {
                        foreach (var result in results)
                        {
                            lst.Add(result);
                        }
                    }
                    else return null;
                }
            }
            catch { }
            return lst;
        }
        public List<PSDanhMucChuongTrinh> GetDanhSachChuongTrinh(bool isLocked)
        {
            List<PSDanhMucChuongTrinh> lst = new List<PSDanhMucChuongTrinh>();
            try
            {
                if (!isLocked)
                {
                    var results = db.PSDanhMucChuongTrinhs.ToList();
                    if (results.Count > 0)
                    {
                        foreach (var result in results)
                        {
                            lst.Add(result);
                        }
                        return lst;
                    }
                    else return null;

                }
                else
                {
                    var results = db.PSDanhMucChuongTrinhs.ToList();
                    if (results.Count > 0)
                    {
                        foreach (var result in results)
                        {
                            lst.Add(result);
                        }
                        return lst;
                    }
                    else return null;
                }
            }
            catch (Exception ex) { return null; }
        }
        public List<PSXN_KetQua> GetDanhSachChoNhanKetQua(DateTime tuNgay, DateTime denNgay, string maDonVi, bool isCoKQ)
        {
            List<PSXN_KetQua> lst = new List<PSXN_KetQua>();
            try
            {
                if (isCoKQ == false)
                {
                    if (string.IsNullOrEmpty(maDonVi) || maDonVi.Equals("all"))
                    {
                        lst = db.PSXN_KetQuas.OrderBy(p => p.MaXetNghiem).Where(p => p.isXoa != true && p.isCoKQ == isCoKQ && p.NgayLamXetNghiem.Value.Date >= tuNgay.Date && p.NgayLamXetNghiem.Value.Date <= denNgay.Date).ToList();

                    }
                    else
                    {
                        lst = db.PSXN_KetQuas.OrderBy(p => p.MaXetNghiem).Where(p => p.isXoa != true && p.isCoKQ == isCoKQ && p.NgayLamXetNghiem.Value.Date >= tuNgay.Date && p.NgayLamXetNghiem.Value.Date <= denNgay.Date && p.MaDonVi.Contains(maDonVi)).ToList();


                    }
                }
                else if (isCoKQ == true)
                {
                    if (string.IsNullOrEmpty(maDonVi) || maDonVi.Equals("all"))
                    {
                        lst = db.PSXN_KetQuas.OrderBy(p => p.MaXetNghiem).Where(p => p.isXoa == false && p.isCoKQ == isCoKQ && p.NgayTraKQ.Value.Date >= tuNgay.Date && p.NgayTraKQ.Value.Date <= denNgay.Date).ToList();

                    }
                    else
                    {
                        lst = db.PSXN_KetQuas.OrderBy(p => p.MaXetNghiem).Where(p => p.isXoa == false && p.isCoKQ == isCoKQ && p.NgayTraKQ.Value.Date >= tuNgay.Date && p.NgayTraKQ.Value.Date <= denNgay.Date && p.MaDonVi.Contains(maDonVi)).ToList();


                    }
                }

            }
            catch
            {
            }
            return lst;
        }
        public List<pro_ReportGanViTriMayXNResult> GanViTriMayXNResult(DateTime tungay, DateTime denngay)
        {
            List<pro_ReportGanViTriMayXNResult> lst = new List<pro_ReportGanViTriMayXNResult>();
            var data = db.pro_ReportGanViTriMayXN(tungay.Date, denngay.Date).ToList();

            foreach (var da in data)
            {
                lst.Add(da);

            }

            return lst;
        }
        public List<PSChiTietDanhGiaChatLuong> GetChiTietDanhGiaMạuKhongDatTrenPhieu(string maPhieu, string maTiepNhan)
        {
            List<PSChiTietDanhGiaChatLuong> lst = new List<PSChiTietDanhGiaChatLuong>();
            var res = db.PSChiTietDanhGiaChatLuongs.Where(p => p.MaTiepNhan == p.MaTiepNhan && p.IDPhieu == maPhieu && p.isXoa == false).ToList();
            if (res.Count > 0) return res;
            else return lst;
        }
        public PSXN_TraKetQua GetThongTinTraKetQua(string maPhieu, string maTiepNhan)
        {
            PSXN_TraKetQua KQ = new PSXN_TraKetQua();
            try
            {
                var result = db.PSXN_TraKetQuas.FirstOrDefault(p => p.isXoa == false && p.MaPhieu == maPhieu && p.MaTiepNhan == maTiepNhan);
                if (result != null)
                    return result;
                else return null;
            }
            catch
            {
                return null;
            }
        }
        public string GetTenDichVuCuaKyThuat(string maKyThuat)
        {
            try
            {
                var result = db.PSDanhMucDichVus.FirstOrDefault(p => p.IDDichVu == db.PSMapsXN_DichVus.FirstOrDefault(o => o.IDKyThuatXN == maKyThuat).IDDichVu).TenHienThiDichVu;
                return result;
            }
            catch { return string.Empty; }
        }
        public string GetTenDichVuVietTatCuaKyThuat(string maKyThuat)
        {
            try
            {
                var result = db.PSDanhMucDichVus.FirstOrDefault(p => p.IDDichVu == db.PSMapsXN_DichVus.FirstOrDefault(o => o.IDKyThuatXN == maKyThuat).IDDichVu).TenDichVu;
                return result;
            }
            catch { return string.Empty; }
        }
        public List<PSXN_TraKQ_ChiTiet> GetThongTinTraKetQuaChiTiet(string maPhieu, string maTiepNhan)
        {
            try
            {
                var results = db.PSXN_TraKQ_ChiTiets.Where(p => p.isXoa == false && p.MaPhieu == maPhieu && p.MaTiepNhan == maTiepNhan).ToList();
                if (results.Count < 1)
                {
                    return null;
                }
                else return results.OrderByDescending(p => p.Stt).ToList();
            }
            catch { return null; }
        }
        public PSPatient GetThongTinBenhNhan(string maBenhNhan)
        {
            PSPatient BN = new PSPatient();
            try
            {
                var bn = db.PSPatients.FirstOrDefault(p => p.MaBenhNhan == maBenhNhan && p.isXoa == false);
                if (bn != null) return bn;
                else return BN;

            }
            catch
            {
                return BN;
            }

        }
        public List<PSXN_TraKetQua> GetDanhSachChoTraKetQua(DateTime tuNgay, DateTime denNgay, string maDonVi, bool isDaDuyetKQ)
        {
            List<PSXN_TraKetQua> lst = new List<PSXN_TraKetQua>();
            try
            {
                if (isDaDuyetKQ == true)
                {
                    if (string.IsNullOrEmpty(maDonVi) || maDonVi.Equals("all"))
                    {
                        var results = db.PSXN_TraKetQuas.OrderBy(p => p.MaPhieu).Where(p => p.isXoa == false && p.isDaDuyetKQ == true && p.NgayCoKQ.Value.Date >= tuNgay.Date && p.NgayCoKQ.Value.Date <= denNgay.Date).ToList();
                        if (results.Count > 0)
                        {
                            foreach (var result in results)
                            {
                                lst.Add(result);
                            }
                        }
                    }
                    else
                    {
                        var results = db.PSXN_TraKetQuas.OrderBy(p => p.MaPhieu).Where(p => p.isXoa == false && p.isDaDuyetKQ == true && p.NgayCoKQ.Value.Date >= tuNgay.Date && p.NgayCoKQ.Value.Date <= denNgay.Date && p.IDCoSo.Contains(maDonVi)).ToList();
                        if (results.Count > 0)
                        {
                            foreach (var result in results)
                            {
                                lst.Add(result);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return lst;
        }
        public List<PSXN_TTTraKQ> GetDanhSachChoTraKetQuaAll(DateTime tuNgay, DateTime denNgay, string maDonVi, bool isDaDuyetKQ)
        {

            List<PSXN_TTTraKQ> lst = new List<PSXN_TTTraKQ>();
            try
            {
                if (isDaDuyetKQ == false)
                {

                    if (string.IsNullOrEmpty(maDonVi) || maDonVi.Equals("all"))
                    {
                        var results = db.PSXN_TraKetQuas.Where(p => p.isXoa == false && p.isDaDuyetKQ == false && p.NgayCoKQ.Value.Date >= tuNgay.Date && p.NgayCoKQ.Value.Date <= denNgay.Date).OrderBy(p => p.MaPhieu).ToList();
                        if (results.Count > 0)
                        {
                            foreach (var result in results)
                            {
                                PSXN_TTTraKQ ls = new PSXN_TTTraKQ();
                                var NhapLieu = db.PSChiDinhDichVus.FirstOrDefault(x => x.MaPhieu == result.MaPhieu && x.isXoa != true && x.IDGoiDichVu != "DVGXNL2").isDaNhapLieu;
                                ConvertObjectToObject(result, ls);
                                ls.isDaNhapLieu = NhapLieu != null ? NhapLieu : false;
                                lst.Add(ls);
                            }
                        }
                    }
                    else
                    {
                        var results = db.PSXN_TraKetQuas.Where(p => p.isXoa == false && p.isDaDuyetKQ == false && p.NgayCoKQ.Value.Date >= tuNgay.Date && p.NgayCoKQ.Value.Date <= denNgay.Date && p.IDCoSo.Contains(maDonVi)).OrderBy(p => p.MaPhieu).ToList();
                        if (results.Count > 0)
                        {
                            foreach (var result in results)
                            {
                                PSXN_TTTraKQ ls = new PSXN_TTTraKQ();
                                var NhapLieu = db.PSChiDinhDichVus.FirstOrDefault(x => x.MaPhieu == result.MaPhieu && x.isXoa != true && x.IDGoiDichVu != "DVGXNL2").isDaNhapLieu;
                                ConvertObjectToObject(result, ls);
                                ls.isDaNhapLieu = NhapLieu != null ? NhapLieu : false;
                                lst.Add(ls);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return lst;
        }
        public List<PSXN_TTTraKQ> GetDanhSachChoTraKetQuaAll()
        {

            List<PSXN_TTTraKQ> lst = new List<PSXN_TTTraKQ>();
            try
            {
                var results = db.PSXN_TraKetQuas.Where(p => p.isXoa == false && p.isDaDuyetKQ != true).OrderBy(p => p.MaPhieu).ToList();
                if (results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        PSXN_TTTraKQ ls = new PSXN_TTTraKQ();
                        var NhapLieu = db.PSChiDinhDichVus.FirstOrDefault(x => x.MaPhieu == result.MaPhieu && x.isXoa != true && x.IDGoiDichVu != "DVGXNL2").isDaNhapLieu;
                        ConvertObjectToObject(result, ls);
                        ls.isDaNhapLieu = NhapLieu != null ? NhapLieu : false;
                        lst.Add(ls);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return lst;
        }
        public object ConvertObjectToObject(object source, object des)
        {
            var props = des.GetType().GetProperties();

            foreach (PropertyInfo prop in props)
            {
                try
                {
                    var data = source.GetType().GetProperty(prop.Name).GetValue(source, null);
                    prop.SetValue(des, data, null);

                }
                catch
                {
                    prop.SetValue(des, null, null);
                }
            }
            return des;
        }
        public List<PSXN_TraKQ_ChiTiet> GetDanhSachTraKetQuaChiTietPhieuCu(string maPhieu)
        {
            List<PSXN_TraKQ_ChiTiet> lst = new List<PSXN_TraKQ_ChiTiet>();
            try
            {
                var results = db.PSXN_TraKQ_ChiTiets.Where(p => p.isXoa == false && p.MaPhieu == maPhieu).ToList();
                if (results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        lst.Add(result);
                    }

                }
            }
            catch { }
            return lst;
        }
        public List<PSXN_TraKQ_ChiTiet> GetDanhSachTraKetQuaChiTiet(string maTiepNhan)
        {
            List<PSXN_TraKQ_ChiTiet> lst = new List<PSXN_TraKQ_ChiTiet>();
            try
            {
                var results = db.PSXN_TraKQ_ChiTiets.Where(p => p.isXoa == false && p.MaTiepNhan == maTiepNhan).ToList();
                if (results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        lst.Add(result);
                    }

                }
            }
            catch { }
            return lst;
        }
        public string GetMaPhieu(string maTiepNhan)
        {

            try
            {
                return db.PSXN_TraKetQuas.FirstOrDefault(p => p.isXoa != true && p.MaTiepNhan == maTiepNhan).MaPhieu;
            }
            catch
            {
                return null;
            }
        }
        public string GetMaPhieu1TheoMaBN(string maBN)
        {
            try
            {
                return db.PSPhieuSangLocs.FirstOrDefault(p => p.MaBenhNhan == maBN && p.isLayMauLan2 != true && p.isXoa != true).IDPhieu;
            }
            catch
            {
                return null;
            }
        }
        public string GetMaPhieu2TheoMaBN(string maBN)
        {
            try
            {
                return db.PSPhieuSangLocs.FirstOrDefault(p => p.MaBenhNhan == maBN && p.isLayMauLan2 == true && p.isXoa != true).IDPhieu;
            }
            catch
            {
                return null;
            }
        }
        public PSDotChuanDoan GetThongTinDotChanDoan(long rowID)
        {
            try
            {
                return db.PSDotChuanDoans.FirstOrDefault(p => p.isXoa == false && p.rowIDDotChanDoan == rowID);
            }
            catch
            {
                return null;
            }
        }
        public List<PSDotChuanDoan> GetDanhSachDotChanDoanCuaBenhNhan(string MaBenhNhan)
        {
            List<PSDotChuanDoan> lst = new List<PSDotChuanDoan>();
            try
            {
                var res = db.PSDotChuanDoans.Where(p => p.isXoa == false && p.MaBenhNhan == MaBenhNhan).ToList();
                if (res != null)
                    lst = res;
            }
            catch { }
            return lst;
        }
        public List<PSDotChuanDoan> GetDanhSachDotChanDoanCuaBenhNhan(string maBenhNhan, string maKhachHang)
        {
            List<PSDotChuanDoan> lst = new List<PSDotChuanDoan>();
            try
            {
                var res = db.PSDotChuanDoans.Where(p => p.isXoa == false && p.MaBenhNhan == maBenhNhan && p.MaKhachHang == maKhachHang).ToList();
                if (res != null)
                    lst = res;
            }
            catch
            {
            }
            return lst;
        }
        public List<PSXN_TraKQ_ChiTiet> GetDanhSachTraKetQuaChiTiet(string maTiepNhan, string maPhieu)
        {
            List<PSXN_TraKQ_ChiTiet> lst = new List<PSXN_TraKQ_ChiTiet>();
            try
            {
                var results = db.PSXN_TraKQ_ChiTiets.Where(p => p.isXoa != true && p.MaTiepNhan == maTiepNhan && p.MaPhieu == maPhieu).ToList();
                if (results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        lst.Add(result);
                    }

                }
            }
            catch { }
            return lst;
        }
        public bool KiemTraPhieuThuMauLaiDaDuocChiDinhChua(string maphieulan1, string maphieulamlai)
        {
            bool k = false;
            try
            {

                var phieu = db.PSPhieuSangLocs.Where(p => p.isXoa == false && p.IDPhieuLan1 == maphieulan1).ToList();
                if (phieu != null)
                {
                    if (phieu.Count() > 1)
                    {
                        k = true;
                    }
                    else
                    {
                        foreach (var ph in phieu)
                        {
                            if (ph.IDPhieu == maphieulamlai)
                            {
                                k = false;
                            }
                            else
                                k = true;
                        }
                    }
                }
                else
                    k = false;

            }
            catch { k = true; }
            return k;
        }
        public List<PSXN_KetQua_ChiTiet> GetDanhSachChiTietKetQua(string maKetQua, string MaXN)
        {
            List<PSXN_KetQua_ChiTiet> lst = new List<PSXN_KetQua_ChiTiet>();
            try
            {
                if (string.IsNullOrEmpty(MaXN))
                {
                    lst = db.PSXN_KetQua_ChiTiets.Where(p => p.isXoa != true && p.MaKQ == maKetQua).ToList();
                }
                else
                {
                    lst = db.PSXN_KetQua_ChiTiets.Where(p => p.isXoa != true && p.MaKQ == maKetQua && p.MaXetNghiem == MaXN).ToList();
                }
            }
            catch
            {
            }
            return lst;
        }
        public PSXN_KetQua GetPhieuXNKetQua(string maPhieu, string MaXN, string MaGoiXN)
        {
            PSXN_KetQua lst = new PSXN_KetQua();
            try
            {
                lst = db.PSXN_KetQuas.FirstOrDefault(p => p.isXoa != true && p.MaPhieu == maPhieu && p.MaXetNghiem == MaXN && p.MaGoiXN == MaGoiXN);
            }
            catch
            {
            }
            return lst;
        }
        public PSPhieuSangLoc GetThongTinPhieu(string maPhieu)
        {
            try
            {
                var result = db.PSPhieuSangLocs.FirstOrDefault(p => p.isXoa != true && p.IDPhieu == maPhieu);
                return result;
            }
            catch
            {
                return null;
            }
        }
        #endregion GET
        #region SET
        public PsReponse DuyetNhanh(string maTiepNhan, string maPhieu)
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
        public PsReponse KoThuLaiMau(string maPhieu, string maTiepNhan)
        {
            PsReponse result = new PsReponse();
            result.Result = true;
            try
            {
                var kq = db.PSXN_TraKetQuas.FirstOrDefault(p => p.isXoa != true && p.MaPhieu == maPhieu && p.MaTiepNhan == maTiepNhan);
                if (kq != null)
                {
                    kq.isDaDuyetKQ = true;
                    kq.isTraKQ = true;
                    kq.isDaDuyetKQ = true;
                    kq.isDongBo = false;
                    kq.NgayTraKQ = DateTime.Now;
                }
                db.SubmitChanges();
                var lstkq = db.PSXN_TraKQ_ChiTiets.Where(p => p.isXoa != true && p.MaPhieu == maPhieu && p.MaTiepNhan == maTiepNhan).ToList();
                if (lstkq.Count > 0)
                {
                    foreach (var item in lstkq)
                    {
                        item.isDaDuyetKQ = true;
                        item.isDongBo = false;
                        db.SubmitChanges();
                    }
                }
                var phieu = db.PSPhieuSangLocs.FirstOrDefault(k => k.IDPhieu == maPhieu && k.isXoa != true);
                if (phieu != null)
                {
                    if (kq.isNguyCoCao == true)
                    {
                        if (phieu.isLayMauLan2 == true)
                        {
                            phieu.TrangThaiMau = 4;
                        }
                        else
                        {

                            phieu.TrangThaiMau = 6;
                        }
                        phieu.isNguyCoCao = true;
                    }
                    else
                    {
                        phieu.TrangThaiMau = 4;
                        phieu.isNguyCoCao = false;
                    }
                    phieu.NgayCoKQ = kq.NgayCoKQ;
                    phieu.isDongBo = false;
                    db.SubmitChanges();
                }
                #region Cập nhật Bệnh nhân nguy cơ cao

                try
                {
                    var Bn = db.PSPatients.FirstOrDefault(p => p.isXoa == false && p.MaBenhNhan == phieu.MaBenhNhan);
                    if (Bn != null)
                    {
                        var _res = db.PSBenhNhanNguyCoCaos.FirstOrDefault(p => p.isXoa == false && p.MaBenhNhan == phieu.MaBenhNhan && p.MaDonVi == phieu.IDCoSo && p.isXoa == false);
                        if (phieu.isNguyCoCao == true)
                        {
                            if (_res == null)
                            {

                                PSBenhNhanNguyCoCao BNNguyCo = new PSBenhNhanNguyCoCao();
                                BNNguyCo.isNguyCoCao = true;
                                BNNguyCo.MaBenhNhan = phieu.MaBenhNhan;
                                BNNguyCo.MaKhachHang = Bn.MaKhachHang;
                                BNNguyCo.MaTiepNhan = kq.MaTiepNhan;
                                BNNguyCo.MaDonVi = phieu.IDCoSo;
                                BNNguyCo.NgayTiepNhan = kq.NgayTiepNhan;
                                BNNguyCo.TenBenhNhan = Bn.TenBenhNhan;
                                BNNguyCo.isDaChanDoan = false;
                                BNNguyCo.isDieuTri = false;
                                BNNguyCo.isXoa = false;
                                BNNguyCo.isDongBo = false;
                                db.PSBenhNhanNguyCoCaos.InsertOnSubmit(BNNguyCo);
                                db.SubmitChanges();

                            }
                            else
                            {
                                _res.isNguyCoCao = true;
                                _res.isXoa = false;
                                _res.isDongBo = false;
                                _res.TenBenhNhan = Bn.TenBenhNhan;
                                if (_res.MaTiepNhan.Equals(kq.MaTiepNhan))
                                    _res.MaTiepNhan = kq.MaTiepNhan;
                                else _res.MaTiepNhan2 = kq.MaTiepNhan;
                                db.SubmitChanges();
                            }
                        }
                        else
                        {
                            if (_res != null)
                            {
                                _res.isNguyCoCao = false;
                                _res.isXoa = false;
                                _res.isDongBo = false;
                                _res.TenBenhNhan = Bn.TenBenhNhan;
                                if (_res.MaTiepNhan.Equals(kq.MaTiepNhan))
                                    _res.MaTiepNhan = kq.MaTiepNhan;
                                else _res.MaTiepNhan2 = kq.MaTiepNhan;
                                db.SubmitChanges();
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
                #endregion
                result.Result = true;

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
        public PsReponse HuyMauPhieu(string maPhieu, string maTiepnhan, string maDonVi, string maNV, string lydoXoa)
        {
            PsReponse result = new PsReponse();
            result.Result = true;
            try
            {
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();
                DateTime ngaygio = DateTime.Now;
                try
                {
                    ngaygio = GetDateTimeServer();
                }
                catch
                {
                    ngaygio = DateTime.Now;
                }
                var phieu = db.PSPhieuSangLocs.FirstOrDefault(p => p.isXoa == false && p.IDPhieu == maPhieu);
                if (phieu != null)
                {
                    if ((phieu.TrangThaiMau ?? 0) == 7)
                    {
                        var lstp = db.PSPhieuSangLocs.FirstOrDefault(p => p.IDPhieuLan1 == maPhieu && p.TrangThaiMau > 1);
                        if (lstp != null)
                        {
                            result.Result = false;
                            result.StringError = "Phiếu " + maPhieu + " đã được thu mẫu lại và đã vào quy trình nên không thể hủy! \r\n Để xóa mẫu này thì cần phải hủy mẫu " + phieu.IDPhieu + "trước!";
                            throw new Exception("Phiếu đã được thu mẫu lại !");
                        }
                    }
                }
                var TraKQ = db.PSXN_TraKetQuas.FirstOrDefault(p => p.MaPhieu == maPhieu && p.MaTiepNhan == maTiepnhan && p.IDCoSo == maDonVi && p.isXoa != true);
                if (TraKQ != null)
                {
                    TraKQ.isXoa = true;
                    TraKQ.isDongBo = false;
                    TraKQ.IDNhanVienXoa = maNV;
                    TraKQ.LyDoXoa = lydoXoa;
                    TraKQ.NgayGioXoa = ngaygio;
                    db.SubmitChanges();
                    var lstchitiet = db.PSXN_TraKQ_ChiTiets.Where(p => p.MaPhieu == maPhieu && p.MaTiepNhan == maTiepnhan && p.isXoa != true).ToList();
                    if (lstchitiet.Count > 0)
                    {
                        foreach (var ct in lstchitiet)
                        {
                            ct.isXoa = true;
                            ct.isDongBo = false;
                            ct.IDNhanVienXoa = maNV;
                            ct.NgayGioXoa = ngaygio;
                            db.SubmitChanges();
                        }
                    }
                }
                var lstKQ = db.PSXN_KetQuas.Where(p => p.MaPhieu == maPhieu && p.MaTiepNhan == maTiepnhan && p.MaDonVi == maDonVi && p.isXoa != true).ToList();
                if (lstKQ.Count > 0)
                {
                    foreach (var KQ in lstKQ)
                    {

                        KQ.isXoa = true;
                        KQ.isDongBo = false;
                        KQ.IDNhanVienXoa = maNV;
                        KQ.LyDoXoa = lydoXoa;
                        KQ.NgayGioXoa = ngaygio;
                        db.SubmitChanges();
                        var lstchitiet = db.PSXN_KetQua_ChiTiets.Where(p => p.MaKQ == KQ.MaKetQua && p.isXoa != true).ToList();
                        if (lstchitiet.Count > 0)
                        {
                            foreach (var ct in lstchitiet)
                            {
                                ct.isXoa = true;
                                ct.isDongBo = false;
                                ct.IDNhanVienXoa = maNV;
                                ct.NgayGioXoa = ngaygio;
                                db.SubmitChanges();
                            }
                        }
                    }
                }
                var lstCD = db.PSChiDinhDichVus.Where(p => p.MaPhieu == maPhieu && p.MaTiepNhan == maTiepnhan && p.MaDonVi == maDonVi && p.isXoa == false).ToList();
                if (lstCD.Count > 0)
                {
                    foreach (var CD in lstCD)
                    {
                        CD.isXoa = true;
                        CD.isDongBo = false;
                        CD.IDNhanVienXoa = maNV;
                        CD.lyDoXoa = lydoXoa;
                        CD.NgayGioXoa = ngaygio;
                        db.SubmitChanges();
                        var lstchitiet = db.PSChiDinhDichVuChiTiets.Where(p => p.MaPhieu == maPhieu && p.MaChiDinh == CD.MaChiDinh && p.isXoa == false).ToList();
                        if (lstchitiet.Count > 0)
                        {
                            foreach (var ct in lstchitiet)
                            {
                                ct.isXoa = true;
                                ct.isDongBo = false;
                                ct.IDNhanVienXoa = maNV;
                                ct.NgayGioXoa = ngaygio;
                                db.SubmitChanges();
                            }
                        }
                    }
                }
                var TN = db.PSTiepNhans.FirstOrDefault(p => p.MaTiepNhan == maTiepnhan && p.isXoa == false);
                if (TN != null)
                {

                    TN.isDongBo = false;
                    //TN.isDaDanhGia = false;
                    TN.isXoa = true;
                    TN.NgayGioXoa = ngaygio;
                    TN.LyDoXoa = lydoXoa;
                    TN.IDNhanVienXoa = maNV;
                    db.SubmitChanges();
                }

                if (phieu != null)
                {
                    //phieu.isLayMauLan2 = false;
                    //phieu.TrangThaiMau = 1;
                    phieu.isXoa = true;
                    phieu.isDongBo = false;
                    db.SubmitChanges();
                }
                PSChiTietXoaPhieu phieuxoa = new PSChiTietXoaPhieu();
                phieuxoa.MaNV = maNV;
                phieuxoa.DateDelete = DateTime.Now;
                phieuxoa.MaPhieu = maPhieu;
                phieuxoa.LydoXoa = lydoXoa;
                db.PSChiTietXoaPhieus.InsertOnSubmit(phieuxoa);
                db.SubmitChanges();
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
        public PsReponse InsertDotChiDinhDichVu(PSTTPhieu dg)
        {
            PsReponse result = new PsReponse();
            result.Result = true;
            try
            {
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();
                bool isNhapLieu = false;



                #endregion Xử lý dữ liệu của đợt chỉ định
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
        public PsReponse InsertDotChiDinhDichVu(PsChiDinhvsDanhGia dg)
        {
            PsReponse result = new PsReponse();
            result.Result = true;
            try
            {
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();
                bool isNhapLieu = false;

                if (dg != null) //Nếu có thông tin đợt chỉ định thì bắt đầu kiểm tra và insert
                {
                    string MaBN = string.Empty;
                    string maPh = string.Empty;
                    var trp = db.PSPhieuSangLocs.FirstOrDefault(p => p.isXoa == false && p.IDPhieu == dg.MaPhieu && p.IDCoSo == dg.MaDonVi);
                    if (trp != null)
                    {
                        if ((trp.TrangThaiMau ?? 0) > 5 || (trp.TrangThaiMau ?? 0) == 4 || (trp.TrangThaiMau ?? 0) < 0)
                        {
                            db.Transaction.Rollback();
                            db.Connection.Close();
                            result.Result = false;
                            result.StringError = "Phiếu đã được duyệt nên không thể lưu kết quả !";
                            return result;
                        }
                    }
                    if (dg.Phieu != null)
                    {
                        //Kiểm tra xem phiêu đã tồn tại trong db chưa? nếu có rồi thì tiến hành update lại thông tin, nếu chưa thì tạo mới
                        if (dg.Phieu.BenhNhan != null)
                        #region //Kiểm tra xem Bệnh nhân đã tồn tại trong db chưa? nếu có rồi thì tiến hành update lại thông tin, nếu chưa thì tạo mới
                        {
                            if (!string.IsNullOrEmpty(dg.Phieu.BenhNhan.MotherName) && !string.IsNullOrEmpty(dg.Phieu.BenhNhan.MotherPhoneNumber) && !string.IsNullOrEmpty(dg.Phieu.BenhNhan.TenBenhNhan) && !string.IsNullOrEmpty(dg.Phieu.BenhNhan.DiaChi) && dg.Phieu.ngayGioLayMau != null)
                                isNhapLieu = true;
                            #region Nếu có thông tin bệnh nhân
                            if (!string.IsNullOrEmpty(dg.Phieu.BenhNhan.MaBenhNhan)) //Kiểm tra mã Bệnh nhân có tồn tại không? nếu có thì cập nhật thông tin bệnh nhân theo mã, nếu ko thì tạo mới.
                            {

                                var pat = db.PSPatients.FirstOrDefault(p => p.MaBenhNhan == dg.Phieu.BenhNhan.MaBenhNhan && p.isXoa == false);
                                if (pat != null) // Nếu có tồn tại bệnh nhân theo mã bệnh nhân
                                {
                                    #region Update Bệnh nhân 
                                    MaBN = dg.Phieu.maBenhNhan;
                                    pat.FatherName = dg.Phieu.BenhNhan.FatherName;
                                    pat.FatherPhoneNumber = dg.Phieu.BenhNhan.FatherPhoneNumber;
                                    pat.FatherBirthday = dg.Phieu.BenhNhan.FatherBirthday;
                                    pat.MotherBirthday = dg.Phieu.BenhNhan.MotherBirthday;
                                    pat.MotherPhoneNumber = dg.Phieu.BenhNhan.MotherPhoneNumber;
                                    pat.MotherName = dg.Phieu.BenhNhan.MotherName;
                                    pat.DiaChi = dg.Phieu.BenhNhan.DiaChi;
                                    pat.TenBenhNhan = dg.Phieu.BenhNhan.TenBenhNhan;
                                    pat.QuocTichID = dg.Phieu.BenhNhan.QuocTichID;
                                    pat.TuanTuoiKhiSinh = dg.Phieu.BenhNhan.TuanTuoiKhiSinh;
                                    pat.PhuongPhapSinh = dg.Phieu.BenhNhan.PhuongPhapSinh;
                                    pat.NoiSinh = dg.Phieu.BenhNhan.NoiSinh;
                                    pat.NgayGioSinh = dg.Phieu.BenhNhan.NgayGioSinh;
                                    pat.GioiTinh = dg.Phieu.BenhNhan.GioiTinh;
                                    pat.DanTocID = dg.Phieu.BenhNhan.DanTocID;
                                    pat.CanNang = dg.Phieu.BenhNhan.CanNang;
                                    pat.IDThaiPhuTienSoSinh = dg.Phieu.BenhNhan.IDThaiPhuTienSoSinh ?? string.Empty;
                                    pat.isDongBo = false;
                                    pat.isXoa = false;
                                    if (string.IsNullOrEmpty(pat.MaKhachHang) || pat.MaKhachHang == "")
                                        pat.MaKhachHang = GetNewMaKhachHang(dg.MaDonVi, SoBanDau());
                                    pat.Para = dg.Phieu.BenhNhan.Para ?? "0000";
                                    db.SubmitChanges();
                                    #endregion Update Bệnh nhân 
                                }
                                else // Nếu ko có tồn tại bệnh nhân theo mã bệnh nhân
                                {
                                    #region Insert Bệnh nhân mới theo mã Bệnh Nhân
                                    PSPatient patient = new PSPatient();
                                    MaBN = dg.Phieu.BenhNhan.MaBenhNhan;
                                    patient.MaBenhNhan = MaBN;
                                    patient.FatherName = dg.Phieu.BenhNhan.FatherName;
                                    patient.FatherPhoneNumber = dg.Phieu.BenhNhan.FatherPhoneNumber;
                                    patient.FatherBirthday = dg.Phieu.BenhNhan.FatherBirthday;
                                    patient.MotherBirthday = dg.Phieu.BenhNhan.MotherBirthday;
                                    patient.MotherPhoneNumber = dg.Phieu.BenhNhan.MotherPhoneNumber;
                                    patient.MotherName = dg.Phieu.BenhNhan.MotherName;
                                    patient.DiaChi = dg.Phieu.BenhNhan.DiaChi;
                                    patient.TenBenhNhan = dg.Phieu.BenhNhan.TenBenhNhan;
                                    patient.QuocTichID = dg.Phieu.BenhNhan.QuocTichID;
                                    patient.TuanTuoiKhiSinh = dg.Phieu.BenhNhan.TuanTuoiKhiSinh;
                                    patient.PhuongPhapSinh = dg.Phieu.BenhNhan.PhuongPhapSinh;
                                    patient.NoiSinh = dg.Phieu.BenhNhan.NoiSinh;
                                    patient.NgayGioSinh = dg.Phieu.BenhNhan.NgayGioSinh;
                                    patient.GioiTinh = dg.Phieu.BenhNhan.GioiTinh;
                                    patient.DanTocID = dg.Phieu.BenhNhan.DanTocID;
                                    patient.CanNang = dg.Phieu.BenhNhan.CanNang;
                                    patient.IDThaiPhuTienSoSinh = dg.Phieu.BenhNhan.IDThaiPhuTienSoSinh ?? string.Empty;
                                    patient.isDongBo = false;
                                    patient.isXoa = false;
                                    patient.MaKhachHang = GetNewMaKhachHang(dg.MaDonVi, SoBanDau());
                                    patient.Para = dg.Phieu.BenhNhan.Para ?? "0000";
                                    db.PSPatients.InsertOnSubmit(patient);
                                    db.SubmitChanges();
                                    #endregion Insert Bệnh nhân mới theo mã Bệnh Nhân
                                }
                            }
                            else //nếu không tồn tại mã bệnh nhân thì insert bệnh nhân mới
                            {
                                #region Insert Bệnh nhân mới dữ liệu rỗng
                                PSPatient patient = new PSPatient();
                                MaBN = GetID();
                                patient.MaBenhNhan = MaBN;
                                patient.FatherName = dg.Phieu.BenhNhan.FatherName;
                                patient.FatherPhoneNumber = dg.Phieu.BenhNhan.FatherPhoneNumber;
                                patient.FatherBirthday = dg.Phieu.BenhNhan.FatherBirthday;
                                patient.MotherBirthday = dg.Phieu.BenhNhan.MotherBirthday;
                                patient.MotherPhoneNumber = dg.Phieu.BenhNhan.MotherPhoneNumber;
                                patient.MotherName = dg.Phieu.BenhNhan.MotherName;
                                patient.DiaChi = dg.Phieu.BenhNhan.DiaChi;
                                patient.TenBenhNhan = dg.Phieu.BenhNhan.TenBenhNhan;
                                patient.QuocTichID = dg.Phieu.BenhNhan.QuocTichID;
                                patient.TuanTuoiKhiSinh = dg.Phieu.BenhNhan.TuanTuoiKhiSinh;
                                patient.PhuongPhapSinh = dg.Phieu.BenhNhan.PhuongPhapSinh;
                                patient.NoiSinh = dg.Phieu.BenhNhan.NoiSinh;
                                patient.NgayGioSinh = dg.Phieu.BenhNhan.NgayGioSinh;
                                patient.GioiTinh = dg.Phieu.BenhNhan.GioiTinh;
                                patient.DanTocID = dg.Phieu.BenhNhan.DanTocID;
                                patient.CanNang = dg.Phieu.BenhNhan.CanNang;
                                patient.IDThaiPhuTienSoSinh = dg.Phieu.BenhNhan.IDThaiPhuTienSoSinh ?? string.Empty;
                                patient.isDongBo = false;
                                patient.isXoa = false;
                                patient.MaKhachHang = GetNewMaKhachHang(dg.MaDonVi, SoBanDau());
                                patient.Para = dg.Phieu.BenhNhan.Para ?? "0000";
                                db.PSPatients.InsertOnSubmit(patient);
                                db.SubmitChanges();
                                #endregion Insert Bệnh nhân mới dữ liệu rỗng
                            }
                            #endregion Nếu có thông tin bệnh nhân
                        }
                        #endregion
                        else
                        #region// Nếu không tồn tại thông tin Bệnh nhận theo phiếu thì tạo 1 bệnh nhân rỗng
                        {
                            #region Nếu khôngcó thông tin bệnh nhân
                            #region Insert Bệnh nhân mới dữ liệu rỗng
                            PSPatient patient = new PSPatient();
                            MaBN = GetID();
                            patient.MaBenhNhan = MaBN;
                            patient.FatherName = dg.Phieu.BenhNhan.FatherName;
                            patient.FatherPhoneNumber = dg.Phieu.BenhNhan.FatherPhoneNumber;
                            patient.FatherBirthday = dg.Phieu.BenhNhan.FatherBirthday;
                            patient.MotherBirthday = dg.Phieu.BenhNhan.MotherBirthday;
                            patient.MotherPhoneNumber = dg.Phieu.BenhNhan.MotherPhoneNumber;
                            patient.MotherName = dg.Phieu.BenhNhan.MotherName;
                            patient.DiaChi = dg.Phieu.BenhNhan.DiaChi;
                            patient.TenBenhNhan = dg.Phieu.BenhNhan.TenBenhNhan;
                            patient.QuocTichID = dg.Phieu.BenhNhan.QuocTichID;
                            patient.TuanTuoiKhiSinh = dg.Phieu.BenhNhan.TuanTuoiKhiSinh;
                            patient.PhuongPhapSinh = dg.Phieu.BenhNhan.PhuongPhapSinh;
                            patient.NoiSinh = dg.Phieu.BenhNhan.NoiSinh;
                            patient.NgayGioSinh = dg.Phieu.BenhNhan.NgayGioSinh;
                            patient.GioiTinh = dg.Phieu.BenhNhan.GioiTinh;
                            patient.DanTocID = dg.Phieu.BenhNhan.DanTocID;
                            patient.CanNang = dg.Phieu.BenhNhan.CanNang;
                            patient.IDThaiPhuTienSoSinh = dg.Phieu.BenhNhan.IDThaiPhuTienSoSinh ?? string.Empty;
                            patient.isDongBo = false;
                            patient.isXoa = false;
                            patient.MaKhachHang = GetNewMaKhachHang(dg.MaDonVi, SoBanDau());
                            patient.Para = dg.Phieu.BenhNhan.Para ?? "0000";
                            db.PSPatients.InsertOnSubmit(patient);
                            db.SubmitChanges();
                            #endregion Insert Bệnh nhân mới dữ liệu rỗng
                            #endregion Nếu không có thông tin bệnh nhân
                        }
                        #endregion

                        if (!string.IsNullOrEmpty(MaBN)) //Kiểm tra dữ liệu bệnh nhân đã được insert vào db chưa?
                        {
                            #region Xử lý thông tin phiếu
                            if (string.IsNullOrEmpty(dg.Phieu.maPhieu)) //Nếu mã phiếu không tồn tại thì tạo phiếu mới
                            {
                                result.StringError = "Mã phiếu không được để trống";
                                throw new Exception("Mã phiếu không được để trống");

                            }
                            else //nếu tồn tại mã phiếu thì...
                            {
                                var phieu = db.PSPhieuSangLocs.FirstOrDefault(p => p.IDCoSo == dg.MaDonVi && p.IDPhieu == dg.MaPhieu && p.isXoa == false);
                                if (phieu != null) //kiểm tra phiếu đã nằm trong db hay chưa, nếu rồi thì update
                                {
                                    #region Update thông tin phiếu
                                    phieu.CheDoDinhDuong = dg.Phieu.maCheDoDinhDuong;
                                    phieu.IDChuongTrinh = dg.Phieu.maChuongTrinh;
                                    phieu.IDCoSo = dg.Phieu.maDonViCoSo;
                                    phieu.IDNhanVienLayMau = dg.Phieu.maNVLayMau;
                                    if (string.IsNullOrEmpty(phieu.IDNhanVienTaoPhieu))
                                        phieu.IDNhanVienTaoPhieu = dg.Phieu.maNVTaoPhieu;
                                    phieu.IDPhieu = dg.Phieu.maPhieu;
                                    phieu.IDPhieuLan1 = dg.Phieu.maPhieuLan1;
                                    phieu.IDViTriLayMau = dg.Phieu.idViTriLayMau;
                                    phieu.isGuiMauTre = dg.Phieu.isGuiMauTre;
                                    phieu.isHuyMau = false;
                                    if (!string.IsNullOrEmpty(phieu.IDPhieuLan1))
                                    {
                                        phieu.isLayMauLan2 = true;
                                    }
                                    else phieu.isLayMauLan2 = false;
                                    phieu.isKhongDat = dg.Phieu.isKhongDat;
                                    phieu.isNheCan = dg.Phieu.isNheCan;
                                    phieu.isSinhNon = dg.Phieu.isSinhNon;
                                    phieu.isTruoc24h = dg.Phieu.isTruoc24h;
                                    phieu.MaBenhNhan = MaBN;
                                    phieu.Para = dg.Phieu.BenhNhan.Para ?? "0000";
                                    if (phieu.TrangThaiMau < 3)
                                        phieu.MaGoiXN = dg.Phieu.maGoiXetNghiem;
                                    if (string.IsNullOrEmpty(phieu.MaXetNghiem))
                                        phieu.MaXetNghiem = dg.Phieu.maXetNghiem;
                                    phieu.NgayGioLayMau = dg.Phieu.ngayGioLayMau;
                                    phieu.NgayNhanMau = dg.Phieu.ngayNhanMau;
                                    //phieu.NgayTaoPhieu = dg.Phieu.ngayTaoPhieu;
                                    phieu.NgayTruyenMau = dg.Phieu.ngayTruyenMau;
                                    phieu.NoiLayMau = dg.Phieu.NoiLayMau;
                                    phieu.DiaChiLayMau = dg.Phieu.DiaChiLayMau;
                                    phieu.Para = dg.Phieu.paRa;
                                    phieu.SDTNhanVienLayMau = dg.Phieu.SoDTNhanVienLayMau;
                                    phieu.SLTruyenMau = dg.Phieu.soLuongTruyenMau;
                                    phieu.TenNhanVienLayMau = dg.Phieu.TenNhanVienLayMau;
                                    phieu.TinhTrangLucLayMau = dg.Phieu.maTinhTrangLucLayMau;
                                    if (phieu.TrangThaiMau < 2)
                                        phieu.TrangThaiMau = 2;
                                    phieu.LuuYPhieu = dg.Phieu.LuuYPhieu != null ? dg.Phieu.LuuYPhieu.TrimEnd() : "";
                                    phieu.isDongBo = false;
                                    phieu.isXoa = false;
                                    phieu.TrangThaiPhieu = true;
                                    phieu.LyDoKhongDat = dg.Phieu.lydokhongdat;
                                    db.SubmitChanges();
                                    #endregion Update thông tin phiếu
                                }
                                else // nếu chưa thì tạo phiếu và insert vào db
                                {
                                    #region Tạo phiếu mới theo thông tin phiếu của đợt chỉ định
                                    PSPhieuSangLoc p = new PSPhieuSangLoc();
                                    p.LuuYPhieu = dg.Phieu.LuuYPhieu != null ? dg.Phieu.LuuYPhieu.TrimEnd() : "";
                                    p.CheDoDinhDuong = dg.Phieu.maCheDoDinhDuong;
                                    p.IDChuongTrinh = dg.Phieu.maChuongTrinh;
                                    p.IDCoSo = dg.Phieu.maDonViCoSo;
                                    p.IDNhanVienLayMau = dg.Phieu.maNVLayMau;
                                    p.IDNhanVienTaoPhieu = dg.Phieu.maNVTaoPhieu;
                                    p.IDPhieu = dg.Phieu.maPhieu;
                                    p.IDPhieuLan1 = dg.Phieu.maPhieuLan1;
                                    p.IDViTriLayMau = dg.Phieu.idViTriLayMau;
                                    p.isGuiMauTre = dg.Phieu.isGuiMauTre;
                                    p.isHuyMau = false;
                                    p.isKhongDat = dg.Phieu.isKhongDat;
                                    if (!string.IsNullOrEmpty(p.IDPhieuLan1))
                                    {
                                        p.isLayMauLan2 = true;
                                    }
                                    else p.isLayMauLan2 = false;
                                    p.isNheCan = dg.Phieu.isNheCan;
                                    p.isXNLan2 = false;
                                    p.isSinhNon = dg.Phieu.isSinhNon;
                                    p.isTruoc24h = dg.Phieu.isTruoc24h;
                                    p.MaBenhNhan = MaBN;
                                    p.MaGoiXN = dg.Phieu.maGoiXetNghiem;
                                    p.MaXetNghiem = dg.Phieu.maXetNghiem;
                                    p.NgayGioLayMau = dg.Phieu.ngayGioLayMau;
                                    p.NgayNhanMau = dg.Phieu.ngayNhanMau;
                                    p.NgayTaoPhieu = dg.Phieu.ngayTaoPhieu;
                                    p.NgayTruyenMau = dg.Phieu.ngayTruyenMau;
                                    p.NoiLayMau = dg.Phieu.NoiLayMau;
                                    p.DiaChiLayMau = dg.Phieu.DiaChiLayMau;
                                    p.Para = dg.Phieu.paRa;
                                    p.SDTNhanVienLayMau = dg.Phieu.SoDTNhanVienLayMau;
                                    p.SLTruyenMau = dg.Phieu.soLuongTruyenMau;
                                    p.TenNhanVienLayMau = dg.Phieu.TenNhanVienLayMau;
                                    p.TinhTrangLucLayMau = dg.Phieu.maTinhTrangLucLayMau;
                                    p.TrangThaiMau = 2;
                                    p.Para = dg.Phieu.BenhNhan.Para ?? "0000";
                                    p.TrangThaiPhieu = true;
                                    p.isDongBo = false;
                                    p.isXoa = false;
                                    p.LyDoKhongDat = dg.Phieu.lydokhongdat;
                                    db.PSPhieuSangLocs.InsertOnSubmit(p);
                                    db.SubmitChanges();//gan mPh = maphieu
                                    #endregion Tạo  phiếu mới theo thông tin phiếu của đợt chỉ định
                                }
                            }
                            #endregion Xử lý thông tin phiếu
                        }
                        else //Nếu Thông tin bệnh nhân chưa được insert vào db thì báo lỗi
                        {
                            result.Result = false;
                            result.StringError = "Không lưu được thông tin bệnh nhân theo phiếu.";
                            throw new Exception();
                        }
                    }
                    else // Nếu không có thông tin phiếu thì tạo 1 phiếu mới + bệnh nhân theo phiếu
                    {
                        #region Nếu phiếu ko chứa dữ liệu
                        #region Insert Bệnh nhân mới Theo mã tự tạo của Phần mềm
                        PSPatient patient = new PSPatient();
                        MaBN = GetID();
                        patient.MaBenhNhan = MaBN;
                        patient.FatherName = dg.Phieu.BenhNhan.FatherName;
                        patient.FatherPhoneNumber = dg.Phieu.BenhNhan.FatherPhoneNumber;
                        patient.FatherBirthday = dg.Phieu.BenhNhan.FatherBirthday;
                        patient.MotherBirthday = dg.Phieu.BenhNhan.MotherBirthday;
                        patient.MotherPhoneNumber = dg.Phieu.BenhNhan.MotherPhoneNumber;
                        patient.MotherName = dg.Phieu.BenhNhan.MotherName;
                        patient.DiaChi = dg.Phieu.BenhNhan.DiaChi;
                        patient.TenBenhNhan = dg.Phieu.BenhNhan.TenBenhNhan;
                        patient.QuocTichID = dg.Phieu.BenhNhan.QuocTichID;
                        patient.TuanTuoiKhiSinh = dg.Phieu.BenhNhan.TuanTuoiKhiSinh;
                        patient.PhuongPhapSinh = dg.Phieu.BenhNhan.PhuongPhapSinh;
                        patient.NoiSinh = dg.Phieu.BenhNhan.NoiSinh;
                        patient.NgayGioSinh = dg.Phieu.BenhNhan.NgayGioSinh;
                        patient.GioiTinh = dg.Phieu.BenhNhan.GioiTinh;
                        patient.DanTocID = dg.Phieu.BenhNhan.DanTocID;
                        patient.CanNang = dg.Phieu.BenhNhan.CanNang;
                        patient.IDThaiPhuTienSoSinh = dg.Phieu.BenhNhan.IDThaiPhuTienSoSinh ?? string.Empty;
                        patient.isDongBo = false;
                        patient.isXoa = false;
                        patient.MaKhachHang = GetNewMaKhachHang(dg.MaDonVi, SoBanDau());
                        patient.Para = dg.Phieu.BenhNhan.Para ?? "0000";
                        db.PSPatients.InsertOnSubmit(patient);
                        db.SubmitChanges();
                        #endregion Insert Bệnh nhân mới Theo mã tự tạo của Phần mềm
                        #region Insert một phiếu mới
                        PSPhieuSangLoc p = new PSPhieuSangLoc();
                        p.CheDoDinhDuong = dg.Phieu.maCheDoDinhDuong;
                        p.IDChuongTrinh = dg.Phieu.maChuongTrinh;
                        p.IDCoSo = dg.Phieu.maDonViCoSo;
                        p.IDNhanVienLayMau = dg.Phieu.maNVLayMau;
                        p.IDNhanVienTaoPhieu = dg.Phieu.maNVTaoPhieu;
                        p.IDPhieu = dg.Phieu.maPhieu;
                        p.IDPhieuLan1 = dg.Phieu.maPhieuLan1;
                        p.IDViTriLayMau = dg.Phieu.idViTriLayMau;
                        p.isGuiMauTre = dg.Phieu.isGuiMauTre;
                        p.isHuyMau = false;
                        p.isKhongDat = dg.Phieu.isKhongDat;
                        if (!string.IsNullOrEmpty(p.IDPhieuLan1))
                            p.isLayMauLan2 = true;
                        else p.isLayMauLan2 = false;
                        p.isNheCan = dg.Phieu.isNheCan;
                        p.isSinhNon = dg.Phieu.isSinhNon;
                        p.isTruoc24h = dg.Phieu.isTruoc24h;
                        p.MaBenhNhan = MaBN;
                        p.MaGoiXN = dg.Phieu.maGoiXetNghiem;
                        p.MaXetNghiem = dg.Phieu.maXetNghiem;
                        p.NgayGioLayMau = dg.Phieu.ngayGioLayMau;
                        p.NgayNhanMau = dg.Phieu.ngayNhanMau;
                        p.NgayTaoPhieu = dg.Phieu.ngayTaoPhieu;
                        p.NgayTruyenMau = dg.Phieu.ngayTruyenMau;
                        p.NoiLayMau = dg.Phieu.NoiLayMau;
                        p.DiaChiLayMau = dg.Phieu.DiaChiLayMau;
                        p.Para = dg.Phieu.paRa;
                        p.SDTNhanVienLayMau = dg.Phieu.SoDTNhanVienLayMau;
                        p.SLTruyenMau = dg.Phieu.soLuongTruyenMau;
                        p.TenNhanVienLayMau = dg.Phieu.TenNhanVienLayMau;
                        p.TinhTrangLucLayMau = dg.Phieu.maTinhTrangLucLayMau;
                        p.TrangThaiMau = 2;
                        p.TrangThaiPhieu = true;
                        p.LyDoKhongDat = dg.Phieu.lydokhongdat;
                        p.isDongBo = false;
                        p.isXoa = false;
                        db.PSPhieuSangLocs.InsertOnSubmit(p);
                        db.SubmitChanges();
                        #endregion Insert một phiếu mới
                        #endregion Nếu phiếu ko chứa dữ liệu
                    }

                    var lstld = db.PSChiTietDanhGiaChatLuongs.Where(p => p.MaTiepNhan == dg.MaTiepNhan && p.IDPhieu == dg.MaPhieu).ToList();
                    if (lstld.Count <= 0)
                    {
                        if (dg.Phieu.lstLyDoKhongDat.Count > 0)
                        {
                            foreach (var ld in dg.Phieu.lstLyDoKhongDat)
                            {
                                ld.isDongBo = false;
                                ld.isXoa = false;
                                db.PSChiTietDanhGiaChatLuongs.InsertOnSubmit(ld);
                                db.SubmitChanges();
                            }
                        }
                    }
                    else
                    {
                        db.PSChiTietDanhGiaChatLuongs.DeleteAllOnSubmit(lstld);
                        db.SubmitChanges();
                        if (dg.Phieu.lstLyDoKhongDat.Count > 0)
                        {
                            foreach (var ld in dg.Phieu.lstLyDoKhongDat)
                            {
                                ld.isDongBo = false;
                                ld.isXoa = false;
                                db.PSChiTietDanhGiaChatLuongs.InsertOnSubmit(ld);
                                db.SubmitChanges();
                            }
                        }
                    }


                    string mCD = string.Empty;
                    #region Xử lý dữ liệu của đợt chỉ định
                    if ((db.PSPhieuSangLocs.FirstOrDefault(p => p.IDPhieu == dg.MaPhieu && p.isXoa == false).TrangThaiMau ?? 0) < 3)
                    {
                        decimal dongia = 0;
                        var goidv = db.PSDanhMucGoiDichVuTheoDonVis.FirstOrDefault(p => p.IDGoiDichVuChung == dg.MaGoiDichVu && p.MaDVCS == dg.MaDonVi);
                        if (goidv != null)
                            dongia = goidv.DonGia ?? 0;
                        if (string.IsNullOrEmpty(dg.MaChiDinh))
                        {
                            var cd = db.PSChiDinhDichVus.FirstOrDefault(p => p.isXoa == false && p.MaPhieu == dg.MaPhieu && p.MaTiepNhan == dg.MaTiepNhan);
                            if (cd != null)
                            {
                                #region update đợt chỉ định 
                                cd.IDGoiDichVu = dg.MaGoiDichVu;
                                if (string.IsNullOrEmpty(cd.IDNhanVienChiDinh))
                                    cd.IDNhanVienChiDinh = dg.MaNVChiDinh;
                                cd.isLayMauLai = dg.isLayMauLai;
                                //cd.MaChiDinh = dg.MaChiDinh;
                                cd.MaDonVi = dg.MaDonVi;
                                cd.isDaNhapLieu = isNhapLieu;
                                cd.MaNVChiDinh = dg.MaNVChiDinh;
                                cd.MaPhieu = dg.Phieu.maPhieu;
                                cd.MaPhieuLan1 = dg.MaPhieuLan1;
                                cd.NgayChiDinhHienTai = cd.NgayChiDinhHienTai == null ? dg.NgayChiDinhHienTai : cd.NgayChiDinhHienTai;
                                cd.NgayChiDinhLamViec = cd.NgayChiDinhLamViec == null ? dg.NgayChiDinhLamViec : cd.NgayChiDinhLamViec;
                                cd.SoLuong = dg.SoLuong;
                                if (cd.DonGia == null)
                                    cd.DonGia = dongia;
                                else if (cd.DonGia == 0)
                                    cd.DonGia = dongia;
                                cd.MaTiepNhan = dg.MaTiepNhan;
                                //if(cd.TrangThai<1)
                                //cd.TrangThai = dg.TrangThaiChiDinh;
                                cd.NgayTiepNhan = dg.NgayTiepNhan;
                                cd.isDongBo = false;
                                cd.isXoa = false;

                                db.SubmitChanges();
                                mCD = cd.MaChiDinh;
                                #endregion update đợt chỉ định 
                            }
                            else
                            {
                                #region Insert đợt chỉ định mới
                                PSChiDinhDichVu cdi = new PSChiDinhDichVu();
                                cdi.IDGoiDichVu = dg.MaGoiDichVu;
                                cdi.IDNhanVienChiDinh = dg.MaNVChiDinh;
                                cdi.isLayMauLai = dg.isLayMauLai;
                                cdi.MaChiDinh = "CD" + GetID();
                                cdi.MaDonVi = dg.MaDonVi;
                                cdi.isDaNhapLieu = isNhapLieu;
                                cdi.MaNVChiDinh = dg.MaNVChiDinh;
                                cdi.MaPhieu = dg.Phieu.maPhieu;
                                cdi.MaPhieuLan1 = dg.MaPhieuLan1;
                                cdi.NgayChiDinhHienTai = dg.NgayChiDinhHienTai;
                                cdi.NgayChiDinhLamViec = dg.NgayChiDinhLamViec;
                                cdi.SoLuong = dg.SoLuong;
                                if (cdi.DonGia == null)
                                    cdi.DonGia = dongia;
                                else if (cdi.DonGia == 0)
                                    cdi.DonGia = dongia;
                                cdi.MaTiepNhan = dg.MaTiepNhan;
                                cdi.TrangThai = 1;
                                cdi.NgayTiepNhan = dg.NgayTiepNhan;
                                cdi.isDongBo = false;
                                cdi.isXoa = false;
                                db.PSChiDinhDichVus.InsertOnSubmit(cdi);
                                db.SubmitChanges();
                                mCD = cdi.MaChiDinh;
                            }
                            #endregion Insert đợt chỉ định mới
                        }
                        else
                        {
                            var cd = db.PSChiDinhDichVus.FirstOrDefault(p => p.isXoa == false && p.MaChiDinh == dg.MaChiDinh);
                            if (cd != null)
                            {
                                #region update đợt chỉ định 
                                cd.IDGoiDichVu = dg.MaGoiDichVu;
                                if (string.IsNullOrEmpty(cd.IDNhanVienChiDinh))
                                    cd.IDNhanVienChiDinh = dg.MaNVChiDinh;
                                cd.isLayMauLai = dg.isLayMauLai;
                                cd.MaChiDinh = dg.MaChiDinh;
                                cd.MaDonVi = dg.MaDonVi;
                                cd.isDaNhapLieu = isNhapLieu;
                                cd.MaNVChiDinh = dg.MaNVChiDinh;
                                cd.MaPhieu = dg.Phieu.maPhieu;
                                cd.MaPhieuLan1 = dg.MaPhieuLan1;
                                cd.NgayChiDinhHienTai = cd.NgayChiDinhHienTai == null ? dg.NgayChiDinhHienTai : cd.NgayChiDinhHienTai;
                                cd.NgayChiDinhLamViec = cd.NgayChiDinhLamViec == null ? dg.NgayChiDinhLamViec : cd.NgayChiDinhLamViec;
                                cd.SoLuong = dg.SoLuong;
                                //if (cd.DonGia == null)
                                //    cd.DonGia = dongia;
                                //else if (cd.DonGia == 0)
                                //    cd.DonGia = dongia;
                                cd.MaTiepNhan = dg.MaTiepNhan;
                                //if(cd.TrangThai<1)
                                //cd.TrangThai = dg.TrangThaiChiDinh;
                                cd.NgayTiepNhan = dg.NgayTiepNhan;
                                cd.isDongBo = false;
                                cd.isXoa = false;
                                db.SubmitChanges();
                                mCD = cd.MaChiDinh;
                                #endregion update đợt chỉ định 
                            }
                            else
                            {
                                #region Insert đợt chỉ định mới
                                PSChiDinhDichVu cdi = new PSChiDinhDichVu();
                                cdi.IDGoiDichVu = dg.MaGoiDichVu;
                                cdi.IDNhanVienChiDinh = dg.MaNVChiDinh;
                                cdi.isLayMauLai = dg.isLayMauLai;
                                cdi.MaChiDinh = dg.MaChiDinh;
                                cdi.MaDonVi = dg.MaDonVi;
                                cdi.isDaNhapLieu = isNhapLieu;
                                cdi.MaNVChiDinh = dg.MaNVChiDinh;
                                cdi.MaPhieu = dg.Phieu.maPhieu;
                                cdi.MaPhieuLan1 = dg.MaPhieuLan1;
                                cdi.NgayChiDinhHienTai = dg.NgayChiDinhHienTai;
                                cdi.NgayChiDinhLamViec = dg.NgayChiDinhLamViec;
                                cdi.SoLuong = dg.SoLuong;
                                cdi.MaTiepNhan = dg.MaTiepNhan;
                                cdi.TrangThai = 1;
                                cdi.NgayTiepNhan = dg.NgayTiepNhan;
                                if (cd.DonGia == null)
                                    cd.DonGia = dongia;
                                else if (cd.DonGia == 0)
                                    cd.DonGia = dongia;
                                cdi.isDongBo = false;
                                cdi.isXoa = false;
                                db.PSChiDinhDichVus.InsertOnSubmit(cdi);
                                db.SubmitChanges();
                                mCD = dg.MaChiDinh;
                                #endregion Insert đợt chỉ định mới
                            }
                        }
                        if (!string.IsNullOrEmpty(mCD))
                        {
                            //******Kiểm tra lại chi tiết các gói dịch vụ đã chỉ định cho trường hợp chỉ định dịch vụ thay đổi gói xét nghiệm
                            // 1 --Lấy tất cả các chi tiết dv của phiếu đã đc chỉ định
                            // 2 --Kiểm tra 
                            // *** LƯU Ý : quá trình cập nhật lại(xóa hoặc thêm ) chi tiết list dịch vụ này thì kiểm tra tình trạng mẫu của mã phiếu trước ( nếu <3 [chưa vào phòng XN] ) thì mới đc thực hiện.
                            List<PSChiDinhDichVuChiTiet> lstdvchitiet = new List<PSChiDinhDichVuChiTiet>();
                            foreach (var itemdv in dg.lstDichVu)
                            {
                                PSChiDinhDichVuChiTiet dv = new PSChiDinhDichVuChiTiet();
                                dv.GiaTien = itemdv.GiaDichVu;
                                dv.isXetNghiemLan2 = false;
                                dv.MaChiDinh = mCD;
                                dv.MaDichVu = itemdv.IDDichVu;
                                dv.MaDonVi = dg.MaDonVi;
                                dv.MaGoiDichVu = dg.MaGoiDichVu;
                                dv.MaPhieu = dg.Phieu.maPhieu;
                                dv.SoLuong = dg.SoLuong;
                                dv.isDongBo = false;
                                dv.isXoa = false;
                                lstdvchitiet.Add(dv);
                            }
                            if (lstdvchitiet.Count > 0)
                            {
                                #region Bỏ
                                //var lstdvcu = db.PSChiDinhDichVuChiTiets.Where(p => p.MaChiDinh == mCD && p.isXoa == false).ToList();
                                //if (lstdvcu.Count > 0) //Kiểm tra nếu các chi tiết dịch vụ đã đc add rồi thì
                                //{
                                //    var lstmagoi = lstdvcu.GroupBy(p => p.MaGoiDichVu).ToList(); //chia nhóm theo gói XN
                                //    if (lstmagoi.Count > 0)
                                //    {
                                //        foreach (var goi in lstmagoi) //với mỗi nhóm
                                //        {
                                //            var lstdvcunggoiXN = lstdvcu.Where(p => p.MaGoiDichVu == dg.MaGoiDichVu).ToList();
                                //            if (dg.MaGoiDichVu == goi.Key) //kiểm tra xem có trùng với mã nhóm xét nghiệm của dữ liệu đầu vào ko?
                                //            {
                                //                //Nếu trùng

                                //                if (lstdvcunggoiXN.Count == dg.lstDichVu.Count) //Kiểm tra số lượng các dịch vụ đi theo gói XN đó có bằng với số lượng dịch vụ dữ liệu đầu vào ko?
                                //                {   //Nếu bằng số lượng thì... 
                                //                    var listGiaonnhautrongdb = lstdvcu.Intersect(lstdvchitiet).ToList();//lấy giá trị giao nhau giữa 2 list
                                //                    if (listGiaonnhautrongdb.Count != lstdvcunggoiXN.Count) // kiểm tra chi tiết trong gói dịch vụ đã add có trùng với tất cả chi tiết dịch vụ của dữ liệu đầu vào ko?
                                //                    {
                                //                        //nếu có bất cứ 1 mẫu ko trùng thì xóa hết list dv đang nằm trong  db
                                //                        foreach (var item in lstdvcunggoiXN)
                                //                        {
                                //                            item.isXoa = true;
                                //                            item.isDongBo = false;
                                //                            db.SubmitChanges();
                                //                        }
                                //                        //foreach (var itemadd in lstdvchitiet) // và add mới
                                //                        //{
                                //                        //    db.PSChiDinhDichVuChiTiets.InsertOnSubmit(itemadd);
                                //                        //    db.SubmitChanges();
                                //                        //}
                                //                    }
                                //                     // nếu như list dv trong db trùng với list dv của dữ liệu đầu vào thì thôi
                                //                }
                                //                else // Nếu số lượng ko bằng
                                //                {
                                //                    foreach( var item in lstdvcunggoiXN) //xóa hết các
                                //                    {
                                //                        item.isXoa = true;
                                //                        item.isDongBo = false;
                                //                        db.SubmitChanges();
                                //                    }
                                //                    //foreach (var itemadd in lstdvchitiet) // và add mới
                                //                    //{
                                //                    //    db.PSChiDinhDichVuChiTiets.InsertOnSubmit(itemadd);
                                //                    //    db.SubmitChanges();
                                //                    //}
                                //                }
                                //            }
                                //            else
                                //            {
                                //                //  var lstcanxoa = db.PSChiDinhDichVuChiTiets.Where(p => p.MaChiDinh == mCD && p.isXoa == false && p.MaGoiDichVu == goi.Key).ToList();
                                //                if (lstdvcunggoiXN.Count > 0)
                                //                {
                                //                    foreach (var item in lstdvcunggoiXN)
                                //                    {
                                //                        item.isXoa = true;
                                //                        item.isDongBo = false;
                                //                        db.SubmitChanges();
                                //                    }
                                //                    //foreach (var itemadd in lstdvchitiet) // và add mới
                                //                    //{
                                //                    //    db.PSChiDinhDichVuChiTiets.InsertOnSubmit(itemadd);
                                //                    //    db.SubmitChanges();
                                //                    //}

                                //                }
                                //            }
                                //        }
                                //    }
                                //}
                                //else {
                                #endregion bỏ
                                if ((db.PSPhieuSangLocs.FirstOrDefault(p => p.isXoa == false && p.IDPhieu == dg.MaPhieu).TrangThaiMau ?? 1) < 3)
                                {
                                    var lstdvcu = db.PSChiDinhDichVuChiTiets.Where(p => p.MaChiDinh == mCD && p.isXoa == false).ToList();
                                    if (lstdvcu.Count > 0) //Kiểm tra nếu các chi tiết dịch vụ đã đc add rồi thì xóa
                                    {
                                        //foreach (var dv in lstdvcu)
                                        //{
                                        //    dv.isDongBo = false;
                                        //    dv.isXoa = true;
                                        //    db.SubmitChanges();
                                        //}
                                        db.PSChiDinhDichVuChiTiets.DeleteAllOnSubmit(lstdvcu);
                                        db.SubmitChanges();
                                    }
                                    //insert chi tiết chi định
                                    //foreach (var dichvu in dg.lstDichVu)
                                    //{
                                    //    var dvu = db.PSChiDinhDichVuChiTiets.FirstOrDefault(p => p.MaChiDinh == mCD & p.MaDichVu == dichvu.IDDichVu && p.isXoa == false);
                                    //    if (dvu == null)
                                    //    {
                                    foreach (var item in lstdvchitiet)
                                    {
                                        db.PSChiDinhDichVuChiTiets.InsertOnSubmit(item);
                                        db.SubmitChanges();
                                    }
                                    //    }
                                    //}
                                }

                            }
                            else
                            {
                                //rollback
                                result.Result = false;
                                result.StringError = "Không tìm thấy thông tin dịch vụ trong gói xét nghiệm\r\n Vui lòng kiểm tra và thử lại.";
                                throw new Exception("Không tìm thấy thông tin dịch vụ trong gói xét nghiệm\r\n Vui lòng kiểm tra và thử lại.");
                            }

                        }
                        else
                        {
                            //rollback
                            result.Result = false;
                            result.StringError = "Không thể thêm mới đợt chỉ định này.";
                            throw new Exception("Không thể thêm mới đợt chỉ định này.");
                        }
                    }
                    else
                    {
                        var cd = db.PSChiDinhDichVus.FirstOrDefault(p => p.isXoa == false && p.MaPhieu == dg.MaPhieu && p.MaTiepNhan == dg.MaTiepNhan);
                        if (cd != null)
                        {
                            if (string.IsNullOrEmpty(cd.IDNhanVienChiDinh))
                                cd.IDNhanVienChiDinh = dg.MaNVChiDinh;
                            cd.isLayMauLai = dg.isLayMauLai;
                            //cd.MaChiDinh = dg.MaChiDinh;
                            cd.MaDonVi = dg.MaDonVi;
                            cd.isDaNhapLieu = isNhapLieu;
                            cd.MaNVChiDinh = dg.MaNVChiDinh;
                            cd.MaPhieu = dg.Phieu.maPhieu;
                            cd.MaPhieuLan1 = dg.MaPhieuLan1;
                            cd.NgayChiDinhHienTai = dg.NgayChiDinhHienTai;
                            //  cd.NgayChiDinhLamViec = dg.NgayChiDinhLamViec;
                            cd.SoLuong = dg.SoLuong;
                            cd.MaTiepNhan = dg.MaTiepNhan;
                            //if(cd.TrangThai<1)
                            //cd.TrangThai = dg.TrangThaiChiDinh;
                            cd.NgayTiepNhan = dg.NgayTiepNhan;
                            cd.isDongBo = false;
                            cd.isXoa = false;
                            db.SubmitChanges();
                        }
                    }


                    var tn = db.PSTiepNhans.Where(p => p.isXoa == false && p.MaPhieu == dg.MaPhieu && p.MaDonVi == dg.MaDonVi).ToList();
                    //if (isNhapLieu)
                    //{
                    if (tn.Count > 0)
                    {

                        foreach (var item in tn)
                        {
                            item.isDaNhapLieu = isNhapLieu;
                            item.isDaDanhGia = true;
                            item.isDongBo = false;
                            db.SubmitChanges();
                        }
                    }
                    //}
                    var ph = db.PSPhieuSangLocs.Where(p => p.isXoa == false && p.IDPhieu == dg.MaPhieu && p.IDCoSo == dg.MaDonVi).ToList();
                    if (ph.Count > 0)
                    {
                        foreach (var item in ph)
                        {
                            item.isXoa = false;
                            item.isDongBo = false;
                            item.TrangThaiPhieu = true;
                            if (item.TrangThaiMau < 3)//nếu mẫu chưa vào phòng xét nghiệm thì cập nhật trạng thái mẫu =2 (mẫu đã đc đánh giá)
                                item.TrangThaiMau = 2;
                            db.SubmitChanges();
                        }
                    }
                    if (!string.IsNullOrEmpty(dg.Phieu.maPhieuLan1))
                    {
                        try
                        {
                            var phieucu = db.PSPhieuSangLocs.FirstOrDefault(p => p.IDPhieu == dg.Phieu.maPhieuLan1 && p.isXoa == false);
                            if (phieucu != null)
                            {
                                phieucu.TrangThaiMau = 4;
                                phieucu.isDongBo = false;
                                db.SubmitChanges();
                            }
                        }
                        catch { }
                    }

                    #endregion Xử lý dữ liệu của đợt chỉ định
                    db.Transaction.Commit();
                    db.Connection.Close();
                }

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
        public PsReponse UpdateThongTinPhieuLan1(string maphieulan1)
        {
            PsReponse result = new PsReponse();
            try
            {
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();
                var tt = db.PSPhieuSangLocs.FirstOrDefault(x => x.IDPhieu == maphieulan1);
                if (tt != null)
                {
                    tt.TrangThaiMau = 7;
                    db.SubmitChanges();
                }
                result.Result = true;
                db.Transaction.Commit();
                db.Connection.Close();
            }
            catch
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                result.Result = false;
            }
            return result;
        }
        public PsReponse UpdateThongTinTrungTam(PSThongTinTrungTam tt)
        {
            PsReponse result = new PsReponse();
            try
            {
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();
                var trtam = db.PSThongTinTrungTams.FirstOrDefault();
                if (trtam != null)
                {
                    trtam.Diachi = tt.Diachi;
                    trtam.DienThoai = tt.DienThoai;
                    trtam.isChoThuLaiMauLan2 = tt.isChoThuLaiMauLan2;
                    trtam.isChoXNLan2 = tt.isChoXNLan2;
                    trtam.TenTrungTam = tt.TenTrungTam;
                    trtam.isCapMaXNTheoMaPhieu = tt.isCapMaXNTheoMaPhieu;
                    trtam.Email = tt.Email;
                    trtam.PassEmail = tt.PassEmail;
                    trtam.EmailTC = tt.EmailTC;
                    trtam.PassEmailTC = tt.PassEmailTC;
                    trtam.Logo = tt.Logo;
                    trtam.Header = tt.Header;
                    trtam.ChuKiTT = tt.ChuKiTT;
                    trtam.ChuKiXN = tt.ChuKiXN;
                    db.SubmitChanges();
                    result.Result = true;
                }
                else
                {
                    result.Result = false;
                    result.StringError = "Không có thông tin trung tâm. Vui lòng liên hệ với người quản trị để cài đặt cấu hình ban đầu!";
                }

                db.Transaction.Commit();
                db.Connection.Close();

            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                result.Result = false;
                result.StringError = ex.ToString();
            }
            return result;
        }
        public PsReponse UpdateGhiChuXetNghiem(string maKQ, string ghiChu)
        {
            PsReponse result = new PsReponse();
            try
            {
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();
                var res = db.PSXN_KetQuas.FirstOrDefault(p => p.isXoa == false && p.MaKetQua == maKQ);
                if (res != null)
                {
                    res.GhiChu = ghiChu;
                    db.SubmitChanges();
                }

                db.Transaction.Commit();
                db.Connection.Close();
                result.Result = true;
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                result.Result = false;
                result.StringError = ex.ToString();
            }
            return result;

        }
        public string GetGhiChuXetNghiem(string maKQ)
        {
            string ghichu = null;
            try
            {
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();
                var res = db.PSXN_KetQuas.FirstOrDefault(p => p.isXoa == false && p.MaKetQua == maKQ);
                if (res != null)
                {
                    ghichu = res.GhiChu;

                }


            }
            catch (Exception ex)
            {

            }
            return ghichu;

        }

        public PsReponse UpdateDanhMucGhiChu(PSDanhMucGhiChu ghichu)
        {
            PsReponse result = new PsReponse();
            result.Result = true;
            try
            {
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();
                var res = db.PSDanhMucGhiChus.Where(p => p.MaGhiChu == ghichu.MaGhiChu).FirstOrDefault();
                if (res != null)
                {
                    res.ThongTinHienThiGhiChu = ghichu.ThongTinHienThiGhiChu;
                    res.isNoiDungDatTruoc = ghichu.isNoiDungDatTruoc;
                    db.SubmitChanges();
                }
                else
                {
                    result.Result = false;
                    result.StringError = "Không tồn tại trường hợp ghi chú này";
                }
                db.Transaction.Commit();
                db.Connection.Close();
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                result.Result = false;
                result.StringError = ex.ToString();
            }
            return result;


        }
        public PsReponse InsertDotChanDoan(PSDotChuanDoan dotChanDoan)
        {
            PsReponse result = new PsReponse();
            try
            {
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();

                if (dotChanDoan != null)
                {
                    var dot = db.PSDotChuanDoans.FirstOrDefault(p => p.isXoa == false && p.rowIDDotChanDoan == dotChanDoan.rowIDDotChanDoan);
                    var benhnhannguycocao = db.PSBenhNhanNguyCoCaos.FirstOrDefault(p => p.isXoa == false && p.MaKhachHang == dotChanDoan.MaKhachHang);
                    if (dot != null)
                    {
                        dot.GhiChu = dotChanDoan.GhiChu;
                        dot.KetQua = dotChanDoan.KetQua;
                        dot.ChanDoan = dotChanDoan.ChanDoan;
                        if (benhnhannguycocao != null)
                        {
                            if (benhnhannguycocao.isDaChanDoan == false)
                            {
                                benhnhannguycocao.isDaChanDoan = true;
                            }
                        }
                        db.SubmitChanges();
                    }
                    else
                    {

                        if (benhnhannguycocao != null)
                        {
                            if (benhnhannguycocao.isDaChanDoan == false)
                            {
                                benhnhannguycocao.isDaChanDoan = true;
                            }
                        }
                        dotChanDoan.MaDotChuanDoan = "1B1" + Guid.NewGuid().ToString();
                        db.PSDotChuanDoans.InsertOnSubmit(dotChanDoan);
                        db.SubmitChanges();
                    }
                }
                db.Transaction.Commit();
                db.Connection.Close();
                result.Result = true;
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                result.Result = false;
                result.StringError = ex.ToString();
            }
            return result;
        }
        public PsReponse UpdateTrangThaiBenhNhanNguyCoCao(bool isNguyCo, long rowIDBN)
        {
            PsReponse result = new PsReponse();
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                var bn = db.PSBenhNhanNguyCoCaos.FirstOrDefault(p => p.isXoa == false && p.rowIDBenhNhanCanTheoDoi == rowIDBN);
                if (bn != null)
                {
                    bn.isNguyCoCao = isNguyCo;
                    bn.isXoa = false;
                    bn.isDongBo = false;
                    db.SubmitChanges();
                    result.Result = true;
                }
                else
                {
                    result.Result = false;
                    result.StringError = "Không tìm thấy thông tin bệnh nhân!";
                }
                db.Transaction.Commit();
                db.Connection.Close();

            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                result.Result = false;
                result.StringError = ex.ToString();
            }
            return result;
        }
        public bool UpdatePhieuTraKetQuaChoXNLan2(string maPhieu, string maTiepNhan, string maDonVi)
        {
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                var kq = db.PSXN_TraKetQuas.FirstOrDefault(p => p.isXoa == false && p.MaPhieu == maPhieu && p.MaTiepNhan == maTiepNhan);
                if (kq != null)
                {
                    kq.isDaDuyetKQ = true;
                    kq.isTraKQ = false;
                    kq.isDongBo = false;
                    kq.isDongBoPDF = false;
                    db.SubmitChanges();
                }
                var resPhieu = db.PSPhieuSangLocs.FirstOrDefault(p => p.isXoa != true && p.IDCoSo == maDonVi && p.IDPhieu == maPhieu);
                if (resPhieu != null)
                {
                    resPhieu.TrangThaiMau = 5;
                    resPhieu.isXNLan2 = true;
                    resPhieu.NgayCoKQ = null;
                    resPhieu.isDongBo = false;
                    db.SubmitChanges();
                }
                string pathpdf = Application.StartupPath + "\\PhieuKetQua\\" + maDonVi + "\\" + maPhieu + ".pdf";
                if (File.Exists(pathpdf))
                {
                    File.Delete(pathpdf);
                }
                db.Transaction.Commit();
                db.Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                return false;
            }
        }
        public List<PSXN_TraKetQua> GetDanhSachPDFChuaDongBo()
        {
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            List<PSXN_TraKetQua> lis = new List<PSXN_TraKetQua>();
            try
            {
                var res = db.PSXN_TraKetQuas.OrderBy(x => x.IDCoSo).Where(p => p.isXoa != true && p.isDongBoPDF != true && p.isTraKQ == true && p.isDaDuyetKQ == true).OrderBy(x => x.MaPhieu).ToList();
                if (res != null)
                {
                    return res;
                }
                else
                {
                    return lis;
                }
            }
            catch
            {
                return lis;
            }

        }
        public PsReponse UpdateDanhSachPDFChuaDongBo(List<string> Maphieu)
        {
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            PsReponse res = new PsReponse();
            try
            {
                foreach (var maphieu in Maphieu)
                {
                    var data = db.PSXN_TraKetQuas.FirstOrDefault(p => p.isXoa != true && p.MaPhieu == maphieu);
                    if (data != null)
                    {
                        data.isDongBoPDF = true;
                        db.SubmitChanges();
                    }
                }
                res.Result = true;
            }
            catch
            {
                res.Result = false;
            }
            db.Transaction.Commit();
            db.Connection.Close();
            return res;
        }

        public PsReponse UpdateDuyetKetQua(bool isDuyet, string maTiepNhan, string maDonVi, string maPhieu)
        {
            PsReponse result = new PsReponse();
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                var res = db.PSXN_TraKetQuas.FirstOrDefault(p => p.isXoa == false && p.MaTiepNhan == maTiepNhan);
                if (res != null)
                {
                    if (res.isTraKQ ?? false)
                    {
                        result.Result = false;
                        result.StringError = "Mẫu đã trả kết quả, không thể hủy duyệt!";
                        // db.Transaction.Commit();
                        db.Connection.Close();
                        return result;
                    }
                }
                else
                {
                    result.Result = false;
                    result.StringError = "Không có thông tin dữ liệu kết quả của phiếu này!";
                    //db.Transaction.Commit();
                    db.Connection.Close();
                    return result;
                }

                var phieu = db.PSPhieuSangLocs.FirstOrDefault(p => p.IDCoSo == maDonVi && p.IDPhieu == maPhieu);
                if (phieu != null)
                {
                    if ((phieu.TrangThaiMau ?? 0) > 2)
                    {
                        var lstChiDinh = db.PSChiDinhDichVus.Where(p => p.MaDonVi == maDonVi && p.MaPhieu == maPhieu && p.isXoa == false).ToList();
                        if (lstChiDinh.Count > 1)
                        {
                            result.Result = false;
                            result.StringError = "Mẫu đã được chỉ định làm lại xét nghiệm.\r\n Vui lòng liên hệ với phòng đánh giá để hủy chỉ định xét nghiệm lại!";
                            //db.Transaction.Commit();
                            db.Connection.Close();
                            return result;
                        }
                        else
                        {
                            var phieuthulai = db.PSPhieuSangLocs.FirstOrDefault(p => p.isXoa == false && p.IDPhieuLan1 == maPhieu);
                            if (phieuthulai != null)
                            {
                                if (phieuthulai.TrangThaiMau > 0)
                                {
                                    result.Result = false;
                                    result.StringError = "Mã phiếu đã được thu mẫu lại nên không thể hủy duyệt phiếu!";
                                    //db.Transaction.Commit();
                                    db.Connection.Close();
                                    return result;
                                }
                            }
                        }
                    }

                }
                else
                {
                    result.Result = false;
                    result.StringError = "Không có thông tin dữ liệu về phiếu này!";
                    //db.Transaction.Commit();
                    db.Connection.Close();
                    return result;
                }

                res.isDaDuyetKQ = isDuyet;
                res.isDongBo = false;
                db.SubmitChanges();
                db.Transaction.Commit();
                db.Connection.Close();
                result.Result = true;
                return result;
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                result.Result = false;
                result.StringError = " Lỗi trong quá trình kiểm tra điều kiện hủy" + ex.ToString();
                return result;
            }
        }
        public bool UpdateBangGhi(string maDuLieu, string GiaTri)
        {
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                var res = db.BangGhiDuLieus.FirstOrDefault(p => p.MaDuLieu == maDuLieu);
                if (res != null)
                {
                    res.DuLieu = GiaTri;
                    db.SubmitChanges();
                }
                else
                {
                    BangGhiDuLieu moi = new BangGhiDuLieu();
                    moi.MaDuLieu = maDuLieu;
                    moi.DuLieu = GiaTri;
                    db.BangGhiDuLieus.InsertOnSubmit(moi);
                    db.SubmitChanges();

                }
                db.Transaction.Commit();
                db.Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                return false;
            }
        }
        public PsReponse InsertChiDinhTheoDanhSachHangLoat(PSTiepNhan dotTiepNhan, string maNVChiDinh, string maGoiXn)
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
                                var ttdv = GetDichVuTheoDonVi(item.IDDichVu, dotTiepNhan.MaDonVi);
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

        public bool UpdateMaKhachHang(string maBenhNhan, string maDonvi, string chuoiNamThang)
        {
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                var res = db.PSPatients.FirstOrDefault(p => p.isXoa == false && p.MaBenhNhan == maBenhNhan);
                if (res != null)
                {
                    if (string.IsNullOrEmpty(res.MaKhachHang))
                    {
                        res.MaKhachHang = GetNewMaKhachHang(maDonvi, chuoiNamThang);
                        db.SubmitChanges();
                    }
                }
                db.Transaction.Commit();
                db.Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                return false;
            }
        }
        public PsReponse UpdatePhieuThucHienThuMauLai(string maPhieu, string maDonvi)
        {
            PsReponse response = new PsReponse();
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                var phieu = db.PSPhieuSangLocs.FirstOrDefault(p => p.isXoa == false && p.IDPhieu == maPhieu && p.IDCoSo == maDonvi);
                if (phieu != null)
                {
                    if (string.IsNullOrEmpty(phieu.IDPhieuLan1))
                    {
                        phieu.TrangThaiMau = 6;
                    }
                    else
                        phieu.TrangThaiMau = 4;//6
                    phieu.isDongBo = false;
                    db.SubmitChanges();
                    response.Result = true;
                }
                else
                {
                    response.Result = false;
                    response.StringError = "Không tìm thấy mã phiếu " + maPhieu + " để cập nhật trạng thái";
                }
                db.Transaction.Commit();
                db.Connection.Close();
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                response.Result = false;
                response.StringError = ex.ToString();
            }
            return response;
        }
        public PsReponse InsertKetQuaXN(PSXN_KetQua rowXN)
        {
            PsReponse response = new PsReponse();
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                bool isOK = false;
                var resKQ = db.PSXN_KetQuas.FirstOrDefault(p => p.isXoa != true && p.MaChiDinh == rowXN.MaChiDinh && p.MaTiepNhan == rowXN.MaTiepNhan);
                if (resKQ != null)
                {
                    rowXN.MaKetQua = resKQ.MaKetQua;
                    resKQ.isCoKQ = false;
                    resKQ.MaGoiXN = rowXN.MaGoiXN;
                    resKQ.isDongBo = false;
                    db.SubmitChanges();
                }
                else
                {
                    rowXN.MaKetQua = GetID();
                    rowXN.isCoKQ = false;
                    rowXN.isXoa = false;
                    rowXN.isDongBo = false;
                    db.PSXN_KetQuas.InsertOnSubmit(rowXN);
                    db.SubmitChanges();
                }
                var listDV = db.PSChiDinhDichVuChiTiets.Where(p => p.isXoa == false && p.MaChiDinh == rowXN.MaChiDinh).ToList();
                if (listDV.Count > 0)
                {
                    foreach (var DV in listDV)
                    {
                        var lstKT = db.PSMapsXN_DichVus.Where(p => p.IDDichVu == DV.MaDichVu).ToList();
                        if (lstKT.Count > 0)
                        {
                            foreach (var KT in lstKT)
                            {
                                var lstThongso = db.PSMapsXN_ThongSos.Where(p => p.IDKyThuatXN == KT.IDKyThuatXN).ToList();
                                if (lstThongso.Count > 0)
                                {
                                    foreach (var tso in lstThongso)
                                    {
                                        var thongso = db.PSDanhMucThongSoXNs.FirstOrDefault(p => p.IDThongSoXN == tso.IDThongSo);
                                        if (thongso != null)
                                        {
                                            PSXN_KetQua_ChiTiet KQct = new PSXN_KetQua_ChiTiet();
                                            KQct.DonViTinh = thongso.DonViTinh;
                                            KQct.GiaTriMaxNam = thongso.GiaTriMaxNam;
                                            KQct.GiaTriMaxNu = thongso.GiaTriMaxNu;
                                            KQct.GiaTriMinNam = thongso.GiaTriMinNam;
                                            KQct.GiaTriMinNu = thongso.GiaTriMinNu;
                                            KQct.GiaTriTrungBinhNam = thongso.GiaTriTrungBinhNam;
                                            KQct.GiaTriTrungBinhNu = thongso.GiaTriTrungBinhNu;
                                            KQct.isNguyCo = false;
                                            KQct.isXoa = false;
                                            KQct.isDongBo = false;
                                            KQct.MaDichVu = DV.MaDichVu;
                                            KQct.MaDonViTinh = thongso.MaDonViTinh;
                                            KQct.MaKQ = rowXN.MaKetQua;
                                            KQct.MaKyThuat = KT.IDKyThuatXN;
                                            KQct.MaThongSoXN = thongso.IDThongSoXN;
                                            KQct.MaXetNghiem = rowXN.MaXetNghiem;
                                            KQct.TenKyThuat = db.PSDanhMucKyThuatXNs.FirstOrDefault(p => p.IDKyThuatXN == KT.IDKyThuatXN).TenKyThuat ?? "";
                                            KQct.TenThongSo = thongso.TenThongSo;
                                            db.PSXN_KetQua_ChiTiets.InsertOnSubmit(KQct);
                                            db.SubmitChanges();
                                            isOK = true;

                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (isOK)
                    {
                        var cDinh = db.PSChiDinhDichVus.FirstOrDefault(p => p.isXoa == false && p.MaChiDinh == rowXN.MaChiDinh);
                        if (cDinh != null)
                        {
                            cDinh.TrangThai = 2;
                            cDinh.isDongBo = false;
                            cDinh.isXoa = false;

                            db.SubmitChanges();
                        }
                        var Phieu = db.PSPhieuSangLocs.FirstOrDefault(p => p.isXoa == false && p.IDPhieu == rowXN.MaPhieu);
                        if (Phieu != null)
                        {
                            Phieu.TrangThaiPhieu = true;
                            Phieu.isDongBo = false;
                            if (Phieu.isXNLan2 == true)
                            {
                                Phieu.TrangThaiMau = 5;
                            }
                            else
                                Phieu.TrangThaiMau = 3;
                            Phieu.NgayCoKQ = null;
                            if (string.IsNullOrEmpty(Phieu.MaXetNghiem))
                                Phieu.MaXetNghiem = rowXN.MaXetNghiem;
                            db.SubmitChanges();


                        }

                    }
                }
                db.Transaction.Commit();
                db.Connection.Close();
                response.Result = true;

            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                response.Result = false;
                response.StringError = ex.ToString();
            }
            return response;
        }
        public PsReponse UpdateKetQuaXN(PSXN_KetQua KQ)
        {
            PsReponse res = new PsReponse();
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                var rsKQ = db.PSXN_KetQuas.FirstOrDefault(p => p.isXoa == false && p.MaKetQua == KQ.MaKetQua);
                if (rsKQ == null)
                {
                    db.Connection.Close();
                    res.Result = false;
                    res.StringError = "Không tồn tại dữ liệu kết quả này";
                    return res;
                }
                else
                {
                    var lstkq = db.PSXN_KetQuas.Where(p => p.isXoa == false && p.MaPhieu == KQ.MaPhieu && p.MaDonVi == KQ.MaDonVi).ToList();
                    if (!KQ.MaChiDinh.Substring(0, 2).Equals("XN"))
                    {
                        if (lstkq.Count > 1)
                        {
                            db.Connection.Close();
                            res.Result = false;
                            res.StringError = "Phiếu này đã làm và có kết quả xét nghiệm lần 2 nên không thể sửa";
                            return res;
                        }
                    }

                    rsKQ.NgayTraKQ = DateTime.Now;
                    rsKQ.isCoKQ = KQ.isCoKQ;
                    rsKQ.UserTraKQ = KQ.UserTraKQ;
                    db.SubmitChanges();
                    var resChiDinh = db.PSChiDinhDichVus.Where(p => p.isXoa == false && p.MaChiDinh == KQ.MaChiDinh).ToList();
                    if (resChiDinh.Count > 0)
                    {
                        foreach (var cd in resChiDinh)
                        {
                            cd.TrangThai = 3;
                            db.SubmitChanges();
                        }
                    }
                    var resPhieu = db.PSPhieuSangLocs.FirstOrDefault(p => p.isXoa == false && p.IDCoSo == KQ.MaDonVi && p.IDPhieu == KQ.MaPhieu);
                    if (resPhieu != null) try { resPhieu.TrangThaiMau = 3; db.SubmitChanges(); } catch { }
                }

                db.Transaction.Commit();
                db.Connection.Close();
                res.Result = true;
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                res.Result = false;
                res.StringError = "Lỗi khi cập nhật dữ liệu : \r\n " + ex.ToString();
            }
            return res;
        }
        public PsReponse LuuKetQuaXN(KetQua_XetNghiem KQ)
        {
            PsReponse res = new PsReponse();
            res.Result = true;
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                bool isCoKQ = true;
                bool isNC = false;
                foreach (var item in KQ.KetQuaChiTiet)
                {
                    var KetQuaCT = db.PSXN_KetQua_ChiTiets.FirstOrDefault(x => x.MaKQ == item.MaKQ && x.MaThongSoXN == item.MaThongSo && x.MaXetNghiem == item.MaXN);
                    if (KetQuaCT != null)
                    {
                        if (!string.IsNullOrEmpty(item.GiaTri.ToString()))
                        {
                            KetQuaCT.GiaTri = item.GiaTri;
                            KetQuaCT.isNguyCo = item.isNguyCoCao;
                            KetQuaCT.isDongBo = false;
                            KetQuaCT.isXoa = false;
                            db.SubmitChanges();
                        }
                        if (string.IsNullOrEmpty(KetQuaCT.GiaTri))
                        {
                            isCoKQ = false;
                        }
                    }
                }
                var KetQua = db.PSXN_KetQuas.FirstOrDefault(x => x.MaPhieu == KQ.maPhieu && x.MaXetNghiem == KQ.maXetNghiem && x.MaChiDinh == KQ.maChiDinh);
                if (KetQua != null)
                {
                    KetQua.isCoKQ = isCoKQ;
                    KetQua.isDongBo = false;
                    KetQua.IDNhanVienNhapKQ = KQ.maNhanVienTraKQ;
                    KetQua.NgayTraKQ = DateTime.Now;
                    KetQua.GhiChu = KQ.GhiChu;
                    KetQua.isXoa = false;
                    db.SubmitChanges();
                }
                if (isCoKQ)
                {
                    var TraKetQua = db.PSXN_TraKetQuas.FirstOrDefault(x => x.MaPhieu == KQ.maPhieu && x.MaTiepNhan == KQ.maTiepNhan);
                    //Tạo dữ liệu bảng kết quả và bảng trả kết quả
                    if (TraKetQua == null)
                    {
                        PSXN_TraKetQua traKQ = new PSXN_TraKetQua();
                        traKQ.MaPhieu = KQ.maPhieu;
                        traKQ.MaTiepNhan = KQ.maTiepNhan;
                        traKQ.MaXetNghiem = KQ.maXetNghiem;
                        traKQ.MaGoiXN = KQ.maGoiXetNghiem;
                        traKQ.NgayChiDinh = KQ.ngayChiDinh;
                        traKQ.NgayCoKQ = DateTime.Now;
                        traKQ.NgayLamXetNghiem = KQ.ngayXetNghiem;
                        traKQ.NgayTiepNhan = KQ.ngayTiepNhan;
                        traKQ.isDongBoPDF = false;
                        traKQ.isDongBo = false;
                        traKQ.isDaDuyetKQ = false;
                        traKQ.IDCoSo = KQ.maDonVi;
                        traKQ.KetLuanTongQuat = string.Empty;
                        traKQ.GhiChu = string.Empty;
                        traKQ.UserTraKQ = KQ.maNhanVienTraKQ;
                        traKQ.NgayTraKQ = DateTime.Now;
                        traKQ.GhiChuPhongXetNghiem = KQ.GhiChu;
                        traKQ.isTraKQ = false;
                        traKQ.isXoa = false;
                        traKQ.isDaGuiMail = false;
                        var phieu = db.PSPhieuSangLocs.FirstOrDefault(p => p.isXoa == false && p.IDPhieu == KQ.maPhieu && p.IDCoSo == KQ.maDonVi);
                        if (phieu.isLayMauLan2 == true)
                        {
                            traKQ.MaPhieuCu = phieu.IDPhieuLan1;
                        }
                        db.PSXN_TraKetQuas.InsertOnSubmit(traKQ);
                        foreach (var item in KQ.KetQuaChiTiet)
                        {
                            var KetQuaCT = db.PSXN_KetQua_ChiTiets.FirstOrDefault(x => x.MaKQ == item.MaKQ && x.MaThongSoXN == item.MaThongSo && x.MaXetNghiem == item.MaXN);
                            if (KetQuaCT != null)
                            {
                                PSXN_TraKQ_ChiTiet CtTraKQ = new PSXN_TraKQ_ChiTiet();
                                CtTraKQ.GiaTri1 = KetQuaCT.GiaTri;
                                CtTraKQ.GiaTriMax = item.GiaTriMax;
                                CtTraKQ.GiaTriMin = item.GiaTriMin;
                                CtTraKQ.GiaTriTrungBinh = item.GiaTriTrungBinh;
                                CtTraKQ.IDDonViTinh = item.MaDonViTinh;
                                CtTraKQ.DonViTinh = item.DonViTinh;
                                CtTraKQ.IDKyThuat = item.MaKyThuat;
                                CtTraKQ.IDThongSoXN = item.MaThongSo;
                                CtTraKQ.isNguyCo = item.isNguyCoCao;
                                CtTraKQ.MaDichVu = item.MaDichVu;
                                CtTraKQ.MaPhieu = KQ.maPhieu;
                                CtTraKQ.isXoa = false;
                                CtTraKQ.isDongBo = false;
                                CtTraKQ.MaTiepNhan = KQ.maTiepNhan;
                                CtTraKQ.TenKyThuat = item.TenKyThuat;
                                CtTraKQ.TenThongSo = item.TenThongSo;
                                CtTraKQ.isMauXNLai = false;
                                CtTraKQ.isDaDuyetKQ = false;
                                try
                                {
                                    float gt = float.Parse(KetQuaCT.GiaTri);
                                    CtTraKQ.GiaTriCuoi = gt.ToString();
                                    if (gt >= item.GiaTriMin && gt < item.GiaTriMax)
                                    {
                                        CtTraKQ.isNguyCo = false;
                                        CtTraKQ.KetLuan = "Nguy cơ thấp";
                                        CtTraKQ.GiaTriCuoi = String.Format("{0:0.##}", gt);
                                    }
                                    else
                                    {
                                        isNC = true;
                                        CtTraKQ.isNguyCo = true;
                                        CtTraKQ.KetLuan = "Nghi ngờ";
                                    }
                                }
                                catch { }
                                db.PSXN_TraKQ_ChiTiets.InsertOnSubmit(CtTraKQ);
                                db.SubmitChanges();
                            }
                        }
                        traKQ.isNguyCoCao = isNC;
                        db.SubmitChanges();
                    }
                    //Update Kết quả xét nghiệm lần 2 vào bảng Trả Kết Quả và Chi Tiết Trả KQ
                    else
                    {
                        TraKetQua.NgayCoKQ = DateTime.Now;
                        TraKetQua.NgayTraKQ = DateTime.Now;
                        TraKetQua.isDongBo = false;
                        TraKetQua.isDongBoPDF = false;
                        TraKetQua.isDaDuyetKQ = false;
                        TraKetQua.isTraKQ = false;
                        TraKetQua.KetLuanTongQuat = string.Empty;
                        TraKetQua.GhiChu = string.Empty;
                        TraKetQua.KetLuanTongQuat = string.Empty;
                        TraKetQua.isDaGuiMail = false;
                        if (!string.IsNullOrEmpty(KQ.GhiChu))
                        {
                            TraKetQua.GhiChuPhongXetNghiem = TraKetQua.GhiChuPhongXetNghiem + ".Ghi chú XN2: " + KQ.GhiChu;
                        }
                        foreach (var item in TraKetQua.PSXN_TraKQ_ChiTiets)
                        {
                            var kqct = KQ.KetQuaChiTiet.FirstOrDefault(x => x.MaThongSo == item.IDThongSoXN);
                            if (kqct != null)
                            {
                                item.GiaTri2 = kqct.GiaTri;
                                item.isDaDuyetKQ = false;
                                float gtcuoi = (float.Parse(item.GiaTri2) + float.Parse(item.GiaTri1)) / 2;
                                item.GiaTriCuoi = String.Format("{0:0.##}", gtcuoi);
                                if (gtcuoi >= item.GiaTriMin && gtcuoi < item.GiaTriMax)
                                {
                                    item.isNguyCo = false;
                                    item.KetLuan = "Nguy cơ thấp";
                                }
                                else
                                {
                                    item.isNguyCo = true;
                                    item.KetLuan = "Nguy cơ cao";
                                    isNC = true;
                                }
                                item.isDaDuyetKQ = false;
                                item.isDongBo = false;
                                item.isXoa = false;
                                item.isMauXNLai = true;
                                db.SubmitChanges();
                            }
                        }
                        TraKetQua.isNguyCoCao = isNC;
                        db.SubmitChanges();
                    }
                }
                db.Transaction.Commit();
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.StringError = ex.ToString();
                db.Transaction.Rollback();
            }
            db.Connection.Close();
            return res;
        }
        public PsReponse LuuKetQuaSuaXN(KetQua_XetNghiem KQ)
        {
            PsReponse res = new PsReponse();
            res.Result = true;
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                bool isNC = false;
                var TraKetQua = db.PSXN_TraKetQuas.FirstOrDefault(x => x.MaPhieu == KQ.maPhieu && x.MaTiepNhan == KQ.maTiepNhan);
                if (TraKetQua.isDaDuyetKQ != true)
                {
                    foreach (var item in KQ.KetQuaChiTiet)
                    {
                        var KetQuaCT = db.PSXN_KetQua_ChiTiets.FirstOrDefault(x => x.MaKQ == item.MaKQ && x.MaThongSoXN == item.MaThongSo && x.MaXetNghiem == item.MaXN);
                        if (KetQuaCT != null)
                        {
                            if (!string.IsNullOrEmpty(item.GiaTri))
                            {
                                KetQuaCT.GiaTri = item.GiaTri;
                                KetQuaCT.isNguyCo = item.isNguyCoCao;
                                KetQuaCT.isDongBo = false;
                                KetQuaCT.isXoa = false;
                                db.SubmitChanges();
                            }
                        }
                        else
                        {
                        }
                    }
                    var KetQua = db.PSXN_KetQuas.FirstOrDefault(x => x.MaPhieu == KQ.maPhieu && x.MaXetNghiem == KQ.maXetNghiem && x.MaChiDinh == KQ.maChiDinh);
                    if (KetQua != null)
                    {
                        KetQua.isDongBo = false;
                        KetQua.NgayTraKQ = DateTime.Now;
                        db.SubmitChanges();
                    }
                    TraKetQua.NgayCoKQ = DateTime.Now;
                    TraKetQua.NgayTraKQ = DateTime.Now;
                    TraKetQua.isDongBo = false;
                    TraKetQua.isDongBoPDF = false;
                    TraKetQua.isDaDuyetKQ = false;
                    TraKetQua.isTraKQ = false;
                    TraKetQua.KetLuanTongQuat = string.Empty;
                    TraKetQua.GhiChu = string.Empty;
                    TraKetQua.KetLuanTongQuat = string.Empty;
                    TraKetQua.isDaGuiMail = false;
                    foreach (var item in TraKetQua.PSXN_TraKQ_ChiTiets)
                    {
                        var kqct = KQ.KetQuaChiTiet.FirstOrDefault(x => x.MaThongSo == item.IDThongSoXN);
                        if (kqct != null)
                        {
                            if (!string.IsNullOrEmpty(kqct.GiaTri))
                            {
                                float gtcuoi;
                                if (item.isMauXNLai == true)
                                {
                                    item.GiaTri2 = kqct.GiaTri;
                                    gtcuoi = (float.Parse(item.GiaTri2) + float.Parse(item.GiaTri1)) / 2;
                                }
                                else
                                {
                                    item.GiaTri1 = kqct.GiaTri;
                                    gtcuoi = float.Parse(item.GiaTri1);
                                }
                                item.isDaDuyetKQ = false;

                                if (gtcuoi >= item.GiaTriMin && gtcuoi < item.GiaTriMax)
                                {
                                    item.isNguyCo = false;
                                    item.KetLuan = "Nguy cơ thấp";
                                    item.GiaTriCuoi = String.Format("{0:0.##}", gtcuoi);
                                }
                                else
                                {
                                    item.isNguyCo = true;
                                    isNC = true;
                                    if (item.isMauXNLai == true)
                                    {
                                        item.KetLuan = "Nguy cơ cao";
                                        item.GiaTriCuoi = String.Format("{0:0.##}", gtcuoi);
                                    }
                                    else
                                    {
                                        item.KetLuan = "Nghi ngờ";
                                    }
                                }
                                item.isDaDuyetKQ = false;
                                item.isDongBo = false;
                                item.isXoa = false;
                                db.SubmitChanges();
                            }
                        }
                    }
                    TraKetQua.isNguyCoCao = isNC;
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                res.Result = false;
                res.StringError = ex.ToString();
            }
            db.Transaction.Commit();
            db.Connection.Close();
            return res;
        }
        public PsReponse UpdateTraKetQua(TraKetQua_XetNghiem KQ, bool isTraKQ)
        {
            PsReponse res = new PsReponse();
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                bool isNguyCoCao = false;
                var result = db.PSXN_TraKetQuas.FirstOrDefault(p => p.isXoa != true && p.MaPhieu == KQ.maPhieu && p.MaTiepNhan == KQ.maTiepNhan);
                if (result != null)
                {
                    if (result.isTraKQ ?? false)
                    {
                        res.Result = false;
                        res.StringError = "Kết quả đã được trả nên không được sửa";
                        db.Transaction.Rollback();
                        db.Connection.Close();
                        return res;
                    }
                    else
                    {
                        result.isDongBo = false;
                        result.GhiChu = KQ.ghiChu;
                        result.NgayTraKQ = DateTime.Now;
                        if (isTraKQ)
                        {
                            result.isTraKQ = true;
                        }
                        else
                        {
                            result.isTraKQ = false;
                        }
                        result.isDaDuyetKQ = true;
                        result.KetLuanTongQuat = KQ.ketLuan;
                        result.MaPhieuCu = KQ.MaPhieuLan1;
                        result.UserTraKQ = KQ.userTraKQ;
                        result.isDongBoPDF = false;
                        result.isDaGuiMail = false;
                        result.isXoa = false;
                        db.SubmitChanges();
                        #region Cập nhật chi tiết trả kết quả
                        if (KQ.chiTietKQ.Count > 0)
                        {
                            foreach (var KQCT in KQ.chiTietKQ)
                            {
                                var ct = db.PSXN_TraKQ_ChiTiets.FirstOrDefault(p => p.isXoa == false && p.MaTiepNhan == KQCT.MaTiepNhan && p.MaPhieu == KQCT.MaPhieu && p.IDThongSoXN == KQCT.IDThongSoXN && p.isXoa == false);
                                if (ct != null)
                                {
                                    ct.GiaTriCuoi = KQCT.GiaTriCuoi;
                                    ct.GiaTri1 = KQCT.GiaTri1;
                                    ct.GiaTri2 = KQCT.GiaTri2;
                                    ct.isNguyCo = KQCT.isNguyCo;
                                    ct.KetLuan = KQCT.KetLuan;
                                    ct.isDaDuyetKQ = true;
                                    ct.isDongBo = false;
                                    db.SubmitChanges();
                                    if (KQCT.isNguyCo && (KQCT.isMauXNLai ?? false)) isNguyCoCao = true;
                                }
                                else
                                {
                                    res.Result = false;
                                    res.StringError = "Một thông số xét nghiệm không tồn tại trong phiếu trả kết quả này ";
                                    db.Transaction.Rollback();
                                    db.Connection.Close();
                                }
                            }
                        }
                        else
                        {
                            res.Result = false;
                            res.StringError = "Dữ liệu lưu ko chứa thông tin các thông số xét nghiệm";
                            db.Transaction.Rollback();
                            db.Connection.Close();
                            return res;
                        }
                        #endregion
                        result.isNguyCoCao = KQ.isNguyCo;
                        db.SubmitChanges();
                    }

                }
                else
                {
                    res.Result = false;
                    res.StringError = "Không tồn tại thông tin kết quả của phiếu này trong hệ thống";
                    db.Transaction.Rollback();
                    db.Connection.Close();
                    return res;
                }
                if (isTraKQ)
                {
                    var phieu = db.PSPhieuSangLocs.FirstOrDefault(p => p.isXoa == false && p.IDPhieu == KQ.maPhieu && p.IDCoSo == KQ.maDonVi && p.isXoa != true);
                    if (phieu != null)
                    {
                        phieu.NgayCoKQ = result.NgayCoKQ;
                        phieu.TrangThaiMau = 4;
                        phieu.isNguyCoCao = KQ.isNguyCo;
                        phieu.isDongBo = false;
                        db.SubmitChanges();
                    }
                    #region Cập nhật Bệnh nhân nguy cơ cao

                    try
                    {
                        var Bn = db.PSPatients.FirstOrDefault(p => p.isXoa == false && p.MaBenhNhan == phieu.MaBenhNhan);
                        if (Bn != null)
                        {
                            var _res = db.PSBenhNhanNguyCoCaos.FirstOrDefault(p => p.isXoa == false && p.MaBenhNhan == phieu.MaBenhNhan && p.MaDonVi == KQ.maDonVi && p.isXoa == false);
                            if (isNguyCoCao)
                            {
                                if (_res == null)
                                {

                                    PSBenhNhanNguyCoCao BNNguyCo = new PSBenhNhanNguyCoCao();
                                    BNNguyCo.isNguyCoCao = true;
                                    BNNguyCo.MaBenhNhan = phieu.MaBenhNhan;
                                    BNNguyCo.MaKhachHang = Bn.MaKhachHang;
                                    BNNguyCo.MaTiepNhan = KQ.maTiepNhan;
                                    BNNguyCo.MaDonVi = KQ.maDonVi;
                                    BNNguyCo.NgayTiepNhan = result.NgayTiepNhan;
                                    BNNguyCo.TenBenhNhan = Bn.TenBenhNhan;
                                    BNNguyCo.isDaChanDoan = false;
                                    BNNguyCo.isDieuTri = false;
                                    BNNguyCo.isXoa = false;
                                    BNNguyCo.isDongBo = false;
                                    db.PSBenhNhanNguyCoCaos.InsertOnSubmit(BNNguyCo);
                                    db.SubmitChanges();

                                }
                                else
                                {
                                    _res.isNguyCoCao = true;
                                    _res.isXoa = false;
                                    _res.isDongBo = false;
                                    _res.TenBenhNhan = Bn.TenBenhNhan;
                                    if (_res.MaTiepNhan.Equals(KQ.maTiepNhan))
                                        _res.MaTiepNhan = KQ.maTiepNhan;
                                    else _res.MaTiepNhan2 = KQ.maTiepNhan;
                                    db.SubmitChanges();
                                }
                            }
                            else
                            {
                                if (_res != null)
                                {
                                    _res.isNguyCoCao = false;
                                    _res.isXoa = false;
                                    _res.isDongBo = false;
                                    _res.TenBenhNhan = Bn.TenBenhNhan;
                                    if (_res.MaTiepNhan.Equals(KQ.maTiepNhan))
                                        _res.MaTiepNhan = KQ.maTiepNhan;
                                    else _res.MaTiepNhan2 = KQ.maTiepNhan;
                                    db.SubmitChanges();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    { }
                    #endregion
                }
                db.Transaction.Commit();
                db.Connection.Close();
                res.Result = true;
                return res; ;
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                res.Result = false;
                if (string.IsNullOrEmpty(res.StringError))
                    res.StringError = ex.ToString();
                return res;
            }
        }

        public PsReponse InsertTiepNhan(PSTiepNhan tiepNhan)
        {
            PsReponse response = new PsReponse();
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                if (tiepNhan != null)
                {
                    tiepNhan.MaTiepNhan = GetID();
                    var tn = db.PSTiepNhans.FirstOrDefault(p => p.isXoa == false && p.MaDonVi == tiepNhan.MaDonVi && p.MaPhieu == tiepNhan.MaPhieu);
                    if (tn != null)
                    {
                        var phieu = db.PSPhieuSangLocs.FirstOrDefault(p => p.isXoa == false && p.IDCoSo == tiepNhan.MaDonVi && p.IDPhieu == tiepNhan.MaPhieu && p.TrangThaiMau == 0);
                        if (phieu != null)
                        {
                            tn.isDongBo = false;
                            if (!tn.isDaDanhGia ?? false)
                                tn.isDaDanhGia = false;
                            if ((phieu.TrangThaiMau ?? 0) < 2)
                                phieu.TrangThaiMau = 1;
                            phieu.NgayNhanMau = DateTime.Now;
                            phieu.TrangThaiPhieu = true;
                            phieu.isDongBo = false;
                            db.SubmitChanges();
                        }
                        else
                        {
                            tiepNhan.isDaNhapLieu = false;
                            tiepNhan.isXoa = false;
                            tiepNhan.isDongBo = false;
                            tiepNhan.isDaDanhGia = false;
                            db.PSTiepNhans.InsertOnSubmit(tiepNhan);
                            db.SubmitChanges();
                        }
                    }
                    else
                    {
                        bool nhaplieu = false;
                        var phieu = db.PSPhieuSangLocs.FirstOrDefault(p => p.isXoa == false && p.IDCoSo == tiepNhan.MaDonVi && p.IDPhieu == tiepNhan.MaPhieu);
                        if (phieu != null)
                        {
                            nhaplieu = true;
                            if ((phieu.TrangThaiMau ?? 0) < 2)
                                phieu.TrangThaiMau = 1;
                            phieu.NgayNhanMau = DateTime.Now;
                            phieu.TrangThaiPhieu = true;
                            phieu.isDongBo = false;
                            db.SubmitChanges();
                        }
                        tiepNhan.isDaNhapLieu = nhaplieu;
                        tiepNhan.isXoa = false;
                        tiepNhan.isDongBo = false;
                        tiepNhan.isDaDanhGia = false;
                        db.PSTiepNhans.InsertOnSubmit(tiepNhan);
                        db.SubmitChanges();
                    }
                }
                db.Transaction.Commit();
                db.Connection.Close();
                response.Result = true;
                return response;
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                response.Result = false;
                response.StringError = ex.ToString();
                return response;
            }
        }
        private string GetID()
        {
            var maTT = db.PSThongTinTrungTams.FirstOrDefault().MaVietTat;
            if (!string.IsNullOrEmpty(maTT))
            {
                return maTT + Guid.NewGuid().ToString();
            }
            else { return "00" + Guid.NewGuid().ToString(); }
        }


        public PsReponse InsertPhieu(PSPhieuSangLoc phieu)
        {
            PsReponse result = new PsReponse();
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {

                var ph = db.PSPhieuSangLocs.FirstOrDefault(p => p.isXoa == false && p.IDPhieu == phieu.IDPhieu);
                if (ph != null)
                {
                    ph.CheDoDinhDuong = phieu.CheDoDinhDuong;
                    ph.IDChuongTrinh = phieu.IDChuongTrinh;
                    ph.IDCoSo = phieu.IDCoSo;
                    ph.IDNhanVienLayMau = phieu.IDNhanVienLayMau;
                    //   ph.IDNhanVienTaoPhieu = phieu.IDNhanVienTaoPhieu;
                    ph.IDPhieu = phieu.IDPhieu;
                    ph.IDPhieuLan1 = phieu.IDPhieuLan1;
                    ph.IDViTriLayMau = phieu.IDViTriLayMau;
                    ph.isGuiMauTre = phieu.isGuiMauTre;
                    ph.isHuyMau = phieu.isHuyMau;
                    ph.isKhongDat = phieu.isKhongDat;
                    ph.isDongBo = false;

                    if (string.IsNullOrEmpty(phieu.IDPhieuLan1))
                        ph.isLayMauLan2 = false;
                    else ph.isLayMauLan2 = true;
                    ph.isLayMauLan2 = ph.isLayMauLan2 ?? false;
                    ph.isNheCan = phieu.isNheCan;
                    ph.isSinhNon = phieu.isSinhNon;
                    ph.isTruoc24h = phieu.isTruoc24h;
                    ph.MaBenhNhan = phieu.MaBenhNhan;
                    ph.MaGoiXN = phieu.MaGoiXN;
                    ph.MaXetNghiem = phieu.MaXetNghiem;
                    ph.NgayGioLayMau = phieu.NgayGioLayMau;
                    ph.NgayNhanMau = phieu.NgayNhanMau;
                    ph.NgayTruyenMau = phieu.NgayTruyenMau;
                    ph.NoiLayMau = phieu.NoiLayMau;
                    ph.DiaChiLayMau = phieu.DiaChiLayMau;
                    ph.Para = phieu.Para;
                    ph.SDTNhanVienLayMau = phieu.SDTNhanVienLayMau;
                    ph.SLTruyenMau = phieu.SLTruyenMau;
                    ph.TenNhanVienLayMau = phieu.TenNhanVienLayMau;
                    ph.TinhTrangLucLayMau = phieu.TinhTrangLucLayMau;

                    ph.TrangThaiPhieu = true;
                    ph.LyDoKhongDat = phieu.LyDoKhongDat;

                    db.SubmitChanges();
                }
                else
                {
                    PSPhieuSangLoc p = new PSPhieuSangLoc();
                    p.CheDoDinhDuong = phieu.CheDoDinhDuong;
                    p.IDChuongTrinh = phieu.IDChuongTrinh;
                    p.IDCoSo = phieu.IDCoSo;
                    p.IDNhanVienLayMau = phieu.IDNhanVienLayMau;
                    //    p.IDNhanVienTaoPhieu = phieu.IDNhanVienTaoPhieu;
                    p.IDPhieu = phieu.IDPhieu;
                    p.IDPhieuLan1 = phieu.IDPhieuLan1;
                    p.IDViTriLayMau = phieu.IDViTriLayMau;
                    p.isGuiMauTre = phieu.isGuiMauTre;
                    p.isHuyMau = phieu.isHuyMau;
                    p.isKhongDat = phieu.isKhongDat;
                    if (string.IsNullOrEmpty(phieu.IDPhieuLan1))
                        ph.isLayMauLan2 = false;
                    else ph.isLayMauLan2 = true;
                    p.isNheCan = phieu.isNheCan;
                    p.isSinhNon = phieu.isSinhNon;
                    p.isTruoc24h = phieu.isTruoc24h;
                    p.MaBenhNhan = phieu.MaBenhNhan;
                    p.MaGoiXN = phieu.MaGoiXN;
                    p.MaXetNghiem = phieu.MaXetNghiem;
                    p.NgayGioLayMau = phieu.NgayGioLayMau;
                    p.NgayNhanMau = phieu.NgayNhanMau;
                    p.NgayTaoPhieu = DateTime.Now;
                    p.NgayTruyenMau = phieu.NgayTruyenMau;
                    p.NoiLayMau = phieu.NoiLayMau;
                    p.DiaChiLayMau = phieu.DiaChiLayMau;
                    p.Para = phieu.Para;
                    p.SDTNhanVienLayMau = phieu.SDTNhanVienLayMau;
                    p.SLTruyenMau = phieu.SLTruyenMau;
                    p.TenNhanVienLayMau = phieu.TenNhanVienLayMau;
                    p.TinhTrangLucLayMau = phieu.TinhTrangLucLayMau;
                    p.TrangThaiMau = 1;
                    p.TrangThaiPhieu = true;
                    p.isDongBo = false;
                    p.isXoa = false;
                    p.LyDoKhongDat = phieu.LyDoKhongDat;
                    db.PSPhieuSangLocs.InsertOnSubmit(p);
                    db.SubmitChanges();
                }
                db.Transaction.Commit();
                db.Connection.Close();
                result.Result = true;
                return result;
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                result.Result = false;
                result.StringError = ex.ToString();
                return result;
            }
        }

        public string InsertBenhNhan(PSPatient thongTinBenhNhan, string maDonvi)
        {
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                string mBN = string.Empty;
                if (!string.IsNullOrEmpty(thongTinBenhNhan.MaBenhNhan))
                {
                    var bn = db.PSPatients.FirstOrDefault(p => p.MaBenhNhan == thongTinBenhNhan.MaBenhNhan);
                    if (bn != null)
                    {
                        bn.FatherName = thongTinBenhNhan.FatherName;
                        bn.FatherPhoneNumber = thongTinBenhNhan.FatherPhoneNumber;
                        bn.FatherBirthday = thongTinBenhNhan.FatherBirthday;
                        bn.MotherBirthday = thongTinBenhNhan.MotherBirthday;
                        bn.MotherName = thongTinBenhNhan.MotherName;
                        bn.MotherPhoneNumber = thongTinBenhNhan.MotherPhoneNumber;
                        if (string.IsNullOrEmpty(bn.MaKhachHang))
                            bn.MaKhachHang = GetNewMaKhachHang(maDonvi, SoBanDau());
                        bn.DiaChi = thongTinBenhNhan.DiaChi;
                        bn.TenBenhNhan = thongTinBenhNhan.TenBenhNhan;
                        bn.QuocTichID = thongTinBenhNhan.QuocTichID;
                        bn.TuanTuoiKhiSinh = thongTinBenhNhan.TuanTuoiKhiSinh;
                        bn.PhuongPhapSinh = thongTinBenhNhan.PhuongPhapSinh;
                        bn.NoiSinh = thongTinBenhNhan.NoiSinh;
                        bn.NgayGioSinh = thongTinBenhNhan.NgayGioSinh;
                        bn.GioiTinh = thongTinBenhNhan.GioiTinh;
                        bn.DanTocID = thongTinBenhNhan.DanTocID;
                        bn.CanNang = thongTinBenhNhan.CanNang;
                        bn.isDongBo = false;
                        bn.Para = thongTinBenhNhan.Para;
                        bn.isXoa = false;
                        db.SubmitChanges();
                        mBN = thongTinBenhNhan.MaBenhNhan;
                    }
                    else
                    {
                        mBN = GetID();
                        PSPatient bnh = new PSPatient();
                        bnh.MaBenhNhan = mBN;
                        bnh.FatherName = thongTinBenhNhan.FatherName;
                        bnh.FatherPhoneNumber = thongTinBenhNhan.FatherPhoneNumber;
                        bnh.FatherBirthday = thongTinBenhNhan.FatherBirthday;
                        bnh.MotherBirthday = thongTinBenhNhan.MotherBirthday;
                        bnh.MotherName = thongTinBenhNhan.MotherName;
                        bnh.MotherPhoneNumber = thongTinBenhNhan.MotherPhoneNumber;
                        if (string.IsNullOrEmpty(thongTinBenhNhan.MaKhachHang))
                            bn.MaKhachHang = GetNewMaKhachHang(maDonvi, SoBanDau());
                        else
                            bnh.MaKhachHang = thongTinBenhNhan.MaKhachHang;
                        bnh.DiaChi = thongTinBenhNhan.DiaChi;
                        bnh.TenBenhNhan = thongTinBenhNhan.TenBenhNhan;
                        bnh.QuocTichID = thongTinBenhNhan.QuocTichID;
                        bnh.TuanTuoiKhiSinh = thongTinBenhNhan.TuanTuoiKhiSinh;
                        bnh.PhuongPhapSinh = thongTinBenhNhan.PhuongPhapSinh;
                        bnh.NoiSinh = thongTinBenhNhan.NoiSinh;
                        bnh.NgayGioSinh = thongTinBenhNhan.NgayGioSinh;
                        bnh.GioiTinh = thongTinBenhNhan.GioiTinh;
                        bnh.DanTocID = thongTinBenhNhan.DanTocID;
                        bnh.CanNang = thongTinBenhNhan.CanNang;
                        bnh.isDongBo = false;
                        bnh.Para = thongTinBenhNhan.Para;
                        bnh.isXoa = false;
                        db.PSPatients.InsertOnSubmit(bnh);
                        db.SubmitChanges();
                    }
                }
                else
                {
                    mBN = GetID();
                    PSPatient bnh = new PSPatient();
                    bnh.MaBenhNhan = mBN;
                    bnh.FatherName = thongTinBenhNhan.FatherName;
                    bnh.FatherPhoneNumber = thongTinBenhNhan.FatherPhoneNumber;
                    bnh.FatherBirthday = thongTinBenhNhan.FatherBirthday;
                    bnh.MotherBirthday = thongTinBenhNhan.MotherBirthday;
                    bnh.MotherName = thongTinBenhNhan.MotherName;
                    bnh.MotherPhoneNumber = thongTinBenhNhan.MotherPhoneNumber;
                    if (string.IsNullOrEmpty(thongTinBenhNhan.MaKhachHang))
                        bnh.MaKhachHang = GetNewMaKhachHang(maDonvi, SoBanDau());
                    else
                        bnh.MaKhachHang = thongTinBenhNhan.MaKhachHang;
                    bnh.DiaChi = thongTinBenhNhan.DiaChi;
                    bnh.TenBenhNhan = thongTinBenhNhan.TenBenhNhan;
                    bnh.QuocTichID = thongTinBenhNhan.QuocTichID;
                    bnh.TuanTuoiKhiSinh = thongTinBenhNhan.TuanTuoiKhiSinh;
                    bnh.PhuongPhapSinh = thongTinBenhNhan.PhuongPhapSinh;
                    bnh.NoiSinh = thongTinBenhNhan.NoiSinh;
                    bnh.NgayGioSinh = thongTinBenhNhan.NgayGioSinh;
                    bnh.GioiTinh = thongTinBenhNhan.GioiTinh;
                    bnh.DanTocID = thongTinBenhNhan.DanTocID;
                    bnh.CanNang = thongTinBenhNhan.CanNang;
                    bnh.isDongBo = false;
                    bnh.Para = thongTinBenhNhan.Para;
                    bnh.isXoa = false;
                    db.PSPatients.InsertOnSubmit(bnh);
                    db.SubmitChanges();
                }
                try
                {
                    bool daNhapLieu = true;
                    if (string.IsNullOrEmpty(thongTinBenhNhan.FatherName)) daNhapLieu = false;
                    if (string.IsNullOrEmpty(thongTinBenhNhan.MotherName)) daNhapLieu = false;
                    if (string.IsNullOrEmpty(thongTinBenhNhan.DiaChi)) daNhapLieu = false;
                    if (string.IsNullOrEmpty(thongTinBenhNhan.FatherPhoneNumber)) daNhapLieu = false;
                    if (string.IsNullOrEmpty(thongTinBenhNhan.MotherPhoneNumber)) daNhapLieu = false;
                    if ((thongTinBenhNhan.CanNang ?? 0) < 500) daNhapLieu = false;
                    if (daNhapLieu)
                    {
                        var lst = db.PSPhieuSangLocs.Where(p => p.isXoa == false && p.MaBenhNhan == mBN).ToList();
                        if (lst.Count > 0)
                        {
                            foreach (var phieu in lst)
                            {
                                if ((phieu.NgayGioLayMau ?? new DateTime(1900, 01, 01)).Year > 2000)
                                {
                                    var lsttiepnhan = db.PSTiepNhans.Where(p => p.isXoa == false && p.MaPhieu == phieu.IDPhieu).ToList();
                                    if (lsttiepnhan.Count > 0)
                                    {
                                        foreach (var tiepnhan in lsttiepnhan)
                                        {
                                            var tn = db.PSTiepNhans.FirstOrDefault(p => p.MaTiepNhan == tiepnhan.MaTiepNhan);
                                            tn.isDaNhapLieu = true;
                                            db.SubmitChanges();
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                catch { }
                db.Transaction.Commit();
                db.Connection.Close();
                return mBN;
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                return string.Empty;
            }

        }
        public List<PSKetQuaXetNghiem> GetDanhSachKetQuaDaDuyet(DateTime tuNGay, DateTime denNay)
        {
            List<PSKetQuaXetNghiem> lstKQ = new List<PSKetQuaXetNghiem>();
            try
            {
                var lstkq = db.PSXN_TraKetQuas.Where(p => p.isDaDuyetKQ == true && p.isXoa == false && p.NgayTraKQ.Value.Date > tuNGay.Date && p.NgayTraKQ.Value.Date < denNay).ToList();

                if (lstkq.Count > 0)
                {
                    foreach (var kq in lstkq)
                    {
                        PSKetQuaXetNghiem KQXN = new PSKetQuaXetNghiem();
                        KQXN.GhiChu = kq.GhiChu;
                        KQXN.GhiChuPhongXetNghiem = kq.GhiChuPhongXetNghiem;
                        KQXN.IDCoSo = kq.IDCoSo;
                        KQXN.IDNhanVienXoa = kq.IDNhanVienXoa;
                        KQXN.isDaDuyetKQ = kq.isDaDuyetKQ;
                        KQXN.isDongBo = kq.isDongBo;
                        KQXN.isTraKQ = kq.isTraKQ;
                        KQXN.isXoa = kq.isXoa;
                        KQXN.KetLuanTongQuat = kq.KetLuanTongQuat;
                        KQXN.LyDoXoa = kq.LyDoXoa;
                        KQXN.MaGoiXN = kq.MaGoiXN;
                        KQXN.MaPhieu = kq.MaPhieu;
                        KQXN.MaPhieuCu = kq.MaPhieuCu;
                        KQXN.MaTiepNhan = kq.MaTiepNhan;
                        KQXN.MaXetNghiem = kq.MaXetNghiem;
                        KQXN.NgayChiDinh = kq.NgayChiDinh;
                        KQXN.NgayCoKQ = kq.NgayCoKQ;
                        KQXN.NgayGioXoa = kq.NgayGioXoa;
                        KQXN.NgayLamXetNghiem = kq.NgayLamXetNghiem;
                        KQXN.NgayTiepNhan = kq.NgayTiepNhan;
                        KQXN.NgayTraKQ = kq.NgayTraKQ;


                        var lstct = db.PSXN_TraKQ_ChiTiets.Where(p => p.MaTiepNhan == kq.MaTiepNhan && p.MaPhieu == kq.MaPhieu).ToList();
                        if (lstct.Count > 0)
                            KQXN.KetQuaChiTiet = lstct;
                        else KQXN.KetQuaChiTiet = new List<PSXN_TraKQ_ChiTiet>();
                        lstKQ.Add(KQXN);
                    }
                }
            }
            catch { }
            return lstKQ;
        }
        public PsReponse InsertDotChiDinhMoi(PsChiDinhvsDanhGia dg)
        {
            PsReponse result = new PsReponse();
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                if (dg != null)
                {
                    if (dg.Phieu != null)
                    {
                    }
                }
                result.Result = true;
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                result.Result = false;
                result.StringError = ex.ToString();
            }
            return result;
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
        public PsReponse HuyKetQuaPhongXetNghiem() // đưa về danh sách chờ kết quả xét nghiệm
        {

            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {

            }
            catch
            {

            }
            return new PsReponse();
        }
        public PsReponse HuyMauTrongPhongXetNghiem() //đưa về danh sách cấp mã  (xét trạng thái bằng 3)
        {
            return new PsReponse();
        }
        public PsReponse HuyTiepNhan() //Đưa phiếu đã tiếp nhận và danh sách hủy
        {
            return new PsReponse();
        }
        public PsReponse HuyChiDinhDichVu(string maCD, string maNV, string lydoHuy, string maPhieu, string maTiepNhan)
        {
            PsReponse res = new PsReponse();
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                var KQ = db.PSXN_KetQuas.Where(p => p.MaChiDinh == maCD && p.isXoa == false).ToList();
                if (KQ.Count > 0)
                {
                    db.Connection.Close();
                    res.Result = false;
                    res.StringError = "Phiếu đã vào phòng xét nghiệm nên không được quyền hủy các dịch vụ ở phiếu này";
                    return res;
                }
                else
                {
                    var cd = db.PSChiDinhDichVus.FirstOrDefault(p => p.MaChiDinh == maCD && p.isXoa == false && p.MaPhieu == maPhieu);
                    if (cd != null)
                    {

                        if (!cd.MaChiDinh.Substring(0, 2).Equals("XN"))
                        {
                            var tn = db.PSTiepNhans.FirstOrDefault(t => t.MaTiepNhan == maTiepNhan && t.MaPhieu == maPhieu && t.isXoa == false);
                            if (tn != null)
                            {
                                tn.isDaDanhGia = false;
                                db.SubmitChanges();
                            }
                            var p = db.PSPhieuSangLocs.FirstOrDefault(x => x.IDPhieu == maPhieu && x.isXoa != true);
                            if (!string.IsNullOrEmpty(p.IDPhieuLan1))
                            {
                                var p1 = db.PSPhieuSangLocs.FirstOrDefault(x => x.IDPhieu == p.IDPhieuLan1 && x.isXoa != true);
                                p1.TrangThaiMau = 6;
                                p1.isDongBo = false;
                                db.SubmitChanges();
                            }
                            p.TrangThaiMau = 1;
                            p.isDongBo = false;
                            db.SubmitChanges();
                        }
                        try
                        {
                            var CDCT = db.PSChiDinhDichVuChiTiets.Where(p => p.MaChiDinh == maCD && p.isXoa == false).ToList();
                            foreach (var item in CDCT)
                            {
                                db.PSChiDinhDichVuChiTiets.DeleteOnSubmit(item);
                            }
                            db.PSChiDinhDichVus.DeleteOnSubmit(cd);
                            db.SubmitChanges();
                        }
                        catch
                        {
                            res.Result = false;
                        }
                    }
                }

                db.Transaction.Commit();
                db.Connection.Close();
                res.Result = true;

            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                res.Result = false;
                if (string.IsNullOrEmpty(res.StringError))
                    res.StringError = ex.ToString();

            }
            return res;
        }

        public PsReponse HuyPhieuDaTiepNhan(string maphieu)
        {
            PsReponse res = new PsReponse();
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                var phieu = db.PSTiepNhans.FirstOrDefault(x => x.MaPhieu == maphieu);
                var psl = db.PSPhieuSangLocs.FirstOrDefault(x => x.IDPhieu == maphieu);
                if (phieu != null)
                {
                    db.PSTiepNhans.DeleteOnSubmit(phieu);
                }
                if (psl != null)
                {
                    psl.NgayNhanMau = null;
                    psl.TrangThaiMau = 0;
                    psl.TrangThaiPhieu = false;
                    if (!string.IsNullOrEmpty(psl.IDPhieuLan1))
                    {
                        var phieu1 = db.PSPhieuSangLocs.FirstOrDefault(x => x.IDPhieu == psl.IDPhieuLan1);
                        phieu1.isDongBo = false;
                        phieu1.TrangThaiMau = 6;
                        db.SubmitChanges();
                    }
                    var Patient = db.PSPatients.FirstOrDefault(x => x.MaBenhNhan == psl.MaBenhNhan);
                    if (Patient == null)
                    {
                        db.PSPhieuSangLocs.DeleteOnSubmit(psl);
                    }
                }

                db.SubmitChanges();
                db.Transaction.Commit();
                db.Connection.Close();
                res.Result = true;
            }
            catch (Exception ex)
            {

                db.Transaction.Rollback();
                db.Connection.Close();
                res.Result = false;
                if (string.IsNullOrEmpty(res.StringError))
                    res.StringError = ex.ToString();
            }
            return res;
        }
        public List<PsBaoCaoTuyChon> GetBaoCaoTuyChon(DateTime NgayBD, DateTime NgayKT, String MaDonVi)
        {
            List<PsBaoCaoTuyChon> lst = new List<PsBaoCaoTuyChon>();
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {

                var data = (from ph in db.PSPhieuSangLocs
                            from pa in db.PSPatients
                            from kq in db.PSXN_TraKetQuas
                            where ph.IDPhieu == kq.MaPhieu && ph.MaBenhNhan == pa.MaBenhNhan && ph.isXoa != true && pa.isXoa != true
                            && kq.isXoa != true && kq.isTraKQ == true && kq.NgayCoKQ.Value.Date >= NgayBD.Date && ph.NgayCoKQ.Value.Date <= NgayKT.Date && ph.IDCoSo.Contains(MaDonVi)
                            select new { ph, pa, kq }).ToList();

                if (data != null)
                {
                    foreach (var dat in data)
                    {
                        PsBaoCaoTuyChon bc = new PsBaoCaoTuyChon();
                        bc.IDPhieu = dat.ph.IDPhieu;
                        bc.MaDVCS = dat.ph.IDCoSo;
                        bc.MaKhachHang = dat.pa.MaKhachHang;
                        bc.MaXetNghiem = dat.ph.MaXetNghiem;
                        bc.GoiXN = dat.ph.MaGoiXN;
                        bc.TenMe = dat.pa.MotherName;
                        bc.NamSinhMe = dat.pa.MotherBirthday.Value.Year.ToString();
                        bc.SDTMe = dat.pa.MotherPhoneNumber;
                        if (!string.IsNullOrEmpty(dat.pa.FatherName))
                        {
                            bc.TenCha = dat.pa.FatherName;
                            bc.NamSinhCha = dat.pa.FatherBirthday.Value.Year.ToString();
                            bc.SDTCha = dat.pa.FatherPhoneNumber;
                        }
                        bc.DiaChiGiaDinh = dat.pa.DiaChi;
                        bc.Para = dat.pa.Para;
                        bc.PPSinh = dat.pa.PhuongPhapSinh.ToString();
                        bc.NoiLayMau = dat.ph.NoiLayMau;
                        bc.DiaChiLayMau = dat.ph.DiaChiLayMau;
                        bc.NguoiLayMau = dat.ph.TenNhanVienLayMau;
                        bc.SdtNguoiLayMau = dat.ph.SDTNhanVienLayMau;
                        bc.IDViTriLayMau = dat.ph.IDViTriLayMau;
                        bc.TenTre = dat.pa.TenBenhNhan;
                        bc.TuoiThai = dat.pa.TuanTuoiKhiSinh.ToString();
                        bc.SLTruyenMau = dat.ph.SLTruyenMau.ToString();
                        bc.GioiTinh = dat.pa.GioiTinh.ToString();
                        bc.NoiSinh = dat.pa.NoiSinh;
                        bc.NgaySinh = dat.pa.NgayGioSinh;
                        bc.NgayTruyenMau = dat.ph.NgayTruyenMau;
                        bc.IDCheDoDinhDuong = dat.ph.CheDoDinhDuong.ToString();
                        bc.IDTinhTrangTre = dat.ph.TinhTrangLucLayMau.ToString();
                        bc.CanNang = dat.pa.CanNang.ToString();
                        bc.IDDanToc = dat.pa.DanTocID.ToString();
                        bc.IDChuongTrinh = dat.ph.IDChuongTrinh;
                        bc.NgayNhanMau = dat.ph.NgayNhanMau;
                        bc.NgayXetNghiem = dat.kq.NgayLamXetNghiem;
                        bc.NgayTraKQ = dat.kq.NgayCoKQ;
                        bc.NgayLayMau = dat.ph.NgayGioLayMau;
                        bc.TinhTrangMau = dat.ph.isLayMauLan2;
                        bc.DanhGiaMau = dat.ph.isKhongDat;
                        bc.IDPhieuLan1 = dat.ph.IDPhieuLan1;
                        bc.LuuY = dat.ph.LuuYPhieu + "\r\n" + dat.kq.GhiChuPhongXetNghiem;
                        bc.GhiChu = dat.kq.GhiChu;
                        try { bc.KLNguyCoCao = dat.kq.KetLuanTongQuat.Split('.')[1]; }
                        catch { bc.KLNguyCoCao = string.Empty; }
                        bc.KLBinhThuong = dat.kq.KetLuanTongQuat.Split('.')[0];
                        bc.LyDoMauKDat = dat.ph.LyDoKhongDat;
                        bc.IDGoiDichVuChung = dat.ph.MaGoiXN;
                        PsCTKQBaoCaoTuyChon kq1 = new PsCTKQBaoCaoTuyChon();
                        PsCTKQBaoCaoTuyChon kq2 = new PsCTKQBaoCaoTuyChon();
                        PsCTKQBaoCaoTuyChon kqcuoi = new PsCTKQBaoCaoTuyChon();
                        PsCTKQBaoCaoTuyChon klcuoi = new PsCTKQBaoCaoTuyChon();

                        foreach (var ct in dat.kq.PSXN_TraKQ_ChiTiets)
                        {
                            if (ct.IDThongSoXN.TrimEnd().Equals("CH"))
                            {
                                kq1.CH = ct.GiaTri1;
                                kq2.CH = ct.GiaTri2;
                                kqcuoi.CH = ct.GiaTriCuoi;
                                klcuoi.CH = ct.KetLuan;
                            }
                            else if (ct.IDThongSoXN.TrimEnd().Equals("CAH"))
                            {
                                kq1.CAH = ct.GiaTri1;
                                kq2.CAH = ct.GiaTri2;
                                kqcuoi.CAH = ct.GiaTriCuoi;
                                klcuoi.CAH = ct.KetLuan;
                            }
                            else if (ct.IDThongSoXN.TrimEnd().Equals("GAL"))
                            {
                                kq1.GAL = ct.GiaTri1;
                                kq2.GAL = ct.GiaTri2;
                                kqcuoi.GAL = ct.GiaTriCuoi;
                                klcuoi.GAL = ct.KetLuan;
                            }
                            else if (ct.IDThongSoXN.TrimEnd().Equals("G6PD"))
                            {
                                kq1.G6PD = ct.GiaTri1;
                                kq2.G6PD = ct.GiaTri2;
                                kqcuoi.G6PD = ct.GiaTriCuoi;
                                klcuoi.G6PD = ct.KetLuan;
                            }
                            else if (ct.IDThongSoXN.TrimEnd().Equals("PKU"))
                            {
                                kq1.PKU = ct.GiaTri1;
                                kq2.PKU = ct.GiaTri2;
                                kqcuoi.PKU = ct.GiaTriCuoi;
                                klcuoi.PKU = ct.KetLuan;
                            }
                            if (dat.ph.MaGoiXN.Equals("DVGXN0001"))
                            {
                                if (string.IsNullOrEmpty(bc.ChiSoXNLai))
                                {
                                    bc.ChiSoXNLai = ct.TenThongSo;
                                }
                                else
                                {
                                    bc.ChiSoXNLai = bc.ChiSoXNLai + ", " + ct.TenThongSo;
                                }

                            }
                        }
                        bc.ctkqL1 = kq1;
                        bc.ctkqL2 = kq2;
                        bc.ctkqCuoi = kqcuoi;
                        bc.ctklCuoi = klcuoi;
                        bc.IDPhieu = dat.ph.IDPhieu;
                        lst.Add(bc);
                    }
                }
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
            }
            return lst;
        }
        public List<PsBaoCaoTuyChon> GetBaoCaoTuyChonTrangThai(DateTime NgayBD, DateTime NgayKT, int TrangThai)
        {
            List<PsBaoCaoTuyChon> lst = new List<PsBaoCaoTuyChon>();
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {
                var data = db.PSTiepNhans.Where(x => x.isXoa != true && x.NgayTiepNhan.Value.Date >= NgayBD.Date && x.NgayTiepNhan.Value.Date <= NgayKT.Date).ToList();
                if (data != null)
                {
                    foreach (var dat in data)
                    {
                        PSPhieuSangLoc phieu = db.PSPhieuSangLocs.FirstOrDefault(x => x.IDPhieu == dat.MaPhieu && x.IDCoSo == dat.MaDonVi && x.isXoa != true && x.TrangThaiMau == TrangThai);
                        if (phieu != null)
                        {
                            PsBaoCaoTuyChon bc = new PsBaoCaoTuyChon();
                            bc.IDPhieu = dat.MaPhieu;
                            bc.MaDVCS = dat.MaDonVi;
                            bc.GoiXN = phieu.MaGoiXN;
                            bc.NoiLayMau = phieu.NoiLayMau;
                            bc.DiaChiLayMau = phieu.DiaChiLayMau;
                            bc.NguoiLayMau = phieu.TenNhanVienLayMau;
                            bc.SdtNguoiLayMau = phieu.SDTNhanVienLayMau;
                            bc.IDViTriLayMau = phieu.IDViTriLayMau;
                            bc.MaXetNghiem = phieu.MaXetNghiem;
                            bc.NgayTruyenMau = phieu.NgayTruyenMau;
                            bc.IDCheDoDinhDuong = phieu.CheDoDinhDuong.ToString();
                            bc.IDTinhTrangTre = phieu.TinhTrangLucLayMau.ToString();
                            bc.IDChuongTrinh = phieu.IDChuongTrinh;
                            bc.NgayNhanMau = phieu.NgayNhanMau;
                            bc.NgayLayMau = phieu.NgayGioLayMau;
                            bc.TinhTrangMau = phieu.isLayMauLan2;
                            bc.DanhGiaMau = phieu.isKhongDat;
                            bc.IDPhieuLan1 = phieu.IDPhieuLan1;
                            bc.SLTruyenMau = phieu.SLTruyenMau != null ? phieu.SLTruyenMau.ToString() : string.Empty;
                            bc.LyDoMauKDat = phieu.LyDoKhongDat;
                            bc.IDGoiDichVuChung = phieu.MaGoiXN;
                            PSPatient pa = db.PSPatients.FirstOrDefault(x => x.MaBenhNhan.Equals(phieu.MaBenhNhan) && x.isXoa != true);
                            if (pa != null)
                            {
                                bc.MaKhachHang = pa.MaKhachHang;
                                bc.TenMe = pa.MotherName;
                                bc.NamSinhMe = pa.MotherBirthday != null ? pa.MotherBirthday.Value.Year.ToString() : null;
                                bc.SDTMe = pa.MotherPhoneNumber;
                                if (!string.IsNullOrEmpty(pa.FatherName))
                                {
                                    bc.TenCha = pa.FatherName;
                                    bc.NamSinhCha = pa.FatherBirthday != null ? pa.FatherBirthday.Value.Year.ToString() : null;
                                    bc.SDTCha = pa.FatherPhoneNumber;
                                }
                                bc.DiaChiGiaDinh = pa.DiaChi;
                                bc.Para = pa.Para;
                                bc.PPSinh = pa.PhuongPhapSinh.ToString();
                                bc.TenTre = pa.TenBenhNhan;
                                bc.TuoiThai = pa.TuanTuoiKhiSinh.ToString();
                                bc.GioiTinh = pa.GioiTinh.ToString();
                                bc.NoiSinh = pa.NoiSinh;
                                bc.NgaySinh = pa.NgayGioSinh;
                                bc.CanNang = pa.CanNang.ToString();
                                bc.IDDanToc = pa.DanTocID.ToString();
                            }
                            PSXN_TraKetQua TraKQ = db.PSXN_TraKetQuas.FirstOrDefault(x => x.isXoa != true && x.MaGoiXN == bc.GoiXN && x.MaPhieu == x.MaPhieu);
                            if (TraKQ != null)
                            {
                                bc.NgayXetNghiem = TraKQ.NgayLamXetNghiem;
                                bc.NgayTraKQ = TraKQ.NgayCoKQ;
                                bc.LuuY = phieu.LuuYPhieu + "\r\n" + TraKQ.GhiChuPhongXetNghiem;
                                bc.GhiChu = TraKQ.GhiChu;
                                try { bc.KLNguyCoCao = TraKQ.KetLuanTongQuat.Split('.')[1]; }
                                catch { bc.KLNguyCoCao = string.Empty; }
                                bc.KLBinhThuong = TraKQ.KetLuanTongQuat.Split('.')[0];
                                PsCTKQBaoCaoTuyChon kq1 = new PsCTKQBaoCaoTuyChon();
                                PsCTKQBaoCaoTuyChon kq2 = new PsCTKQBaoCaoTuyChon();
                                PsCTKQBaoCaoTuyChon kqcuoi = new PsCTKQBaoCaoTuyChon();
                                PsCTKQBaoCaoTuyChon klcuoi = new PsCTKQBaoCaoTuyChon();

                                foreach (var ct in TraKQ.PSXN_TraKQ_ChiTiets)
                                {
                                    if (ct.IDThongSoXN.TrimEnd().Equals("CH"))
                                    {
                                        kq1.CH = ct.GiaTri1;
                                        kq2.CH = ct.GiaTri2;
                                        kqcuoi.CH = ct.GiaTriCuoi;
                                        klcuoi.CH = ct.KetLuan;
                                    }
                                    else if (ct.IDThongSoXN.TrimEnd().Equals("CAH"))
                                    {
                                        kq1.CAH = ct.GiaTri1;
                                        kq2.CAH = ct.GiaTri2;
                                        kqcuoi.CAH = ct.GiaTriCuoi;
                                        klcuoi.CAH = ct.KetLuan;
                                    }
                                    else if (ct.IDThongSoXN.TrimEnd().Equals("GAL"))
                                    {
                                        kq1.GAL = ct.GiaTri1;
                                        kq2.GAL = ct.GiaTri2;
                                        kqcuoi.GAL = ct.GiaTriCuoi;
                                        klcuoi.GAL = ct.KetLuan;
                                    }
                                    else if (ct.IDThongSoXN.TrimEnd().Equals("G6PD"))
                                    {
                                        kq1.G6PD = ct.GiaTri1;
                                        kq2.G6PD = ct.GiaTri2;
                                        kqcuoi.G6PD = ct.GiaTriCuoi;
                                        klcuoi.G6PD = ct.KetLuan;
                                    }
                                    else if (ct.IDThongSoXN.TrimEnd().Equals("PKU"))
                                    {
                                        kq1.PKU = ct.GiaTri1;
                                        kq2.PKU = ct.GiaTri2;
                                        kqcuoi.PKU = ct.GiaTriCuoi;
                                        klcuoi.PKU = ct.KetLuan;
                                    }
                                    if (phieu.MaGoiXN.Equals("DVGXN0001"))
                                    {
                                        if (string.IsNullOrEmpty(bc.ChiSoXNLai))
                                        {
                                            bc.ChiSoXNLai = ct.TenThongSo;
                                        }
                                        else
                                        {
                                            bc.ChiSoXNLai = bc.ChiSoXNLai + ", " + ct.TenThongSo;
                                        }

                                    }
                                }
                                bc.ctkqL1 = kq1;
                                bc.ctkqL2 = kq2;
                                bc.ctkqCuoi = kqcuoi;
                                bc.ctklCuoi = klcuoi;
                            }
                            lst.Add(bc);
                        }
                    }
                }

            }
            catch
            {
                db.Transaction.Rollback();
                db.Connection.Close();
            }
            return lst;
        }
        public List<PsBaoCaoTuyChon> GetBaoCaoTuyChon(DateTime NgayBD, DateTime NgayKT)
        {
            List<PsBaoCaoTuyChon> lst = new List<PsBaoCaoTuyChon>();
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            try
            {

                var data = (from ph in db.PSPhieuSangLocs
                            from pa in db.PSPatients
                            from kq in db.PSXN_TraKetQuas
                            where ph.IDPhieu == kq.MaPhieu && ph.MaBenhNhan == pa.MaBenhNhan && ph.isXoa != true && pa.isXoa != true
                            && kq.isXoa != true && kq.isTraKQ == true && kq.NgayCoKQ.Value.Date >= NgayBD.Date && ph.NgayCoKQ.Value.Date <= NgayKT.Date
                            select new { ph, pa, kq }).ToList();

                if (data != null)
                {
                    foreach (var dat in data)
                    {
                        PsBaoCaoTuyChon bc = new PsBaoCaoTuyChon();
                        bc.IDPhieu = dat.ph.IDPhieu;
                        bc.MaDVCS = dat.ph.IDCoSo;
                        bc.MaKhachHang = dat.pa.MaKhachHang;
                        bc.MaXetNghiem = dat.ph.MaXetNghiem;
                        bc.GoiXN = dat.ph.MaGoiXN;
                        bc.TenMe = dat.pa.MotherName;
                        bc.NamSinhMe = dat.pa.MotherBirthday.Value.Year.ToString();
                        bc.SDTMe = dat.pa.MotherPhoneNumber;
                        if (!string.IsNullOrEmpty(dat.pa.FatherName))
                        {
                            bc.TenCha = dat.pa.FatherName;
                            bc.NamSinhCha = dat.pa.FatherBirthday.Value.Year.ToString();
                            bc.SDTCha = dat.pa.FatherPhoneNumber;
                        }
                        bc.DiaChiGiaDinh = dat.pa.DiaChi;
                        bc.Para = dat.pa.Para;
                        bc.PPSinh = dat.pa.PhuongPhapSinh.ToString();
                        bc.NoiLayMau = dat.ph.NoiLayMau;
                        bc.DiaChiLayMau = dat.ph.DiaChiLayMau;
                        bc.NguoiLayMau = dat.ph.TenNhanVienLayMau;
                        bc.SdtNguoiLayMau = dat.ph.SDTNhanVienLayMau;
                        bc.IDViTriLayMau = dat.ph.IDViTriLayMau;
                        bc.TenTre = dat.pa.TenBenhNhan;
                        bc.TuoiThai = dat.pa.TuanTuoiKhiSinh.ToString();
                        bc.SLTruyenMau = dat.ph.SLTruyenMau.ToString();
                        bc.GioiTinh = dat.pa.GioiTinh.ToString();
                        bc.NoiSinh = dat.pa.NoiSinh;
                        bc.NgaySinh = dat.pa.NgayGioSinh;
                        bc.NgayTruyenMau = dat.ph.NgayTruyenMau;
                        bc.IDCheDoDinhDuong = dat.ph.CheDoDinhDuong.ToString();
                        bc.IDTinhTrangTre = dat.ph.TinhTrangLucLayMau.ToString();
                        bc.CanNang = dat.pa.CanNang.ToString();
                        bc.IDDanToc = dat.pa.DanTocID.ToString();
                        bc.IDChuongTrinh = dat.ph.IDChuongTrinh;
                        bc.NgayNhanMau = dat.ph.NgayNhanMau;
                        bc.NgayXetNghiem = dat.kq.NgayLamXetNghiem;
                        bc.NgayTraKQ = dat.kq.NgayCoKQ;
                        bc.NgayLayMau = dat.ph.NgayGioLayMau;
                        bc.TinhTrangMau = dat.ph.isLayMauLan2;
                        bc.DanhGiaMau = dat.ph.isKhongDat;
                        bc.IDPhieuLan1 = dat.ph.IDPhieuLan1;
                        bc.LuuY = dat.ph.LuuYPhieu + "\r\n" + dat.kq.GhiChuPhongXetNghiem;
                        bc.GhiChu = dat.kq.GhiChu;
                        try { bc.KLNguyCoCao = dat.kq.KetLuanTongQuat.Split('.')[1]; }
                        catch { bc.KLNguyCoCao = string.Empty; }
                        bc.KLBinhThuong = dat.kq.KetLuanTongQuat.Split('.')[0];
                        bc.LyDoMauKDat = dat.ph.LyDoKhongDat;
                        bc.IDGoiDichVuChung = dat.ph.MaGoiXN;
                        PsCTKQBaoCaoTuyChon kq1 = new PsCTKQBaoCaoTuyChon();
                        PsCTKQBaoCaoTuyChon kq2 = new PsCTKQBaoCaoTuyChon();
                        PsCTKQBaoCaoTuyChon kqcuoi = new PsCTKQBaoCaoTuyChon();
                        PsCTKQBaoCaoTuyChon klcuoi = new PsCTKQBaoCaoTuyChon();

                        foreach (var ct in dat.kq.PSXN_TraKQ_ChiTiets)
                        {
                            if (ct.IDThongSoXN.TrimEnd().Equals("CH"))
                            {
                                kq1.CH = ct.GiaTri1;
                                kq2.CH = ct.GiaTri2;
                                kqcuoi.CH = ct.GiaTriCuoi;
                                klcuoi.CH = ct.KetLuan;
                            }
                            else if (ct.IDThongSoXN.TrimEnd().Equals("CAH"))
                            {
                                kq1.CAH = ct.GiaTri1;
                                kq2.CAH = ct.GiaTri2;
                                kqcuoi.CAH = ct.GiaTriCuoi;
                                klcuoi.CAH = ct.KetLuan;
                            }
                            else if (ct.IDThongSoXN.TrimEnd().Equals("GAL"))
                            {
                                kq1.GAL = ct.GiaTri1;
                                kq2.GAL = ct.GiaTri2;
                                kqcuoi.GAL = ct.GiaTriCuoi;
                                klcuoi.GAL = ct.KetLuan;
                            }
                            else if (ct.IDThongSoXN.TrimEnd().Equals("G6PD"))
                            {
                                kq1.G6PD = ct.GiaTri1;
                                kq2.G6PD = ct.GiaTri2;
                                kqcuoi.G6PD = ct.GiaTriCuoi;
                                klcuoi.G6PD = ct.KetLuan;
                            }
                            else if (ct.IDThongSoXN.TrimEnd().Equals("PKU"))
                            {
                                kq1.PKU = ct.GiaTri1;
                                kq2.PKU = ct.GiaTri2;
                                kqcuoi.PKU = ct.GiaTriCuoi;
                                klcuoi.PKU = ct.KetLuan;
                            }
                            if (dat.ph.MaGoiXN.Equals("DVGXN0001"))
                            {
                                if (string.IsNullOrEmpty(bc.ChiSoXNLai))
                                {
                                    bc.ChiSoXNLai = ct.TenThongSo;
                                }
                                else
                                {
                                    bc.ChiSoXNLai = bc.ChiSoXNLai + ", " + ct.TenThongSo;
                                }

                            }
                        }
                        bc.ctkqL1 = kq1;
                        bc.ctkqL2 = kq2;
                        bc.ctkqCuoi = kqcuoi;
                        bc.ctklCuoi = klcuoi;
                        bc.IDPhieu = dat.ph.IDPhieu;
                        lst.Add(bc);
                    }
                }
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
            }
            return lst;
        }
        private static string SoBanDau()
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
        public DateTime GetDateTimeServer()
        {
            return db.ExecuteQuery<DateTime>("Select GETDATE()").FirstOrDefault();
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        #region Language

        public List<PSMenuItem> GetMenuItem(string IDForm)
        {
            List<PSMenuItem> psmenu = new List<PSMenuItem>();
            try
            {
                psmenu = db.PSMenuItems.ToList();
            }
            catch
            {

            }
            return psmenu;
        }

        public void AddMenuItem(List<PSMenuItem> lst, long? IDForm)
        {
            try
            {
                var lang = db.PSMenuLanguages.Where(x => x.IDLanguage != 0).ToList();
                foreach (var ls in lst)
                {
                    if (!string.IsNullOrEmpty(ls.VN))
                    {
                        var fo = db.PSMenuItems.FirstOrDefault(x => x.ItemName.Equals(ls.ItemName) && x.IDForm == IDForm);
                        if (fo == null)
                        {
                            PSMenuItem me = new PSMenuItem
                            {
                                ItemName = ls.ItemName,
                                IDForm = IDForm,
                                VN = ls.VN,
                                ItemType = ls.ItemType,
                            };
                            db.PSMenuItems.InsertOnSubmit(me);
                            db.SubmitChanges();
                            foreach (var la in lang)
                            {
                                PSMenuItemCT ct = new PSMenuItemCT
                                {
                                    IDItem = db.PSMenuItems.FirstOrDefault(x => x.ItemName.Equals(ls.ItemName) && x.IDForm == IDForm).IDItem,
                                    IDLanguage = la.IDLanguage,
                                    CaptionItemTrans = "LTQ" + db.PSMenuItems.FirstOrDefault(x => x.ItemName.Equals(ls.ItemName) && x.IDForm == IDForm).IDItem.ToString()
                                };
                                if (string.IsNullOrEmpty(ct.CaptionItemTrans))
                                {

                                }
                                db.PSMenuItemCTs.InsertOnSubmit(ct);
                                db.SubmitChanges();
                            }
                        }
                        else
                        {
                            if (!fo.VN.Equals(ls.VN.TrimEnd()))
                            {
                                fo.VN = ls.VN;
                                var ctla = db.PSMenuItemCTs.Where(x => x.IDItem == fo.IDItem).ToList();
                                if (ctla.Count() > 0)
                                {
                                    ctla.ForEach(x => x.CaptionItemTrans = string.Empty);
                                }
                                else
                                {
                                    foreach (var la in lang)
                                    {
                                        PSMenuItemCT ct = new PSMenuItemCT
                                        {
                                            IDItem = db.PSMenuItems.FirstOrDefault(x => x.ItemName.Equals(ls.ItemName) && x.IDForm == IDForm).IDItem,
                                            IDLanguage = la.IDLanguage,
                                            CaptionItemTrans = "LTQ" + db.PSMenuItems.FirstOrDefault(x => x.ItemName.Equals(ls.ItemName) && x.IDForm == IDForm).IDItem.ToString()
                                        };
                                        db.PSMenuItemCTs.InsertOnSubmit(ct);
                                        db.SubmitChanges();
                                    }
                                }
                                db.SubmitChanges();
                            }
                        }
                    }
                }
            }
            catch
            { }
        }
        public void SetLanguage(int lang)
        {
            try
            {
                var data = db.PSMenuLanguages.FirstOrDefault(x => x.IDLanguage == lang);
                if (data != null)
                {
                    data.isUse = true;
                    db.PSMenuLanguages.Where(x => x.IDLanguage != lang).ToList().ForEach(x => x.isUse = false);
                    db.SubmitChanges();
                }
            }
            catch
            { }
        }
        public void AddMenuForm(PSMenuForm form)
        {

            try
            {
                var fo = db.PSMenuForms.FirstOrDefault(x => x.NameForm.Equals(form.NameForm));
                if (fo == null)
                {
                    db.PSMenuForms.InsertOnSubmit(form);
                    db.SubmitChanges();
                }
                else
                {
                    fo.Capiton = form.Capiton;
                    db.SubmitChanges();
                }
            }
            catch
            { }
        }

        public long? GetMenuIDForm(string NameForm)
        {
            long? id;
            try
            {
                id = db.PSMenuForms.FirstOrDefault(x => x.NameForm.Equals(NameForm)).IDForm;
            }
            catch
            {
                id = 0;
            }
            return id;
        }
        public PSMenuTrans TransItem(long? IDForm, string NameItem)
        {
            PSMenuTrans kq = new PSMenuTrans();
            try
            {
                var data = (from me in db.PSMenuItems
                            join tran in db.PSMenuItemCTs on me.IDItem equals tran.IDItem
                            join lan in db.PSMenuLanguages on tran.IDLanguage equals lan.IDLanguage
                            where me.IDForm == IDForm && lan.isUse == true && lan.IDLanguage != 0 && me.ItemName.Equals(NameItem)
                            select new { me, tran }).FirstOrDefault();
                if (data != null)
                {
                    kq.IDItem = data.me.IDItem;
                    kq.ItemName = data.me.ItemName;
                    kq.VN = data.me.VN;
                    kq.Trans = data.tran.CaptionItemTrans;
                }
            }
            catch
            {

            }
            return kq;
        }
        public List<PSMenuTrans> Trans(long? IDForm)
        {
            List<PSMenuTrans> kq = new List<PSMenuTrans>();
            try
            {
                var data = (from me in db.PSMenuItems
                            join tran in db.PSMenuItemCTs on me.IDItem equals tran.IDItem
                            join lan in db.PSMenuLanguages on tran.IDLanguage equals lan.IDLanguage
                            where me.IDForm == IDForm && lan.isUse == true && lan.IDLanguage != 0
                            select new { me, tran }).ToList();
                if (data.Count != 0)
                {
                    foreach (var da in data)
                    {
                        PSMenuTrans tr = new PSMenuTrans
                        {
                            IDItem = da.me.IDItem,
                            ItemName = da.me.ItemName,
                            VN = da.me.VN,
                            Trans = da.tran.CaptionItemTrans
                        };
                        kq.Add(tr);
                    }
                }

            }
            catch
            {

            }
            return kq;
        }
        public List<PSMenuTrans> TransAll(long? IDForm)
        {
            List<PSMenuTrans> kq = new List<PSMenuTrans>();
            try
            {
                var data = (from me in db.PSMenuItems
                            join tran in db.PSMenuItemCTs on me.IDItem equals tran.IDItem
                            join lan in db.PSMenuLanguages on tran.IDLanguage equals lan.IDLanguage
                            where me.IDForm == IDForm
                            select new { me, tran }).ToList();
                foreach (var da in data)
                {
                    PSMenuTrans tr = new PSMenuTrans
                    {
                        IDItem = da.me.IDItem,
                        ItemName = da.me.ItemName,
                        VN = da.me.VN,
                        Trans = da.tran.CaptionItemTrans
                    };
                    kq.Add(tr);
                }
            }
            catch
            {
            }
            return kq;
        }

        public List<PSMenuForm> GetMenuForm()
        {
            List<PSMenuForm> kq = new List<PSMenuForm>();
            try
            {
                kq = db.PSMenuForms.ToList();
            }
            catch
            {
            }
            return kq;
        }

        #endregion
        #region Sửa lỗi
        public PsReponse UpdateMaKhachHang()
        {
            PsReponse reponse = new PsReponse();
            try
            {
                var patient = (from pa in db.PSPatients
                               join ph in db.PSPhieuSangLocs on pa.MaBenhNhan equals ph.MaBenhNhan
                               where ph.TrangThaiMau >= 2 && ph.isXoa != true && pa.MaKhachHang==null
                               select new { pa,ph }).ToList();
                if (patient.Count > 0)
                {
                    foreach (var pat in patient)
                    {
                        if (string.IsNullOrEmpty(pat.pa.MaKhachHang))
                        {
                            pat.pa.MaKhachHang = GetNewMaKhachHang(pat.ph.IDCoSo, SoBanDau());
                            pat.pa.isDongBo = false;
                            db.SubmitChanges();
                            var dcd = db.PSDotChuanDoans.Where(x => x.MaBenhNhan.Equals(pat.pa.MaBenhNhan)).ToList();
                            if(dcd.Count>0)
                            {
                                foreach(var dc in dcd)
                                {
                                    dc.MaKhachHang = pat.pa.MaKhachHang;
                                    dc.isDongBo = false;
                                    db.SubmitChanges();
                                }
                            }
                            var dcdc = db.PSDotChuanDoans.Where(x => x.MaBenhNhan.Equals(pat.pa.MaBenhNhan)).ToList();
                            if (dcdc.Count > 0)
                            {
                                foreach (var dc in dcdc)
                                {
                                    dc.MaKhachHang = pat.pa.MaKhachHang;
                                    dc.isDongBo = false;
                                    db.SubmitChanges();
                                }
                            }
                            var bnncc = db.PSBenhNhanNguyCoCaos.Where(x => x.MaBenhNhan.Equals(pat.pa.MaBenhNhan)).ToList();
                            if (bnncc.Count > 0)
                            {
                                foreach (var dc in bnncc)
                                {
                                    dc.MaKhachHang = pat.pa.MaKhachHang;
                                    dc.isDongBo = false;
                                    db.SubmitChanges();
                                }
                            }
                            var sms = db.PSSMSCs.Where(x => x.IDPhieu.Equals(pat.ph.IDPhieu)).ToList();
                            if (sms.Count > 0)
                            {
                                foreach (var dc in sms)
                                {
                                    dc.MaKhachHang = pat.pa.MaKhachHang;
                                    db.SubmitChanges();
                                }
                            }
                        }
                    }
                }

            }
            catch
            {

            }
            return reponse;
        }
        #endregion
    }
}
