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
    public partial class Waitingfrom : SplashScreen
    {
        public Waitingfrom()
        {
            InitializeComponent();
        }

    

        public override void ProcessCommand(Enum cmd, object arg)
        {
           
            base.ProcessCommand(cmd, arg);
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

        private void Waitingfrom_Load(object sender, EventArgs e)
        {
            AddItemForm();
        }
    }
}