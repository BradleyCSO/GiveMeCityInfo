using GiveMeCityInfo.Models;
using Microsoft.AspNetCore.Mvc;

namespace GiveMeCityInfo.Services.GetApiService
{
    public interface IApiService
    {
        Task<List<City>> GetCities();
        Task<PaginatedCities> GetFuzzyCities([FromQuery] string? searchQuery, string? pageNumber);
        Task<List<Country>> GetCountries();
        Task<PaginatedCities> GetCitiesByCountry([FromQuery] string[]? countries, [FromQuery] string pageNumber);
        Task<City> GetCityById([FromQuery] string cityId);
        Task<List<PointsOfInterest>> GetPointsOfInterestForCity([FromQuery] string cityId);
    }
}