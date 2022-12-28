using GiveMeCityInfo.Models;
using GiveMeCityInfo.Services.GetApiService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveMeCityInfo.Pages
{
    public class CityModel : PageModel
    {
        public City? City { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var cityId = Request.Query["cityId"];
            City = await ApiService.GetCityById(cityId);
            return Page();
        }
    }
}
