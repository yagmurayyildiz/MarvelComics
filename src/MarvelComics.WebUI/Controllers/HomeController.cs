using MarvelComics.Core.Interfaces;
using MarvelComics.Core.Models;
using MarvelComics.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MarvelComics.WebUI.Controllers
{
    /// <summary>
    /// Controller responsible for handling requests related to characters and comics search.
    /// </summary>

    public class HomeController : Controller
    {
        private ICharacterService _characterService;
        private IComicService _comicService;
        /// <summary>
        /// Initializes a new instance of the HomeController class.
        /// </summary>
        /// <param name="characterService">Service for handling character-related operations.</param>
        /// <param name="comicService">Service for handling comic-related operations.</param>
        public HomeController(ICharacterService characterService, IComicService comicService)
        {
            _characterService = characterService;
            _comicService = comicService;
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Searches for characters by name and returns the results in a partial view.
        /// </summary>
        /// <param name="searchCharacterBy">The character name to search for.</param>
        [HttpPost]
        [ValidateAntiForgeryToken] // validates the token sent by form in view
        public async Task<ActionResult> Search(string searchCharacterBy)
        {
            try
            {
                // TODO: use cancelation token to cancel any ongoing process
                var characters = await _characterService.SearchCharactersByNameAsync(searchCharacterBy);
                return PartialView("_CharacterSearchResults", characters);
            }
            catch (Exception)
            {
                // TODO: Handle errors to show specific messages or logging
                // In this aproach I will let global error handler to catch error and handle
                throw;
            }
        }

        /// <summary>
        /// Retrieves comic search results based on the character ID and provided search options.
        /// </summary>
        /// <param name="characterId">The ID of the character to base the search on.</param>
        /// <param name="options">Search options for filtering comics.</param>
        public async Task<ActionResult> GetComicSearchResults(int characterId, ComicSearchOptionsViewModel options)
        {
            var comics = Enumerable.Empty<ComicDTO>();
            try
            {
                comics = await _comicService.SearchComicsAsync(characterId, MapFromOptionsViewModel(options));
            }
            catch (Exception)
            {
                // TODO: Handle errors to show specific messages or logging
                // In this aproach I will return an empty list to show not found message in view
            }

            return PartialView("_ComicSearchResults", comics);
        }

        private ComicQueryOptions MapFromOptionsViewModel(ComicSearchOptionsViewModel model)
        {
            return new ComicQueryOptions
            {
                PageNumber = model.PageNumber,
                PageSize = model.PageSize,
                OrderBy = new OrderByOption { OrderBy = "title", Direction = model.OrderBy },
                FilterBy = new FilterOption { FilterBy = "title", Filter = model.FilterBy }
            };
        }
        public ActionResult _Spinner()
        {
            return PartialView();
        }
    }
}