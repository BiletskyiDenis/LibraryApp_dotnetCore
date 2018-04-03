using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _libraryContext;

        public Repository(DbContext context)
        {
            this._libraryContext = context;
        }

        public void Add(TEntity entity)
        {
            _libraryContext.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _libraryContext.Set<TEntity>().AddRange(entities);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _libraryContext.Set<TEntity>().Where(predicate);
        }

        public TEntity Get(int id)
        {
            return _libraryContext.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            _libraryContext.Set<TEntity>().Load<TEntity>();
            return _libraryContext.Set<TEntity>().ToList();
        }

        public void Remove(TEntity entity)
        {
            _libraryContext.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _libraryContext.Set<TEntity>().RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            _libraryContext.Entry<TEntity>(entity).State = EntityState.Modified;
        }
    }
}
