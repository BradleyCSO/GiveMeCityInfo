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
            Cities = await ApiService.GetCitiesByCountry(SelectedCountries, Request.Query["PageNumber"]);
            Countries = await ApiService.GetCountries();

            return Page();
        }
    }
}