using Loan.Api.Models.DTOs;

namespace Loan.Api.Services.Abstractions;

public interface IAuthService
{
    Task<ResponseDTO<LoginResponseDTO>> LoginAsync(LoginDTO loginDto, HttpRequest request, HttpResponse response);
    Task<ResponseDTO<string>> RegisterAsync(RegisterDTO registerDto);
    Task<ResponseDTO<string>> RefreshTokenAsync(RefreshTokenDTO refreshTokenDto, HttpRequest request, HttpResponse response);
    Task<ResponseDTO<string>> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDto);
    Task<ResponseDTO<string>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDto);
    ResponseDTO<string> Logout(HttpResponse response);
}