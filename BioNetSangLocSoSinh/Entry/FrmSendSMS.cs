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
using BioNetModel;

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmSendSMS : DevExpress.XtraEditors.XtraForm
    {
        public FrmSendSMS(PsEmployeeLogin EMP)
        {
            emp = EMP;
            InitializeComponent();
        }
        public static PsEmployeeLogin emp = new PsEmployeeLogin();
        private class VietTat
        {
            public int IDVietTat { get; set; }
            public string KiTu { get; set; }
            public string NoiDung { get; set; }
        }
        private void cbbCTNoiDung_EditValueChanged(object sender, EventArgs e)
        {
            string tam = cbbCTNoiDung.Text.Replace("#maphieu","1234567");
            tam = tam.Replace("#tentre", " TÊN TRẺ A");
            tam= tam.Replace("#tennguoinhan", " MẸ NGUYỄN THỊ B");
            cbbCTNoiDungDemo.Text = tam;
        }
        List<VietTat> lstvt = new List<VietTat>();
        private void FrmSendSMS_Load(object sender, EventArgs e)
        {
            lstvt.Add(new VietTat() { IDVietTat=1,KiTu= "maphieu",NoiDung= "Mã phiếu" });
            lstvt.Add(new VietTat() { IDVietTat = 1, KiTu = "tentre", NoiDung = "Tên trẻ" });
            lstvt.Add(new VietTat() { IDVietTat = 1, KiTu = "tennguoinhan", NoiDung = "Tên người nhân" });



        }
    }
}