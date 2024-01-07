using BakimVeDepoYonetimSistemi.Model;

namespace BakimVeDepoYonetimSistemi.Repositories
{
    public class TeamRepository
    {

        private readonly RepositoryContext _context;
       

        public TeamRepository(RepositoryContext context)
        {
            _context = context;
           
        }

   
        public string? FindTeamNameById(int Id)
        {
            try
            {
                var team = _context.EkipTable.FirstOrDefault(t => t.EkipId == Id);
                return team?.Tanim;
            }
            catch (Exception)
            {

                return null;
            }
          
        }
         public int? FindTeamIdByName(string name)
        {
            try
            {
                var team = _context.EkipTable.FirstOrDefault(t => t.Tanim == name);
                return team?.EkipId;
            }
            catch (Exception)
            {

                return null;
            }
          
        }
    }
}
