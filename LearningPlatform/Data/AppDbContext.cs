<<<<<<< HEAD
﻿using LearningPlatform.Models;
=======
using LearningPlatform.Models;
>>>>>>> origin/main
using Microsoft.EntityFrameworkCore;

namespace LearningPlatform.Data
{
<<<<<<< HEAD
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
=======
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<AddTopic> AddTopic { get; set; }
    }
}
>>>>>>> origin/main
