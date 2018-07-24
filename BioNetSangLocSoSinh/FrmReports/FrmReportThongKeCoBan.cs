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
using DevExpress.XtraCharts;
using BioNetModel;
using BioNetBLL;
using DevExpress.XtraSplashScreen;
using BioNetSangLocSoSinh.DiaglogFrm;
using System.IO;
using BioNetModel.Data;

namespace BioNetSangLocSoSinh.FrmReports
{
    public partial class FrmReportThongKeCoBan : DevExpress.XtraEditors.XtraForm
    {
        public FrmReportThongKeCoBan()
        {
            InitializeComponent();
        }
        TTPhieuCB dataRessult = new TTPhieuCB();
        public class Report {
            public string TenNhom { get; set; }
            public string TenChiTiet { get; set; }
            public int GiaTriChiTiet { get; set; }

        }
        private void FrmReportTrungTam_DonVi_Load(object sender, EventArgs e)
        {
            this.txtChiCuc.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            this.txtDonVi.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi("all");
            this.txtDonVi.EditValue = "all";
            this.txtChiCuc.EditValue = "all";
            this.LoadDuLieu();
            AddItemForm();
        }
        private void LoadDuLieu()
        {
            string MaDonVi = String.Empty;
           
            if (this.txtDonVi.EditValue.ToString() == "all")
            {
                if (this.txtChiCuc.EditValue.ToString() == "all")
                {
                    this.lblTenDonVi.Text = "Thống kê toàn bộ trung tâm";
                    MaDonVi ="all";
                }
                else
                {
                    this.lblTenDonVi.Text = "Thống kê chi cục "+ this.txtChiCuc.Text.ToString();
                    MaDonVi= this.txtChiCuc.EditValue.ToString();
                }
            }
            else
            {
                this.lblTenDonVi.Text = "Thống kê đơn vị " + this.txtDonVi.Text.ToString();
                MaDonVi = this.txtDonVi.EditValue.ToString();
            }
            SplashScreenManager.ShowForm(this, typeof(WaitingLoadData), true, true, false);
            this.dataRessult = BioNetBLL.BioNet_Bus.GetBaoCaoThongTinPhieuTheoTime(MaDonVi, this.dllNgay.tungay.Value.Date, this.dllNgay.denngay.Value.Date);
            TimeSpan ngg;
            if (dllNgay.denngay.Value.Date>DateTime.Now)
            {
                ngg= DateTime.Now - this.dllNgay.tungay.Value.Date;
            }
            else
                ngg = this.dllNgay.denngay.Value.Date - this.dllNgay.tungay.Value.Date;
            int ngay = ngg.Days + 1;
            int tbNgay = Convert.ToInt16(Convert.ToDecimal( dataRessult.TongSoPhieu / ngay));
            int tbThang = tbNgay * 30;
            int tbNam = tbNgay * 365;
            lblUocTinh.Text=tbNgay.ToString() +" phiếu/ngày. " + tbThang.ToString() + " phiếu/tháng. "+tbNam.ToString() + " phiếu/năm.";
            this.LoadChuongTrinh();
            this.LoadThongKeCLMau();
            this.LoadThongKeDGMau();
            this.LoadThongKeGioiTinh();
            this.LoadThongKeGoiXN();
            this.LoadThongKePhieu();
            this.LoadThongKePPSinh();
            this.LoadThongKeThang();
            SplashScreenManager.CloseForm();

        }
        private void LoadThongKeThang()
        {
            Series SLPhieu = new Series("Số lượng phiếu", ViewType.Line);
            if(dataRessult.slphieu!=null)
            {
                foreach (var tkphieu in dataRessult.slphieu)
                {
                    SLPhieu.Points.Add(new SeriesPoint("T" + tkphieu.Thang, tkphieu.SLphieu));
                }
                SLPhieu.Label.TextPattern = "{V:#,#}";
                SLPhieu.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                this.chartThongKePhieu.Series.Clear();
                this.chartThongKePhieu.Series.Add(SLPhieu);
                if (chartThongKePhieu.Series[0].View is LineSeriesView)
                {
                    (chartThongKePhieu.Series[0].View as LineSeriesView).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                    (chartThongKePhieu.Series[0].View as LineSeriesView).Color = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                    (chartThongKePhieu.Series[0].View as LineSeriesView).LineMarkerOptions.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
                }
            }
            
        }

        private void LoadThongKeGioiTinh()
        {
            Series GioiTinh = new Series("Tỉ lệ Nam Nữ", ViewType.StackedBar);
            GioiTinh.Points.Add(new SeriesPoint("Nam", dataRessult.Nam));
            GioiTinh.Points.Add(new SeriesPoint("Nữ", dataRessult.Nu));
            GioiTinh.Points.Add(new SeriesPoint("N/A", dataRessult.GTKhac));
            this.chartGioiTinh.Series.Clear();
            this.chartGioiTinh.Titles.Clear();
            this.chartGioiTinh.Series.Add(GioiTinh);
            (chartGioiTinh.Series[0].View as StackedBarSeriesView).Color = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(172)))), ((int)(((byte)(198)))));
            chartGioiTinh.Titles.Add(new ChartTitle());
            chartGioiTinh.Titles[0].Text = "Tỉ lệ Nam/Nữ =" + (float)dataRessult.Nam / dataRessult.Nu;
            ((XYDiagram)chartGioiTinh.Diagram).Rotated = true;
        }

        private void LoadThongKePPSinh()
        {
            List<ObjectChartReport> lstPPS = new List<ObjectChartReport>();
            ObjectChartReport PPS = new ObjectChartReport { Name = "Sinh thường", Values = this.dataRessult.PPSinhThuong  };
            lstPPS.Add(PPS);
            PPS = new ObjectChartReport { Name = "Sinh mổ", Values = this.dataRessult.PPSinhMo };
            lstPPS.Add(PPS);
            PPS = new ObjectChartReport { Name = "N/A", Values = this.dataRessult.PPSinhKhac};
            lstPPS.Add(PPS);
            this.chartPPSinh.DataSource = lstPPS;
        }

        private void LoadThongKeCLMau()
        {
            List<ObjectChartReport> lstCLM = new List<ObjectChartReport>();           
            ObjectChartReport CLM = new ObjectChartReport { Name = "Đạt", Values = this.dataRessult.MauDat };
            lstCLM.Add(CLM);
            CLM = new ObjectChartReport { Name = "Không Đạt", Values = this.dataRessult.MauKoDat };
            lstCLM.Add(CLM);
            this.chartCLMau.DataSource = lstCLM;
        }

        private void LoadThongKeGoiXN()
        {
            List<ObjectChartReport> lstGoiXN = new List<ObjectChartReport>();
            if(dataRessult.thongkebenh!=null)
            {
                foreach (var tkbenh in dataRessult.thongkebenh)
                {
                    ObjectChartReport GoiXN = new ObjectChartReport { Name = tkbenh.TenThongKe, Values = tkbenh.SoLuong };
                    lstGoiXN.Add(GoiXN);
                }
                this.chartGoiXN.DataSource = lstGoiXN;
            }
           
        }

        private void LoadThongKeDGMau()
        {
            List<ObjectChartReport> lstCTCLMau = new List<ObjectChartReport>();
            if(dataRessult.thongkeDGMau!=null)
            {
                foreach (var tkdgmau in dataRessult.thongkeDGMau)
                {
                    ObjectChartReport CTCLmau = new ObjectChartReport { Name = tkdgmau.TenThongKe, Values = tkdgmau.SoLuong };
                    lstCTCLMau.Add(CTCLmau);
                }
                this.chartCTCLMau.DataSource = lstCTCLMau;
            }           
        }

        private void LoadChuongTrinh()
        {
            List<ObjectChartReport> lstChuongTrinh = new List<ObjectChartReport>();
            if(dataRessult.thongkeCTrinh!=null)
            {
                foreach (var tkctrinh in dataRessult.thongkeCTrinh)
                {
                    ObjectChartReport ChuongTrinh = new ObjectChartReport { Name = tkctrinh.TenThongKe, Values = tkctrinh.SoLuong };
                    lstChuongTrinh.Add(ChuongTrinh);
                }
                this.chartChuongTrinh.DataSource = lstChuongTrinh;
            }
           
        }

        private void LoadThongKePhieu()
        {
            Series Phieu = new Series("Tổng số phiếu: " + dataRessult.TongSoPhieu, ViewType.StackedBar);
            Phieu.Points.Add(new SeriesPoint("Phiếu mới", dataRessult.PhieuThuMoi));
            Phieu.Points.Add(new SeriesPoint("Phiếu thu lại", dataRessult.PhieuThuLai));
            Phieu.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            this.chartPhieu.Series.Clear();
            this.chartPhieu.Titles.Clear();
            this.chartPhieu.Series.Add(Phieu);
           this.chartPhieu.Titles.Add(new ChartTitle());
            this.chartPhieu.Titles[0].Text = "Tổng số phiếu:" + dataRessult.TongSoPhieu;
        }

        private void txtChiCuc_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit sear = sender as SearchLookUpEdit;
                var value = sear.EditValue.ToString();
                this.txtDonVi.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi(value.ToString());
                this.txtDonVi.EditValue = "all";
            }
            catch { }
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            this.LoadDuLieu();
        }

        private void butPrint_Click(object sender, EventArgs e)
        {           
            try
            {
                var tt = BioNet_Bus.GetThongTinTrungTam();
                byte[] b = tt.Header.ToArray();
                MemoryStream ms = new MemoryStream(b);
                Image img = Image.FromStream(ms);
                rptBaoCaoThongKeCoBan baocao = new rptBaoCaoThongKeCoBan();
                if (dataRessult!=null)
                {
                    Reports.rptBaoCaoThongKeCoBan rp = new Reports.rptBaoCaoThongKeCoBan(img);
                    rp.Parameters["TongSoPhieu"].Value = dataRessult.TongSoPhieu;
                    rp.Parameters["PhieuThuMoi"].Value = dataRessult.PhieuThuMoi;
                    rp.Parameters["PhieuThuLai"].Value = dataRessult.PhieuThuLai;
                    rp.Parameters["Nam"].Value = dataRessult.Nam;
                    rp.Parameters["Nu"].Value = dataRessult.Nu;
                    rp.Parameters["TiLeNamNu"].Value= Math.Round((float)dataRessult.Nam/dataRessult.Nu,2);
                    rp.Parameters["GTKhac"].Value = dataRessult.GTKhac;
                    rp.Parameters["PPSinhThuong"].Value = dataRessult.PPSinhThuong;
                    rp.Parameters["PPSinhMo"].Value = dataRessult.PPSinhMo;
                    rp.Parameters["PPSinhKhac"].Value = dataRessult.PPSinhKhac;
                    rp.Parameters["TiLeSinhMoSinhThuong"].Value = Math.Round((float)dataRessult.PPSinhMo/dataRessult.PPSinhThuong,2);
                    rp.Parameters["SoMauCanThuLai"].Value= dataRessult.SoMauCanThuLai;
                    rp.Parameters["SoMauDaThuLai"].Value= dataRessult.SoMauDaThuLai;
                    rp.Parameters["SoMauChuaThuLai"].Value = dataRessult.SoMauChuaThuLai;
                    foreach(var benh in dataRessult.thongkebenh)
                    {
                        if(benh.TenThongKe.Equals("2 Bệnh"))
                        {
                            rp.Parameters["benh2"].Value = benh.SoLuong;
                        }
                        else if (benh.TenThongKe.Equals("3 Bệnh"))
                        {
                            rp.Parameters["benh3"].Value = benh.SoLuong;
                        }
                        else if (benh.TenThongKe.Equals("5 Bệnh"))
                        {
                            rp.Parameters["benh5"].Value = benh.SoLuong;
                        }
                    }
                    foreach (var ctrinh in dataRessult.thongkeCTrinh)
                    {
                        if (ctrinh.TenThongKe.Equals("Chương trình quốc gia"))
                        {
                            rp.Parameters["CTQuocGia"].Value = ctrinh.SoLuong;
                        }
                        else if (ctrinh.TenThongKe.Equals("Chương trình xã hội hóa"))
                        {
                            rp.Parameters["CTXaHoi"].Value = ctrinh.SoLuong;
                        }                       
                    }
                    int temt1 = 0,temt2=0,temt3=0;
                    foreach(var dgmau in dataRessult.thongkeDGMau)
                    {
                        if(dgmau.TenThongKe.Equals("Giọt máu chồng lên nhau"))
                        {
                            rp.Parameters["GiotMauChongLenNhau"].Value = dgmau.SoLuong;
                        }
                        else if (dgmau.TenThongKe.Equals("Mẫu ít"))
                        {                            
                            temt1 = dgmau.SoLuong; ;
                        }
                        else if (dgmau.TenThongKe.Equals("Không thấm đều 2 mặt"))
                        {
                            temt2 = dgmau.SoLuong;
                        } 
                        else if (dgmau.TenThongKe.Equals("Thời gian gửi mẫu muộn"))
                        {
                            rp.Parameters["ThoiGianGuiMauMuon"].Value = dgmau.SoLuong;
                        }
                        else if (dgmau.TenThongKe.Equals("Trẻ sinh non hoặc nhẹ cân"))
                        {
                            rp.Parameters["TreSinhNonHoacNheCan"].Value = dgmau.SoLuong;
                        }
                        else if (dgmau.TenThongKe.Equals("Thu mẫu sớm (trước 24h tuổi)"))
                        {
                            rp.Parameters["ThuMauSom"].Value = dgmau.SoLuong;
                        }
                        else
                        {
                            temt3 = temt3!=0?dgmau.SoLuong:temt3+dgmau.SoLuong;
                        }
                    }
                    rp.Parameters["Mauithoackhongthamdeu2mat"].Value = temt1 + temt2;
                    rp.Parameters["LyDoKhac"].Value = temt3;
                    rp.Parameters["TileSinhConThu3"].Value = dataRessult.TyleSinhConThu3;
                    rp.Parameters["MauDat"].Value = dataRessult.MauDat;
                    rp.Parameters["MauKoDat"].Value = dataRessult.MauKoDat;
                    rp.Parameters["TuoiMeDuoi18"].Value = dataRessult.TuoiMeDuoi18;
                    rp.Parameters["TuoiMeTren35"].Value = dataRessult.TuoiMeTren35;
                    rp.Parameters["TuoiMeTu18den35"].Value = dataRessult.TuoiMeTu18den35;
                    rp.Parameters["NCC1G6PD"].Value = dataRessult.baocaocobanct.NCC1G6PD;
                    rp.Parameters["NCC1CAH"].Value = dataRessult.baocaocobanct.NCC1CAH;
                    rp.Parameters["NCC1CH"].Value = dataRessult.baocaocobanct.NCC1CH;
                    rp.Parameters["NCC1GAL"].Value = dataRessult.baocaocobanct.NCC1GAL;
                    rp.Parameters["NCC1PKU"].Value = dataRessult.baocaocobanct.NCC1PKU;
                    rp.Parameters["NCC2G6PD"].Value = dataRessult.baocaocobanct.NCC2G6PD;
                    rp.Parameters["NCC2CAH"].Value = dataRessult.baocaocobanct.NCC2CAH;
                    rp.Parameters["NCC2CH"].Value = dataRessult.baocaocobanct.NCC2CH;
                    rp.Parameters["NCC2GAL"].Value = dataRessult.baocaocobanct.NCC2GAL;
                    rp.Parameters["NCC2PKU"].Value = dataRessult.baocaocobanct.NCC2PKU;
                    rp.Parameters["NCT2G6PD"].Value = dataRessult.baocaocobanct.NCT2G6PD;
                    rp.Parameters["NCT2CAH"].Value = dataRessult.baocaocobanct.NCT2CAH;
                    rp.Parameters["NCT2CH"].Value = dataRessult.baocaocobanct.NCT2CH;
                    rp.Parameters["NCT2GAL"].Value = dataRessult.baocaocobanct.NCT2GAL;
                    rp.Parameters["NCT2PKU"].Value = dataRessult.baocaocobanct.NCT2PKU;
                    rp.Parameters["TuNgay"].Value = "từ ngày "+(this.dllNgay.tungay.Value).ToString("dd/MM/yyyy")+" đến ngày "+ (this.dllNgay.denngay.Value).ToString("dd/MM/yyyy");
                    rp.Parameters["DenNgay"].Value = (this.dllNgay.denngay.Value).ToString("dd/MM/yyyy");
                    rp.Parameters["NgayHienTai"].Value = (DateTime.Now).ToString("dd/MM/yyyy");
                    rp.Parameters["DonVi"].Value =lblTenDonVi.Text;
                    Reports.frmDanhSachDaCapMa rpt = new Reports.frmDanhSachDaCapMa(rp);
                    
                    rpt.ShowDialog();
                }
                else
                    XtraMessageBox.Show("Không có phiếu nào được cấp mã xét nghiệm trong khoảng thời gian bạn đã chọn!", "BioNet - Chương trình sàng lọc sơ sinh!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) { XtraMessageBox.Show("Lỗi khi export danh sách! \r\n Lỗi chi tiết : " + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }
        public static DataTable ListToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            Type Propiedad = null;
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                Propiedad = prop.PropertyType;
                if (Propiedad.IsGenericType && Propiedad.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Propiedad = Nullable.GetUnderlyingType(Propiedad);
                }
                table.Columns.Add(prop.Name, Propiedad);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
        
        private void AddItemForm()
        {
            PSMenuForm fo = new PSMenuForm
            {
                NameForm = this.Name,
                Capiton = this.Text,
            };
            BioNet_Bus.AddMenuForm(fo);
            long? idfo = BioNet_Bus.GetMenuIDForm(this.Name);
            CustomLayouts.TransLanguage.AddItemCT(this.Controls, idfo);
        }

    }
}