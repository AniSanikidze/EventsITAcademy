using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Infrastructure.BaseRepository
{
    public interface IBaseRepository<T>
    {
        #region Methods

        //Task<List<T>> GetAllAsync(CancellationToken token);

        Task<T> GetAsync(CancellationToken token, Expression<Func<T, bool>> predicate);

        Task<T> AddAsync(CancellationToken token, T entity);

        Task DeleteAsync(CancellationToken token, int id);

        Task<T> UpdateAsync(CancellationToken token, T entity);

        Task<bool> AnyAsync(CancellationToken token, Expression<Func<T, bool>> predicate);

        //void Attach(T entity);
        //void Detach(T entity);

        //void SetModifiedStateWithNested(T entity);

        #endregion
    }
}
