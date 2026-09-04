using LearningPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatform.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Existing/shared tables
        public DbSet<User> Users { get; set; }
        public DbSet<MasterCourse> MasterCourses { get; set; }
        public DbSet<SubCourse> SubCourses { get; set; }

        // Your modules
        public DbSet<AddTopic> AddTopics { get; set; }
        public DbSet<AddMaterial> AddMaterials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // AddTopic → MasterCourse
            modelBuilder.Entity<AddTopic>()
                .HasOne(t => t.MasterCourseData)
                .WithMany()
                .HasForeignKey(t => t.MasterCourseId)
                .OnDelete(DeleteBehavior.NoAction);

            // AddTopic → SubCourse
            modelBuilder.Entity<AddTopic>()
                .HasOne(t => t.SubCourseData)
                .WithMany()
                .HasForeignKey(t => t.SubCourseId)
                .OnDelete(DeleteBehavior.NoAction);

            // AddMaterial → MasterCourse
            modelBuilder.Entity<AddMaterial>()
                .HasOne(m => m.MasterCourseData)
                .WithMany()
                .HasForeignKey(m => m.MasterCourseId)
                .OnDelete(DeleteBehavior.NoAction);

            // AddMaterial → SubCourse
            modelBuilder.Entity<AddMaterial>()
                .HasOne(m => m.SubCourseData)
                .WithMany()
                .HasForeignKey(m => m.SubCourseId)
                .OnDelete(DeleteBehavior.NoAction);

            // AddMaterial → AddTopic
            modelBuilder.Entity<AddMaterial>()
                .HasOne(m => m.TopicData)
                .WithMany()
                .HasForeignKey(m => m.TopicId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}