using GiveMeCityInfo.Models;
using System.Web;
using System.Xml.Linq;

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

        public static async Task<List<City>> GetCitiesByName(string? searchQuery)
        {
            List<City> cities = new();

            // Encode the query
            var query = HttpUtility.ParseQueryString(baseUri.Query);
            query["searchQuery"] = searchQuery;

            HttpResponseMessage response = await httpClient.GetAsync($"{baseUri}?{query}");
            if (response.IsSuccessStatusCode)
            {
                cities = await response.Content.ReadFromJsonAsync<List<City>>() ?? new List<City>();
            }

            return cities;
        }
    }
}
