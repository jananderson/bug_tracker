using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bug_tracker.Models
{
    public class UserProjectsViewModel
    {
        public List<ProjectIndexDetails> AdminProjs { get; set; }
        public List<ProjectIndexDetails> ProjectManProjs { get; set; }
        public List<ProjectIndexDetails> DeveloperProjs { get; set; }
        public List<ProjectIndexDetails> SubmitterProjs { get; set; }
    }
}