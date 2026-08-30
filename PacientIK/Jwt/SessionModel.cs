namespace PacientIK.Jwt
{
    public class SessionModel
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public string Spec {  get; set; }
        public string Role { get; set; }
    }
}
