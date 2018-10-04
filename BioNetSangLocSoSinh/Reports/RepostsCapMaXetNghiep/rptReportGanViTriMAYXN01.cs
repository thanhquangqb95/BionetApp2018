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
        string a = "0";
        Color mau1= System.Drawing.Color.SkyBlue;
        Color mau2 = System.Drawing.Color.Thistle;
        Color mauuse= System.Drawing.Color.White;
        public rptReportGanViTriMAYXN01()
        {
            InitializeComponent();
        }
        
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
                this.col_MaGoiXNTV.ForeColor= System.Drawing.Color.Black;
                this.col_MaGoiXNTV.Text = "";
            }
            else
            {
                this.xrTable2.BackColor = System.Drawing.Color.Transparent;

                if (!col_ViTriThat.Text.Substring(0, 1).Equals(a))
                {
                    a = col_ViTriThat.Text.Substring(0, 1);
                    mauuse = mau2;
                    mau2 = mau1;
                    mau1 = mauuse;
                }
                this.col_ViTriThat.BackColor = mau1;
                this.col_ViTri.BackColor = mau1;
                if(!col_MaGoiXN.Text.Equals("DVGXN0004"))
                {
                    this.col_MaGoiXNTV.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    this.col_MaGoiXNTV.ForeColor = System.Drawing.Color.Black;
                }
                if(col_MaGoiXN.Text.Equals("DVTest"))
                {
                    col_MaGoiXNTV.Text = "";
                }
                
            }

        }
        private void rptReportGanViTriMAYXN01_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
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
