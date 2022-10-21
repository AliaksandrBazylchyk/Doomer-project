using Application.Middlewares.ExceptionMiddlewareModels;
using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application.Pages
{
    public class IndexModel : PageModel
    {

        private readonly ApplicationContext _db;
        private readonly AuthService _authService;

        public UserRegisterModel registerModel { get; set; }
        public UserLoginModel loginModel { get; set; }

        public IndexModel(ApplicationContext db, AuthService authService)
        {
            _db = db;
            _authService = authService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostRegister2(IFormCollection data)
        {
            var model = new UserRegisterModel
            {
                Name = Request.Form["registerModel.Name"],
                Password = Request.Form["registerModel.Password"],
            };

            if (model is null || model.Name is null || model.Password is null) throw new UserException("Wrong inserts");

            var user = _authService.CreateUser(model);

            var result = new OkObjectResult(user);

            return result;
            //return RedirectToPage();
        }
    }
}

