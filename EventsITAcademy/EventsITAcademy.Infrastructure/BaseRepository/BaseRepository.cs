//using EventsITAcademy.Domain;
//using EventsITAcademy.Persistence.Context;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace EventsITAcademy.Infrastructure.BaseRepository
//{
//    public class BaseRepository<T> : IBaseRepository<T> where T : IBaseEntity
//    {
//        #region DbContext And DbSets
//        protected readonly ApplicationContext _applicationContext;
//        //protected readonly DbSet<T> _dbSet;
//        #endregion     
//        #region Ctor
//        public BaseRepository(ApplicationContext applicationContext)
//        {
//            _applicationContext = applicationContext;
//            //_dbSet = _applicationContext.Set<T>();
//        }
//        #endregion

//        #region Methods

//        public async Task<T> GetAsync(CancellationToken token, Expression<Func<T, bool>> predicate)
//        {
//            return await _dbSet.SingleOrDefaultAsync(predicate, token);
//        }
//        public async Task<T> AddAsync(CancellationToken token, T entity)
//        {
//            await _dbSet.AddAsync(entity, token);
//            await _applicationContext.SaveChangesAsync(token);
//            return entity;
//        }
//        public async Task<T> UpdateAsync(CancellationToken token, T entity)
//        {
//            _dbSet.Update(entity);
//            await _applicationContext.SaveChangesAsync(token);
//            //await _contextWrapper.SaveChanges(token, entity);
//            return await GetAsync(token, x => x.Id == entity.Id);
//        }
//        public async Task DeleteAsync(CancellationToken token, int id)
//        {
//            var entity = await GetAsync(token, x => x.Id == id);
//            entity.Status = EntityStatuses.Deleted;
//            _applicationContext.Entry(entity).State = EntityState.Modified;
//            await _applicationContext.SaveChangesAsync(token);
//        }
//        public Task<bool> AnyAsync(CancellationToken token, Expression<Func<T, bool>> predicate)
//        {
//            return _dbSet.AnyAsync(predicate, token);
//        }

//        public Task<IBaseEntity> GetAsync(CancellationToken token, Expression<Func<IBaseEntity, bool>> predicate)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<IBaseEntity> AddAsync(CancellationToken token, IBaseEntity entity)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<IBaseEntity> UpdateAsync(CancellationToken token, IBaseEntity entity)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<bool> AnyAsync(CancellationToken token, Expression<Func<IBaseEntity, bool>> predicate)
//        {
//            throw new NotImplementedException();
//        }

//        #endregion


//    }
//}
