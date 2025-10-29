using Loan.Api.Data.Contexts;
using Loan.Api.Data.Repositories.Abstractions;
using Loan.Api.Data.Repositories.Implementations;
using Loan.Api.Data.UnitOfWork.Abstractions;

namespace Loan.Api.Data.UnitOfWork.Implementations;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly AppDbContext _context = context;

    private ICardAuthorizationRepository _cardAuthorizations;
    private IPaymentRepository _payments;
    public ICardAuthorizationRepository CardAuthorizations => _cardAuthorizations ??= new CardAuthorizationRepository(_context);
    public IPaymentRepository Payments => _payments ??= new PaymentRepository(_context);
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}