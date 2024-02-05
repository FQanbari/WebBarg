using System.ComponentModel.DataAnnotations;

namespace WebBarg.Domain.Entities;

public class Country : BaseEntity
{
    [MaxLength(100)]
    public string Name { get; set; }

    public List<City> Cities { get; set; }
}