using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBarg.Domain.Entities;

public class City : BaseEntity
{
    [MaxLength(100)]
    public string Name { get; set; }

    // Foreign key
    public int CountryId { get; set; }

    // Navigation property
    public virtual Country Country { get; set; }
}