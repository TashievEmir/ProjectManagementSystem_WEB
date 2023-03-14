using Microsoft.EntityFrameworkCore;

namespace ProjectManagementSystem.Entity
{
    public class ApplicationContext:DbContext
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-QTJG2EE;Database=ProjectMS;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>()
            .HasOne(p => p.UserManager)
            .WithMany(p => p.TaskManager)
            .HasForeignKey(p => p.Manager)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Task>()
            .HasOne(p => p.User)
            .WithMany(p => p.TaskUser)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Project>()
            .HasOne(p => p.User)
            .WithMany(p => p.Projects)
            .HasForeignKey(p => p.Manager)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProjectUser>()
                .HasKey(sc => new { sc.UserId, sc.ProjectId });

            modelBuilder.Entity<ProjectUser>()
                .HasOne<AppUser>(sc => sc.User)
                .WithMany(s => s.ProjectUsers)
                .HasForeignKey(sc => sc.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProjectUser>()
                .HasOne<Project>(sc => sc.Project)
                .WithMany(s => s.ProjectUsers)
                .HasForeignKey(sc => sc.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
