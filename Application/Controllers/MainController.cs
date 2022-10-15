
using Application.Middlewares.ExceptionMiddlewareModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

[ApiController]
[Route("api/[controller]")]
public class MainController : ControllerBase
{

    private readonly ApplicationContext _db;

    public MainController(ApplicationContext db)
    {
        _db = db;
    }

    [HttpPost("unregistration")]
    public async Task<User> UnRegistration(User user)
    {
        var ifExist = await _db.Users.SingleOrDefaultAsync(x => x.Name == user.Name);
        if (ifExist is not null || user.Password is null)
        {
            throw new UserException("User already exist or wrong password insert.");
        }

        string reg = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$";
        if (!Regex.IsMatch(user.Password, reg))
        {
            throw new UserException("Wrong password insert.");
        }
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();

        return user;
    }

    [HttpPost("login")]
    public async Task<bool> Login(User user)
    {

        string query =
        $"select * FROM users WHERE users.Name = '{user.Name}' and users.password = '{user.Password}'";

        var us = _db.Users.FromSqlRaw(query).ToList();
        Console.WriteLine(us[0].Password);
        if (us is not null && us.Count == 1)
        {
            HttpContext.Session.Set("SessionUser", user);
            return true;
        }

        throw new Exception("Ошибка");

    }

    [HttpGet("isSession")]
    public bool isSession()

    {

        if (HttpContext.Session.Keys.Contains("SessionUser"))
        {
            return true;
        }

        throw new SessionException("Can not initiate user session.");

    }
    [HttpGet("DeSession")]
    public void DeSession()

    {
        try
        {

            HttpContext.Session.Clear();

        }
        catch
        {
            throw new SessionException("Can not clear user session.");
        }
    }
}
