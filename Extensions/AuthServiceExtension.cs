using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

namespace Loan.Api.Extensions;

public static class AuthenticationServiceExtension
{
    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = configuration["JwtSettings:Audience"],
            ValidIssuer = configuration["JwtSettings:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding
                .UTF8.GetBytes(configuration["JwtSettings:SecretKey"])),
            ClockSkew = TimeSpan.Zero
        };

        services.AddSingleton(tokenValidationParameters);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            // options.DefaultSignInScheme = "Cookies";
        })
       .AddJwtBearer(options =>
       {
           options.SaveToken = true;
           options.TokenValidationParameters = tokenValidationParameters;
           options.Events = new JwtBearerEvents
           {
               OnMessageReceived = context =>
               {
                   // Try to get token from Authorization header first
                   var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                   Console.WriteLine($"Token from header: {token}");

                   // If not found in header, try cookie
                   if (string.IsNullOrEmpty(token))
                   {
                       token = context.Request.Cookies["access_token"];
                       Console.WriteLine($"Token from cookie: {token}");
                   }

                   context.Token = token;
                   return Task.CompletedTask;
               },
               OnAuthenticationFailed = context =>
               {
                   Console.WriteLine($"JWT Authentication failed: {context.Exception.Message}");
                   return Task.CompletedTask;
               }
           };
       });
        //    .AddCookie("Cookies", options =>
        //    {
        //        options.Cookie.Name = "access_token";
        //        options.Cookie.HttpOnly = true;
        //        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //        options.Cookie.SameSite = SameSiteMode.Lax;
        //    });

    }
}