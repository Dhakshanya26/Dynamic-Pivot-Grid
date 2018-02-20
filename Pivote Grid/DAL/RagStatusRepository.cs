//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Web;

//namespace Sample_Code.DAL
//{
//    public interface IRagStatusRepository
//    {
//        PagedResult<RagStatus> GetPivotePagedList(Func<IQueryable<RagStatus>, IOrderedQueryable<RagStatus>> orderBy,
//            Func<IQueryable<RagStatus>, IGrouping<object, RagStatus>[]> groupBy,
//            int pageIndex, int pageSize, Expression<Func<RagStatus, bool>> predicate = null);
//    }

//    public class RagStatusRepository : IRagStatusRepository
//    {
//        private readonly IGenericRepository<RagStatus> _baseRagStatusRepository;
//        public RagStatusRepository()
//        {
//            _baseRagStatusRepository = new GenericRepository<RagStatus>();
//        }
//        public PagedResult<RagStatus> GetPivotePagedList(Func<IQueryable<RagStatus>, IOrderedQueryable<RagStatus>> orderBy,
//            Func<IQueryable<RagStatus>, IGrouping<object, RagStatus>[]> groupBy,
//            int pageIndex, int pageSize, Expression<Func<RagStatus, bool>> predicate = null)
//        {
//            return _baseRagStatusRepository.GetPivotePagedList(orderBy, groupBy,pageIndex, pageSize, predicate);
//        }

//    }
//}