using GameApi2.Models;

namespace GameApi2.Repositories;



public interface IPointRepository
{


    Task<long> GetTotalAsync(string userId);

    Task<long> PostPointAsync(string userId, long amount);

    Task<long> PutTotalAsync(string userId, long total);

    Task<IReadOnlyList<(string UserId, long Total)>> GetAllOrderedByTotalDescAsync();
}