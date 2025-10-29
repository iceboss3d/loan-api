using Loan.Api.Data.Contexts;
using Loan.Api.Data.Repositories.Abstractions;
using Loan.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Loan.Api.Data.Repositories.Implementations;

public class PaymentRepository(AppDbContext context) : GenericRepository<Payment>(context), IPaymentRepository
{
    private readonly AppDbContext _context = context;

    public Task<Payment> GetPaymentByReferenceAsync(string reference)
    {
        return _context.Payments.FirstOrDefaultAsync(x => x.Reference == reference);
    }

    public IQueryable<Payment> GetUserPaymentsAsync(string userId)
    {
        return _context.Payments.Where(x => x.UserId == userId).AsQueryable();
    }
}