namespace BakimVeDepoYonetimSistemi.Model
{
    public class MaintenanceDetails
    {
         public int TalepId { get; set; }
    public string Aciklama { get; set; }
    public string KullaniciAdiSoyadi { get; set; }
    public string VarlikAdi { get; set; }
    public string Durum { get; set; }
    public DateTime OlusturulmaTarihi { get; set; }

    }
}