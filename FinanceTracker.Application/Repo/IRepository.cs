using FinanceTracker.Application.PageModels;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Application.Repo
{
    public interface IRepository<E> where E : class
    {
        Task<Page<E>> GetPaginatedAsync(
            Expression<Func<E, bool>>? filter = null,
            Func<IQueryable<E>, IOrderedQueryable<E>>? orderBy = null,
            IList<Func<IQueryable<E>, IIncludableQueryable<E, object>>>? includes = null,
            int page = 1,
            int pageSize = 10,
            bool trackChanges = false,
            CancellationToken cancellationToken = default
        );
        //Task<E> GetAsync(int id, CancellationToken cancellationToken = default);
        //Task AddAsync(E entity, CancellationToken cancellationToken = default);
        //Task UpdateAsync(E entity, CancellationToken cancellationToken = default);
        //Task DeleteAsync(E entity, CancellationToken cancellationToken = default);
        //Task<int> CountAsync(Expression<Func<E, bool>>? filter = null, CancellationToken cancellationToken = default);
        //Task<bool> AnyAsync(Expression<Func<E, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
