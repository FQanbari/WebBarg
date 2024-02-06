using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using WebBarg.Domain.Entities;
using WebBarg.Infrastructure.Extensions;

namespace WebBarg.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbContextOptions<AppDbContext> Options { get; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Users = Set<User>();
        Cities = Set<City>();
        Countries = Set<Country>();
        Options = options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var entitiesAssembly = typeof(IEntity).Assembly;

        modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
        modelBuilder.AddRestrictDeleteBehaviorConvention();

        modelBuilder.Entity<City>()
           .HasOne(c => c.Country)
           .WithMany(country => country.Cities)
           .HasForeignKey(c => c.CountryId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.City)
            .WithMany()
            .HasForeignKey(u => u.CityId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Country)
            .WithMany()
            .HasForeignKey(u => u.CountryId);
    }
   

    public static void SeedData( AppDbContext context)
    {
        if (context.Database.IsSqlServer())
        {
            context.Database.Migrate();
        }
        // Seed users
        if (!context.Countries.Any())
        {
            var countries = DataGenerator.GenerateCountries(10);
            context.Countries.AddRange(countries);
            context.SaveChanges();
        }

        if (!context.Cities.Any())
        {
            var cities = DataGenerator.GenerateCities(context.Countries.ToList(), 30);
            context.Cities.AddRange(cities);
            context.SaveChanges();
        }

        if (!context.Users.Any())
        {
            var users = DataGenerator.GenerateUsers(context.Cities.ToList(), context.Countries.ToList(), 100);
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }

    public static List<Country> GetCountriesWithCities()
    {
        var countries = new List<Country>
        {
            new Country { Name = "United States", Cities = GetCitiesForCountry("United States", 10) },
            new Country { Name = "Canada", Cities = GetCitiesForCountry("Canada", 10) },
            new Country { Name = "United Kingdom", Cities = GetCitiesForCountry("United Kingdom", 10) },
            new Country { Name = "Australia", Cities = GetCitiesForCountry("Australia", 10) },
            new Country { Name = "Germany", Cities = GetCitiesForCountry("Germany", 10) },
            new Country { Name = "France", Cities = GetCitiesForCountry("France", 10) },
            new Country { Name = "Japan", Cities = GetCitiesForCountry("Japan", 10) },
            new Country { Name = "Brazil", Cities = GetCitiesForCountry("Brazil", 10) },
            new Country { Name = "India", Cities = GetCitiesForCountry("India", 10) },
            new Country { Name = "South Africa", Cities = GetCitiesForCountry("South Africa", 10) },

        };

        return countries;
    }

    private static List<City> GetCitiesForCountry(string countryName, int numberOfCities)
    {
        var cities = new List<City>();
        for (int i = 1; i <= numberOfCities; i++)
        {
            cities.Add(new City { Name = $"{countryName} City {i}" });
        }
        return cities;
    }
}
public class DataGenerator
{
    public static List<Country> GenerateCountries(int count)
    {
        var fakeCountry = new Faker<Country>()
            .RuleFor(c => c.Name, f => f.Address.Country());

        return fakeCountry.Generate(count);
    }

    public static List<City> GenerateCities(List<Country> countries, int count)
    {
        var fakeCity = new Faker<City>()
            .RuleFor(c => c.Name, f => f.Address.City())
            .RuleFor(c => c.CountryId, f => f.PickRandom(countries).Id);

        return fakeCity.Generate(count);
    }

    public static List<User> GenerateUsers(List<City> cities, List<Country> countries, int count)
    {
        var fakeUser = new Faker<User>()
            .RuleFor(u => u.Name, f => f.Name.FirstName())
            .RuleFor(u => u.Family, f => f.Name.LastName())
            .RuleFor(u => u.Picture, f => f.Image.PicsumUrl())
            .RuleFor(u => u.CityId, f => f.PickRandom(cities).Id)
            .RuleFor(u => u.CountryId, f => f.PickRandom(countries).Id);

        return fakeUser.Generate(count);
    }
}


