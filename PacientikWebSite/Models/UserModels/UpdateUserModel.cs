using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace PacientikWebSite.Models.UserModels
{
    public class UpdateUserModel
    {
        public string Name { get; set; }
        public string Surname { get; set; } 
        public string Lastname { get; set; }
        public int Spec { get; set; }
        public int Age { get; set; }
        public string Role { get; set; }
        public IBrowserFile? Photo { get; set; }
    }
}
