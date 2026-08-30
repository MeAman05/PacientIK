using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PacientIK.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int SpecId { get; set; } = 1;
        public string? PhotoUrl { get; set; }
        public string? PhotoName { get; set; }
        public string Core {  get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "User";

        [ForeignKey(nameof(SpecId))]
        public virtual Spec? Spec { get; set; }
    }
}
