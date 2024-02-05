using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebBarg.Domain.Entities;
using WebBarg.Domain.Repos;
using WebBarg.Infrastructure.Context;

namespace WebBarg.Infrastructure.Repositories;

public class GenericRepository<TEntity>
    : IGenericRepository<TEntity> where TEntity : class, IEntity, new()
{
    protected readonly AppDbContext _context;
    public DbSet<TEntity> dbSet { get; }
    public virtual IQueryable<TEntity> Table => dbSet;

    public GenericRepository(AppDbContext dbContext)
    {
        _context = dbContext;
        dbSet = _context.Set<TEntity>(); 
    }
    public Task Create(TEntity Entity, CancellationToken cancellationToken)
        => dbSet.AddAsync(Entity, cancellationToken).AsTask();

    public Task Create(List<TEntity> Entites, CancellationToken cancellationToken)
        => dbSet.AddRangeAsync(Entites, cancellationToken);

    public async Task Delete(TEntity Entity, CancellationToken cancellationToken)
        => dbSet.Remove(Entity);


    public async Task Delete(List<TEntity> KeyGuids, CancellationToken cancellationToken)
        => dbSet.RemoveRange(KeyGuids);

    public async Task Update(TEntity Entity, CancellationToken cancellationToken)
        => dbSet.Update(Entity);

    public async Task Update(List<TEntity> Entites, CancellationToken cancellationToken)
        => dbSet.UpdateRange(Entites);
    public Task<bool> Existing(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        => dbSet.AnyAsync(predicate, cancellationToken);


    public Task<List<TEntity>> GetAll(CancellationToken cancellationToken)
        => dbSet.AsNoTracking().ToListAsync(cancellationToken);

    public Task<TEntity> Find(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        => dbSet.FirstAsync(predicate, cancellationToken);

    public Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        => dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);

    public Task<int> Count(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        => dbSet.CountAsync(predicate, cancellationToken);

    public Task<int> CountAll(CancellationToken cancellationToken)
        => dbSet.CountAsync(cancellationToken);

    public Task<List<TEntity>> GetListPaging(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, int pageSize = 10, int pageNumber = 1)
        => dbSet.AsNoTracking().Where(predicate).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync(cancellationToken);
}
