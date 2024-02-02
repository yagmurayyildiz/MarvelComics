using MarvelComics.Core.Models;
using MarvelComics.Infrastructure.External_Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MarvelComics.InfrastructureIntegrationTests
{
    /// <summary>
    /// MarvelAPIServiceTests class is responsible for testing the MarvelAPIService.
    /// It ensures that the service correctly interacts with the Marvel Comics API.
    ///
    /// Functionalities tested:
    /// 1. GetCharacterJsonByNameAsync_ReturnsValidResponse:
    ///    - Tests if the service can fetch character data using a character's name.
    ///    - Validates that the response is not null and contains the specified character's name.
    ///
    /// 2. GetComicsJsonByCharacterIdAsync_ReturnsValidResponse:
    ///    - Tests the service's ability to fetch comic data based on a character's ID.
    ///    - Ensures that the response is not null and includes the character ID, confirming correct data retrieval.
    ///
    /// TODO:
    /// - Expand test coverage to include negative scenarios, such as invalid character names or IDs.
    /// - Test edge cases and error handling, such as API rate limits or network failures.
    /// </summary>
    public class MarvelAPIServiceTests
    {
        private MarvelAPIService _apiService;
        private HttpClient _httpClient;
        private MarvelAPIServiceOptions _options;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri("https://gateway.marvel.com/") };

            _options = new MarvelAPIServiceOptions
            {
                ApiKey = ConfigurationManager.AppSettings["APIKey"],
                PrivateKey = ConfigurationManager.AppSettings["PrivateKey"]
            };

            _apiService = new MarvelAPIService(_httpClient, _options);
        }

        [Test]
        public async Task GetCharacterJsonByNameAsync_ReturnsValidResponse_ShouldReturnResponseJson()
        {
            // Arrange
            var characterName = "Spider-Man";
            var characterSearchRequest = new CharacterSearchRequest
            {
                CharacterName = characterName,
                ResultLimit = 5,
            };

            // Act
            var result = await _apiService.GetCharactersByNameAsync(characterSearchRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Contains(characterName), Is.True);
        }

        [Test]
        public async Task GetComicsJsonByCharacterIdAsync_ReturnsValidResponse_ShouldReturnResponseJson()
        {
            // Arrange 
            var characterId = 1009610;
            var comicSearchRequest = new ComicSearchRequest 
            { 
                CharacterId = characterId, 
                Direction = OrderByDirection.Descending, 
                PageNumber = 2, 
                PageSize = 10 
            };

            // Act
            var result = await _apiService.GetComicsByCharacterIdAsync(comicSearchRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Contains(characterId.ToString()), Is.True);
        }
    }
}
