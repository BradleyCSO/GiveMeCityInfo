namespace GiveMeCityInfo.Models
{
    public class PaginatedCities
    {
        public List<City> Cities { get; set; }
        public PaginationDetails Pagination { get; set; }
    }
}
