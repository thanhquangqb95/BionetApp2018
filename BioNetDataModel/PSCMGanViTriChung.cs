using BioNetModel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioNetModel
{
    public class PSCMGanViTriChung
    {
        public int STT_bang { get; set; }
        public string MaPhieu { get; set; }
        public string MaXetNghiem { get; set; }
        public string MaGoiXN { get; set; }
        public string GhiChu { get; set; }
        public ViTriXN MAYXN01 { get; set; }
        public ViTriXN MAYXN02 { get; set; }
        public List<PSDanhMucMayXN> may { get; set; }


    }
    public class ViTriXN
    {
        public long? STT { get; set; }
        public long? STTDia { get; set; }
        public long? STTVT { get; set; }
        public string ViTri{get;set;}
        public string GhiChuCT { get; set; }
        public bool isTest { get; set; }
    }
}
