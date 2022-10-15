using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;


namespace WebApplication13.Pages
{

    [ValidateReCaptcha("contact")]
    public class ContactModel : PageModel
    {

        [BindProperty]
        public string Login { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        private readonly ApplicationContext _db;


        public ContactModel(ApplicationContext db)
        {
            _db = db;
        }

        public async Task OnPost()
        {
            if (!ModelState.IsValid)
                throw new Exception("Ошибка");

            User user = new User();
            user.Password = this.Password;

            var ifExist = await _db.Users.SingleOrDefaultAsync(x => x.Name == user.Name);
            if (ifExist is not null || user.Password is null)
            {
                throw new Exception("Ошибка");
            }

            string reg = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$";
            if (!Regex.IsMatch(this.Password, reg))
            {
            throw new Exception("Ошибка");
            }
            this.Password = BCrypt.Net.BCrypt.HashPassword(this.Password);
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }
    }
}