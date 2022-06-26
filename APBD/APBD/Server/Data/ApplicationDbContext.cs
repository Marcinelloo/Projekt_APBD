using APBD.Server.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBD.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {

        public DbSet<UserCompany> WatchedCompany { get; set; }
        public DbSet<CompanyDetails> Companies { get; set; }
        public DbSet<ApplicationUser> User { get; set; }


        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CompanyDetails>(e =>
        {
            e.ToTable("CompanyDetails");
            e.HasKey(e => e.IdWatchedCompany);

            e.Property(e => e.name).IsRequired();
            e.Property(e => e.ticker).IsRequired();
            e.Property(e => e.sic_description).IsRequired();
            e.Property(e => e.total_employees).IsRequired();
            e.Property(e => e.locale).IsRequired();
            e.Property(e => e.phone_number).IsRequired();
            e.Property(e => e.branding).IsRequired();

            e.Property(e => e.LastDayStocks);
            e.Property(e => e.SevenDaysStocks);
            e.Property(e => e.ThreeMonthsStocks);

        });


            modelBuilder.Entity<UserCompany>(e =>
            {
                e.ToTable("UserCompany");
                e.HasKey(e => new { e.IdUser, e.IdWatchedCompany });


                e.HasOne(e => e.User).WithMany(e => e.UserWatchedCompanies).HasForeignKey(e => e.IdUser);
                e.HasOne(e => e.WatchedCompany).WithMany(e => e.UserWatchedCompanies).HasForeignKey(e => e.IdWatchedCompany);


            });


        }
    }

}

