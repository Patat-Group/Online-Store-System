namespace Core.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public string UserSetReportId { get; set; }
        public User UserSetReport { get; set; }
        public string UserGetReportId { get; set; }
        public User UserGetReport { get; set; }
    }
}