using ProjectManagementSystem.Entity;
using System;

namespace ProjectManagementSystem.ViewModels
{
    public class GetTasksVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Manager { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
    }
}
