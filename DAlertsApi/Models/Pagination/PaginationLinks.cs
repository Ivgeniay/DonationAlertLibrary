namespace DAlertsApi.Models.Pagination
{
    /// <summary>
    /// First:      links to the first page
    /// Last:       links to the last page
    /// Prev:       links to the previous page
    /// Next:       links to the next page
    /// </summary>
    public class PaginationLinks
    {
        public string? First { get; set;} = string.Empty;           
        public string? Last { get; set; } = string.Empty;           
        public string? Prev { get; set; } = string.Empty;           
        public string? Next { get; set; } = string.Empty;           
    }
}
