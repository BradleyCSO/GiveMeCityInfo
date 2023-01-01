using GiveMeCityInfo.Models;
using GiveMeCityInfo.Services.GetApiService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveMeCityInfo.Pages
{
    [BindProperties(SupportsGet = true)]
    public class CitiesModel : PageModel
    {
        private readonly ILogger _logger;
        public PaginatedCities? Cities { get; set; }
        public List<Country>? Countries { get; set; }
        public List<string>? SelectedCountries { get; set; }

        public CitiesModel(ILogger<CitiesModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                ApiService apiService = new();
                var selectedCountries = Request.Query["SelectedCountries"].ToArray();
                var pageNumber = Request.Query["PageNumber"].FirstOrDefault() ?? "1";

                Cities = await apiService.GetCitiesByCountry(selectedCountries, pageNumber);
                Countries = await apiService.GetCountries();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return Page();
        }
    }
}