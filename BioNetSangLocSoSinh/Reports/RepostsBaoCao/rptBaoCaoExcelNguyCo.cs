using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace BioNetSangLocSoSinh.Reports.RepostsBaoCao
{
    public partial class rptBaoCaoExcelNguyCo : DevExpress.XtraReports.UI.XtraReport
    {
        public rptBaoCaoExcelNguyCo()
        {
            InitializeComponent();
        }

        private void xrTable1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            switch (XTong.Text.ToLower())
            {
                case "tổng":
                    {
                        this.xrTable1.BackColor = System.Drawing.Color.Bisque;
                        break;
                    }
                case "chưa làm gene":
                    {
                        this.xrTable1.BackColor = System.Drawing.Color.Gainsboro;
                        break;
                    }
                case "tổng đã làm đột biến gene":
                    {
                        this.xrTable1.BackColor = System.Drawing.Color.Gainsboro;
                        break;
                    }
                case "kxđ":
                    {
                        this.xrTable1.BackColor = System.Drawing.Color.Beige;
                        break;
                    }
                case "xac dinh":
                    {
                        this.xrTable1.BackColor = System.Drawing.Color.Beige;
                        break;
                    }
                default:
                    {
                        this.xrTable1.BackColor = System.Drawing.Color.Transparent;
                        break;
                    }
            }
        }
    }
}
