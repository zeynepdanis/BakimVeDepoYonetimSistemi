using System.Data.SqlClient;
using System.Linq;
using BakimVeDepoYonetimSistemi.Model;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BakimVeDepoYonetimSistemi.Repositories
{
    public class MaintenanceRepository
    {

        private readonly RepositoryContext _context;

        public MaintenanceRepository(RepositoryContext context)
        {
            _context = context;
        }
        public int InsertBakimTalep(int ekipUyeId, DateTime olusturulmaTarihi, int durumId, int varlikId, string aciklama)
        {
            try
            {
                var ekipUyeIdParam = new Microsoft.Data.SqlClient.SqlParameter("@EkipUyeId", ekipUyeId);
                var olusturulmaTarihiParam = new Microsoft.Data.SqlClient.SqlParameter("@OlusturulmaTarihi", olusturulmaTarihi);
                var durumIdParam = new Microsoft.Data.SqlClient.SqlParameter("@DurumId", durumId);
                var varlikIdParam = new Microsoft.Data.SqlClient.SqlParameter("@VarlikId", varlikId);
                var aciklamaParam = new Microsoft.Data.SqlClient.SqlParameter("@Aciklama", aciklama);

                var rowsAffected = _context.Database.ExecuteSqlRaw("EXEC Usp_InsertBakimTalep @EkipUyeId, @OlusturulmaTarihi, @DurumId, @VarlikId, @Aciklama",
                                                                  ekipUyeIdParam, olusturulmaTarihiParam, durumIdParam, varlikIdParam, aciklamaParam);

                return rowsAffected;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata oluştu: " + ex.Message);
                return -1;
            }
        }


        public List<Maintaince> GetAllBakimTalep()
        {
            try
            {
                var maintainces = _context.BakimTalep.FromSqlRaw("EXEC Usp_GetAllBakimTalep").ToList();
                return maintainces;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata oluştu: " + ex.Message);
                return null; // Hata durumunu işaretlemek için null ya da başka bir uygun değer döndürebilirsiniz
            }
        }


        public List<object> GetAllMaintainceMembersByTeamType(string maintenanceTeamType)
        {
            try
            {
                var bakimEkipId = _context.EkipTable.FirstOrDefault(e => e.Tanim == maintenanceTeamType)?.EkipId;
                if (bakimEkipId != null)
                {
                    var bakimEkipUyeleri = (from eu in _context.EkipUye
                                            join ku in _context.KullanicilarTable on eu.KullaniciId equals ku.KullaniciId
                                            where eu.EkipId == bakimEkipId
                                            select new { ku.Ad, ku.Soyad, eu.Sicil }).ToList<object>();

                    return bakimEkipUyeleri;
                }
                return null;
            }
            catch (Exception ex)
            {
                // Hata durumunda buraya düşer
                // Hata durumunu işleyebilir ve uygun bir şekilde geri dönüş yapabiliriz
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                // Hata mesajı ya da uygun bir geri dönüş nesnesi oluşturulabilir
                return null;
            }

        }
        public List<MaintenanceDetails> GetMaintenanceDetails()
        {
            try
            {
                var query = from bt in _context.BakimTalep
                            join eu in _context.EkipUye on bt.EkipUyeId equals eu.EkipUyeId into ekipUyeJoin
                            from euJoined in ekipUyeJoin.DefaultIfEmpty()
                            join ku in _context.KullanicilarTable on euJoined.KullaniciId equals ku.KullaniciId into kullaniciJoin
                            from kuJoined in kullaniciJoin.DefaultIfEmpty()
                            join v in _context.VarlikTable on bt.VarlikId equals v.VarlikId into varlikJoin
                            from vJoined in varlikJoin.DefaultIfEmpty()
                            join t in _context.TalepDurumTable on bt.DurumId equals t.DurumId into talepDurumJoin
                            from tJoined in talepDurumJoin.DefaultIfEmpty()
                            select new MaintenanceDetails
                            {
                                TalepId = bt.TalepId,
                                Aciklama = bt.Aciklama,
                                KullaniciAdiSoyadi = kuJoined.Ad + " " + kuJoined.Soyad,
                                VarlikAdi = vJoined.VarlikAdi,
                                Durum = tJoined.Aciklama,
                                OlusturulmaTarihi = bt.OlusturulmaTarihi ?? DateTime.MinValue
                            };

                return query.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata oluştu: " + ex.Message);
                return null;
            }
        }



public string GetEmailForEkipUyeId(int ekipUyeId)
{
    try
    {
        var email = _context.BakimTalep
            .Where(bt => bt.EkipUyeId == ekipUyeId)
            .Join(
                _context.EkipUye,
                bt => bt.EkipUyeId,
                eu => eu.EkipUyeId,
                (bt, eu) => new { bt, eu }
            )
            .Join(
                _context.KullanicilarTable,
                temp => temp.eu.KullaniciId,
                kt => kt.KullaniciId,
                (temp, kt) => kt.Mail
            )
            .FirstOrDefault(); // Tek bir e-posta adresi döndürmek için FirstOrDefault()

        return email;
    }
    catch (System.Exception)
    {
        throw;
    }
}






        public int GetStateId(string durum)
        {
            try
            {
                var state = _context.TalepDurumTable.FirstOrDefault(u => u.Aciklama == durum);

                if (state != null)
                {
                    return state.DurumId;
                }
                else
                {

                    return -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata oluştu: " + ex.Message);
                return -1;
            }
        }

    }




}

