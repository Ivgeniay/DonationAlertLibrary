namespace DAlertsApi.Models.Pagination
{
    /// <summary>
    /// CurrentPage:        current index page
    /// From:               first element index
    /// LastPage:           last element index
    /// Path:               current path
    /// PerPage:            count elements per page
    /// To:                 last element index
    /// Total:              total elements count
    /// </summary>
    public class PaginationInfo
    {
        public int? CurrentPage { get; set; }               
        public int? From { get; set; }                      
        public int? LastPage { get; set; }                  
        public string? Path { get; set; } = string.Empty;   
        public int? PerPage { get; set; }                   
        public int? To { get; set; }                        
        public int? Total { get; set; }                     

    }
}
