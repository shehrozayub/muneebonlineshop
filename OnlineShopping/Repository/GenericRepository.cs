using OnlineShopping.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace OnlineShopping.Repository
{
    // This is used to Isolate the EntityFramework based Data Access Layer from the MVC Controller class
   
    public class GenericRepository<Tbl_Entity> : IRepository<Tbl_Entity> where Tbl_Entity : class
    {
        DbSet<Tbl_Entity> _dbSet;
        private Online_ShoppingEntities _DBEntity;

        public GenericRepository(Online_ShoppingEntities DBEntity)
        {
            _DBEntity = DBEntity;
            _dbSet = _DBEntity.Set<Tbl_Entity>();

        }

        public IEnumerable<Tbl_Entity> GetAllRecords()
        {
            return _dbSet.ToList();
        }

        public IQueryable<Tbl_Entity> GetAllRecordsIQueryable()
        {
            return _dbSet;
        }

        public IEnumerable<Tbl_Entity> GetRecordsToShow(int pageNo, int pageSize, int currentPageNo, Expression<Func<Tbl_Entity, bool>> wherePredict, Expression<Func<Tbl_Entity, int>> orderByPredict)
        {        
            if (wherePredict != null)
                return _dbSet.OrderBy(orderByPredict).Where(wherePredict).ToList();
            else
                return _dbSet.OrderBy(orderByPredict).ToList();
        }

        public int GetAllRecordsCount()
        {
            return _dbSet.Count();
        }

        public void Add(Tbl_Entity entity)
        {
            _dbSet.Add(entity);
            _DBEntity.SaveChanges();
        }

        /// <summary>
        /// Updates table entity passed to it
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Tbl_Entity entity)
        {
            _dbSet.Attach(entity);
            _DBEntity.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateByWhereClause(Expression<Func<Tbl_Entity, bool>> wherePredict, Action<Tbl_Entity> forEachPredict)
        {
            _dbSet.Where(wherePredict).ToList().ForEach(forEachPredict);
        }

        public Tbl_Entity GetFirstOrDefault(int recordId)
        {
            return _dbSet.Find(recordId);
        }

        public Tbl_Entity GetFirstOrDefaultByParameter(Expression<Func<Tbl_Entity, bool>> wherePredict)
        {
            return _dbSet.Where(wherePredict).FirstOrDefault();
        }

        public IEnumerable<Tbl_Entity> GetListByParameter(Expression<Func<Tbl_Entity, bool>> wherePredict)
        {
            return _dbSet.Where(wherePredict).ToList();
        }

        public void Remove(Tbl_Entity entity)
        {
            if (_DBEntity.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);
            _dbSet.Remove(entity);
        }

        public void RemoveByWhereClause(Expression<Func<Tbl_Entity, bool>> wherePredict)
        {
            Tbl_Entity entity = _dbSet.Where(wherePredict).FirstOrDefault();
            Remove(entity);
        }

        public void RemoveRangeByWhereClause(Expression<Func<Tbl_Entity, bool>> wherePredict)
        {
            List<Tbl_Entity> entity = _dbSet.Where(wherePredict).ToList();
            foreach (var ent in entity)
            {
                Remove(ent);
            }
        }
        public void DeleteMarkByWhereClause(Expression<Func<Tbl_Entity, bool>> wherePredict, Action<Tbl_Entity> ForEachPredict)
        {
            _dbSet.Where(wherePredict).ToList().ForEach(ForEachPredict);
            _DBEntity.SaveChanges();
        }

        public void InactiveAndDeleteMarkByWhereClause(Expression<Func<Tbl_Entity, bool>> wherePredict, Action<Tbl_Entity> ForEachPredict)
        {
            _dbSet.Where(wherePredict).ToList().ForEach(ForEachPredict);
            _DBEntity.SaveChanges();
        }

        /// <summary>
        /// Returns result by where clause in descending order
        /// </summary>
        /// <param name="orderByPredict"></param>
        /// <returns></returns>
        public IQueryable<Tbl_Entity> OrderByDescending(Expression<Func<Tbl_Entity, int>> orderByPredict)
        {
            if (orderByPredict == null)
            {
                return _dbSet;
            }
            return _dbSet.OrderByDescending(orderByPredict);
        }

        /// <summary>
        /// Executes procedure in database and returns result
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<Tbl_Entity> GetResultBySqlProcedure(string query, params object[] parameters)
        {
            if (parameters != null)
                return _DBEntity.Database.SqlQuery<Tbl_Entity>(query, parameters).ToList();
            else
                return _DBEntity.Database.SqlQuery<Tbl_Entity>(query).ToList();
        }
    }
}