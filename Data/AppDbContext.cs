using GravitySwingData.Models;
using Microsoft.EntityFrameworkCore;
namespace GravitySwingData.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        public DbSet<Users> Users { get; set; } = null!;
        public DbSet<GameRecord> GameRecords { get; set; } = null!;
}