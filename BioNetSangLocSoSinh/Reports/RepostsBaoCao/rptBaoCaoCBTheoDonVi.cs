using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace BioNetSangLocSoSinh.Reports.RepostsBaoCao
{
    public partial class rptBaoCaoCBTheoDonVi : DevExpress.XtraReports.UI.XtraReport
    {
        public rptBaoCaoCBTheoDonVi(int i)
        {
            InitializeComponent();
            this.xrPageInfo1.StartPageNumber = i;
        }



        private void xrLabel32_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            switch (xrLabel32.Text.Trim().ToLower())
            {
                case "mẫu đạt chất lượng":
                    {
                        this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                        this.xrLabel32.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        break;
                    }
                case "mẫu không đạt chất lượng":
                    {
                        this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                        this.xrLabel32.Font= new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        break;
                    }
                default:
                    {
                        this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                        this.xrLabel32.Font = new System.Drawing.Font("Tahoma", 8F);
                        break;
                    }
            }
        }
    }
}
