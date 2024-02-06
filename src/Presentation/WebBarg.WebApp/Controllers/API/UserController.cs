using Microsoft.AspNetCore.Mvc;
using WebBarg.Application.DTO;
using WebBarg.Application.Interfaces;
using WebBarg.Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebBarg.WebApp.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUserService userService, IUnitOfWork unitOfWork)
        {
            this._userService = userService;
            _unitOfWork = unitOfWork;
        }
       
        [HttpGet]
        public async Task<List<Country>> GetCountries(CancellationToken cancellationToken)
        {
            var countries = await _unitOfWork.CountryRepository.GetAll(cancellationToken);
            return countries;
        }

        [HttpGet]
        public async Task<List<City>> GetCitiesByCountry(int countryId, CancellationToken cancellationToken)
        {
            var cities = await _unitOfWork.CityRepository.GetList(x => x.CountryId == countryId, cancellationToken);
            return cities;
        }
        [HttpPost]
        public async Task Createuser([FromForm] UserCreateDto user, CancellationToken cancellationToken)
        {
            var picture = await Upload(user.Picture);
            var data = await _userService.CreateUserAsync(new User { Name = user.Name, Family = user.Family, CityId = user.CityId, CountryId = user.CountryId, Picture = picture }, cancellationToken);
        }
        private async Task<string> Upload(IFormFile picture)
        {
            try
            {
                if (picture == null)
                {
                    throw new Exception("Invalid request data");
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + picture.FileName;
                string uniqueFilePath = Path.Combine(@"wwwroot\images", uniqueFileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), uniqueFilePath);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await picture.CopyToAsync(fileStream);
                }
                return uniqueFilePath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }
        [HttpGet]
        public async Task<List<UserStatistics>> ChartPie(CancellationToken cancellationToken, string? filter = null)
        {
            var data = await _userService.GetStatisticsAsync(filter, cancellationToken);

            return data;
        }
        [HttpGet]
        public async Task<PagedData<UserDto>> GetUsers(CancellationToken cancellationToken, string? filter = null, int? pageNumber = 1, int? pageSize = 10)
        {
            var data = await _userService.GetAllUsersAsync(filter, pageNumber ?? 1, pageSize ?? 10, cancellationToken);

            return data;
        }
    }
}
