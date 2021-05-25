using System.Linq;
using LudoDataAccess.Models;
using LudoDataAccess.Models.Account;

namespace LudoDataAccess.Database
{
    public interface ILudoRepository
    {
        IQueryable<Account> Accounts { get; }
        IQueryable<Game> Games { get; }
        IQueryable<Player> Players { get; }
        IQueryable<AccountToken> AccountTokens { get; }
        void Add<TEntityType>(TEntityType entity);
        void Update<TEntityType>(TEntityType entity);
        void Remove<TEntityType>(TEntityType entity);
        void SaveChanges();
    }
}
