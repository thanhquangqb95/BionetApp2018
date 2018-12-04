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
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using System.Diagnostics;
using DevExpress.DashboardCommon.Native.DashboardRestfulService;
using BioNetSangLocSoSinh.Reports;
using System.Net.Mail;
using System.Net;
using System.IO.Compression;
using BioNetModel.Data;
using DevExpress.XtraSplashScreen;
using BioNetSangLocSoSinh.DiaglogFrm;
using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace BioNetSangLocSoSinh.FrmReports
{
    public partial class FrmGuiMail : DevExpress.XtraEditors.XtraForm
    {
    
        string pathMail = Application.StartupPath + "\\DSGuiMail\\";
        List<string> lstMaPhieu = new List<string>();
        PSThongTinTrungTam tt = new PSThongTinTrungTam();
        int TongCheck = 0;
        PsEmployeeLogin emp = new PsEmployeeLogin();
        public FrmGuiMail(PsEmployeeLogin Emp)
        {
            emp = Emp;
            InitializeComponent();
        }

        private void LoadDuLieuEmail()
        {
            this.GC_DSPhieuMail.DataSource = BioNet_Bus.GetTinhTrangPhieuMail(this.dllNgay.tungay.Value, this.dllNgay.denngay.Value, txtDonVi.EditValue.ToString(),txtChiCuc.EditValue.ToString());
            TongCheck = 0;
            if (this.GV_DSPhieuMail.DataRowCount == 0)
            {
                MessageBox.Show("Không có dữ liệu phiếu kết quả cần tìm", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK);
                this.bttGuiMail.Enabled = false;
            }
            else
            {
                this.bttGuiMail.Enabled = true;
            }
            lblTongCheck.Text = "Tổng cộng phiếu được chọn: " + TongCheck.ToString() + " phiếu";

        }

        private void FrmGuiMail_Load(object sender, EventArgs e)
        {
            this.txtChiCuc.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_ChiCuc();
            this.txtDonVi.Properties.DataSource = BioNet_Bus.GetDieuKienLocBaoCao_DonVi("all");
            this.lookupeditviettat.DataSource= BioNet_Bus.GetDieuKienLocBaoCao_DonVi("all");
            this.txtDonVi.EditValue = "all";
            this.txtChiCuc.EditValue = "all";
            this.bttGuiMail.Enabled = false;
            lblTongCheck.Text = "Tổng cộng phiếu được chọn: " + TongCheck.ToString() + " phiếu";
            int sl=BioNet_Bus.GetSLChuaDuocGuiMail();
            lblChuaGuiEmail.Text = "Còn " + sl + " phiếu chưa được trả qua email";
            AddItemForm();
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

        private void butOK_Click(object sender, EventArgs e)
        {
            LoadDuLieuEmail();            
        }

        //Chọn tắt cả
        private void ckkTatCa_CheckedChanged(object sender, EventArgs e)
        {

            try
            {
                TongCheck = 0;
                this.ckkTatCa.Text = ckkTatCa.Checked ? "Bỏ chọn" : "Chọn tất cả";
                for (int i = 0; i < GV_DSPhieuMail.DataRowCount; i++)
                {
                    if (this.ckkTatCa.Checked)
                    {

                        GV_DSPhieuMail.SetRowCellValue(i, col_Chon, 1);
                        TongCheck = TongCheck+1;
                    }
                    else
                    {
                        GV_DSPhieuMail.SetRowCellValue(i, col_Chon, 0);
                        TongCheck = 0;
                    }
                }
                lblTongCheck.Text = "Tổng cộng phiếu được chọn: " + TongCheck.ToString() + " phiếu";
            }
            catch (Exception ex) { return; }

        }

        private void bttGuiMail_Click(object sender, EventArgs e)
        {
           
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn là sẽ gửi mail", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try {
                    SplashScreenManager.ShowForm(this, typeof(WaitingformMail), true, true, false);
                    List<PsTinhTrangPhieu> dt = (List<PsTinhTrangPhieu>)GC_DSPhieuMail.DataSource;
                    List<string> MaDVCS = new List<string>();
                    TongCheck = 0;

                    DataTable dtselect = new DataTable();
                    int kq = 0;
                    int chon = 0;
                    for (int i = 0; i < dt.Count; i++)
                    {
                        int kt = 0;
                        if (dt[i].Chon == 1)
                        {
                            chon = 1;
                            string maDVCS = dt[i].MaDonVi.ToString();
                            string maPhieu = dt[i].IDPhieu.ToString();
                            string temdvcs = dt[i].TenDonVi.ToString();
                            string tentre = dt[i].TenBenhNhan.ToString();
                            if (MaDVCS != null)
                            {
                                foreach (string dvcs in MaDVCS)
                                {
                                    if (maDVCS == dvcs)
                                    {
                                        kt = 1;
                                        break;
                                    }
                                }
                            }
                            if (kt == 0) { MaDVCS.Add(maDVCS); }
                            string pathpdf = string.Empty;
                            //Noi lưu phiếu trả kết quả                
                            pathpdf = Application.StartupPath + "\\PhieuKetQua\\" + maDVCS + "\\" + maPhieu + ".pdf";                                                  
                            LuuPDF(maPhieu, maDVCS); 
                            NenGuiMail(pathpdf, maPhieu, maDVCS,tentre);
                        }
                    }
                 
                    if (chon == 1)
                    {
                        foreach (string madvcs in MaDVCS)
                        {

                            kq = GuiMailNEw(madvcs);
                            if (kq == 1)
                            {
                                XtraMessageBox.Show("Gửi mail cho đơn vị " + madvcs + " thất bại", "bionet - chương trình sàng lọc sơ sinh", MessageBoxButtons.OK);
                                break;
                            }
                            else if (kq == 2)
                            {
                                XtraMessageBox.Show("Email của đơn vị " + madvcs + " không tồn tại", "bionet - chương trình sàng lọc sơ sinh", MessageBoxButtons.OK);
                                break;
                            }
                            else if (kq == 5)
                            {
                                XtraMessageBox.Show("Dữ liệu của Email của trung tâm lỗi - Vui lòng kiểm tra lại dữ liệu ", "bionet - chương trình sàng lọc sơ sinh", MessageBoxButtons.OK);
                                break;
                            }
                            else
                            {
                                var donvi = BioNet_Bus.GetThongTinDonViCoSo(madvcs);
                                var MaPhieu = dt.Where(x => x.Chon == 1 && x.MaDonVi == madvcs).Select(x => x.IDPhieu).ToList();
                                var res = BioNet_Bus.CapNhatGuiMail(MaPhieu,madvcs,tt.Email,donvi.Email,emp.EmployeeCode);
                                
                            }
                        }
                        SplashScreenManager.CloseForm();
                        if (kq == 0) { XtraMessageBox.Show("Gửi Mail thành công", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK
                            
                            );

                        }
                        //Xóa file nén đã gửi mail
                        DirectoryInfo dirInfo = new DirectoryInfo(pathMail);
                        FileInfo[] childFiles = dirInfo.GetFiles();
                        foreach (FileInfo childFile in childFiles) { File.Delete(childFile.FullName); }
                    }
                    else { XtraMessageBox.Show("Vui lòng tick vào phiếu cần gửi mail", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK); }
                }
                catch(Exception ex)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(pathMail);
                    FileInfo[] childFiles = dirInfo.GetFiles();
                    foreach (FileInfo childFile in childFiles) { File.Delete(childFile.FullName); }
                    XtraMessageBox.Show("Lỗi khi gửi mail", "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
                lblTongCheck.Text = "Tổng cộng phiếu được chọn: " + TongCheck.ToString() + " phiếu";
            }
            else if (dialogResult == DialogResult.No) { return; }

        }

      

        private int GuiMail(string MaDVCS)
        {
            var donvi = BioNet_Bus.GetThongTinDonViCoSo(MaDVCS);
            this.tt = BioNet_Bus.GetThongTinTrungTam();
            if(this.tt!=null)
            {
                string fromAddress = tt.Email;
                string passEMail = tt.PassEmail;
                string tieude = "BIONET: KẾT QUẢ SLSS - " + donvi.TenDVCS + " " + DateTime.Now.ToShortDateString();
                //Bảng kết quả
                string bangkq = ThongKeMail(MaDVCS);
                string noidung = "<p style='font-size:12.8px;margin:6pt 0in;text-align:justify'><font face='times new roman,serif' size='4' color='black'>" + "Kính gửi:<b> " + donvi.TenDVCS + "</b>,</font></p>" +
                    "<p style='font-size:12.8px;margin:6pt 0in;text-align:justify '><font face='times new roman,serif' size='4' color='black'>" + "Trung tâm SLSS Bionet Việt Nam gửi tới Bệnh viện file kết quả sàng lọc sơ sinh tiếp nhận từ ngày " + this.dllNgay.tungay.Value.ToShortDateString() + " đến ngày " + this.dllNgay.denngay.Value.ToShortDateString() + "." + "</font></p>" +
                   "<p style='font-size:12.8px;margin:6pt 0in;text-align:justify'><font face='times new roman,serif' size = '4' color='black'>" + "Kết quả xét nghiệm như sau:" + "</font></p>" + bangkq +
                  "<p style='font-size:12.8px;margin:6pt 0in;text-align:justify'><font face='times new roman,serif' size = '4' color='black'>" + "Mọi thắc mắc hoặc góp ý, xin quý Bệnh viện vui lòng liên hệ Trung tâm Sàng lọc sơ sinh Bionet Việt Nam." + "</font></p>" + "<p style='font-size:12.8px;margin:6pt 0in;text-align:justify'><font face='times new roman,serif' size = '4' color='black'>" + "Số điện thoại Chăm sóc khách hàng:<span style='color:rgb(54,204,255)'> 024 6686 1304,</span>" + "</font></p>" +
                   "<p></p><p></p>" +
                   "<p style='font-size:12.8px;margin:6pt 0in;text-align:justify'><font face='times new roman,serif' size = '4' color='black'>" + "Kính thư," + "</font></p>";
                string madvcs = MaDVCS;
                string pathzip = string.Empty;
                if (ckkTenTre.Checked==true)
                {
                    pathzip = pathMail + madvcs + "(" + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + ")" + ".zip";
                }
                else
                {
                    pathzip = pathMail + madvcs + "(" + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + ")" + ".pdf";
                }
               
                //string MailKH = BioNet_Bus.GetThongTinMailDonViCoSo(madvcs);
                int kq = DataSync.BioNetSync.GuiMail.Send_Email_With_Attachment(donvi.Email, fromAddress, passEMail, pathzip, tieude, noidung);
                //File.Create(pathzip).Close();
                return kq;
            }
            else
            {
                return 5;
            }
           
        }
        private int GuiMailNEw(string MaDVCS)
        {
            var donvi = BioNet_Bus.GetThongTinDonViCoSo(MaDVCS);
            this.tt = BioNet_Bus.GetThongTinTrungTam();
            if (this.tt != null)
            {
                string fromAddress = tt.Email;
                string passEMail = tt.PassEmail;
                string tieude = "BIONET: KẾT QUẢ SLSS - " + donvi.TenDVCS + " " + DateTime.Now.ToShortDateString();
                //Bảng kết quả
                string bangkq = ThongKeMail(MaDVCS);
                string noidung = "<table align='center' border='0' cellpadding='0' cellspacing='0' width='100%' bgcolor='#dcf0f8' style='margin:0;padding:0;background-color:#f2f2f2;width:100%!important;font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#444;line-height:18px'> <tbody> <tr> <td align='center' valign='top' style='font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#444;line-height:18px;font-weight:normal'> <table border='0' cellpadding='0' cellspacing='0' width='600' style='margin-top:15px'> <tbody> <tr> <td align='center' valign='bottom' id='m_-6403583252414891579headerImage'> <table width='100%' cellpadding='0' cellspacing='0'> <tbody> <tr> <td valign='top' bgcolor='#FFFFFF' width='100%' style='padding:0'> <a style='border:medium none;text-decoration:none;color:#007ed3' href='http://Bionet.vn/' target='_blank'> <img alt='Bionet.vn' src='https://share1.cloudhq-mkt3.net/cbbb635dec.png' style='border:none;outline:none;text-decoration:none;display:inline;height:auto' width='100%' class='CToWUd'> </a> </td> </tr> </tbody> </table> </td> </tr> <tr style='background:#fff'> <td align='left' width='600' height='auto' style='padding:15px'> <table> <tbody> <tr> <td> <p>Kính gửi <b> " + donvi.TenDVCS + "</b>,</font></p>" +
                   "<p>Trung tâm SLSS Bionet Việt Nam kính gửi tới "+ donvi.TenDVCS+" file kết quả sàng lọc sơ sinh tiếp nhận từ ngày " + dllNgay.tungay.Value.ToShortDateString() + " đến ngày " + dllNgay.denngay.Value.ToShortDateString() + ". Bệnh viện vui lòng kiểm tra chi tiết các file kết quả bằng cách tải về file đính kèm dưới email này.</p> <h3 style='font-size:13px;font-weight:bold;color:#02acea;text-transform:uppercase;margin:20px 0 0 0;border-bottom:1px solid #ddd'>Thống kê kết quả xét nghiệm <span style='font-size:12px;color:#777;text-transform:none;font-weight:normal'>(Bảng chỉ có tính chất tham khảo)</span></h3></td></tr><tr></tr>" + bangkq +
                   "<td align='center' valign='top' id='m_-3704480983150737815templateFooter' style='background:#fafafa none no-repeat center/cover;background-color:#fafafa;background-image:none;background-repeat:no-repeat;background-position:center;background-size:cover;border-top:0;border-bottom:0;padding-top:9px;padding-bottom:9px'> <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%' style='border-collapse:collapse;max-width:600px!important'> <tbody> <tr> <td valign='top' > <table border='0' cellpadding='0' cellspacing='0' width='100%' c style='min-width:100%;border-collapse:collapse'> <tbody > <tr> <td valign='top' style='padding-top:9px'> <table align='left' border='0' cellpadding='0' cellspacing='0' style='max-width:100%;min-width:100%;border-collapse:collapse' width='100%' > <tbody> <tr> <td valign='top' c style='padding-top:0;padding-right:18px;padding-bottom:9px;padding-left:18px;word-break:break-word;color:#656565;font-family:Helvetica;font-size:12px;line-height:150%;text-align:center'> <table border='0' cellpadding='0' cellspacing='0' style='border-spacing:0px;width:600px;border-collapse:collapse'> <tbody> <tr> <td style='padding:15px;text-align:center;width:100%;background:#2196f3;vertical-align:middle;line-height:1'> <table border='0' cellpadding='0' cellspacing='0' style='border-spacing:0px;border-collapse:collapse'> <tbody> <tr> <td style='min-width:400px'> <table border='0' cellpadding='0' cellspacing='0' style='border-spacing:0px;border-collapse:collapse'> <tbody> <tr> <td style='color:#e4f2fb;font-size:11px;font-family:Arial;padding:3px;text-align:left'> TRUNG TÂM SÀNG LỌC SƠ SINH BIONET VIÊT NAM <span class='il'></span> </td> </tr> <tr> <td> <table border='0' cellpadding='0' cellspacing='0' style='border-spacing:0px;border-collapse:collapse'> <tbody> <tr> <td style='padding:3px'> <img src='https://ci3.googleusercontent.com/proxy/8SravChKNm8GWbyt87Mc4C6Q_jZIX9JFi-MBePHv_TcjgvH0wIW93T16u6e_rRbfRvSWT4DlLFXKp0E1CDifluz3ro2wRvyeNBq9s-0MEqpfx2QCYyoDNg=s0-d-e1-ft#https://vcdn.tikicdn.com/assets/emails/img/email-promotion/icon-1.png' style='border:0;height:auto!important;outline:none;text-decoration:none' class='CToWUd'> </td> <td style='color:#e4f2fb;font-size:11px;font-family:Arial;padding:3px'>Tầng 2 Tòa nhà GP Invest, 170 Đê La Thành, Quận Đống Đa, TP.Hà Nội </td> </tr> </tbody> </table> </td> </tr> <tr> <td> <table border='0' cellpadding='0' cellspacing='0' style='border-spacing:0px;border-collapse:collapse'> <tbody> <tr> <td style='padding:3px'> <img src='https://ci5.googleusercontent.com/proxy/gDEN2x1rxZd5m01Epg2pY5imxWGFbd6bX5hC9g0ABX1lnD8n6BbJ6DQOoPaNr8nIk3RNM8dhhJ0wGwzA18QNS8DPD-mqNTLRsyVGnEe7Vvy1PwkFucEP3g=s0-d-e1-ft#https://vcdn.tikicdn.com/assets/emails/img/email-promotion/icon-2.png' style='border:0;height:auto!important;outline:none;text-decoration:none' class='CToWUd'> </td> <td style='color:#e4f2fb;font-size:11px;font-family:Arial;padding:3px'> <b>024 6686 1304 - 0975 067 766</b> </td> </tr> </tbody> </table> </td> </tr> <tr> <td> <table border='0' cellpadding='0' cellspacing='0' style='border-spacing:0px;border-collapse:collapse'> <tbody> <tr> <td style='padding:3px'> <img src='https://ci5.googleusercontent.com/proxy/X_Y9BnPVFFZcM8LVAurZb4w3Tt1NRwGjdJX36TPye58Q3On6moxdisObZGRk4AmCoNUOV7uiBIFHw0lJsueIJXcDk8uBs2J0OWKIsOr5ZG-uXRD3LKyl-A=s0-d-e1-ft#https://vcdn.tikicdn.com/assets/emails/img/email-promotion/icon-3.png' style='border:0;height:auto!important;outline:none;text-decoration:none' class='CToWUd'> </td> <td style='color:#e4f2fb;font-size:11px;font-family:Arial;padding:3px'> <b> <a style='color:#e4f2fb;font-size:11px;font-family:Arial;font-weight:bold;text-decoration:underline' href='mailto:Sanglocsosinh@bionet.vn' target='_blank'>Sanglocsosinh@ <span class='il'>bionet</span>.vn</a> </b> </td> </tr> </tbody> </table> </td> </tr> <tr> <td style='color:#e4f2fb;font-size:11px;font-family:Arial;padding:3px;text-align:left'> VĂN PHÒNG ĐẠI DIỆN TẠI TP.HCM <span class='il'></span> </td> </tr> <tr> <td> <table border='0' cellpadding='0' cellspacing='0' style='border-spacing:0px;border-collapse:collapse'> <tbody> <tr> <td style='padding:3px'> <img src='https://ci3.googleusercontent.com/proxy/8SravChKNm8GWbyt87Mc4C6Q_jZIX9JFi-MBePHv_TcjgvH0wIW93T16u6e_rRbfRvSWT4DlLFXKp0E1CDifluz3ro2wRvyeNBq9s-0MEqpfx2QCYyoDNg=s0-d-e1-ft#https://vcdn.tikicdn.com/assets/emails/img/email-promotion/icon-1.png' style='border:0;height:auto!important;outline:none;text-decoration:none' class='CToWUd'> </td> <td style='color:#e4f2fb;font-size:11px;font-family:Arial;padding:3px'>64 Trương Định, P.7, Q.3, TP.HCM, Việt Nam </td> </tr> </tbody> </table> </td> </tr> <tr> <td> <table border='0' cellpadding='0' cellspacing='0' style='border-spacing:0px;border-collapse:collapse'> <tbody> <tr> <td style='padding:3px'> <img src='https://ci5.googleusercontent.com/proxy/gDEN2x1rxZd5m01Epg2pY5imxWGFbd6bX5hC9g0ABX1lnD8n6BbJ6DQOoPaNr8nIk3RNM8dhhJ0wGwzA18QNS8DPD-mqNTLRsyVGnEe7Vvy1PwkFucEP3g=s0-d-e1-ft#https://vcdn.tikicdn.com/assets/emails/img/email-promotion/icon-2.png' style='border:0;height:auto!important;outline:none;text-decoration:none' class='CToWUd'> </td> <td style='color:#e4f2fb;font-size:11px;font-family:Arial;padding:3px'> <b>0283 932 5196 - 079 618 889 </b> </td> </tr> </tbody> </table> </td> </tr> <tr> <td style='color:#e4f2fb;font-size:11px;font-family:Arial;padding:3px;text-align:left'> Email này được gửi tự động từ phần mềm quản lý Sàng lọc sơ sinh của Bionet Việt Nam. </td> </tr> <tr> <td style='color:#e4f2fb;font-size:11px;font-family:Arial;padding:3px;text-align:left'> Quý vị có thể truy cập vào website <a style='color:#e4f2fb;font-size:11px;font-family:Arial;font-weight:bold;text-decoration:underline' href='sanglocsosinh.vn'>sanglocsosinh.vn</a> để biết thêm chi tiết. </td> </tr> </tbody> </table> </td> <td style='vertical-align:bottom;padding-left:25px;width:195px'> <table border='0' cellpadding='0' cellspacing='0' style='border-spacing:0px;border-collapse:collapse'> <tbody> <tr> <td style='color:#e4f2fb;font-size:11px;font-family:Arial;padding:3px'> <table border='0' cellpadding='0' cellspacing='0' style='border-spacing:0px;border-collapse:collapse'> <tbody> <tr> <td> </td> BIONET VIETNAM </td>"
                   + " <td style='padding:1px'> <a href='https://www.facebook.com/sanglocsosinhbionet/' style='color:#656565;font-weight:normal;text-decoration:underline' target='_blank' data-saferedirecturl='www.sanglocsosinh.vn'>"+
                   "<img src='https://share1.cloudhq-mkt3.net/bd394238f3.png'" +
                   "style='border:0;height:auto!important;outline:none;text-decoration:none'> </a> </td> <td style='padding:1px'> <a href='http://www.sanglocsosinh.vn/' style='color:#656565;font-weight:normal;text-decoration:underline' target='_blank' data-saferedirecturl='http://www.sanglocsosinh.vn/'> <img style='-webkit-user-select: none;' src='https://share1.cloudhq-mkt3.net/963766df62.png'> </a> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </td> </tbody> " +
                   " <tr> <td align='left '> <tbody> <tr> <td> <p style='font-family:Arial,Helvetica,sans-serif;font-size:11px;line-height:18px;color:#4b8da5;padding:10px 0;margin:0px;font-weight:normal' align='left '> Quý vị nhận được email này vì " + donvi.TenDVCS +
                   " đã đăng ký địa chỉ email của Quý vị là email nhận kết quả Sàng lọc sơ sinh. <br> Nếu Quý vị không đồng ý nhận kết quả SLSS hoặc bệnh viện có sự thay đổi địa chỉ email nhận kết quả, vui lòng gửi thông tin về email: " + "<strong> <a href='mailto:sanglocsosinh@bionet.vn' target='_blank'>Sanglocsosinh@bionet.vn</a> </strong> để Bionet xử lý kịp thời. <br> <b>Trân trọng! </p> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </td> </tr> </tbody>";

                string madvcs = MaDVCS;
                string pathzip = string.Empty;
                if (ckkTenTre.Checked == true)
                {
                    pathzip = pathMail + madvcs + "(" + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + ")" + ".zip";
                }
                else
                {
                    pathzip = pathMail + madvcs + "(" + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + ")" + ".pdf";
                }

                //string MailKH = BioNet_Bus.GetThongTinMailDonViCoSo(madvcs);
                int kq = DataSync.BioNetSync.GuiMail.Send_Email_With_Attachment(donvi.Email, fromAddress, passEMail, pathzip, tieude, noidung);
                //File.Create(pathzip).Close();
                return kq;
            }
            else
            {
                return 5;
            }

        }
        //THống kê
        private string ThongKeMail(string MaDVCS)
        {
            //string zipPath = Application.StartupPath + "\\DSGuiMail\\" + MaDVCS+ "(" + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year +")"+ ".zip";
            string zipPath = Application.StartupPath + "\\DSGuiMail\\" + MaDVCS+  ".txt";
            // string[] maphieu = null;
            string[] maphieu = File.ReadAllLines(zipPath);

            PSTKKQPhieuMail tk = new PSTKKQPhieuMail();
            tk = BioNet_Bus.GetThongKePhieuMail(maphieu);
            int chitieumaumoi = tk.benh2 * 2 + tk.benh3 * 3 + tk.benh5 * 5+tk.hemo2*3+tk.hemo3*4+tk.hemo5*6;
            int chitieumauxnl = tk.G6PD3 +tk.GAL3+tk.PKU3+tk.CH3+tk.CAH3+tk.HEMO3;
            string bangkq = "<table width='100%' border='1' cellpadding='0' cellspacing='0'>" +
                                                    "<tr style='padding:5px; text-align:center;'>" +
                                                        "<th style='padding:5px; text-align:center; color:#fff; background-color:#02acea;' colspan='3'>" +
                                                            "<b> MẪU XÉT NGHIỆM</b>"
                                                        + "</th>" +
                                                        "<th style='padding:5px; text-align:center; color:#fff; background-color:#02acea;' colspan='8'>" +
                                                        "<b>KẾT QUẢ XÉT NGHIỆM</b>" +
                                                        "</th>    </tr>" +
                                                        "<tr style='padding:5px; text-align:center;'>"+
                                                            "<td style='padding:5px; text-align:center; background-color:#d9e5e8;' colspan='1'>Mẫu mới</td>"+
                                                            "<td style='padding:5px; text-align:center;'>2 bệnh</td>"+
                                                            "<td style='padding:5px; text-align:center;'>" + tk.benh2 + "</td>" +
                                                            "<td style='padding:5px; text-align:center;' rowspan='2'>Nguy cơ thấp</td>" +
                                                            "<td style='padding:5px; text-align:center; background-color:#87dc86;' colspan='1'>Tổng</td>" +
                                                            "<td style='padding:5px; text-align:center; background-color:#d9e5e8;' colspan='1'>G6PD</td>" +
                                                            "<td style='padding:5px; text-align:center; background-color:#d9e5e8;' colspan='1'>TGAL</td>" +
                                                            "<td style='padding:5px; text-align:center; background-color:#d9e5e8;' colspan='1'>PKU</td>" +
                                                            "<td style='padding:5px; text-align:center; background-color:#d9e5e8;' colspan='1'>CH</td>" +
                                                            "<td style='padding:5px; text-align:center; background-color:#d9e5e8;' colspan='1'>CAH</td>" +
                                                            "<td style='padding:5px; text-align:center; background-color:#d9e5e8;' colspan='1'>HEMO</td>" +
                                                        "</tr>" +

                                                       "<tr style='padding:5px; text-align:center;' >" +
                                                            "<td style='padding:5px; text-align:center;' rowspan='5'>" + tk.MauMoi+"<br>"+"( "+ chitieumaumoi+" chỉ tiêu )" + "</td>" +
                                                            "<td style='padding:5px; text-align:center;'>3 bệnh</td>" +
                                                            "<td style='padding:5px; text-align:center;'>" + tk.benh3 + "</td>" +
                                                            "<td style='padding:5px; text-align:center;'>" + tk.NguyCoThap + "</td>" +
                                                            "<td style='padding:5px; text-align:center;'> " + tk.G6PD + "</td>" +
                                                            "<td style='padding:5px; text-align:center;'>" + tk.GAL + "</td>" +
                                                            "<td style='padding:5px; text-align:center;'>" + tk.PKU + "</td>" +
                                                            "<td style='padding:5px; text-align:center;'>" + tk.CH + "</td>" +
                                                            "<td style='padding:5px; text-align:center;'>" + tk.CAH + "</td>" +
                                                            "<td style='padding:5px; text-align:center;'>" + tk.HEMO + "</td>" +
                                                        "</tr>" +

                                                        "<tr style='padding:5px; text-align:center;'>" +
                                                             "<td style='padding:5px; text-align:center;'>5 bệnh</td>" +
                                                             "<td style='padding:5px; text-align:center;'>" + tk.benh5 + "</td>" +
                                                            "<td style='padding:5px; text-align:center;' colspan='8'></td>" +
                                                        "</tr>" +

                                                       "<tr style='padding:5px; text-align:center;'>" +
                                                             "<td style='padding:5px; text-align:center;'>2 bệnh + Hemo</td>" +
                                                             "<td style='padding:5px; text-align:center;'>" + tk.hemo2 + "</td>" +
                                                             "<td style='padding:5px;' rowspan='2' colspan='1'>Nghi ngờ</td>" +
                                                             "<td style='padding:5px; text-align:center;background-color:#87dc86;' colspan='1'>Tổng</td>" +
                                                             "<td style='padding:5px; text-align:center;background-color:#d9e5e8;' colspan='1'>G6PD</td>" +
                                                             "<td style='padding:5px; text-align:center;background-color:#d9e5e8;' colspan='1'>TGAL</td>" +
                                                             "<td style='padding:5px; text-align:center;background-color:#d9e5e8;' colspan='1'>PKU</td>" +
                                                             "<td style='padding:5px; text-align:center;background-color:#d9e5e8;' colspan='1'>CH</td>" +
                                                             "<td style='padding:5px; text-align:center;background-color:#d9e5e8;' colspan='1'>CAH</td>" +
                                                             "<td style='padding:5px; text-align:center;background-color:#d9e5e8;' colspan='1'>HEMO</td>" +
                                                       "</tr>" +

                                                       "<tr style='padding:5px; text-align:center;'>" +
                                                             "<td style='padding:5px; text-align:center;'>3 bệnh + Hemo</td>" +
                                                             "<td style='padding:5px; text-align:center;'>" + tk.hemo3 + "</td>" +
                                                             "<td style='padding:5px; text-align:center;'>" + tk.NguyCoCao + "</td>" +
                                                             "<td style='padding:5px; text-align:center;'> " + tk.G6PD2 + "</td>" +
                                                             "<td style='padding:5px; text-align:center;'>" + tk.GAL2 + "</td>" +
                                                             "<td style='padding:5px; text-align:center;'>" + tk.PKU2 + "</td>" +
                                                             "<td style='padding:5px; text-align:center;'>" + tk.CH2 + "</td>" +
                                                             "<td style='padding:5px; text-align:center;'>" + tk.CAH2 + "</td>" +
                                                             "<td style='padding:5px; text-align:center;'>" + tk.HEMO2 + "</td>" +
                                                       "</tr>" +

                                                       "<tr style='padding:5px; text-align:center;'>" +
                                                             "<td style='padding:5px; text-align:center;'>5 bệnh + Hemo</td>" +
                                                             "<td style='padding:5px; text-align:center;'>" + tk.hemo5 + "</td>" +
                                                             "<td style='padding:5px; text-align:center;' colspan='8'></td>" +
                                                        "</tr>" +

                                                       "<tr><td style = 'padding:2px;text-align:center;background-color:#83e8a7', colspan = '11' ></td></tr>"
                                                        +
                                                             "<tr style='padding:5px; text-align:center;'>" +
                                                             "<td style='padding:5px; text-align:center; background-color:#d9e5e8;' colspan='1'>Mẫu thu lại</td>"+
                                                               "<td style='padding:5px; text-align:center;' colspan='1'>G6PD</td>" +
                                                             "<td style='padding:5px; text-align:center;' colspan='1'>" + tk.G6PD3 + "</td>" +
                                                              "<td style='padding:5px; text-align:center;' rowspan='2'>Nguy cơ thấp</td>" +
                                                               "<td style='padding:5px; text-align:center; background-color:#87dc86;' colspan='1'>Tổng</td>" +
                                                                "<td style='padding:5px; text-align:center; background-color:#d9e5e8;' colspan='1'>G6PD</td>" +
                                                                "<td style='padding:5px; text-align:center; background-color:#d9e5e8;' colspan='1'>TGAL</td>" +
                                                                "<td style='padding:5px; text-align:center; background-color:#d9e5e8;' colspan='1'>PKU</td>" +
                                                                "<td style='padding:5px; text-align:center; background-color:#d9e5e8;' colspan='1'>CH</td>" +
                                                                "<td style='padding:5px; text-align:center; background-color:#d9e5e8;' colspan='1'>CAH</td>" +
                                                                "<td style='padding:5px; text-align:center; background-color:#d9e5e8;' colspan='1'>HEMO</td>" +
                                                        "</tr>" +

                                                        "<tr style='padding:5px; text-align:center;'> " +
                                                             "<td style='padding:5px; text-align:center;' rowspan='5'>" + tk.MauXNL + "<br>" + "( " + chitieumauxnl + " chỉ tiêu )" + "</td>" +
                                                               "<td style='padding:5px; text-align:center;' colspan='1'>TGAL</td>" +
                                                             "<td style='padding:5px; text-align:center;' colspan='1'>" + tk.GAL3 + "</td>" +
                                                             "<td style='padding:5px; text-align:center;'> " + tk.NguyCoThap2 + "</td>" +
                                                             "<td style='padding:5px; text-align:center;'> " + tk.G6PD4 + "</td>" +
                                                             "<td style='padding:5px; text-align:center;'>" + tk.GAL4 + "</td>" +
                                                             "<td style='padding:5px;text-align:center;'>" + tk.PKU4 + "</td>" +
                                                             "<td style='padding:5px;text-align:center;'>" + tk.CH4 + "</td>" +
                                                             "<td style='padding:5px;text-align:center;'>" + tk.CAH4 + "</td>" +
                                                             "<td style='padding:5px;text-align:center;'>" + tk.HEMO4 + "</td>" +
                                                        "</tr>" +
                                                        
                                                    "<tr style='padding:5px; text-align:center;'>" +
                                                             "<td style='padding:5px; text-align:center;'>PKU</td>" +
                                                             "<td style='padding:5px; text-align:center;'>" + tk.PKU3 + "</td>" +
                                                             "<td style='padding:5px; text-align:center;' colspan='8'></td>" +   
                                                    "</tr>" +
                                                    "<tr>"+
                                                         "<td style='padding:5px; text-align:center;' colspan='1'>CH</td>" +
                                                            "<td style='padding:5px; text-align:center;' colspan='1'>" + tk.CH3 + "</td>" +
                                                            "<td style='padding:5px; text-align:center;' rowspan='2' colspan='1'>Nguy cơ cao</td>" +
                                                            "<td style='padding:5px; text-align:center;background-color:#87dc86;' colspan='1'>Tổng</td>" +
                                                            "<td style='padding:5px; text-align:center;background-color:#d9e5e8;' colspan='1'>G6PD</td>" +
                                                            "<td style='padding:5px; text-align:center;background-color:#d9e5e8;' colspan='1'>TGAL</td>" +
                                                            "<td style='padding:5px; text-align:center;background-color:#d9e5e8;' colspan='1'>PKU</td>" +
                                                            "<td style='padding:5px; text-align:center;background-color:#d9e5e8;' colspan='1'>CH</td>" +
                                                            "<td style='padding:5px; text-align:center;background-color:#d9e5e8;' colspan='1'>CAH</td>" +
                                                            "<td style='padding:5px; text-align:center;background-color:#d9e5e8;' colspan='1'>HEMO</td>" +
                                                    "</tr>"+
                
                                                    "<tr style='padding:5px; text-align:center;'>" +
                                                            "<td style='padding:5px; text-align:center;' colspan='1'>CAH</td>" +
                                                            "<td style='padding:5px; text-align:center;' colspan='1'>" + tk.CAH3+ "</td>" +
                                                            "<td style='padding:5px; text-align:center;'> " + tk.NguyCoCao2 + "</td>" +
                                                            "<td style='padding:5px; text-align:center;'> " + tk.G6PD5 + "</td>" +
                                                            "<td style='padding:5px; text-align:center;'>" + tk.GAL5 + "</td>" +
                                                            "<td style='padding:5px; text-align:center;'>" + tk.PKU5 + "</td>" +
                                                            "<td style='padding:5px; text-align:center;'>" + tk.CH5 + "</td>" +
                                                            "<td style='padding:5px; text-align:center;'>" + tk.CAH5 + "</td>" +
                                                            "<td style='padding:5px; text-align:center;'>" + tk.HEMO5 + "</td>" +
                                                     "</tr>" +
                                                      "<tr style='padding:5px; text-align:center;'> " +
                                                           "<td style='padding:5px; text-align:center;'>HEMO</td>" +
                                                            "<td style='padding:5px; text-align:center;'>" + tk.HEMO3 + "</td>" +
                                                             "<td style='padding:5px; text-align:center;' colspan='8'></td>" +
                                                    "</tr>" +
"</table>" + "<br></br>" +
"<tr> <td> <h3 style='font-size:13px;font-weight:bold;color:#02acea;text-transform:uppercase;margin:20px 0 0 0;border-bottom:1px solid #ddd'>Thống kê Chất lượng mẫu <span style='font-size:12px;color:#777;text-transform:none;font-weight:normal'>(Bảng chỉ có tính chất tham khảo)</span> </h3> </td> </tr>"+
"<table width='100%'  border='1' cellpadding='0' cellspacing='0' >" +
                "<tr><td colspan=6 style='padding:5px; text-align:center;color:#ffffff; background-color:#5ec0d6;'>CHẤT LƯỢNG MẪU </td></tr >" +
                "<tr><td colspan=1 style='padding:5px; text-align:center;background-color:#d9e5e8;'>Mẫu đạt </td>" +
                "<td colspan=2 style='padding:5px; text-align:center;background-color:#d9e5e8;'>Mẫu không đạt </td>" +
                "<td colspan=1  style='padding:5px; text-align:center;'>" + tk.MauKDat + "</td>" +
       "<tr>" +
                "<td rowspan=6  style='padding:5px; text-align:center;'>" + tk.MauDat + "</td>" +
                         "<td rowspan=6 style='padding:5px; text-align:center;'>Lý Do</td>"+ 
                         "<td colspan=1 style='padding:5px; '>1. Mẫu không thấm đều 2 mặt hoặc mẫu ít </td>"+ 
                         "<td colspan=1 style='padding:5px; text-align:center;'>"+(tk.MauKdeu+ tk.Mauit).ToString()+" </ td > " + 

           "</tr>"+
           "<tr>"+
                         "<td colspan=1 style='padding:5px; '>2. Giọt máu chồng lên nhau </td> "+
                         "<td colspan=1 style='padding:5px; text-align:center;'>" + +tk.MauChong + " </ td > " +

           "</tr>"+
           "<tr>" +
                         "<td colspan=1 style='padding:5px; '>3. Thời gian gửi mẫu muộn </td> " +
                         "<td colspan=1 style='padding:5px; text-align:center;'>" + tk.GuiCham + " </ td > " +

           "</tr>" +
            "<tr>" +
                         "<td colspan=1 style='padding:5px; '>4. Thu mẫu sớm (trước 24h tuổi) </td>" +
                         "<td colspan=1 style='padding:5px; text-align:center;'>" + tk.ThuSom + "</td>" +
           "</tr>" +
            "<tr>" +
                         "<td colspan=1 style='padding:5px; '>5. Trẻ sinh non hoặc nhẹ cân </td>" +
                         "<td colspan=1 style='padding:5px; text-align:center;'>" + tk.SinhNonNheCan + "</td>" +
           "</tr>" +
            "<tr>" +
                         "<td colspan=1 style='padding:5px; '>6. Lý do khác </td>" +
                         "<td colspan=1 style='padding:5px; text-align:center;'>" + tk.LyDoKhac + "</td>" +
           " </tr> </table> </td> </tr>";
            if (ckkTenTre.Checked == false)
            {
                string zipPathpdf = Application.StartupPath + "\\DSGuiMail\\" + MaDVCS + "(" + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + ")" + ".pdf";
                GopPDF(maphieu, zipPathpdf,MaDVCS);
            }
            return bangkq;
        }
        //Lưu File PDF
        private void LuuPDF(string MaPhieu, string MaDVCS )
        {
            PsRptTraKetQuaSangLoc data = new PsRptTraKetQuaSangLoc();
            try
            {
                var donvi = BioNet_Bus.GetThongTinDonViCoSo(MaDVCS);
                string Matiepnhan = BioNet_Bus.GetThongTinMaTiepNhan(MaPhieu, MaDVCS);
                if (donvi != null)
                {
                    var kieuTraKQ = donvi.KieuTraKetQua ?? 1;
                    data = BioNet_Bus.GetDuLieuInKetQuaSangLoc(MaPhieu, Matiepnhan, "MaBsi", MaDVCS);
                    if (kieuTraKQ == 1) // Cần sửa chỗ này, cho chọn động loat report theo cấu hình của đơn vị
                    {
                        Reports.rptPhieuTraKetQua datarp = new Reports.rptPhieuTraKetQua();
                        frmReportEditGeneral.FileLuuPDF(datarp, data);
                    }
                    else
                    if (kieuTraKQ == 2)
                    {
                        Reports.rptPhieuTraKetQua_TheoTT2 datarp = new Reports.rptPhieuTraKetQua_TheoTT2();
                        frmReportEditGeneral.FileLuuPDF(datarp, data);
                    }
                    else
                    if (kieuTraKQ == 3)
                    {
                        Reports.rptPhieuTraKetQua_TheoDonVi datarp = new Reports.rptPhieuTraKetQua_TheoDonVi();
                        frmReportEditGeneral.FileLuuPDF(datarp, data);
                    }
                    else
                    if (kieuTraKQ == 4)
                    {
                        Reports.rptPhieuTraKetQua_TheoDonVi2 datarp = new Reports.rptPhieuTraKetQua_TheoDonVi2();
                        frmReportEditGeneral.FileLuuPDF(datarp, data);
                    }
                    else
                    if (kieuTraKQ == 5)
                    {
                        Reports.rptPhieuTraKetQua_TheoDonVi3 datarp = new Reports.rptPhieuTraKetQua_TheoDonVi3();
                        frmReportEditGeneral.FileLuuPDF(datarp, data);
                    }
                    else
                    {
                        Reports.rptPhieuTraKetQua datarp = new Reports.rptPhieuTraKetQua();
                        frmReportEditGeneral.FileLuuPDF(datarp, data);
                    }
                }
                else
                {
                    Reports.rptPhieuTraKetQua datarp = new Reports.rptPhieuTraKetQua();
                    frmReportEditGeneral.FileLuuPDF(datarp, data);
                }
            }
            catch (Exception ex) { XtraMessageBox.Show("Lỗi phát sinh khi lấy dữ liệu in \r\n Lỗi chi tiết :" + ex.ToString(), "BioNet - Chương trình sàng lọc sơ sinh", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }
        //Nén file zip
        private void NenGuiMail(string pathpdf, string Maphieu, string MaDVCS,string TenTre)
        {
            string tendvcs = MaDVCS;
            string maphieu = Maphieu;
            string startPath = pathpdf;
            if (!Directory.Exists(pathMail))
            {
                Directory.CreateDirectory(pathMail);
            }
            string zipPath = string.Empty;


            //Lựa chọn phương thức gửi
            if (ckkTenTre.Checked==true)
            {
                //Nén file zip + tên file con lưu dưới dạng [mã phiếu]_[tên trẻ].pdf
                zipPath = Application.StartupPath + "\\DSGuiMail\\" + tendvcs + "(" + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + ")" + ".zip";
                if (Directory.Exists(zipPath))
                {
                    ZipFile.CreateFromDirectory(startPath, zipPath);
                }
                else
                {
                    using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                    {
                        try
                        {
                            archive.CreateEntryFromFile(startPath, maphieu+"_"+ TenTre + ".pdf");
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            else
            {
                
            }

            string list = Application.StartupPath + "\\DSGuiMail\\" + tendvcs + ".txt";
            if (!File.Exists(list))
            { 
                FileStream fs = new FileStream(list, FileMode.Create);//Tạo file mới tên là test.txt            
                StreamWriter sWriter = new StreamWriter(fs, Encoding.UTF8);
                sWriter.WriteLine(Maphieu);
                sWriter.Flush();
                fs.Close();
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(list, true))
                {
                    sw.WriteLine(Maphieu);
                }
            }
           
        }

        //Gộp nhiều file pdf vào 1 file
        public static void GopPDF(string[] MaPhieu, string outFile,string maDVCS)
        {
            Document document = new Document();
            PdfCopy writer = new PdfCopy(document, new FileStream(outFile, FileMode.Create));
            if (writer == null)
            {
                return;
            }
            document.Open();

            foreach (string maPhieu in MaPhieu)
            {
                string fileName =  Application.StartupPath + "\\PhieuKetQua\\" + maDVCS + "\\" + maPhieu + ".pdf";
                PdfReader reader = new PdfReader(fileName);
                reader.ConsolidateNamedDestinations();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                    writer.AddPage(page);
                }
                PRAcroForm form = reader.AcroForm;
                if (form != null)
                {
                    writer.CopyDocumentFields(reader);
                }

                reader.Close();
            }

            writer.Close();
            document.Close();
        }
        private void GV_DSPhieuMail_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                GridView Viewer = sender as GridView;
                
                string isMail = Viewer.GetRowCellValue(e.RowHandle, this.col_isGuiMail).ToString();
                if (isMail=="True" || isMail=="1")
                {
                        e.Appearance.BackColor = Color.LightSkyBlue;
                        e.Appearance.BackColor2 = Color.LightSkyBlue; 
                } 
                if (e.Column.FieldName == "TinhTrangMau_Text")
                {
                    string category = Viewer.GetRowCellValue(e.RowHandle, Viewer.Columns["TinhTrangMau"]).ToString();
                   
                    int trangthai = 0;
                    try
                    {
                        trangthai = int.Parse(category);
                    }
                    catch { }


                    switch (trangthai)
                    {
                        case 1:

                            e.Appearance.BackColor = Color.DeepSkyBlue;
                            e.Appearance.BackColor2 = Color.LightCyan;
                            break;
                        case 2:
                            e.Appearance.BackColor = Color.Wheat;
                            e.Appearance.BackColor2 = Color.LightCyan;
                            break;
                        case 3:
                            e.Appearance.BackColor = Color.LightCoral;
                            e.Appearance.BackColor2 = Color.LightCyan;
                            break;
                        case 4:
                            e.Appearance.BackColor = Color.Aqua;
                            e.Appearance.BackColor2 = Color.LightCyan;
                            break;
                        case 5:
                            e.Appearance.BackColor = Color.LightYellow;
                            e.Appearance.BackColor2 = Color.LightCyan;
                            break;
                        case 6:
                            e.Appearance.BackColor = Color.SandyBrown;
                            e.Appearance.BackColor2 = Color.LightCyan;
                            break;
                    }
                }
            }
            catch (Exception ex) { }
        }        

        private void GV_DSPhieuMail_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                GridView Viewer = sender as GridView;
               
                string isChon = Viewer.GetRowCellValue(e.RowHandle, this.col_Chon).ToString();
                if (isChon == "1" || isChon == "True")
                {
                    GV_DSPhieuMail.SetRowCellValue(e.RowHandle, col_Chon, 0);
                    TongCheck -= 1;
                }
                else
                {
                    GV_DSPhieuMail.SetRowCellValue(e.RowHandle, col_Chon, 1);
                    TongCheck += 1;
                }
                lblTongCheck.Text= "Tổng cộng phiếu được chọn: " + TongCheck.ToString() + " phiếu";

            }
            catch (Exception ex) { }
        }

        private void ckkTenTre_CheckedChanged(object sender, EventArgs e)
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
            //CustomLayouts.TransLanguage.AddItemCT(this.Controls, idfo);
            CustomLayouts.TransLanguage.Trans(this.Controls, idfo);
        }
    }
}