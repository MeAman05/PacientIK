namespace PacientikWebSite.Models
{
    public class UserModel
    {
        public Guid userid { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string lastname { get; set; }
        public int age { get; set; }
        public string photourl { get; set; }
        public string spec {  get; set; }
        public string role { get; set; }
    }
}
