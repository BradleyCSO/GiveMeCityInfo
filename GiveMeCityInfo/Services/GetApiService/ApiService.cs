﻿using GiveMeCityInfo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Web;

namespace GiveMeCityInfo.Services.GetApiService
{
    public class ApiService : IApiService
    {
        private readonly ILogger<ApiService> _logger;
        private readonly UriBuilder baseUri = new("https://localhost:7192/api/v2/cities");
        private readonly HttpClient httpClient = new();

        public ApiService(ILogger<ApiService> logger)
        {
            _logger = logger;
        }

        public ApiService()
        {
        }

        public async Task<List<City>> GetCities()
        {
            List<City> cities = new();

            HttpResponseMessage response = await httpClient.GetAsync(baseUri.Host);
            if (response.IsSuccessStatusCode)
            {
                cities = await response.Content.ReadFromJsonAsync<List<City>>() ?? new List<City>();
            }
            else
            {
                // Add error handling code here
                _logger.LogError($"Error getting cities: {response.ReasonPhrase}");
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
            else
            {
                _logger.LogError($"Error getting query {searchQuery}: {response.ReasonPhrase}");
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
            else
            {
                _logger.LogError($"Error getting countries: {response.ReasonPhrase}");
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
            else
            {
                _logger.LogError($"Error getting cities from {countries}: {response.ReasonPhrase}");
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
            else
            {
                _logger.LogError($"Error getting city by id {cityId}: {response.ReasonPhrase}");
            }

            return res;
        }
        public async Task<List<PointsOfInterest>> GetPointsOfInterestForCity([FromQuery] string cityId)
        {
            List<PointsOfInterest> res = new();

            HttpResponseMessage response = await httpClient.GetAsync($"{baseUri}/{cityId}/pointsofinterest/");

            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<PointsOfInterest>>() ?? new List<PointsOfInterest>();
            }
            else
            {
                _logger.LogError($"Error getting points of interest for city with id {cityId}: {response.ReasonPhrase}");
            }

            return res;
        }
    }
}