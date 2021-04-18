using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Models.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Report> Reports { get; set; }
        public DbSet<GeneralInquiry> GeneralInquiries { get; set; }
        public DbSet<ReportComment> ReportComments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            //renaming tables
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(name: "User");
                entity.Property(u => u.ContractStartDate).HasColumnType("date");
                entity.Property(u => u.ContractEndDate).HasColumnType("date");
            });

            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");

            });

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            //seeding tables
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Administrator", NormalizedName = "Administrator".ToUpper() },
                new IdentityRole { Name = "User", NormalizedName = "User".ToUpper() }
                );
        }
    }
}
