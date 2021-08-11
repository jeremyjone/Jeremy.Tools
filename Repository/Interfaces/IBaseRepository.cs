using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Jeremy.Tools.Repository.Models;

namespace Jeremy.Tools.Repository.Interfaces
{
    /// <summary>
    /// 仓库基础接口，提供基本的增删改查功能
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
        /// ** <b>慎用</b> ** 数量过多时，可能会卡。
        /// </summary>
        /// <param name="expression">筛选条件，为 null 返回全部内容</param>
        /// <returns></returns>
        List<TEntity> GetRange(Expression<Func<TEntity, bool>> expression = null);

        /// <summary>
        /// 异步获取全部内容
        /// ** <b>慎用</b> ** 数量过多时，可能会卡。
        /// </summary>
        /// <param name="expression">筛选条件，为 null 返回全部内容</param>
        /// <returns></returns>
        Task<List<TEntity>> GetRangeAsync(Expression<Func<TEntity, bool>> expression = null);

        /// <summary>
        /// 获取指定条件下当前页的内容
        /// </summary>
        /// <typeparam name="TKey">排序键类型</typeparam>
        /// <param name="expression">筛选条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页数据的数量，默认10</param>
        /// <param name="isDescending">是否倒序。默认正序</param>
        /// <returns></returns>
        PageList<TEntity> GetRange<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, int page = 1, int pageSize = 10, bool isDescending = false);

        /// <summary>
        /// 获取指定条件下当前页的内容
        /// </summary>
        /// <typeparam name="TKey">排序键类型</typeparam>
        /// <param name="expression">筛选条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页数据的数量，默认10</param>
        /// <param name="isDescending">是否倒序。默认正序</param>
        Task<PageList<TEntity>> GetRangeAsync<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, int page = 1, int pageSize = 10, bool isDescending = false);

        /// <summary>
        /// 获取指定条件下当前页的内容
        /// </summary>
        /// <typeparam name="TParam"></typeparam>
        /// <typeparam name="TKey">排序键类型</typeparam>
        /// <param name="expression">筛选条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="param">参数对象。筛选与该对象条件吻合的数据。<br />例如：数据为：{"a": 1, "b": 2, "c": 3}，参数对象传入：{"a": 1}，则可以匹配。</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页数据的数量，默认10</param>
        /// <param name="isDescending">是否倒序。默认正序</param>
        /// <returns></returns>
        Task<PageList<TEntity>> GetRangeAsync<TParam, TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, TParam param = null, int page = 1, int pageSize = 10, bool isDescending = false) where TParam : class;

        /// <summary>
        /// 获取指定条件下当前页的内容。
        /// </summary>
        /// <param name="expression">筛选条件</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页数据的数量，默认10</param>
        /// <returns></returns>
        Task<PageList<TEntity>> GetRangeAsync(Expression<Func<TEntity, bool>> expression, int page, int pageSize = 10);

        /// <summary>
        /// 通过当前仓库查询其他指定附属类型的数据。当附属条件为 null 时，查询数据与普通无异。<br />
        /// 该方法适用于查询一层嵌套数据。
        /// </summary>
        /// <typeparam name="TAccessory">附属条件数据对象的类型</typeparam>
        /// <param name="expression">筛选条件</param>
        /// <param name="includeExpression">附属条件</param>
        /// <returns></returns>
        Task<TAccessory> GetAccessoryAsync<TAccessory>(Expression<Func<TAccessory, bool>> expression, Expression<Func<TAccessory, TEntity>> includeExpression = null) where TAccessory : class, new();

        /// <summary>
        /// 通过当前仓库查询其他指定附属类型的数据。
        /// </summary>
        /// <typeparam name="TAccessory">附属条件数据对象的类型</typeparam>
        /// <typeparam name="TKey">排序键类型</typeparam>
        /// <param name="expression">筛选条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页数据的数量，默认10</param>
        /// <returns></returns>
        Task<PageList<TAccessory>> GetAccessoryRangeAsync<TAccessory, TKey>(Expression<Func<TAccessory, bool>> expression, Expression<Func<TAccessory, TKey>> orderBy, int page, int pageSize) where TAccessory : class, new();

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

        /// <summary>
        /// 异步添加一个附属实体内容
        /// </summary>
        /// <typeparam name="TAccessory">新增的实体类型</typeparam>
        /// <param name="entity">新增的实体</param>
        /// <returns></returns>
        Task<bool> AddAccessoryAsync<TAccessory>(TAccessory entity) where TAccessory : class, new();

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

        /// <summary>
        /// 删除一个附属实体内容
        /// </summary>
        /// <typeparam name="TAccessory">附属实体的类型</typeparam>
        /// <param name="entity">删除的实体</param>
        /// <returns></returns>
        Task<bool> DeleteAccessoryAsync<TAccessory>(TAccessory entity) where TAccessory : class, new();

        /// <summary>
        /// 删除一个附属实体内容
        /// </summary>
        /// <typeparam name="TAccessory">附属实体的类型</typeparam>
        /// <param name="expression">删除的条件。单一删除，若条件匹配多个，则删除第一个</param>
        /// <returns></returns>
        Task<bool> DeleteAccessoryAsync<TAccessory>(Expression<Func<TAccessory, bool>> expression) where TAccessory : class, new();

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
