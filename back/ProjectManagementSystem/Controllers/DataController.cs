using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Org.BouncyCastle.Math.EC.Rfc7748;
using ProjectManagementSystem.Entity;
using ProjectManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static ProjectManagementSystem.Enums.Enum;

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

        //get projects where current user as employee
        [HttpGet("{email}")]
        public async Task<ActionResult<List<GetProjectsVM>>> GetProjectsAsEmployee(string email)
        {
            List<GetProjectsVM> projectList = new List<GetProjectsVM>();
            var userId = await db.Users.FirstOrDefaultAsync(x => x.Email == email);
            var projectIds = await db.ProjectUsers.Where(x => x.UserId == userId.Id).Select(x => x.ProjectId).ToListAsync();
            foreach (var projectId in projectIds)
            {
                List<AppUserVM> memberlist = new List<AppUserVM>();
                var project = await db.Projects.Where(x => x.Id == projectId).ToListAsync();
                var members = await db.ProjectUsers.Where(x => x.ProjectId == projectId).Select(x => x.UserId).ToListAsync();
                foreach (var item in members)
                    {
                        var member = await db.Users.Where(x => x.Id == item).ToListAsync();
                        foreach(var x in member)
                        {
                            AppUserVM uservm = new AppUserVM()
                            {
                                Id=x.Id,
                                Name=x.Name
                            };
                        memberlist.Add(uservm);
                        }                       
                    }
                foreach (var item in project)
                {
                    int enumNum = Convert.ToInt32(item.Status);
                    GetProjectsVM vm = new GetProjectsVM()
                    {
                        Name = item.Name,
                        Manager = item.Manager,
                        Status = Enum.GetName(typeof(EnumStatus), enumNum),
                        StartDate = item.StartDate.Date.ToString("yyyy-MM-dd"),
                        EndDate = item.EndDate.Date.ToString("yyyy-MM-dd"),
                        Members = memberlist
                    };
                    projectList.Add(vm);
                }              
            }
            return projectList;
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
                int enumNum = Convert.ToInt32(item.Status);
                GetTasksVM tasksvm = new GetTasksVM()
                {
                    Name=item.Name,
                    Manager=item.Manager,
                    Status= Enum.GetName(typeof(EnumStatus), enumNum),
                    StartDate =item.StartDate.Date.ToString("yyyy-MM-dd"),
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

