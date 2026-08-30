namespace PacientikWebSite.Models.ReportModels
{
    public class CreateReportModel
    {
        public string pacientname { get; set; }
        public float price { get; set; }
        public List<int> lechids { get; set; } = new List<int>();
        public int period { get; set; }
    }
}
