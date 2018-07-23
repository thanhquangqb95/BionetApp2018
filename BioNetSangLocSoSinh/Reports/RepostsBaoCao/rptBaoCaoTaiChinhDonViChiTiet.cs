using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace BioNetSangLocSoSinh.Reports.RepostsBaoCao
{
    public partial class rptBaoCaoTaiChinhDonViChiTiet : DevExpress.XtraReports.UI.XtraReport
    {
        public rptBaoCaoTaiChinhDonViChiTiet(Image img)
        {
            InitializeComponent();
            xrPictureBox1.Image = img;
        }

        private void xrTableCell35_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
