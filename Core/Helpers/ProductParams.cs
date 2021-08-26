namespace Core.Helpers
{
    public class ProductParams
    {
        private const int maxPageSize = 100;
        private int pageSize = 6;
        // public int MaxPageNumber { get; set; }
        // public int NumberOfPages { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            set { pageSize = (value > maxPageSize) ? 10 : value; }
            get => pageSize;
        }

        public bool SortByNewest { get; set; }
        public bool SortByLowerPrice { get; set; }
        public bool SortByHigerPrice { get; set; }
        public string Search { get; set; }
        public int CategoryIdFilter { get; set; } = 0;
        public int SubCategoryIdFilter { get; set; } = 0;
    }
}