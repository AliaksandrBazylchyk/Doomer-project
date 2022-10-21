using Application.Middlewares.ExceptionMiddlewareModels;
using Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public class AuthService
    {
        private readonly ApplicationContext _db;
        private readonly TokenService _tokenService;

        public AuthService(
            ApplicationContext db,
            TokenService tokenService)
        {
            _db = db;
            _tokenService = tokenService;
        }

        public async Task<User> CreateUser(UserRegisterModel model)
        {
            var newSalt = GenerateSalt();
            var saltHash = sha256(newSalt);

            var hashed = $"{saltHash}{model.Password}";
            var resultHash = sha256(hashed);

            var user = new User
            {
                Name = model.Name,
                Password = resultHash,
                Salt = newSalt
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return user;
        }

        public async Task<Token> LoginUser(UserLoginModel model)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Name == model.Name);
            if (user is null) throw new UserException("User doesn't exist");

            var saltHash = sha256(user.Salt);
            var hashed = $"{saltHash}{model.Password}";
            var newPasswHash = sha256(hashed);
            if (newPasswHash != user.Password) throw new SessionException("Wrong password");

            var claims = await TokenService.GenerateUserClaims(user);
            var token = await _tokenService.GenerateToken(claims);

            var result = new Token
            {
                AccessToken = token,
            };

            return result;
        }
        private string GenerateSalt()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        private static string sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }
    }
}
