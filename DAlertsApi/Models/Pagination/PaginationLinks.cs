using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAlertsApi.Models.Pagination
{
    public class PaginationLinks
    {
        public string First { get; set;} = string.Empty;             // Ссылка на первую страницу
        public string Last { get; set; } = string.Empty;             // Ссылка на последнюю страницу
        public string Prev { get; set; } = string.Empty;             // Ссылка на предыдущую страницу
        public string Next { get; set; } = string.Empty;             // Ссылка на следующую страницу
    }
}
