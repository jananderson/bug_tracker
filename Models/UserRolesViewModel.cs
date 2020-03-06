using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bug_tracker.Models
{
    public class UserRolesViewModel
    {
        public List<Ticket> ProjectManTickets { get; set; }
        public List<Ticket> DeveloperProj { get; set; }
        public List<Ticket> DeveloperTickets { get; set; }
        public List<Ticket> SubmitterTickets { get; set; }
    }
}