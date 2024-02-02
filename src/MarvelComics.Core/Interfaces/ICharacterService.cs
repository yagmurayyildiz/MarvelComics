﻿using MarvelComics.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarvelComics.Core.Interfaces
{
    public interface ICharacterService
    {
        Task<IEnumerable<CharacterDTO>> SearchCharactersByNameAsync(string characterName);
    }
}