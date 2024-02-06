using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBarg.Domain.Entities;
using WebBarg.Domain.Repos;

namespace WebBarg.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository UserRepository { get; }
    IGenericRepository<City> CityRepository { get; }
    IGenericRepository<Country> CountryRepository { get; }
    Task Commit();
    Task Rollback();
    Task Save();
}