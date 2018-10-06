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
using UserControlDate;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid.Views.Grid;

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
        private List<PSDanhMucDonViCoSo> lstDonVi = new List<PSDanhMucDonViCoSo>();
        private List<PSDanhMucChiCuc> lstChiCuc = new List<PSDanhMucChiCuc>();
        private List<PSDanhSachGuiSMS> lstsms = new List<PSDanhSachGuiSMS>();
       private List<DonViChon> lstdvchon = new List<DonViChon>();
        private List<PSDanhMucMauSMS> lstmausms = new List<PSDanhMucMauSMS>();
        private class VietTat
        {
            public int IDVietTat { get; set; }
            public string KiTu { get; set; }
            public string NoiDung { get; set; }
        }
        private class DonViChon
        {
            public string IDDVCS { get; set; }
            public string TenDVCS { get; set; }
            public bool check { get; set; }
        }
        private void cbbCTNoiDung_EditValueChanged(object sender, EventArgs e)
        {
            this.NoiDungDemo();
        }
        private void NoiDungDemo()
        {
            string tam = txtCTNoiDung.Text.Replace("#maphieu", "1234567");
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
            cbbCTNoiDungDemo.Text = tam;
            lblSKT.Text = txtCTNoiDung.Text.Length.ToString() + "/160";
            lblSKTDemo.Text = cbbCTNoiDungDemo.Text.Length.ToString() + "/160";
        }
        List<VietTat> lstvt = new List<VietTat>();
        private void FrmSendSMS_Load(object sender, EventArgs e)
        {
            lstvt.Add(new VietTat() { IDVietTat=1,KiTu= "maphieu",NoiDung= "Mã phiếu" });
            lstvt.Add(new VietTat() { IDVietTat = 2, KiTu = "tentre", NoiDung = "Tên trẻ" });
            lstvt.Add(new VietTat() { IDVietTat = 3, KiTu = "tennguoinhan", NoiDung = "Tên người nhân" });
            lstvt.Add(new VietTat() { IDVietTat = 4, KiTu = "ketqua", NoiDung = "PKu 20, CL 40" });
            lstvt.Add(new VietTat() { IDVietTat = 5, KiTu = "ketluan", NoiDung = "Nguy cơ cao" });
            LoadDanhMuc();
            LoadDSMauTinNhan();
        }

        private void LoadDanhMuc()
        {
            this.txtCTNoiDung.Enabled = false;
            this.cbbCTNoiDungDemo.Enabled = false;
            this.cbbKieukitu.Enabled = false;
            this.btnOK.Enabled = false;
            this.btnCancel.Enabled = true;
            this.cbbKieukitu.EditValue = true;
            this.cbbHinhThuc.SelectedIndex = 0;
            this.cbbDoiTuong.SelectedIndex = 0;
            this.cbbNoiDung.SelectedIndex = 0;
            this.cbbTrangThaiPhieu.SelectedIndex = 0;
            this.cbbTrangThaiGui.SelectedIndex = 0;
            this.CbbNguyCo.SelectedIndex = 0;
            this.cbbSDT.SelectedIndex = 0;
            this.dllNgay.tungay.Value = DateTime.Now;
            this.dllNgay.denngay.Value = DateTime.Now;
            this.lstChiCuc.Clear();
            this.lstChiCuc = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            this.cbbChiCuc.Properties.DataSource = null;
            this.cbbChiCuc.Properties.DataSource = this.lstChiCuc;
            this.cbbChiCuc.EditValue = "all";
            this.txtDSVietTat.Text = "#tentre = Tên trẻ \n #tennguoinhan= Tên người nhận \n #maphieu=mã phiếu \n #trangthaiphieu= trạng thái phiếu ";
            PsReponse reponse = BioNet_Bus.UpdateMaKhachHang();
        }
        private void LoadDSMauTinNhan()
        {
            lstmausms = BioNet_Bus.LoadMauSMS(cbbHinhThuc.EditValue.ToString(), cbbDoiTuong.EditValue.ToString(), cbbNoiDung.EditValue.ToString());
            this.cbbMauTinNhan.Properties.DataSource = null;
            this.cbbMauTinNhan.Properties.DataSource = lstmausms;
            this.cbbMauTinNhan.EditValue = lstmausms.First().RowIDMauSMS;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadDS();
        }
        private void LoadDS()
        {
            string madv = this.cbbDonVi.EditValue == null ? string.Empty : this.cbbDonVi.EditValue.ToString();
            string machicuc = this.cbbChiCuc.EditValue == null ? string.Empty : this.cbbChiCuc.EditValue.ToString();
            if (!machicuc.Equals("all") && madv.Equals("all"))
            {
                madv = machicuc;
            }
            if (cbbHinhThuc.EditValue.ToString().Equals("sms"))
            {
                lstsms = BioNet_Bus.GetDanhSachGuiSMS(this.dllNgay.tungay.Value.Date, this.dllNgay.denngay.Value.Date, int.Parse(cbbTrangThaiPhieu.EditValue.ToString()),
                int.Parse(this.CbbNguyCo.EditValue.ToString()), madv, txtCTNoiDung.Text, cbbDoiTuong.EditValue.ToString(), bool.Parse(cbbKieukitu.EditValue.ToString()),
                int.Parse(cbbTrangThaiGui.EditValue.ToString()), int.Parse(cbbSDT.EditValue.ToString()), cbbNoiDung.EditValue.ToString(), cbbMauTinNhan.EditValue.ToString(), cbbHinhThuc.EditValue.ToString(),bool.Parse(cbbSDT1.IsOn.ToString()));
            }
            else
            {
                MessageBox.Show("Chức năng gửi email đang hoàn thiện", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            GCCTDSGuiTinNhan.DataSource = null;
            GCCTDSGuiTinNhan.DataSource = lstsms;
        }
        private void cbbChiCuc_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit sear = sender as SearchLookUpEdit;
                var value = sear.EditValue.ToString();
                    this.cbbDonVi.Properties.DataSource = null;
                    this.cbbDonVi.Properties.DataSource = BioNet_Bus.GetDanhSachDonViSMS(value.ToString());
                    this.cbbDonVi.EditValue = "all";
             
            }
            catch { }
        }

        private void cbbHinhThuc_SelectedIndexChanged(object sender, EventArgs e)
        {
           switch(cbbHinhThuc.SelectedIndex.ToString())
            {
                case "0":
                    {
                        tabSMS.PageVisible = true;
                        tabEmail.PageVisible = false;
                        break;
                    }
                case "1":
                    {
                        MessageBox.Show("Chức năng đang hoàn thiện", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cbbHinhThuc.SelectedIndex = 0;
                        break;
                    }
            }
        }

        private void cbbTrangThaiPhieu_EditValueChanged(object sender, EventArgs e)
        {
            CbbNguyCo.SelectedIndex = 0;
            if (int.Parse(cbbTrangThaiPhieu.EditValue.ToString())>=3)
            {
                CbbNguyCo.Enabled = true;
            }
            else
            {
                CbbNguyCo.Enabled = false;
            }
           
        }

        private void cbbMauTinNhan_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                lstmausms = BioNet_Bus.LoadMauSMS(cbbHinhThuc.EditValue.ToString(), cbbDoiTuong.EditValue.ToString(), cbbNoiDung.EditValue.ToString());
                if (lstmausms.Count() > 0)
                {
                    this.txtCTNoiDung.Text = lstmausms.FirstOrDefault(x => x.RowIDMauSMS == int.Parse(cbbMauTinNhan.EditValue.ToString())).MauNoiDungGui;
                    this.cbbKieukitu.EditValue = false;
                    this.btnOK.Enabled = false;
                    this.btnCancel.Enabled = true;
                }
                else
                {
                    this.btnOK.Enabled = false;
                    this.btnCancel.Enabled = false;
                }
            }
            catch
            {
            }         
        }

        private void cbbKieukitu_EditValueChanged(object sender, EventArgs e)
        {
            this.NoiDungDemo();
        }

        private void cbbDoiTuong_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbbDoiTuong.SelectedIndex.ToString())
            {
                case "0":
                    {
                        break;
                    }
                case "1":
                    {
                        MessageBox.Show("Chức năng đang hoàn thiện","Thông báo", MessageBoxButtons.OK,MessageBoxIcon.Information);
                        cbbDoiTuong.SelectedIndex = 0;
                        //tabSMS.PageVisible = false;
                        //tabEmail.PageVisible = true;
                        break;
                    }
            }
        }

        private void cbbNoiDung_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbbNoiDung.SelectedIndex.ToString())
            {
                case "2":
                    {
                        MessageBox.Show("Chức năng đang hoàn thiện", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cbbDoiTuong.SelectedIndex = 0;
                        break;
                    }
                default:
                    {
                        LoadDSMauTinNhan();
                        break;
                    }                
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.txtCTNoiDung.Enabled = false;
            this.cbbCTNoiDungDemo.Enabled = false;
            this.cbbKieukitu.Enabled = false;
            this.btnOK.Enabled = false;
            this.btnCancel.Enabled = true;
            PSDanhMucMauSMS sms = new PSDanhMucMauSMS();
            sms.RowIDMauSMS = int.Parse(cbbMauTinNhan.EditValue.ToString());
            sms.DoiTuongNhanTN = cbbDoiTuong.EditValue.ToString();
            sms.HinhThucGuiTN = cbbHinhThuc.EditValue.ToString();
            sms.NoidungGui = cbbNoiDung.EditValue.ToString();
            sms.MauNoiDungGui = txtCTNoiDung.Text;
            PsReponse reponse = BioNet_Bus.InsertMauSMS(sms);
            if (reponse.Result)
            {
                XtraMessageBox.Show("Lưu thành công.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                XtraMessageBox.Show("Lưu thất bại.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtCTNoiDung.Enabled = true;
            this.cbbCTNoiDungDemo.Enabled = true;
            this.cbbKieukitu.Enabled = true;
            this.btnCancel.Enabled = false;
            this.btnOK.Enabled = true;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "Excel File(*.xlsx)|*.xlsx";
            ofd.FileName = "DSGuiTinNhanSMS" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName.Length > 0)
                {
                    try
                    {

                        this.GVCTDSGuiTinNhan.ExportToXlsx(ofd.FileName);
                    }
                    catch
                    {
                        XtraMessageBox.Show("Không thể lưu file này! Vui lòng chọn đường dẫn khác.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void ExportDataToExcelFile()
        {
           
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                PsReponseSMS res = BioNet_Bus.SMS(this.cbbCTNoiDungDemo.Text,txtSDT.Text, Boolean.Parse(cbbKieukitu.EditValue.ToString()));
                if(res.Result)
                {
                    XtraMessageBox.Show("Gửi tin nhắn thàng công.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("Gửi tin nhắn lỗi "+res.StringError, "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void labelControl20_Click(object sender, EventArgs e)
        {

        }

        private void btnDanhSachKiTu_Click(object sender, EventArgs e)
        {
            PanelDanhSachKiTu.Visible = true;
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            PanelDanhSachKiTu.Visible = false;
        }

        private void btnSendSMS_Click(object sender, EventArgs e)
        {
            try
            {
                List<PsReponseSMS> resct = new List<PsReponseSMS>();
                List<PSDanhSachGuiSMS> lisok = new List<PSDanhSachGuiSMS>();
                
                var lstChecked = this.GVCTDSGuiTinNhan.GetSelectedRows();
                if(lstChecked.Count()>0)
                {
                    foreach (var index in lstChecked)
                    {
                        try
                        {
                            string MaPhieu = this.GVCTDSGuiTinNhan.GetRowCellValue(index, this.col_IDPhieu).ToString();
                            string sdt1 = this.GVCTDSGuiTinNhan.GetRowCellValue(index, this.col_SDTGui) != null ? this.GVCTDSGuiTinNhan.GetRowCellValue(index, this.col_SDTGui).ToString() : string.Empty;
                            string NoidungTN = this.GVCTDSGuiTinNhan.GetRowCellValue(index, this.col_NoiDungTN) != null ? this.GVCTDSGuiTinNhan.GetRowCellValue(index, this.col_NoiDungTN).ToString() : string.Empty;
                            string MaKH = this.GVCTDSGuiTinNhan.GetRowCellValue(index, this.col_MaKhachHang) != null ? this.GVCTDSGuiTinNhan.GetRowCellValue(index, this.col_MaKhachHang).ToString() : string.Empty;
                            if (!string.IsNullOrEmpty(sdt1))
                            {
                                PsReponseSMS res = BioNet_Bus.SMS(NoidungTN, sdt1, Boolean.Parse(cbbKieukitu.EditValue.ToString()));
                                var ls = lstsms.FirstOrDefault(x => x.MaPhieu.Equals(MaPhieu) && x.MaKhachHang.Equals(MaKH));
                                BioNet_Bus.InsertSMSNumber(ls, res, emp.EmployeeCode);
                            }
                        }
                        catch
                        {

                        }
                        
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Gửi tin nhắn lỗi ", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.LoadDS();

        }

        private void labelControl21_Click(object sender, EventArgs e)
        {

        }

        private void imageComboBoxEdit1_Toggled(object sender, EventArgs e)
        {

        }

        private void btnXuatSMS_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "Excel File(*.xlsx)|*.xlsx";
            ofd.FileName = "DSMauGuiTinNhanSMS" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName.Length > 0)
                {
                    try
                    {
                        Reports.RepostsSMS.FrmDSSMS rp1 = new Reports.RepostsSMS.FrmDSSMS();
                        rp1.DataSource = GVCTDSGuiTinNhan.DataSource;
                        rp1.ExportToXlsx(ofd.FileName);
                        System.Diagnostics.Process.Start(ofd.FileName);
                    }
                    catch
                    {
                        XtraMessageBox.Show("Không thể lưu file này! Vui lòng chọn đường dẫn khác.", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }     

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DiaglogFrm.FrmThemMauTinNhan frmThemMau = new DiaglogFrm.FrmThemMauTinNhan();
            frmThemMau.ShowDialog();
        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
        }      

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

        }

        private void GVCTDSGuiTinNhan_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                GridView View = sender as GridView;
                if (e.RowHandle >= 0)
                {
                    string nguyCo = View.GetRowCellValue(e.RowHandle, this.col_TinhTrangGui) == null ? string.Empty : View.GetRowCellValue(e.RowHandle, this.col_TinhTrangGui).ToString();
                    if (nguyCo=="True")
                    {
                        e.Appearance.BackColor = Color.Salmon;
                        e.Appearance.BackColor2 = Color.SeaShell;
                    }
                    else if(nguyCo =="False")
                    {
                        e.Appearance.BackColor = Color.Gainsboro;
                        e.Appearance.BackColor2 = Color.LightGray;
                    }
                    else
                    {
                        e.Appearance.BackColor = Color.SkyBlue;
                        e.Appearance.BackColor2 = Color.LightSkyBlue;
                    }
                }
            }
            catch { }
        }

        private void cbbTrangThaiGui_EditValueChanged(object sender, EventArgs e)
        {          
             
        }

        private void CbbNguyCo_EditValueChanged(object sender, EventArgs e)
        {
            this.LocSMS();
        }
        private void LocSMS()
        {
            PSDanhMucMauSMS kq = new PSDanhMucMauSMS();
            try
            {
                switch (CbbNguyCo.EditValue.ToString())
                {
                    case "1":
                        {
                            if (cbbTrangThaiPhieu.EditValue.ToString() == "4")
                            {
                                kq = lstmausms.FirstOrDefault(x => x.RowIDMauSMS == 3);

                            }
                            else if (cbbTrangThaiPhieu.EditValue.ToString() == "6")
                            {
                                kq = lstmausms.FirstOrDefault(x => x.RowIDMauSMS == 4);
                            }
                            break;
                        }
                    case "0":
                        {
                            if (cbbTrangThaiPhieu.EditValue.ToString() == "4")
                            {
                                kq = lstmausms.FirstOrDefault(x => x.RowIDMauSMS == 6);

                            }
                            else if (cbbTrangThaiPhieu.EditValue.ToString() == "6")
                            {
                                kq = lstmausms.FirstOrDefault(x => x.RowIDMauSMS == 5);
                            }
                            break;
                        }
                }
                if (kq.RowIDMauSMS != 0)
                {
                    cbbMauTinNhan.EditValue = kq.RowIDMauSMS;
                }
                else
                {
                    cbbMauTinNhan.EditValue = lstmausms.First().RowIDMauSMS;
                }
            }
            catch
            {

            }

        }
    }
}