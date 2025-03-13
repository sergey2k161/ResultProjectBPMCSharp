using Microsoft.AspNetCore.Mvc;
using ShelekhovResult.DataBase.Models;
using ShelekhovResult.DataBase.Repositories;

namespace SlelekhovResult.Api.Controllers.API;
/// <summary>
/// API контроллер авторизации
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AccountController(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    /// <summary>
    /// Авторизация
    /// </summary>
    /// <param name="model">ДТО модель авторизации</param>
    /// <returns>JWT токен</returns>
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var user = await _userRepository.GetUserByUserDomainName(model.UserDomainName);

        if (user == null)
        {
            return Unauthorized("Пользователь не найден");
        }

        var token = TokenManager.GenerateJwtToken(user, _configuration);

        if (token == null)
        {
            return BadRequest($"Ошибка генерации токена.");
        }
        
        return Ok(token);
    }
    
    
}