using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using DevExpress.XtraCharts;
using BioNetModel;

namespace BioNetSangLocSoSinh.Reports
{
    public partial class rptBaocaoTrungTamSoBo : DevExpress.XtraReports.UI.XtraReport
    {
        public rptBaocaoTrungTamSoBo()
        {
            InitializeComponent();
            
        }
     
       

        private void rptBaocaoTrungTamSoBo_ParametersRequestBeforeShow(object sender, DevExpress.XtraReports.Parameters.ParametersRequestEventArgs e)
        {
           
        }

        private void xrTableRow21_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
