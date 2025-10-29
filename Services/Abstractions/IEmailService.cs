namespace Loan.Api.Services.Abstractions;

public interface IEmailService
{
    Task SendAsync(string subject, string message, string email);
}