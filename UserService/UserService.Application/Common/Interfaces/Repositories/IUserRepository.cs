using CustomerService.Domain.Entities;

namespace CustomerService.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    Task CreateUserAsync(User user);
    Task<User?> GetUserByLoginKeyAsync(string loginKey);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(User user);
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
}
