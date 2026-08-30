namespace PacientikWebSite.Models
{
    public class TokenModel
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public int Spec { get; set; }
        public string Role { get; set; }
    }
}
