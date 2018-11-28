using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Diagnostics;

namespace BioNetSangLocSoSinh.Reports
{
    public partial class frmReport : DevExpress.XtraEditors.XtraForm
    {
        private DataSet dsResult = new DataSet();
        private DevExpress.XtraReports.UI.XtraReport rpt = new DevExpress.XtraReports.UI.XtraReport();
        private string fromdate = string.Empty, todate = string.Empty, sheetname = string.Empty;

        private void documentView_Load(object sender, EventArgs e)
        {
            try
            {                
                this.rpt.CreateDocument(true);
                this.documentView.DocumentSource = this.rpt;
                try
                {
                    Process pdfexport = new Process();
                }
                catch (Exception ex) { }
            }
            catch
            { }
        }
        
        public frmReport(DevExpress.XtraReports.UI.XtraReport _rpt)
        {
            InitializeComponent();
            this.rpt = _rpt;
        }
    }
}