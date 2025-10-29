using Loan.Api.Data.Contexts;
using Loan.Api.Data.Repositories.Abstractions;
using Loan.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Loan.Api.Data.Repositories.Implementations;

public class CardAuthorizationRepository(AppDbContext context) : GenericRepository<CardAuthorization>(context), ICardAuthorizationRepository
{
    private readonly AppDbContext _context = context;


    public Task<CardAuthorization> GetUserCardAuthorizationAsync(string userId, string authorizationId)
    {
        return _context.CardAuthorizations.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == authorizationId);
    }

    public IQueryable<CardAuthorization> GetUserCardAuthorizationsAsync(string userId)
    {
        return _context.CardAuthorizations.Where(x => x.UserId == userId).AsQueryable();
    }
}