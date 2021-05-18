using System.Linq;
using LudoAPI.Models;
using LudoAPI.Models.Account;

namespace LudoAPI.DataAccess
{
    public class DbRepository : ILudoRepository
    {
        private readonly LudoContext _db;
        public DbRepository(LudoContext db) => _db = db;
        public IQueryable<Account> Accounts => _db.Accounts;
        public IQueryable<Game> Games => _db.Games;
        public IQueryable<AccountToken> AccountTokens => _db.AccountTokens;
        public void Add<TEntityType>(TEntityType entity) => _db.Add(entity);
        public void Remove<TEntityType>(TEntityType entity) => _db.Remove(entity);
        public void SaveChanges() => _db.SaveChanges();
        public void Update<TEntityType>(TEntityType entity) => _db.Remove(entity);
    }
}
