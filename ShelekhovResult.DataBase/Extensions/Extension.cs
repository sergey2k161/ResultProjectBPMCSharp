using Microsoft.EntityFrameworkCore;
using ShelekhovResult.DataBase.Repositories;

namespace ShelekhovResult.DataBase.Extensions;

public static class Extension
{
    public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddDbContext<TradeDbContext>(options  =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Строка подключения отсутствует!");
            }

            options.UseSqlServer(connectionString);
        });
        
        return services;
    }
}