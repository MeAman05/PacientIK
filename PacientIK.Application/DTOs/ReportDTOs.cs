using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.DTOs
{
    public sealed record ReportDTO(
        int id,
        string pacientname,
        string sender,
        float price,
        List<string> leches,
        List<int> lechids,
        int period
        );

    public sealed record CreateReportDTO(
        string pacientname,
        float price,
        List<int> lechids,
        int period
        );

    public sealed record UpdateReportDTO(
         string pacientname,
        float price,
        List<int> lechids,
        int period
        );
}
