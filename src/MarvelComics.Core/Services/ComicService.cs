using MarvelComics.Core.Interfaces;
using MarvelComics.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarvelComics.Core.Services
{
    /// <summary>
    /// ComicService is responsible for handling comic search requests from the UI and fetching results
    /// from the Marvel API service. It acts as an intermediary, processing UI requests and ensuring proper
    /// data mapping for both requests to the API service and responses back to the UI.
    /// 
    /// Key Points:
    /// - ComicSearchRequest: Currently, default values of the ComicSearchRequest properties is hard-coded for simplicity.
    /// 
    /// TODO: 
    /// - Current implementation focuses on basic functionality, but this service can include services 
    ///   for advanced validation, error handling, and caching. These additions would contribute to a more 
    ///   robust application and improve overall performance.
    ///   
    /// - Currently, I will use a simplified approach for validation, error handling, and caching will directly 
    ///   within the UI layer.
    ///    
    /// - Add Automapper to perform automated maping between ComicResponse and ComicDTO
    /// </summary>
    public class ComicService : IComicService
    {
        private IMarvelAPIService _marvelAPIService;

        public ComicService(IMarvelAPIService marvelAPIService)
        {
            _marvelAPIService = marvelAPIService;
        }

        public async Task<IEnumerable<ComicDTO>> SearchComicsAsync(int characterId, ComicQueryOptions comicSearchOptions)
        {
            try
            {
                if (characterId > 0)
                {
                    var comicSearchRequest = CreateComicSearchRequest(characterId, comicSearchOptions);
                    var jsonResponse = await _marvelAPIService.GetComicsByCharacterIdAsync(comicSearchRequest);

                    if (!string.IsNullOrEmpty(jsonResponse))
                    {
                        var apiResponse = DesarilizeComicAPIResponse(jsonResponse);
                        if (apiResponse?.Data?.Results != null)
                            return apiResponse.Data.Results.Select(r => MapFromComicResponse(r));
                    }
                }
            }
            catch (JsonReaderException ex)
            {
                // This exception can be caused by invalid api responses,
                // this scnario won't be handled in this test project.
                // Instead method will return empty list.

                // TODO log exception
            }
            catch (Exception ex)
            {
                // All other exceptions will be handled in UI layer
                throw;
            }

            return Enumerable.Empty<ComicDTO>();
        }


        private ComicSearchRequest CreateComicSearchRequest(int characterId, ComicQueryOptions comicSearchOptions)
        {
            if (comicSearchOptions == null)
                comicSearchOptions = new ComicQueryOptions();

            return new ComicSearchRequest
            {
                CharacterId = characterId,
                Filter = comicSearchOptions.FilterBy?.Filter,
                Direction = comicSearchOptions.OrderBy?.Direction ?? default(OrderByDirection),
                PageNumber = comicSearchOptions.PageNumber ?? 1,
                PageSize = comicSearchOptions.PageSize ?? 20
            };
        }


        private MarvelApiResponse<ComicResponse> DesarilizeComicAPIResponse(string jsonResponse)
        {
            return JsonConvert.DeserializeObject<MarvelApiResponse<ComicResponse>>(jsonResponse);
        }

        private ComicDTO MapFromComicResponse(ComicResponse comicResponse)
        {
            return new ComicDTO
            {
                Title = comicResponse.Title,
                Description = comicResponse.Description,
                ThumbnailUrl = GetThumbnailUrl(comicResponse.Thumbnail.Path, comicResponse.Thumbnail.Extension)
            };
        }

        private string GetThumbnailUrl(string url, string extension)
        {
            string imageVariant = "portrait_xlarge";
            return $"{url}/{imageVariant}.{extension}";
        }
    }
}
