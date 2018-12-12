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
        public int Tong { get; set; }
        public List<PSThongKeChiTiet> PsThongKeCT {get;set;}
        public PSThongKePPSinh PSTKPPSinh { get; set; }
        public PSThongKeTuoiMe PSThongKeTuoiMe { get; set; }
        public PSThongKeCanNang PSThongKeCanNang { get; set; }
        public PSThongKeChuongTrinh PSTKChuongTrinh { get; set; }
        public PSThongKeCon PSTKCon { get; set; }
        public PSThongKeGoiBenh PSThongKeGoiBenh { get; set; }
        public PSThongKeGioiTinh PSThongKeGioiTinh { get; set; }
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
    public class PSThongKeGioiTinh
    {
        public int Nam { get; set; }
        public int Nu { get; set; }
        public int NA { get; set; }
    }
    public class PSThongKeTuoiMe
    {
        public int Duoi13 { get; set; }
        public int Tuoi13 { get; set; }
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

    public class PSThongKeChuongTrinh
    {
        public int XaHoi { get; set; }
        public int QuocGia { get; set; }
        public int Demo { get; set; }
    }

    public class PSThongKeCon
    {
        public int Sinh3Con { get; set; }
        public int Sinh4Con { get; set; }
        public int SinhTu5Con { get; set; }
    }

    public class PSThongKeGoiBenh
    {
        public int ThuLaiMau { get; set; }
        public int Benh2 { get; set; }
        public int Benh3 { get; set; }
        public int Benh5 { get; set; }
        public int Benh2Hemo { get; set; }
        public int Benh3Hemo { get; set; }
        public int Benh5Hemo { get; set; }
    }

    #region thống kê đơn vi cơ bản
    public class PSThongKePDFDonVi
    {
        public string ThoiGian { get; set; }
        public string DonVi { get; set; }
        public string LuuY { get; set; }
        public string Tong { get; set; }
        public string GTNam { get; set; }
        public string GTNu { get; set; }
        public string TLNamNu { get; set; }
        public List<PSThongKePDFDonViNhom> PSThongKePDFDonViNhom { get; set; }
    }
    public class PSThongKePDFDonViNhom
    {
        public int STT { get; set; }
        public string TenNhom { get; set; }
        public List<PSThongKePDFDonViCT> ThongKe {get;set;}
    }
    public class PSThongKePDFDonViCT
    {
        public string TenThongKe { get; set; }
        public string SoLuong { get; set; }
        public string Tile { get; set; }
    }
    #endregion
    public class PSThongKePDFXetNghiem
    {
        public string ThoiGian { get; set; }
        public string DonVi { get; set; }
        public string LuuY { get; set; }
        public string CanThuLaiL2 { get; set; }
        public string MauDaThuLaiL2 { get; set; }
        public string MauChuaThuLaiL2 { get; set; }
        public List<PSThongKePDFXetNghiemCT> PSThongKePDFXetNghiemCT { get; set; }
    }

    public class PSThongKePDFXetNghiemCT
    {
        public string TenDichVu { get; set; }
        public string MauL1NCCChuaThuLaiMau { get; set; }
        public string MauL2NCC { get; set; }
        public string MauL2NCT { get; set; }
    }

    #region Chỉ tiêu theo xét nghiệm kq
    public class PSThongKePDFDonViXN
    {
        public string ThoiGian { get; set; }
        public string DonVi { get; set; }
        public string LuuY { get; set; }
        public List<PSThongKePDFDonViXNNhom> PSThongKePDFDonViNhom { get; set; }
    }
    public class PSThongKePDFDonViXNNhom
    {
        public int STT { get; set; }
        public string TenNhom { get; set; }
        public string NguyCoThap1 { get; set; }
        public string NguyCoCao1 { get; set; }
        public string Tong1 { get; set; }
        public string NguyCoThap2 { get; set; }
        public string NguyCoCao2 { get; set; }
        public string Tong2 { get; set; }
        public List<PSThongKePDFDonViXNCT> ThongKe { get; set; }
    }
    public class PSThongKePDFDonViXNCT
    {
        public string TenThongKe { get; set; }
        public string NguyCoThap1 { get; set; }
        public string NguyCoCao1 { get; set; }
        public string Tong1 { get; set; }
        public string NguyCoThap2 { get; set; }
        public string NguyCoCao2 { get; set; }
        public string Tong2 { get; set; }

    }
    #endregion


}
