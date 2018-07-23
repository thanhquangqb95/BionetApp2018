using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioNetModel
{
    public class PsRptBaoCaoTaiChinhCT
    {
        public int STT { get; set; }
        public string MaDVCS { get; set;}
        public string TenDVCS { get; set; }
        public string DiaChiDVCS { get; set; }
        //Giá tiền
        public int Gia2Benh { get; set; }
        public int Gia3Benh { get; set; }
        public int Gia5Benh { get; set; }
        //SLTông
        public string SL2Benh { get; set; }
        public string SL3Benh { get; set; }
        public string SL5Benh { get; set; }
        public string SLThuMauLai { get; set; }
        //SL Tuàn1
        public string  SL2BenhT1{ get; set; }
        public string SL3BenhT1 { get; set; }
        public string SL5BenhT1 { get; set; }
        public string SLThuMauLaiT1 { get; set; }
        //SL Tuàn2
        public string SL2BenhT2 { get; set; }
        public string SL3BenhT2 { get; set; }
        public string SL5BenhT2 { get; set; }
        public string SLThuMauLaiT2 { get; set; }
        //SL Tuàn3
        public string SL2BenhT3 { get; set; }
        public string SL3BenhT3 { get; set; }
        public string SL5BenhT3 { get; set; }
        public string SLThuMauLaiT3 { get; set; }
        //SL Tuàn4
        public string SL2BenhT4 { get; set; }
        public string SL3BenhT4 { get; set; }
        public string SL5BenhT4 { get; set; }
        public string SLThuMauLaiT4 { get; set; }
        //SL Tuàn5
        public string SL2BenhT5 { get; set; }
        public string SL3BenhT5 { get; set; }
        public string SL5BenhT5 { get; set; }
        public string SLThuMauLaiT5 { get; set; }

        //Tong Tien
        public int TongTienT1 { get; set; }
        public int TongTienT2 { get; set; }
        public int TongTienT3 { get; set; }
        public int TongTienT4 { get; set; }
        public int TongTienT5 { get; set; }
        public int TongTienDV { get; set; }
        public int TongTien { get; set; }
    }
}
