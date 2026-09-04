using LearningPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatform.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<MasterCourse> MasterCourse { get; set; }

        public DbSet<SubCourse> SubCourse { get; set; }

        public DbSet<AddTopic> AddTopics { get; set; }

        public DbSet<AddMaterial> AddMaterials { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<CourseProgress> CourseProgress { get; set; }

        public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }

        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SubCourse>()
                .Property(x => x.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Purchase>()
                .Property(x => x.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Cart>()
            .Property(x => x.Amount)
            .HasPrecision(18, 2);
            modelBuilder.Entity<User>()
                .HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubCourse>()
                .HasOne(x => x.MasterCourse)
                .WithMany()
                .HasForeignKey(x => x.MasterCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AddTopic>()
                .HasOne(x => x.MasterCourseData)
                .WithMany()
                .HasForeignKey(x => x.MasterCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AddTopic>()
                .HasOne(x => x.SubCourseData)
                .WithMany()
                .HasForeignKey(x => x.SubCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AddMaterial>()
                .HasOne(x => x.MasterCourseData)
                .WithMany()
                .HasForeignKey(x => x.MasterCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AddMaterial>()
                .HasOne(x => x.SubCourseData)
                .WithMany()
                .HasForeignKey(x => x.SubCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AddMaterial>()
                .HasOne(x => x.TopicData)
                .WithMany()
                .HasForeignKey(x => x.TopicId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Subscription>()
                .HasOne(x => x.MasterCourseData)
                .WithMany()
                .HasForeignKey(x => x.MasterCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Subscription>()
                .HasOne(x => x.SubCourseData)
                .WithMany()
                .HasForeignKey(x => x.SubCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Purchase>()
                .HasOne(x => x.UserData)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Purchase>()
                .HasOne(x => x.MasterCourseData)
                .WithMany()
                .HasForeignKey(x => x.MasterCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Purchase>()
                .HasOne(x => x.SubCourseData)
                .WithMany()
                .HasForeignKey(x => x.SubCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Purchase>()
                .HasOne(x => x.SubscriptionData)
                .WithMany()
                .HasForeignKey(x => x.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cart>().HasOne(x => x.UserData)
    .WithMany()
    .HasForeignKey(x => x.UserId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cart>().HasOne(x => x.MasterCourseData)
                .WithMany()
                .HasForeignKey(x => x.MasterCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cart>().HasOne(x => x.SubCourseData)
                .WithMany()
                .HasForeignKey(x => x.SubCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cart>().HasOne(x => x.SubscriptionData)
                .WithMany()
                .HasForeignKey(x => x.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}