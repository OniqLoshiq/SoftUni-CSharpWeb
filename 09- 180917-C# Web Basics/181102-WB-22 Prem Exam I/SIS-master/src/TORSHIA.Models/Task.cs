using System;
using System.Collections.Generic;

namespace TORSHIA.Models
{
    public class Task
    {
        public Task()
        {
            this.Sectors = new HashSet<TaskSector>();
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Title { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsReported { get; set; }

        public string Description { get; set; }

        public string Participants { get; set; }

        public virtual ICollection<TaskSector> Sectors { get; set; }
    }
}
