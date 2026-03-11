using Microsoft.EntityFrameworkCore;
using GameApi2.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameApi2.Data;

public class DbContextGameApi : DbContext
{

    public DbContextGameApi(DbContextOptions<DbContextGameApi> options)
    : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Order> Orders => Set<Order>();

    public DbSet<Point> Points => Set<Point>();

    public DbSet<Maze> Mazes => Set<Maze>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Point>()
        .ToTable("Point")
        .HasKey(x => x.UserId);
    }
}