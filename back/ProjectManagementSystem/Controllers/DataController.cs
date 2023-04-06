using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProjectManagementSystem.Entity;
using ProjectManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProjectManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DataController : ControllerBase
    {
        ApplicationContext db;
        private readonly IMapper _mapper;

        public DataController(ApplicationContext db, IMapper mapper, IMemoryCache cache)
        {
            this.db = db;
            _mapper = mapper;
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<List<Project>>> GetProjectsAsManager(string email)
        {
                var userId = await db.Users.FirstOrDefaultAsync(x => x.Email == email);
                
            if (userId == null) return NotFound();

                var temp = await db.Projects.Where(x => x.Manager == userId.Id).ToListAsync();
                return temp;
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<List<Project>>> GetProjectsAsEmployee(string email)
        {
            using (var dbc = db)
            {
                List<Project> AllProjects = new List<Project>();
                var userId = await dbc.Users.FirstOrDefaultAsync(x => x.Email == email);
                var projectIds = await dbc.ProjectUsers.Where(x => x.UserId == userId.Id).Select(x => x.ProjectId).ToListAsync();
                foreach (var projectId in projectIds)
                {
                    try
                    {
                        var project =await dbc.Projects.Where(x => x.Id == projectId).ToListAsync();
                        if (project != null)
                        {
                            AllProjects.AddRange(project);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                return AllProjects;
            }

        }

        //get user's tasks 
        [HttpGet("{email}")]
        public async Task<ActionResult<List<GetTasksVM>>> GetTasks(string email)
        {
            List<PrTask> AllProjects = new List<PrTask>();
            var userId = await db.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (userId == null) return NotFound();

            var tasks = await db.Tasks.Where(x => x.UserId == userId.Id).ToListAsync();
            List<GetTasksVM> tasksList=new List<GetTasksVM>();
            foreach (var item in tasks)
            {
                GetTasksVM tasksvm = new GetTasksVM()
                {
                    Name=item.Name,
                    Manager=item.Manager,
                    Status=item.Status,
                    StartDate=item.StartDate.Date.ToString("yyyy-MM-dd"),
                    EndDate=item.EndDate.Date.ToString("yyyy-MM-dd"),
                    UserId=item.UserId,
                    ProjectId=item.ProjectId
                };
                tasksList.Add(tasksvm);
            }
            return tasksList;
        }

    }
}

