﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace DataSync.BioNetSync
{
    public class GuiMail
    {
        public class Email
        {
        }
        public string Send_Email(string SendFrom, string SendTo, string Subject, string Body)
        {
            try
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

                string to = SendTo;
                bool result = regex.IsMatch(to);
                if (result == false)
                {
                    return "Địa chỉ email không hợp lệ.";
                }
                else
                {
                    System.Net.Mail.SmtpClient smtp = new SmtpClient();
                    System.Net.Mail.MailMessage msg = new MailMessage(SendFrom, SendTo, Subject, Body);
                    msg.IsBodyHtml = true;
                    smtp.Host = "smtp.gmail.com";//Sử dụng SMTP của gmail
                    smtp.Send(msg);
                    return "Email đã được gửi đến: " + SendTo + ".";
                }
            }
            catch
            {
                return "";
            }
        }

        public static int Send_Email_With_Attachment(string SendTo, string SendFrom,string pass, string AttachmentPath, string tieude, string noidung)
        {
            try
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

                string from = SendFrom;
                MailAddress fromsend = new MailAddress(SendFrom, "BIONET VN SLSS", Encoding.Unicode);
              
                string to = "thanhquangqb95@gmail.com";
                string subject = tieude;
               string body =noidung;
                string passmail = pass;
                string[] mailcc=to.Split(',');
                if(mailcc.Count()>0)
                {
                    bool result = regex.IsMatch(mailcc[0]);
                    if (result == false)
                    {
                        //Lỗi địa chỉ mail
                        return 2;
                    }
                    else
                    {
                        try
                        {
                            
                           // MailMessage em = new MailMessage(from, to, subject, body);
                            MailAddress tosend = new MailAddress(mailcc[0]);
                            MailMessage emnew = new MailMessage(fromsend, tosend);
                            using (Attachment attach = new Attachment(AttachmentPath))
                            {
                                emnew.Attachments.Add(attach);
                                emnew.Bcc.Add(to);
                                //for(int i=0; i<mailcc.Count();i++)
                                //{
                                //    if(i==0)
                                //    {
                                //        em.Bcc.Add(mailcc[i]);
                                //    }
                                //    else
                                //    {
                                //        em.CC.Add(mailcc[i]);
                                //    }

                                //}
                                emnew.Subject = subject;
                                emnew.Body = noidung;
                                emnew.IsBodyHtml = true;
                                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                                smtp.EnableSsl = true;
                                smtp.Credentials = new NetworkCredential(from, passmail);//Mật khâu mail
                                smtp.Send(emnew);
                                emnew.Dispose();
                                return 0;
                            }

                        }
                        catch (Exception ex)
                        {
                            return 1;
                        }
                    }
                }
                else
                {
                    return 1;
                }
                
            }
            catch
            {
                return 1;
            }
        }
        public string Send_Email_With_BCC_Attachment(string SendTo, string SendBCC, string SendFrom, string Subject, string Body, string AttachmentPath)
        {
            try
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                string from = SendFrom;
                string to = SendTo; //Danh sách email được ngăn cách nhau bởi dấu ";"
                string subject = Subject;
                string body = Body;
                string bcc = SendBCC;
                bool result = true;
                String[] ALL_EMAILS = to.Split(';');
                foreach (string emailaddress in ALL_EMAILS)
                {
                    result = regex.IsMatch(emailaddress);
                    if (result == false)
                    {
                        return "Địa chỉ email không hợp lệ.";
                    }
                }
                if (result == true)
                {
                    try
                    {
                        MailMessage em = new MailMessage(from, to, subject, body);
                        Attachment attach = new Attachment(AttachmentPath);
                        em.Attachments.Add(attach);
                        em.Bcc.Add(bcc);

                        System.Net.Mail.SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";//Ví dụ xử dụng SMTP của gmail
                        smtp.Send(em);

                        return "";
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



    }
}
