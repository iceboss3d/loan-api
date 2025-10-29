using Loan.Api.Data.UnitOfWork.Abstractions;
using Loan.Api.Data.UnitOfWork.Implementations;
using Loan.Api.Services.Abstractions;
using Loan.Api.Services.Implementations;
using Loan.Api.Utils;

namespace Loan.Api.Extensions;

public static class DIServiceExtension
{
    public static void AddDIService(this IServiceCollection services)
    {
        services.AddScoped<GenerateUrl>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, EmailService>();
    }
}