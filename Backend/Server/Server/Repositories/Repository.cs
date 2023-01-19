using System.Linq.Expressions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Server.Repositories
{
    public abstract class Repository<TModel, TKey, TDbContext> where TModel : class, ITimeStampedModel
        where TDbContext : DbContext
    {
        private readonly DbSet<TModel> _dBSet;
        private readonly TDbContext _dbContext;

        protected abstract Expression<Func<TModel, TKey>> Key { get; }

        protected Repository(TDbContext dbContext, Func<TDbContext, DbSet<TModel>> dBSet)
        {
            _dbContext = dbContext;
            _dBSet = dBSet(dbContext);
        }

        public async Task<TModel[]> Select()
        {
            return await _dBSet.ToArrayAsync();
        }

        public async Task<TModel[]> OrderBy(Expression<Func<TModel, TKey>> expression)
        {
            return await _dBSet.OrderBy(expression).ToArrayAsync();
        }

        public async Task<TModel[]> OrderByDescending(Expression<Func<TModel, TKey>> expression)
        {
            return await _dBSet.OrderByDescending(expression).ToArrayAsync();
        }

        public async Task<TModel[]> Paginate(int offset, int limit)
        {
            return await _dBSet.OrderBy(Key).Skip(offset).Take(limit).ToArrayAsync();
        }

        public async Task<TModel[]> Select(Expression<Func<TModel, bool>> predicate)
        {
            return await _dBSet.Where(predicate).ToArrayAsync();
        }

        public async Task<TModel> First(Expression<Func<TModel, bool>> predicate)
        {
            return await _dBSet.Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<TModel> Add(TModel model)
        {
            try
            {
                await _dBSet.AddAsync(model);
                await _dbContext.SaveChangesAsync();
                return model;
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException exception)
            {
                return null;
            }
        }

        public async Task<TModel> Update(TModel entity)
        {
            _dBSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TModel[]> Where(Expression<Func<TModel, bool>> predicate)
        {
            return await _dBSet.Where(predicate).ToArrayAsync();
        }

        public async Task<long> Count()
        {
            return await _dBSet.LongCountAsync();
        }
    }
}