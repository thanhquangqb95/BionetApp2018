using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace BioNetSangLocSoSinh.DiaglogFrm
{
    public partial class FrmWarning :  DevExpress.XtraEditors.XtraForm
    {
        public FrmWarning(string erros)
        {
            InitializeComponent();
            txtErros.Text = erros;
        }

        private void lblTry_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
