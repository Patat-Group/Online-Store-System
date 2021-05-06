namespace Core.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public string UserSourceReportId { get; set; }
        public User UserSourceReport { get; set; }
        public string UserDestinationReportId { get; set; }
        public User UserDestinationReport { get; set; }
    }
}