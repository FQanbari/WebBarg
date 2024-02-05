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
    //public DbSet<User> Users { get; set; }
    //public DbSet<City> Cities { get; set; }
    //public DbSet<Country> Countries { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        //Users = Set<User>();
        //Cities = Set<City>();
        //Countries = Set<Country>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var entitiesAssembly = typeof(IEntity).Assembly;

        modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
        modelBuilder.AddRestrictDeleteBehaviorConvention();

        SeedData(modelBuilder);

    }
    private void SeedData(ModelBuilder modelBuilder)
    {

        var cities = new List<City>();
        var countries = new List<Country>();

        for (int i = 0; i < 50; i++)
        {
            var country = new Country { Name = $"Country{i + 1}" };
            countries.Add(country);

            var city = new City { Name = $"City{i + 1}", CountryId = country.Id };
            cities.Add(city);
        }

        //modelBuilder.Entity<Country>().HasData(countries);
        //modelBuilder.Entity<City>().HasData(cities);
        // Seed users
        var users = new List<User>();

        for (int i = 0; i < 50; i++)
        {
            var user = new User
            {
                Name = $"User{i + 1}",
                Family = $"Family{i + 1}",
                Picture = $"user{i + 1}.jpg",
                City = countries[i % 50].Cities[i % 50], // Assign cities in a round-robin fashion
                Country = countries[i % 50] // Assign countries in a round-robin fashion
            };

            users.Add(user);
        }

        modelBuilder.Entity<User>().HasData(users, countries, cities);
    }
}

public class SeedData
{
    public static List<User> GetUsers(ModelBuilder modelBuilder)
    {
        var cities = new List<City>();
        var countries = new List<Country>();

        for (int i = 0; i < 50; i++)
        {
            var country = new Country { Id = i + 1, Name = $"Country{i + 1}" };
            countries.Add(country);

            var city = new City { Id = i + 1, Name = $"City{i + 1}", CountryId = country.Id };
            cities.Add(city);
        }

        modelBuilder.Entity<Country>().HasData(countries);
        modelBuilder.Entity<City>().HasData(cities);
        // Seed users
        var users = new List<User>();

            for (int i = 0; i < 50; i++)
            {
                var user = new User
                {
                    Name = $"User{i + 1}",
                    Family = $"Family{i + 1}",
                    Picture = $"user{i + 1}.jpg",
                    City = countries[i % 50].Cities[i % 50], // Assign cities in a round-robin fashion
                    Country = countries[i % 50] // Assign countries in a round-robin fashion
                };

                users.Add(user);
            }

        return users;
    
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
    public static List<User> GetUsers()
    {


        // Seed initial users with different cities and countries
        var users = new List<User>();
        var random = new Random();

        for (int i = 0; i < 10; i++)
        {
            // Generate random city and country for each user
            var city = new City { Name = $"City{i + 1}" };
            var country = new Country { Name = $"Country{i + 1}" };
            city.Country = country;

            var user = new User
            {
                Name = $"User{i + 1}",
                CityId = random.Next(1,11),
                CountryId = random.Next(1, 11)
            };

            users.Add(user);
        }

        return users;
    }
}

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{ 
    AppDbContext IDesignTimeDbContextFactory<AppDbContext>.CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Data Source=192.168.1.12;Initial Catalog=WebBargDb;User ID=sa;Password=ASdf!@34;TrustServerCertificate=True");

        return new AppDbContext(optionsBuilder.Options);
    }
}