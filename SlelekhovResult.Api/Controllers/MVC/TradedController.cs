using System.IdentityModel.Tokens.Jwt;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShelekhovResult.DataBase.Models;

namespace SlelekhovResult.Api.Controllers.MVC;

/// <summary>
/// MVC контрорллер для стелок
/// </summary>
public class TradedController : Controller
{
    private readonly HttpClient _httpClient;

    public TradedController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Контроллер для отображения последней сделки авторизованного пользователя С#
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> GetTrade()
    {
        var token = Request.Cookies["JwtToken"];

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth"); 
        }
        
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;

        if (string.IsNullOrEmpty(nameClaim))
        {
            return RedirectToAction("Login", "Account");
        }
        
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync($"https://localhost:7134/api/trade/latest?userDomainName={nameClaim}");

        if (response.IsSuccessStatusCode)
        {
            var trade = JsonConvert.DeserializeObject<Trade>(await response.Content.ReadAsStringAsync());
            return View(trade);
        }

        return View("Error");
    }

    /// <summary>
    /// Контроллер для отображения последней сделки авторизованного пользователя JS
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> GetTradeWithJS()
    {
        var token = Request.Cookies["JwtToken"];

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        // Decode the token and extract the "name" claim
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
        
        var escapedNameClaim = nameClaim?.Replace("\\", "\\\\");
        
        ViewData["JwtToken"] = token; 
        ViewData["NameClaim"] = escapedNameClaim; 
        
        return View("GetTradeWithJS");
    }
}