using FinanceTracker.Application.PageModels;
using FinanceTracker.Application.Repo;
using FinanceTracker.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Infrastructure.RepoImpementation
{
    public class EfRepository<E> : IRepository<E> where E : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<E> _dbSet;
        private IQueryable<E> _entities => _dbSet;

        public EfRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<E>();
        }

        public async Task<Page<E>> GetPaginatedAsync(
            Expression<Func<E, bool>>? filter = null,
            Func<IQueryable<E>, IOrderedQueryable<E>>? orderBy = null,
            IList<Func<IQueryable<E>, IIncludableQueryable<E, object>>>? includes = null,
            int page = 1,
            int pageSize = 10,
            bool trackChanges = false,
            CancellationToken cancellationToken = default
        )
        {
            IQueryable<E> query = _dbSet.AsQueryable();

            // Apply filters
            if (filter != null)
                query = query.Where(filter);

            // Apply includes (eager loading)
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => include(current));

            // Apply sorting
            if (orderBy != null)
                query = orderBy(query);

            // Apply pagination
            int totalItems = await query.CountAsync(cancellationToken);
            query = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsQueryable();

            // Track changes
            if (!trackChanges)
                query = query.AsNoTracking();

            IReadOnlyList<E> items = await query.ToListAsync(cancellationToken);
            return new Page<E>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        // Add other methods (AddAsync, UpdateAsync, etc.)
    }
}
