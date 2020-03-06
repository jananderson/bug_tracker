using bug_tracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace bug_tracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            DashboardViewModel model = new DashboardViewModel();

            var userId = User.Identity.GetUserId();

            List<Ticket> listOfTickets = db.Tickets.ToList();
            model.Tickets = listOfTickets;

            List<Project> listOfProjects = db.Projects.ToList();
            model.Projects = listOfProjects;


            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            EmailModel model = new EmailModel();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
             if(ModelState.IsValid)
                {
                try
                {
                    var body = "<p>Email From: <bold>{0}</bold>" +
                        "({1})</p><p>Message:</p><p>{2}</p>";
                    model.Body = "This is a message from your bug tracker site. The name and" +
                        "the email of the contacting person is above.";

                    var svc = new EmailService();
                    var msg = new IdentityMessage()
                    {
                        Subject = "Contact from bug tracker site",
                        Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
                        Destination = "jan@directstepdevelopment.com"
                    };

                    await svc.SendAsync(msg);
                    return View();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                    return View(model);
                }
            }
             else { return View(model);  }
        }
    }
}