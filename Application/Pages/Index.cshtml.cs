using Application.Middlewares.ExceptionMiddlewareModels;
using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace WebApplication13.Pages
{
    [ValidateReCaptcha("index")]
    public class IndexModel : PageModel
    {

        [BindProperty]
        public string Login { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        private readonly ApplicationContext _db;


        public IndexModel(ApplicationContext db)
        {
            _db = db;
        }

        public async Task OnPost()
        {
            if (!ModelState.IsValid)
            {
                throw new Exception();
            }

            User user = new User
            {
                Name = Login,
                Password = Password,
            };



            var ifExist = await _db.Users.SingleOrDefaultAsync(x => x.Name == user.Name);
            if (ifExist is not null)
            {
                throw new UserException("User already exist.");
            }
            if (user.Password is null)
            {
                throw new UserException("Wrong insert.");
            }

            string reg = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$";
            string log = @"[\w-[\d]]\w{1,9}";

            if (!Regex.IsMatch(user.Password, reg) || !Regex.IsMatch(user.Name, log))
            {
                throw new UserException("Wrong username or password.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }
    }
}

