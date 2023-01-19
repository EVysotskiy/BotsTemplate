using System.Linq.Expressions;
using Domain.Model;
using Server.Database;

namespace Server.Repositories
{
    public class ViberUserRepository : Repository<ViberUser, string, AppDbContext>
    {
        private readonly AppDbContext _dbContext;
        protected override Expression<Func<ViberUser, string>> Key => model => model.UserId;
        public ViberUserRepository(AppDbContext ctx, AppDbContext dbContext) : base(dbContext, 
            (appDbContext) => appDbContext.ViberUsers)
        {
            _dbContext = dbContext;
        }
    }
}