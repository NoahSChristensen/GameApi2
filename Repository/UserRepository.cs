using GameApi2.Data;
using GameApi2.Models;
using GameApi2.Services;
using Microsoft.EntityFrameworkCore;

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

    // private readonly DataService _dataService;

    // public UserRepository(DataService dataService)
    // {
    //     _dataService = dataService;
    // }

    private readonly DbContextGameApi _dbContext;

    public UserRepository(DbContextGameApi dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> AddAsync(User user)
    {
        // MED JSON DATABASE
        // var users = await _dataService.LoadUsersAsync();

        // users.Add(user);

        // await _dataService.SaveUsersAsync(users);

        // return user;

        if (string.IsNullOrWhiteSpace(user.Id)) user.Id = Guid.NewGuid().ToString();

        user.Oprettet = DateTime.UtcNow;
        user.Opdateret = DateTime.UtcNow;

        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        // MED JSON DATABASE
        // var user = _user.FirstOrDefault(u => u.Id == id);

        // if (user == null) return Task.FromResult(false);

        // _user.Remove(user);
        // return Task.FromResult(true);

        User? existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (existingUser == null) return false;

        _dbContext.Users.Remove(existingUser);
        await _dbContext.SaveChangesAsync();

        return true;

    }

    public async Task<List<User>> getAllAsync()
    {
        // MED JSON DATABASE
        // var users = await _dataService.LoadUsersAsync();

        // return users;

        var users = await _dbContext.Users.ToListAsync();

        return users;
    }

    public Task<User?> GetByEmail(string email)
    {
        // MED JSON DATABASE
        // var user = _dataService.LoadUsersAsync().Result.FirstOrDefault(u => u.Email == email);
        // return Task.FromResult(user);

        var user = _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        return user;
    }

    public Task GetByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        // MED JSON DATABASE
        // var user = _dataService.LoadUsersAsync().Result.FirstOrDefault(u => u.Id == id);
        // return Task.FromResult(user);

        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> UpdateAsync(User updatedUser)
    {

        // MED JSON DATABASE

        // var user = await _dataService.LoadUsersAsync();


        // var existingUser = user.FirstOrDefault(u => u.Id == updatedUser.Id);

        // if (existingUser == null) return false;

        // existingUser.Name = updatedUser.Name;
        // existingUser.Email = updatedUser.Email;
        // existingUser.Opdateret = DateTime.UtcNow;
        // await _dataService.SaveUsersAsync(user);
        // return true;

        User? existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == updatedUser.Id);

        if (existingUser == null) return false;

        existingUser.Name = updatedUser.Name;
        existingUser.Email = updatedUser.Email;
        existingUser.Opdateret = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        return true;
    }
}