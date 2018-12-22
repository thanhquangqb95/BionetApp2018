﻿using BioNetBLL;
using BioNetModel;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors;
using Excel;
using iTextSharp.text.pdf;
using iTextSharp.text;
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
        private List<Object> lstdv = new List<Object>();
        private List<Object> lstchicuc = new List<Object>();
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Excel File|*.xls;*.xlsx";
            if (of.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    GCFileExcel.DataSource = null;
                    GVFileExcel.Columns.Clear();
                    GCNghiNgo.DataSource = null;
                    GVNghiNgo.Columns.Clear();
                    GCAmTinh.DataSource = null;
                    GVAmTinh.Columns.Clear();
                    GCDuongtinh.DataSource = null;
                    GVDuongTinh.Columns.Clear();
                    GCNguycocao.DataSource = null;
                    GVNguycocao.Columns.Clear();
                    GCNguycothap2.DataSource = null;
                    GVNguycothap2.Columns.Clear();
                    TabPageBaoCaoTongHop.Controls.Clear();
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
                        DataTable tabtemp = result.Tables[0];
                        int rows = tabtemp.Rows.Count;
                        int cols = tabtemp.Columns.Count;
                        tab = new DataTable();
                        for (int c = 0; c < cols; c++)
                        {
                            string xn = tabtemp.Columns[c].ToString().Trim();
                            tab.Columns.Add(xn);
                        }
                        for (int r = 0; r < rows; r++)
                        {
                           DataRow dr = tab.NewRow();
                           for (int c = 0; c < cols; c++)
                           {
                              dr[tab.Columns[c].ToString().Trim()] = tabtemp.Rows[r][c].ToString();
                           }
                           tab.Rows.Add(dr);
                        }
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
            ThongKeDonVi();
            lstdv= (from DataRow myRow in tab.Rows
                                             where !string.IsNullOrEmpty(myRow["Tên Đơn Vị"].ToString())
                                             select myRow["Tên Đơn Vị"]).Distinct().ToList();
            cbbDonVi.Properties.DataSource = null;
            cbbDonVi.Properties.DataSource = lstdv;
            lstchicuc = (from DataRow myRow in tab.Rows
                     where !string.IsNullOrEmpty(myRow["Chi cục"].ToString())
                     select myRow["Chi cục"]).Distinct().ToList();
            cbbChiCuc.Properties.DataSource = null;
            cbbChiCuc.Properties.DataSource = lstchicuc;
        }
        private void ThongKe()
        {
            rptBaoCaoExcelGeneCoBan baocao = new rptBaoCaoExcelGeneCoBan();
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
            baoct.Tieude = "Trung tâm sàng lọc sơ sinh Bionet (Tổng hợp)";
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
            List<TKGene> GeneTemp = new List<TKGene>();
            foreach (var t in tabTongsoGene)
            {
                if (!t.Equals("KXĐ"))
                {
                    GeneTemp.Add(ThongKeTKExcelGene(t.ToString()));
                }
            }
            baoc.TongGene.AddRange(GeneTemp.OrderByDescending(x => int.Parse(x.Tong)));
            baoct.Gene.Add(baoc);
            rptBaoCaoExcelGene baoc2 = new rptBaoCaoExcelGene();
            baoc2.NgayIn = DateTime.Now;
            baoc2.TenNhom = "Giới tính";
            baoc2.STT = "2";
            baoc2.NgayIn = DateTime.Now;
            baoc2.TongGene = new List<TKGene>();
            baoc2.TongGene.Add(ThongKeTKExcelCLMauTong("Giới tính"));
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
            baoc3.TongGene.Add(ThongKeTKExcelCLMauTong("Cân nặng"));
            baoc3.TongGene.Add(ThongKeTKExcelCanNang(0, 2500));
            baoc3.TongGene.Add(ThongKeTKExcelCanNang(2500, 3000));
            baoc3.TongGene.Add(ThongKeTKExcelCanNang(3000, 3500));
            baoc3.TongGene.Add(ThongKeTKExcelCanNang(3500, 4000));
            baoc3.TongGene.Add(ThongKeTKExcelCanNang(4000, 5000));
            baoc3.TongGene.Add(ThongKeTKExcelCanNang(5000, 0));
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
            rptBaoCaoExcelGene baoc6 = new rptBaoCaoExcelGene();
            baoc6.NgayIn = DateTime.Now;
            baoc6.TenNhom = "Dân tộc";
            baoc6.STT = "6";
            baoc6.NgayIn = DateTime.Now;
            baoc6.TongGene = new List<TKGene>();
            baoc6.TongGene.Add(ThongKeTKExcelCLMauTong("Dân tộc"));
            var dsdantoc = (from DataRow myRow in tab.Rows
                               where !string.IsNullOrEmpty(myRow["Dân tộc"].ToString())
                               select myRow["Dân tộc"]).Distinct().ToList();
            GeneTemp = new List<TKGene>();
            foreach (var dt in dsdantoc)
            {
                GeneTemp.Add(ThongKeTKExcelCLMau(dt.ToString(), "Dân tộc"));
            }
            baoc6.TongGene.AddRange(GeneTemp.OrderByDescending(x => int.Parse(x.Tong)));
            baoct.Gene.Add(baoc6);
            try
            {

                Reports.RepostsBaoCao.rptBaoCaoExcelNguyCo datarp = new Reports.RepostsBaoCao.rptBaoCaoExcelNguyCo();
                datarp.Parameters["LuuY"].Value = "(Lưu ý: Báo cáo thống kê có giá trị tại thời điểm xuất báo cáo ngày " + DateTime.Now.Date.ToShortDateString() + ")";
                datarp.Parameters["ThoiGian"].Value = "Từ ngày " + dateEditTuNgay.DateTime.Date.ToShortDateString() + " đến " + dateEditDenNgay.DateTime.Date.ToShortDateString();
                datarp.DataSource = baoct;
                TabPageBaoCaoTongHop.Controls.Clear();
                Reports.frmReport myForm = new Reports.frmReport(datarp);
                myForm.TopLevel = false;
                myForm.Parent = TabPageBaoCaoTongHop;
                myForm.Dock = DockStyle.Fill;
                myForm.Show();
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
                    tk.Tile = String.Format("{0:0.00}", ((double)tabTongGene.Count() / (double)TongSoMau) * 100) + "%";
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
                    tk.Tile = String.Format("{0:0.00}",((double)int.Parse(tk.Tong) / (double)TongSoMau)  * 100) + "%";
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
                    tk.Tile = String.Format("{0:0.00}", ((double)int.Parse(tk.Tong) / (double)TongSoMau) * 100) + "%";
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
                    tk.Tile = String.Format("{0:0.00}", ((double)tabTongGeneKL.Where(x => x.Equals(cso)).Count() / (double)TongSoLamGene) * 100) + "%";
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
                    tk.Tile = String.Format("{0:0.00}", ((double)tabTongGene.Where(x => x.Equals(cso)).Count() / (double)TongSoLamGene) * 100) + "%";
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
                    tk.Tile = String.Format("{0:0.00}", ((double)tabTongGene.Where(x => x.Equals(cso)).Count() / (double)TongSoMau) * 100) + "%";
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
                    tk.Tile = String.Format("{0:0.00}", ((double.Parse(tk.Tong)) / (double)TongSoMau) * 100) + "%";
                }
                else if (max==0)
                 {
                    if(min==5000)
                    {
                        tk.Ten ="N/A";
                    }
                    else
                    {
                        tk.Ten = ">" + min.ToString();
                    }
                    tk.Tong = tabTongGene.Where(x => int.Parse(x.ToString()) >min).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.Tile = String.Format("{0:0.00}", ((double.Parse(tk.Tong)) / (double)TongSoMau) * 100) + "%";
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
                    tk.Tile = String.Format("{0:0.00}", ((double.Parse(tk.Tong)) / (double)TongSoMau) * 100) + "%";
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
                tk.Tile = String.Format("{0:0.00}", ((double)tabTongGene.Where(x => x.Equals(cso)).Count() / (double)tabTongGene.Count) * 100) + "%";
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
                tk.Tile = String.Format("{0:0.00}", 100)+ "%";
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
                XtraMessageBox.Show("Vui lòng kiểm tra lại dữ liệu file gốc", "Bionet Sàng lọc sơ sinh.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        
        }
        
        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            // ofd.Multiselect = false;
            ofd.Filter = "Execl files |*.xls *.xlsx";
            ofd.FileName = "BaoCaoThongKeTheoExcelNguyCo_" + DateTime.Now.Day+DateTime.Now.Month+DateTime.Now.Year + ".xlsx";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Workbook workbook = new DevExpress.Spreadsheet.Workbook();

                GVFileExcel.ExportToXlsx("FileTempTT.xlsx", new DevExpress.XtraPrinting.XlsxExportOptions() { SheetName = "DataTong" });
                Workbook workbook2 = new DevExpress.Spreadsheet.Workbook();
                using (FileStream stream = new FileStream("FileTempTT.xlsx", FileMode.Open))
                {
                    workbook2.LoadDocument(stream, DocumentFormat.Xlsx);
                }
                
                workbook.Worksheets.Insert(0, "DataTong");
                workbook.Worksheets[0].CopyFrom(workbook2.Worksheets[0]);
                File.Delete("FileTempTT.xlsx");

                GVNghiNgo.ExportToXlsx("FileTempTT.xlsx", new DevExpress.XtraPrinting.XlsxExportOptions() { SheetName = "NghiNgo" });
                using (FileStream stream = new FileStream("FileTempTT" + ".xlsx", FileMode.Open))
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
                workbook.Worksheets[5].CopyFrom(workbook2.Worksheets[0]);
                File.Delete("FileTempTT" + ".xlsx");

                GVBaoCaoDonVi.ExportToXlsx("FileTempTT.xlsx", new DevExpress.XtraPrinting.XlsxExportOptions() { SheetName = "BaoCaoDonVi" });
                using (FileStream stream = new FileStream("FileTempTT" + ".xlsx", FileMode.Open))
                {
                    workbook2.LoadDocument(stream, DocumentFormat.Xlsx);
                }
                workbook.Worksheets.Insert(5, "BaoCaoDonVi");
                workbook.Worksheets[5].CopyFrom(workbook2.Worksheets[0]);
                File.Delete("FileTempTT" + ".xlsx");
                workbook.SaveDocument(ofd.FileName);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            // ofd.Multiselect = false;
            //ofd.Filter = "PDF File(*.pdf)";
            ofd.FileName = "BaoCaoThongKeTheoExcelNguyCo_" +DateTime.Now.ToShortDateString()  + ".pdf";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName.Length > 0)
                {
                    try
                    {

                        XuatPDF(ofd.FileName);
                    }
                    catch
                    {
                        XtraMessageBox.Show("Không thể lưu file này! Vui lòng chọn đường dẫn khác.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void XuatPDF(string filename)
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
            baoc3.TongGene.Add(ThongKeTKExcelCanNang(2500, 3000));
            baoc3.TongGene.Add(ThongKeTKExcelCanNang(3000, 3500));
            baoc3.TongGene.Add(ThongKeTKExcelCanNang(3500, 4000));
            baoc3.TongGene.Add(ThongKeTKExcelCanNang(4000,5000));
            baoc3.TongGene.Add(ThongKeTKExcelCanNang(5000, 0));
            baoct.Gene.Add(baoc3);
            rptBaoCaoExcelGene baoc4 = new rptBaoCaoExcelGene();
            baoc4.NgayIn = DateTime.Now;
            baoc4.TenNhom = "Chất lượng Mẫu lần 1";
            baoc4.STT = "4";
            baoc4.NgayIn = DateTime.Now;
            baoc4.TongGene = new List<TKGene>();
            baoc4.TongGene.Add(ThongKeTKExcelCLMauTong("Chất lượng mẫu 1"));
            baoc4.TongGene.Add(ThongKeTKExcelCLMau("Đạt", "Chất lượng mẫu 1"));
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
                datarp.ExportToPdf(filename);
            }
            catch
            {

            }
        }

        #region Báo cáo đơn vị
        private void ThongKeDonVi()
        {
            try
            {
                DataTable tabledv = new DataTable();
                var kqgene = (from DataRow myRow in tab.Rows
                              where !string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                              select myRow["KQ gene"]).Distinct().ToList();
                tabledv.Columns.Add("STT");//0
                tabledv.Columns.Add("Mã Đơn Vị");//1
                tabledv.Columns.Add("Tên Đơn Vị");//2
                tabledv.Columns.Add("Viết Tắt ĐV");//3
                tabledv.Columns.Add("Tổng");//4
                tabledv.Columns.Add("Nghi ngờ");//5
                tabledv.Columns.Add("Nguy cơ cao");//6
                tabledv.Columns.Add("Nguy cơ thấp L2");//7
                tabledv.Columns.Add("Âm tính");//8
                tabledv.Columns.Add("Dương tính");//9
                foreach (var kq in kqgene)
                {
                    tabledv.Columns.Add(kq.ToString());
                }
                var lisdv = (from DataRow myRow in tab.Rows
                             where !string.IsNullOrEmpty(myRow["Tên Đơn Vị"].ToString())
                             select myRow["Tên Đơn Vị"]).Distinct().ToList();
                int stt = 1;
                var gen = (from DataRow myRow in tab.Rows
                           where !string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                           select myRow).ToList();
                foreach (var dv in lisdv)
                {
                    try
                    {
                        DataRow dr = tabledv.NewRow();
                        try
                        {
                            dr[tabledv.Columns[3].ToString().Trim()] = (from DataRow myRow in tab.Rows
                                                                        where
                                   !string.IsNullOrEmpty(myRow["Tên Đơn Vị"].ToString())
                                                                        where myRow["Tên Đơn Vị"].Equals(dv)
                                                                        select myRow["Viết Tắt ĐV"]).FirstOrDefault();
                            dr[tabledv.Columns[1].ToString().Trim()] = (from DataRow myRow in tab.Rows
                                                                        where
                                        !string.IsNullOrEmpty(myRow["Tên Đơn Vị"].ToString())
                                                                        where myRow["Tên Đơn Vị"].Equals(dv)
                                                                        select myRow["Mã Đơn Vị"]).FirstOrDefault();
                        }
                        catch
                        {

                        }
                        dr[tabledv.Columns[0].ToString().Trim()] = stt.ToString();
                        dr[tabledv.Columns[2].ToString().Trim()] = dv.ToString();
                        var kq= ThongKeTKExcelCLMau(dv.ToString(), "Tên Đơn Vị");
                        dr[tabledv.Columns[4].ToString().Trim()] = kq.Tong;
                        dr[tabledv.Columns[5].ToString().Trim()] = kq.NghiNgo;
                        dr[tabledv.Columns[6].ToString().Trim()] = kq.NguyCoCao;
                        dr[tabledv.Columns[7].ToString().Trim()] = kq.NguyCoThapL2;
                        dr[tabledv.Columns[8].ToString().Trim()] = kq.AmTinh;
                        dr[tabledv.Columns[9].ToString().Trim()] = kq.DuongTinh;
                        var gene = (from DataRow myRow in tab.Rows
                                    where !string.IsNullOrEmpty(myRow["KQ gene"].ToString()) && myRow["Tên Đơn Vị"].Equals(dv)
                                    select myRow["KQ gene"]).ToList();
                        for (int c = 10; c < tabledv.Columns.Count; c++)
                        {
                            string ge = tabledv.Columns[c].ToString();
                            dr[tabledv.Columns[c].ToString().Trim()] = gene.Where(x => x.Equals(ge)).ToList().Count().ToString();
                        }
                        tabledv.Rows.Add(dr);
                        stt = stt + 1;
                    }
                    catch
                    {

                    }
                }
                GCBaoCaoDonVi.DataSource = null;
                GVBaoCaoDonVi.Columns.Clear();
                GCBaoCaoDonVi.DataSource = tabledv;
            }
            catch
            {

            }
        }
        #endregion

        private void btnPDFDonVi_Click(object sender, EventArgs e)
        {
            string Folder = BioNet_Bus.GetFileReport("Gene", "PDF");
            if (cbbDonVi.EditValue!=null)
            {
                PsReponse reponse = ThongKeDonVi(cbbDonVi.EditValue.ToString(), Folder);
                if (reponse.Result != true)
                {
                    XtraMessageBox.Show("Xuất báo cáo lỗi "+ reponse.StringError, "Bionet Sàng lọc sơ sinh.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    XtraMessageBox.Show("Xuất báo cáo thành công", "Bionet Sàng lọc sơ sinh.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start(Folder);
                }
            }
            else
            {
            
                bool kq = true;
                foreach (var dv in lstdv)
                {
                    PsReponse reponse= ThongKeDonVi(dv.ToString(), Folder);
                    if(reponse.Result!=true)
                    {
                        kq = false;
                    }
                }
                if(kq)
                {
                    XtraMessageBox.Show("Xuất báo cáo thành công", "Bionet Sàng lọc sơ sinh.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start(Folder);
                }
                else
                {
                    XtraMessageBox.Show("Xuất báo cáo lỗi", "Bionet Sàng lọc sơ sinh.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            
        }
        private PsReponse ThongKeDonVi(string Donvi,string FoderLink)
            
        {
            PsReponse psReponse = new PsReponse();
            try
            {
                int TongSoMauDV = (from DataRow myRow in tab.Rows
                                   where !string.IsNullOrEmpty(myRow["STT"].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                   select myRow).Count();
                TongSoLamGene = (from DataRow myRow in tab.Rows
                                 where !string.IsNullOrEmpty(myRow["KL gene"].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                 select myRow["KL gene"]).Count();
                var tabTongGene = (from DataRow myRow in tab.Rows
                                   where !string.IsNullOrEmpty(myRow["KQ gene"].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                   select myRow["KQ gene"]).ToList();
                TongSoXD = tabTongGene.Where(x => x.Equals("KXĐ")).Count();
               
                rptBaoCaoExcelTong baoct = new rptBaoCaoExcelTong();
                baoct.Tieude = Donvi;
                baoct.NgayIn = DateTime.Now;
                baoct.Gene = new List<rptBaoCaoExcelGene>();
                rptBaoCaoExcelGene baoc = new rptBaoCaoExcelGene();
                baoc.NgayIn = DateTime.Now;
                baoc.TenNhom = "Thông tin về xét nghiệm gene";
                var tabTongsoGene = (from DataRow myRow in tab.Rows
                                     where !string.IsNullOrEmpty(myRow["KQ gene"].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                     select myRow["KQ gene"]).Distinct().ToList();
                baoc.TongGene = new List<TKGene>();
                baoc.TongGene.Add(ThongKeTKExcelGeneDV("Tổng", Donvi, TongSoMauDV));
                baoc.TongGene.Add(ThongKeTKExcelGeneDV("ChuaXN", Donvi, TongSoMauDV));
                baoc.TongGene.Add(ThongKeTKExcelGeneDV("", Donvi, TongSoMauDV));
                baoc.TongGene.Add(ThongKeTKExcelGeneDV("KXĐ", Donvi, TongSoMauDV));
                baoc.TongGene.Add(ThongKeTKExcelGeneDV("Xac dinh", Donvi, TongSoMauDV));
                baoc.STT = "1";
                List<TKGene> GeneTemp = new List<TKGene>();
                foreach (var t in tabTongsoGene)
                {
                    if (!t.Equals("KXĐ"))
                    {
                        GeneTemp.Add(ThongKeTKExcelGeneDV(t.ToString(), Donvi, TongSoMauDV));
                    }
                }
                baoc.TongGene.AddRange(GeneTemp.OrderByDescending(x=>int.Parse(x.Tong)));
                baoct.Gene.Add(baoc);
                rptBaoCaoExcelGene baoc2 = new rptBaoCaoExcelGene();
                baoc2.NgayIn = DateTime.Now;
                baoc2.TenNhom = "Giới tính";
                baoc2.STT = "2";
                baoc2.NgayIn = DateTime.Now;
                baoc2.TongGene = new List<TKGene>();
                baoc2.TongGene.Add(ThongKeTKExcelDonVi("Tổng", "Giới tính", Donvi));
                baoc2.TongGene.Add(ThongKeTKExcelDonVi("Nam", "Giới tính", Donvi));
                baoc2.TongGene.Add(ThongKeTKExcelDonVi("Nữ", "Giới tính", Donvi));
                baoc2.TongGene.Add(ThongKeTKExcelDonVi("N/A", "Giới tính", Donvi));
                baoct.Gene.Add(baoc2);
                rptBaoCaoExcelGene baoc3 = new rptBaoCaoExcelGene();
                baoc3.NgayIn = DateTime.Now;
                baoc3.TenNhom = "Cân Nặng";
                baoc3.STT = "3";
                baoc3.NgayIn = DateTime.Now;
                baoc3.TongGene = new List<TKGene>();
                baoc3.TongGene.Add(ThongKeTKExcelDonVi("Tổng", "Cân nặng", Donvi));
                baoc3.TongGene.Add(ThongKeTKExcelDonViCanNang(0, 2500,Donvi, TongSoMauDV));
                baoc3.TongGene.Add(ThongKeTKExcelDonViCanNang(2500, 3000,Donvi, TongSoMauDV));
                baoc3.TongGene.Add(ThongKeTKExcelDonViCanNang(3000, 3500,Donvi, TongSoMauDV));
                baoc3.TongGene.Add(ThongKeTKExcelDonViCanNang(3500, 4000, Donvi, TongSoMauDV));
                baoc3.TongGene.Add(ThongKeTKExcelDonViCanNang(4000, 5000, Donvi, TongSoMauDV));
                baoc3.TongGene.Add(ThongKeTKExcelDonViCanNang(5000, 0, Donvi, TongSoMauDV));
                baoct.Gene.Add(baoc3);
                rptBaoCaoExcelGene baoc4 = new rptBaoCaoExcelGene();
                baoc4.NgayIn = DateTime.Now;
                baoc4.TenNhom = "Chất lượng Mẫu lần 1";
                baoc4.STT = "4";
                baoc4.NgayIn = DateTime.Now;
                baoc4.TongGene = new List<TKGene>();
                baoc4.TongGene.Add(ThongKeTKExcelDonVi("Tổng", "Chất lượng mẫu 1", Donvi));
                baoc4.TongGene.Add(ThongKeTKExcelDonVi("Đạt", "Chất lượng mẫu 1", Donvi));
                baoc4.TongGene.Add(ThongKeTKExcelDonVi("Không Đạt", "Chất lượng mẫu 1", Donvi));
                baoct.Gene.Add(baoc4);
                rptBaoCaoExcelGene baoc5 = new rptBaoCaoExcelGene();
                baoc5.NgayIn = DateTime.Now;
                baoc5.TenNhom = "Chất lượng Mẫu lần 2";
                baoc5.STT = "5";
                baoc5.NgayIn = DateTime.Now;
                baoc5.TongGene = new List<TKGene>();
                baoc5.TongGene.Add(ThongKeTKExcelDonVi("Tổng", "Chất lượng mẫu 2", Donvi));
                baoc5.TongGene.Add(ThongKeTKExcelDonVi("Đạt", "Chất lượng mẫu 2", Donvi));
                baoc5.TongGene.Add(ThongKeTKExcelDonVi("Không Đạt", "Chất lượng mẫu 2", Donvi));
                baoct.Gene.Add(baoc5);
                rptBaoCaoExcelGene baoc6 = new rptBaoCaoExcelGene();
                baoc6.NgayIn = DateTime.Now;
                baoc6.TenNhom = "Dân tộc";
                baoc6.STT = "6";
                baoc6.NgayIn = DateTime.Now;
                baoc6.TongGene = new List<TKGene>();
                baoc6.TongGene.Add(ThongKeTKExcelDonVi("Tổng","Dân tộc",Donvi));
                var dsdantoc = (from DataRow myRow in tab.Rows
                                where !string.IsNullOrEmpty(myRow["Dân tộc"].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                select myRow["Dân tộc"]).Distinct().ToList();
                GeneTemp = new List<TKGene>();
                foreach (var dt in dsdantoc)
                {
                    GeneTemp.Add(ThongKeTKExcelDonVi(dt.ToString(), "Dân tộc", Donvi));
                }
                baoc6.TongGene.AddRange(GeneTemp.OrderByDescending( x => int.Parse(x.Tong)));
                baoct.Gene.Add(baoc6);
                Reports.RepostsBaoCao.rptBaoCaoExcelNguyCo datarp = new Reports.RepostsBaoCao.rptBaoCaoExcelNguyCo();
                datarp.Parameters["LuuY"].Value= "(Lưu ý: Báo cáo thống kê có giá trị tại thời điểm xuất báo cáo ngày " + DateTime.Now.Date.ToShortDateString() + ")";
                datarp.Parameters["ThoiGian"].Value = "Từ ngày " + dateEditTuNgay.DateTime.Date.ToShortDateString() + " đến " + dateEditDenNgay.DateTime.Date.ToShortDateString();
                datarp.DataSource = baoct;
                
                string Link = BioNet_Bus.SaveFileReport(FoderLink, "GeneDonVi", Donvi, dateEditTuNgay.DateTime.Date, dateEditDenNgay.DateTime.Date, ".pdf");
                datarp.ExportToPdf(Link);
                int str = datarp.Pages.Count();
                List<TKExcelXNGene> ListExcel = new List<TKExcelXNGene>();
                List<TKExcelXNGene> ListExcelTemp = new List<TKExcelXNGene>();
                var tabDTGene= (from DataRow myRow in tab.Rows
                                        where myRow["Tên Đơn Vị"].Equals(Donvi) && !string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                                        select myRow["Dân tộc"]).Distinct().ToList();

                foreach (var d in tabDTGene)
                {
                    var tabTongGeneDaLam = (from DataRow myRow in tab.Rows
                                            where myRow["Tên Đơn Vị"].Equals(Donvi) && !string.IsNullOrEmpty(myRow["KQ gene"].ToString()) && myRow["KL gene"].Equals("Xac dinh")
                                            && myRow["Dân tộc"].Equals(d.ToString())
                                            select myRow["KQ gene"]).ToList();
                    TKExcelXNGene tKExcel = new TKExcelXNGene();
                    tKExcel.DanToc = d.ToString();
                    tKExcel.Tong= tabTongGeneDaLam.Count().ToString();
                    tKExcel.Viangchan = tabTongGeneDaLam.Count(x => x.Equals("Viangchan")).ToString();
                    tKExcel.Canton = tabTongGeneDaLam.Count(x => x.Equals("Canton")).ToString();
                    tKExcel.Kaiping = tabTongGeneDaLam.Count(x => x.Equals("Kaiping")).ToString();
                    tKExcel.Union = tabTongGeneDaLam.Count(x => x.Equals("Union")).ToString();
                    tKExcel.Coimbra = tabTongGeneDaLam.Count(x => x.Equals("Coimbra")).ToString();
                    tKExcel.Canton_Kaiping = tabTongGeneDaLam.Count(x => x.Equals("Canton + Kaiping")).ToString();
                    tKExcel.Union_Kaiping = tabTongGeneDaLam.Count(x => x.Equals("Union + Kaiping")).ToString();
                    tKExcel.Kaiping_Viangchan = tabTongGeneDaLam.Count(x => x.Equals("Kaiping + Viangchan")).ToString();
                    tKExcel.Canton_Viangchan = tabTongGeneDaLam.Count(x => x.Equals("Canton + Viangchan")).ToString();
                    tKExcel.Union_Viangchan = tabTongGeneDaLam.Count(x => x.Equals("Union + Viangchan")).ToString();
                    tKExcel.Mahidol = tabTongGeneDaLam.Count(x => x.Equals("Mahidol")).ToString();
                    tKExcel.Canton_Coimbra = tabTongGeneDaLam.Count(x => x.Equals("Canton + Coimbra")).ToString();
                    tKExcel.Mediterranean = tabTongGeneDaLam.Count(x => x.Equals("Mediterranean")).ToString();
                    tKExcel.Union_Canton = tabTongGeneDaLam.Count(x => x.Equals("Union + Canton")).ToString();
                    ListExcelTemp.Add(tKExcel);
                }
                ListExcel.AddRange(ListExcelTemp.OrderByDescending(x => int.Parse(x.Tong)).ToList());
                int stt = 1;
                foreach(var l in ListExcel)
                {
                    l.STT = stt++;
                }
                Reports.RepostsBaoCao.rptBaoCaoExcelDanTocGene datarp2 = new Reports.RepostsBaoCao.rptBaoCaoExcelDanTocGene(1);
                datarp2.Parameters["DonVi"].Value = Donvi;
                datarp2.Parameters["LuuY"].Value = "(Lưu ý: Báo cáo thống kê có giá trị tại thời điểm xuất báo cáo ngày " + DateTime.Now.Date.ToShortDateString() + ")";
                datarp2.Parameters["ThoiGian"].Value = "Từ ngày " + dateEditTuNgay.DateTime.Date.ToShortDateString() + " đến " + dateEditDenNgay.DateTime.Date.ToShortDateString();
                datarp2.DataSource = ListExcel;
                string fileName3 =  BioNet_Bus.SaveFileReport(FoderLink, "GeneDonVi", Donvi, DateTime.Now, dateEditDenNgay.DateTime.Date, ".docx");
                datarp2.ExportToDocx(fileName3);
                psReponse.Result = true;
            }
            catch(Exception ex)
            {
                psReponse.Result = false;
                psReponse.StringError = ex.ToString();
            }
            return psReponse;
           
        }
        private TKGene ThongKeTKExcelGeneDV(string cso,string Donvi,int TongSoMauDV)
        {
            TKGene tk = new TKGene();
            try
            {
                var tabTongGene = (from DataRow myRow in tab.Rows
                                   where   myRow["Tên Đơn Vị"].Equals(Donvi)
                                   select myRow["KQ gene"]).ToList();
                var tabNghiNgoGene = (from DataRow myRow in tabNghiNgo.Rows
                                      where  myRow["Tên Đơn Vị"].Equals(Donvi)
                                      select myRow["KQ gene"]).ToList();
                var tabNguyCoCaoTongGene = (from DataRow myRow in tabNguyCoCao.Rows
                                            where myRow["Tên Đơn Vị"].Equals(Donvi)
                                            select myRow["KQ gene"]).ToList();
                var tabNguyCoThapL2Gene = (from DataRow myRow in tabNguycoThap2.Rows
                                           where  myRow["Tên Đơn Vị"].Equals(Donvi)
                                           select myRow["KQ gene"]).ToList();
                var tabAmTinhGene = (from DataRow myRow in tabAmTinh.Rows
                                     where  myRow["Tên Đơn Vị"].Equals(Donvi)
                                     select myRow["KQ gene"]).ToList();
                var tabDuongTinhGene = (from DataRow myRow in tabDuongTinh.Rows
                                        where  myRow["Tên Đơn Vị"].Equals(Donvi)
                                        select myRow["KQ gene"]).ToList();
                if (string.IsNullOrEmpty(cso))
                {
                    tk.Ten = "Tổng đã làm đột biến Gene";
                    tk.Tong = tabTongGene.Where(x => !string.IsNullOrEmpty(x.ToString())).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => !string.IsNullOrEmpty(x.ToString())).Count().ToString();
                    tk.NguyCoCao = tabNguyCoCaoTongGene.Where(x => !string.IsNullOrEmpty(x.ToString())).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => !string.IsNullOrEmpty(x.ToString())).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => !string.IsNullOrEmpty(x.ToString())).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => !string.IsNullOrEmpty(x.ToString())).Count().ToString();
                    tk.Tile = String.Format("{0:0.00}", ((double)int.Parse(tk.Tong) / (double)tabTongGene.Count()) * 100) + "%";
                }
                else if (cso.Equals("Tổng"))
                {
                    tk.Ten = "Tổng";
                    tk.Tong = tabTongGene.Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Count().ToString();
                    tk.NguyCoCao = tabNguyCoCaoTongGene.Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Count().ToString();
                    tk.Tile = String.Format("{0:0.00}", ((double)tabTongGene.Count() / (double)tabTongGene.Count()) * 100) + "%";
                }
                else if (cso.Equals("ChuaXN"))
                {
                    var tkTong = tabTongGene.Where(x => !string.IsNullOrEmpty(x.ToString())).Count();
                    var tabTongChuaLamGene = tabNghiNgoGene.Count()+tabNguyCoCaoTongGene.Count()-tkTong;
                    tk.Ten = "Chưa làm Gene";
                    tk.Tong = tabTongChuaLamGene.ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => x == null || x.Equals("")).Count().ToString();
                    tk.NguyCoCao = tabNguyCoCaoTongGene.Where(x => x == null || x.Equals("")).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => x == null || x.Equals("")).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => x == null || x.Equals("")).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => x == null || x.Equals("")).Count().ToString();
                    tk.Tile = String.Format("{0:0.00}", ((double)int.Parse(tk.Tong) / (double)tabTongGene.Count()) * 100) + "%";
                }
                else if (cso.Equals("KXĐ") || cso.Equals("Xac dinh"))
                {
                    var tkTong = tabTongGene.Where(x => !string.IsNullOrEmpty(x.ToString())).Count().ToString();
                    var tabTongGeneKL = (from DataRow myRow in tab.Rows
                                         where !string.IsNullOrEmpty(myRow["KL gene"].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                         select myRow["KL gene"]).ToList();
                    var tabNghiNgoGeneKL = (from DataRow myRow in tabNghiNgo.Rows
                                            where !string.IsNullOrEmpty(myRow["KL gene"].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                            select myRow["KL gene"]).ToList();
                    var tabNgytCoCaoTongGeneKL = (from DataRow myRow in tabNguyCoCao.Rows
                                                  where !string.IsNullOrEmpty(myRow["KL gene"].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                                  select myRow["KL gene"]).ToList();
                    var tabNguyCoThapL2GeneKL = (from DataRow myRow in tabNguycoThap2.Rows
                                                 where !string.IsNullOrEmpty(myRow["KL gene"].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                                 select myRow["KL gene"]).ToList();
                    var tabAmTinhGeneKL = (from DataRow myRow in tabAmTinh.Rows
                                           where !string.IsNullOrEmpty(myRow["KL gene"].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                           select myRow["KL gene"]).ToList();
                    var tabDuongTinhGeneKL = (from DataRow myRow in tabDuongTinh.Rows
                                              where !string.IsNullOrEmpty(myRow["KL gene"].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                              select myRow["KL gene"]).ToList();
                    tk.Ten = cso;
                    tk.Tong = tabTongGeneKL.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGeneKL.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGeneKL.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2GeneKL.Where(x => x.Equals(cso)).Count().ToString();
                    tk.AmTinh = tabAmTinhGeneKL.Where(x => x.Equals(cso)).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGeneKL.Where(x => x.Equals(cso)).Count().ToString();
                    tk.Tile = String.Format("{0:0.00}", ((double)tabTongGeneKL.Where(x => x.Equals(cso)).Count() / (double)int.Parse(tkTong)) * 100) + "%";
                }
                else
                {
                    tk.Ten = cso;
                    var tabTongGeneKL = (from DataRow myRow in tab.Rows
                                         where !string.IsNullOrEmpty(myRow["KL gene"].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                         select myRow["KL gene"]).ToList();
                    var tkTong = tabTongGene.Where(x => !string.IsNullOrEmpty(x.ToString())).Count().ToString();
                    tk.Tong = tabTongGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NguyCoCao = tabNguyCoCaoTongGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.Tile = String.Format("{0:0.00}", ((double)int.Parse(tk.Tong) / (double)int.Parse(tkTong)) * 100) + "%";
                }
            }
            catch
            {

            }
            return tk;
        }

        private TKGene ThongKeTKExcelDonVi(string cso, string CLM,string Donvi)
        {
            TKGene tk = new TKGene();
            try
            {
                var tabTongGene = (from DataRow myRow in tab.Rows
                                   where !string.IsNullOrEmpty(myRow[CLM].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                   select myRow[CLM]).ToList();
                var tabNghiNgoGene = (from DataRow myRow in tabNghiNgo.Rows
                                      where !string.IsNullOrEmpty(myRow[CLM].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                      select myRow[CLM]).ToList();
                var tabNgytCoCaoTongGene = (from DataRow myRow in tabNguyCoCao.Rows
                                            where !string.IsNullOrEmpty(myRow[CLM].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                            select myRow[CLM]).ToList();
                var tabNguyCoThapL2Gene = (from DataRow myRow in tabNguycoThap2.Rows
                                           where !string.IsNullOrEmpty(myRow[CLM].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                           select myRow[CLM]).ToList();
                var tabAmTinhGene = (from DataRow myRow in tabAmTinh.Rows
                                     where !string.IsNullOrEmpty(myRow[CLM].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                     select myRow[CLM]).ToList();
                var tabDuongTinhGene = (from DataRow myRow in tabDuongTinh.Rows
                                        where !string.IsNullOrEmpty(myRow[CLM].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                        select myRow[CLM]).ToList();
                tk.Ten = cso;
                if(cso.Equals("Tổng"))
                {
                    tk.Tong = tabTongGene.Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGene.Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Count().ToString();
                    tk.Tile = "100%";
                }
                else
                {
                    tk.Tong = tabTongGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.Tile = String.Format("{0:0.00}", ((double)tabTongGene.Where(x => x.Equals(cso)).Count() / (double)tabTongGene.Count()) * 100) + "%";
                }
               
            }
            catch
            {

            }
            return tk;
        }

        private TKGene ThongKeTKExcelDonViCanNang(int? min, int? max, string Donvi,int sophieu)
        {
            TKGene tk = new TKGene();
            try
            {
                var tabTongGene = (from DataRow myRow in tab.Rows
                                   where !string.IsNullOrEmpty(myRow["Cân nặng"].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                   select myRow["Cân nặng"]).ToList();
                var tabNghiNgoGene = (from DataRow myRow in tabNghiNgo.Rows
                                      where !string.IsNullOrEmpty(myRow["Cân nặng"].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                      select myRow["Cân nặng"]).ToList();
                var tabNgytCoCaoTongGene = (from DataRow myRow in tabNguyCoCao.Rows
                                            where !string.IsNullOrEmpty(myRow["Cân nặng"].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                            select myRow["Cân nặng"]).ToList();
                var tabNguyCoThapL2Gene = (from DataRow myRow in tabNguycoThap2.Rows
                                           where !string.IsNullOrEmpty(myRow["Cân nặng"].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                           select myRow["Cân nặng"]).ToList();
                var tabAmTinhGene = (from DataRow myRow in tabAmTinh.Rows
                                     where !string.IsNullOrEmpty(myRow["Cân nặng"].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                     select myRow["Cân nặng"]).ToList();
                var tabDuongTinhGene = (from DataRow myRow in tabDuongTinh.Rows
                                        where !string.IsNullOrEmpty(myRow["Cân nặng"].ToString()) && myRow["Tên Đơn Vị"].Equals(Donvi)
                                        select myRow["Cân nặng"]).ToList();

                if (min == 0)
                {
                    tk.Ten = "<=" + max.ToString();
                    tk.Tong = tabTongGene.Where(x => int.Parse(x.ToString()) <= max).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => int.Parse(x.ToString()) <= max).Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGene.Where(x => int.Parse(x.ToString()) <= max).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => int.Parse(x.ToString()) <= max).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => int.Parse(x.ToString()) <= max).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => int.Parse(x.ToString()) <= max).Count().ToString();
                   // tk.Tile = String.Format("{0:0.00}", ((double)tk.Tong.Count() / (double)sophieu) * 100) + "%";
                    tk.Tile = String.Format("{0:0.00}", ((double.Parse(tk.Tong)) / (double)sophieu) * 100) + "%";
                }
                else if (max == 0)
                {
                    if(min==5000)
                    {
                        tk.Ten = "N/A";
                    }
                    else
                    {
                        tk.Ten = ">" + min.ToString();
                    }
                    tk.Tong = tabTongGene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                  //  tk.Tile = String.Format("{0:0.00}", ((double)tk.Tong.Count() / (double)sophieu) * 100) + "%";
                    tk.Tile = String.Format("{0:0.00}", ((double.Parse(tk.Tong)) / (double)sophieu) * 100) + "%";
                }
                else
                {
                    tk.Ten = min.ToString() + "<X<=" + max.ToString();
                    tk.Tong = tabTongGene.Where(x => int.Parse(x.ToString()) <= max && int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => int.Parse(x.ToString()) <= max && int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGene.Where(x => int.Parse(x.ToString()) <= max && int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => int.Parse(x.ToString()) <= max && int.Parse(x.ToString()) > min).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => int.Parse(x.ToString()) <= max && int.Parse(x.ToString()) > min).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => int.Parse(x.ToString()) <= max && int.Parse(x.ToString()) > min).Count().ToString();
                  //  tk.Tile = String.Format("{0:0.00}", ((double)tk.Tong.Count() / (double)sophieu)*100) + "%";
                    tk.Tile = String.Format("{0:0.00}", ((double.Parse(tk.Tong)) / (double)sophieu) * 100) + "%";
                }
            }
            catch
            {

            }
            return tk;
        }

        private void FrmBaoCaoExcelNguyCo_Load(object sender, EventArgs e)
        {
            dateEditDenNgay.DateTime = DateTime.Now;
            dateEditTuNgay.DateTime = DateTime.Now;
        }

        private void btnPDFChiCuc_Click(object sender, EventArgs e)
        {
            string Folder = BioNet_Bus.GetFileReport("Gene", "PDF");
            if (cbbChiCuc.EditValue != null)
            {
                PsReponse reponse = ThongKeChiCuc(cbbChiCuc.EditValue.ToString(), Folder);
                if (reponse.Result != true)
                {
                    XtraMessageBox.Show("Xuất báo cáo lỗi " + reponse.StringError, "Bionet Sàng lọc sơ sinh.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    XtraMessageBox.Show("Xuất báo cáo thành công", "Bionet Sàng lọc sơ sinh.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start(Folder);
                }
            }
            else
            {

                bool kq = true;
                foreach (var dv in lstchicuc)
                {
                    PsReponse reponse = ThongKeChiCuc(dv.ToString(), Folder);
                    if (reponse.Result != true)
                    {
                        kq = false;
                    }
                }
                if (kq)
                {
                    XtraMessageBox.Show("Xuất báo cáo thành công", "Bionet Sàng lọc sơ sinh.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start(Folder);
                }
                else
                {
                    XtraMessageBox.Show("Xuất báo cáo lỗi", "Bionet Sàng lọc sơ sinh.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private PsReponse ThongKeChiCuc(string ChiCuc, string FoderLink)

        {
            PsReponse psReponse = new PsReponse();
            try
            {
                int TongSoMauDV = (from DataRow myRow in tab.Rows
                                   where !string.IsNullOrEmpty(myRow["STT"].ToString()) && myRow["Chi cục"].Equals(ChiCuc)
                                   select myRow).Count();
                TongSoLamGene = (from DataRow myRow in tab.Rows
                                 where !string.IsNullOrEmpty(myRow["KL gene"].ToString()) && myRow["Chi cục"].Equals(ChiCuc)
                                 select myRow["KL gene"]).Count();
                var tabTongGene = (from DataRow myRow in tab.Rows
                                   where !string.IsNullOrEmpty(myRow["KQ gene"].ToString()) && myRow["Chi cục"].Equals(ChiCuc)
                                   select myRow["KQ gene"]).ToList();
                TongSoXD = tabTongGene.Where(x => x.Equals("KXĐ")).Count();

                rptBaoCaoExcelTong baoct = new rptBaoCaoExcelTong();
                baoct.Tieude = ChiCuc;
                baoct.NgayIn = DateTime.Now;
                baoct.Gene = new List<rptBaoCaoExcelGene>();
                rptBaoCaoExcelGene baoc = new rptBaoCaoExcelGene();
                baoc.NgayIn = DateTime.Now;
                baoc.TenNhom = "Thông tin về xét nghiệm gene";
                var tabTongsoGene = (from DataRow myRow in tab.Rows
                                     where !string.IsNullOrEmpty(myRow["KQ gene"].ToString()) && myRow["Chi cục"].Equals(ChiCuc)
                                     select myRow["KQ gene"]).Distinct().ToList();
                baoc.TongGene = new List<TKGene>();
                baoc.TongGene.Add(ThongKeTKExcelGeneChiCuc("Tổng", ChiCuc, TongSoMauDV));
                baoc.TongGene.Add(ThongKeTKExcelGeneChiCuc("ChuaXN", ChiCuc, TongSoMauDV));
                baoc.TongGene.Add(ThongKeTKExcelGeneChiCuc("", ChiCuc, TongSoMauDV));
                baoc.TongGene.Add(ThongKeTKExcelGeneChiCuc("KXĐ", ChiCuc, TongSoMauDV));
                baoc.TongGene.Add(ThongKeTKExcelGeneChiCuc("Xac dinh", ChiCuc, TongSoMauDV));
                baoc.STT = "1";
                List<TKGene> GeneTemp = new List<TKGene>();
                foreach (var t in tabTongsoGene)
                {
                    if (!t.Equals("KXĐ"))
                    {
                        GeneTemp.Add(ThongKeTKExcelGeneChiCuc(t.ToString(), ChiCuc, TongSoMauDV));
                    }
                }
                baoc.TongGene.AddRange(GeneTemp.OrderByDescending(x => int.Parse(x.Tong)));
                baoct.Gene.Add(baoc);
                rptBaoCaoExcelGene baoc2 = new rptBaoCaoExcelGene();
                baoc2.NgayIn = DateTime.Now;
                baoc2.TenNhom = "Giới tính";
                baoc2.STT = "2";
                baoc2.NgayIn = DateTime.Now;
                baoc2.TongGene = new List<TKGene>();
                baoc2.TongGene.Add(ThongKeTKExcelChiCuc("Tổng", "Giới tính", ChiCuc));
                baoc2.TongGene.Add(ThongKeTKExcelChiCuc("Nam", "Giới tính", ChiCuc));
                baoc2.TongGene.Add(ThongKeTKExcelChiCuc("Nữ", "Giới tính", ChiCuc));
                baoc2.TongGene.Add(ThongKeTKExcelChiCuc("N/A", "Giới tính", ChiCuc));
                baoct.Gene.Add(baoc2);
                rptBaoCaoExcelGene baoc3 = new rptBaoCaoExcelGene();
                baoc3.NgayIn = DateTime.Now;
                baoc3.TenNhom = "Cân Nặng";
                baoc3.STT = "3";
                baoc3.NgayIn = DateTime.Now;
                baoc3.TongGene = new List<TKGene>();
                baoc3.TongGene.Add(ThongKeTKExcelChiCuc("Tổng", "Cân nặng", ChiCuc));
                baoc3.TongGene.Add(ThongKeTKExcelChiCucCanNang(0, 2500, ChiCuc, TongSoMauDV));
                baoc3.TongGene.Add(ThongKeTKExcelChiCucCanNang(2500, 3000, ChiCuc, TongSoMauDV));
                baoc3.TongGene.Add(ThongKeTKExcelChiCucCanNang(3000, 3500, ChiCuc, TongSoMauDV));
                baoc3.TongGene.Add(ThongKeTKExcelChiCucCanNang(3500, 4000, ChiCuc, TongSoMauDV));
                baoc3.TongGene.Add(ThongKeTKExcelChiCucCanNang(4000, 5000, ChiCuc, TongSoMauDV));
                baoc3.TongGene.Add(ThongKeTKExcelChiCucCanNang(5000, 0, ChiCuc, TongSoMauDV));
                baoct.Gene.Add(baoc3);
                rptBaoCaoExcelGene baoc4 = new rptBaoCaoExcelGene();
                baoc4.NgayIn = DateTime.Now;
                baoc4.TenNhom = "Chất lượng Mẫu lần 1";
                baoc4.STT = "4";
                baoc4.NgayIn = DateTime.Now;
                baoc4.TongGene = new List<TKGene>();
                baoc4.TongGene.Add(ThongKeTKExcelChiCuc("Tổng", "Chất lượng mẫu 1", ChiCuc));
                baoc4.TongGene.Add(ThongKeTKExcelChiCuc("Đạt", "Chất lượng mẫu 1", ChiCuc));
                baoc4.TongGene.Add(ThongKeTKExcelChiCuc("Không Đạt", "Chất lượng mẫu 1", ChiCuc));
                baoct.Gene.Add(baoc4);
                rptBaoCaoExcelGene baoc5 = new rptBaoCaoExcelGene();
                baoc5.NgayIn = DateTime.Now;
                baoc5.TenNhom = "Chất lượng Mẫu lần 2";
                baoc5.STT = "5";
                baoc5.NgayIn = DateTime.Now;
                baoc5.TongGene = new List<TKGene>();
                baoc5.TongGene.Add(ThongKeTKExcelChiCuc("Tổng", "Chất lượng mẫu 2", ChiCuc));
                baoc5.TongGene.Add(ThongKeTKExcelChiCuc("Đạt", "Chất lượng mẫu 2", ChiCuc));
                baoc5.TongGene.Add(ThongKeTKExcelChiCuc("Không Đạt", "Chất lượng mẫu 2", ChiCuc));
                baoct.Gene.Add(baoc5);
                rptBaoCaoExcelGene baoc6 = new rptBaoCaoExcelGene();
                baoc6.NgayIn = DateTime.Now;
                baoc6.TenNhom = "Dân tộc";
                baoc6.STT = "6";
                baoc6.NgayIn = DateTime.Now;
                baoc6.TongGene = new List<TKGene>();
                baoc6.TongGene.Add(ThongKeTKExcelChiCuc("Tổng", "Dân tộc", ChiCuc));
                var dsdantoc = (from DataRow myRow in tab.Rows
                                where !string.IsNullOrEmpty(myRow["Dân tộc"].ToString()) && myRow["Chi cục"].Equals(ChiCuc)
                                select myRow["Dân tộc"]).Distinct().ToList();
                GeneTemp = new List<TKGene>();
                foreach (var dt in dsdantoc)
                {
                    GeneTemp.Add(ThongKeTKExcelChiCuc(dt.ToString(), "Dân tộc", ChiCuc));
                }
                baoc6.TongGene.AddRange(GeneTemp.OrderByDescending(x => int.Parse(x.Tong)));
                baoct.Gene.Add(baoc6);
                Reports.RepostsBaoCao.rptBaoCaoExcelNguyCo datarp = new Reports.RepostsBaoCao.rptBaoCaoExcelNguyCo();
                datarp.Parameters["LuuY"].Value = "(Lưu ý: Báo cáo thống kê có giá trị tại thời điểm xuất báo cáo ngày " + DateTime.Now.Date.ToShortDateString() + ")";
                datarp.Parameters["ThoiGian"].Value = "Từ ngày " + dateEditTuNgay.DateTime.Date.ToShortDateString() + " đến " + dateEditDenNgay.DateTime.Date.ToShortDateString();
                datarp.DataSource = baoct;

                string Link = BioNet_Bus.SaveFileReport(FoderLink, "GeneChiCuc", ChiCuc, dateEditTuNgay.DateTime.Date, dateEditDenNgay.DateTime.Date, ".pdf");
                datarp.ExportToPdf(Link);
                int str = datarp.Pages.Count();
                List<TKExcelXNGene> ListExcel = new List<TKExcelXNGene>();
                List<TKExcelXNGene> ListExcelTemp = new List<TKExcelXNGene>();
                var tabDTGene = (from DataRow myRow in tab.Rows
                                 where myRow["Chi cục"].Equals(ChiCuc) && !string.IsNullOrEmpty(myRow["KQ gene"].ToString()) && !myRow["KQ gene"].ToString().Equals("KXĐ")
                                 select myRow["Dân tộc"]).Distinct().ToList();

                foreach (var d in tabDTGene)
                {
                    var tabTongGeneDaLam = (from DataRow myRow in tab.Rows
                                            where myRow["Chi cục"].Equals(ChiCuc) && !string.IsNullOrEmpty(myRow["KQ gene"].ToString()) && myRow["KL gene"].Equals("Xac dinh")
                                            && myRow["Dân tộc"].Equals(d.ToString())
                                            select myRow["KQ gene"]).ToList();
                    TKExcelXNGene tKExcel = new TKExcelXNGene();
                    tKExcel.DanToc = d.ToString();
                    tKExcel.Tong = tabTongGeneDaLam.Count().ToString();
                    tKExcel.Viangchan = tabTongGeneDaLam.Count(x => x.Equals("Viangchan")).ToString();
                    tKExcel.Canton = tabTongGeneDaLam.Count(x => x.Equals("Canton")).ToString();
                    tKExcel.Kaiping = tabTongGeneDaLam.Count(x => x.Equals("Kaiping")).ToString();
                    tKExcel.Union = tabTongGeneDaLam.Count(x => x.Equals("Union")).ToString();
                    tKExcel.Coimbra = tabTongGeneDaLam.Count(x => x.Equals("Coimbra")).ToString();
                    tKExcel.Canton_Kaiping = tabTongGeneDaLam.Count(x => x.Equals("Canton + Kaiping")).ToString();
                    tKExcel.Union_Kaiping = tabTongGeneDaLam.Count(x => x.Equals("Union + Kaiping")).ToString();
                    tKExcel.Kaiping_Viangchan = tabTongGeneDaLam.Count(x => x.Equals("Kaiping + Viangchan")).ToString();
                    tKExcel.Canton_Viangchan = tabTongGeneDaLam.Count(x => x.Equals("Canton + Viangchan")).ToString();
                    tKExcel.Union_Viangchan = tabTongGeneDaLam.Count(x => x.Equals("Union + Viangchan")).ToString();
                    tKExcel.Mahidol = tabTongGeneDaLam.Count(x => x.Equals("Mahidol")).ToString();
                    tKExcel.Canton_Coimbra = tabTongGeneDaLam.Count(x => x.Equals("Canton + Coimbra")).ToString();
                    tKExcel.Mediterranean = tabTongGeneDaLam.Count(x => x.Equals("Mediterranean")).ToString();
                    tKExcel.Union_Canton = tabTongGeneDaLam.Count(x => x.Equals("Union + Canton")).ToString();
                    ListExcelTemp.Add(tKExcel);
                }
                ListExcel.AddRange(ListExcelTemp.OrderByDescending(x => int.Parse(x.Tong)).ToList());
                int stt = 1;
                foreach (var l in ListExcel)
                {
                    l.STT = stt++;
                }
                Reports.RepostsBaoCao.rptBaoCaoExcelDanTocGene datarp2 = new Reports.RepostsBaoCao.rptBaoCaoExcelDanTocGene(1);
                datarp2.Parameters["DonVi"].Value = ChiCuc;
                datarp2.Parameters["LuuY"].Value = "(Lưu ý: Báo cáo thống kê có giá trị tại thời điểm xuất báo cáo ngày " + DateTime.Now.Date.ToShortDateString() + ")";
                datarp2.Parameters["ThoiGian"].Value = "Từ ngày " + dateEditTuNgay.DateTime.Date.ToShortDateString() + " đến " + dateEditDenNgay.DateTime.Date.ToShortDateString();
                datarp2.DataSource = ListExcel;
                string fileName3 = BioNet_Bus.SaveFileReport(FoderLink, "GeneChiCuc", ChiCuc, DateTime.Now, dateEditDenNgay.DateTime.Date, ".docx");
                datarp2.ExportToDocx(fileName3);
                psReponse.Result = true;
            }
            catch (Exception ex)
            {
                psReponse.Result = false;
                psReponse.StringError = ex.ToString();
            }
            return psReponse;

        }
        private TKGene ThongKeTKExcelGeneChiCuc(string cso, string Donvi, int TongSoMauDV)
        {
            TKGene tk = new TKGene();
            try
            {
                var tabTongGene = (from DataRow myRow in tab.Rows
                                   where myRow["Chi cục"].Equals(Donvi)
                                   select myRow["KQ gene"]).ToList();
                var tabNghiNgoGene = (from DataRow myRow in tabNghiNgo.Rows
                                      where myRow["Chi cục"].Equals(Donvi)
                                      select myRow["KQ gene"]).ToList();
                var tabNguyCoCaoTongGene = (from DataRow myRow in tabNguyCoCao.Rows
                                            where myRow["Chi cục"].Equals(Donvi)
                                            select myRow["KQ gene"]).ToList();
                var tabNguyCoThapL2Gene = (from DataRow myRow in tabNguycoThap2.Rows
                                           where myRow["Chi cục"].Equals(Donvi)
                                           select myRow["KQ gene"]).ToList();
                var tabAmTinhGene = (from DataRow myRow in tabAmTinh.Rows
                                     where myRow["Chi cục"].Equals(Donvi)
                                     select myRow["KQ gene"]).ToList();
                var tabDuongTinhGene = (from DataRow myRow in tabDuongTinh.Rows
                                        where myRow["Chi cục"].Equals(Donvi)
                                        select myRow["KQ gene"]).ToList();
                if (string.IsNullOrEmpty(cso))
                {
                    tk.Ten = "Tổng đã làm đột biến Gene";
                    tk.Tong = tabTongGene.Where(x => !string.IsNullOrEmpty(x.ToString())).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => !string.IsNullOrEmpty(x.ToString())).Count().ToString();
                    tk.NguyCoCao = tabNguyCoCaoTongGene.Where(x => !string.IsNullOrEmpty(x.ToString())).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => !string.IsNullOrEmpty(x.ToString())).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => !string.IsNullOrEmpty(x.ToString())).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => !string.IsNullOrEmpty(x.ToString())).Count().ToString();
                    tk.Tile = String.Format("{0:0.00}", ((double)int.Parse(tk.Tong) / (double)tabTongGene.Count()) * 100) + "%";
                }
                else if (cso.Equals("Tổng"))
                {
                    tk.Ten = "Tổng";
                    tk.Tong = tabTongGene.Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Count().ToString();
                    tk.NguyCoCao = tabNguyCoCaoTongGene.Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Count().ToString();
                    tk.Tile = String.Format("{0:0.00}", ((double)tabTongGene.Count() / (double)tabTongGene.Count()) * 100) + "%";
                }
                else if (cso.Equals("ChuaXN"))
                {
                    var tkTong = tabTongGene.Where(x => !string.IsNullOrEmpty(x.ToString())).Count();
                    var tabTongChuaLamGene = tabNghiNgoGene.Count() + tabNguyCoCaoTongGene.Count() - tkTong;
                    tk.Ten = "Chưa làm Gene";
                    tk.Tong = tabTongChuaLamGene.ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => x == null || x.Equals("")).Count().ToString();
                    tk.NguyCoCao = tabNguyCoCaoTongGene.Where(x => x == null || x.Equals("")).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => x == null || x.Equals("")).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => x == null || x.Equals("")).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => x == null || x.Equals("")).Count().ToString();
                    tk.Tile = String.Format("{0:0.00}", ((double)int.Parse(tk.Tong) / (double)tabTongGene.Count()) * 100) + "%";
                }
                else if (cso.Equals("KXĐ") || cso.Equals("Xac dinh"))
                {
                    var tkTong = tabTongGene.Where(x => !string.IsNullOrEmpty(x.ToString())).Count().ToString();
                    var tabTongGeneKL = (from DataRow myRow in tab.Rows
                                         where !string.IsNullOrEmpty(myRow["KL gene"].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                         select myRow["KL gene"]).ToList();
                    var tabNghiNgoGeneKL = (from DataRow myRow in tabNghiNgo.Rows
                                            where !string.IsNullOrEmpty(myRow["KL gene"].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                            select myRow["KL gene"]).ToList();
                    var tabNgytCoCaoTongGeneKL = (from DataRow myRow in tabNguyCoCao.Rows
                                                  where !string.IsNullOrEmpty(myRow["KL gene"].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                                  select myRow["KL gene"]).ToList();
                    var tabNguyCoThapL2GeneKL = (from DataRow myRow in tabNguycoThap2.Rows
                                                 where !string.IsNullOrEmpty(myRow["KL gene"].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                                 select myRow["KL gene"]).ToList();
                    var tabAmTinhGeneKL = (from DataRow myRow in tabAmTinh.Rows
                                           where !string.IsNullOrEmpty(myRow["KL gene"].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                           select myRow["KL gene"]).ToList();
                    var tabDuongTinhGeneKL = (from DataRow myRow in tabDuongTinh.Rows
                                              where !string.IsNullOrEmpty(myRow["KL gene"].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                              select myRow["KL gene"]).ToList();
                    tk.Ten = cso;
                    tk.Tong = tabTongGeneKL.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGeneKL.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGeneKL.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2GeneKL.Where(x => x.Equals(cso)).Count().ToString();
                    tk.AmTinh = tabAmTinhGeneKL.Where(x => x.Equals(cso)).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGeneKL.Where(x => x.Equals(cso)).Count().ToString();
                    tk.Tile = String.Format("{0:0.00}", ((double)tabTongGeneKL.Where(x => x.Equals(cso)).Count() / (double)int.Parse(tkTong)) * 100) + "%";
                }
                else
                {
                    tk.Ten = cso;
                    var tabTongGeneKL = (from DataRow myRow in tab.Rows
                                         where !string.IsNullOrEmpty(myRow["KL gene"].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                         select myRow["KL gene"]).ToList();
                    var tkTong = tabTongGene.Where(x => !string.IsNullOrEmpty(x.ToString())).Count().ToString();
                    tk.Tong = tabTongGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NguyCoCao = tabNguyCoCaoTongGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.Tile = String.Format("{0:0.00}", ((double)int.Parse(tk.Tong) / (double)int.Parse(tkTong)) * 100) + "%";
                }
            }
            catch
            {

            }
            return tk;
        }

        private TKGene ThongKeTKExcelChiCuc(string cso, string CLM, string Donvi)
        {
            TKGene tk = new TKGene();
            try
            {
                var tabTongGene = (from DataRow myRow in tab.Rows
                                   where !string.IsNullOrEmpty(myRow[CLM].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                   select myRow[CLM]).ToList();
                var tabNghiNgoGene = (from DataRow myRow in tabNghiNgo.Rows
                                      where !string.IsNullOrEmpty(myRow[CLM].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                      select myRow[CLM]).ToList();
                var tabNgytCoCaoTongGene = (from DataRow myRow in tabNguyCoCao.Rows
                                            where !string.IsNullOrEmpty(myRow[CLM].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                            select myRow[CLM]).ToList();
                var tabNguyCoThapL2Gene = (from DataRow myRow in tabNguycoThap2.Rows
                                           where !string.IsNullOrEmpty(myRow[CLM].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                           select myRow[CLM]).ToList();
                var tabAmTinhGene = (from DataRow myRow in tabAmTinh.Rows
                                     where !string.IsNullOrEmpty(myRow[CLM].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                     select myRow[CLM]).ToList();
                var tabDuongTinhGene = (from DataRow myRow in tabDuongTinh.Rows
                                        where !string.IsNullOrEmpty(myRow[CLM].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                        select myRow[CLM]).ToList();
                tk.Ten = cso;
                if (cso.Equals("Tổng"))
                {
                    tk.Tong = tabTongGene.Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGene.Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Count().ToString();
                    tk.Tile = "100%";
                }
                else
                {
                    tk.Tong = tabTongGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => x.Equals(cso)).Count().ToString();
                    tk.Tile = String.Format("{0:0.00}", ((double)tabTongGene.Where(x => x.Equals(cso)).Count() / (double)tabTongGene.Count()) * 100) + "%";
                }

            }
            catch
            {

            }
            return tk;
        }

        private TKGene ThongKeTKExcelChiCucCanNang(int? min, int? max, string Donvi, int sophieu)
        {
            TKGene tk = new TKGene();
            try
            {
                var tabTongGene = (from DataRow myRow in tab.Rows
                                   where !string.IsNullOrEmpty(myRow["Cân nặng"].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                   select myRow["Cân nặng"]).ToList();
                var tabNghiNgoGene = (from DataRow myRow in tabNghiNgo.Rows
                                      where !string.IsNullOrEmpty(myRow["Cân nặng"].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                      select myRow["Cân nặng"]).ToList();
                var tabNgytCoCaoTongGene = (from DataRow myRow in tabNguyCoCao.Rows
                                            where !string.IsNullOrEmpty(myRow["Cân nặng"].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                            select myRow["Cân nặng"]).ToList();
                var tabNguyCoThapL2Gene = (from DataRow myRow in tabNguycoThap2.Rows
                                           where !string.IsNullOrEmpty(myRow["Cân nặng"].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                           select myRow["Cân nặng"]).ToList();
                var tabAmTinhGene = (from DataRow myRow in tabAmTinh.Rows
                                     where !string.IsNullOrEmpty(myRow["Cân nặng"].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                     select myRow["Cân nặng"]).ToList();
                var tabDuongTinhGene = (from DataRow myRow in tabDuongTinh.Rows
                                        where !string.IsNullOrEmpty(myRow["Cân nặng"].ToString()) && myRow["Chi cục"].Equals(Donvi)
                                        select myRow["Cân nặng"]).ToList();

                if (min == 0)
                {
                    tk.Ten = "<=" + max.ToString();
                    tk.Tong = tabTongGene.Where(x => int.Parse(x.ToString()) <= max).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => int.Parse(x.ToString()) <= max).Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGene.Where(x => int.Parse(x.ToString()) <= max).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => int.Parse(x.ToString()) <= max).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => int.Parse(x.ToString()) <= max).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => int.Parse(x.ToString()) <= max).Count().ToString();
                    // tk.Tile = String.Format("{0:0.00}", ((double)tk.Tong.Count() / (double)sophieu) * 100) + "%";
                    tk.Tile = String.Format("{0:0.00}", ((double.Parse(tk.Tong)) / (double)sophieu) * 100) + "%";
                }
                else if (max == 0)
                {
                    if (min == 5000)
                    {
                        tk.Ten = "N/A";
                    }
                    else
                    {
                        tk.Ten = ">" + min.ToString();
                    }
                    tk.Tong = tabTongGene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => int.Parse(x.ToString()) > min).Count().ToString();
                    //  tk.Tile = String.Format("{0:0.00}", ((double)tk.Tong.Count() / (double)sophieu) * 100) + "%";
                    tk.Tile = String.Format("{0:0.00}", ((double.Parse(tk.Tong)) / (double)sophieu) * 100) + "%";
                }
                else
                {
                    tk.Ten = min.ToString() + "<X<=" + max.ToString();
                    tk.Tong = tabTongGene.Where(x => int.Parse(x.ToString()) <= max && int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NghiNgo = tabNghiNgoGene.Where(x => int.Parse(x.ToString()) <= max && int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NguyCoCao = tabNgytCoCaoTongGene.Where(x => int.Parse(x.ToString()) <= max && int.Parse(x.ToString()) > min).Count().ToString();
                    tk.NguyCoThapL2 = tabNguyCoThapL2Gene.Where(x => int.Parse(x.ToString()) <= max && int.Parse(x.ToString()) > min).Count().ToString();
                    tk.AmTinh = tabAmTinhGene.Where(x => int.Parse(x.ToString()) <= max && int.Parse(x.ToString()) > min).Count().ToString();
                    tk.DuongTinh = tabDuongTinhGene.Where(x => int.Parse(x.ToString()) <= max && int.Parse(x.ToString()) > min).Count().ToString();
                    //  tk.Tile = String.Format("{0:0.00}", ((double)tk.Tong.Count() / (double)sophieu)*100) + "%";
                    tk.Tile = String.Format("{0:0.00}", ((double.Parse(tk.Tong)) / (double)sophieu) * 100) + "%";
                }
            }
            catch
            {

            }
            return tk;
        }

        private void btnBCTongHop_Click(object sender, EventArgs e)
        {
            string Folder = BioNet_Bus.GetFileReport("Gene", "PDF");
            PsReponse reponse= this.ThongKeTrungTam(Folder);
            if (reponse.Result != true)
            {
                XtraMessageBox.Show("Xuất báo cáo lỗi " + reponse.StringError, "Bionet Sàng lọc sơ sinh.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                XtraMessageBox.Show("Xuất báo cáo thành công", "Bionet Sàng lọc sơ sinh.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(Folder);
            }
        }
        private PsReponse ThongKeTrungTam( string FoderLink)

        {
            PsReponse psReponse = new PsReponse();
            try
            {
                rptBaoCaoExcelGeneCoBan baocao = new rptBaoCaoExcelGeneCoBan();
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
                baoct.Tieude = "Trung tâm sàng lọc sơ sinh Bionet (Tổng hợp)";
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
                List<TKGene> GeneTemp = new List<TKGene>();
                foreach (var t in tabTongsoGene)
                {
                    if (!t.Equals("KXĐ"))
                    {
                        GeneTemp.Add(ThongKeTKExcelGene(t.ToString()));
                    }
                }
                baoc.TongGene.AddRange(GeneTemp.OrderByDescending(x => int.Parse(x.Tong)));
                baoct.Gene.Add(baoc);
                rptBaoCaoExcelGene baoc2 = new rptBaoCaoExcelGene();
                baoc2.NgayIn = DateTime.Now;
                baoc2.TenNhom = "Giới tính";
                baoc2.STT = "2";
                baoc2.NgayIn = DateTime.Now;
                baoc2.TongGene = new List<TKGene>();
                baoc2.TongGene.Add(ThongKeTKExcelCLMauTong("Giới tính"));
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
                baoc3.TongGene.Add(ThongKeTKExcelCLMauTong("Cân nặng"));
                baoc3.TongGene.Add(ThongKeTKExcelCanNang(0, 2500));
                baoc3.TongGene.Add(ThongKeTKExcelCanNang(2500, 3000));
                baoc3.TongGene.Add(ThongKeTKExcelCanNang(3000, 3500));
                baoc3.TongGene.Add(ThongKeTKExcelCanNang(3500, 4000));
                baoc3.TongGene.Add(ThongKeTKExcelCanNang(4000, 5000));
                baoc3.TongGene.Add(ThongKeTKExcelCanNang(5000, 0));
                baoct.Gene.Add(baoc3);
                rptBaoCaoExcelGene baoc4 = new rptBaoCaoExcelGene();
                baoc4.NgayIn = DateTime.Now;
                baoc4.TenNhom = "Chất lượng Mẫu lần 1";
                baoc4.STT = "4";
                baoc4.NgayIn = DateTime.Now;
                baoc4.TongGene = new List<TKGene>();
                baoc4.TongGene.Add(ThongKeTKExcelCLMauTong("Chất lượng mẫu 1"));
                baoc4.TongGene.Add(ThongKeTKExcelCLMau("Đạt", "Chất lượng mẫu 1"));
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
                rptBaoCaoExcelGene baoc6 = new rptBaoCaoExcelGene();
                baoc6.NgayIn = DateTime.Now;
                baoc6.TenNhom = "Dân tộc";
                baoc6.STT = "6";
                baoc6.NgayIn = DateTime.Now;
                baoc6.TongGene = new List<TKGene>();
                baoc6.TongGene.Add(ThongKeTKExcelCLMauTong("Dân tộc"));
                var dsdantoc = (from DataRow myRow in tab.Rows
                                where !string.IsNullOrEmpty(myRow["Dân tộc"].ToString())
                                select myRow["Dân tộc"]).Distinct().ToList();
                GeneTemp = new List<TKGene>();
                foreach (var dt in dsdantoc)
                {
                    GeneTemp.Add(ThongKeTKExcelCLMau(dt.ToString(), "Dân tộc"));
                }
                baoc6.TongGene.AddRange(GeneTemp.OrderByDescending(x => int.Parse(x.Tong)));
                baoct.Gene.Add(baoc6);
                Reports.RepostsBaoCao.rptBaoCaoExcelNguyCo datarp = new Reports.RepostsBaoCao.rptBaoCaoExcelNguyCo();
                datarp.Parameters["LuuY"].Value = "(Lưu ý: Báo cáo thống kê có giá trị tại thời điểm xuất báo cáo ngày " + DateTime.Now.Date.ToShortDateString() + ")";
                datarp.Parameters["ThoiGian"].Value = "Từ ngày " + dateEditTuNgay.DateTime.Date.ToShortDateString() + " đến " + dateEditDenNgay.DateTime.Date.ToShortDateString();
                datarp.DataSource = baoct;

                string Link = BioNet_Bus.SaveFileReport(FoderLink, "GeneTrungTam", "Trung tâm Bionet", dateEditTuNgay.DateTime.Date, dateEditDenNgay.DateTime.Date, ".pdf");
                datarp.ExportToPdf(Link);
                int str = datarp.Pages.Count();
                List<TKExcelXNGene> ListExcel = new List<TKExcelXNGene>();
                List<TKExcelXNGene> ListExcelTemp = new List<TKExcelXNGene>();
                var tabDTGene = (from DataRow myRow in tab.Rows
                                 where !string.IsNullOrEmpty(myRow["KQ gene"].ToString()) && myRow["KL gene"].Equals("Xac dinh")
                                 select myRow["Dân tộc"]).Distinct().ToList();

                foreach (var d in tabDTGene)
                {
                    var tabTongGeneDaLam = (from DataRow myRow in tab.Rows
                                            where   !string.IsNullOrEmpty(myRow["KQ gene"].ToString()) && myRow["KL gene"].Equals("Xac dinh")
                                            && myRow["Dân tộc"].Equals(d.ToString())
                                            select myRow["KQ gene"]).ToList();
                    TKExcelXNGene tKExcel = new TKExcelXNGene();
                    tKExcel.DanToc = d.ToString();
                    tKExcel.Tong = tabTongGeneDaLam.Count().ToString();
                    tKExcel.Viangchan = tabTongGeneDaLam.Count(x => x.Equals("Viangchan")).ToString();
                    tKExcel.Canton = tabTongGeneDaLam.Count(x => x.Equals("Canton")).ToString();
                    tKExcel.Kaiping = tabTongGeneDaLam.Count(x => x.Equals("Kaiping")).ToString();
                    tKExcel.Union = tabTongGeneDaLam.Count(x => x.Equals("Union")).ToString();
                    tKExcel.Coimbra = tabTongGeneDaLam.Count(x => x.Equals("Coimbra")).ToString();
                    tKExcel.Canton_Kaiping = tabTongGeneDaLam.Count(x => x.Equals("Canton + Kaiping")).ToString();
                    tKExcel.Union_Kaiping = tabTongGeneDaLam.Count(x => x.Equals("Union + Kaiping")).ToString();
                    tKExcel.Kaiping_Viangchan = tabTongGeneDaLam.Count(x => x.Equals("Kaiping + Viangchan")).ToString();
                    tKExcel.Canton_Viangchan = tabTongGeneDaLam.Count(x => x.Equals("Canton + Viangchan")).ToString();
                    tKExcel.Union_Viangchan = tabTongGeneDaLam.Count(x => x.Equals("Union + Viangchan")).ToString();
                    tKExcel.Mahidol = tabTongGeneDaLam.Count(x => x.Equals("Mahidol")).ToString();
                    tKExcel.Canton_Coimbra = tabTongGeneDaLam.Count(x => x.Equals("Canton + Coimbra")).ToString();
                    tKExcel.Mediterranean = tabTongGeneDaLam.Count(x => x.Equals("Mediterranean")).ToString();
                    tKExcel.Union_Canton = tabTongGeneDaLam.Count(x => x.Equals("Union + Canton")).ToString();
                    ListExcelTemp.Add(tKExcel);
                }
                ListExcel.AddRange(ListExcelTemp.OrderByDescending(x => int.Parse(x.Tong)).ToList());
                int stt = 1;
                foreach (var l in ListExcel)
                {
                    l.STT = stt++;
                }
                Reports.RepostsBaoCao.rptBaoCaoExcelDanTocGene datarp2 = new Reports.RepostsBaoCao.rptBaoCaoExcelDanTocGene(1);
                datarp2.Parameters["DonVi"].Value = "Trung tâm sàng lọc sơ sinh Bionet(Tổng hợp)";
                datarp2.Parameters["LuuY"].Value = "(Lưu ý: Báo cáo thống kê có giá trị tại thời điểm xuất báo cáo ngày " + DateTime.Now.Date.ToShortDateString() + ")";
                datarp2.Parameters["ThoiGian"].Value = "Từ ngày " + dateEditTuNgay.DateTime.Date.ToShortDateString() + " đến " + dateEditDenNgay.DateTime.Date.ToShortDateString();
                datarp2.DataSource = ListExcel;
               
                string fileName3 = BioNet_Bus.SaveFileReport(FoderLink, "GeneTrungTam", "Trung tâm sàng lọc sơ sinh Bionet(Tổng hợp)", DateTime.Now, dateEditDenNgay.DateTime.Date, ".docx");
                datarp2.ExportToDocx(fileName3);
                psReponse.Result = true;
            }
            catch (Exception ex)
            {
                psReponse.Result = false;
                psReponse.StringError = ex.ToString();
            }
            return psReponse;

        }
    }
}
