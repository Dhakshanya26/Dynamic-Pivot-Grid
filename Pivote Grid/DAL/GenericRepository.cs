//using System;
//using System.Collections.Generic;
//using System.Data.Common;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Web;

//namespace Sample_Code.DAL
//{
//    public class GenericRepository<T> : IGenericRepository<T> where T : class
//    {
//        public DatabaseContext DatabaseContext { get; set; }

//        public GenericRepository() : this(new DatabaseContext())
//        {
//        }

//        public GenericRepository(DatabaseContext dataBaseContext)
//        {
//            DatabaseContext = dataBaseContext;
//        }

//        public PagedResult   GetPivotePagedList(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
//            Func<IQueryable<T>, IGrouping<object,T>[]> groupBy,
//            int pageIndex, int pageSize,  Expression<Func<T, bool>> predicate = null)
//        {
//            var result = new PagedResult<T> ();
//            IQueryable<T> query = DatabaseContext.Set<T>();
//            if (predicate != null)
//            {
//                query = query.Where(predicate);
//            }

//            if (orderBy != null)
//                query = orderBy(query);

//            if (groupBy != null)
//            {
//               var groupedResult= groupBy(query);
//                 result.RowCount = groupedResult.Count();
//                result.Results  = groupBy(query).Skip(((int)pageIndex - 1) * (int)pageSize).Take((int)pageSize).ToList();
//            }
//            else
//            {
//                //(0-1)
//                result.Results = query.Skip(((int)pageIndex - 1) * (int)pageSize).Take((int)pageSize);
//            }
           
//            //var query = predicate != null ? DatabaseContext.Set<T>().Where(predicate) : DatabaseContext.Set<T>();
//            //totalCount = query.Count();
//            //return new PagedResult<T>
//            //{
//            //    Results = groupBy(query).orderBy(query).Skip((pageIndex) * pageSize).Take(pageSize).ToList(),
//            //    RowCount = query.Count(),
//            //};
//            //return groupBy(query).orderBy(query).Skip(pageIndex * pageSize).Take(pageSize);

//             return ;
//        }
       
//        public void Dispose()
//        {
//           //
//        }
//    }

//    public interface IGenericRepository<T> : IDisposable where T : class
//    {
//        IEnumerable<T> GetPivotePagedList(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> predicate = null);

//    }

//    public class PagedResult<T> where T : class
//    {
//        public int RowCount { get; set; }
//        public List<IGrouping<>,T> Results { get; set; }
//    }
//}