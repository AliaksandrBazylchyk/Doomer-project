using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SecuredApi.Data.Entities;
using SecuredApi.Models;
using System.Data;

namespace SecuredApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private UserManager<UserEntity> _userManager;
        public AuthController(
            UserManager<UserEntity> userManager
            )
        {
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Login);
            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            return Ok(result);
        }

        [HttpPost("unsecured/login")]
        public async Task<IActionResult> UnsecuredLogin(LoginModel model)
        {
            string connStr = "Host=localhost;Port=5432;Database=usersdb;Username=root;Password=root";
            NpgsqlConnection conn = new NpgsqlConnection(connStr);
            conn.Open();
            string sql = $"SELECT * FROM public.\"AspNetUsers\" WHERE \"UserName\" = '{model.Login}';";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            var result = reader.Read();
            conn.Close();

            return Ok(result);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var hasher = new PasswordHasher<UserEntity>();
            var user = new UserEntity
            {
                UserName = model.Login,
                NormalizedUserName = model.Login.ToUpper()
            };
            user.PasswordHash = hasher.HashPassword(user, model.Password);

            var result = await _userManager.CreateAsync(user);

            return Ok(result);
        }

    }
}
