using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace BioNetSangLocSoSinh.Reports
{
    public partial class rptPhieuTraKetQua_TheoTT2 : DevExpress.XtraReports.UI.XtraReport
    {
        public rptPhieuTraKetQua_TheoTT2()
        {
            InitializeComponent();
        }

        private void rptPhieuTraKetQua_TheoTT2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (txtNguyCo.Text.ToLower().Equals("true"))
            {
                this.txtKetLuan.Font = new Font("Times New Roman", 10f, FontStyle.Italic | FontStyle.Bold);
                this.txtKetLuan.ForeColor = System.Drawing.Color.Red;
                this.txtGiaTri.Font = new Font("Times New Roman", 10f, FontStyle.Bold);
            }
            else
            {
                this.txtKetLuan.Font = new Font("Times New Roman", 10f);
                this.txtGiaTri.Font = new Font("Times New Roman", 10f);
            }
        }

        private void xrTable3_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (txtNguyCo.Text.ToLower().Equals("true"))
            {
                this.txtKetLuan.Font = new Font("Times New Roman", 10f, FontStyle.Bold);
                // this.txtKetLuan.ForeColor = System.Drawing.Color.Red;
                this.txtGiaTri.Font = new Font("Times New Roman", 10f, FontStyle.Bold);
            }
            else
            {
                this.txtKetLuan.Font = new Font("Times New Roman", 10f);
                this.txtKetLuan.ForeColor = System.Drawing.Color.Black;
                this.txtGiaTri.Font = new Font("Times New Roman", 10f);
            }
            if (txtMaDV.Text.Equals("DVXN00006"))
            {
                txtGiaTri.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                txtGiaTri.Text = string.Empty;
                txtDVDo.Text = string.Empty;
                txtTenDichVu.Text = txtTenDichVu.Text + "\r\n *Không có giá trị sau 3 tháng tuổi.";
                txtNguongBT.Text = string.Empty;
                txtDVDo.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                txtKetLuan.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            }
            else
            {

            }
        }
    }
}
