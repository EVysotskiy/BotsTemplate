using System.Linq.Expressions;
using Domain.Model;
using Server.Database;

namespace Server.Repositories
{
    public class TelegramUserRepository : Repository<TelegramUser, long, AppDbContext>
    {
        private readonly AppDbContext _dbContext;
        protected override Expression<Func<TelegramUser, long>> Key => model => model.IdChat;
        public TelegramUserRepository(AppDbContext ctx, AppDbContext dbContext) : base(dbContext, 
            (appDbContext) => appDbContext.TelegramUsers)
        {
            _dbContext = dbContext;
        }
    }
}