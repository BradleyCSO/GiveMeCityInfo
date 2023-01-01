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
                var pageNumber = Request.Query["PageNumber"].FirstOrDefault() ?? "1";

                Cities = await apiService.GetFuzzyCities(SearchQuery, pageNumber);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return Page();
        }
    }
}
