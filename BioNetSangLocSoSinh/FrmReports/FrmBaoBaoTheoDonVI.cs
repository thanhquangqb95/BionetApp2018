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

namespace BioNetSangLocSoSinh.FrmReports
{
    public partial class FrmBaoBaoTheoDonVI : DevExpress.XtraEditors.XtraForm
    {
        public FrmBaoBaoTheoDonVI()
        {
            InitializeComponent();
        }
        private List<PSBaoCaoTuyChonDonVi> listBaoCao = new List<PSBaoCaoTuyChonDonVi>();
        private List<PSDanhMucDonViCoSo> listDonVi = new List<PSDanhMucDonViCoSo>();
        List<PSThongKeTheoDonVi> lstTK = new List<PSThongKeTheoDonVi>();
        private List<CLPPSinh> CLPPSinhs = new List<CLPPSinh>();
        private void FrmBaoBaoTheoDonVI_Load(object sender, EventArgs e)
        {
            cbbDichVu.Properties.DataSource = BioNet_Bus.GetDanhSachDichVu(false);
            this.txtChiCuc.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            listDonVi= BioNet_Bus.GetDieuKienLocBaoCao_DonVi("all");
            dllNgay.tungay.Value = DateTime.Now;
            dllNgay.denngay.Value = DateTime.Now;
            this.txtChiCuc.EditValue = "all";
            this.LookupeditDV.DataSource = listDonVi;
            this.LookUpEditTenVietTat.DataSource = listDonVi;
            LoadPPSinh();
            CLPPSinhs.Add(new CLPPSinh() { PPSinh = "0", TenPPSinh = "Sinh thường" });
            CLPPSinhs.Add(new CLPPSinh() { PPSinh = "1", TenPPSinh = "Sinh mổ" });
            CLPPSinhs.Add(new CLPPSinh() { PPSinh = "2", TenPPSinh = "N/A" });
        }
        private void LoadPPSinh()
        {
            GridBand band = new GridBand();
            band.Name = "TTTre";
            band.Caption = "Thông tin trẻ";
            band.RowCount = 1;
            band.Visible = true;
            this.GVDanhSachDonVi.Bands.Add(band);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col4 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col4.Name = "PPSinhMo";
            col4.FieldName = "PSTKPPSinh.SinhMo";
            col4.Caption = "Sinh mổ";
            col4.OptionsColumn.AllowEdit = false;
            col4.Visible = true;
            band.Columns.Add(col4);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col3.Name = "PPSinhThuong";
            col3.FieldName = "PSTKPPSinh.SinhThuong";
            col3.Caption = "Sinh thường";
            col3.OptionsColumn.AllowEdit = false;
            col3.Visible = true;
            band.Columns.Add(col3);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col2.Name = "PPSinhNA";
            col2.FieldName = "PSTKPPSinh.NA";
            col2.Caption = "N/A";
            col2.OptionsColumn.AllowEdit = false;
            col2.Visible = true;
            band.Columns.Add(col2);
        }
        private void loadDuLieu()
        {
            GCDanhSachDonVi.DataSource = null;
            GCDanhSachDonVi.DataSource = lstTK;
        }
        public class CLPPSinh
        {
            public string TenPPSinh { get; set; }
            public string PPSinh { get; set; }
        }

        public class CLViTriLayMau
        {
            public string TenViTriLayMau { get; set; }
            public byte? IDViTriLayMau { get; set; }
        }

        public class CLTinhTrangTre
        {
            public string TenTrinhTrangTre { get; set; }
            public string IDTrinhTrangTre { get; set; }
        }

        public class CLCheDoDD
        {
            public string CheDoDinhDuong { get; set; }
            public string IDCheDoDinhDuong { get; set; }
        }
        public class CLGioiTinh
        {
            public string TenGioiTinh { get; set; }
            public string GioiTinh { get; set; }
        }
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
                listBaoCao = null;
                listBaoCao = BioNet_Bus.LoadDSThongKeDonVi(dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date, MaDonVi);
                lstTK = BioNet_Bus.LoadDSThongKeDonViCT(listBaoCao);
                loadDuLieu();
            }
            catch
            {

            }
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

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            ThongKePPSinh();
        }
        #region Thông kê
        private void ThongKePPSinh()
        {
           lstTK= BioNet_Bus.LoadDSThongKePPSinh(listBaoCao, lstTK);
            loadDuLieu();
        }
        #endregion
    }
}