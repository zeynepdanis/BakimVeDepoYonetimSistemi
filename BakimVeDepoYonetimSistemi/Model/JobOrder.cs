namespace BakimVeDepoYonetimSistemi.Model
{
    public class JobOrder{

         public int IsEmriId { get; set; }
    public int? VarlikId { get; set; }
    public DateTime? OlusturulmaTarihi { get; set; }
    public int? BakimTalepId { get; set; }
    public int? ArizaId { get; set; }
    public int? FirmaId { get; set; }
    public string Aciklama { get; set; }
    public int? IsEmriDurumId { get; set; }
    }
    
}