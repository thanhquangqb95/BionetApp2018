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
using BioNetSangLocSoSinh.Reports.RepostsBaoCao;
using System.IO;
using System.Diagnostics;

namespace BioNetSangLocSoSinh.Reports
{
    public partial class frmReportCTTaiChinh : DevExpress.XtraEditors.XtraForm
    {
        public frmReportCTTaiChinh(DevExpress.XtraReports.UI.XtraReport _rpt)
        {
            InitializeComponent();
         
        }
        public static void SaveFilePDF(DevExpress.XtraReports.UI.XtraReport datarp,string TenDV, string Thang,string Nam)
        {
          
            
            string paththumuc = Application.StartupPath + "\\BaoCaoCTTaiChinhDonVi";
            
            if (!System.IO.Directory.Exists(paththumuc))
            {
                Directory.CreateDirectory(paththumuc);
            }
          
                string pathpdf = paththumuc + "\\" + TenDV;
                if (!System.IO.Directory.Exists(pathpdf))
                {
                    Directory.CreateDirectory(pathpdf);
                }
                //Đường dẫn file pdf
                string path = pathpdf + @"\" + "BaoCaoTaiChinh_" + TenDV + "_Thang" + Thang + "Nam" + Nam + ".pdf";
                try
                {
                    //Lưu file pdf phiếu kết quả theo tên mã phiếu
                    datarp.ExportToPdf(path);
                }
                catch
                {
                }

            

        }

  
        
    }
}