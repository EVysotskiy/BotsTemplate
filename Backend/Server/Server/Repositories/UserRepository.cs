using System.Linq.Expressions;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Repositories;

public class UserRepository: Repository<User, long, AppDbContext>
{
    protected override Expression<Func<User, long>> Key => model => model.Id;

    public UserRepository(AppDbContext ctx, AppDbContext dbContext) : base(dbContext,
        (appDbContext) => appDbContext.Users)
    {

    }
}