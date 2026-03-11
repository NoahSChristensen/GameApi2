// using System.Drawing;
using GameApi2.Data;
using Microsoft.EntityFrameworkCore;
namespace GameApi2.Repositories;

using GameApi2.Models;


public class PointRepository : IPointRepository
{


    private readonly DbContextGameApi _db;


    public PointRepository(DbContextGameApi db)
    {
        _db = db;
    }

    // private static readonly ConcurrentDictionary<string, long> _store = new();

    public async Task<long> GetTotalAsync(string userId)
    {

        var row = await _db.Points.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);

        return row?.Total ?? 0;
    }


    public async Task<long> PostPointAsync(string userId, long amount)
    {
        var row = await _db.Points.FirstOrDefaultAsync(x => x.UserId == userId);
        if (row == null)
        {

            row = new Point { UserId = userId, Total = amount };
            _db.Points.Add(row);
        }
        else
        {
            row.Total += amount;
        }
        await _db.SaveChangesAsync();
        return row.Total;
    }

    public async Task<long> PutTotalAsync(string userId, long total)
    {
        var row = await _db.Points.FirstOrDefaultAsync(x => x.UserId == userId);

        if (row == null)
        {
            row = new Point { UserId = userId, Total = total };
            _db.Points.Add(row);
        }
        else
        {
            row.Total += total;
        }

        await _db.SaveChangesAsync();
        return row.Total;
    }

    public async Task<IReadOnlyList<(string UserId, long Total)>> GetAllOrderedByTotalDescAsync()
    {
        var list = await _db.Points.AsNoTracking()
        .OrderByDescending(x => x.Total)
        .Select(x => new { x.UserId, x.Total })
        .ToListAsync();

        return list.Select(x => (x.UserId, x.Total)).ToList();
    }
}