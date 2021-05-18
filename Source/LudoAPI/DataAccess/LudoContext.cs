using LudoAPI.Models;
using LudoAPI.Models.Account;
using Microsoft.EntityFrameworkCore;

namespace LudoAPI.DataAccess
{
    public class LudoContext : DbContext
    {
        public LudoContext(DbContextOptions<LudoContext> options) : base(options) { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<AccountToken> AccountTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         
        }

    }
}