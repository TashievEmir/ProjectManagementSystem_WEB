using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProjectManagementSystem.Entity;
using ProjectManagementSystem.Interfaces;
using ProjectManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProjectManagementSystem.Enums.Enum;

namespace ProjectManagementSystem.Services
{
    public class ProjectsDataService : IProjectsDataService
    {
        private IMemoryCache cache;
        private ApplicationContext db;
        private readonly IMapper _mapper;
        public ProjectsDataService(ApplicationContext context, IMemoryCache cache, IMapper mapper)
        {
            db = context;
            this.cache = cache;
            this._mapper = mapper;
        }
        public async Task DeleteProject(int id)
        {
            var project = db.Projects.FirstOrDefault(x => x.Id == id);
            var projectUsers = db.ProjectUsers.Where(x => x.ProjectId == id);

            if (project == null || projectUsers == null)
            {
                return ;
            }
            try
            {
                foreach (var item in projectUsers)
                {
                    db.ProjectUsers.Remove(item);
                }
                await db.SaveChangesAsync();

                db.Projects.Remove(project);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<List<GetProjectsVM>> GetProjects(string email)
        {
            List<GetProjectsVM> projectList = new List<GetProjectsVM>();
            var userId =  db.Users.FirstOrDefault(x => x.Email == email);
            var projectIds = await db.ProjectUsers.Where(x => x.UserId == userId.Id).Select(x => x.ProjectId).ToListAsync();
            foreach (var projectId in projectIds)
            {
                List<AppUserVM> memberlist = new List<AppUserVM>();
                var project = await db.Projects.Where(x => x.Id == projectId).ToListAsync();
                var members = await db.ProjectUsers.Where(x => x.ProjectId == projectId).Select(x => x.UserId).ToListAsync();
                foreach (var item in members)
                {
                    var member =  db.Users.Where(x => x.Id == item).ToList();
                    foreach (var x in member)
                    {
                        AppUserVM uservm = new AppUserVM()
                        {
                            Id = x.Id,
                            Name = x.Name
                        };
                        memberlist.Add(uservm);
                    }
                }
                foreach (var item in project)
                {
                    var managerName =  db.Users.FirstOrDefault(x => x.Id == item.Manager);
                    int enumNum = Convert.ToInt32(item.Status);
                    GetProjectsVM vm = new GetProjectsVM()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Manager = managerName.Name,
                        Status = Enum.GetName(typeof(EnumStatus), enumNum),
                        StartDate = item.StartDate.Date.ToString("yyyy-MM-dd"),
                        EndDate = item.EndDate.Date.ToString("yyyy-MM-dd"),
                        Members = memberlist
                    };
                    projectList.Add(vm);
                }
            }
            try
            {
                return projectList;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
            return null;
        }

        public async Task<ActionResult<Project>> UpdateProjects(ProjectVM projectvm)
        {
            if (!db.Projects.Any(x => x.Id == projectvm.Id))
            {
                throw new Exception();
            }
            Project project = _mapper.Map<Project>(projectvm);
            try
            {
                db.Update(project);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return project;
        }
    }
}
