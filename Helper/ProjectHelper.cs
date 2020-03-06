using bug_tracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace bug_tracker.Helper
{
    public class ProjectHelper
    {

        ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var flag = project.Users.Any(u => u.Id == userId);
            return (flag);
        }

        public ICollection<Project> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);

            var projects = user.Projects.ToList();
            return (projects);
        }

        public bool AddProjectManager(string userId, int projectId)
        {
            Project proj = db.Projects.Find(projectId);
            var newPMID = db.Users.Find(userId);

            proj.ProjectManagerId = newPMID.Id;
            db.SaveChanges();
            return true;
        }

        public bool RemoveProjectManager(string userId, int projectId)
        {
            Project proj = db.Projects.Find(projectId);
            var newPMID = db.Users.Find(userId);

            proj.ProjectManagerId = null;
            db.SaveChanges();
            return true;
        }

        public bool AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var newUser = db.Users.Find(userId);

                proj.Users.Add(newUser);
                db.SaveChanges();
                return true;
            }
            else { return false; }
        }
        public void RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var delUser = db.Users.Find(userId);

                proj.Users.Remove(delUser);
                db.Entry(proj).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ICollection<ApplicationUser> UsersOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }

        public ICollection<ApplicationUser> ProjectUsersInRole(int projectId, string roleName)
        {
            UserRolesHelper rolesHelper = new UserRolesHelper();
            var project = db.Projects.Find(projectId);
            var usersList = new List<ApplicationUser>();

            foreach (var user in project.Users)
            {
                if (rolesHelper.IsUserInRole(user.Id, roleName))
                {
                    usersList.Add(user);
                }
            }
            return usersList;
        }
    }
}