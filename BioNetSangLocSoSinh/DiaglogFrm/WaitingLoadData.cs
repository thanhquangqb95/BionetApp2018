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
    public partial class WaitingLoadData : SplashScreen
    {
        public WaitingLoadData()
        {
            InitializeComponent();
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
        int gio = 0, phut = 0, giay = 0;
        private void WaitingLoadData_Load(object sender, EventArgs e)
        {
            AddItemForm();
            timer1.Interval = 1000;
            timer1.Start();
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            giay++;
            if(giay==60)
            {
                phut++;
                giay = 0;
            }
            if(phut==60)
            { gio++;
                phut = 0;
            }
            txtTime.Text = gio.ToString() + " : " + phut.ToString() + " : " + giay.ToString();
        }
    }
}