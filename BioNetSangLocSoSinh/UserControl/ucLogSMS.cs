using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BioNetBLL;

namespace BioNetSangLocSoSinh.UserControl
{
    public partial class ucLogSMS : DevExpress.XtraEditors.XtraForm
    
    {
        private string maPhieu,  maKhachHang, sdt, dv;

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public ucLogSMS(string MaPhieu, string MaKhachHang,string SDT,string DV)
        {
            InitializeComponent();
            maPhieu = MaPhieu;
            maKhachHang = MaKhachHang;
            sdt = SDT;dv = DV;
        }

        private void sốMẹToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ucLogSMS_Load(object sender, EventArgs e)
        {
            this.rtbStatus.SelectionColor = Color.LightYellow;
            this.rtbStatus.AppendText(string.Concat(new object[] { BioNet_Bus.GetLogSMS(maPhieu, maKhachHang, sdt, dv) }));
          
        }
    }
}
