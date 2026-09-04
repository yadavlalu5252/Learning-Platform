using LearningPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatform.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Subscription> Subscription { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<MasterCourse> MasterCourse { get; set; }
        public DbSet<SubCourse> SubCourse { get; set; }

        public DbSet<Cart> Cart { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

      
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.SubCourseData)
                .WithMany()
                .HasForeignKey(s => s.SubCourseId)
                .OnDelete(DeleteBehavior.NoAction);
        }







    }
}
