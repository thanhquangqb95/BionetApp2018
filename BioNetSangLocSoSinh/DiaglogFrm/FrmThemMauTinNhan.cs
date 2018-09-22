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
using BioNetModel.Data;
using BioNetModel;
using BioNetBLL;
using System.Text.RegularExpressions;

namespace BioNetSangLocSoSinh.DiaglogFrm
{
    public partial class FrmThemMauTinNhan : DevExpress.XtraEditors.XtraForm
    {
        public FrmThemMauTinNhan()
        {
            InitializeComponent();
            this.cbbKieukitu.EditValue = false;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                PSDanhMucMauSMS sms = new PSDanhMucMauSMS();
                sms.RowIDMauSMS = 0;
                sms.NameMauSMS = txtNameMauSMS.Text;
                sms.DoiTuongNhanTN = cbbDoiTuongSMS.EditValue.ToString();
                sms.HinhThucGuiTN = cbbHinhThucSMS.EditValue.ToString();
                sms.NoidungGui = cbbNoiDungSMS.EditValue.ToString();
                sms.MauNoiDungGui = txtCTNoiDungSMS.Text;
                PsReponse reponse = BioNet_Bus.InsertMauSMS(sms);
                if (reponse.Result)
                {
                    XtraMessageBox.Show("Lưu thành công.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("Thêm mẫu tin thất bại.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                XtraMessageBox.Show("Thêm mẫu tin thất bại.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtCTNoiDungSMS_EditValueChanged(object sender, EventArgs e)
        {
            NoiDungDemoSMS();
        }
        private void NoiDungDemoSMS()
        {
            string tam = txtCTNoiDungSMS.Text.Replace("#maphieu", "1234567");
            tam = tam.Replace("#tentre", " TÊN TRẺ A");
            tam = tam.Replace("#tennguoinhan", " MẸ NGUYỄN THỊ B");
            tam = tam.Replace("#trangthaiphieu", "đã có kết quả");
            tam = tam.Replace("#ngaysinh", "01/01/2018");
            tam = tam.Replace("#ketqua", " Nguy co thap(CH,CAH,GAL,PKU), Nguy co cao(G6PD)");
            tam = tam.Replace("#ketluan", "Nguy cơ cao");
            if (!bool.Parse(cbbKieukitu.EditValue.ToString()))
            {
                Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
                tam = tam.Normalize(NormalizationForm.FormD);
                tam = regex.Replace(tam, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            }
            txtAdDemo.Text = tam;
            lblSKTSMS.Text = txtCTNoiDungSMS.Text.Length.ToString() + "/160";
            lblSKTDemoSMS.Text = txtAdDemo.Text.Length.ToString() + "/160";
        }

        private void cbbKieukitu_EditValueChanged(object sender, EventArgs e)
        {
            NoiDungDemoSMS();
        }
    }
}