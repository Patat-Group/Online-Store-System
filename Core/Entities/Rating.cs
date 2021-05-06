using Core.Entities.Enums;

namespace Core.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public string UserSourceRateId { get; set; }
        public User UserSourceRate { get; set; }
        public string UserDestinationRateId { get; set; }
        public User UserDestinationRate { get; set; }
        public RatingStar Star { get; set; }
    }
}