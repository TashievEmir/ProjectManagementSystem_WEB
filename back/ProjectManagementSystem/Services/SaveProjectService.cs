using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Entity;
using ProjectManagementSystem.Interfaces;
using ProjectManagementSystem.ViewModels;
using System;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Services
{
    public class SaveProjectService : ISaveProjectService
    {
        private ApplicationContext db;
        private readonly IMapper _mapper;

        public SaveProjectService(ApplicationContext dbContext, IMapper mapper)
        {
            this.db = dbContext;
            this._mapper = mapper;
        }
        public async Task SavePeopleInProject(ProjectVM obj)
        {
            var projectId = await db.Projects.FirstOrDefaultAsync(x => x.Name == obj.Name);
            foreach (var item in obj.Members)
            {
                ProjectUser projectUser = new ProjectUser()
                {
                    ProjectId = projectId.Id,
                    UserId = item
                };
                try
                {
                    await db.ProjectUsers.AddAsync(projectUser);
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public async Task SaveProject(ProjectVM obj)
        {
            try
            {
                Project project = _mapper.Map<Project>(obj);
                await db.Projects.AddAsync(project);
                await db.SaveChangesAsync();
            }
            catch(Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }
    }
}
