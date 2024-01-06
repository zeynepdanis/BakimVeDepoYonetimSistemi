
using BakimVeDepoYonetimSistemi.Model;
using BakimVeDepoYonetimSistemi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BakimVeDepoYonetimSistemi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMemberController : ControllerBase
    {

       private readonly TeamMemberRepository _ekipUyeRepository;

        public TeamMemberController(TeamMemberRepository ekipUyeRepository)
        {
            _ekipUyeRepository = ekipUyeRepository;
        }


    

       
        
    }
}