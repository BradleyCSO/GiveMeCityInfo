using GiveMeCityInfo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveMeCityInfo.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Search? Search { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("/Cities", new { Search?.SearchQuery });
        }
    }
}