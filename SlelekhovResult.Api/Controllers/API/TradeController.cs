using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShelekhovResult.DataBase.Repositories;

namespace SlelekhovResult.Api.Controllers.API;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TradeController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public TradeController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatestTrade([FromQuery] string userDomainName)
    {
        var user = await _userRepository.GetUserByUserDomainName(userDomainName);
        
        if (user == null)
        {
            return NotFound("Пользователь не найден");
        }

        var authenticatedUserDomainName = User.FindFirst(ClaimTypes.Name).Value;
        
        if (authenticatedUserDomainName != user.Id.ToString())
        {
            return Unauthorized("Ошибка авторизации: запрашиваемый пользователь не совпадает с авторизованным.");
        }
        
        var trade = await _userRepository.GetLatestTrade(user);

        if (trade == null)
        {
            return NotFound("Транзакции не найдены");
        }

        return Ok(trade);
    }
}