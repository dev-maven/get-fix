using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technicians.ViewModels;
using Technicians.Models;

namespace Technicians.Models
{
    public class ProfileContext : IdentityDbContext<ApplicationUser>
    {
        public ProfileContext(DbContextOptions<ProfileContext> options) : base(options)
        {

        }
        public DbSet<Request> Request { get; set; }
        public DbSet<Technicians.Models.Technician> Technician { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<Request>()
        //        .HasKey(c => new { c.RequestId, c.RefId });
        //}
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.HasSequence<int>("RefId", schema: "dbo.Request")
        //        .StartsAt(100)
        //        .IncrementsBy(1);

        //    modelBuilder.Entity<Request>()
        //        .Property(o => o.RefId)
        //        .HasDefaultValueSql("NEXT VALUE FOR dbo.Request.RefId");
        //}
    }
}
