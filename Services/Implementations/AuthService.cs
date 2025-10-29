using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Loan.Api.Models;
using Loan.Api.Models.DTOs;
using Loan.Api.Services.Abstractions;
using Loan.Api.Utils;
using Loan.Api.Utils.EmailTemplates;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Loan.Api.Services.Implementations;

public class AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, GenerateUrl generateUrl, IEmailService emailService) : IAuthService
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly SignInManager<AppUser> _signInManager = signInManager;
    private readonly IConfiguration _configuration = configuration;
    private readonly GenerateUrl _generateUrl = generateUrl;
    private readonly IEmailService _emailService = emailService;

    public async Task<ResponseDTO<string>> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
        if (user != null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = TokenConverter.EncodeToken(token);
            var path = "auth/reset-password";
            var query = $"token={encodedToken}&id={user.Id}";
            var confirmationUrl = _generateUrl.Generate(path, query);

            await _emailService.SendAsync("Reset Password", AuthEmails.ResetPassword(confirmationUrl), user.Email);
        }

        return ResponseDTO<string>.Success("If this email address is associated with an account, you will receive an email with instructions to reset your password.", "If this email address is associated with an account, you will receive an email with instructions to reset your password.", StatusCodes.Status200OK);
    }

    public async Task<ResponseDTO<LoginResponseDTO>> LoginAsync(LoginDTO loginDto, HttpRequest request, HttpResponse response)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return ResponseDTO<LoginResponseDTO>.Fail("Invalid credentials", StatusCodes.Status400BadRequest);
        }

        var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
        if (!result.Succeeded)
        {
            return ResponseDTO<LoginResponseDTO>.Fail("Invalid credentials", StatusCodes.Status400BadRequest);
        }

        var token = await GenerateTokens(user);
        var refreshToken = GenerateRefreshToken(user);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddMinutes(60) // Match JWT expiry
        };

        response.Cookies.Append("access_token", token, cookieOptions);

        return ResponseDTO<LoginResponseDTO>.Success("Login successful", new LoginResponseDTO { Token = token, RefreshToken = refreshToken }, StatusCodes.Status200OK);
    }

    public async Task<ResponseDTO<string>> RefreshTokenAsync(RefreshTokenDTO refreshTokenDto, HttpRequest request, HttpResponse response)
    {
        try
        {
            // Validate and extract the refresh token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidAudience = _configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:RefreshTokenSecretKey"])),
                ClockSkew = TimeSpan.Zero
            };

            // Validate the token and get the principal
            var principal = tokenHandler.ValidateToken(refreshTokenDto.RefreshToken, tokenValidationParameters, out var validatedToken);

            // Extract user ID from the token
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return ResponseDTO<string>.Fail("Invalid refresh token: User ID not found", StatusCodes.Status400BadRequest);
            }
            var userId = userIdClaim.Value;

            // Get the user from the database
            var user = await _userManager.FindByIdAsync(userId) ?? throw new Exception("User not found");

            // Generate a new access token
            var newToken = await GenerateTokens(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddMinutes(60) // Match JWT expiry
            };

            response.Cookies.Append("access_token", newToken, cookieOptions);

            return ResponseDTO<string>.Success("Token refreshed successfully", newToken, StatusCodes.Status200OK);
        }
        catch (SecurityTokenExpiredException)
        {
            throw new Exception("Refresh token has expired");
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            throw new Exception("Invalid refresh token signature");
        }
        catch (Exception ex)
        {
            throw new Exception($"Token refresh failed: {ex.Message}");
        }
    }

    public async Task<ResponseDTO<string>> RegisterAsync(RegisterDTO registerDto)
    {
        AppUser user = new()
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
        };
        IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }

        List<string> roles =
        [
            Constants.Roles.Student
        ];

        await _userManager.AddToRolesAsync(user, roles);
        return ResponseDTO<string>.Success("User registered successfully", user.Id, StatusCodes.Status200OK);
    }

    public async Task<ResponseDTO<string>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDto)
    {
        var user = await _userManager.FindByIdAsync(resetPasswordDto.Id);
        if (user == null)
        {
            return ResponseDTO<string>.Fail("Password Reset Failed");
        }

        var decodedToken = TokenConverter.DecodeToken(resetPasswordDto.Token);
        var result = await _userManager.ResetPasswordAsync(user, decodedToken, resetPasswordDto.Password);

        if (!result.Succeeded)
        {
            return ResponseDTO<string>.Fail("Password Reset Failed", StatusCodes.Status400BadRequest);
        }

        return ResponseDTO<string>.Success("Password Reset Successfully", "Password Reset Successfully");
    }

    private async Task<string> GenerateTokens(AppUser user)
    {
        Console.WriteLine($"Issuer: {_configuration["JwtSettings:Issuer"]}");
        Console.WriteLine($"Audience: {_configuration["JwtSettings:Audience"]}");
        Console.WriteLine($"Expires: {DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpiryInMinutes"] ?? "60"))}");
        Console.WriteLine($"Signing Credentials: {_configuration["JwtSettings:SecretKey"]}");
        List<Claim> claims = [
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString())
            ];

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }


        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpiryInMinutes"] ?? "60")),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken(AppUser user)
    {
        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
            claims: [
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
            ],
            expires: DateTime.UtcNow.AddDays(int.Parse(_configuration["JwtSettings:RefreshTokenExpiryInDays"] ?? "7")),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:RefreshTokenSecretKey"])), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ResponseDTO<string> Logout(HttpResponse response)
    {
        response.Cookies.Delete("access_token");
        return ResponseDTO<string>.Success("User Logged out Successfully", "User Logged out Successfully", StatusCodes.Status200OK);
    }
}