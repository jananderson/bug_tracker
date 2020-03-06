using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace bug_tracker.Models
{
    public class Project
    {
        public Project()
        {
            Users = new HashSet<ApplicationUser>();
            Tickets = new HashSet<Ticket>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Project Manager")]
        public string ProjectManagerId { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}