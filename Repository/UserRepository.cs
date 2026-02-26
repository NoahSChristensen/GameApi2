using GameApi2.Models;
using GameApi2.Services;

namespace GameApi2.Repositories;


public class UserRepository : IUserRepository
{
    private readonly List<User> _user = new List<User>
    {

        // new User
        // {
        //     Id = "1",
        //     Email = "test@example.com",
        //     Name = "Test Bruger",
        //     Oprettet = DateTime.UtcNow.AddDays(-10),
        //     Opdateret = DateTime.UtcNow
        // },
        // new User
        // {
        //     Id = Guid.NewGuid().ToString(),
        //     Email = "test2@example.com",
        //     Name = "Test Bruger 2",
        //     Oprettet = DateTime.UtcNow.AddDays(-10),
        //     Opdateret = DateTime.UtcNow
        // }

    };

    private readonly DataService _dataService;

    public UserRepository(DataService dataService)
    {
        _dataService = dataService;
    }

    public async Task<User> AddAsync(User user)
    {
        var users = await _dataService.LoadUsersAsync();

        users.Add(user);

        await _dataService.SaveUsersAsync(users);

        return user;
    }

    public Task<bool> DeleteAsync(string id)
    {
        var user = _user.FirstOrDefault(u => u.Id == id);

        if (user == null) return Task.FromResult(false);

        _user.Remove(user);
        return Task.FromResult(true);

    }

    public async Task<List<User>> getAllAsync()
    {
        var users = await _dataService.LoadUsersAsync();

        return users;
    }

    public Task<User?> GetByEmail(string email)
    {
        var user = _dataService.LoadUsersAsync().Result.FirstOrDefault(u => u.Email == email);
        return Task.FromResult(user);
    }

    public Task GetByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByIdAsync(string id)
    {
        var user = _dataService.LoadUsersAsync().Result.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public async Task<bool> UpdateAsync(User updatedUser)
    {

        var user = await _dataService.LoadUsersAsync();


        var existingUser = user.FirstOrDefault(u => u.Id == updatedUser.Id);

        if (existingUser == null) return false;

        existingUser.Name = updatedUser.Name;
        existingUser.Email = updatedUser.Email;
        existingUser.Opdateret = DateTime.UtcNow;
        await _dataService.SaveUsersAsync(user);
        return true;

    }
}