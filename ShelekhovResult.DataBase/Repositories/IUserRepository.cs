using ShelekhovResult.DataBase.Models;

namespace ShelekhovResult.DataBase.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByUserDomainName(string userDomainName);
    
    Task<Trade?> GetLatestTrade(User user);
}