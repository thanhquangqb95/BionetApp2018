using BioNetBLL;
using BioNetModel;
using BioNetModel.Data;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BioNetSangLocSoSinh.DiaglogFrm
{
    public partial class FrmLoginNew : Form
    {
      
        public bool cancel = false;
        public DateTime dtimeServer = new DateTime();
        public string NameCopany = string.Empty;
        public PsEmployeeLogin emp = new PsEmployeeLogin();
        public FrmLoginNew()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtUsername.Text == string.Empty)
                {
                    lblError.Visible = true;
                    lblError.Text = "Vui lòng nhập tên tài khoản!";
                    this.txtUsername.Focus();
                    return;
                }
                if (this.txtPassword.Text == string.Empty)
                {
                    lblError.Visible = true;
                    lblError.Text = "Vui lòng nhập mật khẩu!";
                    this.txtPassword.Focus();
                    return;
                }
                if (txtUsername.Text != string.Empty && txtPassword.Text != string.Empty)
                {
                    string pass = BioBLL.GetMD5(txtPassword.Text);
                    bool bCheckLogin = BioBLL.CheckLogin(txtUsername.Text.Trim(), pass);
                    if (bCheckLogin)
                    {

                        emp.EmployeeCode = BioBLL.GetEmployeeCode(txtUsername.Text.Trim().TrimEnd());
                        emp.EmployeeName = txtUsername.Text.TrimEnd();
                        //FrmStartup.emp = emp;
                        this.Close();
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "Vui lòng kiểm tra lại tài khoản hoặc mật khẩu!";
                        this.txtUsername.Focus();
                        return;
                    }
                }
            }
            catch 
            {
                lblError.Visible = true;
                lblError.Text = "Vui lòng kiểm tra lại kết nối với Máy chủ!";
                return;
            }
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                this.txtPassword.Focus();
                this.txtPassword.ResetText();
            }              
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                this.btnLogin.Focus();
            }
        }

        private void FrmLoginNew_Load(object sender, EventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string versionCurrent = fileVersionInfo.ProductVersion;
            NameCopany = fileVersionInfo.CompanyName;
            lblUpdate.Text = "Ngày cập nhật: 05/09/2018";
            lblVersion.Text = "SLSS.2.0." + versionCurrent;
            this.lblError.Visible = false;
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

        private void lblExit_Click(object sender, EventArgs e)
        {
            
              Application.Exit();
                               
        }

        private void lblExit_MouseHover(object sender, EventArgs e)
        {
            lblExit.ForeColor = Color.Red;
        }

        private void lblExit_MouseLeave(object sender, EventArgs e)
        {
            this.lblExit.ForeColor = Color.SaddleBrown;
        }

        private void txtPassword_Click(object sender, EventArgs e)
        {
            this.txtPassword.ResetText();
        }
    }
}
