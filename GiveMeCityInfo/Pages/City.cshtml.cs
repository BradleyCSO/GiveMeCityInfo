using GiveMeCityInfo.Models;
using GiveMeCityInfo.Services.GetApiService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveMeCityInfo.Pages
{
    public class CityModel : PageModel
    {
        private readonly ILogger<CityModel> _logger;

        public City? City { get; set; }

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

                City = await apiService.GetCityById(cityId);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return Page();
        }
    }
}
