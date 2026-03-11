using System.Drawing;
using GameApi2.DTOs;
using GameApi2.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace GameApi2.Services;


public class PointService
{
    private readonly IPointRepository _pointRepository;
    private readonly IUserRepository _userRepository;


    public PointService(IPointRepository pointRepository, IUserRepository userRepository)
    {
        _pointRepository = pointRepository;
        _userRepository = userRepository;
    }


    public async Task<GetPointDto> GetPointAsync(string userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        var name = user?.Name ?? userId;
        var total = await _pointRepository.GetTotalAsync(userId);

        return new GetPointDto(userId, name, total);
    }


    public async Task<PostPointResponseDto> PostPointAsync(string userId, string source, int amount)
    {
        var safeAmount = amount is >= 1 and <= 100 ? amount : 1;

        var safeSource = string.IsNullOrWhiteSpace(source) ? "Unknown" : source.Trim();

        var newTotal = await _pointRepository.PostPointAsync(userId, safeAmount);

        return new PostPointResponseDto(userId, newTotal, safeAmount, safeSource);
    }


    public async Task<List<GetPointDto>> GetAllPointsAsync()
    {
        var pointsList = await _pointRepository.GetAllOrderedByTotalDescAsync();

        var result = new List<GetPointDto>();

        foreach (var (userId, total) in pointsList)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var name = user
            
            
            
            
            
            
            
            
            
            ?.Name ?? userId;
            result.Add(new GetPointDto(userId, name, total));
        }
        return result;
    }


    public async Task<GetPointDto?> GetPointsForUserAsync(string userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        var name = user?.Name ?? userId;

        if(user == null) return null;

        var total = await _pointRepository.GetTotalAsync(userId);

        return new GetPointDto(userId, name, total);
    } 


    public async Task<GetPointDto?> PutPointAsync(string userId, long total)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        var name = user?.Name ?? userId;

        if(user == null) return null;

        var newTotal = await _pointRepository.PutTotalAsync(userId, total);

        return new GetPointDto(userId, name, newTotal);

    }
}