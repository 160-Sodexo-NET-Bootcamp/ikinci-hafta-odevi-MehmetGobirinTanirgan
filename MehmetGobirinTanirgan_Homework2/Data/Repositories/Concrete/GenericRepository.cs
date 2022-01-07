using Data.Context;
using Data.DataModels.Base;
using Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
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

        // Temel database işlemleri, burada context üzerinden temelde tüm entity'ler
        // için kullanabileceğim aksiyonları yazmış oldum.

        // Entity ekleme
        public virtual async Task AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
        }

        // Entity güncelleme
        public virtual void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }

        // Id üzerinden entity silme
        public virtual void Delete(long id)
        {
            context.Set<T>().Remove(new T { Id = id });
        }

        // Gelen expression'a göre toplu entity silme
        public virtual async Task DeleteRangeByExpressionAsync(Expression<Func<T, bool>> exp)
        {
            context.Set<T>().RemoveRange(await GetListByExpression(exp).ToListAsync());
        }

        // Gelen expression'a göre entity getirme
        public virtual async Task<T> GetByExpression(Expression<Func<T, bool>> exp)
        {
            return await context.Set<T>().FirstOrDefaultAsync(exp);
        }

        // Id üzerinden entity getirme
        public virtual async Task<T> GetByIdAsync(long id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        // Tüm tabloyu çekme
        public virtual IQueryable<T> GetAll()
        {
            return context.Set<T>();
        }

        // Gelen expression'a göre filtrelenmiş liste çekme
        public virtual IQueryable<T> GetListByExpression(Expression<Func<T, bool>> exp)
        {
            return context.Set<T>().Where(exp);
        } 
    }
}
