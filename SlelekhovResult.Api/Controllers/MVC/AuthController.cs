﻿using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShelekhovResult.DataBase.Models;

namespace SlelekhovResult.Api.Controllers.MVC;

/// <summary>
/// MVC контроллер авторизации
/// </summary>
public class AuthController : Controller
{
    private readonly HttpClient _httpClient;

    public AuthController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    /// <summary>
    /// Контроллер страницы авторизации
    /// </summary>
    /// <returns></returns>
    public IActionResult Login()
    {
        return View();
    }
    
    /// <summary>
    /// Контроллер для атворизации
    /// </summary>
    /// <param name="model">ДТО модели авторизации</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://localhost:7134/api/Account", content);
        
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Ошибка подключения к api. Попробуйте снова.");
            return View(); 
        }
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddHours(12),
            HttpOnly = false
        };
        
        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();
            Response.Cookies.Append("JwtToken", token, cookieOptions);
            return RedirectToAction("GetTrade", "Traded"); 
        }

        ModelState.AddModelError("", "Ошибка авторизации. Попробуйте снова.");
        return View();
    }

    /// <summary>
    /// Контроллер для удаления JWT токена
    /// </summary>
    /// <returns></returns>
    public IActionResult Logout()
    {
        Response.Cookies.Delete("JwtToken");
        return RedirectToAction("Login");
    }
}