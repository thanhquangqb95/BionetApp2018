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
           
            dsmay = BioNet_Bus.GetDSMayXN();
            if (dsmay.Count > 0)
            {
                foreach (var may in dsmay)
                {
                    GridBand band = new GridBand();
                    band.Name = may.IDMayXN;
                    band.Caption = may.TenMayXN;
                    band.RowCount = 1;
                    band.Visible = true;
                    gridBandViTri.Children.Add(band);    

                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                    col2.Name = may.IDMayXN + "_ViTri";
                    col2.FieldName = may.IDMayXN + ".ViTri";
                    col2.Caption = "Vị Trí";
                    col2.OptionsColumn.AllowEdit = false;
                    col2.Visible = true;
                    band.Columns.Add(col2);

                    mapViTri.AddRange(BioNet_Bus.GetDSMapViTriMayXN(may.IDMayXN));
                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                    col1.Name = may.IDMayXN + "_GhiChu";
                    col1.FieldName = may.IDMayXN + ".GhiChu";
                    col1.Caption = "Ghi chú";
                    col1.OptionsColumn.AllowEdit = false;
                    col1.Visible = true;
                    band.Columns.Add(col1);
                }
            }
        }

        private void txtMaPhieu_Enter(object sender, EventArgs e)
        {
            
                
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
                        ph.STT_bang = vt.Count + 1;
                        if(vt.Count>0)
                        {
                            if (ph.may.Count() > 0)
                            {
                                foreach (var ctm in ph.may)
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
                                                gvt.STT = vt.Select(y=>y.MAYXN01).Where(x => x.MAYXN01.Where(y=>y. !=null)).Count() + 1;
                                                break;
                                            }
                                        case "MAYXN02":
                                            {
                                                gvt.STT = vt.Select(x => x.MAYXN02).Count() + 1;
                                                break;
                                            }
                                        default:
                                            break;
                                    }
                                   
                                    gvt.STTDia = 1;
                                    long? Max = mapViTri.Where(x => x.IDMayXN == ctm.IDMayXN).Count();
                                    
                                    if (gvt.STT > Max)
                                    {

                                    }
                                    else
                                    {
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
                                    }

                                }
                            }
                            vt.Add(ph);
                        }
                        else
                        {
                            if (ph.may.Count() > 0)
                            {
                                foreach (var ctm in ph.may)
                                {
                                    PSCM_GanViTriCT gvt = new PSCM_GanViTriCT();
                                    gvt.MaPhieu = ph.MaPhieu;
                                    gvt.MaXetNghiem = ph.MaXetNghiem;
                                    gvt.isDaDuyet = false;
                                    gvt.IDMayXN = ctm.IDMayXN;
                                    gvt.STT = 1;
                                    gvt.STTDia = 1;
                                    long? Max= mapViTri.Where(x => x.IDMayXN == ctm.IDMayXN ).Count();
                                    if (gvt.STT >Max)
                                    {

                                    }
                                    else
                                    {
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
                                    }
                                         
                                }
                            }
                            vt.Add(ph);
                        }
                        GCDanhSachGanViTri.DataSource = null;
                        GCDanhSachGanViTri.DataSource = vt;
                    }
                }
            }
            catch
            {

            }
           

        }
    }
}