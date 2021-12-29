﻿using Data.DataModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repositories.Abstract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(long id);
        Task DeleteRangeByExpressionAsync(Expression<Func<T, bool>> exp);
        Task<T> GetByIdAsync(long id);
        Task<T> GetByExpression(Expression<Func<T, bool>> exp);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> GetListByExpression(Expression<Func<T, bool>> exp);
    }
}