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

namespace BioNetSangLocSoSinh.DiaglogFrm
{
    public partial class FrmOK : DevExpress.XtraEditors.XtraForm
    {
        public FrmOK(string erros)
        {
            InitializeComponent();
            this.txtErros.Text = erros;
        }

        private void lblTry_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}