

using AutoMapper;

using BakimVeDepoYonetimSistemi.Model;

namespace BakimVeDepoYonetimSistemi.Repositories{

public class TeamMemberRepository
{

  private readonly RepositoryContext _context;
        private readonly IMapper _mapper;

        public TeamMemberRepository(RepositoryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

   
    public TeamMember? FindUserById(int userId)
    {
            try
            {
                return _context.EkipUye.FirstOrDefault(u => u.kullanici_id == userId);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return null;
            }
          
    }



    


   
}




}
