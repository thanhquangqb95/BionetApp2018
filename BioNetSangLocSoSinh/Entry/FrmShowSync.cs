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
using BioNetModel;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using BioNetSangLocSoSinh.DiaglogFrm;
using BioNetBLL;

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmShowSync : DevExpress.XtraEditors.XtraForm
    {
        public static string PathDir = Application.StartupPath + "\\CTDongBo";
        public static string pathLoi = PathDir + "\\Sync" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + ".txt";
        public FrmShowSync()
        {
            InitializeComponent();
        }
        public void ShowSync (DateTime datestart,DateTime dateend)
            {
            List<PsLoiDongBocs> dsSync = new List<PsLoiDongBocs>();
            PsLoiDongBocs psloi = new PsLoiDongBocs();
            string[] fileEntries = Directory.GetFiles(PathDir);
            foreach (string file in fileEntries)
            {
                List<PsLoiDongBocs> list = new List<PsLoiDongBocs>();
                string text = File.ReadAllText(file);
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                list = jsonSerializer.Deserialize<List<PsLoiDongBocs>>(text);
                dsSync.AddRange(list);
            }
            dsSync= dsSync.Where(x => x.DateDB.Date >= datestart.Date && x.DateDB.Date <= dateend.Date).ToList();
            GCShowKQSync.DataSource = dsSync;
           
        }
        public void CapNhatSync(int stt,DateTime date,List<string> mphieu)
        {
            List<PsLoiDongBocs> dsSync = new List<PsLoiDongBocs>();
            PsLoiDongBocs psloi = new PsLoiDongBocs();

            string[] fileEntries = Directory.GetFiles(PathDir);
            string pathLoi = PathDir + "\\Sync" + date.Day + date.Month + date.Year + ".txt";
            
                List<PsLoiDongBocs> list = new List<PsLoiDongBocs>();
                string text = File.ReadAllText(pathLoi);
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                list = jsonSerializer.Deserialize<List<PsLoiDongBocs>>(text);
            foreach(var lst in list)
            {
                if(lst.STT==stt)
                {
                    lst.NoiDungLoi= String.Join(", ", mphieu.ToArray());
                    if(string.IsNullOrEmpty(lst.NoiDungLoi))
                    {
                        lst.TrangThaiDB = true;
                    }
                    break;
                }
            }
            using (StreamWriter file = File.CreateText(pathLoi))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, list);
            }
        }
        private void GCShowKQSync_Load(object sender, EventArgs e)
        {

            ShowSync(this.dllNgay.tungay.Value, this.dllNgay.denngay.Value);
        }
    
        private void butOK_Click(object sender, EventArgs e)
        {
            ShowSync(this.dllNgay.tungay.Value, this.dllNgay.denngay.Value);
        }

        private void GVShowKQSync_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {

        }

        private void GVShowKQSync_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                GridView View = sender as GridView;
                if (e.RowHandle >= 0)
                {
                    bool kq = View.GetRowCellDisplayText(e.RowHandle, col_KQSync) == null ? false : (bool)View.GetRowCellValue(e.RowHandle, this.col_KQSync);
                    if (!kq)
                    {
                        e.Appearance.BackColor = Color.Orange;
                        e.Appearance.BackColor2 = Color.Silver;
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

        private void GVShowKQSync_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            bool enable = false;
            if (e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell)
            {
                enable = true;
                popupMenuGVChuaKetQua.ItemLinks[0].Visible = enable;
                popupMenuGVChuaKetQua.ShowPopup(GCShowKQSync.PointToScreen(e.Point));
            }
            else
            {
                enable = false;
                popupMenuGVChuaKetQua.ItemLinks[0].Visible = enable;
                popupMenuGVChuaKetQua.ShowPopup(GCShowKQSync.PointToScreen(e.Point));
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PsReponse res = new PsReponse();
            res.Result = true;
            string maPhieuhoan = this.GVShowKQSync.GetRowCellValue(this.GVShowKQSync.FocusedRowHandle, this.col_NoiDungLoi).ToString();
            string date = this.GVShowKQSync.GetRowCellValue(this.GVShowKQSync.FocusedRowHandle, this.col_DateSync).ToString();
            string stt = this.GVShowKQSync.GetRowCellValue(this.GVShowKQSync.FocusedRowHandle, this.col_STT).ToString();
            if (XtraMessageBox.Show("Bạn chắn chắn muốn hoàn đồng bộ danh sách phiếu này?", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                List<string> dsphieu = maPhieuhoan.Split(',').ToList();
                List<string> dsphieuloi = new List<string>();
                foreach(var ph in dsphieu)
                {
                    PsReponse rese = BioNet_Bus.HoanDongBoPhieu(ph);
                    if (rese.Result == false)
                    {
                        res.Result = false;
                        dsphieuloi.Add(ph);
                    }                  
                }
                CapNhatSync(Int32.Parse(stt), DateTime.Parse(date), dsphieuloi);
                if (res.Result==true)
                {
                    XtraMessageBox.Show("Hoàn đồng bộ thành công", "BioNet - Chương trình sàng lọc sơ sinh!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("Hoàn đồng bộ thất bại.", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }                
            }            
        }
    }
}