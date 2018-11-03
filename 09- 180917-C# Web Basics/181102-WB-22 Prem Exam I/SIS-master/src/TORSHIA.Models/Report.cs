using System;
using System.ComponentModel.DataAnnotations.Schema;
using TORSHIA.Models.Enums;

namespace TORSHIA.Models
{
    public class Report
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public Status Status { get; set; }

        public DateTime ReportedOn { get; set; }

        [ForeignKey(nameof(Task))]
        public string TaskId { get; set; }
        public virtual Task Task { get; set; }

        [ForeignKey(nameof(Reporter))]
        public string ReporterId { get; set; }
        public virtual User Reporter { get; set; }
    }
}
