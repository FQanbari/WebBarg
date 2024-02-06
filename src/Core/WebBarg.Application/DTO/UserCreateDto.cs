using Microsoft.AspNetCore.Http;

namespace WebBarg.Application.DTO;

public class UserCreateDto
{
    public string Name { get; set; }
    public string Family { get; set; }
    public int CountryId { get; set; }
    public int CityId { get; set; }
    public IFormFile Picture { get; set; }
}