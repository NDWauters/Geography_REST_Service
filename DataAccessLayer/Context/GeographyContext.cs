using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Context
{
    public class GeographyContext : DbContext
    {
        public DbSet<Continent> Continents { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<River> Rivers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Continent>()
                .HasMany(con => con.Countries)
                .WithOne(co => co.Continent)
                .HasForeignKey(co => co.ContinentID);

            modelBuilder.Entity<Country>()
                .HasMany(co => co.Cities)
                .WithOne(ci => ci.Country)
                .HasForeignKey(ci => ci.CountryID);

            modelBuilder.Entity<Country>()
                .HasMany(co => co.Rivers)
                .WithMany(r => r.Countries);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(
                    @"Data Source=.\SQLEXPRESS;Initial Catalog=GeographyDb;Integrated Security=True");
        }
    }
}
