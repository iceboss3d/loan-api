using Loan.Api.Data.Contexts;
using Microsoft.EntityFrameworkCore;
namespace Loan.Api.Extensions;

public static class DbContextExtension
{
    private static string ParseConnectionString()
    {
        string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        var databaseUri = new Uri(connectionUrl);
        string db = databaseUri.LocalPath.TrimStart('/');
        string[] userInfo = databaseUri.UserInfo.Split([':'], StringSplitOptions.RemoveEmptyEntries);

        return $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};" +
        $"Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";

    }
    public static void AddDbContextAndConfiguration(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
    {
        services.AddDbContextPool<AppDbContext>(options =>
        {
            string connectionString = env.IsProduction() ? ParseConnectionString() : config.GetConnectionString("default");
            options.UseNpgsql(connectionString);
        });
    }
}