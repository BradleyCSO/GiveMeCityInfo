using GiveMeCityInfo.Models;
using Microsoft.AspNetCore.Mvc;

namespace GiveMeCityInfo.Services.GetApiService
{
    public interface IApiService
    {
        Task<List<City>> GetCities();
        Task<PaginatedCities> GetCitiesByCountry([FromQuery] string[]? countries, [FromQuery] string pageNumber);
        Task<City> GetCityById([FromQuery] string cityId);
        Task<List<Country>> GetCountries();
    }
}