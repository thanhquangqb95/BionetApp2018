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

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmViewTTPhieu : DevExpress.XtraEditors.XtraForm
    {
        public FrmViewTTPhieu()
        {
            InitializeComponent();
        }

        private void FrmViewTTPhieu_Load(object sender, EventArgs e)
        {
            AddItemForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TimKiem(txtMaPhieu.Text.TrimEnd());
        }

        private void txtMaPhieu_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                TimKiem(txtMaPhieu.Text.Trim());
            }
        }
        private void TimKiem(string MaPhieu)
        {
            try
            {
                Reports.rptPhieuViewTT datarp = new Reports.rptPhieuViewTT();
                if (!string.IsNullOrEmpty(MaPhieu))
                {
                    PsRptViewTT kq = BioNet_Bus.GetDuLieuViewTT(MaPhieu);
                    if (kq == null)
                    {
                        MessageBox.Show("Mã phiếu không tồn tại", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK);
                       // panelControl3.Controls.Clear();
                    }
                    else
                    {
                        datarp.DataSource = kq.chitietKetQua;
                        datarp.Parameters["TenTre"].Value = kq.TenTre;
                        datarp.Parameters["CanNang"].Value = kq.CanNang;
                        datarp.Parameters["NgaySinh"].Value = kq.NgaySinh;
                        datarp.Parameters["GioiTinh"].Value = kq.GioiTinh;
                        datarp.Parameters["CanNang"].Value = kq.CanNang;
                        datarp.Parameters["TuoiThai"].Value = kq.TuoiThai;
                        datarp.Parameters["TenMe"].Value = kq.TenMe;
                        datarp.Parameters["DienThoaiMe"].Value = kq.DienThoaiMe;
                        datarp.Parameters["TenCha"].Value = kq.TenCha;
                        datarp.Parameters["DienThoaiCha"].Value = kq.DienThoaiCha;
                        datarp.Parameters["TenDonVi"].Value = kq.TenDonVi;
                        datarp.Parameters["MaDonVi"].Value = kq.MaDonVi;
                        datarp.Parameters["DiaChiDonVi"].Value = kq.DiaChiDonVi;
                        datarp.Parameters["DiaChiTre"].Value = kq.DiaChiTre;
                        datarp.Parameters["MaPhieu"].Value = kq.MaPhieu;
                        datarp.Parameters["MaKhachHang"].Value = kq.MaKhachHang;
                        datarp.Parameters["MaXetNghiem"].Value = kq.MaXetNghiem;
                        datarp.Parameters["NgayThuMau"].Value = kq.NgayThuMau;
                        datarp.Parameters["NgayNhanMau"].Value = kq.NgayNhanMau;
                        datarp.Parameters["NgayXetNghiem"].Value = kq.NgayXetNghiem;
                        datarp.Parameters["NgayCoKQ"].Value = kq.NgayCoKQ;
                        datarp.Parameters["PPSinh"].Value = kq.PPSinh;
                        datarp.Parameters["Para"].Value = kq.Para;
                        datarp.Parameters["GoiXN"].Value = kq.GoiXN;
                        datarp.Parameters["CTSangLoc"].Value = kq.CTSangLoc;
                        datarp.Parameters["NguoiLayMau"].Value = kq.NguoiLayMau;
                        datarp.Parameters["LyDoKoDat"].Value = kq.LyDoKoDat;
                        datarp.Parameters["CLuongMau"].Value = kq.CLuongMau;
                        datarp.Parameters["GhiChu"].Value = kq.GhiChu;
                        datarp.Parameters["KetLuanNguyCoCao"].Value = kq.KetLuanNguyCoCao;
                        datarp.Parameters["KetLuanBinhThuong"].Value = kq.KetLuanBinhThuong;
                        datarp.Parameters["NSCha"].Value = "NS Cha: " + kq.NSCha;
                        datarp.Parameters["NSMe"].Value = "NS Mẹ: " + kq.NSMe;
                        datarp.CreateDocument(true);
                        documentView.DocumentSource = datarp;
                        //Reports.frmDanhSachDaCapMa myForm = new Reports.frmDanhSachDaCapMa(datarp);
                        //myForm.TopLevel = false;
                        //myForm.AutoScroll = true;
                        //myForm.ShowIcon = false;
                        //myForm.ControlBox = false;
                        //myForm.Text = "";
                        //myForm.ShowInTaskbar = false;
                        //panelControl3.Controls.Add(myForm);
                        //myForm.Dock = DockStyle.Fill;
                        //myForm.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Nhập mã phiếu cần tra cứu", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK);
                    panelControl3.Controls.Clear();
                }
            }
            catch
            {
                MessageBox.Show("Lỗi hiện thị thông tin phiếu", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }         
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
        }
    }
}