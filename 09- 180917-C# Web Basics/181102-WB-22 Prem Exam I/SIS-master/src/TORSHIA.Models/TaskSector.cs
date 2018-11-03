using System.ComponentModel.DataAnnotations.Schema;

namespace TORSHIA.Models
{
    public class TaskSector
    {
        [ForeignKey(nameof(Task))]
        public string TaskId { get; set; }
        public virtual Task Task { get; set; }

        [ForeignKey(nameof(Sector))]
        public int SectorId { get; set; }
        public virtual Sector Sector { get; set; }
    }
}
