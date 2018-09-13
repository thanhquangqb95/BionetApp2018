using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BioNetBLL;
using BioNetModel;
using BioNetModel.Data;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Spreadsheet;
using System.IO;
using System.Diagnostics;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmGanViTriMayXN : DevExpress.XtraEditors.XtraForm
    {
        public FrmGanViTriMayXN(PsEmployeeLogin EMP)
        {
            emp = EMP;
            InitializeComponent();
        }
        public static PsEmployeeLogin emp = new PsEmployeeLogin();
        private List<PSCMGanViTriChung> vt = new List<PSCMGanViTriChung>();
        private List<PSDanhMucMayXN> dsmay = new List<PSDanhMucMayXN>();
        private List<PSMapsViTriMayXN> mapViTri = new List<PSMapsViTriMayXN>();
        private static List<ViTriXN> vtMayXN01 = new List<ViTriXN>();
        private static List<ViTriXN> vtMayXN02 = new List<ViTriXN>();
        private List<PSGanViTriXNKQ> lstMauChoKQ = new List<PSGanViTriXNKQ>();
        private List<PSDanhMucDonViCoSo> lstDonVi = new List<PSDanhMucDonViCoSo>();
        private List<PSDanhMucChiCuc> lstChiCuc = new List<PSDanhMucChiCuc>();
        private List<PSDanhMucDonViCoSo> lstDonViResponsitory = new List<PSDanhMucDonViCoSo>();
        private List<PSDanhMucGoiDichVuChung> lstgoiXN = new List<PSDanhMucGoiDichVuChung>();

        private void FrmGanViTriMayXN_Load(object sender, EventArgs e)
        {

            dsmay = BioNet_Bus.GetDSMayXN();
            mapViTri = new List<PSMapsViTriMayXN>();
            if (dsmay.Count > 0)
            {
                foreach (var may in dsmay)
                {
                    mapViTri.AddRange(BioNet_Bus.GetDSMapViTriMayXN(may.IDMayXN));
                    List<PSMapsViTriMayXN> a = BioNet_Bus.GetDSMapViTriMayXN(may.IDMayXN);
                }
            }
            LoadGoiDichVuXetNGhiem();
            cbbMayXN.EditValue = "MAYXN00";
            cbbGanVT.EditValue = 0;
            this.LoadDSDaLuu();
            this.LOadDanhMuc();
            this.LoadLstChuaKetQua();
            this.txtTuNgay_ChuaKQ.EditValue = DateTime.Now;
            this.txtDenNgay_ChuaKQ.EditValue = DateTime.Now;
        }

        #region Load Danh mục
        private void LOadDanhMuc()
        {
            try
            {
                this.lstChiCuc.Clear();
                this.lstChiCuc = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
                this.cbbChiCucChuaCoKQ.Properties.DataSource = null;
                this.cbbChiCucChuaCoKQ.Properties.DataSource = this.lstChiCuc;
                this.cbbChiCucChuaCoKQ.EditValue = "all";
                this.lstDonViResponsitory.Clear();
                this.lstDonVi.Clear();
                this.lstDonVi = BioNet_Bus.GetDanhSachDonViCoSo();
                this.repositoryItemLookUpDonVi_GCChuaCoKQ.DataSource = this.lstDonVi;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi khi load danh sách chi cục \r\n Lỗi chi tiết :" + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.cbbChiCucChuaCoKQ.Focus();
            }
        }
        private void LoadDSDaLuu()
        {
            this.vt.Clear();
            vtMayXN01.Clear();
            vtMayXN02.Clear();
            vt = BioNet_Bus.GetDanhSachGanXNLuu();
            var m1 = vt.Where(x => x.MAYXN01 != null).Select(x => x.MAYXN01).ToList();
            foreach (var m in m1)
            {
                ViTriXN v = new ViTriXN();
                v.STT = m.STT;
                v.STTDia = m.STTDia;
                v.isTest = m.isTest;
                v.STTVT = m.STTVT;
                v.ViTri = m.ViTri;
                vtMayXN01.Add(v);
            }
            var m2 = vt.Where(x => x.MAYXN02 != null).Select(x => x.MAYXN02).ToList();
            foreach (var m in m2)
            {
                ViTriXN v = new ViTriXN();
                v.STT = m.STT;
                v.STTDia = m.STTDia;
                v.isTest = m.isTest;
                v.STTVT = m.STTVT;
                v.ViTri = m.ViTri;
                vtMayXN02.Add(v);
            }
            this.LoadDanhSachGanVT();
        }
        private void LoadDanhSachGanVT()
        {
            this.LookupMayXN01.DataSource = null;
            this.LookupMayXN01.DataSource = vtMayXN01.ToList().OrderBy(x=>x.STT);
            this.LookupMayXN02.DataSource = null;
            this.LookupMayXN02.DataSource = vtMayXN02.ToList().OrderBy(x => x.STT);
            this.GCDanhSachGanViTri.DataSource = null;
            this.GCDanhSachGanViTri.DataSource = vt;
            this.GVDanhSachGanViTri.OptionsSelection.MultiSelect = false;
            this.GVDanhSachGanViTri.ClearSelection();
            this.GVDanhSachGanViTri.FocusedRowHandle = vt.Count();
            this.GVDanhSachGanViTri.SelectRow(vt.Count());
            this.GVDanhSachGanViTri.MoveLastVisible();
        }

        private void LoadLstChuaKetQua()
        {
            this.lstMauChoKQ.Clear();
            this.lstMauChoKQ = BioNet_Bus.GetDSChuaGanXN();
            this.GCChuaKQ.DataSource = null;
            this.GCChuaKQ.DataSource = this.lstMauChoKQ;
            this.GVChuaKQ.ExpandAllGroups();
        }
        
        private void LoadGoiDichVuXetNGhiem()
        {
            try
            {
                this.lstgoiXN = BioNet_Bus.GetDanhsachGoiDichVuChung();
                
                PSDanhMucGoiDichVuChung goi = new PSDanhMucGoiDichVuChung();
                goi.IDGoiDichVuChung = "DVGXNL2";
                goi.TenGoiDichVuChung = "Xét Nghiệm Lần 2";
                goi.Stt = 6;
                goi.DonGia = 0;
                goi.ChietKhau = 0;
                this.lstgoiXN.Add(goi);
                PSDanhMucGoiDichVuChung goi2 = new PSDanhMucGoiDichVuChung();
                goi2.IDGoiDichVuChung = "DVTest";
                goi2.TenGoiDichVuChung = "TEST";
                goi2.Stt = 7;
                goi2.DonGia = 0;
                goi2.ChietKhau = 0;
                this.lstgoiXN.Add(goi2);
                PSDanhMucGoiDichVuChung goi3 = new PSDanhMucGoiDichVuChung();
                goi3.IDGoiDichVuChung = "DVKhac";
                goi3.TenGoiDichVuChung = "Khac";
                goi3.Stt = 8;
                goi3.DonGia = 0;
                goi3.ChietKhau = 0;
                this.lstgoiXN.Add(goi3);
                this.LookupGoiXN.DataSource = this.lstgoiXN;
                PSDanhMucGoiDichVuChung goi4 = new PSDanhMucGoiDichVuChung();
                goi4.IDGoiDichVuChung = "ALL";
                goi4.TenGoiDichVuChung = "Tất cả";
                goi4.Stt = 9;
                goi4.DonGia = 0;
                goi4.ChietKhau = 0;
                this.lstgoiXN.Add(goi4);
                this.lookUpGoiXN_GCChuaCoKQ.DataSource = this.lstgoiXN;
                this.cbbGoiXNLoc.Properties.DataSource = this.lstgoiXN;
                this.cbbGoiXNLoc.EditValue = "ALL";
            }
            catch { }
        }

        #endregion

        #region Chỉnh sửa danh sách
        private void GanVaoDS(PSCMGanViTriChung ph)
        {
            try
            {
                PSCMGanViTriChung kq = new PSCMGanViTriChung();
                bool gg = true;
                List<PSCMGanViTriChung> vtN = vt.Where(x => x.MaXetNghiem != null).ToList();
                kq = vtN.FirstOrDefault(x => x.MaXetNghiem.Equals(ph.MaXetNghiem));
                if (kq == null)
                {
                    SplashScreenManager.ShowForm(this, typeof(DiaglogFrm.Waitingfrom), true, true, false);
                    if (ph.may.Count() > 0)
                    {
                        PSCMGanViTriChung vtNull = vt.FirstOrDefault(x => x.MaXetNghiem == null && x.MaGoiXN == null);
                        if (vtNull != null)
                        {
                            int countMay = (from pscm in vtNull.may
                                            join phcm in ph.may on pscm.IDMayXN equals phcm.IDMayXN
                                            where pscm.IDMayXN == phcm.IDMayXN
                                            select new { phcm }).Count();
                            if (countMay == vtNull.may.Count())
                            {
                                ph.STT_bang = vtNull.STT_bang;
                                ph.IDRowGanXN = vtNull.IDRowGanXN;
                                ph.IDLanGanXN = vtNull.IDLanGanXN;
                                foreach (var pm in ph.may)
                                {
                                    switch (pm.IDMayXN)
                                    {
                                        case "MAYXN01":
                                            {
                                                ph.MAYXN01 = vtNull.MAYXN01;
                                                break;
                                            }
                                        case "MAYXN02":
                                            {
                                                ph.MAYXN02 = vtNull.MAYXN02;
                                                break;
                                            }
                                    }
                                }
                                BioNet_Bus.SuaDanhSachGanXNLuu(ph);
                                gg = true;
                            }
                            else
                            {
                                gg = false;
                            }
                        }
                        else
                        {
                            gg = false;
                        }
                        if (gg != true)
                        {
                            string MaLanGan = BioNet_Bus.GetMaRowIDLanGanXN();
                            ph.IDLanGanXN = MaLanGan;
                            foreach (var ctm in ph.may)
                            {
                                do
                                {
                                    long? SttCt = 1;
                                    switch (ctm.IDMayXN)
                                    {
                                        case "MAYXN01":
                                            {
                                                SttCt = vtMayXN01.Count() + 1;
                                                break;
                                            }
                                        case "MAYXN02":
                                            {
                                                SttCt = vtMayXN02.Count() + 1;
                                                break;
                                            }
                                        default:
                                            break;
                                    }
                                    long? MaxViTri = mapViTri.Where(x => x.IDMayXN == ctm.IDMayXN).Count();
                                    long? STTVTGan = SttCt % MaxViTri == 0 ? MaxViTri : SttCt % MaxViTri;
                                    long? STTDia = 1;
                                    if (SttCt % MaxViTri == 0)
                                    {
                                        STTDia = SttCt / MaxViTri;
                                    }
                                    else
                                    {
                                        STTDia = SttCt / MaxViTri + 1;
                                    }
                                    var VitriGan = mapViTri.FirstOrDefault(x => x.IDMayXN == ctm.IDMayXN && x.STT == STTVTGan);
                                    ViTriXN vtxn = new ViTriXN
                                    {
                                        ViTri = STTDia + VitriGan.TenViTri,
                                        STTDia = STTDia,
                                        STT = SttCt,
                                        STTVT = STTVTGan,
                                        isTest = false
                                    };
                                    switch (ctm.IDMayXN)
                                    {
                                        case "MAYXN01":
                                            {

                                                if (VitriGan.isTest != true)
                                                {
                                                    //vtMayXN01.Add(vtxn);
                                                    ph.MAYXN01 = new ViTriXN();
                                                    ph.MAYXN01.STTVT = vtxn.STTVT;
                                                    ph.MAYXN01.STTDia = vtxn.STTDia;
                                                    ph.MAYXN01.ViTri = vtxn.ViTri;
                                                    ph.MAYXN01.isTest = vtxn.isTest;
                                                    ph.MAYXN01.STT = vtxn.STT;
                                                }
                                                break;
                                            }
                                        case "MAYXN02":
                                            {
                                                if (VitriGan.isTest != true)
                                                {
                                                    //vtMayXN02.Add(vtxn);
                                                    ph.MAYXN02 = new ViTriXN();
                                                    ph.MAYXN02.STTVT = vtxn.STTVT;
                                                    ph.MAYXN02.STTDia = vtxn.STTDia;
                                                    ph.MAYXN02.ViTri = vtxn.ViTri;
                                                    ph.MAYXN02.isTest = vtxn.isTest;
                                                    ph.MAYXN02.STT = vtxn.STT;
                                                }

                                                break;
                                            }
                                        default:
                                            break;
                                    }

                                } while (GanViTriTest(ctm.IDMayXN));
                            }
                            ph.STT_bang = vt.Count + 1;
                            PsReponse rs = BioNet_Bus.SaveGanXN(ph);
                            if (ph.MaPhieu != null)
                            {
                                var ganthaydoi = lstMauChoKQ.FirstOrDefault(x => x.MaPhieu == ph.MaPhieu && x.MaXetNghiem == ph.MaXetNghiem);
                                if (ganthaydoi.isDaDuyet != true)
                                {
                                    ganthaydoi.isDaDuyet = false;
                                }
                            }
                        }
                    }
                    // LoadLstChuaKetQua();
                    LoadDSDaLuu();
                    SplashScreenManager.CloseForm();
                }
                else
                {
                    DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Mã xét nghiệm đã tồn tại trong danh sách.");
                    warning.ShowDialog();
                }
            }
            catch
            {

            }           
        }
        private bool GanViTriTest(string IDMayXN)

        {
            bool result = false;
            try
            {
                long? SttCt = 1;
                switch (IDMayXN)
                {
                    case "MAYXN01":
                        {
                            SttCt = vtMayXN01.Count() + 1;
                            break;
                        }
                    case "MAYXN02":
                        {
                            SttCt = vtMayXN02.Count() + 1;
                            break;
                        }
                    default:
                        break;
                }
                long? MaxViTri = mapViTri.Where(x => x.IDMayXN == IDMayXN).Count();
                long? STTVTGan = SttCt % MaxViTri == 0 ? MaxViTri : SttCt % MaxViTri;
                long? STTDia = 1;
                if (SttCt % MaxViTri == 0)
                {
                    STTDia = SttCt / MaxViTri;
                }
                else
                {
                    STTDia = SttCt / MaxViTri + 1;
                }
                STTDia = SttCt / MaxViTri + 1;
                var VitriGan = mapViTri.FirstOrDefault(x => x.IDMayXN == IDMayXN && x.STT == STTVTGan);
                if (VitriGan.isTest == true)
                {
                    result = true;
                    PSCMGanViTriChung test = new PSCMGanViTriChung();
                    test.MaXetNghiem = VitriGan.GiaTriTest;
                    test.STT_bang = vt.Count + 1;
                    test.MaGoiXN = "DVTest";
                    string MaLanGan = BioNet_Bus.GetMaRowIDLanGanXN();
                    test.IDLanGanXN = MaLanGan;
                    ViTriXN vtxn = new ViTriXN
                    {
                        ViTri = STTDia + VitriGan.TenViTri,
                        STTDia = STTDia,
                        STT = SttCt,
                        STTVT = STTVTGan,
                        isTest = true
                    };
                    switch (VitriGan.IDMayXN)
                    {
                        case "MAYXN01":
                            {
                                test.MAYXN01 = new ViTriXN();
                                vtMayXN01.Add(vtxn);
                                test.MAYXN01.STTVT = vtxn.STTVT;
                                test.MAYXN01.STTDia = vtxn.STTDia;
                                test.MAYXN01.ViTri = vtxn.ViTri;
                                test.MAYXN01.isTest = vtxn.isTest;
                                test.MAYXN01.STT = vtxn.STT;
                                break;
                            }
                        case "MAYXN02":
                            {
                                test.MAYXN02 = new ViTriXN();
                                vtMayXN02.Add(vtxn);
                                test.MAYXN02.STTVT = vtxn.STTVT;
                                test.MAYXN02.STTDia = vtxn.STTDia;
                                test.MAYXN02.ViTri = vtxn.ViTri;
                                test.MAYXN02.isTest = vtxn.isTest;
                                test.MAYXN02.STT = vtxn.STT;
                                break;
                            }
                        default:
                            break;
                    }
                    BioNet_Bus.SaveGanXN(test);
                    vt.Add(test);
                }
            }
            catch
            {
            }
            return result;
        }
        #endregion

        #region Xuất file
        private void btnXuatFile_Click(object sender, EventArgs e)
        {
            List<PsRptDanhSachDaCapMaXetNghiem> data = new List<PsRptDanhSachDaCapMaXetNghiem>();
            List<pro_ReportGanViTriMayXNResult> dataGanVT = new List<pro_ReportGanViTriMayXNResult>();
            try
            {
                BioNet_Bus.DuyetCapMaGanMayXN(vt);
                string link = Application.StartupPath + @"\DSSoDoXetNghiem\";
                if (!Directory.Exists(link))
                {
                    Directory.CreateDirectory(link);
                }
                Workbook workbook = new DevExpress.Spreadsheet.Workbook();
                Reports.RepostsCapMaXetNghiep.rptReportGanViTriMAYXN01 rp1 = new Reports.RepostsCapMaXetNghiep.rptReportGanViTriMAYXN01();
                string Idlangan = vt.Distinct().Select(x => x.IDLanGanXN).FirstOrDefault().ToString();
                var vtmay1= vt.Where(y => y.MAYXN01 != null).OrderBy(x => x.MAYXN01.STT);
                rp1.DataSource = vtmay1.Where(y => y.MAYXN01 != null).OrderBy(x => x.MAYXN01.STT);
                rp1.PaperName = "May3Benh";
                rp1.Parameters["TenNV"].Value = emp.EmployeeName;
                rp1.Parameters["NgayTaoDS"].Value = DateTime.Now.ToString();
                rp1.Parameters["SLGoi5benh"].Value = vtmay1.Where(y => y.MAYXN01 != null && y.MaGoiXN == "DVGXN0004").Count();
                rp1.Parameters["SLGoi3benh"].Value = vtmay1.Where(y => y.MAYXN01 != null && y.MaGoiXN == "DVGXN0003").Count();
                rp1.Parameters["SLGoi2benh"].Value = vtmay1.Where(y => y.MAYXN01 != null && y.MaGoiXN == "DVGXN0002").Count();
                rp1.Parameters["SLXNL"].Value = vt.Where(y => y.MAYXN01 != null && y.MaGoiXN == "DVGXN0001").Count();
                rp1.Parameters["SLXNL2benh"].Value = vtmay1.Where(y => y.MAYXN01 != null && y.MaGoiXN == "DVGXNL2").Count();
                rp1.Parameters["SLXNKhac"].Value = vtmay1.Where(y => y.MAYXN01 != null && y.MaGoiXN == "DVKhac").Count();
                rp1.ExportToXlsx("May3Benh" + ".xlsx", new DevExpress.XtraPrinting.XlsxExportOptions() { SheetName = "May3Benh" });

                Workbook workbook2 = new DevExpress.Spreadsheet.Workbook();
                using (FileStream stream = new FileStream("May3Benh" + ".xlsx", FileMode.Open))
                {
                    workbook2.LoadDocument(stream, DocumentFormat.Xlsx);
                }
                workbook.Worksheets.Insert(0, "May3Benh");
                workbook.Worksheets[0].CopyFrom(workbook2.Worksheets[0]);
                File.Delete("May3Benh" + ".xlsx");
                var vtmay2= vt.Where(y => y.MAYXN02 != null).OrderBy(x => x.MAYXN02.STT);
                Reports.RepostsCapMaXetNghiep.rptReportGanViTriMAYXN02 rp2 = new Reports.RepostsCapMaXetNghiep.rptReportGanViTriMAYXN02();
                rp2.DataSource = vtmay2.Where(y => y.MAYXN02 != null).OrderBy(x => x.MAYXN02.STT);
                rp2.Parameters["SLGoi5benh"].Value = vtmay2.Where(y => y.MAYXN02 != null && y.MaGoiXN == "DVGXN0004").Count();
                rp2.Parameters["SLGoi3benh"].Value = vtmay2.Where(y => y.MAYXN02 != null && y.MaGoiXN == "DVGXN0003").Count();
                rp2.Parameters["SLGoi2benh"].Value = vtmay2.Where(y => y.MAYXN02 != null && y.MaGoiXN == "DVGXN0002").Count();
                rp2.Parameters["SLXNL"].Value = vtmay2.Where(y => y.MAYXN02 != null && y.MaGoiXN == "DVGXN0001").Count();
                rp2.Parameters["SLXNL2benh"].Value = vtmay2.Where(y => y.MAYXN02 != null && y.MaGoiXN == "DVGXNL2").Count();
                rp2.Parameters["SLXNKhac"].Value = vtmay2.Where(y => y.MAYXN02 != null && y.MaGoiXN == "DVKhac").Count();
                rp2.PaperName = "May2Benh";
                rp2.Parameters["TenNV"].Value = emp.EmployeeName;
                rp2.Parameters["NgayTaoDS"].Value = DateTime.Now.ToString();
                rp2.ExportToXlsx("May2Benh" + ".xlsx", new DevExpress.XtraPrinting.XlsxExportOptions() { SheetName = "May2Benh" });
                Workbook workbook3 = new DevExpress.Spreadsheet.Workbook();
                using (FileStream stream = new FileStream("May2Benh" + ".xlsx", FileMode.Open))
                {
                    workbook3.LoadDocument(stream, DocumentFormat.Xlsx);
                }
                workbook.Worksheets.Insert(1, "May2Benh");
                workbook.Worksheets[1].CopyFrom(workbook3.Worksheets[0]);
                File.Delete("May2Benh" + ".xlsx");

                string l = link + "SodoXetNghiemNgay" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + Idlangan+ ".xlsx";  
                    workbook.SaveDocument(l);
                    System.Diagnostics.Process.Start(l);
                    LoadLstChuaKetQua();
                    LoadDSDaLuu();
                
            }
            catch
            {
                DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Xuất file lỗi.");
                warning.ShowDialog();
            }
        }
        private void btnExportReview_Click(object sender, EventArgs e)
        {
            List<PsRptDanhSachDaCapMaXetNghiem> data = new List<PsRptDanhSachDaCapMaXetNghiem>();
            List<pro_ReportGanViTriMayXNResult> dataGanVT = new List<pro_ReportGanViTriMayXNResult>();
            try
            {
                string link = Application.StartupPath + @"\DSSoDoXetNghiem\";
                if (!Directory.Exists(link))
                {
                    Directory.CreateDirectory(link);
                }
                Workbook workbook = new DevExpress.Spreadsheet.Workbook();
                Reports.RepostsCapMaXetNghiep.rptReportGanViTriMAYXN01 rp1 = new Reports.RepostsCapMaXetNghiep.rptReportGanViTriMAYXN01();
                string Idlangan = vt.Distinct().Select(x => x.IDLanGanXN).FirstOrDefault().ToString();
                var vtmay1 = vt.Where(y => y.MAYXN01 != null).OrderBy(x => x.MAYXN01.STT);
                rp1.DataSource = vtmay1.Where(y => y.MAYXN01 != null).OrderBy(x => x.MAYXN01.STT);
                rp1.PaperName = "May3Benh";
                rp1.Parameters["TenNV"].Value = emp.EmployeeName;
                rp1.Parameters["NgayTaoDS"].Value = DateTime.Now.ToString();
                rp1.Parameters["SLGoi5benh"].Value = vtmay1.Where(y => y.MAYXN01 != null && y.MaGoiXN == "DVGXN0004").Count();
                rp1.Parameters["SLGoi3benh"].Value = vtmay1.Where(y => y.MAYXN01 != null && y.MaGoiXN == "DVGXN0003").Count();
                rp1.Parameters["SLGoi2benh"].Value = vtmay1.Where(y => y.MAYXN01 != null && y.MaGoiXN == "DVGXN0002").Count();
                rp1.Parameters["SLXNL"].Value = vt.Where(y => y.MAYXN01 != null && y.MaGoiXN == "DVGXN0001").Count();
                rp1.Parameters["SLXNL2benh"].Value = vtmay1.Where(y => y.MAYXN01 != null && y.MaGoiXN == "DVGXNL2").Count();
                rp1.Parameters["SLXNKhac"].Value = vtmay1.Where(y => y.MAYXN01 != null && y.MaGoiXN == "DVKhac").Count();
                rp1.ExportToXlsx("May3Benh" + ".xlsx", new DevExpress.XtraPrinting.XlsxExportOptions() { SheetName = "May3Benh" });

                Workbook workbook2 = new DevExpress.Spreadsheet.Workbook();
                using (FileStream stream = new FileStream("May3Benh" + ".xlsx", FileMode.Open))
                {
                    workbook2.LoadDocument(stream, DocumentFormat.Xlsx);
                }
                workbook.Worksheets.Insert(0, "May3Benh");
                workbook.Worksheets[0].CopyFrom(workbook2.Worksheets[0]);
                File.Delete("May3Benh" + ".xlsx");
                var vtmay2 = vt.Where(y => y.MAYXN02 != null).OrderBy(x => x.MAYXN02.STT);
                Reports.RepostsCapMaXetNghiep.rptReportGanViTriMAYXN02 rp2 = new Reports.RepostsCapMaXetNghiep.rptReportGanViTriMAYXN02();
                rp2.DataSource = vtmay2.Where(y => y.MAYXN02 != null).OrderBy(x => x.MAYXN02.STT);
                rp2.Parameters["SLGoi5benh"].Value = vtmay2.Where(y => y.MAYXN02 != null && y.MaGoiXN == "DVGXN0004").Count();
                rp2.Parameters["SLGoi3benh"].Value = vtmay2.Where(y => y.MAYXN02 != null && y.MaGoiXN == "DVGXN0003").Count();
                rp2.Parameters["SLGoi2benh"].Value = vtmay2.Where(y => y.MAYXN02 != null && y.MaGoiXN == "DVGXN0002").Count();
                rp2.Parameters["SLXNL"].Value = vtmay2.Where(y => y.MAYXN02 != null && y.MaGoiXN == "DVGXN0001").Count();
                rp2.Parameters["SLXNL2benh"].Value = vtmay2.Where(y => y.MAYXN02 != null && y.MaGoiXN == "DVGXNL2").Count();
                rp2.Parameters["SLXNKhac"].Value = vtmay2.Where(y => y.MAYXN02 != null && y.MaGoiXN == "DVKhac").Count();
                rp2.PaperName = "May2Benh";
                rp2.Parameters["TenNV"].Value = emp.EmployeeName;
                rp2.Parameters["NgayTaoDS"].Value = DateTime.Now.ToString();
                rp2.ExportToXlsx("May2Benh" + ".xlsx", new DevExpress.XtraPrinting.XlsxExportOptions() { SheetName = "May2Benh" });
                Workbook workbook3 = new DevExpress.Spreadsheet.Workbook();
                using (FileStream stream = new FileStream("May2Benh" + ".xlsx", FileMode.Open))
                {
                    workbook3.LoadDocument(stream, DocumentFormat.Xlsx);
                }
                workbook.Worksheets.Insert(1, "May2Benh");
                workbook.Worksheets[1].CopyFrom(workbook3.Worksheets[0]);
                File.Delete("May2Benh" + ".xlsx");

                string l = link + "SodoXetNghiemReviewNgay" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + Idlangan + DateTime.Now.Hour + DateTime.Now.Minute + ".xlsx";
                workbook.SaveDocument(l);
                System.Diagnostics.Process.Start(l);
                LoadLstChuaKetQua();
                LoadDSDaLuu();

            }
            catch
            {
                DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Xuất file lỗi.");
                warning.ShowDialog();
            }
        }
        #endregion

        #region Style 
        private void GVDanhSachGanViTri_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                GridView View = sender as GridView;
                if (e.RowHandle >= 0)
                {
                    string Stt = View.GetRowCellDisplayText(e.RowHandle, col_STT);
                    if (e.Column.FieldName == col_MaGoiXetNghiem.FieldName)
                    {
                        var MaXN = View.GetRowCellValue(e.RowHandle, col_MaGoiXetNghiem);
                        switch (MaXN.ToString())
                        {
                            case "DVKhac":
                                {
                                    e.Appearance.BackColor = Color.DarkSeaGreen;
                                    break;
                                }
                            case "DVGXN0001":
                                {
                                    e.Appearance.BackColor = Color.Bisque;
                                    break;
                                }
                            case "DVGXN0002":
                                {
                                    e.Appearance.BackColor = Color.Bisque;
                                    break;
                                }
                            case "DVGXNL2":
                                {
                                    e.Appearance.BackColor = Color.Bisque;
                                    break;
                                }
                            case "DVTest":
                                {
                                    e.Appearance.BackColor = Color.BurlyWood;
                                    break;
                                }
                            default:
                                {
                                    e.Appearance.BackColor = Color.Transparent;
                                    break;
                                }
                        }
                    }
                    try
                    {
                        if (e.Column.FieldName == col_MaXetNghiem.FieldName)
                        {
                            var MaXN = View.GetRowCellDisplayText(e.RowHandle, col_MaGoiXetNghiem);
                            switch (MaXN.ToString())
                            {
                                case "0":
                                    {
                                        e.Appearance.BackColor = Color.DarkSeaGreen;
                                        break;
                                    }
                                default:
                                    {
                                        e.Appearance.BackColor = Color.Transparent;
                                        break;
                                    }
                            }
                        }
                    }
                    catch
                    {

                    }
                    try
                    {
                        if (e.Column.FieldName == col_MayXN01_STTDia.FieldName)
                        {

                            int STTVT = int.Parse(View.GetRowCellDisplayText(e.RowHandle, col_MayXN01_STTDia)) % 2;
                            switch (STTVT)
                            {
                                case 1:
                                    {
                                        e.Appearance.BackColor = Color.LightSkyBlue;
                                        break;
                                    }
                                case 0:
                                    {
                                        e.Appearance.BackColor = Color.PapayaWhip;
                                        break;
                                    }
                                default:
                                    {
                                        e.Appearance.BackColor = Color.Transparent;
                                        break;
                                    }
                            }
                        }
                    }
                    catch
                    {

                    }
                    try
                    {
                        if (e.Column.FieldName == col_MayXN02_STTDia.FieldName)
                        {

                            int STTVT = int.Parse(View.GetRowCellDisplayText(e.RowHandle, col_MayXN02_STTDia)) % 2;
                            switch (STTVT)
                            {
                                case 1:
                                    {
                                        e.Appearance.BackColor = Color.LightSkyBlue;
                                        break;
                                    }
                                case 0:
                                    {
                                        e.Appearance.BackColor = Color.PapayaWhip;
                                        break;
                                    }
                                default:
                                    {
                                        e.Appearance.BackColor = Color.Transparent;
                                        break;
                                    }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch { }
        }
        private void GVChuaKQ_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                GridView View = sender as GridView;
                if (e.RowHandle >= 0)
                {
                    string daganVT = View.GetRowCellValue(e.RowHandle, this.col_isDaDuyet).ToString();
                    switch(daganVT)
                    {
                        case ("True"):
                            {
                                e.Appearance.BackColor = Color.BurlyWood;
                                e.Appearance.BackColor2 = Color.Bisque;
                                break;
                            }
                        case ("False"):
                            {
                                e.Appearance.BackColor = Color.Salmon;
                                e.Appearance.BackColor2 = Color.SeaShell;
                                break;
                            }
                         default:
                            {
                                e.Appearance.BackColor = Color.Transparent;
                                break;
                            }
                    }
                }
            }
            catch { }
        }
        #endregion

        #region Sụ kiện phím
        private void GVDanhSachGanViTri_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    if (MessageBox.Show("Bạn có chắn chắn xóa phiếu không?", "Cảnh báo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;
                    GridView view = sender as GridView;
                    int rowfocus = view.FocusedRowHandle;
                    var stt = view.GetRowCellValue(rowfocus, this.col_STT);
                    var idPhieu = view.GetRowCellValue(rowfocus, this.col_IDPhieu);
                    var maxn = view.GetRowCellValue(rowfocus, this.col_MaXetNghiem);
                    if(maxn==null)
                    {
                        PSCMGanViTriChung rss = vt.FirstOrDefault(p => p.STT_bang == int.Parse(stt.ToString()) && p.MaXetNghiem==null);
                        BioNet_Bus.DeleteDanhSachGanXNLuu(rss);
                    }
                    else
                    {
                        PSCMGanViTriChung rss = vt.FirstOrDefault(p => p.STT_bang == int.Parse(stt.ToString()) && p.MaXetNghiem == maxn.ToString());
                        BioNet_Bus.DeleteDanhSachGanXNLuu(rss);
                    }
                   
                    LoadDSDaLuu();
                }
            }
            catch
            {
            }
        }
        private void txtMaPhieu_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Enter))
                {
                    PSCMGanViTriChung ph = BioNet_Bus.GetPhieuChuaCoKQ(txtMaPhieu.Text);
                    if (ph.MaPhieu == null)
                    {
                        DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Mã phiếu không hợp lệ.");
                        warning.ShowDialog();
                    }
                    else
                    {
                        GanVaoDS(ph);
                    }
                    txtMaPhieu.ResetText();
                    txtMaPhieu.Focus();
                }
            }
            catch
            {
            }
        }
        private void txtMaNgoai_KeyPress(object sender, KeyPressEventArgs e)
        {

            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Enter))
                {
                    string MaXNNgoai = txtMaNgoai.Text.TrimEnd();

                    string MaMayNgoai = cbbMayXN.EditValue.ToString();
                    PSCMGanViTriChung ph = new PSCMGanViTriChung();
                    ph.may = new List<PSDanhMucMayXN>();
                    if (string.IsNullOrEmpty(MaMayNgoai))
                    {

                    }
                    else
                    {
                        if (MaMayNgoai.Equals("MAYXN00"))
                        {
                            ph.may = dsmay;
                        }
                        else
                        {
                            ph.may = dsmay.Where(x => x.IDMayXN.Equals(MaMayNgoai)).ToList();
                        }
                        ph.MaXetNghiem = MaXNNgoai;
                        ph.MaGoiXN = "DVKhac";
                    }
                    GanVaoDS(ph);
                    txtMaNgoai.ResetText();
                    txtMaNgoai.Focus();
                }
            }
            catch
            {
            }
        }
        private void GVDanhSachGanViTri_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                var columnHandle = e.Column.ColumnHandle;
                int rowfocus = e.RowHandle;
                var stt = view.GetRowCellValue(rowfocus, this.col_STT);
                var idPhieu = view.GetRowCellValue(rowfocus, this.col_IDPhieu);
                var maxn = view.GetRowCellValue(rowfocus, this.col_MaXetNghiem);
                var idrow = view.GetRowCellValue(rowfocus, this.col_IDRow);
                if (rowfocus < view.RowCount)
                {
                    if (columnHandle == this.col_MayXN01_ViTri.ColumnHandle)
                    {
                        var tenvitri = view.GetRowCellDisplayText(rowfocus, this.col_MayXN01_ViTri);
                        List<PSCMGanViTriChung> rss = vt.Where(p => p.MAYXN01 != null).ToList();
                        PSCMGanViTriChung rsmoi = rss.Where(p => p.MAYXN01.ViTri == tenvitri.ToString() && p.STT_bang == long.Parse(stt.ToString())).FirstOrDefault();
                        ViTriXN vtmoi = vtMayXN01.Where(p => p.ViTri == tenvitri.ToString()).FirstOrDefault();
                        var valueOld = view.ActiveEditor.OldEditValue;
                        ViTriXN vtcu = vtMayXN01.Where(p => p.ViTri == valueOld.ToString()).FirstOrDefault();
                        PSCMGanViTriChung rsthay = rss.Where(p => p.MAYXN01.ViTri == tenvitri.ToString() && p.MAYXN01.STT == long.Parse(vtmoi.STT.ToString())).FirstOrDefault();
                        if (vtcu != null && rsmoi != null && rsthay != null)
                        {
                            long? gt = rsthay.STT_bang;
                            long? SttCtMax = vtMayXN01.Count();
                            rsmoi.MAYXN01.STT = vtmoi.STT;
                            rsmoi.MAYXN01.STTDia = vtmoi.STTDia;
                            rsmoi.MAYXN01.STTVT = vtmoi.STTVT;
                            rsmoi.MAYXN01.ViTri = vtmoi.ViTri;
                            rsthay.MAYXN01.STT = vtcu.STT;
                            rsthay.MAYXN01.STTDia = vtcu.STTDia;
                            rsthay.MAYXN01.STTVT = vtcu.STTVT;
                            rsthay.MAYXN01.ViTri = vtcu.ViTri;
                            if (vtmoi.isTest == true)
                            {
                                rsmoi.MAYXN01.isTest = false;
                                vtmoi.isTest = false;
                                rsthay.MAYXN01.isTest = true;
                            }
                            BioNet_Bus.SuaDanhSachGanXNLuu(rsmoi);
                            BioNet_Bus.SuaDanhSachGanXNLuu(rsthay);
                            this.LoadDSDaLuu();
                        }
                    }
                    if (columnHandle == this.col_MayXN02_ViTri.ColumnHandle)
                    {
                        var tenvitri = view.GetRowCellDisplayText(rowfocus, this.col_MayXN02_ViTri);
                        List<PSCMGanViTriChung> rss = vt.Where(p => p.MAYXN02 != null).ToList();
                        PSCMGanViTriChung rsmoi = rss.Where(p => p.MAYXN01.ViTri == tenvitri.ToString() && p.STT_bang == long.Parse(stt.ToString())).FirstOrDefault();
                        ViTriXN vtmoi = vtMayXN02.Where(p => p.ViTri == tenvitri.ToString()).FirstOrDefault();
                        var valueOld = view.ActiveEditor.OldEditValue;
                        ViTriXN vtcu = vtMayXN02.Where(p => p.ViTri == valueOld.ToString()).FirstOrDefault();
                        PSCMGanViTriChung rsthay = rss.Where(p => p.MAYXN02.ViTri == tenvitri.ToString() && p.MAYXN02.STT == long.Parse(vtmoi.STT.ToString())).FirstOrDefault();
                        if (vtcu != null && rsmoi != null && rsthay != null)
                        {
                            long? gt = rsthay.STT_bang;
                            long? SttCtMax = vtMayXN01.Count();
                            rsmoi.MAYXN02.STT = vtmoi.STT;
                            rsmoi.MAYXN02.STTDia = vtmoi.STTDia;
                            rsmoi.MAYXN02.STTVT = vtmoi.STTVT;
                            rsmoi.MAYXN02.ViTri = vtmoi.ViTri;
                            rsthay.MAYXN02.STT = vtcu.STT;
                            rsthay.MAYXN02.STTDia = vtcu.STTDia;
                            rsthay.MAYXN02.STTVT = vtcu.STTVT;
                            rsthay.MAYXN02.ViTri = vtcu.ViTri;
                            if (vtmoi.isTest == true)
                            {
                                rsmoi.MAYXN02.isTest = false;
                                vtmoi.isTest = false;
                                rsthay.MAYXN02.isTest = true;
                            }
                            BioNet_Bus.SuaDanhSachGanXNLuu(rsmoi);
                            BioNet_Bus.SuaDanhSachGanXNLuu(rsthay);
                            this.LoadDSDaLuu();
                        }
                    }
                    if (columnHandle == this.col_IDPhieu.ColumnHandle)
                    {
                        PSCMGanViTriChung ph = BioNet_Bus.GetPhieuChuaCoKQ(idPhieu.ToString());
                        if(ph!=null)
                        { 
                            if (BioNet_Bus.GetMaPhieuDaCoDSCapVT(idPhieu.ToString()))
                            {
                                PSCMGanViTriChung rss = vt.FirstOrDefault(p => p.STT_bang == long.Parse(stt.ToString()) && p.IDRowGanXN == long.Parse(idrow.ToString()));
                                int countMay = (from pscm in rss.may
                                                join phcm in ph.may on pscm.IDMayXN equals phcm.IDMayXN
                                                where pscm.IDMayXN == phcm.IDMayXN
                                                select new { phcm }).Count();
                                if (countMay == rss.may.Count())
                                {
                                    ph.STT_bang = rss.STT_bang;
                                    ph.IDRowGanXN = rss.IDRowGanXN;
                                    ph.IDLanGanXN = rss.IDLanGanXN;
                                    foreach (var pm in ph.may)
                                    {
                                        switch (pm.IDMayXN)
                                        {
                                            case "MAYXN01":
                                                {
                                                    ph.MAYXN01 = rss.MAYXN01;
                                                    break;
                                                }
                                            case "MAYXN02":
                                                {
                                                    ph.MAYXN02 = rss.MAYXN02;
                                                    break;
                                                }
                                        }
                                    }
                                    BioNet_Bus.SuaDanhSachGanXNLuu(ph);
                                }
                                else
                                {
                                    DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Thay đổi mã phiếu không hợp lệ vui lòng kiểm tra lại.");
                                    warning.ShowDialog();
                                }
                            }
                            else
                            {
                                DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Mã phiếu đã có trong danh sách.");
                                warning.ShowDialog();
                            }                            
                        }
                        else
                        {
                            DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Mã phiếu không tồn tại.");
                            warning.ShowDialog();
                        }
                        LoadDSDaLuu();
                    }
                    if (columnHandle == this.col_MayXN02_GhiChu.ColumnHandle)
                    {
                        var tenvitri = view.GetRowCellDisplayText(rowfocus, this.col_MayXN02_ViTri);
                        var ghichu = view.GetRowCellDisplayText(rowfocus, this.col_MayXN02_GhiChu);
                        List<PSCMGanViTriChung> rss = vt.Where(p => p.MAYXN02 != null).ToList();
                        PSCMGanViTriChung rsmoi = rss.Where(p => p.MAYXN02.ViTri == tenvitri.ToString() && p.STT_bang == long.Parse(stt.ToString())).FirstOrDefault();
                        rsmoi.MAYXN02.GhiChuCT = ghichu.ToString();
                        BioNet_Bus.SuaDanhSachGanXNLuu(rsmoi);
                    }
                    if (columnHandle == this.col_GhiNhoChung.ColumnHandle)
                    {
                        var ghichu = view.GetRowCellDisplayText(rowfocus, this.col_GhiNhoChung);
                        PSCMGanViTriChung rsmoi = vt.Where(p => p.STT_bang == long.Parse(stt.ToString())).FirstOrDefault();
                        rsmoi.GhiChuChung = ghichu.ToString();
                        BioNet_Bus.SuaDanhSachGanXNLuu(rsmoi);
                    }
                    if (columnHandle == this.col_MayXN01_GhiChu.ColumnHandle)
                    {
                        var tenvitri = view.GetRowCellDisplayText(rowfocus, this.col_MayXN01_ViTri);
                        var ghichu = view.GetRowCellDisplayText(rowfocus, this.col_MayXN01_GhiChu);
                        List<PSCMGanViTriChung> rss = vt.Where(p => p.MAYXN01 != null).ToList();
                        PSCMGanViTriChung rsmoi = rss.Where(p => p.MAYXN01.ViTri == tenvitri.ToString() && p.STT_bang == long.Parse(stt.ToString())).FirstOrDefault();
                        rsmoi.MAYXN01.GhiChuCT = ghichu.ToString();
                        BioNet_Bus.SuaDanhSachGanXNLuu(rsmoi);
                    }
                    if (columnHandle == this.col_MayXN02_GhiChu.ColumnHandle)
                    {
                        var tenvitri = view.GetRowCellDisplayText(rowfocus, this.col_MayXN02_ViTri);
                        var ghichu = view.GetRowCellDisplayText(rowfocus, this.col_MayXN02_GhiChu);
                        List<PSCMGanViTriChung> rss = vt.Where(p => p.MAYXN02 != null).ToList();
                        PSCMGanViTriChung rsmoi = rss.Where(p => p.MAYXN02.ViTri == tenvitri.ToString() && p.STT_bang == long.Parse(stt.ToString())).FirstOrDefault();
                        rsmoi.MAYXN02.GhiChuCT = ghichu.ToString();
                        BioNet_Bus.SuaDanhSachGanXNLuu(rsmoi);
                    }
                }
                LoadLstChuaKetQua();
                LoadDSDaLuu();
            }
            catch
            {
                LoadLstChuaKetQua();
                LoadDSDaLuu();
            }
        }
        private void btnCapNhatDSChuaCoKQ_Click(object sender, EventArgs e)
        {
            LoadLstChuaKetQua();
            LoadDSDaLuu();
        }
        #endregion

        #region Lọc DS
        private void btnClear_Click(object sender, EventArgs e)
        {
            cbbChiCucChuaCoKQ.EditValue = "all";
            cbbDonViChuaCoKQ.EditValue = "all";
            cbbGoiXNLoc.EditValue = "ALL";
            cbbGanVT.EditValue = 2;
            txtTuNgay_ChuaKQ.EditValue = DateTime.Now;
            txtDenNgay_ChuaKQ.EditValue = DateTime.Now;
            txtSearchMaPhieuChuaCoKQ.ResetText();
            GVChuaKQ.ClearColumnsFilter();
        }
        private void cbbChiCucChuaCoKQ_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit sear = sender as SearchLookUpEdit;
                var value = sear.EditValue.ToString();
                this.cbbDonViChuaCoKQ.Properties.DataSource = null;
                this.cbbDonViChuaCoKQ.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi(value.ToString());
                this.cbbDonViChuaCoKQ.EditValue = "all";
                if (cbbChiCucChuaCoKQ.EditValue.ToString() != "all")
                {
                    FilterDSChuaCoKQTheoDonVi();
                }
                else
                {
                    GVChuaKQ.Columns["MaDonVi"].ClearFilter();
                }
            }
            catch { }
        }
        private void cbbGanVT_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string filterisDaDuyet = string.Empty;
                switch (cbbGanVT.EditValue)
                {
                    case 3:
                        {
                            filterisDaDuyet = "[isDaDuyet]=true";
                            break;
                        }
                    case 1:
                        {
                            filterisDaDuyet = "[isDaDuyet]=null";
                            break;
                        }
                    case 2:
                        {
                            filterisDaDuyet = "[isDaDuyet]=false";
                            break;
                        }
                    default:
                        {
                            break;
                        }

                }
                if(string.IsNullOrEmpty(filterisDaDuyet))
                {
                    GVChuaKQ.Columns["isDaDuyet"].ClearFilter();
                }
                else
                {
                    GVChuaKQ.Columns["isDaDuyet"].FilterInfo = new ColumnFilterInfo(filterisDaDuyet);
                    
                }
                this.GVChuaKQ.ExpandAllGroups();
            }
            catch { }
        }
        private void FilterDSChuaCoKQTheoDonVi() //Lọc theo đơn vị
        {
            string machicuc = cbbChiCucChuaCoKQ.EditValue.ToString();
            string madv = cbbDonViChuaCoKQ.EditValue.ToString();
            if (!machicuc.Equals("all") && madv.Equals("all"))
            {
                madv = machicuc;
            }
            if(!madv.Equals("all"))
            {
                string filterMaDV = "Contains([MaDonVi], '" + madv + "')";
                GVChuaKQ.Columns["MaDonVi"].FilterInfo = new ColumnFilterInfo(filterMaDV);
            }
            else {
                GVChuaKQ.Columns["MaDonVi"].ClearFilter();
            }
            this.GVChuaKQ.ExpandAllGroups();
        }

        private void cbbDonViChuaCoKQ_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                    FilterDSChuaCoKQTheoDonVi();
                this.GVChuaKQ.ExpandAllGroups();
            }
            catch { }
        }
        private void cbbGoiXNLoc_EditValueChanged(object sender, EventArgs e)
        {
            string MaGoiXN = cbbGoiXNLoc.EditValue.ToString();
            if (!MaGoiXN.Equals("ALL") )
            {
                string filterMaDV = "Contains([MaGoiXN], '" + MaGoiXN + "')";
                GVChuaKQ.Columns["MaGoiXN"].FilterInfo = new ColumnFilterInfo(filterMaDV);
            }
            else
            {
                GVChuaKQ.Columns["MaGoiXN"].ClearFilter(); 
            }
            this.GVChuaKQ.ExpandAllGroups();
        }
        private void txtSearchMaPhieuChuaCoKQ_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(txtSearchMaPhieuChuaCoKQ.EditValue.ToString()))
                {
                    string filterMaPhieu = "Contains([MaPhieu], '" + txtSearchMaPhieuChuaCoKQ.EditValue + "')";
                    GVChuaKQ.Columns["MaPhieu"].FilterInfo = new ColumnFilterInfo(filterMaPhieu);
                }
                else
                {
                    GVChuaKQ.Columns["MaPhieu"].ClearFilter();
                }
                
            }
            catch
            {
                GVChuaKQ.Columns["MaPhieu"].FilterInfo = new ColumnFilterInfo();
            }
            this.GVChuaKQ.ExpandAllGroups();
        }
        private void txtTuNgay_ChuaKQ_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
               
                 
            }
            catch
            {
                
            }
        }

        private void txtDenNgay_ChuaKQ_EditValueChanged(object sender, EventArgs e)
        {

        }

        #endregion

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            List<PSXN_KetQua> lstMauChoKQ = new List<PSXN_KetQua>();
            lstMauChoKQ = BioNet_Bus.GetDanhSachChoKetQuaXN(DateTime.Parse("2018-07-01"), DateTime.Parse("2018-08-01"), "all");
            foreach (var a in lstMauChoKQ)
            {
                PSCMGanViTriChung ph = BioNet_Bus.GetPhieuChuaCoKQ(a.MaPhieu);
                if (ph.MaPhieu == null)
                {
                    DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Mã phiếu không hợp lệ.");
                    warning.ShowDialog();
                }
                else
                {
                    GanVaoDS(ph);
                }
            }
        }

        private void btnViewGhiChu_Click(object sender, EventArgs e)
        {
            DiaglogFrm.FrmDanhMucGhiChuXN dmgc = new DiaglogFrm.FrmDanhMucGhiChuXN(emp);
            dmgc.Show();
        }

        private void btnHuyDanhSach_Click(object sender, EventArgs e)
        {
            try
            {
                if(vt.Count>0)
                {
                    if (MessageBox.Show("Bạn có chắn chắn hủy hết danh sách phiếu không?", "Cảnh báo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;
                    BioNet_Bus.DeleteDanhSachAllGanXNLuu(vt);
                }
                else
                {
                    DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Không có danh sách cần hủy.");
                    warning.ShowDialog();
                }              
            }
            catch
            {
            }
            LoadLstChuaKetQua();
            LoadDSDaLuu();
        }

        private void GCDanhSachGanViTri_Click(object sender, EventArgs e)
        {
        }

        private void GVChuaKQ_RowStyle(object sender, RowStyleEventArgs e)
        {

        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            var kq = lstMauChoKQ.Where(x=>x.isDaDuyet!=false).Take(110).ToList();
            foreach (var a in kq)
            {
                PSCMGanViTriChung ph = BioNet_Bus.GetPhieuChuaCoKQ(a.MaPhieu);
                if (ph.MaPhieu == null)
                {
                    DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Mã phiếu không hợp lệ.");
                    warning.ShowDialog();
                }
                else
                {
                    GanVaoDS(ph);
                }
            }
        }
    }
}