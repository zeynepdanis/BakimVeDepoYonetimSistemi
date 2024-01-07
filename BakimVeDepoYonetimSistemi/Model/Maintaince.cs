namespace BakimVeDepoYonetimSistemi.Model
{
    public class Maintaince
    {
            public int TalepId { get; set; }
            public int? EkipUyeId { get; set; }
            public DateTime? OlusturulmaTarihi { get; set; }

            public int? DurumId { get; set; }

            public int? VarlikId { get; set; }

            public string? Aciklama { get; set; }


    }
}
