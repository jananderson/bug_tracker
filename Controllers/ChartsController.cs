using bug_tracker.ChartModels;
using bug_tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bug_tracker.Controllers
{
    public class ChartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public JsonResult GetBarData(string property)
        {
            var dataObject = new ChartJsBarData();
            switch (property)
            {
                case "Priority":
                    foreach (var priority in db.Priorities.ToList())
                    {
                        dataObject.Labels.Add(priority.Name);
                        dataObject.Data.Add(db.Tickets.AsNoTracking().Count(t => t.TicketPriorityId == priority.Id));
                    }
                    break;
                case "Type":
                    foreach (var type in db.TicketTypes.ToList())
                    {
                        dataObject.Labels.Add(type.Name);
                        dataObject.Data.Add(db.Tickets.AsNoTracking().Count(t => t.TicketTypeId == type.Id));
                    }
                    break;
                case "Status":
                    foreach (var status in db.Statuses.ToList())
                    {
                        dataObject.Labels.Add(status.Name);
                        dataObject.Data.Add(db.Tickets.AsNoTracking().Count(t => t.TicketStatusId == status.Id));
                    }
                    break;
            }
            return Json(dataObject);
        }
    }
}
