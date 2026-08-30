using PacientIK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PacientIK.Application.DTOs
{
    public static class ReportDTOExtension
    {
        public static ReportDTO ToReportDTO(this Report report)
        {
            return new ReportDTO(
                report.Id,
                report.PacientName,
                report.Sender.Surname + " " + report.Sender.Name + " " + report.Sender.LastName,
                report.Price,
                report.Leches.Select(l => l.Name).ToList(),
                report.Leches.Select(l => l.Id).ToList(),
                report.Period
                );
        }

        public static Report ToAddReportEntity(this CreateReportDTO dTO, Guid uid)
        {
            return new Report
            {
                PacientName = dTO.pacientname,
                SenderId = uid,
                Price = dTO.price,
                Leches = new List<Lech>(),
                Period = dTO.period,
            };
        }

        public static Report ToUpdateReportEntity(this UpdateReportDTO dTO)
        {
            return new Report
            {
                PacientName = dTO.pacientname,
                Price = dTO.price,
                Leches = new List<Lech>(),
                Period = dTO.period,
            };
        }
    }
}
