using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebBarg.Domain.Entities;

namespace WebBarg.Domain.Repos;

public interface IGenericRepository<TEntity> where TEntity : class, IEntity, new()
{
    Task Create(TEntity Entity, CancellationToken cancellationToken);

    Task Create(List<TEntity> Entites, CancellationToken cancellationToken);

    Task Update(TEntity Entity, CancellationToken cancellationToken);

    Task Update(List<TEntity> Entites, CancellationToken cancellationToken);

    Task Delete(TEntity Entity, CancellationToken cancellationToken);

    Task Delete(List<TEntity> KeyGuids, CancellationToken cancellationToken);
    Task<List<TEntity>> GetListPaging(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, int pageSize = 10, int pageNumber = 1);

    Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    Task<List<TEntity>> GetAll(CancellationToken cancellationToken);

    Task<TEntity> Find(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    Task<bool> Existing(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    Task<int> Count(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    Task<int> CountAll(CancellationToken cancellationToken);
}
