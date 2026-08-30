using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.DTOs
{
    public class SpecDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateSpecDTO
    {
        public string Name { get; set; }
    }

    public class UpdateSpecDTO
    {
        public string Name { get; set; }
    }
}
