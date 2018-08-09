using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using BioNetModel.Data;
using System.Collections.Generic;
using BioNetBLL;
using System.Linq;

namespace BioNetSangLocSoSinh.Reports.RepostsCapMaXetNghiep
{
    public partial class rptReportGanViTriMAYXN01 : DevExpress.XtraReports.UI.XtraReport
    {
        int i = 1;
        public rptReportGanViTriMAYXN01()
        {
            InitializeComponent();
        }
        
        private void xrTableCell5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
        private List<PSMapsViTriMayXN> mapViTri = new List<PSMapsViTriMayXN>();
        private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if(!string.IsNullOrEmpty(col_STTVT.Text.ToLower()))
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
                this.xrTable2.BackColor = System.Drawing.Color.White;
            }

            switch(col_MaGoiXN.Text.ToString())
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

        private void rptReportCapMa3Benh_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                mapViTri.AddRange(BioNet_Bus.GetDSMapViTriMayXN("MAYXN01"));
            }
            catch
            {
            }         
        }
    }
}
