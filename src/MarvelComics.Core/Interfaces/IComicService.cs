using MarvelComics.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarvelComics.Core.Interfaces
{
    public interface IComicService
    {
        Task<IEnumerable<ComicDTO>> SearchComicsAsync(int characterId, ComicQueryOptions comicSearchOptions);
    }
}