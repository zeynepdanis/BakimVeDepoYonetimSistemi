using BakimVeDepoYonetimSistemi.Model;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using System.Text;

namespace BakimVeDepoYonetimSistemi.Repositories
{
    public class UserRepository
    {
        private readonly RepositoryContext _context;

        public UserRepository(RepositoryContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUserAsync(User kullanici)
        {

            try
            {
                if (kullanici.Parola != null)
                {
                    string hashedPasswordInput = HashPassword(kullanici.Parola);
                    kullanici.Parola = hashedPasswordInput;
                }
                else
                {
                    kullanici.Parola = null;
                }


                await _context.KullanicilarTable.AddAsync(kullanici);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
        public bool GetByEmail(string email)
        {
            var user = _context.KullanicilarTable.FirstOrDefault(u => u.Mail == email);

            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public User? GetUserByEmail(string email)
        {

            try {  


           var user = _context.KullanicilarTable.FirstOrDefault(u => u.Mail == email);

            if (user == null)
            {
                return null;
            }
            else
            {
                return user;
            }
             }catch (Exception ex)
            {

                Console.WriteLine(ex.Message);   
                return null;
            }

            
        }

        public string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                // Şifre özetleme (hash) işlemi
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Byte'ları string'e dönüştürme
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2")); // Hexadecimal formatına dönüştürme
                }

                return builder.ToString();
            }


        }
         public User? GetUserById(int id)
        {

            try {  


           var user = _context.KullanicilarTable.FirstOrDefault(u => u.KullaniciId == id);

            if (user == null)
            {
                return null;
            }
            else
            {
                return user;
            }
             }catch (Exception ex)
            {

                Console.WriteLine(ex.Message);   
                return null;
            }

            
        }
      

    }
}
