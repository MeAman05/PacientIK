using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace PacientikWebSite.Models.UserModels
{
    public class CreateUserModel
    {
        public string name {  get; set; }
        public string surname { get; set; }
        public string lastname { get; set; }
        public string code { get; set; }
        public string pwd { get; set; }
        public int spec { get; set; }
        public int age { get; set; }
        public string role { get; set; }
        public IBrowserFile photo { get; set; }  
    }
}
