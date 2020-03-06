using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bug_tracker.Models
{
    public class AssignUserRoleViewModel
    {
        public SelectList UserId { get; set; }
        public SelectList RoleName { get; set; }
        public ICollection<ApplicationUser> UserRoles { get; set; }
    }
}