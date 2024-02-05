using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBarg.Domain.Repos;

namespace WebBarg.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository UserRepository { get; }
    Task Commit();
    Task Rollback();
    Task Save();
}