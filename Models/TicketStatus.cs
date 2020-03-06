using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bug_tracker.Models
{
    public class TicketStatus
    {
        public TicketStatus()
        {
            Tickets = new HashSet<Ticket>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}