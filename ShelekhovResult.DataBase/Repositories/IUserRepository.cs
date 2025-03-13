using ShelekhovResult.DataBase.Models;

namespace ShelekhovResult.DataBase.Repositories;

/// <summary>
/// Интерфейс для UserRepository
/// </summary>
public interface IUserRepository
{
    Task<User?> GetUserByUserDomainName(string userDomainName);
    
    Task<Trade?> GetLatestTrade(User user);
}