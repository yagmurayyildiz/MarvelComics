using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelComics.Core.Models
{
    /// <summary>
    /// Created to keep API key and Private Key in configration file 
    /// and set values in application start up
    /// </summary>
    public class MarvelAPIServiceOptions

    {
        public string ApiKey { get; set; }
        public string PrivateKey { get; set; }
    }
}
