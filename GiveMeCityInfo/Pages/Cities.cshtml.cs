using GiveMeCityInfo.Models;
using GiveMeCityInfo.Services.GetApiService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveMeCityInfo.Pages
{
    [BindProperties(SupportsGet = true)]
    public class CitiesModel : PageModel
    {
        public string? SearchQuery { get; set; }
        public PaginatedCities? Cities { get; set; }
        public List<string>? Countries { get; set; }
        public string[]? SelectedCountries { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            SelectedCountries = Request.Query["SelectedCountries"];

            var pageNumber = Request.Query["PageNumber"].FirstOrDefault() ?? "1";
            var selectedCountries = Request.Query["SelectedCountries"].ToArray();

            Cities = await ApiService.GetCitiesByCountry(selectedCountries, pageNumber);
            Countries = await ApiService.GetCountries();

            return Page();
        }
    }
}