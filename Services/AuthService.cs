using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GameApi2.DTOs;
using GameApi2.Models;
using GameApi2.Repositories;
using Microsoft.IdentityModel.Tokens;



namespace GameApi2.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;
    public AuthService(IUserRepository userRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _config = config;
    }
    public async Task<LoginPostDto?> RegisterAsync(RegisterDto dto)
    {
        if (await _userRepository.GetByEmail(dto.Email) != null)
            return null;

        // Første bruger der registreres bliver Admin; resten bliver User
        List<User> existingUsers = await _userRepository.getAllAsync();
        Console.WriteLine($"Existing users: {existingUsers.Count}");
        // string? role = existingUsers.Count == 0 ? "Admin" : "User";

        string? role = "Admin";



        User? user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = dto.Email,
            Name = dto.Name,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = role,
            Oprettet = DateTime.UtcNow,
            Opdateret = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        var token = GenerateJwt(user);

        return new LoginPostDto { Token = token, User = ToUserInfo(user) };
    }

    public async Task<LoginPostDto?> LoginAsync(LoginDto dto)
    {
        User? user = await _userRepository.GetByEmail(dto.Email);

        if (user == null || string.IsNullOrEmpty(user.PasswordHash)) return null;

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)) return null;

        string? token = GenerateJwt(user);

        return new LoginPostDto
        {
            Token = token,
            User = ToUserInfo(user)
        };
    }


    private string GenerateJwt(User user)
    {
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        Claim[]? claims =
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };


        JwtSecurityToken? token = new JwtSecurityToken(
        issuer: _config["Jwt:Issuer"],
        audience: _config["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddDays(30),
        signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    private static UserInfoDto ToUserInfo(User user)
    {
        return new UserInfoDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Role = user.Role
        };
    }
}


