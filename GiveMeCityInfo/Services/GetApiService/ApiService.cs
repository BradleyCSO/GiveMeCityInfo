using GiveMeCityInfo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Web;

namespace GiveMeCityInfo.Services.GetApiService
{
    public static class ApiService
    {
        static readonly UriBuilder baseUri = new("https://localhost:7192/api/v2/cities");
        static readonly HttpClient httpClient = new();

        public static async Task<List<City>> GetCities()
        {
            List<City> cities = new();

            HttpResponseMessage response = await httpClient.GetAsync(baseUri.Host);
            if (response.IsSuccessStatusCode)
            {
                cities = await response.Content.ReadFromJsonAsync<List<City>>() ?? new List<City>();
            }

            return cities;
        }
        public static async Task<List<string>> GetCountries()
        {
            List<string> countries = new();

            HttpResponseMessage response = await httpClient.GetAsync($"{baseUri}/GetCountries");

            if (response.IsSuccessStatusCode)
            {
                countries = await response.Content.ReadFromJsonAsync<List<string>>() ?? new List<string>();
            }

            return countries;
        }
        public static async Task<PaginatedCities> GetCitiesByCountry([FromQuery] string[]? countries, [FromQuery] string pageNumber)
        {
            if (pageNumber == null)
            {
                pageNumber = "1";
            }
            PaginatedCities res = new();

            // Encode the query
            var query = HttpUtility.ParseQueryString(baseUri.Query);

            // Build query string
            foreach (var country in countries)
            {
                query.Add("country", country);
            }
            query["pageNumber"] = pageNumber;

            HttpResponseMessage response = await httpClient.GetAsync($"{baseUri}?{query}");
            if (response.IsSuccessStatusCode)
            {
                res.Cities = await response.Content.ReadFromJsonAsync<List<City>>() ?? new List<City>();
                
                // Deserialise pagination header meta tag into PaginationDetails model
                res.Pagination = JsonSerializer.Deserialize<PaginationDetails>(response?.Headers.GetValues("X-Pagination").FirstOrDefault());
            }

            return res;
        }
    }
}