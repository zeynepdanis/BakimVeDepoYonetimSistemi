

﻿namespace BakimVeDepoYonetimSistemi.Model
{
    public class Varlık
    {
        public int VarlikID { get; set; }

        public int VarlikTuruID { get; set; }

        public string VarlikAdi { get; set; }

        public string Marka { get; set; }

        public string Model { get; set; }

        public int SeriNo { get; set; }

        public int DurumID { get; set; }

        public int LokasyonID { get; set; }

        public string AlindigiTarih { get; set; }

    }
}
