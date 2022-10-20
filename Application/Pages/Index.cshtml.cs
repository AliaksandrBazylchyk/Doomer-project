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

    }
}

