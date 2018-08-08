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

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmGanViTriMayXN : DevExpress.XtraEditors.XtraForm
    {
        public FrmGanViTriMayXN()
        {
            InitializeComponent();
        }
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
            this.vtMayXN01_Tam.Clear();
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
                goi2.IDGoiDichVuChung = "DVKhac";
                goi2.TenGoiDichVuChung = "Khac";
                goi2.Stt = 8;
                goi2.DonGia = 0;
                goi2.ChietKhau = 0;
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
                                                SttCt = vtMayXN01.Max(x => x.STT) == null ? 1 : vtMayXN01.Max(x => x.STT) + 1;
                                                break;
                                            }
                                        case "MAYXN02":
                                            {
                                                SttCt = vtMayXN02.Max(x => x.STT) == null ? 1 : vtMayXN02.Max(x => x.STT) + 1;
                                                break;
                                            }
                                        default:
                                            break;
                                    }

                                    long? MaxViTri = mapViTri.Where(x => x.IDMayXN == ctm.IDMayXN).Count();
                                    long? STTVTGan = SttCt % MaxViTri == 0 ? MaxViTri : SttCt % MaxViTri;
                                    long? STTDia = STTVTGan / MaxViTri + 1;
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
                                                
                                                if(VitriGan.isTest!=true)
                                                {
                                                    this.vtMayXN01.Add(vtxn);
                                                    this.vtMayXN01_Tam.Add(vtxn);
                                                }
                                                ph.MAYXN01 = new ViTriXN();
                                                ph.MAYXN01.ViTri = vtxn.ViTri;
                                                break;
                                            }
                                        case "MAYXN02":
                                            {                          
                                                if (VitriGan.isTest != true)
                                                {
                                                    this.vtMayXN02.Add(vtxn);
                                                }
                                                ph.MAYXN02 = vtxn;
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
                    }
                    
                    LoadDanhSachGanVT();
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
                            SttCt = vtMayXN01.Max(x => x.STT) == null ? 1 : vtMayXN01.Max(x => x.STT) + 1;
                            break;
                        }
                    case "MAYXN02":
                        {
                            SttCt = vtMayXN02.Max(x => x.STT) == null ? 1 : vtMayXN02.Max(x => x.STT) + 1;
                            break;
                        }
                    default:
                        break;
                }

                long? MaxViTri = mapViTri.Where(x => x.IDMayXN == IDMayXN).Count();
                long? STTVTGan = SttCt % MaxViTri == 0 ? MaxViTri : SttCt % MaxViTri;
                long? STTDia = STTVTGan / MaxViTri + 1;
                var VitriGan = mapViTri.FirstOrDefault(x => x.IDMayXN == IDMayXN && x.STT == STTVTGan);
                if (VitriGan.isTest == true)
                {
                    result = true;
                    PSCMGanViTriChung test = new PSCMGanViTriChung();
                    test.MaXetNghiem = VitriGan.GiaTriTest;
                    test.STT_bang = vt.Count + 1;
                    test.MaGoiXN = "DVTest";
                    ViTriXN vtxn = new ViTriXN
                    {ViTri = STTDia + VitriGan.TenViTri,
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
                                this.vtMayXN01_Tam.Add(vtxn);
                                test.MAYXN01.ViTri = vtxn.ViTri;
                                break;
                            }
                        case "MAYXN02":
                            {                          
                                this.vtMayXN02.Add(vtxn);
                                test.MAYXN02 = vtxn;
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
        public  List<ViTriXN> vtc = new List<ViTriXN>();
        private void LoadDanhSachGanVT()
        {

            this.LookupMayXN01.DataSource = null;
            this.LookupMayXN01.DataSource =this.vtMayXN01;           
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

        }

        private void LookupMayXN01_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void GVDanhSachGanViTri_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            GridView view = sender as GridView;
            int rowfocus = e.RowHandle;
            var stt = view.GetRowCellValue(rowfocus, this.col_STT);
            var idPhieu = view.GetRowCellValue(rowfocus, this.col_IDPhieu);
            var maxn = view.GetRowCellValue(rowfocus, this.col_MaXetNghiem);
          if (e.Valid)
            {
                var tenvitri = view.GetRowCellDisplayText(rowfocus, this.col_MayXN01_ViTri);
                var STT = view.GetRowCellValue(rowfocus, this.col_MayXN01_STT);
                var STTDia = view.GetRowCellDisplayText(rowfocus, this.col_MayXN01_STTDia);
                List<PSCMGanViTriChung> rss = vt.Where(p => p.MAYXN01 != null).ToList();
                PSCMGanViTriChung rsmoi = rss.Where(p => p.MAYXN01.ViTri == tenvitri.ToString() && p.STT_bang == long.Parse(stt.ToString())).FirstOrDefault();

                ViTriXN vtmoi = vtMayXN01.Where(p => p.ViTri == tenvitri.ToString()).FirstOrDefault();
                var valueOld = view.ActiveEditor.OldEditValue;
                ViTriXN vtcu = vtMayXN01.Where(p => p.ViTri == valueOld.ToString()).FirstOrDefault();
                PSCMGanViTriChung rscu = rss.Where(p => p.MAYXN01.ViTri == valueOld.ToString() && p.MAYXN01.STT == long.Parse(stt.ToString())).FirstOrDefault();

                if (vtcu != null && rsmoi != null)
                {
                    ViTriXN VTTam = new ViTriXN();
                    long? gt = rscu.STT_bang;
                    long? SttCtMax = vtMayXN01.Max(x => x.STT) == null ? 1 : vtMayXN01.Max(x => x.STT);
                    var xn = vtMayXN01.FirstOrDefault(x => x.ViTri == valueOld.ToString());
                    if (xn != null)
                    {
                        VTTam = vtmoi;
                        if (vtmoi.isTest)
                        {
                            rsmoi.MAYXN01.isTest = false;
                            vtMayXN01.Remove(vtcu);
                            vt.Remove(rscu);
                            foreach (var v in vt)
                            {
                                if (v.STT_bang > gt)
                                {
                                    v.STT_bang = v.STT_bang - 1;
                                }
                            }
                        }
                        else
                        {
                            rsmoi.MAYXN01.STT = rscu.MAYXN01.STT;
                            rsmoi.MAYXN01.STTDia = rscu.MAYXN01.STTDia;
                            rsmoi.MAYXN01.STTVT = rscu.MAYXN01.STTVT;
                            rsmoi.MAYXN01.ViTri = rscu.MAYXN01.ViTri;

                            rscu.MAYXN01.STT = VTTam.STT;
                            rscu.MAYXN01.STTDia = VTTam.STTDia;
                            rscu.MAYXN01.STTVT = VTTam.STTVT;
                            rscu.MAYXN01.ViTri = VTTam.ViTri;
                        }
                    }
                }
            }
            LoadDanhSachGanVT();
        }
    }
}