using WebBarg.Application.DTO;
using WebBarg.Domain.Entities;

namespace WebBarg.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User newUser, CancellationToken cancellationToken);
        Task<bool> DeleteUserAsync(int userId, CancellationToken cancellationToken);
        Task<PagedData<UserDto>> GetAllUsersAsync(string filter, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<List<UserStatistics>> GetStatisticsAsync(string filter, CancellationToken cancellationToken);
        Task<User> UpdateUserAsync(int userId, User updatedUser, CancellationToken cancellationToken);
    }
}