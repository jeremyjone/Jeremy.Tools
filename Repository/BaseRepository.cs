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
    public class BaseRepository<TEntity, TContext, TRepository> : Repository<TContext, TRepository>, IBaseRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
        where TRepository : IBaseRepository<TEntity>
    {
        protected BaseRepository(TContext db, ILogger<TRepository> logger) : base(db, logger)
        {
        }

        #region 查

        public virtual TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            return Db.Set<TEntity>().FirstOrDefault(expression);
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Db.Set<TEntity>().FirstOrDefaultAsync(expression);
        }

        public virtual List<TEntity> GetRange(Expression<Func<TEntity, bool>> expression = null)
        {
            return Db.Set<TEntity>().WhereIf(expression != null, expression).ToList();
        }

        public virtual async Task<List<TEntity>> GetRangeAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            return await Db.Set<TEntity>().WhereIf(expression != null, expression).ToListAsync();
        }

        #endregion


        #region 增

        public virtual bool Add(TEntity entity)
        {
            if (entity != null) Db.Set<TEntity>().Add(entity);
            return Db.SaveChanges() > 0;
        }

        public virtual async Task<bool> AddAsync(TEntity entity)
        {
            if (entity != null) await Db.Set<TEntity>().AddAsync(entity);
            return await Db.SaveChangesAsync() > 0;
        }

        public virtual bool AddRange(IEnumerable<TEntity> entities)
        {
            entities = entities.ToList();
            if (entities.Any())
            {
                Db.Set<TEntity>().AddRange(entities);
            }

            return Db.SaveChanges() > 0;
        }

        public virtual async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            entities = entities.ToList();
            if (entities.Any())
            {
                await Db.Set<TEntity>().AddRangeAsync(entities);
            }

            return await Db.SaveChangesAsync() > 0;
        }

        #endregion



        #region 改

        public virtual bool Update(TEntity entity)
        {
            if (entity != null) Db.Set<TEntity>().Update(entity);

            return Db.SaveChanges() > 0;
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            if (entity != null) Db.Set<TEntity>().Update(entity);

            return await Db.SaveChangesAsync() > 0;
        }

        public virtual bool UpdateRange(IEnumerable<TEntity> entities)
        {
            entities = entities.ToList();
            if (entities.Any())
            {
                Db.Set<TEntity>().UpdateRange(entities);
            }

            return Db.SaveChanges() > 0;
        }

        public virtual async Task<bool> UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            entities = entities.ToList();
            if (entities.Any())
            {
                Db.Set<TEntity>().UpdateRange(entities);
            }

            return await Db.SaveChangesAsync() > 0;
        }

        #endregion




        #region 删

        public virtual bool Delete(TEntity entity)
        {
            if (entity != null) Db.Set<TEntity>().Remove(entity);

            return Db.SaveChanges() > 0;
        }

        public virtual bool Delete(Expression<Func<TEntity, bool>> expression)
        {
            var entity = Get(expression);
            if (entity != null) Db.Set<TEntity>().Remove(entity);

            return Db.SaveChanges() > 0;
        }

        public virtual bool DeleteRange(IEnumerable<TEntity> entities)
        {
            entities = entities.ToList();
            if (entities.Any())
            {
                Db.Set<TEntity>().RemoveRange(entities);
            }

            return Db.SaveChanges() > 0;
        }

        public virtual bool DeleteRange(Expression<Func<TEntity, bool>> expression)
        {
            var entities = GetRange(expression);
            if (entities.Any())
            {
                Db.Set<TEntity>().RemoveRange(entities);
            }

            return Db.SaveChanges() > 0;
        }

        public virtual async Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> expression)
        {
            var entities = await GetRangeAsync(expression);
            if (entities.Any())
            {
                Db.Set<TEntity>().RemoveRange(entities);
            }

            return await Db.SaveChangesAsync() > 0;
        }

        #endregion



        public virtual bool Save()
        {
            try
            {
                return Db.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                return false;
            }
        }

        public virtual async Task<bool> SaveAsync()
        {
            try
            {
                return await Db.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                return false;
            }
        }
    }
}