namespace DAlertsApi.Models.Pagination
{
    public class PaginationInfo
    {
        public int CurrentPage { get; set; }          // Индекс текущей страницы
        public int From { get; set; }                 // Индекс первого элемента
        public int LastPage { get; set; }             // Общее количество страниц
        public string Path { get; set; } = string.Empty; // Текущий путь
        public int PerPage { get; set; }              // Количество элементов на странице
        public int To { get; set; }                   // Индекс последнего элемента
        public int Total { get; set; }                // Общее количество элементов

    }
}
