using System.Linq.Expressions;
using Domain.Model;
using Server.Database;

namespace Server.Repositories
{
    public class WAUserRepository : Repository<WAUser, string, AppDbContext>
    {
        private readonly AppDbContext _dbContext;
        protected override Expression<Func<WAUser, string>> Key => model => model.PhoneNumber;
        public WAUserRepository(AppDbContext ctx, AppDbContext dbContext) : base(dbContext, 
            (appDbContext) => appDbContext.WAUsers)
        {
            _dbContext = dbContext;
        }
    }
}