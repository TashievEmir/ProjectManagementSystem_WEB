using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProjectManagementSystem.Entity;
using ProjectManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AddingController:ControllerBase
    {
        ApplicationContext db;
        private readonly IMapper _mapper;
        public AddingController(ApplicationContext db, IMapper mapper, IMemoryCache cache)
        {
            this.db = db;
            _mapper = mapper;
        }

        //get all users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> Get()
        {
            var users= await db.Users.ToListAsync();
            return users;
        }

        //get all managers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetManagers()
        {
            var managers= await db.Users.Where(x => x.Role == 1).ToListAsync();
            return managers;
        }

        //get all projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            var projects = await db.Projects.ToListAsync();
            return projects;
        }

        //save the data about new project
        [HttpPost]
        public async Task<ActionResult<IEnumerable<AppUser>>> SaveProject( ProjectVM obj)
        {
            //adding project
            Project project = _mapper.Map<Project>(obj);
            await db.Projects.AddAsync(project);
            await db.SaveChangesAsync();

            //adding members of project to the current project
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
            return Ok();
        }

        //save the data about new task
        [HttpPost]
        public async Task<ActionResult<IEnumerable<AppUser>>> SaveTask(PrTaskVM obj)
        {
            PrTask task = _mapper.Map<PrTask>(obj);
            await db.Tasks.AddAsync(task);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
