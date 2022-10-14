using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Models.ViewModels
{
    public class UserViewModel
    {
        public IEnumerable<IdentityUser> Users { get; set; }

        public string SearchString { get; set; }

        public string Filter { get; set; }

        public List<SelectListItem> Columns { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "UserName", Text = "User Name"},
            new SelectListItem { Value = "PasswordHash", Text = "PasswordHash"},
            new SelectListItem { Value = "UnsecuredUserName", Text = "Unsecured UserName"},
        };
    }
}
