using System.Runtime.Intrinsics.X86;

namespace PacientikWebSite.Models.ReportModels
{
    public class UpdateReportModel
    {
        public string pacientname { get; set; }
        public float price { get; set; }
        public HashSet<int> lechids { get; set; } = new();
        public int period { get; set; }
    }
}
