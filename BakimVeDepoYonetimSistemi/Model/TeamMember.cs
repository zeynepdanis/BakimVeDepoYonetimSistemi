namespace BakimVeDepoYonetimSistemi.Model
{
    public class TeamMember
    {
        public int id { get; set; }
        public int kullanici_id { get; set; }
        public int ekip_id { get; set; }
        public int yetki_id { get; set; }
        public int isgucu_id { get; set; }
        public string sicil_numara { get; set; }
    }
}
