namespace Loan.Api.Models;

public class CardAuthorization : BaseEntity
{
    public string UserId { get; set; }
    public AppUser User { get; set; } = default!;
    public string AuthorizationCode { get; set; } = default!;
    public string Bin { get; set; } = default!;
    public string Last4 { get; set; } = default!;
    public string ExpMonth { get; set; } = default!;
    public string ExpYear { get; set; } = default!;
    public string Channel { get; set; } = default!;
    public string CardType { get; set; } = default!;
    public string Bank { get; set; } = default!;
    public string CountryCode { get; set; } = default!;
    public string Brand { get; set; } = default!;
    public bool Reusable { get; set; } = default!;
    public string Signature { get; set; } = default!;
    public string AccountName { get; set; } = default!;
}