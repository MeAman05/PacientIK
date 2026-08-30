using PacientIK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.DTOs
{
    public static class SpecDTOExtensions
    {
        public static SpecDTO ToSpecDTO(this Spec sp)
        {
            return new SpecDTO
            {
                Id = sp.Id,
                Name = sp.Name,
            };
        }

        public static Spec ToSpecAddEntity(this CreateSpecDTO dTO)
        {
            return new Spec
            {
                Name = dTO.Name,
            };
        }

        public static Spec ToSpecUpdateEntity(this UpdateSpecDTO dTO)
        {
            return new Spec
            {
                Name = dTO.Name
            };
        }
    }

}
