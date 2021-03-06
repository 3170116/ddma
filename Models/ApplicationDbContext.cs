﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ddma.Models;

namespace ddma.Models
{
    public class ApplicationDbContext: DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<TaskAssignmentGroup> TaskAssignmentGroups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<TaskAssignmentUser> TaskAssignmentUsers { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<TaskLog> TaskLogs { get; set; }
        public DbSet<AssetLog> AssetLogs { get; set; }
        public DbSet<TaskAssignmentAsset> TaskAssignmentAssets { get; set; }

    }
}
