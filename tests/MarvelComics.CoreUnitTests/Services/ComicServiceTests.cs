using MarvelComics.Core.Interfaces;
using MarvelComics.Core.Models;
using MarvelComics.Core.Services;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MarvelComics.CoreUnitTests.Services
{
    /// <summary>
    /// Developed using TDD with FIRST principles, ensuring that tests are 
    /// Fast, Independent, Repeatable, Self-validating, and Timely. 
    /// The development followed the Red-Green-Refactor cycle:
    /// 1. Red: Wrote initial failing tests for new features.
    /// 2. Green: Implemented minimal code changes to pass the tests.
    /// 3. Refactor: Improved the code while keeping tests green.
    /// The 'Fake it 'Til You Make it' approach was also employed to 
    /// achieve a green test bar, followed by iterative refinement of the code.
    /// 
    /// TODO:
    /// Increase test cases
    /// </summary>
    [TestFixture]
    public class ComicServiceTests
    {
        private Mock<IMarvelAPIService> _mockMarvelAPIService;
        private ComicService _comicService;

        [SetUp]
        public void SetUp()
        {
            _mockMarvelAPIService = new Mock<IMarvelAPIService>();
            _comicService = new ComicService(_mockMarvelAPIService.Object);
        }

        [Test]
        public async Task SearchComicsAsync_GivenValidCharacterIdAndOptions_ShouldReturnComics()
        {
            //Arrange
            var characterId = 1;
            var comicSearchOptions = new ComicQueryOptions();
            var expectedResult = new List<ComicDTO> { new ComicDTO { Title = "Avengers", Description = "some text", ThumbnailUrl = "imagePath/portrait_xlarge.jpg" } }; 
            var mockJsonResponse = @"{
                ""code"": 200,
                ""status"": ""Ok"",
                ""data"": {
                    ""results"": [
                        { 
                            ""id"": 1, 
                            ""title"": ""Avengers"",
                            ""thumbnail"": {
                                ""path"": ""imagePath"",
                                ""extension"": ""jpg""
                            },
                            ""description"":""some text""
                        }
                    ]
                }
            }";
            _mockMarvelAPIService.Setup(s => s.GetComicsByCharacterIdAsync(It.IsAny<ComicSearchRequest>())).ReturnsAsync(mockJsonResponse);

            //Act
            var actual = await _comicService.SearchComicsAsync(characterId, comicSearchOptions);

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Not.Empty);
            CollectionAssert.AreEquivalent(actual, expectedResult);
            _mockMarvelAPIService.Verify(s => s.GetComicsByCharacterIdAsync(It.IsAny<ComicSearchRequest>()), Times.Once); // Verifies method has been called only once
        }

        [Test]
        public async Task SearchComicsAsync_GivenInValidOptions_ShouldReturnComics()
        {
            //Arrange
            var characterId = 1;
            var expectedResult = new List<ComicDTO> { new ComicDTO { Title = "Avengers", Description = "some text", ThumbnailUrl = "imagePath/portrait_xlarge.jpg" } };
            var mockJsonResponse = @"{
                ""code"": 200,
                ""status"": ""Ok"",
                ""data"": {
                    ""results"": [
                        { 
                            ""id"": 1, 
                            ""title"": ""Avengers"",
                            ""thumbnail"": {
                                ""path"": ""imagePath"",
                                ""extension"": ""jpg""
                            },
                            ""description"":""some text""
                        }
                    ]
                }
            }";
            _mockMarvelAPIService.Setup(s => s.GetComicsByCharacterIdAsync(It.IsAny<ComicSearchRequest>())).ReturnsAsync(mockJsonResponse);

            //Act
            var actual = await _comicService.SearchComicsAsync(characterId, null);

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Not.Empty);
            CollectionAssert.AreEquivalent(actual, expectedResult);
            _mockMarvelAPIService.Verify(s => s.GetComicsByCharacterIdAsync(It.IsAny<ComicSearchRequest>()), Times.Once); // Verifies method has been called only once
        }

        [TestCase(0)]
        [TestCase(-1)]
        public async Task SearchComicsAsync_GivenInValidCharacterId_ShouldReturnEmptyList(int characterId)
        {
            //Arrange
            var expectedResult = new List<ComicDTO> { new ComicDTO { Title = "Avengers", Description = "some text", ThumbnailUrl = "imagePath/portrait_xlarge.jpg" } };
            
            //Act
            var actual = await _comicService.SearchComicsAsync(characterId, null);

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Empty);
            _mockMarvelAPIService.Verify(s => s.GetComicsByCharacterIdAsync(It.IsAny<ComicSearchRequest>()), Times.Never); // Verifies method hasn't been called
        }

        [Test]
        public async Task SearchComicsAsync_WhenApiServiceThrowsException_ThrowsSameException()
        {
            // Arrange
            _mockMarvelAPIService.Setup(client => client.GetComicsByCharacterIdAsync(It.IsAny<ComicSearchRequest>()))
                                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act & Assert
            Assert.ThrowsAsync<HttpRequestException>(async () =>
                await _comicService.SearchComicsAsync(1, new ComicQueryOptions()));
        }
    }
}
