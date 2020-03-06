using bug_tracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace bug_tracker.Helper
{
    public static class NotificationManager
    { 
        public static void ManageTicketNotifications(Ticket oldTicket, Ticket newTicket)
        {
            ManageGeneralAssignmentNotification(oldTicket, newTicket);
            ManagePropertyChangeNotifications(oldTicket, newTicket);
        }
        private static void ManageGeneralAssignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            var assigned = oldTicket.AssignedToUserId == null && newTicket.AssignedToUserId != null;
            var unassign = oldTicket.AssignedToUserId != null && newTicket.AssignedToUserId == null;
            var reassign = newTicket.AssignedToUserId != null && newTicket.AssignedToUserId != oldTicket.AssignedToUserId;

            var newNotification = new TicketNotification();
            newNotification.TicketId = newTicket.Id;

            if (assigned)
            {
                newNotification.RecipientId = newTicket.AssignedToUserId;
                newNotification.Message = $"You have been assigned to Ticket Id {newTicket.Id}";
                GenerateNotification(newNotification);
            }
            else if (unassign)
            {
                newNotification.RecipientId = oldTicket.AssignedToUserId;
                newNotification.Message = $"You have been unassigned to Ticket Id {newTicket.Id}";
                GenerateNotification(newNotification);
            }
            else if (reassign)
            {
                newNotification.RecipientId = newTicket.AssignedToUserId;
                newNotification.Message = $"You have been assigned to Ticket Id {newTicket.Id}";
                GenerateNotification(newNotification);
            }
            else
            {
                newNotification.RecipientId = oldTicket.AssignedToUserId;
                newNotification.Message = $"You have been unassigned to Ticket Id {newTicket.Id}";
                GenerateNotification(newNotification);
            }
        }
        private static void ManagePropertyChangeNotifications(Ticket oldTicket, Ticket newTicket)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            if (oldTicket.Title != newTicket.Title)
            {
                //var newTicketTitle = db.Tickets.Titles.Find(newTicket.TicketTitleId).Name;
                //var oldTicketTitle = db.Tickets.Titles.Find(oldTicket.TicketTitleId).Name;
                GenerateNotification(new TicketNotification
                {
                    TicketId = newTicket.Id,
                    RecipientId = newTicket.AssignedToUserId,
                    //Message = $"The title has changed for ticket id {newTicket.Id} from {newTicketTitle} to {newTicketTitle}",
                    Message = $"The description has changed for ticket id {newTicket.Id} from {newTicket.Title} to {newTicket.Title}",
                });
            }
            if (oldTicket.Description != newTicket.Description)
            {
                //var newTicketDescription = db.Tickets.Description.Find(newTicket.Description).Name;
                //var oldTicketDescription = db.Description.Find(oldTicket.Description).Name;
                GenerateNotification(new TicketNotification
                {
                    TicketId = newTicket.Id,
                    RecipientId = newTicket.AssignedToUserId,
                    //Message = $"The title has changed for ticket id {newTicket.Id} from {newTicketDescription} to {newTicketDescription}",
                    Message = $"The description has changed for ticket id {newTicket.Id} from {oldTicket.Description} to {newTicket.Description}",
                });
            }
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                //var newTicketPriority = db.Priorities.Find(newTicket.TicketPriorityId).Name;
                //var oldTicketPriority = db.Priorities.Find(oldTicket.TicketPriorityId).Name;
                GenerateNotification(new TicketNotification
                {
                    TicketId = newTicket.Id,
                    RecipientId = newTicket.AssignedToUserId,
                    //Message = $"The priority has changed for ticket id {newTicket.Id} from {oldTicketPriority} to {newTicketPriority}",
                    Message = $"The priority has changed for ticket id {newTicket.Id} from {oldTicket.TicketPriority.Name} to {newTicket.TicketPriority.Name}",
                });
            }
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                //var newTicketStatus = db.Statuses.Find(newTicket.TicketStatusId).Name;
                //var oldTicketStatus = db.Statuses.Find(oldTicket.TicketStatusId).Name;
                GenerateNotification(new TicketNotification
                {
                    TicketId = newTicket.Id,
                    RecipientId = newTicket.AssignedToUserId,
                    //Message = $"The priority has changed for ticket id {newTicket.Id} from {oldTicketStatus} to {newTicketStatus}",
                    Message = $"The status has changed for ticket id {newTicket.Id} from {oldTicket.TicketStatus.Name} to {newTicket.TicketStatus.Name}",
                });
            }
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                //var newTicketType = db.TicketTypes.Find(newTicket.TicketTypeId).Name;
                //var oldTicketType = db.TicketTypes.Find(oldTicket.TicketTypeId).Name;
                GenerateNotification(new TicketNotification
                {
                    TicketId = newTicket.Id,
                    RecipientId = newTicket.AssignedToUserId,
                    // Message = $"The priority has changed for ticket id {newTicket.Id} from {oldTicketType} to {newTicketType}",
                    Message = $"The type has changed for ticket id {newTicket.Id} from {oldTicket.TicketType.Name} to {newTicket.TicketType.Name}",
                });
            }

        }
        public static void ManageAttachmentNotifications(TicketAttachment ticketAttachment, string assignedToUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var newNotification = new TicketNotification();
            var theTicket = db.Tickets.Where(t => t.Id == ticketAttachment.TicketId).FirstOrDefault();
            newNotification.TicketId = ticketAttachment.TicketId;
            newNotification.RecipientId = assignedToUserId;
            newNotification.Message = $"There is a new attachment for {theTicket.Title} TicketId{newNotification.TicketId}";
            GenerateNotification(newNotification);

        }

        public static void ManageCommentNotifications(TicketComment ticketComment, string assignedToUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var newNotification = new TicketNotification();
            var theTicket = db.Tickets.Where(t => t.Id == ticketComment.TicketId).FirstOrDefault();
            newNotification.TicketId = ticketComment.TicketId;
            newNotification.RecipientId = assignedToUserId;
            newNotification.Message = $"There is a new comment for {theTicket.Title} TicketId{newNotification.TicketId}";
            GenerateNotification(newNotification);

        }
        private static void GenerateNotification(TicketNotification notification)
        {
            var db = new ApplicationDbContext();
            var newNotification = new TicketNotification
            {
                Created = DateTime.Now,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                RecipientId = notification.RecipientId,
                Message = notification.Message,
                HasBeenRead = false,
                TicketId = notification.TicketId,
            };
            db.Notifications.Add(newNotification);
            db.SaveChanges();
            //newNotification.SenderId = HttpContext.Current.User.Identity.GetUserId();
        }
        //if (oldTicket.AssignedToUserId != newTicket.AssignedToUserId)
        //var assigned = oldTicket.AssignedToUserId == null && newTicket.AssignedToUserId != null;
        //var unassign = oldTicket.AssignedToUserId != null && newTicket.AssignedToUserId == null;
        //var reassign = newTicket.AssignedToUserId != null && newTicket.AssignedToUserId != oldTicket.AssignedToUserId;
    }
}