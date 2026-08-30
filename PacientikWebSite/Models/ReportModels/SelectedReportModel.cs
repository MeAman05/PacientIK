namespace PacientikWebSite.Models.ReportModels
{
    public class SelectedReportModel
    {
        public int id { get; set; }
        public string pacientname { get; set; }
        public string sender { get; set; }
        public float price { get; set; }
        public List<string> lech { get; set; } = new();
        public int period { get; set; }
    }
}
