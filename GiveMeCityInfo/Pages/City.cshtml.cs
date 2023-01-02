using GiveMeCityInfo.Models;
using GiveMeCityInfo.Services.GetApiService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveMeCityInfo.Pages
{
    [BindProperties(SupportsGet = true)]
    public class CityModel : PageModel
    {
        private readonly ILogger<CityModel> _logger;

        public City? City { get; set; }
        public List<PointsOfInterest>? PointsOfInterest { get; set; }
        public List<string>? ActivityTypes { get; set; }
        public string? SelectedActivity { get; set; }

        public CityModel(ILogger<CityModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                ApiService apiService = new();
                var cityId = Request.Query["cityId"];
                var activityType = Request.Query["activityType"];

                City = await apiService.GetCityById(cityId);
                PointsOfInterest = await apiService.GetPointsOfInterestForCity(cityId);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return Page();
        }
    }
}
