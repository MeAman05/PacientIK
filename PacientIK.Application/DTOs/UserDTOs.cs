using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PacientIK.Application.DTOs
{
    public sealed record UserDTO(
        Guid userid,
        string name,
        string surname,
        string lastname,
        int age,
        string photourl,
        string spec,
        string role
        );

    public sealed record CreateUserDTO(
        string name,
        string surname,
        string lastname,
        string code,
        string pwd,
        int spec,
        int age,
        string role,
        IFormFile photo
        );

    public class UpdateUserDTO
    {
        public string? Name { get; set; }
        public string? Surname { get; set; } 
        public string? Lastname { get; set; }
        public int Spec { get; set; }
        public int Age { get; set; }
        public string? Role { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
