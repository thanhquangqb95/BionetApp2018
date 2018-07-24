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
    public partial class FrmNotData : DevExpress.XtraEditors.XtraForm
    {
        public FrmNotData(string erros)
        {
            InitializeComponent();
            this.txtErros.Text = erros;
        }
    }
}