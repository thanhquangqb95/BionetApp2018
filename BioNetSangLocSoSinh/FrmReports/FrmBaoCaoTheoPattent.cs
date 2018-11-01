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
using DevExpress.XtraSplashScreen;
using BioNetSangLocSoSinh.DiaglogFrm;
using BioNetModel.Data;

namespace BioNetSangLocSoSinh.FrmReports
{
    public partial class FrmBaoCaoTheoPattent : DevExpress.XtraEditors.XtraForm
    {
        public FrmBaoCaoTheoPattent()
        {
            InitializeComponent();
        }
        int gio = 0, phut = 0, giay = 0;
        private List<PSDanhMucGoiDichVuChung> lstgoiXN = new List<PSDanhMucGoiDichVuChung>();
        private void btnThongke_Click(object sender, EventArgs e)
        {
            try
            {
                string MaDonVi = String.Empty;
                if (this.txtDonVi.EditValue.ToString() == "all")
                {
                    if (this.txtChiCuc.EditValue.ToString() == "all")
                    {
                        MaDonVi = "all";
                    }
                    else
                    {
                        MaDonVi = this.txtChiCuc.EditValue.ToString();
                    }
                }
                else
                {
                    MaDonVi = this.txtDonVi.EditValue.ToString();
                }
                SplashScreenManager.ShowForm(typeof(WaitingLoadData), true, false);
                gio = 0; phut = 0; giay = 0;
                timer1.Interval = 1000;
                timer1.Start();
                DateTime TIme1 = DateTime.Now;
                GCDanhSachMauDuongTinh.DataSource = null;
                GCDanhSachMauDuongTinh.DataSource = BioNet_Bus.LoadDSBaoCaoTuyChonDichVu(dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date, cbbDichVu.EditValue.ToString(), MaDonVi);
                SplashScreenManager.CloseForm();
                DateTime TIme2 = DateTime.Now;
            }
            catch
            {

            }
        }

        private void FrmBaoCaoTheoPattent_Load(object sender, EventArgs e)
        {
           this.LoadGoiDichVuXetNGhiem();
            cbbDichVu.Properties.DataSource = BioNet_Bus.GetDanhSachDichVu(false);
            this.txtChiCuc.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            dllNgay.tungay.Value = DateTime.Now;
            dllNgay.denngay.Value = DateTime.Now;
            this.txtChiCuc.EditValue = "all";
        }
        private void LoadGoiDichVuXetNGhiem()
        {
            try
            {

                this.lstgoiXN = BioNet_Bus.GetDanhsachGoiDichVuChung();
                this.LookUpEditGoiXN.DataSource = null;
                this.LookUpEditGoiXN.DataSource = this.lstgoiXN;
            }
            catch { }
        }
        private void txtChiCuc_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit sear = sender as SearchLookUpEdit;
                var value = sear.EditValue.ToString();
                this.txtDonVi.Properties.DataSource = null;
                this.txtDonVi.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi(value.ToString());
                this.txtDonVi.EditValue = "all";
                if (txtDonVi.EditValue.ToString() != "all")
                {
                }
                else
                {
                }
            }
            catch { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            giay++;
            if (giay == 60)
            {
                phut++;
                giay = 0;
            }
            if (phut == 60)
            {
                gio++;
                phut = 0;
            }
            
        }
        
    }
}