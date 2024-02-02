using MarvelComics.Core.Interfaces;
using MarvelComics.Core.Models;
using MarvelComics.Core.Services;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarvelComics.CoreUnitTests
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
    /// Increase test cases for not matching character names, case sensitivity, exceptions etc.
    /// </summary>
    [TestFixture]
    public class CharacterServiceTests
    {
        private Mock<IMarvelAPIService> _mockMarvelAPIService;
        private CharacterService _characterService;
        [SetUp]
        public void SetUp()
        {
            _mockMarvelAPIService = new Mock<IMarvelAPIService>();
            _characterService = new CharacterService(_mockMarvelAPIService.Object);
        }

        [Test]
        public async Task SearchCharactersByNameAsync_GivenValidName_ShouldReturnMatchingCharacters()
        {
            //Arrange
            var characterName = "Iron";
            var character = new CharacterDTO { Id = 1, Name = "Iron Man" };
            var expectedResult = new List<CharacterDTO> { character };
            var mockJsonResponse = @"{
                ""code"": 200,
                ""status"": ""Ok"",
                ""data"": {
                    ""results"": [
                        { ""id"": 1, ""name"": ""Iron Man"" }
                    ]
                }
            }";
            _mockMarvelAPIService.Setup(s => s.GetCharactersByNameAsync(It.IsAny<CharacterSearchRequest>())).ReturnsAsync(mockJsonResponse);

            //Act
            var actual = await _characterService.SearchCharactersByNameAsync(characterName);

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Not.Empty);
            CollectionAssert.AreEquivalent(actual, expectedResult);
            _mockMarvelAPIService.Verify(s => s.GetCharactersByNameAsync(It.IsAny<CharacterSearchRequest>()), Times.Once); // Verifies method hasn't been called more than once
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public async Task SearchCharactersByNameAsync_GivenInvalidCharacterName_ShouldReturnEmptyList(string characterName)
        {
            //Arrange
            var expectedResult = Enumerable.Empty<CharacterDTO>();

            //Act
            var actual = await _characterService.SearchCharactersByNameAsync(characterName);

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Empty);
            _mockMarvelAPIService.Verify(s => s.GetCharactersByNameAsync(It.IsAny<CharacterSearchRequest>()), Times.Never); // Verifies method never been called 
        }

        [Test]
        public async Task SearchCharactersByNameAsync_WithInvalidJson_ShouldReturnEmptyList()
        {
            //Arrange
            var characterName = "Iron";
            var expectedResult = Enumerable.Empty<CharacterDTO>();
            var mockJsonResponse = "Invalid JSON";
            _mockMarvelAPIService.Setup(s => s.GetCharactersByNameAsync(It.IsAny<CharacterSearchRequest>())).ReturnsAsync(mockJsonResponse);

            //Act
            var actual = await _characterService.SearchCharactersByNameAsync(characterName);

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Empty);
            CollectionAssert.AreEquivalent(actual, expectedResult);
            _mockMarvelAPIService.Verify(s => s.GetCharactersByNameAsync(It.IsAny<CharacterSearchRequest>()), Times.Once); // Verifies method hasn't been called more than once
        }
    }
}
