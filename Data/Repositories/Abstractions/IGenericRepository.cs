namespace Loan.Api.Data.Repositories.Abstractions;

public interface IGenericRepository<T> where T : class
{
    Task InsertAsync(T entity);
    Task InsertRangeAsync(IEnumerable<T> entities);
    void Update(T entity);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
}