using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioNetModel
{
  public  class PsRptDanhSachDaCapMaXetNghiem
    {
        public string  MaDonVi { get; set; }
        public string  TenDonVi { get; set; }
        public string  MaPhieu { get; set; }
        public string  MaXetNghiem { get; set; }
        public string GhiChu { get; set; }
        public string MaGoiXetNghiem { get; set; }
        public string TenGoiXetNghiem { get; set; }
        public string TenGoiXetNghiemKhongDau { get; set; }
        public string NVCapMa { get; set; }
    }
    public class PSRptDanhSachGanVTXN
    {
        public List<PSCMGanViTriChungReport> PSCMGanViTriChung { get; set; }
        public List<PSDanhSachXN> PSDanhSachXN { get; set; }
    }
    public class PSDanhSachXN
    {
        public string STT { get; set; }
        public string MaGoiXN { get; set; }
        public string TenGoiXN { get; set; }
        public string SL { get; set; }

    }
}
