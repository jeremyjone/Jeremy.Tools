using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Jeremy.Tools.Repository.Extensions;
using Jeremy.Tools.Repository.Models;

namespace Jeremy.Tools.Repository.Interfaces
{
    /// <summary>
    /// 具有比较完备的公共方法仓库基础接口，提供增删改查功能，查询提供分页结果。
    /// </summary>
    public interface IUtilityRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        #region 查

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
        PagedList<TEntity> GetRange<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, int page = 1, int pageSize = 10, bool isDescending = false);

        /// <summary>
        /// 获取指定条件下当前页的内容
        /// </summary>
        /// <typeparam name="TKey">排序键类型</typeparam>
        /// <param name="expression">筛选条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页数据的数量，默认10</param>
        /// <param name="isDescending">是否倒序。默认正序</param>
        Task<PagedList<TEntity>> GetRangeAsync<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, int page = 1, int pageSize = 10, bool isDescending = false);

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
        Task<PagedList<TEntity>> GetRangeAsync<TParam, TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, TParam param = null, int page = 1, int pageSize = 10, bool isDescending = false) where TParam : class;

        /// <summary>
        /// 【该方法是自定义方法，需要自行实现】自定义获取指定条件下当前页的内容<br />
        /// 用户可以通过调用 Db.Set&lt;TEntity&gt; 的方法来获取需要的数据。Db 对象是 Repository 中设定好的。<br />
        /// </summary>
        /// <remarks>
        /// 分页结果可以在查询结果最后调用 <see cref="EnumerableExtension.ToPagedList{TEntity}">ToPagedList(int totalCount, int pageSize)</see> 扩展方法将其转换。<br />
        /// 如：Db.Set&lt;TEntity&gt;.Where(expression).<see cref="QueryableExtension.PagedBy{T,TKey}">PagedBy</see>(x => x.Id, page, pageSize).<see cref="EnumerableExtension.ToPagedList{TEntity}">ToPagedList</see>(await Db.Set&lt;TEntity&gt;.Where(expression).CountAsync(), pageSize);
        /// </remarks>
        /// <param name="expression">筛选条件</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页数据的数量，默认10</param>
        /// <returns></returns>
        Task<PagedList<TEntity>> GetRangeAsync(Expression<Func<TEntity, bool>> expression, int page, int pageSize = 10);

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
        Task<PagedList<TAccessory>> GetAccessoryRangeAsync<TAccessory, TKey>(Expression<Func<TAccessory, bool>> expression, Expression<Func<TAccessory, TKey>> orderBy, int page, int pageSize) where TAccessory : class, new();

        #endregion



        #region 增

        /// <summary>
        /// 异步添加一个附属实体内容
        /// </summary>
        /// <typeparam name="TAccessory">新增的实体类型</typeparam>
        /// <param name="entity">新增的实体</param>
        /// <returns></returns>
        Task<bool> AddAccessoryAsync<TAccessory>(TAccessory entity) where TAccessory : class, new();

        #endregion



        #region 改


        #endregion



        #region 删

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
    }
}
