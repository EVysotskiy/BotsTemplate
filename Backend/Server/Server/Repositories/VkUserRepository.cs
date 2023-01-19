using System.Linq.Expressions;
using Domain.Model;
using Server.Database;

namespace Server.Repositories
{
    public class VkUserRepository : Repository<VkUser, long, AppDbContext>
    {
        private readonly AppDbContext _dbContext;
        protected override Expression<Func<VkUser, long>> Key => model => model.UserId;
        public VkUserRepository(AppDbContext ctx, AppDbContext dbContext) : base(dbContext, 
            (appDbContext) => appDbContext.VkUsers)
        {
            _dbContext = dbContext;
        }
    }
}