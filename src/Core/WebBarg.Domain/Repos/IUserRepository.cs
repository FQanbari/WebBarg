using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebBarg.Domain.Entities;

namespace WebBarg.Domain.Repos;

public interface IUserRepository : IGenericRepository<User>
{
    Task<List<UserStatistics>> GetListByCity(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken);
   
    //Task<List<User>> GetListPaging(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken, int pageSize = 10, int pageNumber = 1);
}
