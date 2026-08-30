using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PacientIK.Domain.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public string PacientName { get; set; }
        public Guid SenderId { get; set; }
        public float Price { get; set; }
        public int Period {  get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string Created { get; set; } = DateTime.UtcNow.ToString("dd/MM/yyyy");

        [ForeignKey(nameof(SenderId))]
        public virtual User Sender { get; set; }

        public ICollection<Lech> Leches { get; set; } = new List<Lech>();
    }
}
