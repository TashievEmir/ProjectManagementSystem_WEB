using ProjectManagementSystem.Entity;
using System.Collections.Generic;
using System;
using static ProjectManagementSystem.Enums.Enum;

namespace ProjectManagementSystem.ViewModels
{
    public class GetProjectsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Manager { get; set; }
        public List<AppUserVM> Members { get; set; }
    }
}
