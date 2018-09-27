﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BioNetBLL;

namespace BioNetSangLocSoSinh.DiaglogFrm
{
    public partial class FrmYeuCauMatKhauXacThuc : DevExpress.XtraEditors.XtraForm
    {
        public FrmYeuCauMatKhauXacThuc(string maPhieu,string maDonVi,string maNVHuyMau)
        {
            this.maNV = maNVHuyMau;
            this.maDonVi = maDonVi;
            this.maPhieu = maPhieu;
            InitializeComponent();
        }
        private string maNV = string.Empty;
        private string maPhieu = string.Empty;
        private string maDonVi = string.Empty;

        private void butOK_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(this.txtPassword.Text.Trim()))
            {
                XtraMessageBox.Show("Vui lòng nhập mật khẩu!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(string.IsNullOrEmpty(this.txtLyDo.Text.Trim()))
            {
                XtraMessageBox.Show("Vui lòng nhập lý do hủy mẫu!", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
           
                string pass = BioBLL.GetMD5(this.txtPassword.Text.Trim());
            if (BioNet_Bus.XacThucNhanVien(this.maNV, pass))
            {
                if (XtraMessageBox.Show("Bạn có chắn chắn muốn xóa mẫu này không?", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                   // var res = BioNet_Bus.HuyMauPhieu(maPhieu, maTiepNhan, maDonVi, maNV, this.txtLyDo.Text.Trim());
                    List<String> MaPhieu = new List<string>();
                    MaPhieu.Add(maPhieu);
                    var res = BioNet_Bus.DeletePhieu(MaPhieu, false,this.maNV,this.txtLyDo.Text.TrimEnd());
                    if (res.Result)
                    {
                        XtraMessageBox.Show("Đã hủy mẫu của phiếu " + this.maPhieu, "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        XtraMessageBox.Show(res.StringError, "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        DialogResult = DialogResult.Cancel;
                        this.Close();
                    }
                }
                else
                {
                    DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            else
            {
                XtraMessageBox.Show("Mật khẩu không đúng, vui lòng thử lại hoặc hủy bỏ", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void butCANCEL_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}