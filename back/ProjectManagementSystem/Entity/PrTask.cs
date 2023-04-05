using System;

namespace ProjectManagementSystem.Entity
{
    public class PrTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public DateTime StartDate {get; set;}
        public DateTime EndDate {get; set;}
        public int Manager { get; set; }
        public AppUser UserManager { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }

    }
}
