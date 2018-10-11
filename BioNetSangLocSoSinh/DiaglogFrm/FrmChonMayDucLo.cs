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
using BioNetBLL;

namespace BioNetSangLocSoSinh.DiaglogFrm
{
    public partial class FrmChonMayDucLo : DevExpress.XtraEditors.XtraForm
    {
        public FrmChonMayDucLo()
        {
            InitializeComponent();
        }
        public string MaMay = string.Empty;
        private void cbbDanhSachMayDucLo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void FrmChonMayDucLo_Load(object sender, EventArgs e)
        {
            this.cbbDanhSachMayDucLo.Properties.DataSource = BioNet_Bus.GetDanhSachMayDucLo(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            MaMay = string.Empty;
            this.Close();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            MaMay = this.cbbDanhSachMayDucLo.EditValue.ToString();
            this.Close();
        }
    }
}