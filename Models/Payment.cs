namespace Loan.Api.Models;

public class Payment : BaseEntity
{
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public decimal Fee { get; set; }
    public string Reference { get; set; }
}