using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models
{
    public class UserEvent
    {
        public int EventID { get; set; }
        public Event Event { get; set; }

        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
    }
}
