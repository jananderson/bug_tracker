using bug_tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bug_tracker.Helper
{
    public class TicketHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper rolesHelper = new UserRolesHelper();

        public List<Ticket> GetUserTickets(string userId)
        {
            var listUserOfRoles = rolesHelper.ListUserRoles(userId).ToList();
            var userRole = listUserOfRoles.FirstOrDefault();
            var userTickets = new List<Ticket>();
            switch (userRole)
            {
                case "Admin":
                    userTickets = db.Tickets.ToList();
                    break;
                case "ProjectManager":
                    userTickets = db.Users.Find(userId).Projects.SelectMany(p => p.Tickets).ToList();
                    break;
                case "Developer":
                    userTickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
                    break;
                case "Submitter":
                    userTickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                    break;
            }
            return (userTickets);
        }
    }
}