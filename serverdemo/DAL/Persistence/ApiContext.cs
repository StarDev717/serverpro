using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DAL.Core.Models;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace DAL.Persistence
{
    public partial class ApiContext : DbContext
    {
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Building> Building { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectionData> ProjectionData { get; set; }
        public virtual DbSet<Report> Report { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MapsDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .Property(b => b.Created)
            .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Building>()
            .Property(b => b.Created)
            .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Employee>()
            .Property(b => b.Created)
            .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Project>()
            .Property(b => b.Created)
            .HasDefaultValueSql("getdate()");

            
            modelBuilder.Entity<ProjectionData>()
            .Property(b => b.Created)
            .HasDefaultValueSql("getdate()");

            
            modelBuilder.Entity<Report>()
            .Property(b => b.Created)
            .HasDefaultValueSql("getdate()");

        }

        //public override int SaveChanges(bool acceptAllChangesOnSuccess)
        //{
        //    OnBeforeSaving();
        //    return base.SaveChanges(acceptAllChangesOnSuccess);
        //}

        //public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    OnBeforeSaving();
        //    return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        //}

        //private void OnBeforeSaving()
        //{
        //    var entries = ChangeTracker.Entries();
        //    foreach (var entry in entries)
        //    {
        //        if (entry.Entity is ITrackable trackable)
        //        {
        //            var now = DateTime.UtcNow;
        //            var user = "";
        //            switch (entry.State)
        //            {
        //                case EntityState.Modified:
        //                    trackable.LastUpdatedAt = now;
        //                    trackable.LastUpdatedBy = user;
        //                    break;

        //                case EntityState.Added:
        //                    trackable.CreatedAt = now;
        //                    trackable.CreatedBy = user;
        //                    trackable.LastUpdatedAt = now;
        //                    trackable.LastUpdatedBy = user;
        //                    break;
        //            }
        //        }
        //    }
        //}

    }
}
