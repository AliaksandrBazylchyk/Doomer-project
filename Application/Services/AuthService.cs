using Application.Middlewares.ExceptionMiddlewareModels;
using Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
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
            // Генерация соли и ее хэширование
            var newSalt = GenerateSalt();
            var saltHash = sha256(newSalt);

            // Объеденение хэша соли с паролем, полученым с формы регистрации
            var hashed = $"{saltHash}{model.Password}";

            // Хэш для бд
            var resultHash = sha256(hashed);

            // Создание объекта, который будет сохранен в бд
            var user = new User
            {
                Name = model.Name,
                Password = resultHash,
                Salt = newSalt
            };

            // Сохраняем созданного юзера в бд
            await _db.Users.AddAsync(user);

            // Сохраняем состояние БД
            await _db.SaveChangesAsync();

            return user;
        }

        public async Task<Token> LoginUser(UserLoginModel model)
        {
            // Проверяем, существует ли юзер?
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Name == model.Name);
            if (user is null) throw new UserException("User doesn't exist");

            // Генерируем хэш по алгоритму, который соответствует алгоритму при регистрации
            var saltHash = sha256(user.Salt);
            var hashed = $"{saltHash}{model.Password}";
            var newPasswHash = sha256(hashed);

            // Если хэши не совпадают, то пробрасываем ошибку, которая отправляет в мидлвейр результат
            if (newPasswHash != user.Password) throw new SessionException("Wrong password");

            // Генерируем клаймы юзер (уникальные данные юзера)
            var claims = await TokenService.GenerateUserClaims(user);

            // Генерируем токен юзера на основе его клаймов
            var token = await _tokenService.GenerateToken(claims);

            var result = new Token
            {
                AccessToken = token,
            };

            return result;
        }

        public async Task<Token> LoginUnsecuredUser(UserLoginModel model)
        {
            List<User> users = new List<User>();
            var dt = new DataTable();

            var sqlExpression = $"SELECT * FROM dbo.Users WHERE Name='{model.Name}' AND Password='{model.Password}';";
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=application;Trusted_Connection=True;MultipleActiveResultSets=true";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                var reader = command.ExecuteReader();
                dt.Load(reader);
                string json = JsonConvert.SerializeObject(dt);
                users = JsonConvert.DeserializeObject<List<User>>(json);
            }

            var claims = await TokenService.GenerateUserClaims(users.ToArray()[0]);
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
