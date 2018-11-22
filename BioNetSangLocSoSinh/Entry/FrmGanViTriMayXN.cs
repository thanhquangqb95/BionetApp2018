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
using DevExpress.XtraRichEdit.Commands;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmGanViTriMayXN : DevExpress.XtraEditors.XtraForm
    {
        public FrmGanViTriMayXN(PsEmployeeLogin EMP,string MayDL)
        {
            emp = EMP;
            MayDucLo = MayDL;
            InitializeComponent();
        }
        public static string MayDucLo = string.Empty;
        public static string IDLanDucLo = string.Empty;
        public static PsEmployeeLogin emp = new PsEmployeeLogin();
        private List<PSCMGanViTriChung> vt = new List<PSCMGanViTriChung>();
        private List<PSDanhMucMayXN> dsmay = new List<PSDanhMucMayXN>();
        private List<PSMapsViTriMayXN> mapViTri = new List<PSMapsViTriMayXN>();
        private static List<ViTriXN> vtMayXN001 = new List<ViTriXN>();
        private static List<ViTriXN> vtMayXN002 = new List<ViTriXN>();
        private List<PSGanViTriXNKQ> lstMauChoKQ = new List<PSGanViTriXNKQ>();
        private List<PSDanhMucDonViCoSo> lstDonVi = new List<PSDanhMucDonViCoSo>();
        private List<PSDanhMucChiCuc> lstChiCuc = new List<PSDanhMucChiCuc>();
        private List<PSDanhMucDonViCoSo> lstDonViResponsitory = new List<PSDanhMucDonViCoSo>();
        private List<PSDanhMucGoiDichVuChung> lstgoiXN = new List<PSDanhMucGoiDichVuChung>();

        private void FrmGanViTriMayXN_Load(object sender, EventArgs e)
        {

            dsmay = BioNet_Bus.GetDSMayXNTheoMayDucLo(MayDucLo);
            LoadGoiDichVuXetNGhiem();
            LoadMay();
            mapViTri = new List<PSMapsViTriMayXN>();
            if (dsmay.Count > 0)
            {
                foreach (var may in dsmay)
                {
                    mapViTri.AddRange(BioNet_Bus.GetDSMapViTriMayXN(may.IDMayXN));
                    List<PSMapsViTriMayXN> a = BioNet_Bus.GetDSMapViTriMayXN(may.IDMayXN);
                }
            }
            cbbMayXN.EditValue = "MAYXN00";
            cbbGanVT.EditValue = 0;
            this.LoadDSDaLuu();
            this.LOadDanhMuc();
            this.LoadLstChuaKetQua();
            this.GetIDLanGanXN();
            this.txtTuNgay_ChuaKQ.EditValue = DateTime.Now;
            this.txtDenNgay_ChuaKQ.EditValue = DateTime.Now;
            this.GVChuaKQ.ClearColumnsFilter();
            this.GVChuaKQ.ExpandAllGroups();
        }

        #region Load Danh mục
        private void GetIDLanGanXN()
        {
            IDLanDucLo = BioNet_Bus.GetMaRowIDLanGanXN(MayDucLo);
        }
        private void LoadMay()
        {
            if (dsmay != null)
            {
                foreach (var may in dsmay)
                {
                    GridBand band = new GridBand();
                    band.Name = may.IDMayXN;
                    band.Caption = may.TenMayXN;
                    band.RowCount = 1;
                    band.Visible = true;
                    gridBandViTri.Children.Add(band);

                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col4 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                    col4.Name = "col_"+may.IDMayXN+"_STT";
                    col4.FieldName = may.IDMayXN +".STT";
                    col4.Caption = "STT Máy";
                    col4.OptionsColumn.AllowEdit = false;
                    col4.Visible = true;
                    col4.Width = 20;
                   
                    band.Columns.Add(col4);

                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                    col3.Name = "col_" + may.IDMayXN + "_STTDia";
                    col3.FieldName = may.IDMayXN + ".STTDia";
                    col3.Caption = "Đĩa";
                    col3.OptionsColumn.AllowEdit = false;
                    col3.Visible = true;
                    col3.Width = 20;
                    band.Columns.Add(col3);

                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                    col2.Name = "col_" + may.IDMayXN + "_ViTri";
                    col2.FieldName = may.IDMayXN + ".ViTri";
                    col2.Caption = "Vị Trí";
                    col2.Visible = true;
                    col2.Width = 20;
                    col2.OptionsColumn.AllowEdit = true;

                    List<ViTriXN> vtMay = new List<ViTriXN>();
                    switch (may.IDMayXN)
                        {
                            case "MAYXN001":
                                {
                                LookUpEditMAYXN001.NullText = "";
                                col2.ColumnEdit = LookUpEditMAYXN001;
                                break;
                                }
                            case "MAYXN002":
                                {

                                LookUpEditMAYXN002.NullText = "";
                                col2.ColumnEdit = LookUpEditMAYXN002;
                                break;
                                }
                        }
                    band.Columns.Add(col2);
                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                    col1.Name = "col_" + may.IDMayXN + "_GhiChu";
                    col1.FieldName = may.IDMayXN + ".GhiChu";
                    col1.Caption = "Ghi chú";
                    col1.OptionsColumn.AllowEdit = true;
                    col1.Visible = true;
                    col1.Width = 40;
                    band.Columns.Add(col1);
                }
            }
        }

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
            vtMayXN001.Clear();
            vtMayXN002.Clear();
            vt = BioNet_Bus.GetDanhSachGanXNLuu(MayDucLo);
            foreach (var may in dsmay)
            {
                switch (may.IDMayXN)
                {
                    case "MAYXN001":
                        {
                            var m1 = vt.Where(x => x.MAYXN001 != null).Select(x => x.MAYXN001).ToList();
                            foreach (var m in m1)
                            {
                                ViTriXN v = new ViTriXN();
                                v.STT = m.STT;
                                v.STTDia = m.STTDia;
                                v.isTest = m.isTest;
                                v.STTVT = m.STTVT;
                                v.ViTri = m.ViTri;
                                vtMayXN001.Add(v);
                            }
                            LookUpEditMAYXN001.DataSource = null;
                            LookUpEditMAYXN001.DataSource = vtMayXN001;
                            break;
                        }
                    case "MAYXN002":
                        {
                            var m1 = vt.Where(x => x.MAYXN002 != null).Select(x => x.MAYXN002).ToList();
                            foreach (var m in m1)
                            {
                                ViTriXN v = new ViTriXN();
                                v.STT = m.STT;
                                v.STTDia = m.STTDia;
                                v.isTest = m.isTest;
                                v.STTVT = m.STTVT;
                                v.ViTri = m.ViTri;
                                vtMayXN002.Add(v);
                            }
                            LookUpEditMAYXN002.DataSource = null;
                            LookUpEditMAYXN002.DataSource = vtMayXN002;
                            break;
                        }
                }
            }
            this.LoadDanhSachGanVT();
        }
        private void LoadDanhSachGanVT()
        {
            
            this.GCDanhSachGanViTri.DataSource = null;
            this.GCDanhSachGanViTri.DataSource = vt;
            this.GVDanhSachGanViTri.OptionsSelection.MultiSelect = false;
            this.GVDanhSachGanViTri.ClearSelection();
            this.GVDanhSachGanViTri.FocusedRowHandle = vt.Count();
            this.GVDanhSachGanViTri.SelectRow(vt.Count());
            this.GVDanhSachGanViTri.MoveLastVisible();
            this.txtDem.Text = vt.Where(x => x.MaGoiXN.Equals("DVTest") != true).ToList().Count().ToString();
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
                                        case "MAYXN001":
                                            {
                                                ph.MAYXN001 = vtNull.MAYXN001;
                                                break;
                                            }
                                        case "MAYXN002":
                                            {
                                                ph.MAYXN002 = vtNull.MAYXN002;
                                                break;
                                            }
                                    }
                                }
                                BioNet_Bus.SuaDanhSachGanXNLuuNew(ph, MayDucLo, IDLanDucLo, emp.EmployeeCode);
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
                            string MaLanGan = BioNet_Bus.GetMaRowIDLanGanXN(MayDucLo);
                            ph.IDLanGanXN = MaLanGan;
                            foreach (var ctm in ph.may)
                            {
                                do
                                {
                                    long? SttCt = 1;
                                    switch (ctm.IDMayXN)
                                    {
                                        case "MAYXN001":
                                            {
                                                SttCt = vtMayXN001.Count() + 1;
                                                break;
                                            }
                                        case "MAYXN002":
                                            {
                                                SttCt = vtMayXN002.Count() + 1;
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
                                        case "MAYXN001":
                                            {

                                                if (VitriGan.isTest != true)
                                                {
                                                    //vtMayXN01.Add(vtxn);
                                                    ph.MAYXN001 = new ViTriXN();
                                                    ph.MAYXN001.STTVT = vtxn.STTVT;
                                                    ph.MAYXN001.STTDia = vtxn.STTDia;
                                                    ph.MAYXN001.ViTri = vtxn.ViTri;
                                                    ph.MAYXN001.isTest = vtxn.isTest;
                                                    ph.MAYXN001.STT = vtxn.STT;
                                                }
                                                break;
                                            }
                                        case "MAYXN002":
                                            {
                                                if (VitriGan.isTest != true)
                                                {
                                                    //vtMayXN02.Add(vtxn);
                                                    ph.MAYXN002 = new ViTriXN();
                                                    ph.MAYXN002.STTVT = vtxn.STTVT;
                                                    ph.MAYXN002.STTDia = vtxn.STTDia;
                                                    ph.MAYXN002.ViTri = vtxn.ViTri;
                                                    ph.MAYXN002.isTest = vtxn.isTest;
                                                    ph.MAYXN002.STT = vtxn.STT;
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
                    //LoadLstChuaKetQua();
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
        private void GanVaoDSNew(PSCMGanViTriChung ph)
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
                        
                            BioNet_Bus.SuaDanhSachGanXNLuuNew(ph, MayDucLo, IDLanDucLo, emp.EmployeeCode);
                    }
                    LoadLstChuaKetQua();
                    //LoadDSDaLuu();
                    SplashScreenManager.CloseForm();
                }
                else
                {

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
                    case "MAYXN001":
                        {
                            SttCt = vtMayXN001.Count() + 1;
                            break;
                        }
                    case "MAYXN002":
                        {
                            SttCt = vtMayXN002.Count() + 1;
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
                var VitriGan = mapViTri.FirstOrDefault(x => x.IDMayXN == IDMayXN && x.STT == STTVTGan);
                if (VitriGan.isTest == true)
                {
                    result = true;
                    PSCMGanViTriChung test = new PSCMGanViTriChung();
                    test.MaXetNghiem = VitriGan.GiaTriTest;
                    test.STT_bang = vt.Count + 1;
                    test.MaGoiXN = "DVTest";
                    string MaLanGan = BioNet_Bus.GetMaRowIDLanGanXN(MayDucLo);
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
                        case "MAYXN001":
                            {
                                test.MAYXN001 = new ViTriXN();
                                vtMayXN001.Add(vtxn);
                                test.MAYXN001.STTVT = vtxn.STTVT;
                                test.MAYXN001.STTDia = vtxn.STTDia;
                                test.MAYXN001.ViTri = vtxn.ViTri;
                                test.MAYXN001.isTest = vtxn.isTest;
                                test.MAYXN001.STT = vtxn.STT;
                                break;
                            }
                        case "MAYXN002":
                            {
                                test.MAYXN002 = new ViTriXN();
                                vtMayXN002.Add(vtxn);
                                test.MAYXN002.STTVT = vtxn.STTVT;
                                test.MAYXN002.STTDia = vtxn.STTDia;
                                test.MAYXN002.ViTri = vtxn.ViTri;
                                test.MAYXN002.isTest = vtxn.isTest;
                                test.MAYXN002.STT = vtxn.STT;
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
                if (MessageBox.Show("Bạn có chắn chắn duyệt hết danh sách phiếu không?", "Cảnh báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                   PsReponse rep= BioNet_Bus.DuyetCapMaGanMayXN(IDLanDucLo,emp.EmployeeCode);
                   this.XuatFile(true); 
                   this.LoadLstChuaKetQua();
                   this.LoadDSDaLuu();
                   this.GetIDLanGanXN();
                }
            }
         catch
            {
                DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Duyệt file bị lỗi.");
                 warning.ShowDialog();
            }
}
        private void btnExportReview_Click(object sender, EventArgs e)
        {
                this.XuatFile(false);
        }
        #endregion
        #region Xuat File
        private BindingList<PSCMGanViTriChungReport> XuatCMGanViTriChungReport(int May)
        {
            BindingList<PSCMGanViTriChungReport> re = new BindingList<PSCMGanViTriChungReport>();
            List<PSDanhMucGoiDichVuChung> goi = BioNet_Bus.GetDanhsachGoiDichVuChung();
           switch(May)
            {
                case 1:
                    {
                        var vtmay1 = vt.Where(y => y.MAYXN001 != null).OrderBy(x => x.MAYXN001.STT);
                        foreach (var vtm in vtmay1)
                        {
                            PSCMGanViTriChungReport rep = new PSCMGanViTriChungReport();
                            rep.isTest = vtm.MAYXN001.isTest;
                            rep.MaPhieu = vtm.MaPhieu == null ? string.Empty : vtm.MaPhieu;
                            rep.MaXetNghiem = vtm.MaXetNghiem.Trim().ToUpper();
                            rep.MaGoiXN = vtm.MaGoiXN;
                            rep.TenGoiXN = lstgoiXN.FirstOrDefault(x => x.IDGoiDichVuChung.Equals(vtm.MaGoiXN)).TenGoiDichVuChung;
                            rep.STT = vtm.MAYXN001.STT;
                            rep.STTDia = vtm.MAYXN001.STTDia;
                            rep.STTVT = vtm.MAYXN001.STTVT;
                            rep.GhiChuCT = vtm.MAYXN001.GhiChuCT == null ? string.Empty : vtm.MAYXN001.GhiChuCT;
                            rep.ViTri = vtm.MAYXN001.ViTri;
                            rep.GhiChuChung = vtm.GhiChuChung == null ? string.Empty : vtm.GhiChuChung;
                            rep.isMoi = goi.FirstOrDefault(x => x.IDGoiDichVuChung.Equals(rep.MaGoiXN)) != null?true:false;
                            re.Add(rep);
                        }
                        break;
                    }
                case 2:
                    {
                        var vtmay1 = vt.Where(y => y.MAYXN002 != null).OrderBy(x => x.MAYXN002.STT);
                        foreach (var vtm in vtmay1)
                        {
                            PSCMGanViTriChungReport rep = new PSCMGanViTriChungReport();
                            rep.isTest = vtm.MAYXN002.isTest;
                            rep.MaPhieu = vtm.MaPhieu == null ? string.Empty : vtm.MaPhieu;
                            rep.MaXetNghiem = vtm.MaXetNghiem.Trim().ToUpper();
                            rep.MaGoiXN = vtm.MaGoiXN;
                            rep.TenGoiXN = lstgoiXN.FirstOrDefault(x => x.IDGoiDichVuChung.Equals(vtm.MaGoiXN)).TenGoiDichVuChung;
                            rep.STT = vtm.MAYXN002.STT;
                            rep.STTDia = vtm.MAYXN002.STTDia;
                            rep.STTVT = vtm.MAYXN002.STTVT;
                            rep.GhiChuCT = vtm.MAYXN002.GhiChuCT == null ? string.Empty : vtm.MAYXN002.GhiChuCT;
                            rep.ViTri = vtm.MAYXN002.ViTri;
                            rep.GhiChuChung = vtm.GhiChuChung == null ? string.Empty : vtm.GhiChuChung;
                            rep.isMoi = goi.FirstOrDefault(x => x.IDGoiDichVuChung.Equals(rep.MaGoiXN)) != null ? true : false;
                            re.Add(rep);
                        }
                        break;
                    }
            }
          
            return re;
        }
        private void XuatFile(bool SaveFile)
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
                string LinkDate=link+DateTime.Now.Year.ToString()+"."+DateTime.Now.Month.ToString()+"."+DateTime.Now.Date.Day.ToString()+"\\";
                 if (!Directory.Exists(LinkDate))
                {
                    Directory.CreateDirectory(LinkDate);
                }
                Workbook workbook = new DevExpress.Spreadsheet.Workbook();
                string Idlangan = vt.Distinct().Select(x => x.IDLanGanXN).FirstOrDefault().ToString();
                #region Máy 1
                Reports.RepostsCapMaXetNghiep.rptReportGanViTriMAYXN01 rp1 = new Reports.RepostsCapMaXetNghiep.rptReportGanViTriMAYXN01();
                BindingList<PSCMGanViTriChungReport> vtmay1 = XuatCMGanViTriChungReport(1);
                rp1.DataSource = vtmay1;
                rp1.PaperName = "May2Benh";
                rp1.Parameters["TenNV"].Value = emp.EmployeeName;
                rp1.Parameters["NgayTaoDS"].Value = DateTime.Now.ToString();
                rp1.Parameters["SLGoi5benh"].Value = vtmay1.Where(y => y.MaGoiXN == "DVGXN0004").Count();
                rp1.Parameters["SLGoi3benh"].Value = vtmay1.Where(y => y.MaGoiXN == "DVGXN0003").Count();
                rp1.Parameters["SLGoi2benh"].Value = vtmay1.Where(y => y.MaGoiXN == "DVGXN0002").Count();
                rp1.Parameters["SLGoiHemo"].Value = vtmay1.Where(y => y.MaGoiXN == "DVGXN0006" || y.MaGoiXN.Equals("DVGXN0007")).Count();
                rp1.Parameters["SLXNL"].Value = vtmay1.Where(y => y.MaGoiXN == "DVGXN0001").Count();
                rp1.Parameters["SLXNNgoai"].Value = vtmay1.Where(x => x.MaGoiXN == "DVKhac").Count();
                rp1.Parameters["SLXNL2benh"].Value = vtmay1.Where(y => y.MaGoiXN == "DVGXNL2").Count();
                rp1.Parameters["TongMauMoi"].Value = vtmay1.Where(y => y.isMoi == true).Count();
                rp1.Parameters["TongMauDuc"].Value = vtmay1.Count() - vtmay1.Where(x => x.isTest == true).Count();
                rp1.Parameters["SLXNTest"].Value = vtmay1.Where(x => x.isTest == true).Count();
                rp1.Parameters["SLXNConLai"].Value = int.Parse(rp1.Parameters["TongMauMoi"].Value.ToString()) - int.Parse(rp1.Parameters["SLGoi5benh"].Value.ToString())
                - int.Parse(rp1.Parameters["SLGoi3benh"].Value.ToString()) - int.Parse(rp1.Parameters["SLGoi2benh"].Value.ToString()) - int.Parse(rp1.Parameters["SLGoiHemo"].Value.ToString());
                rp1.ExportToXlsx("May3Benh" + ".xlsx", new DevExpress.XtraPrinting.XlsxExportOptions() { SheetName = "May3Benh" });

                Workbook workbook2 = new DevExpress.Spreadsheet.Workbook();
                using (FileStream stream = new FileStream("May3Benh" + ".xlsx", FileMode.Open))
                {
                    workbook2.LoadDocument(stream, DocumentFormat.Xlsx);
                }
                workbook.Worksheets.Insert(0, "May3Benh");
                workbook.Worksheets[0].CopyFrom(workbook2.Worksheets[0]);
                File.Delete("May3Benh" + ".xlsx");
                #endregion
                #region Máy 2
                Reports.RepostsCapMaXetNghiep.rptReportGanViTriMAYXN02 rp2 = new Reports.RepostsCapMaXetNghiep.rptReportGanViTriMAYXN02();
                BindingList<PSCMGanViTriChungReport> vtmay2 = XuatCMGanViTriChungReport(2);
                rp2.DataSource = vtmay2;
                rp2.PaperName = "May3Benh";
                rp2.Parameters["TenNV"].Value = emp.EmployeeName;
                rp2.Parameters["NgayTaoDS"].Value = DateTime.Now.ToString();
                rp2.Parameters["SLGoi5benh"].Value = vtmay2.Where(x => x.MaGoiXN == "DVGXN0004").Count();
                rp2.Parameters["SLGoi3benh"].Value = vtmay2.Where(x => x.MaGoiXN == "DVGXN0003").Count();
                rp2.Parameters["SLGoi2benh"].Value = vtmay2.Where(y => y.MaGoiXN == "DVGXN0002").Count();
                rp2.Parameters["SLGoiHemo"].Value = vtmay2.Where(x => x.MaGoiXN == "DVGXN0006" || x.MaGoiXN.Equals("DVGXN0007")).Count();
                rp2.Parameters["SLXNNgoai"].Value = vtmay2.Where(x => x.MaGoiXN == "DVKhac").Count();
                rp2.Parameters["SLXNL"].Value = vtmay2.Where(y => y.MaGoiXN == "DVGXN0001").Count();
                rp2.Parameters["SLXNL2benh"].Value = vtmay2.Where(y => y.MaGoiXN == "DVGXNL2").Count();
                rp2.Parameters["TongMauMoi"].Value = vtmay2.Where(y => y.isMoi == true).Count();
                rp2.Parameters["TongMauDuc"].Value = vtmay2.Count() - vtmay2.Where(x => x.isTest == true).Count();
                rp2.Parameters["SLXNTest"].Value = vtmay2.Where(x => x.isTest == true).Count();
                rp2.Parameters["SLXNConLai"].Value = int.Parse(rp2.Parameters["TongMauMoi"].Value.ToString()) - int.Parse(rp2.Parameters["SLGoi5benh"].Value.ToString())
                - int.Parse(rp2.Parameters["SLGoi3benh"].Value.ToString()) - int.Parse(rp2.Parameters["SLGoi2benh"].Value.ToString()) - int.Parse(rp2.Parameters["SLGoiHemo"].Value.ToString());
                rp2.ExportToXlsx("May2Benh" + ".xlsx", new DevExpress.XtraPrinting.XlsxExportOptions() { SheetName = "May2Benh" });
                Workbook workbook3 = new DevExpress.Spreadsheet.Workbook();
                using (FileStream stream = new FileStream("May2Benh" + ".xlsx", FileMode.Open))
                {
                    workbook3.LoadDocument(stream, DocumentFormat.Xlsx);
                }
                workbook.Worksheets.Insert(1, "May2Benh");
                workbook.Worksheets[1].CopyFrom(workbook3.Worksheets[0]);
                File.Delete("May2Benh" + ".xlsx");
                #endregion
                string linkfile;
                List<string> listmay1 = vtmay1.Select(x => x.MaXetNghiem).ToList();
                if (SaveFile)
                {
                    linkfile = LinkDate + "SodoXetNghiem" + DateTime.Now.Day+"." + DateTime.Now.Month+"." + DateTime.Now.Year+ ".xlsx";
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(LinkDate + "SoDoMay3Benh" + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + ".txt") )
                    {
                        foreach (string line in listmay1)
                        {
                                file.WriteLine(line);
                        }
                    }
                }
                else
                {
                    linkfile = LinkDate + "SodoXetNghiemReviewNgay" + DateTime.Now.Day +"." + DateTime.Now.Month + "." + DateTime.Now.Year+"."+ DateTime.Now.Hour+"." + DateTime.Now.Minute + ".xlsx";
                    
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(LinkDate + "SoDoReviewMay3Benh" + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + "." + DateTime.Now.Hour + "." + DateTime.Now.Minute + ".txt"))
                    {
                        foreach (string line in listmay1)
                        {
                            file.WriteLine(line);
                        }
                    }
                }
               
                workbook.SaveDocument(linkfile);
                System.Diagnostics.Process.Start(linkfile);

            }
            catch (Exception ex)
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
                    int max = vt.Count();
                    if(e.RowHandle==max-1)
                    {
                        e.Appearance.Font=  new Font(e.Appearance.Font, FontStyle.Bold);
                    }
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
                                    e.Appearance.BackColor = Color.Chocolate;
                                    break;
                                }
                            case "DVGXN0002":
                                {
                                    e.Appearance.BackColor = Color.PaleGreen;
                                    break;
                                }
                            case "DVGXN0003":
                                {
                                    e.Appearance.BackColor = Color.PaleGreen;
                                    break;
                                }
                            case "DVGXN0004":
                                {
                                    e.Appearance.BackColor = Color.Bisque;
                                    break;
                                }
                            case "DVGXN0006":
                                {
                                    e.Appearance.BackColor = Color.Firebrick;
                                    break;
                                }
                            case "DVGXNL2":
                                {
                                    e.Appearance.BackColor = Color.Salmon;
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
                        if (e.Column.Name.Equals("col_MAYXN001_STTDia"))
                        {

                            int sttvt = int.Parse(View.GetRowCellDisplayText(e.RowHandle, "MAYXN001.STTDia")) % 2;
                            switch (sttvt)
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
                        if (e.Column.Name.Equals("col_MAYXN002_STTDia"))
                        {

                            int STTVT = int.Parse(View.GetRowCellDisplayText(e.RowHandle, "MAYXN002.STTDia")) % 2;
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
                        // GanVaoDS(ph);

                        GanVaoDSNew(ph);
                        var kq = lstMauChoKQ.FirstOrDefault(x => x.MaPhieu == ph.MaPhieu && x.MaXetNghiem == ph.MaXetNghiem);
                        if(kq!=null)
                        {
                            kq.isDaDuyet = false;
                        }
                        else
                        {
                            LoadLstChuaKetQua();
                        }
                        this.LoadDSDaLuu();
                        this.GVChuaKQ.OptionsSelection.MultiSelect = false;
                        this.GVChuaKQ.ClearSelection();
                        this.GVChuaKQ.FocusedRowHandle =2;
                        this.GVChuaKQ.SelectRow(2);
                        this.GVChuaKQ.MoveLastVisible();
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
                    GanVaoDSNew(ph);
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
                    if (e.Column.Name.Equals("col_MAYXN001_ViTri"))
                    {
                        var tenvitri = view.GetRowCellDisplayText(rowfocus, "MAYXN001.ViTri");
                        List<PSCMGanViTriChung> rss = vt.Where(p => p.MAYXN001 != null).ToList();
                        PSCMGanViTriChung rsmoi = rss.Where(p => p.MAYXN001.ViTri == tenvitri.ToString() && p.STT_bang == long.Parse(stt.ToString())).FirstOrDefault();
                        ViTriXN vtmoi = vtMayXN001.Where(p => p.ViTri == tenvitri.ToString()).FirstOrDefault();
                        var valueOld = view.ActiveEditor.OldEditValue;
                        ViTriXN vtcu = vtMayXN001.Where(p => p.ViTri == valueOld.ToString()).FirstOrDefault();
                        PSCMGanViTriChung rsthay = rss.Where(p => p.MAYXN001.ViTri == tenvitri.ToString() && p.MAYXN001.STT == long.Parse(vtmoi.STT.ToString())).FirstOrDefault();
                        if (vtcu != null && rsmoi != null && rsthay != null)
                        {
                            BioNet_Bus.DoiViTriGanXN(rsthay,rsmoi,"MAYXN001");
                            this.LoadDSDaLuu();
                        }
                    }
                    if( e.Column.Name.Equals("col_MAYXN002_ViTri"))
                    {
                        var tenvitri = view.GetRowCellDisplayText(rowfocus, "MAYXN002.ViTri");
                        List<PSCMGanViTriChung> rss = vt.Where(p => p.MAYXN002 != null).ToList();
                        PSCMGanViTriChung rsmoi = rss.Where(p => p.MAYXN002.ViTri == tenvitri.ToString() && p.STT_bang == long.Parse(stt.ToString())).FirstOrDefault();
                        ViTriXN vtmoi = vtMayXN002.Where(p => p.ViTri == tenvitri.ToString()).FirstOrDefault();
                        var valueOld = view.ActiveEditor.OldEditValue;
                        ViTriXN vtcu = vtMayXN002.Where(p => p.ViTri == valueOld.ToString()).FirstOrDefault();
                        PSCMGanViTriChung rsthay = rss.Where(p => p.MAYXN002.ViTri == tenvitri.ToString() && p.MAYXN002.STT == long.Parse(vtmoi.STT.ToString())).FirstOrDefault();
                        if (vtcu != null && rsmoi != null && rsthay != null)
                        {
                            BioNet_Bus.DoiViTriGanXN(rsthay, rsmoi, "MAYXN002");
                            this.LoadDSDaLuu();
                        }
                    }
                    //if (columnHandle == this.col_IDPhieu.ColumnHandle)
                    //{
                    //    PSCMGanViTriChung ph = BioNet_Bus.GetPhieuChuaCoKQ(idPhieu.ToString());
                    //    if (ph != null)
                    //    {
                    //        if (BioNet_Bus.GetMaPhieuDaCoDSCapVT(idPhieu.ToString()))
                    //        {
                    //            PSCMGanViTriChung rss = vt.FirstOrDefault(p => p.STT_bang == long.Parse(stt.ToString()) && p.IDRowGanXN == long.Parse(idrow.ToString()));
                    //            int countMay = (from pscm in rss.may
                    //                            join phcm in ph.may on pscm.IDMayXN equals phcm.IDMayXN
                    //                            where pscm.IDMayXN == phcm.IDMayXN
                    //                            select new { phcm }).Count();
                    //            if (countMay == rss.may.Count())
                    //            {
                    //                ph.STT_bang = rss.STT_bang;
                    //                ph.IDRowGanXN = rss.IDRowGanXN;
                    //                ph.IDLanGanXN = rss.IDLanGanXN;
                    //                foreach (var pm in ph.may)
                    //                {
                    //                    switch (pm.IDMayXN)
                    //                    {
                    //                        case "MAYXN01":
                    //                            {
                    //                                ph.MAYXN01 = rss.MAYXN01;
                    //                                break;
                    //                            }
                    //                        case "MAYXN02":
                    //                            {
                    //                                ph.MAYXN02 = rss.MAYXN02;
                    //                                break;
                    //                            }
                    //                    }
                    //                }
                    //                BioNet_Bus.SuaDanhSachGanXNLuu(ph);
                    //            }
                    //            else
                    //            {
                    //                DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Thay đổi mã phiếu không hợp lệ vui lòng kiểm tra lại.");
                    //                warning.ShowDialog();
                    //            }
                    //        }
                    //        else
                    //        {
                    //            DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Mã phiếu đã có trong danh sách.");
                    //            warning.ShowDialog();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        DiaglogFrm.FrmWarning warning = new DiaglogFrm.FrmWarning("Mã phiếu không tồn tại.");
                    //        warning.ShowDialog();
                    //    }
                    //    LoadDSDaLuu();
                    //}
                    //if (columnHandle == this.col_MayXN02_GhiChu.ColumnHandle)
                    //{
                    //    var tenvitri = view.GetRowCellDisplayText(rowfocus, this.col_MayXN02_ViTri);
                    //    var ghichu = view.GetRowCellDisplayText(rowfocus, this.col_MayXN02_GhiChu);
                    //    List<PSCMGanViTriChung> rss = vt.Where(p => p.MAYXN02 != null).ToList();
                    //    PSCMGanViTriChung rsmoi = rss.Where(p => p.MAYXN02.ViTri == tenvitri.ToString() && p.STT_bang == long.Parse(stt.ToString())).FirstOrDefault();
                    //    rsmoi.MAYXN02.GhiChuCT = ghichu.ToString();
                    //    BioNet_Bus.SuaDanhSachGanXNLuu(rsmoi);
                    //}
                    //if (columnHandle == this.col_GhiNhoChung.ColumnHandle)
                    //{
                    //    var ghichu = view.GetRowCellDisplayText(rowfocus, this.col_GhiNhoChung);
                    //    PSCMGanViTriChung rsmoi = vt.Where(p => p.STT_bang == long.Parse(stt.ToString())).FirstOrDefault();
                    //    rsmoi.GhiChuChung = ghichu.ToString();
                    //    BioNet_Bus.SuaDanhSachGanXNLuu(rsmoi);
                    //}
                    //if (columnHandle == this.col_MayXN01_GhiChu.ColumnHandle)
                    //{
                    //    var tenvitri = view.GetRowCellDisplayText(rowfocus, this.col_MayXN01_ViTri);
                    //    var ghichu = view.GetRowCellDisplayText(rowfocus, this.col_MayXN01_GhiChu);
                    //    List<PSCMGanViTriChung> rss = vt.Where(p => p.MAYXN01 != null).ToList();
                    //    PSCMGanViTriChung rsmoi = rss.Where(p => p.MAYXN01.ViTri == tenvitri.ToString() && p.STT_bang == long.Parse(stt.ToString())).FirstOrDefault();
                    //    rsmoi.MAYXN01.GhiChuCT = ghichu.ToString();
                    //    BioNet_Bus.SuaDanhSachGanXNLuu(rsmoi);
                    //}
                    //if (columnHandle == this.col_MaXetNghiem.ColumnHandle)
                    //{

                    //}
                    //if (columnHandle == this.col_MayXN02_GhiChu.ColumnHandle)
                    //{
                    //    var tenvitri = view.GetRowCellDisplayText(rowfocus, this.col_MayXN02_ViTri);
                    //    var ghichu = view.GetRowCellDisplayText(rowfocus, this.col_MayXN02_GhiChu);
                    //    List<PSCMGanViTriChung> rss = vt.Where(p => p.MAYXN02 != null).ToList();
                    //    PSCMGanViTriChung rsmoi = rss.Where(p => p.MAYXN02.ViTri == tenvitri.ToString() && p.STT_bang == long.Parse(stt.ToString())).FirstOrDefault();
                    //    rsmoi.MAYXN02.GhiChuCT = ghichu.ToString();
                    //    BioNet_Bus.SuaDanhSachGanXNLuu(rsmoi);
                    //}
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
            this.GVChuaKQ.ExpandAllGroups();
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
                    string filterMaPhieu = "Contains([MaPhieu], '" + txtSearchMaPhieuChuaCoKQ.EditValue.ToString().Trim() + "')";
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
                FilterDatetime();
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
                    GanVaoDSNew(ph);
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
                    BioNet_Bus.DeleteDanhSachAllGanXNLuu(IDLanDucLo,emp.EmployeeCode);
                    this.GetIDLanGanXN();
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

        private void txtDenNgay_ChuaKQ_EditValueChanged_1(object sender, EventArgs e)
        {
            FilterDatetime();
        }

        private void FilterDatetime()
        {
            try
            {
                string filterMaDV;
                filterMaDV = "[NgayLamXetNghiem] Between(#" + txtTuNgay_ChuaKQ.DateTime.ToString("yyyy'-'MM'-'dd") + "#,#" + txtDenNgay_ChuaKQ.DateTime.ToString("yyyy'-'MM'-'dd") + "#)";
                GVChuaKQ.Columns["NgayLamXetNghiem"].FilterInfo = new ColumnFilterInfo(filterMaDV);
            }
            catch
            {
                GVChuaKQ.ClearColumnsFilter();
                GVChuaKQ.ExpandAllGroups();
            }
        }
    }
}