using System.Collections.Generic;

namespace Privote_Grid.Models
{
     
    public class DynamicGridModel
    {
        public List<RowModel> RowModels { get; set; }
        public ColumnModel ColumnModel { get; set; }
        public PagingRequestModel PagingRequest { get; set; }
        public string ColumnHeading { get; set; }
        public string RowHeading { get; set; }
        public DynamicGridModel()
        {
            RowModels = new List<RowModel>();
            ColumnModel = new ColumnModel();
            PagingRequest = new PagingRequestModel();
        }
        }

    public enum AnchorTagType
    {
        ActionLink,
        Self,
        Blank,
    }
     

    public class PagingRequestModel
    {
        private int _pageSize;
        public string PagingUrl { get; set; }
        public int PageNumber { get; set; }
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value == 0 ? 10 : value;
            }
        }
        public int TotalRecord { get; set; }
        public string SearchTerm { get; set; }
        public bool SortColumn { get; set; }
        public bool SortRow { get; set; }
        public SortDirection SortDirection { get; set; }
        public PagingRequestModel()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public PagingRequestModel(string searchTerm, int pageNumber, int pageSize, bool sortColumn, bool sortRow, SortDirection sortDirection)
        {
            SearchTerm = searchTerm;
            PageNumber = pageNumber;
            PageSize = pageSize == 0 ? 4 : pageSize;
            SortColumn = sortColumn;
            SortRow = sortRow;
            SortDirection = sortDirection;
        }
    }

    public enum SortDirection
    {
        Desc,
        Asc
    }
    public class RowModel
    {
        public string Heading { get; set; }
        public List<DataModel> DataModels { get; set; }
    }

    public class ColumnModel
    {
        public string Heading { get; set; }
        public  List<string> Columns {get; set;}
    }

     
    public abstract class DataModel
    {
       
        public string CssClass { get; set; }
    }
}
