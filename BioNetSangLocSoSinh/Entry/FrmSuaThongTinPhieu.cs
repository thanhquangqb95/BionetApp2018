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
using BioNetBLL;
using BioNetModel;
using BioNetModel.Data;

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmSuaThongTinPhieu : DevExpress.XtraEditors.XtraForm
    {
        public static PsEmployeeLogin emp = new PsEmployeeLogin();
        private static string MaBenhNhan = string.Empty;
        private static string MaTiepNhan = string.Empty;
        private static DateTime NgayTiepNhan = new DateTime();
        private static List<PSDanhMucGoiDichVuTheoDonVi> list = new List<PSDanhMucGoiDichVuTheoDonVi>();
        PSSuaPhieuTT ttphieugoc = new PSSuaPhieuTT();
        public FrmSuaThongTinPhieu(PsEmployeeLogin EMP)
        {
            InitializeComponent();
            emp = EMP;
        }

        private void FrmSuaThongTinPhieu_Load(object sender, EventArgs e)
        {
            try
            {
                this.cbbChiCuc.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
                this.cbbDonVi.Properties.DataSource = BioNet_Bus.GetDanhSachDonViCoSo();
                this.cbbGoiXN.Properties.DataSource = BioNet_Bus.GetDanhsachGoiDichVuChung();
                this.cbbChiCuc.EditValue = "all";
            }
            catch  { }
        }

        private void txtMaPhieu_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Enter))
                {
                    string MaBN = BioNet_Bus.GetPatientTheoMaPhieu(txtMaPhieu.Text.Trim());
                    if (!String.IsNullOrEmpty(MaBN))
                    {
                        ResetThongTin();
                        LoadThongTin(MaBN,txtMaPhieu.Text.Trim());
                        txtMaPhieu.ResetText();
                    }
                    else
                    {
                        ResetThongTin();
                        XtraMessageBox.Show("Mã phiếu không tồn tại", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch {  }
        }

        private void LoadThongTin(string MaBN,string MaPhieu)
        {
            try
            {
                string MaDV = BioNet_Bus.GetThongTinTiepNhanTheoMaPhieu(MaPhieu).MaDonVi;
                cbbDonVi.EditValue = MaDV;
                var ttphieu = BioNet_Bus.GetThongTinPhieu(MaPhieu, MaDV);
                string  MaTiepNhan= BioNet_Bus.GetThongTinMaTiepNhan(MaPhieu, MaDV);
                cbbTrangThaiPhieu.EditValue = ttphieu.trangThaiMau.ToString();
                txtMaPhieuKQ.Text = MaPhieu;
                cbbGoiXN.EditValue = ttphieu.maGoiXetNghiem;
                txtDiaChiLayMau.EditValue = ttphieu.DiaChiLayMau;
                txtNoiLayMau.EditValue = ttphieu.NoiLayMau;
                dateTiepNhan.EditValue = ttphieu.ngayNhanMau;
                GioTiepNhan.EditValue = ttphieu.ngayNhanMau;
                txtMaPhieuKQ.EditValue = txtMaPhieu.EditValue;
               
                ttphieugoc = new PSSuaPhieuTT();
                ttphieugoc.IDCoSo = ttphieu.maDonViCoSo;
                ttphieugoc.IDGoiXN = ttphieu.maGoiXetNghiem;
                ttphieugoc.DiaChiLayMau = ttphieu.DiaChiLayMau;
                ttphieugoc.NoiLayMau = ttphieu.NoiLayMau;
                ttphieugoc.NgayTiepNhan = ttphieu.ngayNhanMau;
                ttphieugoc.MaBenhNhan = ttphieu.maBenhNhan;
                ttphieugoc.MaTiepNhan = MaTiepNhan;
                ttphieugoc.MaPhieu = MaPhieu;
                ttphieugoc.GiaGoiXN = BioNet_Bus.GetThongTinhChiDinh(ttphieu.maPhieu, MaTiepNhan).DonGia;
                GiaGoiXN.EditValue = BioNet_Bus.GetThongTinhChiDinh(ttphieu.maPhieu, MaTiepNhan).DonGia;
                
            }
            catch { }
        }

        private void ResetThongTin()
        {
            try
            {
                cbbDonVi.EditValue = null;
                cbbTrangThaiPhieu.EditValue = null;
                cbbGoiXN.EditValue = null;
                txtDiaChiLayMau.ResetText();
                txtNoiLayMau.ResetText();
                dateTiepNhan.EditValue = null;
                GioTiepNhan.EditValue = null;
                GiaGoiXN.Text = null;
                txtMaPhieuKQ.ResetText(); 
                txtMatKhau.ResetText();
                txtLyDo.ResetText();
                NgayTiepNhan = new DateTime();
            }
            catch { }
        }

        private void cbbChiCuc_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit sear = sender as SearchLookUpEdit;
                var value = sear.EditValue.ToString();
                this.cbbDonVi.Properties.DataSource = null;
                this.cbbDonVi.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi(value.ToString());
                this.cbbDonVi.EditValue = "all";
            }
            catch { }
        }
        
        private void cbbDonVi_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit sear = sender as SearchLookUpEdit;
                var value = sear.EditValue.ToString();
                var donvi=BioNet_Bus.GetThongTinDonViCoSo(value);
                txtNoiLayMau.Text = donvi.TenDVCS;
                txtDiaChiLayMau.Text = donvi.DiaChiDVCS;
                list = new List<PSDanhMucGoiDichVuTheoDonVi>();
                list= BioNet_Bus.GetDanhsachGoiDichVuTrungTam(value);
                GiaGoiXN.EditValue = list.FirstOrDefault(x => x.IDGoiDichVuChung.Equals(cbbGoiXN.EditValue)).DonGia;
            }
            catch { }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (string.IsNullOrEmpty(txtMatKhau.Text))
                {
                    XtraMessageBox.Show("Yêu cầu nhập mật khẩu tài khoản", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string pass = BioNet_Bus.GetThongTinNhanVien(emp.EmployeeCode).Password;
                    string passNhap = BioBLL.GetMD5(txtMatKhau.Text.Trim());
                    if (pass.Equals(passNhap))
                    {
                        if (!string.IsNullOrEmpty(txtLyDo.Text))
                        {
                            if(XtraMessageBox.Show("Phiếu đã được thay đổi thông tin", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)==DialogResult.OK)
                            {
                                PSSuaPhieuTT ttsuaphieu = new PSSuaPhieuTT();
                                ttsuaphieu.IDCoSo = cbbDonVi.EditValue.ToString();
                                ttsuaphieu.NoiLayMau = txtNoiLayMau.Text;
                                ttsuaphieu.DiaChiLayMau = txtDiaChiLayMau.Text;
                                ttsuaphieu.IDGoiXN = cbbGoiXN.EditValue.ToString();
                                ttsuaphieu.GiaGoiXN = decimal.Parse(GiaGoiXN.EditValue.ToString());
                                NgayTiepNhan = DateTime.ParseExact(dateTiepNhan.Text + " " + GioTiepNhan.Text, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                                ttsuaphieu.NgayTiepNhan = NgayTiepNhan;
                                ttsuaphieu.MaTiepNhan = ttphieugoc.MaTiepNhan;
                                ttsuaphieu.MaBenhNhan = ttphieugoc.MaBenhNhan;
                                ttsuaphieu.MaPhieu = ttphieugoc.MaPhieu;

                                PsReponse reponse = BioNet_Bus.EditThongTinPhieu(ttsuaphieu, ttphieugoc, emp.EmployeeCode, txtLyDo.Text);
                                if (reponse.Result)
                                {
                                    XtraMessageBox.Show("Phiếu đã được thay đổi thông tin", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    XtraMessageBox.Show("Lỗi khi thay đổi thông tin", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            
                        }
                        else
                        {
                            XtraMessageBox.Show("Yêu cầu nhập lý do thay đổi", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("Mật khẩu tài khoản không chính xác", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch {   }
        }

        private void cbbGoiXN_EditValueChanged(object sender, EventArgs e)
        {
            GiaGoiXN.EditValue = list.FirstOrDefault(x => x.IDGoiDichVuChung.Equals(cbbGoiXN.EditValue)).DonGia;
        }

        private void cbbTrangThaiPhieu_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if(cbbTrangThaiPhieu.EditValue!=null)
                {
                    cbbChiCuc.Enabled = true;
                    cbbDonVi.Enabled = true;
                    txtDiaChiLayMau.Enabled = true;
                    txtNoiLayMau.Enabled = true;
                    GiaGoiXN.Enabled = true;
                    dateTiepNhan.Enabled = true;
                    GioTiepNhan.Enabled = true;
                    txtMatKhau.Enabled = true;
                    txtLyDo.Enabled = true;
                }
                else
                {
                    cbbChiCuc.Enabled = false;
                    cbbDonVi.Enabled = false;
                    txtDiaChiLayMau.Enabled = false;
                    txtNoiLayMau.Enabled = false;
                    GiaGoiXN.Enabled = false;
                    dateTiepNhan.Enabled = false;
                    GioTiepNhan.Enabled = false;
                    txtMatKhau.Enabled = false;
                    txtLyDo.Enabled = false;
                }
              
            }
            catch
            {

            }
        }
    }
}