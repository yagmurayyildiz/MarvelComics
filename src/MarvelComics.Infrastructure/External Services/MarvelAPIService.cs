using MarvelComics.Core.Interfaces;
using MarvelComics.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MarvelComics.Infrastructure.External_Services
{
    /// <summary>
    /// The MarvelAPIService class is responsible for interacting with the Marvel Comics API.
    /// It handles constructing and sending requests to fetch character and comic data,
    /// and processes the responses received from the API.
    ///
    /// Functionalities:
    /// - GetCharactersByNameAsync: Fetches character data based on a name search.
    /// - GetComicsByCharacterIdAsync: Retrieves comic data related to a specific character ID.
    /// - Pagination support is provided for fetching comics data.
    /// - Implements Marvel's required authentication mechanism for API requests.
    ///
    /// TODO:
    /// 1. Error Handling can be expanded to manage specific HTTP status codes and network exceptions.
    ///    It can provide more detailed error information to the caller.
    ///    Logging errors can be also integrated, for troubleshooting and monitoring.
    ///
    /// 2. The query string building process can modularize for better readability and reuse.
    ///    For example a dedicated class for handling query parameters.
    ///
    /// 3. Input validation can be improved to ensure data intigrity.
    /// </summary>
    public class MarvelAPIService : IMarvelAPIService
    {
        private readonly HttpClient _httpClient;
        private readonly MarvelAPIServiceOptions _options;

        public MarvelAPIService(HttpClient httpClient, MarvelAPIServiceOptions options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        public async Task<string> GetCharactersByNameAsync(CharacterSearchRequest characterSearchRequest)
        {
            //Add search parameters to request url
            var parameters = new NameValueCollection();
            parameters["nameStartsWith"] = characterSearchRequest.CharacterName;
            parameters["limit"] = characterSearchRequest.ResultLimit.ToString();

            var requestUrl = GenerateQueryString("/v1/public/characters", parameters);

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetComicsByCharacterIdAsync(ComicSearchRequest comicSearchRequest)
        {
            //Add search parameters to request url
            var parameters = new NameValueCollection();
            if (!string.IsNullOrEmpty(comicSearchRequest.Filter))
                parameters["titleStartsWith"] = comicSearchRequest.Filter;
            parameters["orderBy"] = GetOrderDirection("title", comicSearchRequest.Direction);
            parameters["limit"] = comicSearchRequest.PageSize.ToString();
            parameters["offset"] = CalculateOffset(comicSearchRequest.PageNumber, comicSearchRequest.PageSize).ToString();

            var endPoint = $"/v1/public/characters/{comicSearchRequest.CharacterId}/comics";
            var requestUrl = GenerateQueryString(endPoint, parameters);

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private int CalculateOffset(int pageNumber, int pageSize)
        {
            if (pageNumber > 1)
                return (pageNumber - 1) * pageSize;
            return 0;
        }

        private string GetOrderDirection(string orderBy, OrderByDirection direction)
        {
            if (direction == OrderByDirection.Descending)
            {
                return "-" + orderBy;
            }
            return orderBy;
        }

        private string GenerateQueryString(string endPoint, NameValueCollection parameters)
        {
            string timestamp = GenerateTimeStamp();
            string toBeHashed = timestamp + _options.PrivateKey + _options.ApiKey;
            string hash = CreateMD5Hash(toBeHashed);

            var fullUrl = new Uri(_httpClient.BaseAddress, endPoint);
            var builder = new UriBuilder(fullUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["apikey"] = _options.ApiKey;
            query["ts"] = timestamp;
            query["hash"] = hash;
            query.Add(parameters);
            builder.Query = query.ToString();

            return builder.ToString();
        }

        private string GenerateTimeStamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        }

        private string CreateMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
