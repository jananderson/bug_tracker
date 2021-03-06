﻿using bug_tracker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace bug_tracker.Helper
{
    public class TicketHistoryHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
        TicketHistory history = new TicketHistory();
        public void GenerateHistory(Ticket oldTicket, Ticket newTicket, string UserId)
        {
            var user = db.Users.Find(UserId);

            if (oldTicket.Title != newTicket.Title)
            {
                history.TicketId = newTicket.Id;
                history.Property = "Title";
                history.OldValue = oldTicket.Title;
                history.NewValue = newTicket.Title;
                history.Changed = DateTimeOffset.Now;
                history.ChangedBy = user.Email;
                db.TicketHistories.Add(history);
                db.SaveChanges();
            }
            if (oldTicket.TicketPriority != newTicket.TicketPriority)
            {
                history.TicketId = newTicket.Id;
                history.Property = "Priority";
                history.OldValue = oldTicket.TicketPriority.Name;
                history.NewValue = newTicket.TicketPriority.Name;
                history.Changed = DateTimeOffset.Now;
                history.ChangedBy = user.Email;
                db.TicketHistories.Add(history);
                db.SaveChanges();
            }
            if (oldTicket.AssignedToUserId != null && oldTicket.AssignedToUserId != newTicket.AssignedToUserId)
            {

                history.TicketId = newTicket.Id;
                history.Property = "AssignedTo";
                history.OldValue = oldTicket.AssignedToUser.Email;
                history.NewValue = newTicket.AssignedToUser.Email;
                history.Changed = DateTimeOffset.Now;
                history.ChangedBy = user.Email;
                db.TicketHistories.Add(history);
                db.SaveChanges();
            
            }
            else
            {
                history.TicketId = newTicket.Id;
                history.Property = "AssignedTo";
                history.OldValue = "No assigned developer";
                history.NewValue = "No assigned developer";
                history.Changed = DateTimeOffset.Now;
                history.ChangedBy = user.Email;
                db.TicketHistories.Add(history);
                db.SaveChanges();
            }
            if (oldTicket.Description != newTicket.Description)
            {
                history.TicketId = newTicket.Id;
                history.Property = "Description";
                history.OldValue = oldTicket.Description;
                history.NewValue = newTicket.Description;
                history.Changed = DateTimeOffset.Now;
                history.ChangedBy = user.Email;
                db.TicketHistories.Add(history);
                db.SaveChanges();
            }
            if (oldTicket.TicketStatus != newTicket.TicketStatus)
            {
                history.TicketId = newTicket.Id;
                history.Property = "Status";
                history.OldValue = oldTicket.TicketStatus.Name;
                history.NewValue = newTicket.TicketStatus.Name;
                history.Changed = DateTimeOffset.Now;
                history.ChangedBy = user.Email;
                db.TicketHistories.Add(history);
                db.SaveChanges();
            }
            if (oldTicket.TicketType != newTicket.TicketType)
            {
                history.TicketId = newTicket.Id;
                history.Property = "Type";
                history.OldValue = oldTicket.TicketType.Name;
                history.NewValue = newTicket.TicketType.Name;
                history.Changed = DateTimeOffset.Now;
                history.ChangedBy = user.Email;
                db.TicketHistories.Add(history);
                db.SaveChanges();
            }
        }
    }
}