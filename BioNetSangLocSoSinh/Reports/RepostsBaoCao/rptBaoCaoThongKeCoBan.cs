using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace BioNetSangLocSoSinh.Reports
{
    public partial class rptBaoCaoThongKeCoBan : DevExpress.XtraReports.UI.XtraReport
    {
        public rptBaoCaoThongKeCoBan(Image img)
        {
          
            InitializeComponent();
            xrPictureBox1.Image = img;
        }

    }
}
