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
using BioNetModel.Data;
using BioNetBLL;

namespace BioNetSangLocSoSinh.DiaglogFrm
{
    public partial class FrmMenuUser : DevExpress.XtraEditors.XtraForm
    {
        public FrmMenuUser( PsEmployeeLogin Emp)
        {
            InitializeComponent();
            emp = Emp;
        }
        private PsEmployeeLogin emp=new PsEmployeeLogin();
        private PSEmployee PSEmp = new PSEmployee();
        private Boolean edpass = false;
     

        private void FrmMenuUser_Load(object sender, EventArgs e)
        {
            this.LoadText();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(edpass)
            {
                DialogResult dialog = MessageBox.Show("Bạn có chắc sẽ thay đổi thông tin mật khẩu?", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dialog == DialogResult.OK)
                {
                    this.EditPassWord();
                }
            }
            else
            {
                DialogResult dialog = MessageBox.Show("Bạn có chắc sẽ thay đổi thông tin tài khoản?", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dialog == DialogResult.OK)
                {
                    SaveEmployee();
                }
            } 
        }
        private void SaveEmployee()
        {
            PSEmp.EmployeeName= txtName.Text;
            PSEmp.Address = txtDiaChi.Text;
            PSEmp.Birthday= DateTime.Parse(txtNgaySinh.Text);
            PSEmp.Mobile=txtSDT.Text;
            PSEmp.IDCard= txtCMND.Text;
            PSEmp.Username=txtNameUser.Text ;
            PSEmp.Sex= int.Parse(cbbGioiTinh.EditValue.ToString());
            BioBLL.UpdEmployee(PSEmp);
            MessageBox.Show("Cập nhật thông tin thành công.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void EditPassWord()
        {
            
            PSEmp = BioBLL.GetEmployeeByCode(emp.EmployeeCode);
            if (PSEmp.Password != BioBLL.GetMD5(txtMatKhau.Text))
            {
                XtraMessageBox.Show("Sai mật khẩu cũ. Vui lòng nhập lại!", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhau.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtMatKhauEdit.Text))
            {
                XtraMessageBox.Show("Không được để trống mật khẩu mới", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhauEdit.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtMatKhauEditCopy.Text))
            {
                XtraMessageBox.Show("Không được để trống nhập lại mật khấu mới", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhauEditCopy.Focus();
                return;
            }
            if (txtMatKhauEdit.Text != txtMatKhauEditCopy.Text)
            {
                XtraMessageBox.Show("Nhập lại mật khẩu mới không trùng nhau", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhauEditCopy.Focus();
                return;
            }
            if (BioBLL.UpdPassEmployee(emp.EmployeeCode, txtMatKhauEditCopy.Text))
            {
                XtraMessageBox.Show("Cập nhật mật khẩu thành công", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.EditPass(true);
                this.LoadText();
            }
            else
                XtraMessageBox.Show("Cập nhật mật khẩu thất bại", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
       
        private void EditPass(bool gt)
        {
            txtMatKhau.Enabled = !gt;
            txtMatKhauEdit.Enabled = !gt;
            txtMatKhauEditCopy.Enabled = !gt;

            txtName.Enabled = gt;
            txtDiaChi.Enabled = gt;
            txtNgaySinh.Enabled = gt;
            txtSDT.Enabled = gt;
            txtCMND.Enabled = gt;
            txtNameUser.Enabled = gt;
            cbbGioiTinh.Enabled = gt;
        }

        private void LoadText()
        {
            PSEmp = BioNetBLL.BioNet_Bus.GetThongTinNhanVien(emp.EmployeeCode);
            txtName.Text = PSEmp.EmployeeName;
            txtMaNV.Text = PSEmp.EmployeeCode;
            txtDiaChi.Text = PSEmp.Address;
            txtNgaySinh.Text = PSEmp.Birthday.Value.ToString("dd/MM/yyyy");
            txtSDT.Text = PSEmp.Mobile;
            txtCMND.Text = PSEmp.IDCard;
            txtNameUser.Text = PSEmp.Username;
            cbbGioiTinh.SelectedIndex = PSEmp.Sex;
        }

        private void btnEditPass_DoubleClick(object sender, EventArgs e)
        {
            this.EditPass(false);
            edpass = true;
        }

        private void btnResetPass_DoubleClick(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("Bạn có muốn thay đổi mật khẩu về mặc định không?", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.No)
            {
                try
                {
                    if (BioBLL.UpdPassEmployee(emp.EmployeeCode, string.Empty))
                        XtraMessageBox.Show("Cập nhật mật khẩu nhân viên thành công!", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        XtraMessageBox.Show("Cập nhật mật khẩu nhân viên thất bại!", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.EditPass(true);
                    this.LoadText();
                }
                catch { return; }
            }
        }

        private void txtMatKhau_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            PSEmployee PSEmp = BioBLL.GetEmployeeByCode(emp.EmployeeCode);
            if (PSEmp.Password != BioBLL.GetMD5(txtMatKhau.Text))
            {
                XtraMessageBox.Show("Sai mật khẩu cũ. Vui lòng nhập lại!", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhau.Focus();
                return;
            }
        }

        private void txtMatKhauEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (string.IsNullOrEmpty(txtMatKhauEdit.Text))
            {
                XtraMessageBox.Show("Không được để trống mật khẩu mới", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhauEdit.Focus();
                return;
            }
        }

        private void txtMatKhauEditCopy_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (txtMatKhauEdit.Text != txtMatKhauEditCopy.Text)
            {
                XtraMessageBox.Show("Nhập lại mật khẩu mới không trùng nhau", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhauEditCopy.Focus();
                return;
            }
        }

        private void cbbGioiTinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbbGioiTinh.EditValue.ToString()=="0")
            {
                PicUser.Image = BioNetSangLocSoSinh.Properties.Resources.girl__1_;
            }
            else
            {
                PicUser.Image = BioNetSangLocSoSinh.Properties.Resources.man__1_;
            }
        }
    }
}