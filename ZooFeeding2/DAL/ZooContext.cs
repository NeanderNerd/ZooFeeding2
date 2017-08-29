using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration.Conventions;
using ZooFeeding2.Models;

namespace ZooFeeding2.DAL
{
    public class ZooContext : DbContext
    {
        public ZooContext() : base("ZooContext")
        {
        }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<Food> Foods { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}