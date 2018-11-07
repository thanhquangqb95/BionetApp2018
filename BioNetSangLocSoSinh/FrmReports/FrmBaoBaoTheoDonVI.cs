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
            //cbbDichVu.Properties.DataSource = BioNet_Bus.GetDanhSachDichVu(false);
        
            this.txtChiCuc.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            listDonVi= BioNet_Bus.GetDieuKienLocBaoCao_DonVi("all");
            dllNgay.tungay.Value = DateTime.Now;
            dllNgay.denngay.Value = DateTime.Now;
            this.txtChiCuc.EditValue = "all";
            this.LookupeditDV.DataSource = listDonVi;
            this.LookUpEditTenVietTat.DataSource = listDonVi;
            this.LoadPPSinh();
            this.LoadTuoiMe();
            this.LoadCanNang();
            this.LoadChuongTrinh();
            this.LoadSLSinh();
            this.LoadGoiXN();
            CLPPSinhs.Add(new CLPPSinh() { PPSinh = "0", TenPPSinh = "Sinh thường" });
            CLPPSinhs.Add(new CLPPSinh() { PPSinh = "1", TenPPSinh = "Sinh mổ" });
            CLPPSinhs.Add(new CLPPSinh() { PPSinh = "2", TenPPSinh = "N/A" });
        }
        #region Load cột
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
        private void LoadChuongTrinh()
        {
            GridBand band = new GridBand();
            band.Name = "LoadChuongTrinh";
            band.Caption = "Thông tin chương trình";
            band.RowCount = 1;
            band.Visible = true;
            this.GVDanhSachDonVi.Bands.Add(band);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col4 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col4.Name = "ChuongTrinhQuocGia";
            col4.FieldName = "PSTKChuongTrinh.QuocGia";
            col4.Caption = "Quốc gia";
            col4.OptionsColumn.AllowEdit = false;
            col4.Visible = true;
            band.Columns.Add(col4);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col3.Name = "ChuongTrinhXaHoi";
            col3.FieldName = "PSTKChuongTrinh.XaHoi";
            col3.Caption = "Xã hội hóa";
            col3.OptionsColumn.AllowEdit = false;
            col3.Visible = true;
            band.Columns.Add(col3);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col2.Name = "ChuongTrinhDemo";
            col2.FieldName = "PSTKChuongTrinh.Demo";
            col2.Caption = "Demo";
            col2.OptionsColumn.AllowEdit = false;
            col2.Visible = true;
            band.Columns.Add(col2);
        }
        private void LoadSLSinh()
        {
            GridBand band = new GridBand();
            band.Name = "LoadSLSinh";
            band.Caption = "Thông tin Para";
            band.RowCount = 1;
            band.Visible = true;
            this.GVDanhSachDonVi.Bands.Add(band);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col4 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col4.Name = "PSTKCon3Sinh";
            col4.FieldName = "PSTKCon.Sinh3Con";
            col4.Caption = "Sinh con thứ 3";
            col4.OptionsColumn.AllowEdit = false;
            col4.Visible = true;
            band.Columns.Add(col4);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col3.Name = "PSTKCon4Sinh";
            col3.FieldName = "PSTKCon.Sinh4Con";
            col3.Caption = "Sinh con thứ 4";
            col3.OptionsColumn.AllowEdit = false;
            col3.Visible = true;
            band.Columns.Add(col3);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col2.Name = "PSTKCon5Sinh";
            col2.FieldName = "PSTKCon.SinhTu5Con";
            col2.Caption = "Sinh con thứ 5 trờ lên";
            col2.OptionsColumn.AllowEdit = false;
            col2.Visible = true;
            band.Columns.Add(col2);
        }
        private void LoadGoiXN()
        {
            GridBand band = new GridBand();
            band.Name = "LoadGoiXN";
            band.Caption = "Thông tin gói xét nghiệm";
            band.RowCount = 1;
            band.Visible = true;
            this.GVDanhSachDonVi.Bands.Add(band);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col4 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col4.Name = "PSTKCon3Sinh";
            col4.FieldName = "PSThongKeGoiBenh.Benh2";
            col4.Caption = "2 Bệnh";
            col4.OptionsColumn.AllowEdit = false;
            col4.Visible = true;
            band.Columns.Add(col4);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col3.Name = "PSTKCon4Sinh";
            col3.FieldName = "PSThongKeGoiBenh.Benh3";
            col3.Caption = "3 Bệnh";
            col3.OptionsColumn.AllowEdit = false;
            col3.Visible = true;
            band.Columns.Add(col3);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col2.Name = "PSTKCon5Sinh";
            col2.FieldName = "PSThongKeGoiBenh.Benh5";
            col2.Caption = "5 Bệnh";
            col2.OptionsColumn.AllowEdit = false;
            col2.Visible = true;
            band.Columns.Add(col2);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col5 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col5.Name = "Benh2Hemo";
            col5.FieldName = "PSThongKeGoiBenh.Benh2";
            col5.Caption = "2 Bệnh + Hemo";
            col5.OptionsColumn.AllowEdit = false;
            col5.Visible = true;
            band.Columns.Add(col5);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col6 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col6.Name = "Benh3Hemo";
            col6.FieldName = "PSThongKeGoiBenh.Benh3Hemo";
            col6.Caption = "3 Bệnh +Hemo";
            col6.OptionsColumn.AllowEdit = false;
            col6.Visible = true;
            band.Columns.Add(col6);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col7 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col7.Name = "Benh5Hemo";
            col7.FieldName = "PSThongKeGoiBenh.Benh5Hemo";
            col7.Caption = "5 Bệnh + Hemo";
            col7.OptionsColumn.AllowEdit = false;
            col7.Visible = true;
            band.Columns.Add(col7);
        }
        private void LoadTuoiMe()
        {
            GridBand band = new GridBand();
            band.Name = "TuoiMe";
            band.Caption = "Thông tin tuổi mẹ";
            band.RowCount = 1;
            band.Visible = true;
            this.GVDanhSachDonVi.Bands.Add(band);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col1.Name = "TuoiMeDuoi13";
            col1.FieldName = "PSThongKeTuoiMe.Duoi13";
            col1.Caption = "Dưới 13 tuổi";
            col1.OptionsColumn.AllowEdit = false;
            col1.Visible = true;
            band.Columns.Add(col1);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col2.Name = "TuoiMeTuoi14";
            col2.FieldName = "PSThongKeTuoiMe.Tuoi14";
            col2.Caption = "14 tuổi";
            col2.OptionsColumn.AllowEdit = false;
            col2.Visible = true;
            band.Columns.Add(col2);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col3.Name = "TuoiMeTuoi15";
            col3.FieldName = "PSThongKeTuoiMe.Tuoi15";
            col3.Caption = "15 tuổi";
            col3.OptionsColumn.AllowEdit = false;
            col3.Visible = true;
            band.Columns.Add(col3);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col4 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col4.Name = "TuoiMeTuoi16";
            col4.FieldName = "PSThongKeTuoiMe.Tuoi16";
            col4.Caption = "16 tuổi";
            col4.OptionsColumn.AllowEdit = false;
            col4.Visible = true;
            band.Columns.Add(col4);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col5 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col5.Name = "TuoiMeTuoi17";
            col5.FieldName = "PSThongKeTuoiMe.Tuoi17";
            col5.Caption = "17 tuổi";
            col5.OptionsColumn.AllowEdit = false;
            col5.Visible = true;
            band.Columns.Add(col5);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col6 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col6.Name = "TuoiMe20den25";
            col6.FieldName = "PSThongKeTuoiMe.Tuoi20den25";
            col6.Caption = "Từ 21 đến 25 tuổi";
            col6.OptionsColumn.AllowEdit = false;
            col6.Visible = true;
            band.Columns.Add(col6);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col7 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col7.Name = "TuoiMe25den30";
            col7.FieldName = "PSThongKeTuoiMe.Tuoi25den30";
            col7.Caption = "Từ 26 đến 30 tuổi";
            col7.OptionsColumn.AllowEdit = false;
            col7.Visible = true;
            band.Columns.Add(col7);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col9 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col9.Name = "TuoiMe30den35";
            col9.FieldName = "PSThongKeTuoiMe.Tuoi30den35";
            col9.Caption = "Từ 31 đến 35 tuổi";
            col9.OptionsColumn.AllowEdit = false;
            col9.Visible = true;
            band.Columns.Add(col9);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col10 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col10.Name = "TuoiMe35den40";
            col10.FieldName = "PSThongKeTuoiMe.Tuoi35den40";
            col10.Caption = "Từ 36 đến 40 tuổi";
            col10.OptionsColumn.AllowEdit = false;
            col10.Visible = true;
            band.Columns.Add(col10);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col8 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col8.Name = "TuoiMe40den45";
            col8.FieldName = "PSThongKeTuoiMe.Tuoi40den45";
            col8.Caption = "Từ 41 đến 45 tuổi";
            col8.OptionsColumn.AllowEdit = false;
            col8.Visible = true;
            band.Columns.Add(col8);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col11 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col11.Name = "TuoiMeTren45";
            col11.FieldName = "PSThongKeTuoiMe.TuoiTren45";
            col11.Caption = "Trên 46 tuổi";
            col11.OptionsColumn.AllowEdit = false;
            col11.Visible = true;
            band.Columns.Add(col11);


        }
        private void LoadCanNang()
        {
            GridBand band = new GridBand();
            band.Name = "TuoiMe";
            band.Caption = "Thông tin cân nặng";
            band.RowCount = 1;
            band.Visible = true;
            this.GVDanhSachDonVi.Bands.Add(band);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col1.Name = "TuoiMeDuoi13";
            col1.FieldName = "PSThongKeCanNang.Duoi25";
            col1.Caption = "Dưới 2,5 kg";
            col1.OptionsColumn.AllowEdit = false;
            col1.Visible = true;
            band.Columns.Add(col1);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col2.Name = "TuoiMeTuoi14";
            col2.FieldName = "PSThongKeCanNang.Tu25Den30";
            col2.Caption = "Từ 2,5kg đến 3,0kg";
            col2.OptionsColumn.AllowEdit = false;
            col2.Visible = true;
            band.Columns.Add(col2);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col3.Name = "TuoiMeTuoi15";
            col3.FieldName = "PSThongKeCanNang.Tu30Den35";
            col3.Caption = "Từ 3,0kg đến 3,5kg";
            col3.OptionsColumn.AllowEdit = false;
            col3.Visible = true;
            band.Columns.Add(col3);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col4 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col4.Name = "TuoiMeTuoi16";
            col4.FieldName = "PSThongKeCanNang.Tu35Den40";
            col4.Caption = "Từ 3,5kg đến 4,0kg";
            col4.OptionsColumn.AllowEdit = false;
            col4.Visible = true;
            band.Columns.Add(col4);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col5 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col5.Name = "TuoiMeTuoi17";
            col5.FieldName = "PSThongKeCanNang.Tu40Den45";
            col5.Caption = "Từ 4,0kg đến 4,5kg";
            col5.OptionsColumn.AllowEdit = false;
            col5.Visible = true;
            band.Columns.Add(col5);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col6 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col6.Name = "TuoiMe20den25";
            col6.FieldName = "PSThongKeCanNang.Tu45Den50";
            col6.Caption = "Từ 4,5kg đến 5,0kg";
            col6.OptionsColumn.AllowEdit = false;
            col6.Visible = true;
            band.Columns.Add(col6);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col7 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col7.Name = "TuoiMe25den30";
            col7.FieldName = "PSThongKeCanNang.Tren50";
            col7.Caption = "Từ 5kg trở lên";
            col7.OptionsColumn.AllowEdit = false;
            col7.Visible = true;
            band.Columns.Add(col7);
        }
        
        #endregion
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
            
            //this.GVDanhSachDonVi.Bands.Remove(2);
            //switch(cbbThongKe.EditValue)
            //{
            //    case "0":
            //        {
            //            ThongKeTuoiMe();
            //            ThongKePPSinh();
            //            break;
            //        }
            //    case "1":
            //        {
            //            ThongKeTuoiMe();
            //            break;
            //        }
            //    case "2":
            //        {
            //            ThongKePPSinh();
            //            break;
            //        }
            //}
        }
        #region Thông kê
        
        #endregion
    }
}