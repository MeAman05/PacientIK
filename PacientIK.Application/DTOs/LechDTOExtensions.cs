using PacientIK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.DTOs
{
    public static class LechDTOExtensions
    {
        public static LechDTO ToLechDTO(this Lech lech)
        {
            return new LechDTO
            {
                Id = lech.Id,
                Spec = lech.Spec.Name,
                Name = lech.Name,
            };
        }

        public static Lech ToAddLechEntity(this CreateLechDTO dTO)
        {
            return new Lech
            {
                SpecId = dTO.Spec,
                Name = dTO.Name,
            };
        }

        public static Lech ToUpdateLechEntity(this UpdateLechDTO dTO)
        {
            return new Lech
            {
                SpecId = dTO.Spec,
                Name = dTO.Name
            };
        }
    }

}
