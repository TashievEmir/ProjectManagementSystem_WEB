namespace ProjectManagementSystem.Entity
{
    public class ProjectUser
    {
        public int UserId { get; set; }
        public AppUser User { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
