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
        public static List<PSCMGanViTriChung> vt = new List<PSCMGanViTriChung>();
        public static List<PSDanhMucMayXN> dsmay = new List<PSDanhMucMayXN>();
        public static List<PSMapsViTriMayXN> mapViTri = new List<PSMapsViTriMayXN>();

        private void FrmGanViTriMayXN_Load(object sender, EventArgs e)
        {
            vt=new List<PSCMGanViTriChung>();
            dsmay = BioNet_Bus.GetDSMayXN();
            
            if (dsmay.Count > 0)
            {
                foreach (var may in dsmay)
                {
                    mapViTri.AddRange(BioNet_Bus.GetDSMapViTriMayXN(may.IDMayXN));
                    List<PSMapsViTriMayXN> a = BioNet_Bus.GetDSMapViTriMayXN(may.IDMayXN);
                    switch (may.IDMayXN)
                    {
                        case "MAYXN01":
                            {

                                LookupMayXN01.DataSource = from res in a
                                                           select new { res.STT, res.TenViTri,res.isTest };
                                break;
                            }
                        case "MAYXN02":
                            {
                                LookupMayXN02.DataSource = from res in a
                                                           select new { res.STT, res.TenViTri, res.isTest };
                                break;
                            }
                        default:
                            break;
                    }
                                
                }
            }
            //        GridBand band = new GridBand();
            //        band.Name = may.IDMayXN;
            //        band.Caption = may.TenMayXN;
            //        band.RowCount = 1;
            //        band.Visible = true;
            //        gridBandViTri.Children.Add(band);    

            //        DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            //        col2.Name = may.IDMayXN + "_ViTri";
            //        col2.FieldName = may.IDMayXN + ".ViTri";
            //        col2.Caption = "Vị Trí";
            //        col2.OptionsColumn.AllowEdit = false;
            //        col2.Visible = true;
            //        band.Columns.Add(col2);


            //        DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            //        col1.Name = may.IDMayXN + "_GhiChu";
            //        col1.FieldName = may.IDMayXN + ".GhiChu";
            //        col1.Caption = "Ghi chú";
            //        col1.OptionsColumn.AllowEdit = false;
            //        col1.Visible = true;
            //        band.Columns.Add(col1);
            //    }
            //}
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
                                    PSCM_GanViTriCT gvt = new PSCM_GanViTriCT();
                                    gvt.MaPhieu = ph.MaPhieu;
                                    gvt.MaXetNghiem = ph.MaXetNghiem;
                                    gvt.isDaDuyet = false;
                                    gvt.IDMayXN = ctm.IDMayXN;
                                    switch (ctm.IDMayXN)
                                    {
                                        case "MAYXN01":
                                            {
                                                gvt.STT = vt.Where(x => x.MAYXN01 != null).Count() + 1;

                                                break;
                                            }
                                        case "MAYXN02":
                                            {
                                                gvt.STT = vt.Where(x => x.MAYXN02 != null).Count() + 1;
                                                break;
                                            }
                                        default:
                                            break;
                                    }

                                    gvt.STTDia = 1;
                                    long? Max = mapViTri.Where(x => x.IDMayXN == ctm.IDMayXN).Count();
                                    long? STTGan = gvt.STT % Max == 0 ? Max : gvt.STT % Max;
                                    var VitriGan = mapViTri.FirstOrDefault(x => x.IDMayXN == ctm.IDMayXN && x.STT == STTGan);
                                    gvt.ViTri = mapViTri.FirstOrDefault(x => x.IDMayXN == ctm.IDMayXN && x.STT == gvt.STT).TenViTri;
                                    ViTriXN vtxn = new ViTriXN();
                                    vtxn.ViTri = gvt.ViTri;
                                    switch (ctm.IDMayXN)
                                    {
                                        case "MAYXN01":
                                            {
                                                ph.MAYXN01 = vtxn;
                                                break;
                                            }
                                        case "MAYXN02":
                                            {
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
                    GCDanhSachGanViTri.DataSource = null;
                    GCDanhSachGanViTri.DataSource = vt;
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
                long? dem = 1;
                switch (IDMayXN)
                {
                    case "MAYXN01":
                        {
                            dem = vt.Where(x => x.MAYXN01 != null).Count() + 1;

                            break;
                        }
                    case "MAYXN02":
                        {
                            dem = vt.Where(x => x.MAYXN02 != null).Count() + 1;

                            break;
                        }
                    default:
                        break;
                }
                long? Max = mapViTri.Where(x => x.IDMayXN == IDMayXN).Count();
                long? STTGan = dem % Max == 0 ? Max : dem % Max;
                var VitriGan = mapViTri.FirstOrDefault(x => x.IDMayXN == IDMayXN && x.STT == STTGan);
                if (VitriGan.isTest == true)
                {
                    result = true;
                    PSCMGanViTriChung test = new PSCMGanViTriChung();
                    test.MaXetNghiem = VitriGan.GiaTriTest;
                    test.STT_bang = vt.Count + 1;
                    test.MaGoiXN = "DVTest";
                    ViTriXN vtxn = new ViTriXN();
                    vtxn.ViTri = VitriGan.TenViTri;
                    switch (VitriGan.IDMayXN)
                    {
                        case "MAYXN01":
                            {
                                test.MAYXN01 = vtxn;
                                break;
                            }
                        case "MAYXN02":
                            {
                                test.MAYXN02=vtxn;
                                break;
                            }
                        default:
                            break;
                    }
                    vt.Add(test);
                }
                GCDanhSachGanViTri.DataSource = null;
                GCDanhSachGanViTri.DataSource = vt;
            }
            catch
            {
              
            }
            return result;
        }

        private void GVDanhSachGanViTri_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            var columnHandle = e.Column.ColumnHandle;
            var rowHandle = e.RowHandle;
            var stt = view.GetRowCellValue(rowHandle, this.col_STT);
            var idPhieu = view.GetRowCellValue(rowHandle, this.col_IDPhieu);
            var maxn = view.GetRowCellValue(rowHandle, this.col_MaXetNghiem);
            var tenvitri = view.GetRowCellValue(rowHandle, this.col_MayXN01_ViTri);
            if (rowHandle < view.RowCount)
            {
                if (columnHandle == this.col_MayXN01_ViTri.ColumnHandle)
                {
                    List<PSCMGanViTriChung> rss = vt.Where(p => p.MAYXN01 != null).ToList();
                    PSCMGanViTriChung rs = rss.Where(p => p.MAYXN01.ViTri==tenvitri.ToString()).FirstOrDefault();
                    
                    var valueOld = view.ActiveEditor.OldEditValue;
                    if (rs != null)
                    {
                        if (!string.IsNullOrEmpty(valueOld.ToString()))
                        {
                            long? gt = rs.STT_bang;
                            vt.Remove(rs);
                           foreach(var v in vt)
                            {
                                if(v.STT_bang>gt)
                                {
                                    v.STT_bang = v.STT_bang - 1;
                                }                  
                            }
                        }

                    }
                }
            }
            GCDanhSachGanViTri.DataSource = null;
            GCDanhSachGanViTri.DataSource = vt;
        }
    }
}