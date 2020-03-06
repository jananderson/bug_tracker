namespace bug_tracker.Migrations
{
    using bug_tracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<bug_tracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);
            var role = new IdentityRole();

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                role = new IdentityRole { Name = "Admin" };
                manager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                role = new IdentityRole { Name = "Developer" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                role = new IdentityRole { Name = "Submitter" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                role = new IdentityRole { Name = "ProjectManager" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Tester"))
            {
                role = new IdentityRole { Name = "Tester" };
                manager.Create(role);
            }

            //context.Roles.AddOrUpdate(n => n.Name == "Tester");

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var demoPassword = WebConfigurationManager.AppSettings["DemoPassword"];
            if (!context.Users.Any(u => u.Email == "jchristiananderson@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "jchristiananderson@gmail.com",
                    Email = "jchristiananderson@gmail.com",
                    FirstName = "Jan",
                    LastName = "Anderson",
                    //DisplayName = "JCA"
                };

                userManager.Create(user, "Abc&123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Admin",
                        "ProjectManager",
                        "Developer",
                        "Submitter",
                        "Tester"
                    });

            }
            if (!context.Users.Any(u => u.Email == "admin@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FirstName = "Administrator",
                    LastName = "Role",
                    //DisplayName = "ADMIN"
                };

                userManager.Create(user, "Abc&123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Admin"
                    });

            }
            if (!context.Users.Any(u => u.Email == "demoadmin@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "demoadmin@gmail.com",
                    Email = "demoadmin@gmail.com",
                    FirstName = "DemoAdministrator",
                    LastName = "DemoRole",
                    //DisplayName = "DEMOADMIN"
                };

                userManager.Create(user, demoPassword);

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Admin"
                    });

            }
            if (!context.Users.Any(u => u.Email == "manager@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "manager@gmail.com",
                    Email = "manager@gmail.com",
                    FirstName = "Manager",
                    LastName = "Role",
                    //DisplayName = "MANGR"
                };

                userManager.Create(user, "Abc&123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "ProjectManager"
                    });
            }
            if (!context.Users.Any(u => u.Email == "demomanager@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "demomanager@gmail.com",
                    Email = "demomanager@gmail.com",
                    FirstName = "DemoManager",
                    LastName = "DemoRole",
                    //DisplayName = "DEMOMANGR"
                };

                userManager.Create(user, demoPassword);

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "ProjectManager"
                    });

            }
            if (!context.Users.Any(u => u.Email == "developer@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "developer@gmail.com",
                    Email = "developer@gmail.com",
                    FirstName = "Developer",
                    LastName = "Role",
                    //DisplayName = "DEVPR"
                };

                userManager.Create(user, "Abc&123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Developer"
                    });
            }
            if (!context.Users.Any(u => u.Email == "demodeveloper@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "demodeveloper@gmail.com",
                    Email = "demodeveloper@gmail.com",
                    FirstName = "DemoDeveloper",
                    LastName = "DemoRole",
                    //DisplayName = "DEMODEVPR"
                };

                userManager.Create(user, demoPassword);

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Developer"
                    });
            }
            if (!context.Users.Any(u => u.Email == "submitter@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "submitter@gmail.com",
                    Email = "submitter@gmail.com",
                    FirstName = "Submitter",
                    LastName = "Role",
                    //DisplayName = "SUBMT"
                };

                userManager.Create(user, "Abc&123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Submitter"
                    });
            }
            if (!context.Users.Any(u => u.Email == "demosubmitter@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "demosubmitter@gmail.com",
                    Email = "demosubmitter@gmail.com",
                    FirstName = "DemoSubmitter",
                    LastName = "DemoRole",
                    //DisplayName = "SUBMT"
                };

                userManager.Create(user, demoPassword);

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "Submitter"
                    });
            }
            if (!context.Users.Any(u => u.Email == "testuser1@gmail.com"))
            {
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "testuser1@gmail.com",
                        Email = "testuser1@gmail.com",
                        FirstName = "Test1",
                        LastName = "User1",
                        //DisplayName = "ImaTest1"
                    }, "Abc&123!");
                }
            }

            if (!context.Priorities.Any(u => u.Name == "High"))
            { context.Priorities.Add(new TicketPriority { Name = "High" }); }

            if (!context.Priorities.Any(u => u.Name == "Medium"))
            { context.Priorities.Add(new TicketPriority { Name = "Medium" }); }

            if (!context.Priorities.Any(u => u.Name == "Low"))
            { context.Priorities.Add(new TicketPriority { Name = "Low" }); }

            if (!context.Priorities.Any(u => u.Name == "Urgent"))
            { context.Priorities.Add(new TicketPriority { Name = "Urgent" }); }

            if (!context.TicketTypes.Any(u => u.Name == "Production Fix"))
            { context.TicketTypes.Add(new TicketType { Name = "Production Fix" }); }

            if (!context.TicketTypes.Any(u => u.Name == "Project Task"))
            { context.TicketTypes.Add(new TicketType { Name = "Project Task" }); }

            if (!context.TicketTypes.Any(u => u.Name == "Software Update"))
            { context.TicketTypes.Add(new TicketType { Name = "Software Update" }); }

            if (!context.Statuses.Any(u => u.Name == "New"))
            { context.Statuses.Add(new TicketStatus { Name = "New" }); }

            if (!context.Statuses.Any(u => u.Name == "Development in Process"))
            { context.Statuses.Add(new TicketStatus { Name = "Development in Process" }); }

            if (!context.Statuses.Any(u => u.Name == "Resolved"))
            { context.Statuses.Add(new TicketStatus { Name = "Resolved" }); }

        }
    }
}