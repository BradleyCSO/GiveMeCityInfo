using System.Text.Json.Serialization;

namespace GiveMeCityInfo.Models
{
    public class PaginationDetails
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
        public int TotalPageCount { get; set; }
    }
}