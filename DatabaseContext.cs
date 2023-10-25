using Microsoft.EntityFrameworkCore;
using VolgaIT2023.Models;

using Microsoft.Extensions;

namespace VolgaIT2023
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<Transport> Transports { get; set; }
        public DbSet<Session> Sessions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rent>()
                .HasOne(e => e.RentedTransport)
                .WithMany(e=>e.Rents)
                .HasForeignKey(e => e.TransportId);

            modelBuilder.Entity<Rent>()
                .HasOne(e => e.User)
                .WithMany(e=>e.Rents)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Transport>()
                .HasOne(e => e.Owner)
                .WithMany(e=>e.Transports)
                .HasForeignKey(e => e.OwnerId);
            modelBuilder.Entity<Session>()
                .HasOne(e => e.Owner)
                .WithMany(e => e.Sessions)
                .HasForeignKey(e => e.AccountId);

            modelBuilder.Entity<Account>().HasIndex(e=>e.Username).IsUnique();
            modelBuilder.Entity<Rent>().ToTable(t=>t.HasCheckConstraint("Rents", @"""PriceType"" IN ('Days','Minutes')"));
            modelBuilder.Entity<Transport>().ToTable(t => t.HasCheckConstraint("Transports", @"""TransportType"" IN ('Bike','Car','Scooter')"));
        }
    }
}
