using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioNetModel
{
    public class rptBaoCaoExcelGeneCoBan
    {
        //Thống kê mẫu
        public TKExcelSoMau TongSoMau { get; set; }
       // public TKExcelSoMau MauNguyCoThapL1 { get; set; }
        public TKExcelSoMau MauNghiNgo { get; set; }
        public TKExcelSoMau MauNguyCoCao { get; set; }
        public TKExcelSoMau MauNguyCoThapL2 { get; set; }
        public TKExcelSoMau MauDuongTinh { get; set; }
        public TKExcelSoMau MauAMTinh { get; set; }
        //Thống kê gene
        public TKExcelXNGene TongGene { get; set; }
        public TKExcelXNGene GeneTile { get; set; }
        public TKExcelXNGene GeneNghiNgo { get; set; }
        public TKExcelXNGene GeneNguycocao { get; set; }
        public TKExcelXNGene GeneNguycothapL2 { get; set; }
        public TKExcelXNGene GeneAmTinh { get; set; }
        public TKExcelXNGene GeneDuongTinh { get; set; }
      //  public TKExcelXNGene GeneNguycothapL1 { get; set; }
        //CanNang
       // public TKExcelXNGene CanNgangDuoi2500 { get; set; }
        //public TKExcelXNGene CanNgang2500Den3000 { get; set; }
       // public TKExcelXNGene CanNangTren3000 { get; set; }
        //GioiTinh
        //public TKExcelXNGene GioiTinhNam { get; set; }
       // public TKExcelXNGene GioiTinhNu { get; set; }
       // public TKExcelXNGene GioiTinhNA { get; set; }
        //GioiTinh
      //  public TKExcelXNGene CLMauL1KDat { get; set; }
       // public TKExcelXNGene CLMauL1Dat { get; set; }
        //GioiTinh
     //   public TKExcelXNGene CLMauL2KDat { get; set; }
      //  public TKExcelXNGene CLMauL2Dat { get; set; }

    }
    public class TKExcelSoMau
    {
        public string SoLuong { get; set; }
        public string Tile { get; set; }
        public string SLLamGene { get; set; }
        public string SLChuaLamGene { get; set; }
    }
    public class TKExcelXNGene
    {
        public string TongDaLamXNGene { get; set; }
        public string GeneKXD { get; set; }
        public string GeneXD { get; set; }
        public string Conton { get; set; }
        public string Kaiping { get; set; }
        public string Viangchan { get; set; }
        public string Coimbra { get; set; }
        public string Union { get; set; }
        public string Mahidol { get; set; }
        public string Mediterradean { get; set; }
        public string VanuaLava { get; set; }
        public string CantonKaiping { get; set; }
        public string CantonViangchang { get; set; }
        public string UnionViangchan { get; set; }
        public string UnionKaiping { get; set; }
        public string KaipingViangchan { get; set; }
    }
}
