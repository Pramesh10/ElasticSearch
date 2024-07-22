using ElasticSearch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch.DataAccess.AppDbContexts
{
    public class AppDbContext : DbContext
    {


        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<AttendanceLog>()
        //        .HasKey(al => new { al.EmployeeId, al.Date });
        //}
        public DbSet<DocumentDetail> DocumentDetails { get; set; }

      
    }
}
