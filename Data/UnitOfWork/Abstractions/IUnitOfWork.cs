using Loan.Api.Data.Repositories.Abstractions;

namespace Loan.Api.Data.UnitOfWork.Abstractions;

public interface IUnitOfWork
{
    ICardAuthorizationRepository CardAuthorizations { get; }
    IPaymentRepository Payments { get; }
    Task SaveAsync();
}