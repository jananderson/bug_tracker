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
    //[Authorize]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper helper = new UserRolesHelper();
        private ProjectHelper projHelper = new ProjectHelper();

        // GET: Projects/AssignProject
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult ManageProjectUsers(int? projectId)
        {
            if (!User.IsInRole("Admin, ProjectManager"))
            {
                RedirectToAction("Index", "Home");
            }
            var project = db.Projects.FirstOrDefault(p => p.Id == projectId);

            var managers = helper.UsersInRole("ProjectManager");
            ViewBag.ManagerId = new SelectList(managers, "Id", "FirstName", project.ProjectManagerId);

            var developers = helper.UsersInRole("Developer");
            ViewBag.DeveloperIds = new MultiSelectList(developers, "Id", "FirstName");

            var submitters = helper.UsersInRole("Submitter");
            ViewBag.SubmitterIds = new MultiSelectList(submitters, "Id", "FirstName");

            //var assignedManagers = helper.UsersInRole("ProjectManager");
            //ViewBag.ManagerId = new SelectList(managers, "Id", "FirstName", project.ProjectManagerId);

            var assignedDevelopers = projHelper.ProjectUsersInRole(projectId.Value, "Developer");
            ViewBag.AssignedDeveloperIds = new MultiSelectList(assignedDevelopers, "Id", "FirstName");

            var assignedSubmitters = projHelper.ProjectUsersInRole(projectId.Value, "Submitter");
            ViewBag.AssignedSubmitterIds = new MultiSelectList(assignedSubmitters, "Id", "FirstName");

            return View(project);
        }

        // POST: Projects/AssignProject
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult ManageProjectUsers(string managerId, List<string> developerIds, List<string> submitterIds, int projectId)
        {
            //foreach (var projectId in projectId)
            //{
                if (!string.IsNullOrEmpty(managerId))
                {
                    //if (!projHelper.IsUserOnProject(managerId, projectId.Value))
                        projHelper.AddProjectManager(managerId, projectId);
                }

                if (developerIds != null)
                {
                    foreach (var devId in developerIds)
                    {
                        if (!projHelper.IsUserOnProject(devId, projectId))
                            projHelper.AddUserToProject(devId, projectId);
                    }
                }

                if (submitterIds != null)
                {
                    foreach (var submitterId in submitterIds)
                    {
                        if (!projHelper.IsUserOnProject(submitterId, projectId))
                            projHelper.AddUserToProject(submitterId, projectId);
                    }
                }
            //}

            return Redirect(Request.UrlReferrer.ToString());
        }

        // POST: Projects/AssignProject
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult RemoveProjectUsers(string managerId, List<string> AssignedDeveloperIds, List<string> AssignedSubmitterIds, int projectId)
        {
            //foreach (var projectId in projectId)
            //{
            //if (!string.IsNullOrEmpty(managerId))
            //{
            //    if (!projHelper.IsUserOnProject(managerId, projectId.Value))
            //        projHelper.RemoveProjectManager(managerId, projectId);
            //}

            if (AssignedDeveloperIds != null)
            {
                foreach (var devId in AssignedDeveloperIds)
                {
                    if (projHelper.IsUserOnProject(devId, projectId))
                    {
                        projHelper.RemoveUserFromProject(devId, projectId);
                    }
                }
            }

            if (AssignedSubmitterIds != null)
            {
                foreach (var submitterId in AssignedSubmitterIds)
                {
                    if (projHelper.IsUserOnProject(submitterId, projectId))
                    {
                        projHelper.RemoveUserFromProject(submitterId, projectId);
                    }
                        
                }
            }
            //}

            return Redirect(Request.UrlReferrer.ToString());
        }

        // GET: Projects with authorization
        public ActionResult UserProjects() //Based off of user tickets in the ticket controller. UserProjectsViewModel is based off UserRolesViewModel
        {
            var userProjectsViewModel = new UserProjectsViewModel();
            var userId = User.Identity.GetUserId();

            if (User.IsInRole("Admin"))
            {
                var projects = db.Projects.Where(p => p.Users.Any(u => u.Id == userId)).ToList();
                var model = new List<ProjectIndexDetails>();

                foreach (var project in projects)
                {
                    ProjectIndexDetails newProject = new ProjectIndexDetails();


                    newProject.Project = project;
                    if (project.ProjectManagerId != null)
                    {
                        string firstName = db.Users.FirstOrDefault(u => u.Id == project.ProjectManagerId).FirstName;
                        string lastName = db.Users.FirstOrDefault(u => u.Id == project.ProjectManagerId).LastName;
                        newProject.ProjectManagerName = $"{firstName} {lastName}";
                    }
                    else
                    {
                        newProject.ProjectManagerName = "Unassigned";
                    }


                    model.Add(newProject);
                }


                userProjectsViewModel.AdminProjs = model;
            }

            if (User.IsInRole("ProjectManager"))
            {
                var projects = db.Projects.Where(p => p.ProjectManagerId == userId).ToList();
                var model = new List<ProjectIndexDetails>();

                foreach (var project in projects)
                {
                    ProjectIndexDetails newProject = new ProjectIndexDetails();


                    newProject.Project = project;
                    if (project.ProjectManagerId != null)
                    {
                        string firstName = db.Users.FirstOrDefault(u => u.Id == project.ProjectManagerId).FirstName;
                        string lastName = db.Users.FirstOrDefault(u => u.Id == project.ProjectManagerId).LastName;
                        newProject.ProjectManagerName = $"{firstName} {lastName}";
                    }
                    else
                    {
                        newProject.ProjectManagerName = "Unassigned";
                    }


                    model.Add(newProject);
                }

                userProjectsViewModel.ProjectManProjs = model;

            }

            if (User.IsInRole("Developer"))
            {
                var projects = db.Projects.Where(p => p.Users.Any(u => u.Id == userId)).ToList();
                var model = new List<ProjectIndexDetails>();

                foreach (var project in projects)
                {
                    ProjectIndexDetails newProject = new ProjectIndexDetails();


                    newProject.Project = project;
                    if (project.ProjectManagerId != null)
                    {
                        string firstName = db.Users.FirstOrDefault(u => u.Id == project.ProjectManagerId).FirstName;
                        string lastName = db.Users.FirstOrDefault(u => u.Id == project.ProjectManagerId).LastName;
                        newProject.ProjectManagerName = $"{firstName} {lastName}";
                    }
                    else
                    {
                        newProject.ProjectManagerName = "Unassigned";
                    }


                    model.Add(newProject);
                }

                userProjectsViewModel.DeveloperProjs = model;

            }

            if (User.IsInRole("Submitter"))
            {
                var projects = db.Projects.Where(p => p.Users.Any(u => u.Id == userId)).ToList();
                var model = new List<ProjectIndexDetails>();

                foreach (var project in projects)
                {
                    ProjectIndexDetails newProject = new ProjectIndexDetails();


                    newProject.Project = project;
                    if (project.ProjectManagerId != null)
                    {
                        string firstName = db.Users.FirstOrDefault(u => u.Id == project.ProjectManagerId).FirstName;
                        string lastName = db.Users.FirstOrDefault(u => u.Id == project.ProjectManagerId).LastName;
                        newProject.ProjectManagerName = $"{firstName} {lastName}";
                    }
                    else
                    {
                        newProject.ProjectManagerName = "Unassigned";
                    }


                    model.Add(newProject);
                }

                userProjectsViewModel.SubmitterProjs = model;

            }

            return View(userProjectsViewModel);
        }

        // GET: Projects
        public ActionResult Index()
        {

            var model = new List<ProjectIndexDetails>();
            var projects = db.Projects.ToList();

            foreach (var project in projects)
            {
                ProjectIndexDetails newProject = new ProjectIndexDetails();


                newProject.Project = project;
                if (project.ProjectManagerId != null)
                {
                    string firstName = db.Users.FirstOrDefault(u => u.Id == project.ProjectManagerId).FirstName;
                    string lastName = db.Users.FirstOrDefault(u => u.Id == project.ProjectManagerId).LastName;
                    newProject.ProjectManagerName = $"{firstName} {lastName}";
                }
                else
                {
                    newProject.ProjectManagerName = "Unassigned";
                }


                model.Add(newProject);
            }


            return View(model);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Create()
        {
            if (!User.IsInRole("Admin, ProjectManager"))
            {
                RedirectToAction("Index", "Home");
            }
            ProjectCreateViewModel projectCreateViewModel = new ProjectCreateViewModel();

            var PMList = helper.UsersInRole("ProjectManager");
            SelectList projectManagers = new SelectList(PMList, "Id", "FirstName");

            projectCreateViewModel.ProjectManagerId = projectManagers;
            return View(projectCreateViewModel);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ProjectManagerId")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Edit(int? id)
        {
            if (!User.IsInRole("Admin, ProjectManager"))
            {
                RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            //ProjectCreateViewModel projectCreateViewModel = new ProjectCreateViewModel();

            var PMList = helper.UsersInRole("ProjectManager");
            ViewBag.ProjectManagerId = new SelectList(PMList, "Id", "FirstName", project.ProjectManagerId);
            //ViewBag.AssignedToUserId = new SelectList(helper.UsersInRole("Developer"), "Id", "FirstName", project.Users);

            //projectCreateViewModel.ProjectManagerId = projectManagers;
            return View(project/*projectCreateViewModel*/);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Project project, string projectManagerId)
        {
            if (ModelState.IsValid)
            {
                //project.Users.Add()
                project.ProjectManagerId = projectManagerId;
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
        // GET: Projects/AssignProject
        public ActionResult AssignProject(int? id)
        {
            UserRolesHelper helper = new UserRolesHelper();
            var project = db.Projects.Find(id);
            var users = helper.UsersInRole("ProjectManager").ToList();
            ViewBag.ProjectManagerId = new SelectList(users, "Id", "Email", project.ProjectManagerId);
            return View(project);
        }
        // POST: Projects/AssignProject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignProject(Project model)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var project = db.Projects.Find(model.Id);
            project.ProjectManagerId = model.ProjectManagerId;

            db.SaveChanges();

            var callbackUrl = Url.Action("Details", "Projects", new { id = project.Id },
                protocol: Request.Url.Scheme);
            try
            {
                EmailService ems = new EmailService();
                IdentityMessage msg = new IdentityMessage();
                ApplicationUser user = db.Users.Find(model.ProjectManagerId);

                msg.Body = "You have been assigned a new project." + Environment.NewLine + "Please click the following link for details" + "<a href=\"" + callbackUrl + "\">New Project</a>";
                msg.Destination = user.Email;
                msg.Subject = "New project assignment";

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
