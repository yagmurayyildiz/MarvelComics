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
    /// CharacterService is responsible for handling character search requests from the UI and fetching results
    /// from the Marvel API service. It acts as an intermediary, processing UI requests and ensuring proper
    /// data mapping for both requests to the API service and responses back to the UI.
    /// 
    /// Key Points:
    /// - Result Limit: Currently, the result limit for API requests is hard-coded for simplicity. Ideally,
    ///   this should be configurable and passed from the UI to allow flexibility.
    /// 
    /// TODO: 
    /// - Current implementation focuses on basic functionality, but this service can include services 
    ///   for advanced validation, error handling, and caching. These additions would contribute to a more 
    ///   robust application and improve overall performance.
    ///   
    /// - Currently, I will use a simplified approach for validation, error handling, and caching will directly 
    ///   within the UI layer.
    /// </summary>
    public class CharacterService : ICharacterService
    {
        private IMarvelAPIService _marvelAPIService;

        public CharacterService(IMarvelAPIService marvelAPIService)
        {
            _marvelAPIService = marvelAPIService;
        }

        public async Task<IEnumerable<CharacterDTO>> SearchCharactersByNameAsync(string characterName)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(characterName))
                {
                    var characterSearchRequest = CreateCharacterSearchRequest(characterName);
                    var jsonResponse = await _marvelAPIService.GetCharactersByNameAsync(characterSearchRequest);

                    if (!string.IsNullOrEmpty(jsonResponse))
                    {
                        var apiResponse = DesarilizeAPIResponse(jsonResponse);
                        return apiResponse?.Data?.Results ?? Enumerable.Empty<CharacterDTO>();
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

            return Enumerable.Empty<CharacterDTO>();
        }

        private CharacterSearchRequest CreateCharacterSearchRequest(string characterName)
        {
            return new CharacterSearchRequest { CharacterName = characterName, ResultLimit = 5 };
        }

        private MarvelApiResponse<CharacterDTO> DesarilizeAPIResponse(string jsonResponse)
        {
            return JsonConvert.DeserializeObject<MarvelApiResponse<CharacterDTO>>(jsonResponse);
        }
    }
}
