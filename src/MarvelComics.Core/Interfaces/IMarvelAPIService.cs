using MarvelComics.Core.Models;
using System.Threading.Tasks;

namespace MarvelComics.Core.Interfaces
{
    /// <summary>
    /// This will be the wrapper for the Marvel API interactions
    /// Implementation will be done in Infrastructure
    /// </summary>
    public interface IMarvelAPIService
    {
        Task<string> GetCharactersByNameAsync(CharacterSearchRequest characterSearchRequest);
        Task<string> GetComicsByCharacterIdAsync(ComicSearchRequest comicSearchRequest);
    }
}
