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
        string a = "0";
        Color mau1 = System.Drawing.Color.SkyBlue;
        Color mau2 = System.Drawing.Color.Thistle;
        Color mauuse = System.Drawing.Color.Transparent;
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
                this.col_ViTriThat.BackColor = System.Drawing.Color.PeachPuff;
                this.col_ViTri.BackColor = System.Drawing.Color.PeachPuff;
                this.col_MaGoiXNTV.Text = "";
            }
            else
            {
                this.xrTable2.BackColor = System.Drawing.Color.Transparent;

                if (int.Parse(col_ViTriThat.Text.Substring(1)) % 2 == 0)
                {
                    this.col_ViTriThat.BackColor = mau1;
                    this.col_ViTri.BackColor = mau1;
                }
                else
                {
                    this.col_ViTriThat.BackColor = mau2;
                    this.col_ViTri.BackColor = mau2;
                }
                if (!col_MaGoiXN.Text.Equals("DVGXN0004"))
                {
                    this.col_MaGoiXNTV.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    this.col_MaGoiXNTV.ForeColor = System.Drawing.Color.Black;
                }
                if (col_MaGoiXN.Text.Equals("DVTest"))
                {
                    col_MaGoiXNTV.Text = "";
                }
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
