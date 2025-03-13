using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShelekhovResult.DataBase.Models;

namespace ShelekhovResult.DataBase.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TradeDbContext _context;

    public UserRepository(TradeDbContext context)
    {
        _context = context;
    }
    
    public async Task<User?> GetUserByUserDomainName(string userDomainName)
    {
        try
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserDomainName == userDomainName);
            
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Trade?> GetLatestTrade(User user)
    {
        try
        {
            var trades = await _context.Data
                .Where(u => u.UserId == user.Id)
                .ToListAsync();
            
            return trades
                .Select(t => JsonConvert.DeserializeObject<Trade>(t.Entity))
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefault();
        }
        catch (Exception )
        {
            return null;
        }
    }
}