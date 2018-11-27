using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace BionetUpdate
{
    public partial class FrmBionetUpdate : Form
    {
        public FrmBionetUpdate()
        {
            InitializeComponent();
        }
        public void FrmBionetUpdate_Load(object sender, EventArgs e)
        {
            lblFileCopy.Text = string.Empty;
            lblCopyLink.Text = string.Empty;
        }
        private void BBUpdate()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(calltringconect()))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                        string sql = "select LinkUpdate from PSThongTinTrungTam";
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int lint = reader.GetOrdinal("LinkUpdate");
                                    string empName = reader.GetString(lint);
                                    CopyfileFull(empName, Application.StartupPath);
                                    //progressBarControlDownload.Properties.Step = 1;
                                    //progressBarControlDownload.Properties.PercentView = true;
                                    //progressBarControlDownload.Properties.Maximum = 100;
                                    //progressBarControlDownload.Properties.Minimum = 0;
                                    //WebClient Wc = new WebClient();

                                    //DirectoryInfo dirInfo = new DirectoryInfo(empName);
                                    //FileInfo[] childFiles = dirInfo.GetFiles();

                                    //foreach (FileInfo childFile in childFiles)
                                    //{
                                    //    try
                                    //    {
                                    //        File.Exists(childFile.FullName);
                                    //        Wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                                    //        Wc.DownloadFileAsync(new Uri(childFile.FullName), Application.StartupPath + "\\" + childFile.Name);
                                    //        progressBarControlDownload.PerformStep();
                                    //        progressBarControlDownload.Update();
                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //    }
                                    //}

                                }
                            }
                        }

                    }
                }
            }
            catch(Exception ex)
            {

            }
           
        }
        private static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            // Displays the operation identifier, and the transfer progress.
            Console.WriteLine("{0}    downloaded {1} of {2} bytes. {3} % complete...",
                (string)e.UserState,
                e.BytesReceived,
                e.TotalBytesToReceive,
                e.ProgressPercentage);
        }
        private ServerInfoUpdate serverInfo = new ServerInfoUpdate();
        private string calltringconect()
        {
            try
            {
                string connectionstring;
                string pathFileName = (Application.StartupPath) + "\\xml\\configiBionet.xml";
                ServerInfoUpdate server = this.LoadFileXml(pathFileName);
                if (server != null && server.Encrypt == "true")
                {
                    this.serverInfo.Encrypt = server.Encrypt;
                    this.serverInfo.ServerName = server.ServerName;
                    this.serverInfo.Database = server.Database;
                    this.serverInfo.UserName = server.UserName; 
                    this.serverInfo.Password = server.Password;
                }
                else
                {
                    this.serverInfo.Encrypt = "false";
                    this.serverInfo.ServerName = "powersoft.vn";
                    this.serverInfo.Database = "Bio";
                    this.serverInfo.UserName = "sa";
                    this.serverInfo.Password = "****";
                }
                connectionstring = "Data Source=" + this.serverInfo.ServerName + ";Initial Catalog=" + this.serverInfo.Database + ";User Id=" + this.serverInfo.UserName + ";Password=" + this.serverInfo.Password + ";";
                return connectionstring;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ServerInfoUpdate LoadFileXml(string FileName)
        {
            using (var stream = System.IO.File.OpenRead(FileName))
            {
                var serializer = new XmlSerializer(typeof(ServerInfoUpdate));
                return serializer.Deserialize(stream) as ServerInfoUpdate;
            }
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            this.BBUpdate();
            Application.Exit();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void CopyfileFull(string _srcPath, string _disPath)// string _currentPath)
        {
            try
            {
                string[] fileName = Directory.GetFiles(_srcPath);
                if (fileName.Length != 0)
                {
                    progressBarControlDownload.Properties.Maximum = fileName.Length;
                    progressBarControlDownload.Properties.Minimum = 0;
                    progressBarControlDownload.Properties.Step = 1;
                    progressBarControlDownload.Properties.PercentView = true;
                    foreach (string filename in fileName)
                    {
                        try
                        {
                            this.lblCopyLink.Text = filename.Substring(_srcPath.Length + 1);
                            this.Refresh();
                            int index = filename.LastIndexOf("\\");
                            string tenfile = filename.Substring(index).Trim('\\');
                            this.lblFileCopy.Text = tenfile;
                            if (tenfile.ToLower() == "BionetUpdate.exe")
                            {
                                //if (System.IO.File.Exists(_currentPath + "\\BionetUpdate.exe"))
                                    continue;
                            }
                            File.Copy(filename, _disPath + filename.Substring(_srcPath.Length), true);
                            progressBarControlDownload.PerformStep();
                            progressBarControlDownload.Update();
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show(" Lỗi copy file: " + ex.ToString() + " \n\t Vui lòng kiểm tra lại!", "Bionet Sàng lọc sơ sinh.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
                string[] Directs = Directory.GetDirectories(_srcPath);
                foreach (string direct in Directs)
                {
                    Directory.CreateDirectory(_disPath + direct.Substring(_srcPath.Length));
                    this.CopyfileFull(direct, _disPath + direct.Substring(_srcPath.Length));// _currentPath);
                }
                DialogResult dlr=XtraMessageBox.Show("Cập nhật phần mềm thành công. \n\t Khởi động lại phần mềm? ", "Bionet Sàng lọc sơ sinh.", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if(dlr==DialogResult.OK)
                {
                    Process.Start(Application.StartupPath + "\\BioNetSangLocSoSinh.exe");
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(" Không tồn tại file copy: " + ex.ToString(), "Bionet Sàng lọc sơ sinh.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }
    }
}

