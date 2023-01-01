using GiveMeCityInfo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Web;

namespace GiveMeCityInfo.Services.GetApiService
{
    public class ApiService : IApiService
    {
        private readonly UriBuilder baseUri = new("https://localhost:7192/api/v2/cities");
        private readonly HttpClient httpClient = new();

        public async Task<List<City>> GetCities()
        {
            List<City> cities = new();

            HttpResponseMessage response = await httpClient.GetAsync(baseUri.Host);
            if (response.IsSuccessStatusCode)
            {
                cities = await response.Content.ReadFromJsonAsync<List<City>>() ?? new List<City>();
            }

            return cities;
        }

        public async Task<PaginatedCities> GetFuzzyCities([FromQuery] string? searchQuery, string? pageNumber)
        {
            PaginatedCities res = new();

            // Encode the query
            var query = HttpUtility.ParseQueryString(baseUri.Query);
            query.Add("searchQuery", searchQuery);
            query["pageNumber"] = pageNumber;

            HttpResponseMessage response = await httpClient.GetAsync($"{baseUri}?{query}");

            if (response.IsSuccessStatusCode)
            {
                res.Cities = await response.Content.ReadFromJsonAsync<List<City>>() ?? new List<City>();

                // Deserialise pagination header meta tag into PaginationDetails model
                res.Pagination = JsonSerializer.Deserialize<PaginationDetails>(response?.Headers?.GetValues("X-Pagination").FirstOrDefault() ?? "");
            }

            return res;
        }

        public async Task<List<Country>> GetCountries()
        {
            List<Country> countries = new();

            HttpResponseMessage response = await httpClient.GetAsync($"{baseUri}/GetCountries");

            if (response.IsSuccessStatusCode)
            {
                countries = await response.Content.ReadFromJsonAsync<List<Country>>() ?? new List<Country>();
            }

            return countries;
        }

        public async Task<PaginatedCities> GetCitiesByCountry([FromQuery] string[]? countries, [FromQuery] string pageNumber)
        {
            if (pageNumber == null)
            {
                pageNumber = "1";
            }
            PaginatedCities res = new();

            // Encode the query
            var query = HttpUtility.ParseQueryString(baseUri.Query);

            // Build query string
            foreach (var country in countries ?? new string[0])
            {
                query.Add("countries", country);
            }
            query["pageNumber"] = pageNumber;

            HttpResponseMessage response = await httpClient.GetAsync($"{baseUri}?{query}");
            if (response.IsSuccessStatusCode)
            {
                res.Cities = await response.Content.ReadFromJsonAsync<List<City>>() ?? new List<City>();

                // Deserialise pagination header meta tag into PaginationDetails model
                res.Pagination = JsonSerializer.Deserialize<PaginationDetails>(response?.Headers?.GetValues("X-Pagination").FirstOrDefault() ?? "");
            }

            return res;
        }

        public async Task<City> GetCityById([FromQuery] string cityId)
        {
            City res = new();

            HttpResponseMessage response = await httpClient.GetAsync($"{baseUri}/{cityId}");
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<City>() ?? new City();
            }

            return res;
        }
    }
}