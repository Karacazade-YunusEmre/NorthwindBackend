using System.Linq.Expressions;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.DataAccess.EntityFramework;

public class EfEntityRepository<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext: DbContext, new() 
{
        public List<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null)
        {
                using var context = new TContext();
                return filter == null ? context.Set<TEntity>().ToList() : context.Set<TEntity>().Where(filter).ToList();
        }

        public TEntity? Get(Expression<Func<TEntity, bool>> filter)
        {
                using var context = new TContext();
                return context.Set<TEntity>().FirstOrDefault(filter);
        }

        public void Add(TEntity entity)
        {
                using var context = new TContext();
                var entry = context.Entry(entity);
                entry.State = EntityState.Added;
                context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
                using var context = new TContext();
                var entry = context.Entry(entity);
                entry.State = EntityState.Modified;
                context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
                using var context = new TContext();
                var entry = context.Entry(entity);
                entry.State = EntityState.Deleted;
                context.SaveChanges();
        }
}