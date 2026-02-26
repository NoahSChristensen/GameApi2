using GameApi2.DTOs;
using GameApi2.Models;
using GameApi2.Repositories;
namespace GameApi2.Services;

public class UserService
{

    private readonly IUserRepository _userRepository;


    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserGetDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.getAllAsync();
        return users.Select(MapToGetDto).ToList();
    }


    public async Task<UserGetDto?> GetUserByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : MapToGetDto(user);
    }

    public async Task<UserGetDto?> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmail(email);
        return user == null ? null : MapToGetDto(user);
    }

    public async Task<UserGetDto> CreateUserAsync(UserPostDto postDto)
    {
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = postDto.Email,
            Name = postDto.Name,
            Oprettet = DateTime.UtcNow,
            Opdateret = DateTime.UtcNow

        };

        var createdUser = await _userRepository.AddAsync(user);
        return MapToGetDto(createdUser);
    }

    public async Task<UserGetDto?> PatchUserAsync(string id, UserPatchDto patchDto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;

        if (patchDto.Email != null) user.Email = patchDto.Email;
        if (patchDto.Name != null) user.Name = patchDto.Name;

        // user.Email = patchDto.Email ?? user.Email;
        // user.Name = patchDto.Name ?? user.Name;
        user.Opdateret = DateTime.UtcNow;

        var updated = await _userRepository.UpdateAsync(user);
        return updated ? MapToGetDto(user) : null;
    }

    public async Task<UserGetDto?> UpdateUserAsync(string id, UserUpdateDto updateDto)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null) return null;

        if (updateDto.Email != null) user.Email = updateDto.Email;
        if (updateDto.Name != null) user.Name = updateDto.Name;

        user.Opdateret = DateTime.UtcNow;

        var updated = await _userRepository.UpdateAsync(user);
        return updated ? MapToGetDto(user) : null;
    }

    public async Task<bool> DeleteUserAsync(string id, UserDeleteDto? deleteDto = null)
    {
        return await _userRepository.DeleteAsync(id);
    }


    private static UserGetDto MapToGetDto(User user)
    {
        return new UserGetDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Oprettet = user.Oprettet,
            Opdateret = user.Opdateret
        };
    }



}