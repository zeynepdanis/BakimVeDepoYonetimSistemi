using BakimVeDepoYonetimSistemi.Model;

namespace BakimVeDepoYonetimSistemi.Repositories
{
    public class AssetRepository
    {

        private readonly RepositoryContext _context;
       

        public AssetRepository(RepositoryContext context)
        {
            _context = context;
           
        }
         
           public int GetAssetId(string durum)
        {
            try
            {
               var state = _context.VarlikTable.FirstOrDefault(u => u.VarlikAdi == durum);

        if (state != null)
        {
            return state.VarlikId;
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
