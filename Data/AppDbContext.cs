using GravitySwingData.Models;
using Microsoft.EntityFrameworkCore;
namespace GravitySwingData.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        public DbSet<Users> Users { get; set; } = null!;
        public DbSet<GameRecord> GameRecords { get; set; } = null!;
        public DbSet<AppSession> AppSessions { get; set; } = null!;
        public DbSet<GameSession> GameSessions { get; set; } = null!;
        public DbSet<UserFeedback> UserFeedbacks { get; set; } = null!;
}