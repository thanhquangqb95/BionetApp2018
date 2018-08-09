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
using BioNetBLL;
using BioNetModel;
using BioNetModel.Data;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Spreadsheet;
using System.IO;
using System.Diagnostics;

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
        private List<ViTriXN> vtMayXN01 = new List<ViTriXN>();
        private List<ViTriXN> vtMayXN01_Tam = new List<ViTriXN>();
        private List<ViTriXN> vtMayXN02 = new List<ViTriXN>();
        public class xn
        {
            public string ViTri { get; set; }
        }
        private void FrmGanViTriMayXN_Load(object sender, EventArgs e)
        {
            this.vt.Clear();
            this.vtMayXN01.Clear();
            this.vtMayXN02.Clear();
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
        }

        private List<PSDanhMucGoiDichVuChung> lstgoiXN = new List<PSDanhMucGoiDichVuChung>();
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
            }
            catch { }
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
                long? STTDia = SttCt / MaxViTri + 1;
                var VitriGan = mapViTri.FirstOrDefault(x => x.IDMayXN == IDMayXN && x.STT == STTVTGan);
                if (VitriGan.isTest == true)
                {
                    result = true;
                    PSCMGanViTriChung test = new PSCMGanViTriChung();
                    test.MaXetNghiem = VitriGan.GiaTriTest;
                    test.STT_bang = vt.Count + 1;
                    test.MaGoiXN = "DVTest";
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
                                this.vtMayXN01.Add(vtxn);
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
                                this.vtMayXN02.Add(vtxn);
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
                    vt.Add(test);
                }
            }
            catch
            {
            }
            return result;
        }
        public List<ViTriXN> vtc = new List<ViTriXN>();
        private void LoadDanhSachGanVT()
        {

            this.LookupMayXN01.DataSource = null;
            this.LookupMayXN01.DataSource = this.vtMayXN01;
            this.LookupMayXN02.DataSource = null;
            this.LookupMayXN02.DataSource = this.vtMayXN02;
            this.GCDanhSachGanViTri.DataSource = null;
            this.GCDanhSachGanViTri.DataSource = vt;
        }

        private void GVDanhSachGanViTri_RowStyle(object sender, RowStyleEventArgs e)
        {
           
        }

        private void btnDuyetMaNgoai_Click(object sender, EventArgs e)
        {
            string MaXNNgoai = txtMaNgoai.Text.TrimEnd();
            
            string MaMayNgoai = cbbMayXN.EditValue.ToString();
            PSCMGanViTriChung ph = new PSCMGanViTriChung();
            ph.may = new List<PSDanhMucMayXN>();
           if(string.IsNullOrEmpty(MaMayNgoai))
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
        }

        private void LookupMayXN01_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void GVDanhSachGanViTri_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {

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
                            if (vtmoi.isTest)
                            {
                                rsmoi.MAYXN01.isTest = false;
                                vtmoi.isTest = false;
                                //vt.Remove(rsthay);
                                if (vtcu.STT == vtMayXN01.Count())
                                {
                                    vtMayXN01.Remove(vtcu);
                                }
                                foreach (var v in vt)
                                {
                                    if (v.STT_bang > gt)
                                    {
                                        v.STT_bang = v.STT_bang - 1;
                                    }
                                }
                                rsthay.MAYXN01.isTest = true;

                            }
                            rsthay.MAYXN01.STT = vtcu.STT;
                            rsthay.MAYXN01.STTDia = vtcu.STTDia;
                            rsthay.MAYXN01.STTVT = vtcu.STTVT;
                            rsthay.MAYXN01.ViTri = vtcu.ViTri;
                        }
                    }
                    if (columnHandle == this.col_MayXN02_ViTri.ColumnHandle)
                    {
                        var tenvitri = view.GetRowCellDisplayText(rowfocus, this.col_MayXN02_ViTri);
                        List<PSCMGanViTriChung> rss = vt.Where(p => p.MAYXN02 != null).ToList();
                        PSCMGanViTriChung rsmoi = rss.Where(p => p.MAYXN02.ViTri == tenvitri.ToString() && p.STT_bang == long.Parse(stt.ToString())).FirstOrDefault();
                        ViTriXN vtmoi = vtMayXN02.Where(p => p.ViTri == tenvitri.ToString()).FirstOrDefault();
                        var valueOld = view.ActiveEditor.OldEditValue;
                        ViTriXN vtcu = vtMayXN02.Where(p => p.ViTri == valueOld.ToString()).FirstOrDefault();
                        PSCMGanViTriChung rsthay = rss.Where(p => p.MAYXN02.ViTri == tenvitri.ToString() && p.MAYXN02.STT == long.Parse(vtmoi.STT.ToString())).FirstOrDefault();
                        if (vtcu != null && rsmoi != null && rsthay != null)
                        {
                            long? gt = rsthay.STT_bang;
                            long? SttCtMax = vtMayXN02.Count();
                            rsmoi.MAYXN02.STT = vtmoi.STT;
                            rsmoi.MAYXN02.STTDia = vtmoi.STTDia;
                            rsmoi.MAYXN02.STTVT = vtmoi.STTVT;
                            rsmoi.MAYXN02.ViTri = vtmoi.ViTri;
                            if (vtmoi.isTest)
                            {
                                rsmoi.MAYXN02.isTest = false;
                                vtmoi.isTest = false;
                                //vt.Remove(rsthay);
                                if (vtcu.STT == vtMayXN02.Count())
                                {
                                    vtMayXN02.Remove(vtcu);
                                }
                                foreach (var v in vt)
                                {
                                    if (v.STT_bang > gt)
                                    {
                                        v.STT_bang = v.STT_bang - 1;
                                    }
                                }
                                rsthay.MAYXN02.isTest = true;
                            }
                                rsthay.MAYXN02.STT = vtcu.STT;
                                rsthay.MAYXN02.STTDia = vtcu.STTDia;
                                rsthay.MAYXN02.STTVT = vtcu.STTVT;
                                rsthay.MAYXN02.ViTri = vtcu.ViTri;
                        }
                    }
                }
                LoadDanhSachGanVT();
            }
            catch
            {

            }

        }
        private void GanVaoDS(PSCMGanViTriChung ph)
        {
            PSCMGanViTriChung kq=new PSCMGanViTriChung();
            kq=null;
            if (vt.Count() > 0)
            {
                kq = vt.FirstOrDefault(x => x.MaXetNghiem.Equals(ph.MaXetNghiem));

            }
            if (kq == null)
                {
                    if (ph.may.Count() > 0)
                    {
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
                                long? STTDia = SttCt / MaxViTri + 1;
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
                                                this.vtMayXN01.Add(vtxn);
                                            }
                                            ph.MAYXN01 = new ViTriXN();
                                            ph.MAYXN01.STTVT = vtxn.STTVT;
                                            ph.MAYXN01.STTDia = vtxn.STTDia;
                                            ph.MAYXN01.ViTri = vtxn.ViTri;
                                            ph.MAYXN01.isTest = vtxn.isTest;
                                            ph.MAYXN01.STT = vtxn.STT;
                                            break;
                                        }
                                    case "MAYXN02":
                                        {
                                            if (VitriGan.isTest != true)
                                            {
                                                this.vtMayXN02.Add(vtxn);
                                            }
                                            ph.MAYXN02 = new ViTriXN();
                                            ph.MAYXN02.STTVT = vtxn.STTVT;
                                            ph.MAYXN02.STTDia = vtxn.STTDia;
                                            ph.MAYXN02.ViTri = vtxn.ViTri;
                                            ph.MAYXN02.isTest = vtxn.isTest;
                                            ph.MAYXN02.STT = vtxn.STT;
                                            break;
                                        }
                                    default:
                                        break;
                                }

                            } while (GanViTriTest(ctm.IDMayXN));
                        }
                    }
                    ph.STT_bang = vt.Count + 1;
                    vt.Add(ph);
                    LoadDanhSachGanVT();
                
            }
            else
            {
                DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Mã xét nghiệm đã tồn tại trong danh sách.");
                warning.ShowDialog();
            }
           
           
        }
        private void btnXuatFile_Click(object sender, EventArgs e)
        {
            List<PsRptDanhSachDaCapMaXetNghiem> data = new List<PsRptDanhSachDaCapMaXetNghiem>();
            List<pro_ReportGanViTriMayXNResult> dataGanVT = new List<pro_ReportGanViTriMayXNResult>();
            try
            {
                string link = Application.StartupPath + @"\DSSoDoXetNghiem\";
                    Workbook workbook = new DevExpress.Spreadsheet.Workbook();
                    Reports.RepostsCapMaXetNghiep.rptReportGanViTriMAYXN01 rp1 = new Reports.RepostsCapMaXetNghiep.rptReportGanViTriMAYXN01();
                    rp1.DataSource = vt.Where(y => y.MAYXN01 != null).OrderBy(x => x.MAYXN01.STT);
                    rp1.PaperName = "May3Benh";
                    rp1.Parameters["TenNV"].Value = emp.EmployeeName;
                    rp1.Parameters["NgayTaoDS"].Value = DateTime.Now.ToString();
                    rp1.ExportToXlsx("May3Benh" + ".xlsx", new DevExpress.XtraPrinting.XlsxExportOptions() { SheetName = "May3Benh" });
                    Workbook workbook2 = new DevExpress.Spreadsheet.Workbook();
                    using (FileStream stream = new FileStream("May3Benh" + ".xlsx", FileMode.Open))
                    {
                        workbook2.LoadDocument(stream, DocumentFormat.Xlsx);
                    }
                    workbook.Worksheets.Insert(0, "May3Benh");
                    workbook.Worksheets[0].CopyFrom(workbook2.Worksheets[0]);
                    File.Delete("May3Benh" + ".xlsx");

                Reports.RepostsCapMaXetNghiep.rptReportGanViTriMAYXN02 rp2 = new Reports.RepostsCapMaXetNghiep.rptReportGanViTriMAYXN02();
                rp2.DataSource = vt.Where(y => y.MAYXN02 != null).OrderBy(x => x.MAYXN02.STT);
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

                string l = link + "SodoXetNghiemNgay" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + ".xlsx";
                workbook.SaveDocument(l);
                Process.Start(l);
            }
            catch
            {
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            List<PSXN_KetQua> lstMauChoKQ = new List<PSXN_KetQua>();
            lstMauChoKQ = BioNet_Bus.GetDanhSachChoKetQuaXN(DateTime.Parse("2018-07-01"), DateTime.Parse("2018-08-01"), "all");
            foreach(var a in lstMauChoKQ)
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

        private void GCDanhSachGanViTri_Click(object sender, EventArgs e)
        {

        }

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
                                    e.Appearance.BackColor = Color.White;
                                    break;
                                }
                        }
                    }

                    try {
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
                                        e.Appearance.BackColor = Color.White;
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
    }
}