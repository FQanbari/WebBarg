﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBarg.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string Family { get; set; }
    public string Picture { get; set; }
    public int CityId { get; set; }
    public int CountryId { get; set; }

    [ForeignKey(nameof(CityId))]
    public City City { get; set; }
    [ForeignKey(nameof(CountryId))]
    public Country Country { get; set; }
}