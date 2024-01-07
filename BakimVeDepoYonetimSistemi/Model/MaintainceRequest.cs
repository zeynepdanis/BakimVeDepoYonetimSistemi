namespace BakimVeDepoYonetimSistemi.Model
{
    public class MaintainceRequest
    {
        public int creatorId { get; set; }
        public string demandTitle { get; set; }
        public string demandDescription { get; set; }
        public Asset asset { get; set; }


    }
}