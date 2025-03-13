using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ConsoleApp;

public static class QueryManager
{
    /// <summary>
    /// Авторизация по API
    /// </summary>
    /// <param name="apiUrl">Api</param>
    /// <param name="userDomainName">Логин - UserDomainName</param>
    /// <returns>Токен</returns>
    public static async Task<string> LoginAsync(string apiUrl, string userDomainName)
    {
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(apiUrl), "Недоступное URL API");
            ArgumentException.ThrowIfNullOrEmpty(nameof(userDomainName), "Недоступный логин");

            using (var client = new HttpClient())
            {
                var requestData = new
                {
                    UserDomainName = userDomainName
                };

                var jsonData = JsonSerializer.Serialize(requestData);

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync($"{apiUrl}/api/Account", content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка авторизации: код {response.StatusCode}");
                }
                
                var token = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Токен пользователя: {token}");
                return token;
            }
        }
        catch (ArgumentException ex)
        {
            return $"Произошла ошибка: {ex}";
        }
        catch (JsonException ex)
        {
            throw new JsonException($"Ошибка при десериализации JSON: {ex.Message}");
        }
        catch (Exception ex)
        {
            return $"Произошла ошибка: {ex}";
        }
    }

    /// <summary>
    /// Получение последней сделки по API
    /// </summary>
    /// <param name="apiUrl">Api</param>
    /// <param name="userDomainName">Логин - UserDomainName</param>
    /// <param name="token">JWT токен</param>
    public static async Task GetLatestTradeAsync(string apiUrl, string userDomainName, string token)
    {
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(apiUrl), "Недоступное URL API");
            ArgumentException.ThrowIfNullOrEmpty(nameof(userDomainName), "Недоступный логин");
            ArgumentException.ThrowIfNullOrEmpty(nameof(token), "Отсутствует токен");
            
            var userTimeZone = TimeZoneInfo.Local;

            if (userTimeZone == null)
            {
                Console.WriteLine("Не удалось определить часовой пояс. Используется UTC.");
                userTimeZone = TimeZoneInfo.Utc;
            }
            
            Console.WriteLine($"Часовой пояс: {userTimeZone}");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var requestUrl = $"{apiUrl}/api/Trade/latest?userDomainName={Uri.EscapeDataString(userDomainName)}";

                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonSerializer.Deserialize<ResponseJson>(result);

                    var time = DateTimeOffset.Parse(responseJson.CreatedAt);
                    var localTime = time.ToLocalTime();
                    Console.WriteLine($"Ответ от API");
                    Console.WriteLine($"Сумма: {responseJson.Amount}");
                    Console.WriteLine($"Время в БД: {responseJson.CreatedAt}");
                    Console.WriteLine($"Время локальное: {localTime}");
                }
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex}");
            throw;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex}");
            throw;
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex}");
            throw;
        }
        
    }
}