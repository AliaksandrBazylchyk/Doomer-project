using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApplication13.Pages
{
    public class SafeLoginModel : PageModel
    {

        [BindProperty]
        public string Login { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        private readonly ApplicationContext _db;


        public SafeLoginModel(ApplicationContext db)
        {
            _db = db;
        }
        public async Task OnPost()
        {
            try
            {
                User user = new User();
                user.Name = this.Login;
                user.Password = this.Password;

                var ifExist = await _db.Users.SingleOrDefaultAsync(x => (x.Name == user.Name));
                if (ifExist is null)
                {
                    throw new Exception("ќшибка");
                }
                if (!BCrypt.Net.BCrypt.Verify(user.Password, ifExist.Password))
                {
                    throw new Exception("ќшибка");
                }
                HttpContext.Session.Set<User>("SessionUser", user);
            }
            catch
            {
                throw new Exception("”пс, что-то пошло не так");
            }
        }
    }
}