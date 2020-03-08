using bug_tracker.Helper;
using bug_tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace bug_tracker.Controllers
{
    public class RolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper helper = new UserRolesHelper();
        // GET: Assign User Roles
        public ActionResult AssignUserRole()
        {
            ViewBag.UserList = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.RoleList = new SelectList(db.Users, "Id", "FirstName");
            AssignUserRoleViewModel model = new AssignUserRoleViewModel();
            model.UserId = new SelectList(db.Users, "Id", "FirstName");
            model.RoleName = new SelectList(db.Roles, "Name", "Name");
            model.UserRoles = db.Users.ToList();
            return View(model);
        }

        // POST: Assign User Roles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUserRole(string UserId, string RoleName)
        {
            if (helper.AddUserToRole(UserId, RoleName))
            {
                return RedirectToAction("AssignUserRole", "Roles");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // GET: Remove User Roles
        public ActionResult RemoveUserRole()
        {
            //ViewBag.UserList = new SelectList(db.Users, "Id", "FirstName");
            //ViewBag.RoleList = new SelectList(db.Users, "Id", "FirstName");
            RemoveUserRoleViewModel model = new RemoveUserRoleViewModel();
            model.UserId = new SelectList(db.Users, "Id", "FirstName");
            model.RoleName = new SelectList(db.Roles, "Name", "Name");
            model.UserRoles = db.Users.ToList();
            return View(model);
        }
        // POST: Remove User Roles

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveUserRole(string UserId, string RoleName)
        {
            if (helper.RemoveUserFromRole(UserId, RoleName))
            {
                return RedirectToAction("RemoveUserRole", "Roles");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }
}

