using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Privote_Grid.Models;

namespace Privote_Grid.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string searchTerm)
        {
           
            return View(CreateGridModel(new PagingRequestModel
            {
                SearchTerm = searchTerm,
                SortDirection = SortDirection.Desc
            }));
        }
       
        [HttpPost]
        public ActionResult Ajaxget(string searchTerm, int pageNumber = 0, int pageSize = 0,
            bool sortByColumn = false, bool sortByRow = false)
        {
            var response = CreateGridModel(
                new PagingRequestModel(searchTerm, pageNumber, pageSize, sortByColumn, sortByRow,
                    Enum.TryParse("ASC", true, out SortDirection sortDirection) && (sortDirection == SortDirection.Asc)
                        ? SortDirection.Asc
                        : SortDirection.Desc));
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        private DynamicGridModel CreateGridModel(PagingRequestModel pagingRequestModel)
        {
            var dynamicModel = new DynamicGridModel() { };
            IEnumerable<Grid> s = GetRecordsFromDb(pagingRequestModel.SearchTerm);
            var rowModels = SortBy(pagingRequestModel, ref s);
            var pagedRowKeys = SetPaging(pagingRequestModel, s);
            var columnKeys = ConstructPivot(s, pagedRowKeys, rowModels);
            dynamicModel.RowModels = rowModels;
            dynamicModel.ColumnModel = new ColumnModel()
            {
                Heading = "Description",
                Columns = columnKeys
            };
            pagingRequestModel.PagingUrl = "/Home/Ajaxget/";
            dynamicModel.PagingRequest = pagingRequestModel;
            dynamicModel.ColumnHeading = "Document";
            dynamicModel.RowHeading = "Description";
            return dynamicModel;
        }

        private static List<string> ConstructPivot(IEnumerable<Grid> s, List<string> pagedRowKeys, List<RowModel> rowModels)
        {
            var pagedGroupedData = s.Where(x => pagedRowKeys.Any(y => y.ToString() == x.Description)).ToLookup(
                k => new ValueKey(
                    k.Description, // 1st dimension
                    k.Document // 2nd dimension
                ));

            var columnKeys = pagedGroupedData.Select(g => (string) g.Key.DimKeys[1]).Distinct().OrderBy(k => k)?.ToList();
            foreach (var row in pagedRowKeys)
            {
                var rowModel = new RowModel {Heading = row};
                var dataModels = new List<DataModel>();
                foreach (var column in columnKeys)
                {
                    var data = pagedGroupedData[new ValueKey(row, column)].Max(r => r);
                    if (data != null)
                    {
                        dataModels.Add(new MyObject()
                        {
                            Id = data.Id,
                            Name = data.Name,
                            CssClass = data.Name == "1" ? "data-filled" : data.Name == "2" ? "data-partail" : "data-tobefilled"
                        });
                    }
                    else
                    {
                        dataModels.Add(new MyObject() {Id = 0, Name = string.Empty, CssClass = "no-data"});
                    }
                }
                rowModel.DataModels = dataModels;
                rowModels.Add(rowModel);
            }
            return columnKeys;
        }

        private static List<string> SetPaging(PagingRequestModel pagingRequestModel, IEnumerable<Grid> s)
        {
            var groupedData = s.ToLookup(
                k => new ValueKey(
                    k.Description, // 1st dimension
                    k.Document // 2nd dimension
                ));

            var rowKeys = groupedData.Select(g => (string) g.Key.DimKeys[0]).Distinct().OrderBy(k => k)?.ToList();

            pagingRequestModel.TotalRecord = rowKeys.Count;
            var pagedRowKeys = rowKeys
                .Skip(pagingRequestModel.PageSize * (pagingRequestModel.PageNumber - 1))
                .Take(pagingRequestModel.PageSize)?.ToList();
            return pagedRowKeys;
        }

        private static List<RowModel> SortBy(PagingRequestModel pagingRequestModel, ref IEnumerable<Grid> s)
        {
            var rowModels = new List<RowModel>();
            if (pagingRequestModel.SortColumn && !pagingRequestModel.SortRow)
            {
                s = s.OrderBy(x => x.Document)?.ToList();
            }
            else if (!pagingRequestModel.SortColumn && pagingRequestModel.SortRow)
            {
                s = s.OrderBy(x => x.Description)?.ToList();
            }
            else if (pagingRequestModel.SortColumn && pagingRequestModel.SortRow)
            {
                s = s.OrderBy(x => x.Description).ThenBy(x => x.Document)?.ToList();
            }
            return rowModels;
        }

        public class MyObject : DataModel
        {
            public int Id { get; set; }
            public string Name { get; set; }

        }

        public sealed class ValueKey
        {
            public readonly object[] DimKeys;
            public ValueKey(params object[] dimKeys)
            {
                DimKeys = dimKeys;
            }
            public override int GetHashCode()
            {
                if (DimKeys == null) return 0;
                int hashCode = DimKeys.Length;
                for (int i = 0; i < DimKeys.Length; i++)
                {
                    hashCode ^= DimKeys[i].GetHashCode();
                }
                return hashCode;
            }
            public override bool Equals(object obj)
            {
                if (obj == null || !(obj is ValueKey))
                    return false;
                var x = DimKeys;
                var y = ((ValueKey)obj).DimKeys;
                if (ReferenceEquals(x, y))
                    return true;
                if (x.Length != y.Length)
                    return false;
                for (int i = 0; i < x.Length; i++)
                {
                    if (!x[i].Equals(y[i]))
                        return false;
                }
                return true;
            }
        }
        private List<Grid>  GetRecordsFromDb(string searchTerm)
        {
            List<RagStatus> records = null;
            using (RagDBContext1 db = new RagDBContext1())
            {
                records = db.RagStatuss.ToList();
            }
            var gridModel = records.Select(x => new Grid()
            {
                Id =x.Id,
                Description =x.AddressLine1,
                Document = x.DocName,
                Name = x.StatusId.ToString(),

            })?.ToList();
            return string.IsNullOrEmpty(searchTerm)? gridModel:
                gridModel.Where(x=>
            string.Equals(x.Description, searchTerm,StringComparison.OrdinalIgnoreCase))?.ToList();
        }


    }

    public class Grid: IComparable<Grid>
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Document { get; set; }
        public string Name { get; set; }

        public int CompareTo(Grid x, Grid y)
        {
            return 1;
        }

        public int CompareTo(Grid other)
        {
            return 1;
        }
    }
}