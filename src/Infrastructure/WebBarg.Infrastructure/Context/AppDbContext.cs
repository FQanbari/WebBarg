using Microsoft.EntityFrameworkCore;
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

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var entitiesAssembly = typeof(IEntity).Assembly;

        modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
        modelBuilder.AddRestrictDeleteBehaviorConvention();

        Seed(modelBuilder);

    }
    private void Seed(ModelBuilder modelBuilder)
    {

        List<Country> countriesWithCities = SeedData.GetCountriesWithCities();
        modelBuilder.Entity<Country>().HasData(countriesWithCities);
    }
}

public class SeedData
{
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
            // Add more countries as needed
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
