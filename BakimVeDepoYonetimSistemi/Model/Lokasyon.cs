using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Deneme.Class
{
    public class Lokasyon
    {
        public int LokasyonID { get; set; }

        public string LokasyonAdi { get; set; }

        public int UstLokasyonID { get; set; }
    }
}