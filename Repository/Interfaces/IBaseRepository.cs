using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Jeremy.Tools.Repository.Extensions;
using Jeremy.Tools.Repository.Models;

namespace Jeremy.Tools.Repository.Interfaces
{
    /// <summary>
    /// 具有最基本的公共方法仓库基础接口，仅仅提供最基本的增删改查功能
    /// </summary>
    public interface IBaseRepository<TEntity> : IRepository where TEntity : class
    {
        #region 查

        /// <summary>
        /// 根据条件获取内容
        /// </summary>
        /// <param name="expression">筛选条件</param>
        /// <returns></returns>
        TEntity Get(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 根据条件异步获取内容
        /// </summary>
        /// <param name="expression">筛选条件</param>
        /// <returns></returns>
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 获取全部内容<br />
        /// ** <b>慎用</b> ** 数量过多时，可能会卡。【如果可能，请重载该方法】
        /// </summary>
        /// <param name="expression">筛选条件，为 null 返回全部内容</param>
        /// <returns></returns>
        List<TEntity> GetRange(Expression<Func<TEntity, bool>> expression = null);

        /// <summary>
        /// 异步获取全部内容
        /// ** <b>慎用</b> ** 数量过多时，可能会卡。【如果可能，请重载该方法】
        /// </summary>
        /// <param name="expression">筛选条件，为 null 返回全部内容</param>
        /// <returns></returns>
        Task<List<TEntity>> GetRangeAsync(Expression<Func<TEntity, bool>> expression = null);

        #endregion



        #region 增

        /// <summary>
        /// 添加一个内容
        /// </summary>
        /// <param name="entity">新增的实体</param>
        /// <returns></returns>
        bool Add(TEntity entity);

        /// <summary>
        /// 异步添加一个内容
        /// </summary>
        /// <param name="entity">新增的实体</param>
        /// <returns></returns>
        Task<bool> AddAsync(TEntity entity);

        /// <summary>
        /// 添加多个内容
        /// </summary>
        /// <param name="entities">新增的实体</param>
        /// <returns></returns>
        bool AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// 异步添加多个内容
        /// </summary>
        /// <param name="entities">新增的实体</param>
        /// <returns></returns>
        Task<bool> AddRangeAsync(IEnumerable<TEntity> entities);

        #endregion



        #region 改

        /// <summary>
        /// 更新一个内容
        /// </summary>
        /// <param name="entity">更新的实体</param>
        /// <returns></returns>
        bool Update(TEntity entity);

        /// <summary>
        /// 更新一个内容
        /// </summary>
        /// <param name="entity">更新的实体</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity);

        /// <summary>
        /// 更新多个内容
        /// </summary>
        /// <param name="entities">更新的实体</param>
        /// <returns></returns>
        bool UpdateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// 更新多个内容
        /// </summary>
        /// <param name="entities">更新的实体</param>
        /// <returns></returns>
        Task<bool> UpdateRangeAsync(IEnumerable<TEntity> entities);

        #endregion



        #region 删

        /// <summary>
        /// 删除指定内容
        /// </summary>
        /// <param name="entity">删除的实体</param>
        /// <returns></returns>
        bool Delete(TEntity entity);

        /// <summary>
        /// 删除指定内容
        /// </summary>
        /// <param name="expression">删除的条件。单一删除，若条件匹配多个，则删除第一个</param>
        /// <returns></returns>
        bool Delete(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 删除多个内容
        /// </summary>
        /// <param name="entities">删除的实体</param>
        bool DeleteRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// 删除多个内容
        /// </summary>
        /// <param name="expression">删除的条件。删除所有匹配到的内容</param>
        bool DeleteRange(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 异步删除多个内容
        /// </summary>
        /// <param name="expression">删除的条件。删除所有匹配到的内容</param>
        Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> expression);

        #endregion


        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        bool Save();

        /// <summary>
        /// 异步保存
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAsync();
    }
}
