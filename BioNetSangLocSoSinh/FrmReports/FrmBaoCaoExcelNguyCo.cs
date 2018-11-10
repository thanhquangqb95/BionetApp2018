using BioNetModel;
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
                {

                }
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
            baocao.TongSoMau = ThongKeTKExcelSoMau(tab);
            baocao.MauNghiNgo = ThongKeTKExcelSoMau(tabNghiNgo);
            baocao.MauNguyCoCao = ThongKeTKExcelSoMau(tabNguyCoCao);
            baocao.MauAMTinh = ThongKeTKExcelSoMau(tabAmTinh);
            baocao.MauDuongTinh = ThongKeTKExcelSoMau(tabDuongTinh);
            baocao.MauNguyCoThapL2 = ThongKeTKExcelSoMau(tabNguycoThap2);
            //Mẫu nghi ngờ
            baocao.TongGene = ThongKeTKExcelGene(tab);
            baocao.GeneTile = ThongKeTKExcelTiLeGene(tab);
            baocao.GeneNghiNgo = ThongKeTKExcelGene(tabNghiNgo);
            baocao.GeneNguycocao = ThongKeTKExcelGene(tabNguyCoCao);
            baocao.GeneNguycothapL2 = ThongKeTKExcelGene(tabNguycoThap2);
            baocao.GeneDuongTinh = ThongKeTKExcelGene(tabDuongTinh);
            baocao.GeneAmTinh = ThongKeTKExcelGene(tabAmTinh);
            try
            {
                Reports.RepostsBaoCao.rptBaoCaoExcelNguyCo datarp = new Reports.RepostsBaoCao.rptBaoCaoExcelNguyCo();
                datarp.DataSource = baocao;
                documentView.DocumentSource = datarp;
            }
            catch
            {

            }
        }
        private TKExcelXNGene ThongKeTKExcelGene(DataTable dataTable)
        {
            TKExcelXNGene tk = new TKExcelXNGene();
            var tabTongGene = (from DataRow myRow in dataTable.Rows
                               where !string.IsNullOrEmpty(myRow["KL gene"].ToString())
                               select myRow["KL gene"]).ToList();
            tk.TongDaLamXNGene = tabTongGene.Count().ToString();
            
            tk.GeneXD = tabTongGene.Where(x=>x.Equals("Xac dinh")).Count().ToString();
            tk.GeneKXD = tabTongGene.Where(x => x.Equals("KXĐ")).Count().ToString();
            var tabKQGene = (from DataRow myRow in dataTable.Rows
                                   where !string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                                   select myRow["KQ gene"]).ToList();
            tk.Conton = tabKQGene.Where(x => x.Equals("Canton")).Count().ToString();
            tk.Kaiping = tabKQGene.Where(x => x.Equals("Kaiping")).Count().ToString();
            tk.Viangchan = tabKQGene.Where(x => x.Equals("Viangchan")).Count().ToString();
            tk.Coimbra = tabKQGene.Where(x => x.Equals("Coimbra")).Count().ToString();
            tk.Union = tabKQGene.Where(x => x.Equals("Union")).Count().ToString();
            tk.Mahidol = tabKQGene.Where(x => x.Equals("Mahidol")).Count().ToString();
            tk.Mediterradean  = tabKQGene.Where(x => x.Equals("Mediterradean")).Count().ToString();
            tk.VanuaLava = tabKQGene.Where(x => x.Equals("VanuaLava")).Count().ToString();
            tk.CantonKaiping = tabKQGene.Where(x => x.Equals("Canton + Kaiping")).Count().ToString();
            tk.CantonViangchang = tabKQGene.Where(x => x.Equals("Canton + Viangchang")).Count().ToString();
            tk.UnionViangchan = tabKQGene.Where(x => x.Equals("Union + Viangchan")).Count().ToString();
            tk.UnionKaiping = tabKQGene.Where(x => x.Equals("Union + Kaiping")).Count().ToString();
            tk.KaipingViangchan = tabKQGene.Where(x => x.Equals("Kaiping + Viangchan")).Count().ToString();
            return tk;
        }
        private TKExcelXNGene ThongKeTKExcelTiLeGene(DataTable dataTable)
        {
            TKExcelXNGene tk = new TKExcelXNGene();
            var tabTongGene = (from DataRow myRow in dataTable.Rows
                               where !string.IsNullOrEmpty(myRow["KL gene"].ToString())
                               select myRow["KL gene"]).ToList();
            tk.TongDaLamXNGene = String.Format("{0:00}", ((double)tabTongGene.Count()/ (double)TongSoLamGene) * 100) + "%"; 
            tk.GeneXD = String.Format("{0:00}", ((double)tabTongGene.Where(x => x.Equals("Xac dinh")).Count() / (double)TongSoLamGene) * 100) + "%";
            tk.GeneKXD = String.Format("{0:00}", ((double)tabTongGene.Where(x => x.Equals("KXĐ")).Count() / (double)TongSoLamGene) * 100) + "%";
            var tabKQGene = (from DataRow myRow in dataTable.Rows
                             where string.IsNullOrEmpty(myRow["KQ gene"].ToString())
                             select myRow["KQ gene"]).ToList();
            tk.Conton = String.Format("{0:00}", ((double)tabKQGene.Where(x => x.Equals("Conton")).Count() / (double)TongSoXD) * 100) + "%";
            tk.Kaiping = String.Format("{0:00}", ((double)tabKQGene.Where(x => x.Equals("Kaiping")).Count() / (double)TongSoXD) * 100) + "%";
            tk.Viangchan = String.Format("{0:00}", ((double)tabKQGene.Where(x => x.Equals("Viangchan")).Count() / (double)TongSoXD) * 100) + "%";
            tk.Coimbra = String.Format("{0:00}", ((double)tabKQGene.Where(x => x.Equals("Coimbra")).Count() / (double)TongSoXD) * 100) + "%";
            tk.Union = String.Format("{0:00}", ((double)tabKQGene.Where(x => x.Equals("Union")).Count() / (double)TongSoXD) * 100) + "%";
            tk.Mahidol = String.Format("{0:00}", ((double)tabKQGene.Where(x => x.Equals("Mahidol")).Count() / (double)TongSoXD) * 100) + "%";
            tk.Mediterradean = String.Format("{0:00}", ((double)tabKQGene.Where(x => x.Equals("Mediterradean")).Count()/ (double)TongSoXD) * 100) + "%";
            tk.VanuaLava = String.Format("{0:00}", ((double)tabKQGene.Where(x => x.Equals("VanuaLava")).Count() / (double)TongSoXD) * 100) + "%";
            tk.CantonKaiping = String.Format("{0:00}", ((double)tabKQGene.Where(x => x.Equals("Canton + Kaiping")).Count() / (double)TongSoXD) * 100) + "%";
            tk.CantonViangchang = String.Format("{0:00}", ((double)tabKQGene.Where(x => x.Equals("Canton + Viangchang")).Count() / (double)TongSoXD) * 100) + "%";
            tk.UnionViangchan = String.Format("{0:00}", ((double)tabKQGene.Where(x => x.Equals("Union + Viangchan")).Count() / (double)TongSoXD) * 100) + "%";
            tk.UnionKaiping = String.Format("{0:00}", ((double)tabKQGene.Where(x => x.Equals("Union + Kaiping")).Count() / (double)TongSoXD) * 100) + "%";
            tk.KaipingViangchan = String.Format("{0:00}", ((double)tabKQGene.Where(x => x.Equals("Kaiping + Viangchan")).Count() / (double)TongSoXD) * 100) + "%";
            return tk;
        }
        private int TongSoMau = 0;
        private int TongSoLamGene = 0;
        private int TongSoXD = 0;
        private TKExcelSoMau ThongKeTKExcelSoMau(DataTable dataTable)
        {
            TKExcelSoMau tk = new TKExcelSoMau();
            var tabTong = (from DataRow myRow in dataTable.Rows
                           where !string.IsNullOrEmpty(myRow["STT"].ToString())
                           select myRow).ToList();
            tk.SoLuong = tabTong.Count().ToString();
            tk.Tile = String.Format("{0:00}",((double)tabTong.Count()/ (double)TongSoMau)*100)+ "%";
            var tabTongGene = (from DataRow myRow in dataTable.Rows
                               where !string.IsNullOrEmpty(myRow["KL gene"].ToString())
                               select myRow["KL gene"]).ToList();
            tk.SLLamGene = tabTongGene.Count().ToString();
            var tabTongChuaGene = (from DataRow myRow in dataTable.Rows
                                   where string.IsNullOrEmpty(myRow["KL gene"].ToString())
                                   select myRow["KL gene"]).ToList();
            tk.SLChuaLamGene = tabTongChuaGene.Count().ToString();
            return tk;

        }
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
    }
}
