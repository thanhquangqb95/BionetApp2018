using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace BioNetSangLocSoSinh.Reports.RepostsCapMaXetNghiep
{
    public partial class rptReportCapMa3Benh : DevExpress.XtraReports.UI.XtraReport
    {
        int i = 1;
        public rptReportCapMa3Benh()
        {
            InitializeComponent();
        }

        private void xrTableCell5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (col_isTest.Text.ToLower().Equals("true"))
            {
                if(col_ViTri.Text.ToString().Equals("A1"))
                {
                    col_Bang.Text = "Đĩa "+i;
                    i++;
                }
                else
                {
                    col_Bang.Text = "";
                }
                this.xrTable2.BackColor = System.Drawing.Color.PeachPuff;
            }
            else
            {
                if (!col_ViTri.Text.ToString().Equals("A1"))
                {
                    col_Bang.Text = "";
                }
                this.xrTable2.BackColor = System.Drawing.Color.White;
            }
            switch (col_MaGoiXN.Text.ToString())
            {
                case "DVGXN0004":
                   col_5Benh.Text="5Benh";
                    col_3Benh.Text = "";
                    col_2Benh.Text = "";
                    col_XNL.Text = "";
                    col_XNKhac.Text = "";
                    break;
                case "DVGXN0003":
                    col_3Benh.Text = "3Benh";
                    col_5Benh.Text = "";
                    col_2Benh.Text = "";
                    col_XNL.Text = "";
                    col_XNKhac.Text = "";
                    break;
                case "DVGXN0002":               
                    col_2Benh.Text = "2Benh";
                    col_3Benh.Text = "";
                    col_5Benh.Text = "";
                    col_XNL.Text = "";
                    col_XNKhac.Text = "";
                    break;
                case "DVGXN0001":
                    col_2Benh.Text = "";
                    col_3Benh.Text = "";
                    col_5Benh.Text = "";
                    col_XNL.Text = "";
                    col_XNKhac.Text = "XNL";
                    break;
                case "DVGXNL2":
                    col_XNL.Text = "XNL2";
                    col_3Benh.Text = "";
                    col_2Benh.Text = "";
                    col_5Benh.Text = "";
                    col_XNKhac.Text = "";
                    break;
                default:
                    col_XNKhac.Text=col_MaGoiXN.Text;
                    col_3Benh.Text = "";
                    col_2Benh.Text = "";
                    col_XNL.Text = "";
                    col_5Benh.Text = "";
                    break;
            }

        }
    }
}
