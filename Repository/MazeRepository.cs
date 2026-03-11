using GameApi2.Data;
using GameApi2.Models;
using Microsoft.EntityFrameworkCore;

namespace GameApi2.Repositories;

public class MazeRepository : IMazeRepository {

    private readonly DbContextGameApi _dbContext;

    public MazeRepository(DbContextGameApi context)
    {
        _dbContext = context;
    }

    public async Task<List<Maze>> getAllAsync(string userId)
    {
        return await _dbContext.Mazes.Where(m => m.UserId == userId).ToListAsync();
    }

    public async Task unlockMazeAsync(string userId, int levelId)
    {
        var exists = await _dbContext.Mazes.FirstOrDefaultAsync(m => m.UserId == userId && m.LevelId == levelId);

        if (exists != null) return;

        var maze = new Maze { UserId = userId, LevelId = levelId };
        _dbContext.Mazes.Add(maze);
        await _dbContext.SaveChangesAsync();
    }
}