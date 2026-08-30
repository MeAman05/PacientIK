namespace PacientikWebSite.Models.ReportModels
{
    public class ReportModel
    {
        public int id { get; set; }
        public string pacientname { get; set; }
        public string sender { get; set; }
        public float price { get; set; }
        public List<string> leches { get; set; } = new List<string>();
        public List<int> lechids { get; set; } = new List<int>();
        public int period { get; set; }
    }
}
