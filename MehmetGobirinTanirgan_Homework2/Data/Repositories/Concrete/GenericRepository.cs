using Data.Context;
using Data.DataModels.Base;
using Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repositories.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        private readonly SwcsDbContext context;

        public GenericRepository(SwcsDbContext context)
        {
            this.context = context;
        }

        // Temel database işlemleri
        public virtual async Task AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
        }
        public virtual void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }

        public virtual void Delete(long id)
        {
            context.Set<T>().Remove(new T { Id = id });
        }

        public virtual async Task DeleteRangeByExpressionAsync(Expression<Func<T, bool>> exp)
        {
            context.Set<T>().RemoveRange(await GetListByExpression(exp).ToListAsync());
        }

        public virtual async Task<T> GetByExpression(Expression<Func<T, bool>> exp)
        {
            return await context.Set<T>().Where(exp).FirstOrDefaultAsync();
        }

        public virtual async Task<T> GetByIdAsync(long id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public virtual IQueryable<T> GetListByExpression(Expression<Func<T, bool>> exp)
        {
            return context.Set<T>().Where(exp);
        } 
    }
}
