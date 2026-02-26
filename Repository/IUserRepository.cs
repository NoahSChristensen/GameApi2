using GameApi2.Models;

namespace GameApi2.Repositories;

public interface IUserRepository
{

    Task<List<User>> getAllAsync();

    Task<User?> GetByIdAsync(string id);

    Task<User?> GetByEmail(string email);

    Task<User> AddAsync(User user);

    Task<bool> UpdateAsync(User user);

    Task<bool> DeleteAsync(string id);
    Task GetByEmailAsync(string email);
}