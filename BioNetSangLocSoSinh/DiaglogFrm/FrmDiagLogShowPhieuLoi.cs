using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BioNetModel;
using BioNetModel.Data;
using BioNetBLL;

namespace BioNetSangLocSoSinh.DiaglogFrm
{
    public partial class FrmDiagLogShowPhieuLoi : DevExpress.XtraEditors.XtraForm
    {
        public FrmDiagLogShowPhieuLoi(List<PsPhieuLoiKhiDanhGia> lstPhieu)
        {
            InitializeComponent();
            this.lst = lstPhieu;
        }
        private List<PsPhieuLoiKhiDanhGia> lst = new List<PsPhieuLoiKhiDanhGia>();
        private void LoadDanhSach()
        {
            this.gridControl1.DataSource = this.lst;
        }

        private void FrmDiagLogShowPhieuLoi_Load(object sender, EventArgs e)
        {
            this.LoadDanhSach();
            AddItemForm();
        }
        private void AddItemForm()
        {
            PSMenuForm fo = new PSMenuForm
            {
                NameForm = this.Name,
                Capiton = this.Text,
            };
            BioNet_Bus.AddMenuForm(fo);
            long? idfo = BioNet_Bus.GetMenuIDForm(this.Name);
            CustomLayouts.TransLanguage.AddItemCT(this.Controls, idfo);
            CustomLayouts.TransLanguage.Trans(this.Controls, idfo);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}