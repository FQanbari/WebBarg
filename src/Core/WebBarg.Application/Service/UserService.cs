using System.Threading;
using WebBarg.Application.DTO;
using WebBarg.Application.Interfaces;
using WebBarg.Domain.Entities;
using WebBarg.Domain.Repos;

namespace WebBarg.Application.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<PagedData<UserDto>> GetAllUsersAsync(string filter, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetListPaging(x => string.IsNullOrWhiteSpace(filter) || x.Name.Contains(filter), cancellationToken, pageSize, pageNumber);
        var result = new PagedData<UserDto>
        {
            PageInfo = new PageInfo
            {
                PageSize = pageSize,
                PageNumber = pageNumber
            }
        };

        result.Data = user.Select(x => new UserDto { Name = x.Name, Family = x.Family, CityName = x.City.Name, CountryName = x.Country.Name }).ToList();
        result.PageInfo.TotalCount = await _unitOfWork.UserRepository.Count(x => string.IsNullOrWhiteSpace(filter) || x.Name.Contains(filter),cancellationToken);
        return result;
    }

    public async Task<User> CreateUserAsync(User newUser, CancellationToken cancellationToken)
    {
        await _userRepository.Create(newUser, cancellationToken);
        await _unitOfWork.Save();
        return newUser;
    }

    public async Task<User> UpdateUserAsync(int userId, User updatedUser, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.Find(x => x.Id == userId, cancellationToken);

        if (existingUser == null)
        {
            // Handle not found scenario
            return null;
        }

        existingUser.Name = updatedUser.Name;
        // Update other properties as needed

        _userRepository.Update(existingUser, cancellationToken);
        await _unitOfWork.Save();

        return existingUser;
    }

    public async Task<bool> DeleteUserAsync(int userId, CancellationToken cancellationToken)
    {
        var userToDelete = await _userRepository.Find(x => x.Id == userId, cancellationToken);

        if (userToDelete == null)
        {
            // Handle not found scenario
            return false;
        }

        _userRepository.Delete(userToDelete, cancellationToken);
        await _unitOfWork.Save();

        return true;
    }
    public async Task<List<UserStatistics>> GetStatisticsAsync(string filter, CancellationToken cancellationToken)
    {
        var statistics = await _userRepository.GetListByCity(x => string.IsNullOrWhiteSpace(filter) || x.Name.Contains(filter), cancellationToken);

        return statistics;
    }
}
//public class CityService
//{
//    private readonly IGenericRepository<City> _cityRepository;
//    private readonly IUnitOfWork _unitOfWork;

//    public CityService(IGenericRepository<City> cityRepository, IUnitOfWork unitOfWork)
//    {
//        _cityRepository = cityRepository;
//        _unitOfWork = unitOfWork;
//    }


//}