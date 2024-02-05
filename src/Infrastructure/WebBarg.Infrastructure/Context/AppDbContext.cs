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
        // Seed countries
        var countries = new List<Country>();
        if (!context.Cities.Any()) 
        {
            countries = GetCountriesWithCities();
            context.Countries.AddRange(countries);
            context.SaveChanges();
        }

        countries = context.Countries.Include(x => x.Cities).ToList();
        // Seed users
        var users = new List<User>();
        var random = new Random();
        if (!context.Users.Any())
        {
            for (int i = 1; i <= 50; i++)
            {
                int index = random.Next(1, 10);
                users.Add(new User
                {
                    Name = $"User{i}",
                    Family = $"Family{i}",
                    Picture = $"Picture{i}",
                    CityId = countries[index].Cities.First().Id,
                    CountryId = countries[index].Id
                });
            }
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



public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{ 
    AppDbContext IDesignTimeDbContextFactory<AppDbContext>.CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Data Source=FATEMEH\\FQ2019;Initial Catalog=WebBargDb;TrustServerCertificate=True;Trusted_Connection=True;");
        //optionsBuilder.UseSqlServer("Data Source=192.168.1.12;Initial Catalog=WebBargDb;User ID=sa;Password=ASdf!@34;TrustServerCertificate=True");

        return new AppDbContext(optionsBuilder.Options);
    }
}