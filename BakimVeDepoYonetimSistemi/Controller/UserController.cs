using BakimVeDepoYonetimSistemi.Model;
using BakimVeDepoYonetimSistemi.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using static Google.Apis.Auth.OAuth2.Web.AuthorizationCodeWebApp;

namespace BakimVeDepoYonetimSistemi.Controller
{

    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserRepository _userRepository;
        private readonly RepositoryContext _context;


        public UserController(UserRepository userRepository, RepositoryContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddKullanici([FromBody] Kullanici kullanici)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var addedKullanici = await _userRepository.AddKullaniciAsync(kullanici);

                if (addedKullanici != null)
                {
                    return Ok(addedKullanici);
                }
                else
                {
                    return BadRequest("Kullanıcı eklenemedi");
                }
            }
            catch (Exception ex)
            {
                // Loglama veya uygun hata mesajı
                return StatusCode(StatusCodes.Status500InternalServerError, "Bir hata oluştu: " + ex.Message);
            }
        }
        [HttpGet("signin-google")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action(nameof(GoogleResponse)) };
            return Challenge(properties, "Google");
        }

        [HttpGet("Account/signin-callback")]
        public async Task<IActionResult> GoogleResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme); // Değişiklik burada

            if (!authenticateResult.Succeeded)
            {
                // Kimlik doğrulama başarısız oldu
                return BadRequest("Kimlik doğrulama başarısız oldu.");
            }

            var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var surname = authenticateResult.Principal.FindFirst(ClaimTypes.Surname)?.Value;
            var fullName = authenticateResult.Principal.FindFirst(ClaimTypes.Name)?.Value;
            var firstName = authenticateResult.Principal.FindFirst(ClaimTypes.GivenName)?.Value;

            var user = _userRepository.GetByEmail(email);

            Kullanici kullanici = new Kullanici();

            if (!user)
            {
                kullanici.Email = email;
                kullanici.Name = firstName;
                kullanici.Surname = surname;

                var addedKullanici = await _userRepository.AddKullaniciAsync(kullanici);

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
                kullanici.Email = email;
                kullanici.Name = fullName;
                kullanici.Surname = surname;
            }

            return Ok(new { Email = kullanici.Email, Name = kullanici.Name, Surname = kullanici.Surname });
        }



    }
}
