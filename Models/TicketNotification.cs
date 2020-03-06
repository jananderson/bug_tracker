using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bug_tracker.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public DateTime Created { get; set; }
        public string Message { get; set; }
        public bool HasBeenRead { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Recipient { get; set; }

    }
}