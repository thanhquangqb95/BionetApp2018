using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using BioNetBLL;
using BioNetModel;
using BioNetModel.Data;
using DevExpress.XtraSplashScreen;
using BioNetSangLocSoSinh.DiaglogFrm;

namespace BioNetSangLocSoSinh.Entry
{
    public partial class FrmInfoPerson : DevExpress.XtraEditors.XtraForm
    {
        private string maBenhNhan = string.Empty;
        public FrmInfoPerson()
        {
            InitializeComponent();
        }
        private List<PSPatient> patientlst = new List<PSPatient>();

        private void LoadDanhSachBenhNhan()
        {
            patientlst = BioBLL.GetDanhSachBenhNhan(int.Parse(this.cbbSL.Text));
            this.gridControl_Info.DataSource = patientlst;
        }
        public class CLGioiTinh
        {
            public string TenGioiTinh { get; set; }
            public string GioiTinh { get; set; }
        }
        private void FrmInfoPerson_Load(object sender, EventArgs e)
        {
            List<CLGioiTinh> CLGioiTinhs = new List<CLGioiTinh>();
            CLGioiTinhs.Add(new CLGioiTinh() { GioiTinh = "0", TenGioiTinh = "Nam" });
            CLGioiTinhs.Add(new CLGioiTinh() { GioiTinh = "1", TenGioiTinh = "Nữ" });
            CLGioiTinhs.Add(new CLGioiTinh() { GioiTinh = "2", TenGioiTinh = "N/A" });
            this.LookUpEditGioiTinh.DataSource = CLGioiTinhs;
            this.LookUpEditGioiTinh.DisplayMember = "TenGioiTinh";
            this.LookUpEditGioiTinh.ValueMember = "GioiTinh";
            this.LoadDanhmuc();
            this.lookUpDanToc.Properties.DataSource = BioNet_Bus.GetDanhSachDanToc(-1);
            btnEdit.Enabled = btnCancel.Enabled = btnSave.Enabled = false;
            this.txtChiCuc.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            this.txtDonVi.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi("all");
            this.txtDonVi.EditValue = "all";
            this.txtChiCuc.EditValue = "all";
            ReadOnlyText(true);
            AddItemForm();
        }
        private void LoadDanhmuc()
        {
            this.cbbSL.EditValue = 50;
            this.cbbTTPhieu.EditValue = 1;
            this.txtGioiTinh.EditValue = 3;
            this.txtSeachGioiTinh.SelectedIndex = 3;
            this.txtSearchNgaySinh.EditValue = null;
            this.txtSearchChaMe.ResetText();
            this.txtSearchTenTre.ResetText();
            this.cbbTTPhieu.EditValue = 0;
            this.btnSendEmail1.ImageOptions.ImageIndex = 0;
            this.btnSendEmail2.ImageOptions.ImageIndex = 0;
            this.btnSendEmail1.Enabled = false;
            this.btnSendEmail2.Enabled = false;
            this.btnChiTietKQ1.Enabled = false;
            this.btnChiTietKQ2.Enabled = false;
            this.txtChiCuc.EditValue = "all";
            LoadDanhSachBenhNhan();

        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitingLoadData), true, true, false);
                string machicuc = txtChiCuc.EditValue.ToString();
                string madv = txtDonVi.EditValue.ToString();
                if (!machicuc.Equals("all") && madv.Equals("all"))
                {
                    madv = machicuc;
                }
                if (!machicuc.Equals("all") && madv.Equals("all"))
                {
                    madv = machicuc;
                }
                this.gridControl_Info.DataSource = null;
                this.gridControl_Info.DataSource = BioBLL.GetListPatient(this.txtSearchTenTre.Text.TrimEnd(), this.txtSearchChaMe.Text.TrimEnd(),
                int.Parse(this.txtSeachGioiTinh.EditValue.ToString()), this.txtSearchNgaySinh.DateTime, int.Parse(this.cbbSL.Text), int.Parse(this.cbbTTPhieu.EditValue.ToString()), madv);
                this.CLearText();
                SplashScreenManager.CloseForm();
            }
            catch
            {
                MessageBox.Show("Lỗi tìm kiếm.", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK);
            }
           
        }

        private void gridView_Info_DoubleClick(object sender, EventArgs e)
        {
            CLearText();
            this.maBenhNhan = gridView_Info.GetRowCellValue(gridView_Info.FocusedRowHandle, "MaBenhNhan").ToString();
            PSPatient infoPerson = BioBLL.GetInfoPersonByMa(maBenhNhan);
            txtTenMe.Text = infoPerson.MotherName;
            txtTenCha.Text = infoPerson.FatherName;
            txtNamSinhMe.EditValue = !string.IsNullOrEmpty(infoPerson.MotherBirthday.ToString())? Convert.ToDateTime(infoPerson.MotherBirthday).ToString("yyyy") : string.Empty;
            txtNamSinhCha.EditValue = !string.IsNullOrEmpty(infoPerson.FatherBirthday.ToString())? Convert.ToDateTime(infoPerson.FatherBirthday).ToString("yyyy") : string.Empty;
            txtSDTMe.Text = infoPerson.MotherPhoneNumber;
            txtSDTCha.Text = infoPerson.FatherPhoneNumber;
            txtAddress.Text = infoPerson.DiaChi;
            txtTenBenhNhan.Text = infoPerson.TenBenhNhan;
            txtGioiTinh.EditValue = infoPerson.GioiTinh;
            txtNamSinhBenhNhan.EditValue = !string.IsNullOrEmpty(infoPerson.NgayGioSinh.ToString()) ? Convert.ToDateTime(infoPerson.NgayGioSinh).ToString() : string.Empty;
            txtNoiSinh.Text = infoPerson.NoiSinh;
            lookUpDanToc.EditValue = infoPerson.DanTocID;
            txtTuan.Value = Convert.ToDecimal(infoPerson.TuanTuoiKhiSinh);
            txtNang.Text = infoPerson.CanNang.ToString();
            cboPhuongPhapSinh.EditValue = infoPerson.PhuongPhapSinh;
            txtMaPhieu1.Text = BioNet_Bus.GetMaPhieu1TheoMaBN(maBenhNhan);
            txtMaKhachHang.Text = infoPerson.MaKhachHang;
           
         
            if(!string.IsNullOrEmpty(txtMaPhieu1.Text))
            {
                string MaDonVi = BioNet_Bus.GetThongTinTiepNhanTheoMaPhieu(txtMaPhieu1.Text).MaDonVi;
                var Phieu = BioNet_Bus.GetThongTinPhieu(txtMaPhieu1.Text,MaDonVi);
                string maTiepNhan = BioNet_Bus.GetThongTinMaTiepNhan(txtMaPhieu1.Text, MaDonVi);
                
                if(Phieu!=null)
                {
                    txtMaDVSC.Text = Phieu.maDonViCoSo;
                    btnSendEmail1.Enabled = true;
                    string CTNguyCoCao = string.Empty;
                    var kq = BioNet_Bus.GetThongTinKetQuaXN(txtMaPhieu1.Text, maTiepNhan);
                    if(kq!=null)
                    {
                        btnChiTietKQ1.Enabled = true;
                       
                        if (kq.PSXN_TraKQ_ChiTiets != null)
                        {
                            foreach (var kqct in kq.PSXN_TraKQ_ChiTiets)
                            {
                                if (kqct.isNguyCo)
                                {
                                    CTNguyCoCao = CTNguyCoCao + " " + kqct.TenThongSo;
                                }
                            }
                        }
                    }
                   
                    switch (Phieu.trangThaiMau)
                    {
                        case 1:
                            {
                                txtTrangThaiPhieu.Text = "Đã tiếp nhận";
                                break;
                            }
                        case 2:
                            {
                                txtTrangThaiPhieu.Text = "Đã đánh giá";
                                break;
                            }
                        case 3:
                            {
                                txtTrangThaiPhieu.Text = "Đã vào phòng xét nghiệm";
                                break;
                            }
                        case 4:
                            {
                                txtTrangThaiPhieu.Text = "Đã có kết quả";
                                break;
                            }
                        case 5:
                            {
                                txtTrangThaiPhieu.Text = "Đang làm xét nghiệm lại lần 2";
                                break;
                            }
                        case 6:
                            {
                                txtTrangThaiPhieu.Text = "Cần thu lại mẫu"+ CTNguyCoCao;
                                break;
                            }
                        case 7:
                            {
                                txtTrangThaiPhieu.Text = "Đã thu lại mẫu"+ CTNguyCoCao;
                              
                                break;
                            }
                        default:
                            {
                                txtTrangThaiPhieu.Text = "Không xác định";
                                break;
                            }
                    }
                    if (kq != null)
                    {
                       
                        if(kq.isDaGuiMail!=true)
                        {
                            btnSendEmail1.ImageOptions.ImageIndex = 0;
                        }
                        else
                        {
                            btnSendEmail1.ImageOptions.ImageIndex = 1;
                        }
                    }
                }
            }
            else{
                btnChiTietKQ1.Enabled = false;
                btnSendEmail1.Enabled = false;
            }
            txtMaPhieu2.Text = BioNet_Bus.GetMaPhieu2TheoMaBN(maBenhNhan);

            if (!string.IsNullOrEmpty(txtMaPhieu2.Text))
            {
                string MaDonVi = BioNet_Bus.GetThongTinTiepNhanTheoMaPhieu(txtMaPhieu2.Text).MaDonVi;
                var Phieu = BioNet_Bus.GetThongTinPhieu(txtMaPhieu2.Text, MaDonVi);
                string maTiepNhan = BioNet_Bus.GetThongTinMaTiepNhan(txtMaPhieu2.Text, MaDonVi);
                btnSendEmail2.Enabled = true;
                if (Phieu != null)
                {
                    var kq = BioNet_Bus.GetThongTinKetQuaXN(txtMaPhieu2.Text, maTiepNhan);
                    btnChiTietKQ2.Enabled = true;
                    switch (Phieu.trangThaiMau)
                    {
                        case 1:
                            {
                                txtTrangThaiPhieu2.Text = "Đã tiếp nhận";
                                break;
                            }
                        case 2:
                            {
                                txtTrangThaiPhieu2.Text = "Đã đánh giá";
                                break;
                            }
                        case 3:
                            {
                                txtTrangThaiPhieu2.Text = "Đã vào phòng xét nghiệm";
                                break;
                            }
                        case 4:
                            {
                                txtTrangThaiPhieu2.Text = "Đã có kết quả";
                                break;
                            }
                        case 5:
                            {
                                txtTrangThaiPhieu2.Text = "Đang làm xét nghiệm lại lần 2";
                                break;
                            }
                        case 6:
                            {
                                txtTrangThaiPhieu.Text = "Cần thu lại mẫu";
                                break;
                            }
                        case 7:
                            {
                                txtTrangThaiPhieu.Text = "Đã thu lại mẫu";
                                break;
                            }
                        default:
                            {
                                txtTrangThaiPhieu2.Text = "Không xác định";
                                break;
                            }
                    }
                    if (kq != null)
                    {
                       
                        if (kq.isDaGuiMail != true)
                        {
                            btnSendEmail2.ImageOptions.ImageIndex = 0;
                        }
                        else
                        {
                            btnSendEmail2.ImageOptions.ImageIndex = 1;
                        }
                    }
                }
            }
            else
            {
                btnChiTietKQ2.Enabled = false;
                btnSendEmail2.Enabled = false;
            }
            
            btnEdit.Enabled = true;
            ReadOnlyText(true);
        }

        private void CLearText()
        {
            txtTenMe.Text = string.Empty;
            txtTenCha.Text = string.Empty;
            txtNamSinhMe.Text = string.Empty;
            txtNamSinhCha.Text = string.Empty;
            txtSDTMe.Text = string.Empty;
            txtSDTCha.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtTenBenhNhan.Text = string.Empty;
            txtGioiTinh.Text = string.Empty;
            txtNamSinhBenhNhan.Text = string.Empty;
            txtNoiSinh.Text = string.Empty;
            lookUpDanToc.Text = string.Empty;
            txtTuan.Text = string.Empty;
            txtNang.Text = string.Empty;
            cboPhuongPhapSinh.Text = string.Empty;
            txtMaPhieu1.ResetText();
            txtMaPhieu2.ResetText();
            txtTrangThaiPhieu.ResetText();
            txtTrangThaiPhieu2.ResetText();
        }

        private void ReadOnlyText(bool en)
        {
            txtTenMe.ReadOnly = en;
            txtTenCha.ReadOnly = en;
            txtNamSinhMe.ReadOnly = en;
            txtNamSinhCha.ReadOnly = en;
            txtSDTMe.ReadOnly = en;
            txtSDTCha.ReadOnly = en;
            txtAddress.ReadOnly = en;
            txtTenBenhNhan.ReadOnly = en;
            txtGioiTinh.ReadOnly = en;
            txtNamSinhBenhNhan.ReadOnly = en;
            txtNoiSinh.ReadOnly = en;
            lookUpDanToc.ReadOnly = en;
            txtTuan.ReadOnly = en;
            txtNang.ReadOnly = en;
            cboPhuongPhapSinh.ReadOnly = en;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            btnEdit.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            ReadOnlyText(false);
            CLearText();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {              
             PSPatient   Patients = new BioNetModel.Data.PSPatient();
                
                if (string.IsNullOrEmpty(this.maBenhNhan))
                {
                    XtraMessageBox.Show("Không tìm thấy mã thông tin", "PowerSoft - Bệnh viện điện tử", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if(ValidationForm())
                {
                    Patients.MaBenhNhan = this.maBenhNhan;
                    Patients.MotherName = txtTenMe.Text;
                   Patients.FatherName = txtTenCha.Text;
                    if (!string.IsNullOrEmpty(txtNamSinhMe.EditValue.ToString()))
                       Patients.MotherBirthday = Convert.ToDateTime(txtNamSinhMe.EditValue);
                    if (!string.IsNullOrEmpty(txtNamSinhCha.EditValue.ToString()))
                        Patients.FatherBirthday = Convert.ToDateTime(txtNamSinhCha.EditValue);
                    Patients.MotherPhoneNumber = txtSDTMe.Text;
                    Patients.FatherPhoneNumber = txtSDTCha.Text;
                    Patients.DiaChi = txtAddress.Text;
                    Patients.TenBenhNhan = txtTenBenhNhan.Text;
                    if (!string.IsNullOrEmpty(txtGioiTinh.EditValue.ToString()))
                        Patients.GioiTinh = int.Parse(txtGioiTinh.EditValue.ToString()??"2");
                    string strNgayGio = Convert.ToDateTime(txtNamSinhBenhNhan.EditValue).ToString();
                    Patients.NgayGioSinh = Convert.ToDateTime(strNgayGio);
                    Patients.NoiSinh = txtNoiSinh.Text;
                    if (!string.IsNullOrEmpty(lookUpDanToc.EditValue.ToString()))
                        Patients.DanTocID = Convert.ToInt32(lookUpDanToc.EditValue);
                    
                    if (!string.IsNullOrEmpty(txtTuan.Value.ToString()))
                        Patients.TuanTuoiKhiSinh = Convert.ToByte(txtTuan.Value);
                    if (!string.IsNullOrEmpty(txtNang.Text))
                        Patients.CanNang = Convert.ToInt16(txtNang.Text);
                    if (!string.IsNullOrEmpty(cboPhuongPhapSinh.EditValue.ToString()))
                        Patients.PhuongPhapSinh =Convert.ToByte( cboPhuongPhapSinh.EditValue);
                 
                    if (BioBLL.UpdInfoPerson(Patients))
                    {
                        XtraMessageBox.Show("Cập nhật thông tin bệnh nhân thành công", "PowerSoft - Bệnh viện điện tử", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.LoadDanhSachBenhNhan();
                    }
                    else
                    {
                        XtraMessageBox.Show("Lỗi ! Không thể chỉnh sửa thông tin", "PowerSoft - Bệnh viện điện tử", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    ReadOnlyText(true);
                }
                
            }
            catch(Exception ex) { XtraMessageBox.Show("Lỗi! Chỉnh sửa thông tin thất bại", "PowerSoft - Bệnh viện điện tử", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private bool ValidationForm()
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(txtTenMe.Text))
                error += "Tên mẹ không được để trống \n";
            if(string.IsNullOrEmpty(txtTenCha.Text))
                error += "Tên cha không được để trống \n";
            if (string.IsNullOrEmpty(txtNamSinhMe.Text))
                error += "Năm sinh mẹ không được để trống \n";
            if (string.IsNullOrEmpty(txtNamSinhCha.Text))
                error += "Năm sinh cha không được để trống \n";
            if (string.IsNullOrEmpty(txtTenBenhNhan.Text))
                error += "Tên trẻ không được để trống \n";
            if (string.IsNullOrEmpty(txtGioiTinh.Text))
                error += "Giới tính trẻ không được để trống \n";
            if (string.IsNullOrEmpty(txtNamSinhBenhNhan.Text))
                error += "Năm sinh trẻ không được để trống \n";
            if (!string.IsNullOrEmpty(error))
            {
                XtraMessageBox.Show("Lỗi ! \n" + error, "PowerSoft - Bệnh viện điện tử", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnEdit.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            ReadOnlyText(true);
        }

        private void label1_Click(object sender, EventArgs e)
        {

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

        private void gridControl_Info_Click(object sender, EventArgs e)
        {

        }
        
       private void btnChiTietKQ1_Click(object sender, EventArgs e)
        {
            ViewTTCTPhieu(txtMaPhieu1.Text);
        }
        private void ViewTTCTPhieu(string MaPhieu)
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
                        Reports.frmDanhSachDaCapMa myForm = new Reports.frmDanhSachDaCapMa(datarp);
                        myForm.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Nhập mã phiếu cần tra cứu", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK);

                }
            }
            catch
            {
                MessageBox.Show("Lỗi hiện thị thông tin phiếu", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void labelControl18_Click(object sender, EventArgs e)
        {

        }

        private void txtTenCha_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void btnChiTietKQ2_Click(object sender, EventArgs e)
        {
            ViewTTCTPhieu(txtMaPhieu2.Text);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.LoadDanhmuc();
            this.CLearText();
        }

        private void btnSendEmail1_Click(object sender, EventArgs e)
        {

        }

        private void btnSendEmail2_Click(object sender, EventArgs e)
        {

        }

        private void txtChiCuc_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit sear = sender as SearchLookUpEdit;
                var value = sear.EditValue.ToString();
                this.txtDonVi.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi(value.ToString());
                this.txtDonVi.EditValue = "all";
            }
            catch { }
        }

        private void btnSDT1_Click(object sender, EventArgs e)
        {
            UserControl.ucLogSMS ucLogSMS = new UserControl.ucLogSMS(txtMaPhieu1.Text,txtMaKhachHang.Text,txtSDTMe.Text,txtMaDVSC.Text);
            ucLogSMS.ShowDialog();

        }
    }
}