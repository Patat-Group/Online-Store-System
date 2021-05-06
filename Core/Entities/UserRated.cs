namespace Core.Entities
{
    public class UserRated
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public long OneStarCount { get; set; }
        public long TwoStarCount { get; set; }
        public long ThirdStarCount { get; set; }
        public long FourthStarCount { get; set; }
        public long FifthStarCount { get; set; }
    }
}