using System.ComponentModel.DataAnnotations;

namespace WebBarg.Domain.Entities;

public class Country : BaseEntity
{
    [MaxLength(100)]
    public string Name { get; set; }

    // Navigation property
    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}