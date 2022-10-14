using Application.Models.ViewModels;
using Application.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Net;
using System.Reflection;

namespace Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<IdentityUser> _userRepository;

        public HomeController(
            ILogger<HomeController> logger,
            SignInManager<IdentityUser> signInManager,
            IRepository<IdentityUser> userRepository
            )
        {
            _logger = logger;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }

        public IActionResult Index(string searchString, string chosenFilter)
        {

            if (_signInManager.IsSignedIn(User))
            {
                var viewModel = new UserViewModel();
                var users = _userRepository.GetAll().Where(x => x.Id != "e900c589-a52b-4183-8d2c-34864a0036c7");

                if (!String.IsNullOrEmpty(searchString))
                {
                    switch (chosenFilter)
                    {
                        case "UserName":
                            viewModel.Filter = "UserName";
                            users = users.Where(s => s.UserName.Contains(searchString)).ToList();
                            break;
                        case "PasswordHash":
                            viewModel.Filter = "PasswordHash";
                            users = users.Where(s => s.PasswordHash.Contains(searchString)).ToList();
                            break;
                        case "UnsecuredUserName":
                            viewModel.Filter = "UnsecuredUserName";
                            //users = sqlRequest(searchString);
                            break;
                    }
                }

                viewModel.Users = users;
                viewModel.SearchString = searchString;

                return View(viewModel);
            }
            return View();
        }
        private IEnumerable<IdentityUser> sqlRequest(string searchString)
        {
            List<IdentityUser> users = new List<IdentityUser>();
            var dt = new DataTable();

            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=aspnet-Application-443ED096-10DB-49D1-97CD-BCFC4209136A;Trusted_Connection=True;MultipleActiveResultSets=true";
            string sqlExpression = $"SELECT * FROM AspNetUsers WHERE UserName='{searchString}';";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                var reader = command.ExecuteReader();

                users = ConvertDataTable<IdentityUser>(dt);
            }
            return users;
        }

        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}