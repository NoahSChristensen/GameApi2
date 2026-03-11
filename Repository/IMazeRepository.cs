using GameApi2.Models;

namespace GameApi2.Repositories;

public interface IMazeRepository
{

    Task<List<Maze>> getAllAsync(string userId);

    Task unlockMazeAsync(string userId, int levelId);
}