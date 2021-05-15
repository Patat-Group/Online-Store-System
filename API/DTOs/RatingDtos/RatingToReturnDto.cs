using Core.Entities.Enums;

namespace API.DTOs.RatingDtos
{
    public class RatingToReturnDto
    {
        public int Id { get; set; }
        public string UserSourceRateId { get; set; }
        public string UserDestinationRateId { get; set; }
        private RatingStar _star;
        public RatingStar Star
        {
            get => _star;
            set => _star = value+1;
        }
    }
}