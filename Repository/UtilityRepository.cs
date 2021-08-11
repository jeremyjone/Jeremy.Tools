using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Jeremy.Tools.Extensions;
using Jeremy.Tools.Repository.Extensions;
using Jeremy.Tools.Repository.Interfaces;
using Jeremy.Tools.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Jeremy.Tools.Repository
{
    public abstract class UtilityRepository<TEntity, TContext, TRepository> : BaseRepository<TEntity, TContext, TRepository>, IUtilityRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
        where TRepository : IUtilityRepository<TEntity>
    {
        protected UtilityRepository(TContext db, ILogger<TRepository> logger) : base(db, logger)
        {
        }

        #region 查

        public virtual PageList<TEntity> GetRange<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, int page = 1, int pageSize = 10,
            bool isDescending = false)
        {
            var res = Db.Set<TEntity>().Where(expression).PageBy(orderBy, page, pageSize, isDescending).ToList();
            return res.ToPageList(res.Count, pageSize);
        }

        public virtual async Task<PageList<TEntity>> GetRangeAsync<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, int page = 1, int pageSize = 10, bool isDescending = false)
        {
            return Db.Set<TEntity>()
                .Where(expression)
                .PageBy(orderBy, page, pageSize, isDescending)
                .ToPageList(await Db.Set<TEntity>().Where(expression).CountAsync(), pageSize);
        }

        public async Task<PageList<TEntity>> GetRangeAsync<TParam, TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, TParam param = null, int page = 1, int pageSize = 10,
            bool isDescending = false) where TParam : class
        {
            var res = await Db.Set<TEntity>().Where(expression).ToListAsync();
            if (param != null)
                res = param.ToDictionary()
                    .Aggregate(res, (current, o) => current
                        .Where(x => Equals(x.GetType().GetProperty(o.Key)?.GetValue(x), o.Value)).AsQueryable()
                        .PageBy(orderBy, page, pageSize, isDescending)
                        .ToList());

            return res.ToPageList(res.Count, pageSize);
        }

        public abstract Task<PageList<TEntity>> GetRangeAsync(Expression<Func<TEntity, bool>> expression, int page,
            int pageSize = 10);

        public virtual async Task<TAccessory> GetAccessoryAsync<TAccessory>(Expression<Func<TAccessory, bool>> expression, Expression<Func<TAccessory, TEntity>> includeExpression = null) where TAccessory : class, new()
        {
            return await Db.Set<TAccessory>().IncludeIf(includeExpression).FirstOrDefaultAsync(expression);
        }

        public virtual async Task<PageList<TAccessory>> GetAccessoryRangeAsync<TAccessory, TKey>(Expression<Func<TAccessory, bool>> expression, Expression<Func<TAccessory, TKey>> orderBy, int page, int pageSize) where TAccessory : class, new()
        {
            return (await Db.Set<TAccessory>().Where(expression).PageBy(orderBy, page, pageSize).ToListAsync())
                .ToPageList(await Db.Set<TAccessory>().Where(expression).CountAsync(), pageSize);
        }

        #endregion


        #region 增

        public async Task<bool> AddAccessoryAsync<TAccessory>(TAccessory entity) where TAccessory : class, new()
        {
            if (entity != null) await Db.Set<TAccessory>().AddAsync(entity);
            return await Db.SaveChangesAsync() > 0;
        }

        #endregion



        #region 改


        #endregion




        #region 删

        public async Task<bool> DeleteAccessoryAsync<TAccessory>(TAccessory entity) where TAccessory : class, new()
        {
            if (entity != null) Db.Set<TAccessory>().Remove(entity);

            return await Db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAccessoryAsync<TAccessory>(Expression<Func<TAccessory, bool>> expression) where TAccessory : class, new()
        {
            var entity = await GetAccessoryAsync(expression);
            if (entity != null) Db.Set<TAccessory>().Remove(entity);
            return await Db.SaveChangesAsync() > 0;
        }

        #endregion
    }
}