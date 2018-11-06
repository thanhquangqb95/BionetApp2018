using BioNetModel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioNetModel
{
    public class PSBaoCaoTuyChonDichVu
    {
        public int STT { get; set; }
        public PSPhieuSangLoc phieu1 { get; set; }
        public PSPhieuSangLoc phieu2 { get; set; }
        public PSPatient patient { get; set; }
        public PSXN_TraKetQua KQ1 { get; set; }
        public PSXN_TraKQ_ChiTiet CTKQ1 { get; set; }
        public PSXN_TraKetQua KQ2 { get; set; }
        public PSXN_TraKQ_ChiTiet CTKQ2 { get; set; }
        public string KetLuan { get; set; }
    }
    public class PSBaoCaoTuyChonDonVi
    {
        public int STT { get; set; }
        public PSPhieuSangLoc phieu1 { get; set; }
        public PSPhieuSangLoc phieu2 { get; set; }
        public PSPatient patient { get; set; }
        public PSXN_TraKetQua KQ1 { get; set; }
        public PSXN_TraKetQua KQ2 { get; set; }
    }
    public class PSThongKeTheoDonVi
    {
        public int STT { get; set; }
        public string MaDV { get; set; }
        public List<PSThongKeChiTiet> PsThongKeCT {get;set;}
        public PSThongKePPSinh PSTKPPSinh { get; set; }
        public PSThongKeTuoiMe PSThongKeTuoiMe { get; set; }
        public PSThongKeCanNang PSThongKeCanNang { get; set; }
    }
    public class PSThongKeChiTiet
    {
        public int STT { get; set; }
        public string TenThongKe { get; set; }
        public int SLuong { get; set; }
    }
    public class PSThongKePPSinh
    {
        public int SinhThuong { get; set; }
        public int SinhMo { get; set; }
        public int NA { get; set; }
    }
    public class PSThongKeTuoiMe
    {
        public int Duoi13 { get; set; }
        public int Tuoi14 { get; set; }
        public int Tuoi15 { get; set; }
        public int Tuoi16 { get; set; }
        public int Tuoi17 { get; set; }
        public int Tuoi17den20 { get; set; }
        public int Tuoi20den25 { get; set; }
        public int Tuoi25den30 { get; set; }
        public int Tuoi30den35 { get; set; }
        public int Tuoi35den40 { get; set; }
        public int Tuoi40den45 { get; set; }
        public int TuoiTren45 { get; set; }
    }
    public class PSThongKeCanNang
    {
        public int Duoi25 { get; set; }
        public int Tu25Den30 { get; set; }
        public int Tu30Den35 { get; set; }
        public int Tu35Den40 { get; set; }
        public int Tu40Den45 { get; set; }
        public int Tu45Den50 { get; set; }
        public int Tren50 { get; set; }
    }
}
