namespace API.Helpers
{
    public class ProductParams
    {
        private const int pageSize = 10;
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => pageSize;
        }
    }
}