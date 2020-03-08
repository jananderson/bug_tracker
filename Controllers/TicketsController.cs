using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using bug_tracker.Helper;
using bug_tracker.Models;
using Microsoft.AspNet.Identity;

namespace bug_tracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private TicketHistoryHelper ticketHistoryHelper = new TicketHistoryHelper();
        private TicketDetailsViewModel ticketDetailsViewModel = new TicketDetailsViewModel();
        private ProjectHelper projectHelper = new ProjectHelper();

        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());
        }

        // GET: Tickets with authorization
        public ActionResult UserTickets()
        {
            var userRolesViewModel = new UserRolesViewModel();
            var userId = User.Identity.GetUserId();
            var userProjects = projectHelper.ListUserProjects(userId);

            if (User.IsInRole("ProjectManager"))
            {
                List<Ticket> ticketList = db.Tickets.Where(t => t.Project.ProjectManagerId == userId).ToList();
                userRolesViewModel.ProjectManTickets = ticketList;
            }
            if (User.IsInRole("Developer"))
            {
                List<Ticket> assignedTicketList = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
                List<Ticket> projectTicketList = db.Tickets.Where(t => t.Project.Users.Any(u => u.Id == userId)).ToList();
                userRolesViewModel.DeveloperTickets = assignedTicketList;
                userRolesViewModel.DeveloperProj = projectTicketList;
            }
            if (User.IsInRole("Submitter"))
            {
                List<Ticket> submitterTicketList = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                userRolesViewModel.SubmitterTickets = submitterTicketList;
            }
            return View(userRolesViewModel);
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            TicketDetailsViewModel TicketDetailsVM = new TicketDetailsViewModel();
            TicketDetailsVM.Histories = db.TicketHistories.Where(tH => tH.TicketId == id).ToList();
            return View(ticket);
        }


        //public ActionResult Details(int? id)

        //{
        //    UserRolesHelper userRolesHelper = new UserRolesHelper();
        //    UserRolesViewModel userRoles = new UserRoles();
        //    var userId = User.Identity.GetUserId();
        //    TicketHelper ticketHelper = new TicketHelper();
        //    Ticket ticket = db.Tickets.Find(id);

        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    if (ticket == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var userTickets = ticketHelper.GetUserTickets(userId);

        //    if (userTickets.Any(t => t.Id == ticket.Id) || User.IsInRole("Admin"))
        //    {
        //        TicketDetailsViewModel TicketDetailsVM = new TicketDetailsViewModel();
        //        TicketDetailsVM.Histories = db.TicketHistories.Where(tH => tH.TicketId == id).ToList();
        //        return View(ticket);
        //    }
        //    return RedirectToAction("Index", "Home");

        //}



        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create()
        {
            if (!User.IsInRole("Submitter"))
            {
                RedirectToAction("Index", "Home");
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.Priorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.Statuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,AssignedToUserId,Published,Attachments")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Created = DateTimeOffset.Now;
                ticket.OwnerUserId = User.Identity.GetUserId();
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.Priorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.Statuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssignedToUserId = new SelectList(roleHelper.UsersInRole("Developer"), "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.Priorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.Statuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId,Published,Attachments")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                ticket.Updated = DateTimeOffset.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                var userId = User.Identity.GetUserId();
                ticketHistoryHelper.GenerateHistory(oldTicket, newTicket, userId);
                NotificationManager.ManageTicketNotifications(oldTicket, newTicket);
                return RedirectToAction("Index");
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.Priorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.Statuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // GET: Tickets/AssignTicket
        public ActionResult AssignTicket(int? id)
        {
            UserRolesHelper helper = new UserRolesHelper();
            var ticket = db.Tickets.Find(id);
            var users = helper.UsersInRole("Developer").ToList();
            ViewBag.AssignedToUserId = new SelectList(users, "Id", "FirstName", ticket.AssignedToUserId);
            return View(ticket);
        }
        // POST: Tickets/AssignTicket
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignTicket(Ticket model)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var ticket = db.Tickets.Find(model.Id);
            ticket.AssignedToUserId = model.AssignedToUserId;

            db.SaveChanges();

            var callbackUrl = Url.Action("Details", "Tickets", new { id = ticket.Id },
                protocol: Request.Url.Scheme);
            try
            {
                EmailService ems = new EmailService();
                IdentityMessage msg = new IdentityMessage();
                ApplicationUser user = db.Users.Find(model.AssignedToUserId);

                msg.Body = "You have been assigned a new Ticket." + Environment.NewLine + "Please click the following link for details" + "<a href=\"" + callbackUrl + "\">New Ticket</a>";
                msg.Destination = user.Email;
                msg.Subject = "New ticket assignment";

                await ems.SendMailAsync(msg);

            }
            catch (Exception ex)
            {
                await Task.FromResult(0);
            }
            return RedirectToAction("Index");
        }
    }
}