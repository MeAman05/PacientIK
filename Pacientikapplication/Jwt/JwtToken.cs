using System;
using System.Collections.Generic;
using System.Text;

namespace Pacientikapplication.Jwt
{
    public class JwtToken
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public int Spec { get; set; }
        public string Role { get; set; }
        public DateTime Date { get; set; }
    }
}
