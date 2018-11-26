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
                                    progressBarControlDownload.Properties.Step = 1;
                                    progressBarControlDownload.Properties.PercentView = true;
                                    progressBarControlDownload.Properties.Maximum = 100;
                                    progressBarControlDownload.Properties.Minimum = 0;
                                    WebClient Wc = new WebClient();
                                   
                                    DirectoryInfo dirInfo = new DirectoryInfo(empName);
                                    FileInfo[] childFiles = dirInfo.GetFiles();

                                    foreach (FileInfo childFile in childFiles)
                                    {
                                        try
                                        {
                                            File.Exists(childFile.FullName);
                                            Wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                                            Wc.DownloadFileAsync(new Uri(childFile.FullName), Application.StartupPath + "\\" + childFile.Name);
                                            progressBarControlDownload.PerformStep();
                                            progressBarControlDownload.Update();
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }

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
            //Process.Start(Application.StartupPath + "\\BioNetSangLocSoSinh.exe");
           Application.Exit();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

