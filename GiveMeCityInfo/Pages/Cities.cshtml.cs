using GiveMeCityInfo.Models;
using GiveMeCityInfo.Services.GetApiService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace GiveMeCityInfo.Pages
{
    public class CitiesModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? SearchQuery { get; set; }
        public List<City>? Cities { get; set; }
        public async Task OnGetAsync()
        {
            Cities = await ApiService.GetCitiesByName(SearchQuery);
        }
    }
}
