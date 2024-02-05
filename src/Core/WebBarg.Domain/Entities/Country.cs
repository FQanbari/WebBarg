using System.ComponentModel.DataAnnotations;

namespace WebBarg.Domain.Entities;

public class Country : BaseEntity
{
    [MaxLength(100)]
    public string Name { get; set; }

    // Navigation property
    public List<City> Cities { get; set; }
}