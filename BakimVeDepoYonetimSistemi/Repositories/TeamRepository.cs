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
                var team = _context.EkipTable.FirstOrDefault(t => t.id == Id);
                return team?.tanim;
            }
            catch (Exception)
            {

                return null;
            }
          
        }
    }
}
