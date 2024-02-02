using System.Collections.Generic;

namespace MarvelComics.Core.Models
{
    /// <summary>
    /// A generic approach to help desarilization of api json response 
    /// T will be used to for CharacterDTO or ComicResponse
    /// To get thumbnail from API response ComicResponse created and 
    /// will be map to ComicDTO after deserilazing API results
    /// No need to create CharacterResponse, since it only needs a simple response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MarvelApiResponse<T>
    {
        public int Code { get; set; }
        public string Status { get; set; }
        public MarvelApiData<T> Data { get; set; }
    }

    public class MarvelApiData<T>
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }
        public List<T> Results { get; set; }
    }

    public class ComicResponse
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Thumbnail Thumbnail { get; set; }
    }

    public class Thumbnail
    {
        public string Path { get; set; }
        public string Extension { get; set; }
    }
}
