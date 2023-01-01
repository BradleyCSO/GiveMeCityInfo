using GiveMeCityInfo.Models;
using GiveMeCityInfo.Services.GetApiService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveMeCityInfo.Pages
{
    [BindProperties(SupportsGet = true)]
    public class SearchResultsModel : PageModel
    {
        private readonly ILogger _logger;
        public string? SearchQuery { get; set; }
        public PaginatedCities Cities{ get; set; }

        public SearchResultsModel(ILogger<SearchResultsModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                ApiService apiService = new();
                Cities = await apiService.GetFuzzyCities(SearchQuery);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return Page();
        }
    }
}
