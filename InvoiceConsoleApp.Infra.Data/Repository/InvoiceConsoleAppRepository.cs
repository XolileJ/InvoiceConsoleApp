using InvoiceConsoleApp.Infra.Data.Context;
using InvoiceConsoleApp.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InvoiceConsoleApp.Infra.Data.Repository
{
    public class InvoiceConsoleAppRepository<TEntity> : IInvoiceConsoleAppRepository<TEntity> where TEntity : class
    {
        protected InvoiceConsoleAppDbContext Db;
        protected DbSet<TEntity> DbSet;

        /// <summary>
        /// Inject Billing databese context
        /// </summary>
        /// <param name="context"></param>
        public InvoiceConsoleAppRepository(InvoiceConsoleAppDbContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
        }

        /// <summary>
        /// Create new Entity
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Add(TEntity obj)
        {
            DbSet.Add(obj);
        }

        /// <summary>
        /// Create new Entities
        /// </summary>
        /// <param name="entities"></param>
        public virtual void AddRange(List<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        /// <summary>
        /// Get specific record
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).AsNoTracking().FirstOrDefault();
        }

        /// <summary>
        /// Select entity by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity GetById(long id)
        {
            return DbSet.Find(id);
        }

        /// <summary>
        /// Select all records from database
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            return DbSet.ToList();
        }

        /// <summary>
        /// Select all records from database
        /// </summary>
        /// <returns></returns>
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).ToList();
        }

        /// <summary>
        /// Update existing entity
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Update(TEntity obj)
        {
            DbSet.Update(obj);
        }

        public virtual void attach(TEntity obj)
        {
            DbSet.Update(obj);
        }

        /// <summary>
        /// Delete entity by GUID
        /// </summary>
        /// <param name="id"></param>
        public virtual void Remove(Guid id)
        {
            DbSet.Remove(DbSet.Find(id));
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="id"></param>
        public virtual void RemoveRange(List<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Select from database
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsNoTracking().Where(predicate);
        }

        /// <summary>
        /// Select from database
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        /// <summary>
        /// Commit database changes
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return Db.SaveChanges();
        }       

        public void Remove(long id)
        {
            DbSet.Remove(DbSet.Find(id));
        }

        public void Remove(TEntity obj)
        {
            var entry = Db.Entry(obj);
            entry.State = EntityState.Deleted;
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
