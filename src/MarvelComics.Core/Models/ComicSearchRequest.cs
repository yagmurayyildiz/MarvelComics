namespace MarvelComics.Core.Models
{
    /// <summary>
    /// To pass request data from comic service to api
    /// As a result of the decision to restrict filtering and ordering by title
    /// only the filter data and direction will be pass to api service
    /// Offset will be calculated by comic service
    /// </summary>
    public class ComicSearchRequest
    {
        public int CharacterId { get; set; }
        public string Filter { get; set; } // filter data for title
        public OrderByDirection Direction { get; set; } // order direction for title
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
}
