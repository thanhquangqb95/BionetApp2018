using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace BioNetSangLocSoSinh.FrmReports
{
    public partial class FrmBaoCaoTaiChinh : DevExpress.XtraEditors.XtraForm
    {
        public FrmBaoCaoTaiChinh()
        {
            InitializeComponent();
        }

        private void FrmBaoCaoChiTiet_TrungTam_Load(object sender, EventArgs e)
        {
            UserControl.ucBaoCaoTaiChinhDonVi urc = new UserControl.ucBaoCaoTaiChinhDonVi();
            urc.Dock = DockStyle.Fill;
            this.Controls.Clear();
            this.Controls.Add(urc);

        }
    }
}