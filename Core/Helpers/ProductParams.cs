namespace Core.Helpers
{
    public class ProductParams
    {
        private int pageSize = 10;
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            set { pageSize = value > 10 ? 10 : value; }
            get => pageSize;
        }

        public bool SortByNewest { get; set; }
        public bool SortByLowerPrice { get; set; }
        public bool SortByHigerPrice { get; set; }
        public string Search { get; set; }
        public string CategoryNameFilter { get; set; }
        public string SubCategoryNameFilter { get; set; }
    }
}