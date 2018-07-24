using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BioNetBLL;
using BioNetModel.Data;
using DevExpress.XtraSplashScreen;

namespace BioNetSangLocSoSinh.DiaglogFrm
{
    public partial class WaitingfromSave : SplashScreen
    {
        public WaitingfromSave()
        {
            InitializeComponent();
        }
        public WaitingfromSave(string noidung)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(noidung))
                this.txtNoiDung.Text = "Đang lưu dữ liệu, vui lòng đợi...";
            else this.txtNoiDung.Text = noidung;
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum SplashScreenCommand
        {
        }


        private void WaitingfromSave_Load(object sender, EventArgs e)
        {
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
    }
}