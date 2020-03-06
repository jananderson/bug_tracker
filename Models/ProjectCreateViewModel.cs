using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bug_tracker.Models
{
    public class ProjectCreateViewModel
    {
        [Display(Name = "Project Name")]
        public string Name { get; set; }
        public SelectList ProjectManagerId { get; set; }
    }
}