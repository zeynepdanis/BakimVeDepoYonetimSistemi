
using BakimVeDepoYonetimSistemi.Model;
using BakimVeDepoYonetimSistemi.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BakimVeDepoYonetimSistemi.Controller
{
    
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;
     
        private readonly TeamMemberRepository _ekipUyeRepository;
        private readonly TeamRepository _ekipRepository;

        public AuthController(UserRepository userRepository, TeamMemberRepository ekipUyeRepository, TeamRepository ekipRepository)
        {
            _userRepository = userRepository;
            _ekipUyeRepository = ekipUyeRepository;
            _ekipRepository = ekipRepository;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(User kullanici)
        {
            
                
                    try{
                        var isregistered = _userRepository.AddUserAsync(kullanici);

                        if(isregistered.Result)
                        {
                            return Ok("Kullanıcı eklendi");
                        }
                        else
                        {
                            return BadRequest("Kullanıcı eklenemedi");
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return BadRequest("Kullanıcı eklenemedi");
                    }
            }


        [HttpPost("login")]
        public IActionResult Login(Login login)
        {
            try
            {
                var user = _userRepository.GetUserByEmail(login.mail);
                if(user == null)
                {
                    return BadRequest("Kullanıcı bulunamadı");
                }
              
                try
                {
                    var teamMember = _ekipUyeRepository.FindUserById(user.KullaniciId);
                    if(teamMember == null)
                    {
                        return BadRequest("Kullanıcı bulunamadı");
                    }
                    try
                    {
                        var team = _ekipRepository.FindTeamNameById(teamMember.EkipId);
                        if (team == null)
                        {
                            return BadRequest("Kullanıcı bulunamadı");
                        }
                        else
                        {
                            login.password = _userRepository.HashPassword(login.password);
                            if (user.Parola == login.password)
                            {
                                var response = new LoginResponse
                                {
                                    userType = team,
                                    userId = user.KullaniciId
                                };
                                return Ok(response);
                            }
                            else
                            {
                                return BadRequest("Şifre hatalı");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return BadRequest("Kullanıcı bulunamadı");
                    }

                    

                 
                }
                catch (System.Exception)
                {

                    return BadRequest("Kullanıcı bulunamadı");
                }   
            }
            catch (Exception)
            {

                return BadRequest("Kullanıcı bulunamadı");
            }
           
          }
         
        
        


    

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleLoginCallback))
            };

            return Challenge(authenticationProperties, "Google");
        }

        [HttpGet("google-login-callback")]
        public async Task<IActionResult> GoogleLoginCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync("Google");

            if (!authenticateResult.Succeeded)
            {
                // Handle authentication failure
                return BadRequest();
            }

            var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var surname = authenticateResult.Principal.FindFirst(ClaimTypes.Surname)?.Value;
            var fullName = authenticateResult.Principal.FindFirst(ClaimTypes.Name)?.Value;
            var firstName = authenticateResult.Principal.FindFirst(ClaimTypes.GivenName)?.Value;

            var user = _userRepository.GetByEmail(email);

            User kullanici = new User();

            if (!user)
            {
                kullanici.Mail = email;
                kullanici.Ad = firstName;
                kullanici.Soyad = surname;

                var addedKullanici = await _userRepository.AddUserAsync(kullanici);

                if (addedKullanici != null)
                {
                    return Ok(addedKullanici);
                }
                else
                {
                    return BadRequest("Kullanıcı eklenemedi");
                }
            }
            else
            {
                kullanici.Mail = email;
                kullanici.Ad = fullName;
                kullanici.Soyad = surname;
            }

            return Ok(new { Email = kullanici.Mail, Name = kullanici.Ad, Surname = kullanici.Soyad });
        }
    }
}

