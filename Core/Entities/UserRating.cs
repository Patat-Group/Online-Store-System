namespace Core.Entities
{
    public class UserRating
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public long OneStar { get; set; }
        public long TwoStar { get; set; }
        public long ThreeStar { get; set; }
        public long FourStar { get; set; }
        public long FiveStar { get; set; }
    }
}