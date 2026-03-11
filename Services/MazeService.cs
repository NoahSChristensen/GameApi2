using GameApi2.DTOs;
using GameApi2.Models;
using GameApi2.Repositories;

namespace GameApi2.Services;

public class MazeService
{

    private readonly IMazeRepository _mazeRepository;

    public MazeService(IMazeRepository mazeRepository)
    {
        _mazeRepository = mazeRepository;
    }

    public async Task<List<MazeGetDto>> GetMazesAsync(string userId)
    {
        var mazes = await _mazeRepository.getAllAsync(userId);
        return mazes.Select(MapToGetDto).ToList();
    }

    public async Task UnlockNextLevelAsync(string userId, int completedLevelId)
    {
        int nextLevelId = completedLevelId + 1;
        await _mazeRepository.unlockMazeAsync(userId, nextLevelId);
    }

    private static MazeGetDto MapToGetDto(Maze maze)
    {
        return new MazeGetDto
        {
            Id = maze.Id,
            UserId = maze.UserId,
            levelId = maze.LevelId
        };
    }
}