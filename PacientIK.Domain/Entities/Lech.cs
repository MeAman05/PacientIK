using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PacientIK.Domain.Entities
{
    public class Lech
    {
        public int Id { get; set; }
        public int SpecId { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(SpecId))]
        public virtual Spec Spec { get; set; }
        public ICollection<Report>? Reports { get; set; }
    }
}
