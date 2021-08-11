using Microsoft.Extensions.Logging;
using Jeremy.Tools.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Jeremy.Tools.Repository
{
    public class Repository<TContext, TRepository> : IRepository
        where TContext : DbContext
        where TRepository : IRepository
    {
        public TContext Db { get; }
        protected ILogger<TRepository> Logger { get; }

        public Repository(TContext db, ILogger<TRepository> logger)
        {
            Db = db;
            Logger = logger;
        }
    }
}
