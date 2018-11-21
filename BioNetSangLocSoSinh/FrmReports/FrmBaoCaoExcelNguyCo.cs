using BioNetModel;
using DevExpress.Spreadsheet;
using Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BioNetSangLocSoSinh.FrmReports
{
    public partial class FrmBaoCaoExcelNguyCo : DevExpress.XtraEditors.XtraForm
    {
        public FrmBaoCaoExcelNguyCo()
        {
            InitializeComponent();
        }
        private Reports.RepostsBaoCao.rptBaoCaoExcelNguyCo datarp = new Reports.RepostsBaoCao.rptBaoCaoExcelNguyCo();
        private DataTable tab = new DataTable();
        private DataTable tabNghiNgo = new DataTable();
        private DataTable tabNguyCoCao = new DataTable();
        private DataTable tabNguycoThap2 = new DataTable();
        private DataTable tabDuongTinh = new DataTable();
        private DataTable tabAmTinh = new DataTable();
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Excel File|*.xls;*.xlsx";
            if (of.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    GCFileExcel.DataSource = null;
                    GCNghiNgo.DataSource = null;
                    GCFileExcel.DataSource = OpenFile(of.FileName);
                }
                catch
                {  }
            }                   
        }
       
            public DataTable  OpenFile(string fileName)
            {
            // var fullFileName = string.Format("{0}\\{1}", Directory.GetCurrentDirectory(), fileName);
            
            if (!File.Exists(fileName))
                {
                    System.Windows.Forms.MessageBox.Show("File not found");
                    return null;
                }
                using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader excelReader = null;
                    if (fileName.EndsWith(".xls"))
                    {
                        excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (fileName.EndsWith(".xlsx"))
                    {
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        throw new Exception("File format is not supported");
                    }
                    using (excelReader)
                    {
                    try
                    {
                        excelReader.IsFirstRowAsColumnNames = true;
                        var result = excelReader.AsDataSet();
                        excelReader.Close();
                        tab = result.Tables[0];
                    }
                    catch
                    {
                        MessageBox.Show("Lỗi mở file");
                    }
                    }
                }
            return tab;
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            LocDanhSach();

        }
        private void ThongKe()
        {
            rptBaoCaoExcelGeneCoBan baocao = new rptBaoCaoExcelGeneCoBan();
            // Tổng
            baocao.TongSoMau = new TKExcelSoMau();
            TongSoMau = (from DataRow myRow in tab.Rows
                         where !string.IsNullOrEmpty(myRow["STT"].ToString())
                         select myRow).Count();
            TongSoLamGene = (from DataRow myRow in tab.Rows
                             where !string.IsNullOrEmpty(myRow["KL gene"].ToString())
                             select myRow["KL gene"]).Count();
            var tabTongGene = (from DataRow myRow in tab.Rows
                               where string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                               select myRow["KQ gene"]).ToList();
            TongSoXD = tabTongGene.Where(x => x.Equals("KXĐ")).Count();
            baocao.NgayIn = DateTime.Now;
            rptBaoCaoExcelTong baoct = new rptBaoCaoExcelTong();
            baoct.Tieude = "";
            baoct.NgayIn = DateTime.Now;
            baoct.Gene = new List<rptBaoCaoExcelGene>();
            rptBaoCaoExcelGene baoc = new rptBaoCaoExcelGene();
            baoc.NgayIn = DateTime.Now;
            baoc.TenNhom = "Thông tin về xét nghiệm gene";
            var tabTongsoGene = (from DataRow myRow in tab.Rows
                                 where !string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                                 select myRow["KQ gene"]).Distinct().ToList();
            baoc.TongGene = new List<TKGene>();
            baoc.TongGene.Add(ThongKeTKExcelGene("Tổng"));
            baoc.TongGene.Add(ThongKeTKExcelGene("ChuaXN"));
            baoc.TongGene.Add(ThongKeTKExcelGene(""));
            baoc.TongGene.Add(ThongKeTKExcelGene("KXĐ"));
            baoc.TongGene.Add(ThongKeTKExcelGene("Xac dinh"));
            baoc.STT = "1";
            foreach (var t in tabTongsoGene)
            {
                if (!t.Equals("KXĐ"))
                {
                    baoc.TongGene.Add(ThongKeTKExcelGene(t.ToString()));
                }
            }
            baoct.Gene.Add(baoc);
            rptBaoCaoExcelGene baoc2 = new rptBaoCaoExcelGene();
            baoc2.NgayIn = DateTime.Now;
            baoc2.TenNhom = "Giới tính";
            baoc2.STT = "2";
            baoc2.NgayIn = DateTime.Now;
            baoc2.TongGene = new List<TKGene>();
            baoc2.TongGene.Add(ThongKeTKExcelGioiTinh("Nam"));
            baoc2.TongGene.Add(ThongKeTKExcelGioiTinh("Nữ"));
            baoc2.TongGene.Add(ThongKeTKExcelGioiTinh("N/A"));
            baoct.Gene.Add(baoc2);
            rptBaoCaoExcelGene baoc3 = new rptBaoCaoExcelGene();
            baoc3.NgayIn = DateTime.Now;
            baoc3.TenNhom = "Cân Nặng";
            baoc3.STT = "3";
            baoc3.NgayIn = DateTime.Now;
            baoc3.TongGene = new List<TKGene>();
            baoc3.TongGene.Add(ThongKeTKExcelCanNang(0, 2500));
            baoc3.TongGene.Add(ThongKeTKExcelCanNang(2500,3000));
            baoc3.TongGene.Add(ThongKeTKExcelCanNang(3000,0));
            baoct.Gene.Add(baoc3);
            rptBaoCaoExcelGene baoc4 = new rptBaoCaoExcelGene();
            baoc4.NgayIn = DateTime.Now;
            baoc4.TenNhom = "Chất lượng Mẫu lần 1";
            baoc4.STT = "4";
            baoc4.NgayIn = DateTime.Now;
            baoc4.TongGene = new List<TKGene>();
            baoc4.TongGene.Add(ThongKeTKExcelCLMauTong("Chất lượng mẫu 1"));
            baoc4.TongGene.Add(ThongKeTKExcelCLMau("Đạt","Chất lượng mẫu 1"));
            baoc4.TongGene.Add(ThongKeTKExcelCLMau("Không Đạt", "Chất lượng mẫu 1"));
            baoct.Gene.Add(baoc4);
            rptBaoCaoExcelGene baoc5 = new rptBaoCaoExcelGene();
            baoc5.NgayIn = DateTime.Now;
            baoc5.TenNhom = "Chất lượng Mẫu lần 2";
            baoc5.STT = "5";
            baoc5.NgayIn = DateTime.Now;
            baoc5.TongGene = new List<TKGene>();
            baoc5.TongGene.Add(ThongKeTKExcelCLMauTong("Chất lượng mẫu 2"));
            baoc5.TongGene.Add(ThongKeTKExcelCLMau("Đạt", "Chất lượng mẫu 2"));
            baoc5.TongGene.Add(ThongKeTKExcelCLMau("Không Đạt", "Chất lượng mẫu 2"));
            baoct.Gene.Add(baoc5);
            try
            {

                //Reports.RepostsBaoCao.rptThongKeExcelGene datarp = new Reports.RepostsBaoCao.rptThongKeExcelGene();
                datarp = new Reports.RepostsBaoCao.rptBaoCaoExcelNguyCo();
                datarp.DataSource = baoct;
                documentView.DocumentSource = datarp;
            }
            catch
            {

            }
        }
        private TKGene ThongKeTKExcelGene(string cso)
        {
            TKGene tk = new TKGene();
            try
            {
                var tabTongGene = (from DataRow myRow in tab.Rows
                                   where !string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                                   select myRow["KQ gene"]).ToList();
                var tabNghiNgoGene = (from DataRow myRow in tabNghiNgo.Rows
                                      where !string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                                      select myRow["KQ gene"]).ToList();
                var tabNgytCoCaoTongGene = (from DataRow myRow in tabNguyCoCao.Rows
                                            where !string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                                            select myRow["KQ gene"]).ToList();
                var tabNguyCoThapL2Gene = (from DataRow myRow in tabNguycoThap2.Rows
                                           where !string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                                           select myRow["KQ gene"]).ToList();
                var tabAmTinhGene = (from DataRow myRow in tabAmTinh.Rows
                                     where !string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                                     select myRow["KQ gene"]).ToList();
                var tabDuongTinhGene = (from DataRow myRow in tabDuongTinh.Rows
                                        where !string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                                        select myRow["KQ gene"]).ToList();
                if (string.IsNullOrEmpty(cso))
                {
                    tk.Ten = "Tổng đã làm đột biến gene";
                    tk.Tong = tabTongGene.Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGene.Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Count().ToString();
                    tk.Tile = String.Format("{0:00}", ((double)tabTongGene.Count() / (double)TongSoMau) * 100) + "%";
                }
                else if (cso.Equals("Tổng"))
                {
                    tk.Ten = "Tổng";
                    tk.Tong =  (from DataRow myRow in tab.Rows
                                           where !string.IsNullOrEmpty(myRow["STT"].ToString())
                                           select myRow).Count().ToString();
                    tk.NghiNgo = (from DataRow myRow in tabNghiNgo.Rows
                                  where !string.IsNullOrEmpty(myRow["STT"].ToString())
                                  select myRow).Count().ToString();
                    tk.NguyCoCao = (from DataRow myRow in tabNguyCoCao.Rows
                                    where !string.IsNullOrEmpty(myRow["STT"].ToString())
                                    select myRow).Count().ToString();
                    tk.NguyCoThapL2 = (from DataRow myRow in tabNguycoThap2.Rows
                                       where !string.IsNullOrEmpty(myRow["STT"].ToString())
                                       select myRow).Count().ToString();
                    tk.AmTinh = (from DataRow myRow in tabAmTinh.Rows
                                 where !string.IsNullOrEmpty(myRow["STT"].ToString())
                                 select myRow).Count().ToString();
                    tk.DuongTinh = (from DataRow myRow in tabDuongTinh.Rows
                                    where !string.IsNullOrEmpty(myRow["STT"].ToString())
                                    select myRow).Count().ToString();
                    tk.Tile = String.Format("{0:00}",((double)int.Parse(tk.Tong) / (double)TongSoMau)  * 100) + "%";
                }
                else if (cso.Equals("ChuaXN"))
                {
                    tk.Ten = "Chưa làm gene";
                    tk.Tong = (from DataRow myRow in tab.Rows
                               where !string.IsNullOrEmpty(myRow["STT"].ToString()) && string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                               select myRow).Count().ToString();
                    tk.NghiNgo = (from DataRow myRow in tabNghiNgo.Rows
                                  where !string.IsNullOrEmpty(myRow["STT"].ToString()) && string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                                  select myRow).Count().ToString();
                    tk.NguyCoCao = (from DataRow myRow in tabNguyCoCao.Rows
                                    where !string.IsNullOrEmpty(myRow["STT"].ToString()) && string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                                    select myRow).Count().ToString();
                    tk.NguyCoThapL2 = (from DataRow myRow in tabNguycoThap2.Rows
                                       where !string.IsNullOrEmpty(myRow["STT"].ToString()) && string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                                       select myRow).Count().ToString();
                    tk.AmTinh = (from DataRow myRow in tabAmTinh.Rows
                                 where !string.IsNullOrEmpty(myRow["STT"].ToString()) && string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                                 select myRow).Count().ToString();
                    tk.DuongTinh = (from DataRow myRow in tabDuongTinh.Rows
                                    where !string.IsNullOrEmpty(myRow["STT"].ToString()) && string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                                    select myRow).Count().ToString();
                    tk.Tile = String.Format("{0:00}", ((double)int.Parse(tk.Tong) / (double)TongSoMau) * 100) + "%";
                }
                else if (cso.Equals("KXĐ") || cso.Equals("Xac dinh"))
                {
                    var tabTongGeneKL = (from DataRow myRow in tab.Rows
                                       where !string.IsNullOrEmpty(myRow["KL gene"].ToString())
                                       select myRow["KL gene"]).ToList();
                    var tabNghiNgoGeneKL = (from DataRow myRow in tabNghiNgo.Rows
                                          where !string.IsNullOrEmpty(myRow["KL gene"].ToString())
                                          select myRow["KL gene"]).ToList();
                    var tabNgytCoCaoTongGeneKL = (from DataRow myRow in tabNguyCoCao.Rows
                                                where !string.IsNullOrEmpty(myRow["KL gene"].ToString())
                                                select myRow["KL gene"]).ToList();
                    var tabNguyCoThapL2GeneKL = (from DataRow myRow in tabNguycoThap2.Rows
                                               where !string.IsNullOrEmpty(myRow["KL gene"].ToString())
                                               select myRow["KL gene"]).ToList();
                    var tabAmTinhGeneKL = (from DataRow myRow in tabAmTinh.Rows
                                         where !string.IsNullOrEmpty(myRow["KL gene"].ToString())
                                         select myRow["KL gene"]).ToList();
                    var tabDuongTinhGeneKL = (from DataRow myRow in tabDuongTinh.Rows
                                            where !string.IsNullOrEmpty(myRow["KL gene"].ToString())
                                            select myRow["KL gene"]).ToList();
                    tk.Ten = cso;
                    tk.Tong = tabTongGeneKL.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGeneKL.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGeneKL.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2GeneKL.Where(x => x.Equals(cso)).Count().ToString();
                    tk.AmTinh = tabAmTinhGeneKL.Where(x => x.Equals(cso)).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGeneKL.Where(x => x.Equals(cso)).Count().ToString();
                    tk.Tile = String.Format("{0:00}", ((double)tabTongGeneKL.Where(x => x.Equals(cso)).Count() / (double)TongSoLamGene) * 100) + "%";
                }
                else
                {
                    tk.Ten = cso;
                    tk.Tong = tabTongGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.Tile = String.Format("{0:00}", ((double)tabTongGene.Where(x => x.Equals(cso)).Count() / (double)TongSoLamGene) * 100) + "%";
                }
            }
            catch
            {

            }
            return tk;
        }
        private TKGene ThongKeTKExcelGioiTinh(string cso)
        {
            TKGene tk = new TKGene();
            try
            {
                var tabTongGene = (from DataRow myRow in tab.Rows
                                   where !string.IsNullOrEmpty(myRow["Giới tính"].ToString())
                                   select myRow["Giới tính"]).ToList();
                var tabNghiNgoGene = (from DataRow myRow in tabNghiNgo.Rows
                                      where !string.IsNullOrEmpty(myRow["Giới tính"].ToString())
                                      select myRow["Giới tính"]).ToList();
                var tabNgytCoCaoTongGene = (from DataRow myRow in tabNguyCoCao.Rows
                                            where !string.IsNullOrEmpty(myRow["Giới tính"].ToString())
                                            select myRow["Giới tính"]).ToList();
                var tabNguyCoThapL2Gene = (from DataRow myRow in tabNguycoThap2.Rows
                                           where !string.IsNullOrEmpty(myRow["Giới tính"].ToString())
                                           select myRow["Giới tính"]).ToList();
                var tabAmTinhGene = (from DataRow myRow in tabAmTinh.Rows
                                     where !string.IsNullOrEmpty(myRow["Giới tính"].ToString())
                                     select myRow["Giới tính"]).ToList();
                var tabDuongTinhGene = (from DataRow myRow in tabDuongTinh.Rows
                                        where !string.IsNullOrEmpty(myRow["Giới tính"].ToString())
                                        select myRow["Giới tính"]).ToList();
                
                    tk.Ten = cso;
                    tk.Tong = tabTongGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.Tile = String.Format("{0:00}", ((double)tabTongGene.Where(x => x.Equals(cso)).Count() / (double)TongSoMau) * 100) + "%";
            }
            catch
            {

            }
            return tk;
        }

        private TKGene ThongKeTKExcelCanNang(int? min,int? max)
        {
            TKGene tk = new TKGene();
            try
            {
                var tabTongGene = (from DataRow myRow in tab.Rows
                                   where !string.IsNullOrEmpty(myRow["Cân nặng"].ToString())
                                   select myRow["Cân nặng"]).ToList();
                var tabNghiNgoGene = (from DataRow myRow in tabNghiNgo.Rows
                                      where !string.IsNullOrEmpty(myRow["Cân nặng"].ToString())
                                      select myRow["Cân nặng"]).ToList();
                var tabNgytCoCaoTongGene = (from DataRow myRow in tabNguyCoCao.Rows
                                            where !string.IsNullOrEmpty(myRow["Cân nặng"].ToString())
                                            select myRow["Cân nặng"]).ToList();
                var tabNguyCoThapL2Gene = (from DataRow myRow in tabNguycoThap2.Rows
                                           where !string.IsNullOrEmpty(myRow["Cân nặng"].ToString())
                                           select myRow["Cân nặng"]).ToList();
                var tabAmTinhGene = (from DataRow myRow in tabAmTinh.Rows
                                     where !string.IsNullOrEmpty(myRow["Cân nặng"].ToString())
                                     select myRow["Cân nặng"]).ToList();
                var tabDuongTinhGene = (from DataRow myRow in tabDuongTinh.Rows
                                        where !string.IsNullOrEmpty(myRow["Cân nặng"].ToString())
                                        select myRow["Cân nặng"]).ToList();

                if(min==0)
                {
                    tk.Ten ="<="+max.ToString();
                    tk.Tong = tabTongGene.Where(x => int.Parse(x.ToString())<=max).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => int.Parse(x.ToString()) <= max).Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGene.Where(x => int.Parse(x.ToString()) <= max).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => int.Parse(x.ToString()) <= max).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => int.Parse(x.ToString()) <= max).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => int.Parse(x.ToString()) <= max).Count().ToString();
                    tk.Tile = String.Format("{0:00}", ((double)tabTongGene.Where(x => int.Parse(x.ToString()) <= max).Count() / (double)TongSoMau) * 100) + "%";
                }
                else if (max==0)
                 {
                    tk.Ten = ">" + min.ToString();
                    tk.Tong = tabTongGene.Where(x => int.Parse(x.ToString()) >min).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.Tile = String.Format("{0:00}", ((double)tabTongGene.Where(x => int.Parse(x.ToString())> min).Count() / (double)TongSoMau) * 100) + "%";
                }
                else
                {
                    tk.Ten =min.ToString()+"<X<=" + max.ToString();
                    tk.Tong = tabTongGene.Where(x => int.Parse(x.ToString()) <= max && int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => int.Parse(x.ToString()) <= max && int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGene.Where(x => int.Parse(x.ToString()) <= max && int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => int.Parse(x.ToString()) <= max && int.Parse(x.ToString()) > min).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => int.Parse(x.ToString()) <= max && int.Parse(x.ToString()) > min).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => int.Parse(x.ToString()) <= max && int.Parse(x.ToString()) > min).Count().ToString();
                    tk.Tile = String.Format("{0:00}", ((double)tabTongGene.Where(x => int.Parse(x.ToString()) <= max).Count() / (double)TongSoMau) * 100) + "%";
                }
            }
            catch
            {

            }
            return tk;
        }
        private TKGene ThongKeTKExcelCLMau(string cso,string CLM)
        {
            TKGene tk = new TKGene();
            try
            {
                var tabTongGene = (from DataRow myRow in tab.Rows
                                   where !string.IsNullOrEmpty(myRow[CLM].ToString())
                                   select myRow[CLM]).ToList();
                var tabNghiNgoGene = (from DataRow myRow in tabNghiNgo.Rows
                                      where !string.IsNullOrEmpty(myRow[CLM].ToString())
                                      select myRow[CLM]).ToList();
                var tabNgytCoCaoTongGene = (from DataRow myRow in tabNguyCoCao.Rows
                                            where !string.IsNullOrEmpty(myRow[CLM].ToString())
                                            select myRow[CLM]).ToList();
                var tabNguyCoThapL2Gene = (from DataRow myRow in tabNguycoThap2.Rows
                                           where !string.IsNullOrEmpty(myRow[CLM].ToString())
                                           select myRow[CLM]).ToList();
                var tabAmTinhGene = (from DataRow myRow in tabAmTinh.Rows
                                     where !string.IsNullOrEmpty(myRow[CLM].ToString())
                                     select myRow[CLM]).ToList();
                var tabDuongTinhGene = (from DataRow myRow in tabDuongTinh.Rows
                                        where !string.IsNullOrEmpty(myRow[CLM].ToString())
                                        select myRow[CLM]).ToList();
                tk.Ten = cso;
                tk.Tong = tabTongGene.Where(x => x.Equals(cso)).Count().ToString();
                tk.NghiNgo = tabNghiNgoGene.Where(x => x.Equals(cso)).Count().ToString();
                tk.NguyCoCao = tabNgytCoCaoTongGene.Where(x => x.Equals(cso)).Count().ToString();
                tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => x.Equals(cso)).Count().ToString();
                tk.AmTinh = tabAmTinhGene.Where(x => x.Equals(cso)).Count().ToString();
                tk.DuongTinh = tabDuongTinhGene.Where(x => x.Equals(cso)).Count().ToString();
                tk.Tile = String.Format("{0:00}", ((double)tabTongGene.Where(x => x.Equals(cso)).Count() / (double)tabTongGene.Count) * 100) + "%";
            }
            catch
            {

            }
            return tk;
        }
        private TKGene ThongKeTKExcelCLMauTong(string CLM)
        {
            TKGene tk = new TKGene();
            try
            {
                var tabTongGene = (from DataRow myRow in tab.Rows
                                   where !string.IsNullOrEmpty(myRow[CLM].ToString())
                                   select myRow[CLM]).ToList();
                var tabNghiNgoGene = (from DataRow myRow in tabNghiNgo.Rows
                                      where !string.IsNullOrEmpty(myRow[CLM].ToString())
                                      select myRow[CLM]).ToList();
                var tabNgytCoCaoTongGene = (from DataRow myRow in tabNguyCoCao.Rows
                                            where !string.IsNullOrEmpty(myRow[CLM].ToString())
                                            select myRow[CLM]).ToList();
                var tabNguyCoThapL2Gene = (from DataRow myRow in tabNguycoThap2.Rows
                                           where !string.IsNullOrEmpty(myRow[CLM].ToString())
                                           select myRow[CLM]).ToList();
                var tabAmTinhGene = (from DataRow myRow in tabAmTinh.Rows
                                     where !string.IsNullOrEmpty(myRow[CLM].ToString())
                                     select myRow[CLM]).ToList();
                var tabDuongTinhGene = (from DataRow myRow in tabDuongTinh.Rows
                                        where !string.IsNullOrEmpty(myRow[CLM].ToString())
                                        select myRow[CLM]).ToList();
                tk.Ten = "Tổng";
                tk.Tong = tabTongGene.Count.ToString();
                tk.NghiNgo = tabNghiNgoGene.Count.ToString();
                tk.NguyCoCao = tabNgytCoCaoTongGene.Count.ToString();
                tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Count.ToString();
                tk.AmTinh = tabAmTinhGene.Count.ToString();
                tk.DuongTinh = tabDuongTinhGene.Count.ToString();
                tk.Tile = String.Format("{0:00}", 100)+ "%";
            }
            catch
            {

            }
            return tk;
        }
        private int TongSoMau = 0;
        private int TongSoLamGene = 0;
        private int TongSoXD = 0;
        private void LocDanhSach()
        {
            try
            {
                tabNghiNgo = new DataTable();
                tabNguyCoCao = new DataTable();
                tabNguycoThap2 = new DataTable();
                tabAmTinh = new DataTable();
                tabDuongTinh = new DataTable(); ;
                int rows = tab.Rows.Count;
                int cols = tab.Columns.Count;
                string MaDichVu = string.Empty;
                string MaXN = string.Empty;
                int CNhomMau = 0;
                int CKLChung = 0;
                for (int c = 0; c < cols; c++)
                {
                    string xn = tab.Columns[c].ToString().Trim();
                    tabNghiNgo.Columns.Add(xn);
                    tabNguyCoCao.Columns.Add(xn);
                    tabNguycoThap2.Columns.Add(xn);
                    tabAmTinh.Columns.Add(xn);
                    tabDuongTinh.Columns.Add(xn);
                    switch (xn.ToUpper())
                    {
                        case "NHÓM MẪU":
                            {
                                CNhomMau = c;
                                break;
                            }
                        case "KẾT LUẬN CHUNG":
                            {
                                CKLChung = c;
                                break;
                            }
                        default:
                            break;
                    }
                }
                List<int> listrowNghiNgo = new List<int>();
                List<int> listrowNguycocao = new List<int>();
                List<int> listrowNguycothapL2 = new List<int>();
                List<int> listrowAmTinh = new List<int>();
                List<int> listrowDuongTinh = new List<int>();
                for (int r = 0; r < rows; r++)
                {
                    string xnr = tab.Rows[r][CNhomMau].ToString().Trim();
                    switch (xnr)
                    {
                        case "Nguy co cao":
                            {
                                listrowNguycocao.Add(r);
                                break;
                            }

                        case "Nguy cơ cao":
                            {
                                listrowNguycocao.Add(r);
                                break;
                            }
                        case "Nghi ngo":
                            {
                                listrowNghiNgo.Add(r);
                                break;
                            }
                        case "Nghi ngờ":
                            {
                                listrowNghiNgo.Add(r);
                                break;
                            }
                        case "Nguy co thap L2":
                            {
                                listrowNguycothapL2.Add(r);
                                break;
                            }
                        case "Nguy cơ thấp L2":
                            {
                                listrowNguycothapL2.Add(r);
                                break;
                            }
                        default:
                            break;
                    }
                    string xnkl = tab.Rows[r][CKLChung].ToString();
                    switch (xnkl.ToUpper())
                    {
                        case "POSITIVE":
                            {
                                listrowDuongTinh.Add(r);
                                break;
                            }
                        case "POSITIVE_NOTE":
                            {
                                listrowDuongTinh.Add(r);
                                break;
                            }
                        case "NEGATIVE":
                            {
                                listrowAmTinh.Add(r);
                                break;
                            }
                            
                    }

                }

                foreach (var ro in listrowNghiNgo)
                {
                    DataRow dr = tabNghiNgo.NewRow();
                    for (int c = 0; c < cols; c++)
                    {
                        dr[tab.Columns[c].ToString().Trim()] = tab.Rows[ro][c].ToString();
                    }
                    tabNghiNgo.Rows.Add(dr);
                }
                foreach (var ro in listrowNguycothapL2)
                {
                    DataRow dr = tabNguycoThap2.NewRow();
                    for (int c = 0; c < cols; c++)
                    {
                        dr[tab.Columns[c].ToString().Trim()] = tab.Rows[ro][c].ToString();

                    }
                    tabNguycoThap2.Rows.Add(dr);
                }
                foreach (var ro in listrowNguycocao)
                {
                    DataRow dr = tabNguyCoCao.NewRow();
                    for (int c = 0; c < cols; c++)
                    {
                        dr[tab.Columns[c].ToString().Trim()] = tab.Rows[ro][c].ToString();

                    }
                    tabNguyCoCao.Rows.Add(dr);
                }
                foreach (var ro in listrowAmTinh)
                {
                    DataRow dr = tabAmTinh.NewRow();
                    for (int c = 0; c < cols; c++)
                    {
                        dr[tab.Columns[c].ToString().Trim()] = tab.Rows[ro][c].ToString();

                    }
                    tabAmTinh.Rows.Add(dr);
                }
                foreach (var ro in listrowDuongTinh)
                {
                    DataRow dr = tabDuongTinh.NewRow();
                    for (int c = 0; c < cols; c++)
                    {
                        dr[tab.Columns[c].ToString().Trim()] = tab.Rows[ro][c].ToString();

                    }
                    tabDuongTinh.Rows.Add(dr);
                }
                GCNghiNgo.DataSource = null;
                GCNghiNgo.DataSource = tabNghiNgo;
                GCNguycothap2.DataSource = null;
                GCNguycothap2.DataSource = tabNguycoThap2;
                GCNguycocao.DataSource = null;
                GCNguycocao.DataSource = tabNguyCoCao;
                GCDuongtinh.DataSource = null;
                GCDuongtinh.DataSource = tabDuongTinh;
                GCAmTinh.DataSource = null;
                GCAmTinh.DataSource = tabAmTinh;
               ThongKe();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Kiểm tra lại file dữ liệu góc");
            }
        
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void xtraTabPage3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void xtraTabPage4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            GVFileExcel.ExportToXlsx("FileTempTT.xlsx", new DevExpress.XtraPrinting.XlsxExportOptions() { SheetName = "DataTong" });

            Workbook workbook2 = new DevExpress.Spreadsheet.Workbook();
            using (FileStream stream = new FileStream("FileTempTT.xlsx", FileMode.Open))
            {
                workbook2.LoadDocument(stream, DocumentFormat.Xlsx);
            }
            Workbook workbook = new DevExpress.Spreadsheet.Workbook();
            workbook.Worksheets.Insert(0, "DataTong");
            workbook.Worksheets[0].CopyFrom(workbook2.Worksheets[0]);
            File.Delete("FileTempTT.xlsx");
            GVNghiNgo.ExportToXlsx("FileTempTT.xlsx", new DevExpress.XtraPrinting.XlsxExportOptions() { SheetName = "NghiNgo" });
            using (FileStream stream = new FileStream("NghiNgo" + ".xlsx", FileMode.Open))
            {
                workbook2.LoadDocument(stream, DocumentFormat.Xlsx);
            }
            workbook.Worksheets.Insert(1, "NghiNgo");
            workbook.Worksheets[1].CopyFrom(workbook2.Worksheets[0]);
            File.Delete("FileTempTT" + ".xlsx");
            GVNguycothap2.ExportToXlsx("FileTempTT.xlsx", new DevExpress.XtraPrinting.XlsxExportOptions() { SheetName = "NguyCoThapL2" });
            using (FileStream stream = new FileStream("FileTempTT" + ".xlsx", FileMode.Open))
            {
                workbook2.LoadDocument(stream, DocumentFormat.Xlsx);
            }
            workbook.Worksheets.Insert(2, "NguyCoThapL2");
            workbook.Worksheets[2].CopyFrom(workbook2.Worksheets[0]);
            File.Delete("FileTempTT" + ".xlsx");
            GVNguycocao.ExportToXlsx("FileTempTT.xlsx", new DevExpress.XtraPrinting.XlsxExportOptions() { SheetName = "NguyCoCao" });
            using (FileStream stream = new FileStream("FileTempTT" + ".xlsx", FileMode.Open))
            {
                workbook2.LoadDocument(stream, DocumentFormat.Xlsx);
            }
            workbook.Worksheets.Insert(3, "NguyCoCao");
            workbook.Worksheets[3].CopyFrom(workbook2.Worksheets[0]);
            File.Delete("FileTempTT" + ".xlsx");
            GVDuongTinh.ExportToXlsx("FileTempTT.xlsx", new DevExpress.XtraPrinting.XlsxExportOptions() { SheetName = "DuongTinh" });
            using (FileStream stream = new FileStream("FileTempTT" + ".xlsx", FileMode.Open))
            {
                workbook2.LoadDocument(stream, DocumentFormat.Xlsx);
            }
            workbook.Worksheets.Insert(4, "DuongTinh");
            workbook.Worksheets[4].CopyFrom(workbook2.Worksheets[0]);
            File.Delete("FileTempTT" + ".xlsx");
            GVAmTinh.ExportToXlsx("FileTempTT.xlsx", new DevExpress.XtraPrinting.XlsxExportOptions() { SheetName = "AmTinh" });
            using (FileStream stream = new FileStream("FileTempTT" + ".xlsx", FileMode.Open))
            {
                workbook2.LoadDocument(stream, DocumentFormat.Xlsx);
            }
            workbook.Worksheets.Insert(5, "AmTinh");
            workbook.Worksheets[6].CopyFrom(workbook2.Worksheets[0]);
            File.Delete("FileTempTT" + ".xlsx");

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }
    }
}
