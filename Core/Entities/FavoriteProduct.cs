namespace Core.Entities
{
    public class FavoriteProduct
    {
        public int Id {get; set;}
        public string UserId { get; set; }
        public User User { get; set; }
        public int ProductId { get; set; }
    }
}