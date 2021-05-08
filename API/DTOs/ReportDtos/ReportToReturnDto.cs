using System;

namespace API.DTOs.ReportDtos
{
    public class ReportToReturnDto
    {
        public int Id { get; set; }
        public string UserSourceReportId { get; set; }
        public string UserDestinationReportId { get; set; }
        public  string ReportString { get; set; }
        public DateTime ReportDate { get; set; }
    }
}