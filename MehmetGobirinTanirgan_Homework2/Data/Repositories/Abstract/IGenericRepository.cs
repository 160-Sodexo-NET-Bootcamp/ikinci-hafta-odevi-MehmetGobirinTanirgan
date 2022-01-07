using Data.DataModels.Base;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repositories.Abstract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // Temel database işlemleri
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(long id);
        Task DeleteRangeByExpressionAsync(Expression<Func<T, bool>> exp);
        Task<T> GetByIdAsync(long id);
        Task<T> GetByExpression(Expression<Func<T, bool>> exp);
        IQueryable<T> GetAll();
        IQueryable<T> GetListByExpression(Expression<Func<T, bool>> exp);
    }
}
