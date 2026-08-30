using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.DTOs
{
    public class LechDTO
    {
        public int Id { get; set; }
        public string Spec {  get; set; }
        public string Name { get; set; }
    }

    public class CreateLechDTO
    {
        public int Spec { get; set; }
        public string Name { get; set; }
    }

    public class UpdateLechDTO
    {
        public int Spec { get; set; }
        public string Name { get; set; }
    }
}
