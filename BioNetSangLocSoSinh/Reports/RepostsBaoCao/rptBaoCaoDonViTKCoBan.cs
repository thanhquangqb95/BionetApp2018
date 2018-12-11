using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace BioNetSangLocSoSinh.Reports.RepostsBaoCao
{
    public partial class rptBaoCaoDonViTKCoBan : DevExpress.XtraReports.UI.XtraReport
    {
        public rptBaoCaoDonViTKCoBan(int i)
        {
            InitializeComponent();
            this.xrPageInfo1.StartPageNumber = i;
        }

    }
}
