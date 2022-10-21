using Application.Middlewares.ExceptionMiddlewareModels;
using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    private readonly ApplicationContext _db;
    private readonly AuthService _authService;
    public UserController(ApplicationContext db,
        AuthService authService
        )
    {
        _db = db;
        _authService = authService;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(UserRegisterModel model)
    {
        // Minimum eight characters, at least one letter, one number and one special character
        Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$");

        // НОрм ли данные пришли?
        if (model is null) return BadRequest("Smth going wrong");

        // Валидируем данные
        if (model.Name is null || model.Password is null) throw new UserException("Wrong inserts");
        if (!regex.IsMatch(model.Password)) throw new UserException("Wrong password insert");

        // Существует ли уже юзер с таким неймом
        var ifExist = await _db.Users.FirstOrDefaultAsync(u => u.Name == model.Name);
        if (ifExist is not null) throw new UserException("User already exist");

        // Создаем юзера
        var user = await _authService.CreateUser(model);

        // Возвращаем
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginModel model)
    {
        // минимум 8 символов, 1 маленькая, 1 большая, символ, цифра
        Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$");

        // НОрм ли данные пришли?
        if (model is null) return BadRequest("Smth going wrong");

        // Валидируем данные
        if (model.Name is null || model.Password is null) throw new UserException("Wrong inserts");
        if (!regex.IsMatch(model.Password)) throw new UserException("Wrong password insert");

        // Логинимся и получаем токен, если логин удачный
        var userToken = await _authService.LoginUser(model);

        // Отдаем токен
        return Ok(userToken);
    }
}
