using LearningPlatform.Models;
using Microsoft.EntityFrameworkCore;


namespace LearningPlatform.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<AddTopic> AddTopic { get; set; }
    }
}
