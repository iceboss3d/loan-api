using Loan.Api.Services.Abstractions;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Loan.Api.Services.Implementations;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly string sendgridKey;

    public SendGridClient Client { get; set; }

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        sendgridKey = _configuration["Email:SendGridKey"];
        Client = new SendGridClient(sendgridKey);
    }
    public async Task SendAsync(string subject, string message, string email)
    {
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("noreply@loanapi.com", "Loan API"),
            Subject = subject,
            HtmlContent = message,
        };

        msg.AddTo(new EmailAddress(email));
        msg.SetClickTracking(true, true);
        await Client.SendEmailAsync(msg);
    }
}