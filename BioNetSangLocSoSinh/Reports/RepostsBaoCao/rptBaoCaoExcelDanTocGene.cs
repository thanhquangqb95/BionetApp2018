using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace BioNetSangLocSoSinh.Reports.RepostsBaoCao
{
    public partial class rptBaoCaoExcelDanTocGene : DevExpress.XtraReports.UI.XtraReport
    {
        public rptBaoCaoExcelDanTocGene(int i)
        {
            InitializeComponent();
            this.xrPageInfo1.StartPageNumber = i;
        }

    }
}
