namespace BakimVeDepoYonetimSistemi.Repositories
{
    public class MaintenanceRepository
    {

        private readonly RepositoryContext _context;

        public MaintenanceRepository(RepositoryContext context)
        {
            _context = context;
        }

    }
}
