using Loan.Api.Utils;

namespace Loan.Api.Models.DTOs;

public class RegisterDTO
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}

public class LoginDTO
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class RefreshTokenDTO
{
    public string RefreshToken { get; set; } = default!;
}

public class ForgotPasswordDTO
{
    public string Email { get; set; } = default!;
}

public class ResetPasswordDTO
{
    public string Id { get; set; }
    public string Token { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class LoginResponseDTO
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}