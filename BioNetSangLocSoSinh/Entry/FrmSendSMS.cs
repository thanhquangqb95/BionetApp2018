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
        private List<PSMauSMS> lstmausms = new List<PSMauSMS>();
        private class VietTat
        {
            public int IDVietTat { get; set; }
            public string KiTu { get; set; }
            public string NoiDung { get; set; }
        }
        public class PSMauSMS
        {
            public int IDMauSMS { get; set; }
            public string TenMauSMS { get; set; }
            public string TieuDe { get; set; }
            public string NoiDung { get; set; }
            public bool isCoDau { get; set; }
        }
        private void cbbCTNoiDung_EditValueChanged(object sender, EventArgs e)
        {
            this.NoiDungDemo();
        }
        private void NoiDungDemo()
        {
            string tam = cbbCTNoiDung.Text.Replace("#maphieu", "1234567");
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
            lblSKT.Text = cbbCTNoiDung.Text.Length.ToString() + "/160";
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
            lstmausms.Add(new PSMauSMS() { IDMauSMS = 1,
                TenMauSMS = "Mẫu SMS trạng thái phiếu",
                NoiDung = "[BionetSangLocSoSinh]Chào #tennguoinhan. Phiếu #maphieu của bé #tentre, sinh:#ngaysinh đã #trangthaiphieu. Chi tiết tại sanglocsosinh.vn",
                isCoDau=false
            });
            lstmausms.Add(new PSMauSMS()
            {
                IDMauSMS = 2,
                TenMauSMS = "Mẫu SMS KQ phiếu",
                NoiDung = "[BionetSangLocSoSinh]Chào #tennguoinhan. Phiếu #maphieu. Trạng thái:#trangthaiphieu. Có kết quả: #ketqua. KL: #ketluan. Chi tiết tại sanglocsosinh.vn",
                isCoDau = false
            });
            LoadDanhMuc();
            LoadDSMauTinNhan();
        }

        private void LoadDanhMuc()
        {
            this.cbbKieukitu.EditValue = true;
           
            this.cbbMauTinNhan.EditValue = 1;
            this.cbbHinhThuc.SelectedIndex = 0;
            this.cbbDoiTuong.SelectedIndex = 0;
            this.cbbNoiDung.SelectedIndex = 1;
            this.cbbTrangThaiPhieu.SelectedIndex = 0;
            this.CbbNguyCo.SelectedIndex = 0;
            this.dllNgay.tungay.Value = DateTime.Now;
            this.dllNgay.denngay.Value = DateTime.Now;
            this.lstChiCuc.Clear();
            this.lstChiCuc = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            this.cbbChiCuc.Properties.DataSource = null;
            this.cbbChiCuc.Properties.DataSource = this.lstChiCuc;
            this.cbbChiCuc.EditValue = "all";
            this.txtDSVietTat.Text = "#tentre = Tên trẻ \n #tennguoinhan= Tên người nhận \n #maphieu=mã phiếu \n #trangthaiphieu= trạng thái phiếu ";
        }
        private void LoadDSMauTinNhan()
        {
            this.cbbMauTinNhan.Properties.DataSource = null;
            this.cbbMauTinNhan.Properties.DataSource = lstmausms;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string madv = this.cbbDonVi.EditValue == null ? string.Empty : this.cbbDonVi.EditValue.ToString();
            string machicuc = this.cbbChiCuc.EditValue == null ? string.Empty : this.cbbChiCuc.EditValue.ToString();
            if (!machicuc.Equals("all") && madv.Equals("all"))
            {
                madv = machicuc;
            }
            if(cbbHinhThuc.EditValue.ToString().Equals("0"))
            {
                lstsms = BioNet_Bus.GetDanhSachGuiSMS(this.dllNgay.tungay.Value.Date, this.dllNgay.denngay.Value.Date, int.Parse(cbbTrangThaiPhieu.EditValue.ToString()),
                int.Parse(this.CbbNguyCo.EditValue.ToString()), madv, cbbCTNoiDung.Text, int.Parse(cbbDoiTuong.EditValue.ToString()),bool.Parse(cbbKieukitu.EditValue.ToString()));
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
                this.cbbDonVi.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi(value.ToString());
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
                        //tabSMS.PageVisible = false;
                        //tabEmail.PageVisible = true;
                        break;
                    }
            }
        }

        private void cbbTrangThaiPhieu_EditValueChanged(object sender, EventArgs e)
        {
            if (int.Parse(cbbTrangThaiPhieu.EditValue.ToString()) > 3)
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
            this.cbbCTNoiDung.Text = lstmausms.FirstOrDefault(x => x.IDMauSMS == int.Parse(cbbMauTinNhan.EditValue.ToString())).NoiDung;
            this.cbbKieukitu.EditValue = lstmausms.FirstOrDefault(x => x.IDMauSMS == int.Parse(cbbMauTinNhan.EditValue.ToString())).isCoDau;
            this.btnOK.Enabled = true;
            this.btnCancel.Enabled = false;
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
                case "0":
                    {
                        cbbMauTinNhan.EditValue = 2;
                        break;
                    }
                case "1":
                    {
                        cbbMauTinNhan.EditValue = 1;
                        break;
                    }
                case "2":
                    {
                        MessageBox.Show("Chức năng đang hoàn thiện", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cbbDoiTuong.SelectedIndex = 0;
                        break;
                    }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.cbbMauTinNhan.Enabled = false;
            this.cbbCTNoiDung.Enabled = false;
            this.cbbCTNoiDungDemo.Enabled = false;
            this.cbbKieukitu.Enabled = false;
            this.btnOK.Enabled = false;
            this.btnCancel.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.cbbMauTinNhan.Enabled = true;
            this.cbbCTNoiDung.Enabled = true;
            this.cbbCTNoiDungDemo.Enabled = true;
            this.cbbKieukitu.Enabled = true;
            this.btnCancel.Enabled = false;
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
                foreach (var ls in lstsms)
                {
                    string sdt = string.IsNullOrEmpty(ls.SDTNguoiNhan) ? "" : ls.SDTNguoiNhan;
                    if(!string.IsNullOrEmpty(sdt))
                    {
                        PsReponseSMS res = BioNet_Bus.SMS(ls.NoiDungTinNhan, sdt, Boolean.Parse(cbbKieukitu.EditValue.ToString()));
                        if (res.Result)
                        {
                            lisok.Add(ls);
                          
                        }
                        BioNet_Bus.InsertSMSNumber(ls, res, emp.EmployeeCode);
                    }
                   
                }
                if(lisok.Count>0)
                {
                    foreach (var ls in lisok)
                    {
                        lstsms.Remove(ls);
                        GCCTDSGuiTinNhan.DataSource = null;
                        GCCTDSGuiTinNhan.DataSource = lstsms;
                    }
                }
            }
            catch (Exception ex)
            {
            XtraMessageBox.Show("Gửi tin nhắn lỗi ", "BioNet - Sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
        }
    }
}