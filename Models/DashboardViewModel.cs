using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bug_tracker.Models
{
    public class DashboardViewModel
    {
        public List<Project> Projects { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}