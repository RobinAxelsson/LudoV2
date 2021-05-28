using LudoDataAccess.Models;
using LudoDataAccess.Models.Account;
using Microsoft.EntityFrameworkCore;

namespace LudoDataAccess.Database
{
    public class LudoContext : DbContext
    {
       // public LudoContext(DbContextOptions<LudoContext> options) : base(options) { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<AccountToken> AccountTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
            modelBuilder.Entity<Player>()
                .HasKey(o => new { o.Game, o.AccountId }); //Composite key => only same Account once in every game.

            modelBuilder.Entity<Player>()
                .HasAlternateKey(o => new { o.color, o.Game }); //Unique constraint color and game => no duplicate colors.

            modelBuilder.Entity<Account>()
                .HasAlternateKey(o => new { o.EmailAdress }); //Same e-mail is only allowed once.
            */
            }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsbuilder)
        {
            //hard code connectionstring in onconfiguring method when migratin
            optionsbuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=LudoTest6;Trusted_Connection=True;");
        }
    }
}