using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bug_tracker.Models
{
    public class TicketDetailsViewModel
    {
    public ICollection<TicketHistory> Histories { get; set; }
    }
}