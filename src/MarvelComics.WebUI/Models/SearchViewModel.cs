using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MarvelComics.WebUI.Models
{
    /// <summary>
    /// Created to enable automatic validations using data annotations
    /// and automaticly send data from view to controller via Ajax
    /// </summary>
    public class SearchViewModel
    {
        [Required]
        public string SearchCharacterBy { get; set; }
    }
}