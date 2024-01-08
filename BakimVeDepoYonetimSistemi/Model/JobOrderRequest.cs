namespace BakimVeDepoYonetimSistemi.Model
{
    public class JobOrderRequest{

       
    public int? BakimTalepId { get; set; }
   
    public string Aciklama { get; set; }
    
    public List<string>CalisanEkipUyeleri { get; set; }
    }
    
}