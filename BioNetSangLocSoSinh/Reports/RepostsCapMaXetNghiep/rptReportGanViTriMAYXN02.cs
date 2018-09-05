using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using BioNetBLL;
using BioNetModel.Data;
using System.Collections.Generic;
using System.Linq;

namespace BioNetSangLocSoSinh.Reports.RepostsCapMaXetNghiep
{
    public partial class rptReportGanViTriMAYXN02 : DevExpress.XtraReports.UI.XtraReport
    {
        public rptReportGanViTriMAYXN02()
        {
            InitializeComponent();
        }
        Color mau1 = System.Drawing.Color.SkyBlue;
        Color mau2 = System.Drawing.Color.Thistle;
        private List<PSMapsViTriMayXN> mapViTri = new List<PSMapsViTriMayXN>();
        private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!string.IsNullOrEmpty(col_STTVT.Text.ToLower()))
            {
                col_ViTriThat.Text = mapViTri.FirstOrDefault(x => x.STT == long.Parse(col_STTVT.Text.ToString())).TenViTri;
            }
            else
            {
                col_ViTriThat.Text = "";
            }

            if (col_isTest.Text.ToLower().Equals("true"))
            {
                this.xrTable2.BackColor = System.Drawing.Color.PeachPuff;
            }
            else
            {
                if(int.Parse(col_ViTriThat.Text.Substring(1))%2==0)
                {
                    this.xrTable2.BackColor = System.Drawing.Color.SkyBlue;
                }
                else
                {
                    this.xrTable2.BackColor = System.Drawing.Color.Thistle;
                }
                
            }

            switch (col_MaGoiXN.Text.ToString())
            {
                case "DVGXN0004":
                    col_MaGoiXNTV.Text = "5Benh";
                    break;
                case "DVGXN0003":
                    col_MaGoiXNTV.Text = "3Benh";
                    break;
                case "DVGXN0002":
                    col_MaGoiXNTV.Text = "2Benh";
                    break;
                case "DVGXN0001":
                    col_MaGoiXNTV.Text = "XNL";
                    break;
                case "DVGXNL2":
                    col_MaGoiXNTV.Text = "XNL2";
                    break;
                case "DVTest":
                    col_MaGoiXNTV.Text = "Test";
                    break;
                default:
                    col_MaGoiXNTV.Text = "KXD";
                    break;
            }
        }

        private void rptReportGanViTriMAYXN02_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                mapViTri.AddRange(BioNet_Bus.GetDSMapViTriMayXN("MAYXN02"));
            }
            catch
            {
            }
        }
    }
}
