using System.Transactions;
using BakimVeDepoYonetimSistemi.Model;
using Microsoft.EntityFrameworkCore;

namespace BakimVeDepoYonetimSistemi.Repositories
{

    public class JobOrderRepository
    {


        private readonly RepositoryContext _context;

        public JobOrderRepository(RepositoryContext context)
        {
            _context = context;
        }

        public void AddIsEmriWithTrigger(JobOrder jobOrder, List<string> calisanEkipUyeleri)
        {

            var bakimTalepId = jobOrder.BakimTalepId; // Örnek bir değer, gerçek değerlerinizi kullanmalısınız.
            var varlikId = jobOrder.VarlikId; // Örnek bir değer, gerçek değerlerinizi kullanmalısınız.
            var aciklama = jobOrder.Aciklama; // Örnek bir açıklama, gerçek değerlerinizi kullanmalısınız.
            var isEmriDurumId = jobOrder.IsEmriDurumId; // Örnek bir durum ID'si, gerçek değerlerinizi kullanmalısınız.

            try
            {
                if(_context.IsEmri.FirstOrDefault(u => u.BakimTalepId == bakimTalepId) is null)
                {
                    var sqlCommand = "INSERT INTO [dbo].[IsEmri] ([VarlikId], [OlusturulmaTarihi], [BakimTalepId], [Aciklama], [IsEmriDurumId]) VALUES (@VarlikId, @OlusturulmaTarihi, @BakimTalepId, @Aciklama, @IsEmriDurumId)";
                var parameters = new object[]
                {
                    new Microsoft.Data.SqlClient.SqlParameter("@VarlikId", varlikId),
                    new Microsoft.Data.SqlClient.SqlParameter("@OlusturulmaTarihi", DateTime.Now),
                    new Microsoft.Data.SqlClient.SqlParameter("@BakimTalepId", bakimTalepId),

                    new Microsoft.Data.SqlClient.SqlParameter("@Aciklama", aciklama),
                    new Microsoft.Data.SqlClient.SqlParameter("@IsEmriDurumId", isEmriDurumId)
                };

                _context.Database.ExecuteSqlRaw(sqlCommand, parameters);



                _context.SaveChanges();

                }
              
                
                var varlik = _context.IsEmri.FirstOrDefault(u => u.BakimTalepId == bakimTalepId);

                foreach (var item in calisanEkipUyeleri)
                {
                    string[] nameSurname = item.Split(' ');

                    var name = nameSurname[0];
                    var surname = nameSurname[nameSurname.Length - 1];

                    var kullaniciId = _context.KullanicilarTable.FirstOrDefault(u => u.Ad == name && u.Soyad == surname).KullaniciId;
                    var ekipUyeId = _context.EkipUye.FirstOrDefault(u => u.KullaniciId == kullaniciId).EkipUyeId;
                    var sqlCommand2 = "UPDATE [dbo].[CalisanEkipUyeleri] SET [EkipUyeId] = @EkipUyeId WHERE [IsEmriId] = @IsEmriId";
                    var parameters_2 = new object[]
                {
                    new Microsoft.Data.SqlClient.SqlParameter("@EkipUyeId", ekipUyeId),
                    new Microsoft.Data.SqlClient.SqlParameter("@IsEmriId", varlik.IsEmriId)

                };

                _context.Database.ExecuteSqlRaw(sqlCommand2, parameters_2);
                _context.SaveChanges();

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Hata oluştu: " + ex.Message);

            }


        
    

    }
    }
}