using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using WebBarg.Domain.Entities;
using WebBarg.Domain.Repos;
using WebBarg.Infrastructure.Context;

namespace WebBarg.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context)
        :base(context)
    {
    }

    public async Task<List<UserStatistics>> GetListByCity(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken)
    {
        var userList = await Table.Include(x => x.City).Include(x => x.Country).AsNoTracking()
        .Where(predicate)
        .ToListAsync(cancellationToken);

        var groupedUsers = userList
            .GroupBy(u => u.City.Name)
            .Select(group => new UserStatistics
            {
                CityName = group.Key,
                UserCount = group.Count(),
                Percentage = (double)group.Count() / userList.Count * 100
            })
            .ToList();

        return groupedUsers;
    }
}
