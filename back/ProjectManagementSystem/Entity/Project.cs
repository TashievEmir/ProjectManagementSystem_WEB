using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementSystem.Entity
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Manager { get; set; }
        public AppUser User { get; set; }
        public ICollection<ProjectUser> ProjectUsers { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
