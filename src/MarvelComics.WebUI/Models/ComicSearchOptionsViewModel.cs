using MarvelComics.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarvelComics.WebUI.Models
{
    public class ComicSearchOptionsViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string FilterBy { get; set; }
        public OrderByDirection OrderBy { get; set; } //0 for asc, 1 for desc
    }
}