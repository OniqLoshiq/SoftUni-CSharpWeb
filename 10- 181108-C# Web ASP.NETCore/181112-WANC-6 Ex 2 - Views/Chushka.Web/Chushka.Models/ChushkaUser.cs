using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Chushka.Models
{
    public class ChushkaUser : IdentityUser
    {
        public string FullName { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();

    }
}
