using Microsoft.EntityFrameworkCore;
using SalesWebApi.Models;

namespace SalesWebApi.Data
{
    public class SalesWebApiContext : DbContext
    {
        public SalesWebApiContext(DbContextOptions<SalesWebApiContext> options) : base(options)
        { }

        public DbSet<Department> Department { get; set; }

        public DbSet<Seller> Seller { get; set; }

        public DbSet<SalesRecord> SalesRecord { get; set; }
    }
}