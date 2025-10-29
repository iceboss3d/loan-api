using Loan.Api.Models;

namespace Loan.Api.Data.Repositories.Abstractions;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    Task<Payment> GetPaymentByReferenceAsync(string reference);
    IQueryable<Payment> GetUserPaymentsAsync(string userId);
}