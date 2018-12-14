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
using DevExpress.XtraSplashScreen;
using BioNetSangLocSoSinh.DiaglogFrm;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Diagnostics;

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
        private List<PSDanhMucChiCuc> listChiCuc = new List<PSDanhMucChiCuc>();
        private List<PSThongKeTheoDonVi> lstTK = new List<PSThongKeTheoDonVi>();
        private List<CLPPSinh> CLPPSinhs = new List<CLPPSinh>();
        private List<CLGioiTinh> CLGioiTinhs = new List<CLGioiTinh>();
        private List<pro_Report_BaoCaoTongHopTheoBenhNhanResult> ListBC = new List<pro_Report_BaoCaoTongHopTheoBenhNhanResult>();
        private void FrmBaoBaoTheoDonVI_Load(object sender, EventArgs e)
        {
            //cbbDichVu.Properties.DataSource = BioNet_Bus.GetDanhSachDichVu(false);
           listChiCuc= BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            this.txtChiCuc.Properties.DataSource = listChiCuc;
            listDonVi = BioNet_Bus.GetDieuKienLocBaoCao_DonVi("all");
            dllNgay.tungay.Value = DateTime.Now;
            dllNgay.denngay.Value = DateTime.Now;
            this.txtChiCuc.EditValue = "all";
            this.LookupeditDV.DataSource = listDonVi;
            this.LookUpEditTenVietTat.DataSource = listDonVi;
            this.LoadGioiTinh();
            this.LoadPPSinh();
            this.LoadTuoiMe();
            this.LoadCanNang();
            this.LoadChuongTrinh();
            this.LoadSLSinh();
            this.LoadGoiXN();
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
            col1.Caption = "N/A";
            col1.OptionsColumn.AllowEdit = false;
            col1.Visible = true;
            band.Columns.Add(col1);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col13 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col13.Name = "TuoiMeTuoi13";
            col13.FieldName = "PSThongKeTuoiMe.Tuoi13";
            col13.Caption = "13 tuổi";
            col13.OptionsColumn.AllowEdit = false;
            col13.Visible = true;
            band.Columns.Add(col13);

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

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col12 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col12.Name = "TuoiMe17den20";
            col12.FieldName = "PSThongKeTuoiMe.Tuoi17den20";
            col12.Caption = "Từ 18 đến 20 tuổi";
            col12.OptionsColumn.AllowEdit = false;
            col12.Visible = true;
            band.Columns.Add(col12);

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
        private void LoadGioiTinh()
        {
            GridBand band = new GridBand();
            band.Name = "TTGioiTinh";
            band.Caption = "Giới tính";
            band.RowCount = 1;
            band.Visible = true;
            this.GVDanhSachDonVi.Bands.Add(band);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col3.Name = "GioiTinhNam";
            col3.FieldName = "PSThongKeGioiTinh.Nam";
            col3.Caption = "Nam";
            col3.OptionsColumn.AllowEdit = false;
            col3.Visible = true;
            band.Columns.Add(col3);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col2.Name = "GioiTinhNu";
            col2.FieldName = "PSThongKeGioiTinh.Nu";
            col2.Caption = "Nữ";
            col2.OptionsColumn.AllowEdit = false;
            col2.Visible = true;
            band.Columns.Add(col2);

            DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn col1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            col1.Name = "GioiTinhNA";
            col1.FieldName = "PSThongKeGioiTinh.NA";
            col1.Caption = "N/A";
            col1.OptionsColumn.AllowEdit = false;
            col1.Visible = true;
            band.Columns.Add(col1);
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
        public class CLGioiTinh
        {
            public string TenGioiTinh { get; set; }
            public string GioiTinh { get; set; }
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
                ListBC = null;
                SplashScreenManager.ShowForm(typeof(WaitingLoadData), true, false);
                DateTime TIme1 = DateTime.Now;
                //listBaoCao = BioNet_Bus.LoadDSThongKeDonVi(dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date, MaDonVi);
                ListBC= BioNet_Bus.LoadDSThongKeTheoBenhNhan(dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date, MaDonVi);
                lstTK = BioNet_Bus.LoadDSThongKeDonViCTNew(ListBC);
                loadDuLieu();
                SplashScreenManager.CloseForm();
                DateTime TIme2 = DateTime.Now;
                TimeSpan kt = TIme2 - TIme1;
                txtTime.Text = string.Format("{0:00}:{1:00}:{2:00}", kt.Hours, kt.Minutes, kt.Seconds);
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

        private void btnXuatFile_Click(object sender, EventArgs e)
        {
            if (this.GVDanhSachDonVi.RowCount > 0)
            {
                this.ExportDataToExcelFile();
            }
            else
            {
                XtraMessageBox.Show("Không có dữ liệu, vui lòng lấy dữ liệu lại và kiểm tra điều kiện lọc.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExportDataToExcelFile()
        {
            try
            {
                string TenDonVi = string.Empty;
                if (txtDonVi.EditValue.ToString() == "all")
                {
                    if (txtChiCuc.EditValue.ToString() == "all")
                    {
                        TenDonVi = BioNet_Bus.GetThongTinTrungTam().TenTrungTam;
                    }
                    else
                    {
                        TenDonVi = BioNet_Bus.GetThongTinChiCuc(txtChiCuc.EditValue.ToString()).TenChiCuc;
                    }
                }
                else
                {
                    TenDonVi = BioNet_Bus.GetThongTinDonViCoSo(txtDonVi.EditValue.ToString()).TenDVCS;
                }
                string Folder = BioNet_Bus.GetFileReport("DonVi", "Excel");
                string Link = BioNet_Bus.SaveFileReport(Folder, "Donvi", TenDonVi, dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date, ".xlsx");
                this.GVDanhSachDonVi.ExportToXlsx(Link);
                XtraMessageBox.Show("Xuất file báo cáo thành công", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(Folder);
            }
            catch
            {
                XtraMessageBox.Show("Xuất file báo cáo lỗi", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXuatPDF_Click(object sender, EventArgs e)
        {
            GopPDF();
        }
        private void XuatFile1(PSThongKeTheoDonVi tk, string Link)
        {
            try
            {
                PSThongKePDFDonVi pdfdv = new PSThongKePDFDonVi();
                pdfdv.DonVi = BioNet_Bus.GetThongTinDonViCoSo(tk.MaDV).TenDVCS;
                pdfdv.ThoiGian = "Từ ngày " + dllNgay.tungay.Value.Date.ToShortDateString() + " đến " + dllNgay.denngay.Value.Date.ToShortDateString();
                pdfdv.LuuY = "(Lưu ý: Báo cáo thống kê có giá trị tại thời điểm xuất báo cáo ngày " + DateTime.Now.Date.ToShortDateString() + ")";
                pdfdv.Tong = tk.Tong.ToString();
                pdfdv.GTNam = tk.PSThongKeGioiTinh.Nam.ToString();
                pdfdv.GTNu = tk.PSThongKeGioiTinh.Nu.ToString();
                pdfdv.TLNamNu = String.Format("{0:0.00}", ((double)tk.PSThongKeGioiTinh.Nam / (double)tk.PSThongKeGioiTinh.Nu));
                pdfdv.PSThongKePDFDonViNhom = new List<PSThongKePDFDonViNhom>();
                PSThongKePDFDonViNhom nhom = new PSThongKePDFDonViNhom();
                nhom.STT = 3;
                nhom.TenNhom = "Phương pháp sinh";
                nhom.ThongKe = new List<PSThongKePDFDonViCT>();
                nhom.ThongKe.Add(GetPDFCT("Sinh mổ", tk.PSTKPPSinh.SinhMo, tk.Tong));
                nhom.ThongKe.Add(GetPDFCT("Sinh thường", tk.PSTKPPSinh.SinhThuong, tk.Tong));
                nhom.ThongKe.Add(GetPDFCT("N/A", tk.PSTKPPSinh.NA, tk.Tong));
                pdfdv.PSThongKePDFDonViNhom.Add(nhom);
                PSThongKePDFDonViNhom nhom2 = new PSThongKePDFDonViNhom();
                nhom2.STT = 4;
                nhom2.TenNhom = "Tuổi mẹ khi sinh (Chi tiết tại Trang 2)";
                nhom2.ThongKe = new List<PSThongKePDFDonViCT>();
                nhom2.ThongKe.Add(GetPDFCT("N/A", tk.PSThongKeTuoiMe.Duoi13, tk.Tong));
                nhom2.ThongKe.Add(GetPDFCT("Dưới 18 tuổi", tk.PSThongKeTuoiMe.Tuoi14 + tk.PSThongKeTuoiMe.Tuoi15 + tk.PSThongKeTuoiMe.Tuoi16
                    + tk.PSThongKeTuoiMe.Tuoi17 + tk.PSThongKeTuoiMe.Tuoi13, tk.Tong));
                nhom2.ThongKe.Add(GetPDFCT("Từ 18 đến 35 tuổi", tk.PSThongKeTuoiMe.Tuoi17den20 + tk.PSThongKeTuoiMe.Tuoi20den25 + tk.PSThongKeTuoiMe.Tuoi25den30 + tk.PSThongKeTuoiMe.Tuoi30den35, tk.Tong));
                nhom2.ThongKe.Add(GetPDFCT("Trên 35 tuổi", tk.PSThongKeTuoiMe.Tuoi35den40 + tk.PSThongKeTuoiMe.Tuoi40den45 + tk.PSThongKeTuoiMe.TuoiTren45, tk.Tong));
                pdfdv.PSThongKePDFDonViNhom.Add(nhom2);
                PSThongKePDFDonViNhom nhom3 = new PSThongKePDFDonViNhom();
                nhom3.STT = 5;
                nhom3.TenNhom = "Sinh con thứ 3 trở lên (Dựa vào Para)";
                nhom3.ThongKe = new List<PSThongKePDFDonViCT>();
                nhom3.ThongKe.Add(GetPDFCT("Sinh con thứ 3", tk.PSTKCon.Sinh3Con, tk.Tong));
                nhom3.ThongKe.Add(GetPDFCT("Sinh con thứ 4", tk.PSTKCon.Sinh4Con, tk.Tong));
                nhom3.ThongKe.Add(GetPDFCT("Sinh con thứ 5 trở lên", tk.PSTKCon.SinhTu5Con, tk.Tong));
                pdfdv.PSThongKePDFDonViNhom.Add(nhom3);
                PSThongKePDFDonViNhom nhom4 = new PSThongKePDFDonViNhom();
                nhom4.STT = 6;
                nhom4.TenNhom = "Cân nặng trẻ (g)";
                nhom4.ThongKe = new List<PSThongKePDFDonViCT>();
                nhom4.ThongKe.Add(GetPDFCT("< 2500", tk.PSThongKeCanNang.Duoi25, tk.Tong));
                nhom4.ThongKe.Add(GetPDFCT("2500 ≤ X < 3000", tk.PSThongKeCanNang.Tu25Den30, tk.Tong));
                nhom4.ThongKe.Add(GetPDFCT("3000 ≤ X < 3500", tk.PSThongKeCanNang.Tu30Den35, tk.Tong));
                nhom4.ThongKe.Add(GetPDFCT("3500 ≤ X < 4000", tk.PSThongKeCanNang.Tu35Den40, tk.Tong));
                nhom4.ThongKe.Add(GetPDFCT("4000 ≤ X < 5000", tk.PSThongKeCanNang.Tu40Den45 + tk.PSThongKeCanNang.Tu45Den50, tk.Tong));
                nhom4.ThongKe.Add(GetPDFCT("≥ 5000", tk.PSThongKeCanNang.Tren50, tk.Tong));
                pdfdv.PSThongKePDFDonViNhom.Add(nhom4);
                PSThongKePDFDonViNhom nhom5 = new PSThongKePDFDonViNhom();
                nhom5.STT = 7;
                nhom5.TenNhom = "Gói xét nghiệm";
                nhom5.ThongKe = new List<PSThongKePDFDonViCT>();
                nhom5.ThongKe.Add(GetPDFCT("2 bệnh", tk.PSThongKeGoiBenh.Benh2, tk.Tong));
                nhom5.ThongKe.Add(GetPDFCT("3 bệnh", tk.PSThongKeGoiBenh.Benh3, tk.Tong));
                nhom5.ThongKe.Add(GetPDFCT("5 bệnh", tk.PSThongKeGoiBenh.Benh5, tk.Tong));
                nhom5.ThongKe.Add(GetPDFCT("2 bệnh + Hemo", tk.PSThongKeGoiBenh.Benh2Hemo, tk.Tong));
                nhom5.ThongKe.Add(GetPDFCT("3 bệnh + Hemo", tk.PSThongKeGoiBenh.Benh3Hemo, tk.Tong));
                nhom5.ThongKe.Add(GetPDFCT("5 bệnh + Hemo", tk.PSThongKeGoiBenh.Benh5Hemo, tk.Tong));
                pdfdv.PSThongKePDFDonViNhom.Add(nhom5);
                PSThongKePDFDonViNhom nhom6 = new PSThongKePDFDonViNhom();
                nhom6.STT = 8;
                nhom6.TenNhom = "Chương trình sàng lọc";
                nhom6.ThongKe = new List<PSThongKePDFDonViCT>();
                nhom6.ThongKe.Add(GetPDFCT("Quốc gia", tk.PSTKChuongTrinh.QuocGia, tk.Tong));
                nhom6.ThongKe.Add(GetPDFCT("Xã hội hóa", tk.PSTKChuongTrinh.XaHoi, tk.Tong));
                nhom6.ThongKe.Add(GetPDFCT("Demo", tk.PSTKChuongTrinh.Demo, tk.Tong));
                pdfdv.PSThongKePDFDonViNhom.Add(nhom6);

                Reports.RepostsBaoCao.rptBaoCaoCBTheoDonVi datarp = new Reports.RepostsBaoCao.rptBaoCaoCBTheoDonVi();
                datarp.DataSource = pdfdv;
                // string Link = BioNet_Bus.SaveFileReport("Temp", "Temp", tk.MaDV, DateTime.Now, DateTime.Now, ".pdf");
                datarp.ExportToPdf(Link);
                //Reports.frmReport rpt = new Reports.frmReport(datarp);
                //rpt.ShowDialog();
            }
            catch
            {
            }
        }
        private void XuatFile2(PSThongKeTheoDonVi tk, string Link, int i)
        {

            var p1 = ListBC.Where(y => y.IDCoSo.Equals(tk.MaDV)).ToList();
            var p2 = ListBC.Where(x => x.IDPhieu2 != null && x.IDCoSo.Equals(tk.MaDV)).ToList();
            var t1 = p1.Where(y => y.NguyCoCao1 == false).ToList();
            var t2 = p2.Where(y => y.NguyCoCao2 == false).ToList();
            var c1 = p1.Where(y => y.NguyCoCao1 == true).ToList();
            var c2 = p2.Where(y => y.NguyCoCao2 == true).ToList();
            PSThongKeTheoDonVi ph1 = new PSThongKeTheoDonVi();
            PSThongKeTheoDonVi ph2 = new PSThongKeTheoDonVi();
            PSThongKeTheoDonVi ph1thap = new PSThongKeTheoDonVi();
            PSThongKeTheoDonVi ph2thap = new PSThongKeTheoDonVi();
            PSThongKeTheoDonVi ph1cao = new PSThongKeTheoDonVi();
            PSThongKeTheoDonVi ph2cao = new PSThongKeTheoDonVi();
            PSThongKeTheoDonVi dvtk = new PSThongKeTheoDonVi();
            dvtk.MaDV = tk.MaDV;
            ph1 = BioNet_Bus.LoadDSThongKeDVNew(p1, dvtk, 1);
            dvtk = new PSThongKeTheoDonVi();
            dvtk.MaDV = tk.MaDV;
            ph2 = BioNet_Bus.LoadDSThongKeDVNew(p2, dvtk, 2);
            dvtk = new PSThongKeTheoDonVi();
            dvtk.MaDV = tk.MaDV;
            ph1thap = BioNet_Bus.LoadDSThongKeDVNew(t1, dvtk, 1);
            dvtk = new PSThongKeTheoDonVi();
            dvtk.MaDV = tk.MaDV;
            ph1cao = BioNet_Bus.LoadDSThongKeDVNew(c1, dvtk, 2);
            dvtk = new PSThongKeTheoDonVi();
            dvtk.MaDV = tk.MaDV;
            ph2thap = BioNet_Bus.LoadDSThongKeDVNew(t2, dvtk, 1);
            dvtk = new PSThongKeTheoDonVi();
            dvtk.MaDV = tk.MaDV;
            ph2cao = BioNet_Bus.LoadDSThongKeDVNew(c2, dvtk, 2);

            PSThongKePDFDonViXNNhom nhom = new PSThongKePDFDonViXNNhom();
            nhom.STT = 1;
            nhom.TenNhom = "Cân nặng trẻ (g)";
            nhom.NguyCoThap1 = t1.Count().ToString();
            nhom.NguyCoThap2 = t2.Count().ToString();
            nhom.NguyCoCao1 = c1.Count().ToString();
            nhom.NguyCoCao2 = c2.Count().ToString();
            nhom.Tong1 = p1.Count().ToString();
            nhom.Tong2 = p2.Count().ToString();
            nhom.ThongKe = new List<PSThongKePDFDonViXNCT>();
            nhom.ThongKe.Add(GetCTThongKe("< 2500", ph1thap.PSThongKeCanNang.Duoi25, ph1cao.PSThongKeCanNang.Duoi25, ph1.PSThongKeCanNang.Duoi25, ph2thap.PSThongKeCanNang.Duoi25, ph2cao.PSThongKeCanNang.Duoi25, ph2.PSThongKeCanNang.Duoi25));
            nhom.ThongKe.Add(GetCTThongKe("2500 ≤ X < 3000", ph1thap.PSThongKeCanNang.Tu25Den30, ph1cao.PSThongKeCanNang.Tu25Den30, ph1.PSThongKeCanNang.Tu25Den30, ph2thap.PSThongKeCanNang.Tu25Den30, ph2cao.PSThongKeCanNang.Tu25Den30, ph2.PSThongKeCanNang.Tu25Den30));
            nhom.ThongKe.Add(GetCTThongKe("3000 ≤ X < 3500", ph1thap.PSThongKeCanNang.Tu30Den35, ph1cao.PSThongKeCanNang.Tu30Den35, ph1.PSThongKeCanNang.Tu30Den35, ph2thap.PSThongKeCanNang.Tu30Den35, ph2cao.PSThongKeCanNang.Tu30Den35, ph2.PSThongKeCanNang.Tu30Den35));
            nhom.ThongKe.Add(GetCTThongKe("3500 ≤ X < 4000", ph1thap.PSThongKeCanNang.Tu35Den40, ph1cao.PSThongKeCanNang.Tu35Den40, ph1.PSThongKeCanNang.Tu35Den40, ph2thap.PSThongKeCanNang.Tu35Den40, ph2cao.PSThongKeCanNang.Tu35Den40, ph2.PSThongKeCanNang.Tu35Den40));
            nhom.ThongKe.Add(GetCTThongKe("4000 ≤ X < 4500", ph1thap.PSThongKeCanNang.Tu40Den45, ph1cao.PSThongKeCanNang.Tu40Den45, ph1.PSThongKeCanNang.Tu40Den45, ph2thap.PSThongKeCanNang.Tu40Den45, ph2cao.PSThongKeCanNang.Tu40Den45, ph2.PSThongKeCanNang.Tu40Den45));
            nhom.ThongKe.Add(GetCTThongKe("4500 ≤ X < 5000", ph1thap.PSThongKeCanNang.Tu45Den50, ph1cao.PSThongKeCanNang.Tu45Den50, ph1.PSThongKeCanNang.Tu45Den50, ph2thap.PSThongKeCanNang.Tu45Den50, ph2cao.PSThongKeCanNang.Tu45Den50, ph2.PSThongKeCanNang.Tu45Den50));
            nhom.ThongKe.Add(GetCTThongKe("≥ 5000", ph1thap.PSThongKeCanNang.Tren50, ph1cao.PSThongKeCanNang.Tren50, ph1.PSThongKeCanNang.Tren50, ph2thap.PSThongKeCanNang.Tren50, ph2cao.PSThongKeCanNang.Tren50, ph2.PSThongKeCanNang.Tren50));

            PSThongKePDFDonViXNNhom nhom2 = new PSThongKePDFDonViXNNhom();
            nhom2.STT = 2;
            nhom2.TenNhom = "Tuổi mẹ";
            nhom2.NguyCoThap1 = t1.Count().ToString();
            nhom2.NguyCoThap2 = t2.Count().ToString();
            nhom2.NguyCoCao1 = c1.Count().ToString();
            nhom2.NguyCoCao2 = c2.Count().ToString();
            nhom2.Tong1 = p1.Count().ToString();
            nhom2.Tong2 = p2.Count().ToString();
            nhom2.ThongKe = new List<PSThongKePDFDonViXNCT>();
            nhom2.ThongKe.Add(GetCTThongKe("N/A", ph1thap.PSThongKeTuoiMe.Duoi13, ph1cao.PSThongKeTuoiMe.Duoi13, ph1.PSThongKeTuoiMe.Duoi13, ph2thap.PSThongKeTuoiMe.Duoi13, ph2cao.PSThongKeTuoiMe.Duoi13, ph2.PSThongKeTuoiMe.Duoi13));
            nhom2.ThongKe.Add(GetCTThongKe("13", ph1thap.PSThongKeTuoiMe.Tuoi13, ph1cao.PSThongKeTuoiMe.Tuoi13, ph1.PSThongKeTuoiMe.Tuoi13, ph2thap.PSThongKeTuoiMe.Tuoi13, ph2cao.PSThongKeTuoiMe.Tuoi13, ph2.PSThongKeTuoiMe.Tuoi13));
            nhom2.ThongKe.Add(GetCTThongKe("14", ph1thap.PSThongKeTuoiMe.Tuoi14, ph1cao.PSThongKeTuoiMe.Tuoi14, ph1.PSThongKeTuoiMe.Tuoi14, ph2thap.PSThongKeTuoiMe.Tuoi14, ph2cao.PSThongKeTuoiMe.Tuoi14, ph2.PSThongKeTuoiMe.Tuoi14));
            nhom2.ThongKe.Add(GetCTThongKe("15", ph1thap.PSThongKeTuoiMe.Tuoi15, ph1cao.PSThongKeTuoiMe.Tuoi15, ph1.PSThongKeTuoiMe.Tuoi15, ph2thap.PSThongKeTuoiMe.Tuoi15, ph2cao.PSThongKeTuoiMe.Tuoi15, ph2.PSThongKeTuoiMe.Tuoi15));
            nhom2.ThongKe.Add(GetCTThongKe("16", ph1thap.PSThongKeTuoiMe.Tuoi16, ph1cao.PSThongKeTuoiMe.Tuoi16, ph1.PSThongKeTuoiMe.Tuoi16, ph2thap.PSThongKeTuoiMe.Tuoi16, ph2cao.PSThongKeTuoiMe.Tuoi16, ph2.PSThongKeTuoiMe.Tuoi16));
            nhom2.ThongKe.Add(GetCTThongKe("17", ph1thap.PSThongKeTuoiMe.Tuoi17, ph1cao.PSThongKeTuoiMe.Tuoi17, ph1.PSThongKeTuoiMe.Tuoi17, ph2thap.PSThongKeTuoiMe.Tuoi17, ph2cao.PSThongKeTuoiMe.Tuoi17, ph2.PSThongKeTuoiMe.Tuoi17));
            nhom2.ThongKe.Add(GetCTThongKe("18 ≤ X < 20", ph1thap.PSThongKeTuoiMe.Tuoi17den20, ph1cao.PSThongKeTuoiMe.Tuoi17den20, ph1.PSThongKeTuoiMe.Tuoi17den20, ph2thap.PSThongKeTuoiMe.Tuoi17den20, ph2cao.PSThongKeTuoiMe.Tuoi17den20, ph2.PSThongKeTuoiMe.Tuoi17den20));
            nhom2.ThongKe.Add(GetCTThongKe("20 ≤ X < 25", ph1thap.PSThongKeTuoiMe.Tuoi20den25, ph1cao.PSThongKeTuoiMe.Tuoi20den25, ph1.PSThongKeTuoiMe.Tuoi20den25, ph2thap.PSThongKeTuoiMe.Tuoi20den25, ph2cao.PSThongKeTuoiMe.Tuoi20den25, ph2.PSThongKeTuoiMe.Tuoi20den25));
            nhom2.ThongKe.Add(GetCTThongKe("25 ≤ X < 30", ph1thap.PSThongKeTuoiMe.Tuoi25den30, ph1cao.PSThongKeTuoiMe.Tuoi25den30, ph1.PSThongKeTuoiMe.Tuoi25den30, ph2thap.PSThongKeTuoiMe.Tuoi25den30, ph2cao.PSThongKeTuoiMe.Tuoi25den30, ph2.PSThongKeTuoiMe.Tuoi25den30));
            nhom2.ThongKe.Add(GetCTThongKe("30 ≤ X <35", ph1thap.PSThongKeTuoiMe.Tuoi30den35, ph1cao.PSThongKeTuoiMe.Tuoi30den35, ph1.PSThongKeTuoiMe.Tuoi30den35, ph2thap.PSThongKeTuoiMe.Tuoi30den35, ph2cao.PSThongKeTuoiMe.Tuoi30den35, ph2.PSThongKeTuoiMe.Tuoi30den35));
            nhom2.ThongKe.Add(GetCTThongKe("35 ≤ X < 40", ph1thap.PSThongKeTuoiMe.Tuoi35den40, ph1cao.PSThongKeTuoiMe.Tuoi35den40, ph1.PSThongKeTuoiMe.Tuoi35den40, ph2thap.PSThongKeTuoiMe.Tuoi35den40, ph2cao.PSThongKeTuoiMe.Tuoi35den40, ph2.PSThongKeTuoiMe.Tuoi35den40));
            nhom2.ThongKe.Add(GetCTThongKe("40 ≤ X<45", ph1thap.PSThongKeTuoiMe.Tuoi40den45, ph1cao.PSThongKeTuoiMe.Tuoi40den45, ph1.PSThongKeTuoiMe.Tuoi40den45, ph2thap.PSThongKeTuoiMe.Tuoi40den45, ph2cao.PSThongKeTuoiMe.Tuoi40den45, ph2.PSThongKeTuoiMe.Tuoi40den45));
            nhom2.ThongKe.Add(GetCTThongKe("≥ 45", ph1thap.PSThongKeTuoiMe.TuoiTren45, ph1cao.PSThongKeTuoiMe.TuoiTren45, ph1.PSThongKeTuoiMe.TuoiTren45, ph2thap.PSThongKeTuoiMe.TuoiTren45, ph2cao.PSThongKeTuoiMe.TuoiTren45, ph2.PSThongKeTuoiMe.TuoiTren45));

            PSThongKePDFDonViXNNhom nhom3 = new PSThongKePDFDonViXNNhom();
            nhom3.STT = 3;
            nhom3.TenNhom = "Dân tộc";
            nhom3.NguyCoThap1 = t1.Count().ToString();
            nhom3.NguyCoThap2 = t2.Count().ToString();
            nhom3.NguyCoCao1 = c1.Count().ToString();
            nhom3.NguyCoCao2 = c2.Count().ToString();
            nhom3.Tong1 = p1.Count().ToString();
            nhom3.Tong2 = p2.Count().ToString();
            nhom3.ThongKe = new List<PSThongKePDFDonViXNCT>();
            var lsdt = t1.Select(x => x.DanTocID).Distinct();
            List<PSThongKePDFDonViXNCT> lstTam = new List<PSThongKePDFDonViXNCT>();
            foreach (var dt in lsdt)
            {
                lstTam.Add(GetCTThongKeDanToc(dt.Value, t1, c1, p1, t2, c2, p2));
            }
            nhom3.ThongKe.AddRange(lstTam.OrderByDescending(x => int.Parse(x.Tong1)));
            //PSThongKePDFDonViXNNhom nhom4 = new PSThongKePDFDonViXNNhom();
            //nhom4.STT = 4;
            //nhom4.TenNhom = "Kết quả sàng lọc sơ sinh";
            //nhom4.NguyCoThap1 = t1.Count().ToString();
            //nhom4.NguyCoThap2 = t2.Count().ToString();
            //nhom4.NguyCoCao1 = c1.Count().ToString();
            //nhom4.NguyCoCao2 = c2.Count().ToString();
            //nhom4.Tong1 = p1.Count().ToString();
            //nhom4.Tong2 = p2.Count().ToString();
            //nhom4.ThongKe = new List<PSThongKePDFDonViXNCT>();
            //foreach (var dt in BioNet_Bus.GetDanhSachDichVu(false))
            //{
            //    nhom4.ThongKe.Add(GetCTThongKeKQ(dt.IDDichVu,dt.TenDichVu, t1, c1, p1, t2, c2, p2));
            //}
            PSThongKePDFDonViXN tkdv = new PSThongKePDFDonViXN();
            tkdv.DonVi = BioNet_Bus.GetThongTinDonViCoSo(tk.MaDV).TenDVCS;
            tkdv.ThoiGian = "Từ ngày " + dllNgay.tungay.Value.Date.ToShortDateString() + " đến " + dllNgay.denngay.Value.Date.ToShortDateString();
            tkdv.LuuY = "(Lưu ý: Báo cáo thống kê có giá trị tại thời điểm xuất báo cáo ngày " + DateTime.Now.Date.ToShortDateString() + ")";
            tkdv.PSThongKePDFDonViNhom = new List<PSThongKePDFDonViXNNhom>();
            tkdv.PSThongKePDFDonViNhom.Add(nhom);
            tkdv.PSThongKePDFDonViNhom.Add(nhom2);
            tkdv.PSThongKePDFDonViNhom.Add(nhom3);
            Reports.RepostsBaoCao.rptBaoCaoNhomDonViTheoKQXN rptdv = new Reports.RepostsBaoCao.rptBaoCaoNhomDonViTheoKQXN(i);
            rptdv.DataSource = tkdv;
            //string Link = BioNet_Bus.SaveFileReport("Temp", "Temp", tk.MaDV, DateTime.Now, DateTime.Now, ".pdf");
            rptdv.ExportToPdf(Link);
            //Reports.frmReport reponse = new Reports.frmReport(rptdv);
            //reponse.ShowDialog();


        }
        private void XuatFile3(List<pro_Report_BaoCaoTongHopTheoBenhNhanResult> tk, string MaDV, string Link, int i)
        {
            Reports.RepostsBaoCao.rptBaoCaoDonViTKCoBan bv = new Reports.RepostsBaoCao.rptBaoCaoDonViTKCoBan(i);
            bv.DataSource = BioNet_Bus.GetBaoCaoDonViXNCoBan(tk, MaDV, dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date);
            bv.ExportToPdf(Link);
        }
        public void GopPDF()
        {
            try
            {
                string Folder = BioNet_Bus.GetFileReport("DonVi", "PDF");
                //File1
                SplashScreenManager.ShowForm(typeof(WaitingLoadData), true, false);
                foreach (var tk in lstTK)
                {
                    string TenDonVi = BioNet_Bus.GetThongTinDonViCoSo(tk.MaDV).TenDVCS;

                    string Link = BioNet_Bus.SaveFileReport(Folder, "DonVi", TenDonVi, dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date, ".pdf");
                    Document document = new Document();
                    PdfCopy writer = new PdfCopy(document, new FileStream(Link, FileMode.Create));
                    if (writer == null)
                    {
                        return;
                    }
                    document.Open();
                    string fileName = BioNet_Bus.SaveFileTemp("DonVi", TenDonVi, dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date, "1.pdf");
                    this.XuatFile1(tk, fileName);
                    PdfReader reader = new PdfReader(fileName);
                    reader.ConsolidateNamedDestinations();
                    int str = 1;
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                        str = str + 1;
                    }
                    PRAcroForm form = reader.AcroForm;
                    if (form != null)
                    {
                        writer.CopyDocumentFields(reader);
                    }
                    reader.Close();
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                    //File2
                    string fileName2 = BioNet_Bus.SaveFileTemp("DonVi", TenDonVi, dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date, "2.pdf");
                    this.XuatFile2(tk, fileName2, str);
                    PdfReader reader2 = new PdfReader(fileName2);
                    reader2.ConsolidateNamedDestinations();
                    for (int i = 1; i <= reader2.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader2, i);
                        writer.AddPage(page);
                        str = str + 1;
                    }
                    PRAcroForm form2 = reader2.AcroForm;
                    if (form2 != null)
                    {
                        writer.CopyDocumentFields(reader2);
                    }
                    reader2.Close();
                    if (File.Exists(fileName2))
                    {
                        File.Delete(fileName2);
                    }
                    //File 3
                    string fileName3 = BioNet_Bus.SaveFileTemp("DonVi", TenDonVi, dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date, "3.pdf");
                    List<pro_Report_BaoCaoTongHopTheoBenhNhanResult> listdv = ListBC.Where(x => x.IDCoSo.Equals(tk.MaDV)).ToList();
                    XuatFile3(listdv, tk.MaDV, fileName3, str);
                    PdfReader reader3 = new PdfReader(fileName3);
                    reader3.ConsolidateNamedDestinations();
                    for (int i = 1; i <= reader3.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader3, i);
                        writer.AddPage(page);
                        str = str + 1;
                    }
                    PRAcroForm form3 = reader3.AcroForm;
                    if (form3 != null)
                    {
                        writer.CopyDocumentFields(reader3);
                    }
                    reader3.Close();
                    if (File.Exists(fileName3))
                    {
                        File.Delete(fileName3);
                    }

                    writer.Close();
                    document.Close();
                }
                SplashScreenManager.CloseForm();
                XtraMessageBox.Show("Xuất file báo cáo thành công", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(Folder);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Xuất file báo cáo thất bại", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SplashScreenManager.CloseForm();
            }

        }
        private PSThongKePDFDonViCT GetPDFCT(string Tk, int SL, int Tong)
        {
            PSThongKePDFDonViCT ct = new PSThongKePDFDonViCT();
            ct.TenThongKe = Tk;
            ct.SoLuong = SL.ToString();
            ct.Tile = String.Format("{0:0.00}", ((double)SL / (double)Tong) * 100) + "%";
            return ct;
        }



        private PSThongKePDFDonViXNCT GetCTThongKe(string name, int thap1, int cao1, int p1, int thap2, int cao2, int p2)
        {
            PSThongKePDFDonViXNCT ct = new PSThongKePDFDonViXNCT();
            ct.TenThongKe = name;
            ct.NguyCoThap1 = thap1.ToString();
            ct.NguyCoThap2 = thap2.ToString();
            ct.Tong1 = p1.ToString();
            ct.NguyCoCao1 = cao1.ToString();
            ct.NguyCoCao2 = cao2.ToString();
            ct.Tong2 = p2.ToString();
            return ct;
        }

        private PSThongKePDFDonViXNCT GetCTThongKeDanToc(int name, List<pro_Report_BaoCaoTongHopTheoBenhNhanResult> thap1, List<pro_Report_BaoCaoTongHopTheoBenhNhanResult> cao1, List<pro_Report_BaoCaoTongHopTheoBenhNhanResult> p1, List<pro_Report_BaoCaoTongHopTheoBenhNhanResult> thap2, List<pro_Report_BaoCaoTongHopTheoBenhNhanResult> cao2, List<pro_Report_BaoCaoTongHopTheoBenhNhanResult> p2)
        {
            PSThongKePDFDonViXNCT ct = new PSThongKePDFDonViXNCT();
            ct.TenThongKe = BioNet_Bus.GetTenDanToc(name);
            ct.NguyCoThap1 = thap1.Where(x => x.DanTocID.Equals(name)).Count().ToString();
            ct.NguyCoThap2 = thap2.Where(x => x.DanTocID.Equals(name)).Count().ToString();
            ct.Tong1 = p1.Where(x => x.DanTocID.Equals(name)).Count().ToString();
            ct.NguyCoCao1 = cao1.Where(x => x.DanTocID.Equals(name)).Count().ToString();
            ct.NguyCoCao2 = cao2.Where(x => x.DanTocID.Equals(name)).Count().ToString();
            ct.Tong2 = p2.Where(x => x.DanTocID.Equals(name)).Count().ToString();
            return ct;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            string Folder = BioNet_Bus.GetFileReport("ChiCuc", "PDF");
            SplashScreenManager.ShowForm(typeof(WaitingLoadData), true, false);
            int TT = 1;
            foreach (var cc in listChiCuc.Where(x=>x.MaChiCuc!="all"))
            {
                PSThongKeTheoDonVi tk = new PSThongKeTheoDonVi();
                    string TenDonVi = cc.TenChiCuc;
                tk=BioNet_Bus.LoadDSThongKeChiCucTNew(ListBC, cc.MaChiCuc, TT++);
                    string Link = BioNet_Bus.SaveFileReport(Folder, "ChiCuc", TenDonVi, dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date, ".pdf");
                    Document document = new Document();
                    PdfCopy writer = new PdfCopy(document, new FileStream(Link, FileMode.Create));
                    if (writer == null)
                    {
                        return;
                    }
                    document.Open();
                    string fileName = BioNet_Bus.SaveFileTemp("ChiCuc", TenDonVi, dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date, "1.pdf");
                    this.XuatFile1(tk, fileName);
                    PdfReader reader = new PdfReader(fileName);
                    reader.ConsolidateNamedDestinations();
                    int str = 1;
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                        str = str + 1;
                    }
                    PRAcroForm form = reader.AcroForm;
                    if (form != null)
                    {
                        writer.CopyDocumentFields(reader);
                    }
                    reader.Close();
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                    //File2
                    string fileName2 = BioNet_Bus.SaveFileTemp("ChiCuc", TenDonVi, dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date, "2.pdf");
                    this.XuatFile2(tk, fileName2, str);
                    PdfReader reader2 = new PdfReader(fileName2);
                    reader2.ConsolidateNamedDestinations();
                    for (int i = 1; i <= reader2.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader2, i);
                        writer.AddPage(page);
                        str = str + 1;
                    }
                    PRAcroForm form2 = reader2.AcroForm;
                    if (form2 != null)
                    {
                        writer.CopyDocumentFields(reader2);
                    }
                    reader2.Close();
                    if (File.Exists(fileName2))
                    {
                        File.Delete(fileName2);
                    }
                    //File 3
                    string fileName3 = BioNet_Bus.SaveFileTemp("ChiCuc", TenDonVi, dllNgay.tungay.Value.Date, dllNgay.denngay.Value.Date, "3.pdf");
                    List<pro_Report_BaoCaoTongHopTheoBenhNhanResult> listdv = ListBC.Where(x => x.IDCoSo.Equals(tk.MaDV)).ToList();
                    XuatFile3(listdv, tk.MaDV, fileName3, str);
                    PdfReader reader3 = new PdfReader(fileName3);
                    reader3.ConsolidateNamedDestinations();
                    for (int i = 1; i <= reader3.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader3, i);
                        writer.AddPage(page);
                        str = str + 1;
                    }
                    PRAcroForm form3 = reader3.AcroForm;
                    if (form3 != null)
                    {
                        writer.CopyDocumentFields(reader3);
                    }
                    reader3.Close();
                    if (File.Exists(fileName3))
                    {
                        File.Delete(fileName3);
                    }

                    writer.Close();
                    document.Close();
                }
             SplashScreenManager.CloseForm();
             XtraMessageBox.Show("Xuất file báo cáo thành công", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
             Process.Start(Folder);
        }
    }
       
}