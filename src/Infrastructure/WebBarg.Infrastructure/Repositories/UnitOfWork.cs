using Microsoft.EntityFrameworkCore.Storage;
using WebBarg.Application.Interfaces;
using WebBarg.Domain.Entities;
using WebBarg.Domain.Repos;
using WebBarg.Infrastructure.Context;

namespace WebBarg.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{

    private readonly AppDbContext _context;
    private IUserRepository _userRepository;
    private IGenericRepository<Country> _countryRepository;
    private IGenericRepository<City> _cityRepository;
    private readonly IDbContextTransaction transaction;
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }
    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);
    public IGenericRepository<Country> CountryRepository => _countryRepository ??= new GenericRepository<Country>(_context);
    public IGenericRepository<City> CityRepository => _cityRepository ??= new GenericRepository<City>(_context);

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task Rollback()
    {
        await transaction.RollbackAsync();
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}