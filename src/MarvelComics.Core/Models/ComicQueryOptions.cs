namespace MarvelComics.Core.Models
{
    /// <summary>
    /// To get query options from UI to pass API via ComicService
    /// For simplicity OrderBy and FilterBy will be restricted for 'title'
    /// Ideally, OrderBy and FilterBy properties should define and limited 
    /// by abstract class or enum to encapsulate valid fields and prevent invalid requests 
    /// </summary>
    public class ComicQueryOptions
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public OrderByOption OrderBy { get; set; }
        public FilterOption FilterBy { get; set; } 
    }

    public class OrderByOption
    {
        public string OrderBy { get; set; }
        public OrderByDirection Direction { get; set; } = OrderByDirection.Ascending;
    }

    public enum OrderByDirection
    {
        Ascending,
        Descending
    }

    public class FilterOption
    {
        public string FilterBy { get; set; }
        public string Filter { get; set; }
    }
}
