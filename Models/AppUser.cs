using Microsoft.AspNetCore.Identity;

namespace Loan.Api.Models;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
