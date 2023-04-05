using System.Collections.Generic;

namespace ProjectManagementSystem.Entity
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public short Role { get; set; }
        public string Email { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<PrTask> TaskManager { get; set; }
        public ICollection<PrTask> TaskUser { get; set; }
        public ICollection<ProjectUser> ProjectUsers { get; set; }

    }
}
