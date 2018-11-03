using System.Collections.Generic;

namespace TORSHIA.Models
{
    public class Sector
    {
        public Sector()
        {
            this.Tasks = new HashSet<TaskSector>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<TaskSector> Tasks { get; set; }
    }
}
