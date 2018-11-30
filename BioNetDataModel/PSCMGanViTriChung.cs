using BioNetModel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioNetModel
{
    public class PSCMGanViTriChung
    {
        public PSCM_GanViTri GGanViTri { get; set; }
        public PSCM_GanViTriCT CTMAYXN001 { get; set; }
        public PSCM_GanViTriCT CTMAYXN002 { get; set; }
        public long IDRowGanXN { get; set; }
        public string IDLanDucLo { get; set; }
        public long? STT_bang { get; set; }
        public string MaPhieu { get; set; }
        public string MaXetNghiem { get; set; }
        public string MaGoiXN { get; set; }
        public string GhiChuChung { get; set; }
        public ViTriXN MAYXN001 { get; set; }
        public ViTriXN MAYXN002 { get; set; }
       // public List<PSCM_GanViTriCT> MAYXN { get; set; }
        public List<PSDanhMucMayXN> may { get; set; }
    }
    public class PSCMGanViTriChungReport
    {
        public string MaPhieu { get; set; }
        public string MaXetNghiem { get; set; }
        public string MaGoiXN { get; set; }
        public string GhiChuChung { get; set; }
        public string TenGoiXN { get; set; }
        public long? STT { get; set; }
        public long? STTDia { get; set; }
        public long? STTVT { get; set; }
        public string ViTri { get; set; }
        public string GhiChuCT { get; set; }
        public bool? isTest { get; set; }
        public bool? isMoi { get; set; }
    }
    public class ViTriXN
    {
        public long? STT { get; set; }
        public long? STTDia { get; set; }
        public long? STTVT { get; set; }
        public string ViTri{get;set;}
        public string GhiChuCT { get; set; }
        public bool? isTest { get; set; }
    }
}
