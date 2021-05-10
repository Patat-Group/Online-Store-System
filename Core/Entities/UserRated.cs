using System;

namespace Core.Entities
{
    public class UserRated
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public long OneStarCount { get; set; }
        public long TwoStarCount { get; set; }
        public long ThreeStarCount { get; set; }
        public long FourStarCount { get; set; }
        public long FiveStarCount { get; set; }
        public  string ReportString { get; set; }
        public DateTime ReportDate { get; set; }
    }
}