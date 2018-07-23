using BioNetBLL;
using BioNetModel;
using BioNetModel.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace DataSync.BioNetSync
{
    
    public class CTLoiDongBo
    {
        public static string PathDir = Application.StartupPath + "\\CTDongBo";
        public static string pathLoi = PathDir +"\\Sync"+ DateTime.Now.Day +  DateTime.Now.Month + DateTime.Now.Year+".txt";
        
        public static void LoiDongBo(string NoiDungLoi,string TableDongBo,bool trangthai)
        {
            List<PsLoiDongBocs> list = new List<PsLoiDongBocs>();
            PsLoiDongBocs psloi = new PsLoiDongBocs();
            if (File.Exists(pathLoi))
            {
                string text = File.ReadAllText(pathLoi);
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                list = jsonSerializer.Deserialize<List<PsLoiDongBocs>>(text);
          
                psloi.DateDB = DateTime.Now;
                psloi.NoiDungLoi = NoiDungLoi;
                int max = list != null ? list.Count - 1 : 1;
                psloi.STT = list != null ? list[max].STT + 1 : 1;
                psloi.TableSync = TableDongBo;
                if (list == null)
                {
                    list = new List<PsLoiDongBocs>();
                }
                psloi.TrangThaiDB = trangthai;
                list.Add(psloi);
            }
            else
            {
                if (File.Exists(PathDir))
                {
                    Directory.CreateDirectory(PathDir);
                }
                using (StreamWriter sw = File.CreateText(pathLoi))
                psloi.DateDB = DateTime.Now;
                psloi.NoiDungLoi = NoiDungLoi;
                psloi.TableSync = TableDongBo;
                psloi.STT = 1;
                psloi.TrangThaiDB = trangthai;
                list.Add(psloi);
            }
            using (StreamWriter file = File.CreateText(pathLoi))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, list);
            }
        }
    }
}
