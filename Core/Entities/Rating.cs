using Core.Entities.Enums;

namespace Core.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public string UserSetRateId { get; set; }
        public User UserSetRate { get; set; }
        public string UserGetRateId { get; set; }
        public User UserGetRate { get; set; }
        public RatingStar Star { get; set; }
    }
}