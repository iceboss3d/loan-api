using Loan.Api.Models;

namespace Loan.Api.Data.Repositories.Abstractions;

public interface ICardAuthorizationRepository : IGenericRepository<CardAuthorization>
{
    IQueryable<CardAuthorization> GetUserCardAuthorizationsAsync(string userId);
    Task<CardAuthorization> GetUserCardAuthorizationAsync(string userId, string authorizationId);
}