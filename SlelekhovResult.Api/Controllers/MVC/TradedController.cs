using System.IdentityModel.Tokens.Jwt;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShelekhovResult.DataBase.Models;

namespace SlelekhovResult.Api.Controllers.MVC;

public class TradedController : Controller
{
    private readonly HttpClient _httpClient;

    public TradedController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Страница для отображения последней сделки
    public async Task<IActionResult> GetTrade()
    {
        // Read the JWT token from the cookie
        var token = Request.Cookies["JwtToken"];

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Account"); // Redirect to the login page if the token is missing
        }

        // Decode the token and extract the "name" claim
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;

        if (string.IsNullOrEmpty(nameClaim))
        {
            return RedirectToAction("Login", "Account"); // Redirect if the "name" claim is missing
        }

        // Add the token to the request header
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Use the "name" claim as the userDomainName in the API request
        var response = await _httpClient.GetAsync($"https://localhost:7134/api/trade/latest?userDomainName={nameClaim}");

        if (response.IsSuccessStatusCode)
        {
            // Deserialize the response content into a Trade object
            var trade = JsonConvert.DeserializeObject<Trade>(await response.Content.ReadAsStringAsync());
            return View(trade);
        }

        return View("Error"); // Show an error page if something went wrong
    }

    public async Task<IActionResult> GetTradeWithJS()
    {
        // Read the JWT token from the cookie
        var token = Request.Cookies["JwtToken"];

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Account"); // Redirect to the login page if the token is missing
        }

        // Decode the token and extract the "name" claim
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
        
        // Экранируем обратный слэш
        var escapedNameClaim = nameClaim.Replace("\\", "\\\\");
        
        ViewData["JwtToken"] = token; // Сохраняем токен в ViewData
        ViewData["NameClaim"] = escapedNameClaim; // Сохраняем nameClaim (userDomainName) в ViewData
        
        return View("GetTradeWithJS");
    }
}