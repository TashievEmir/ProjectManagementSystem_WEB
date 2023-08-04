using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Caching.Memory;
using MimeKit.Encodings;
using Org.BouncyCastle.Math.EC.Rfc7748;
using ProjectManagementSystem.Entity;
using ProjectManagementSystem.Interfaces;
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
        private readonly IProjectsDataService projectsDataService;
        private readonly ITasksDataService tasksDataService;
        private readonly IUsersDataService usersDataService;

        public DataController(ApplicationContext db, IMapper mapper, IMemoryCache cache, ITasksDataService tasksDataService, IUsersDataService usersDataService, IProjectsDataService projectsDataService)
        {
            this.db = db;
            _mapper = mapper;
            this.tasksDataService = tasksDataService;
            this.usersDataService = usersDataService;
            this.projectsDataService = projectsDataService;
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<List<GetProjectsVM>>> GetProjects(string email)
        {
            var listOfProjects = await projectsDataService.GetProjects(email);
            return Ok(listOfProjects);
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<List<GetTasksVM>>> GetTasks(string email)
        {
            var listOfTasks = await tasksDataService.GetTasks(email);
            return Ok(listOfTasks);
        }
 
        [HttpGet("{id}")]
        public async Task<ActionResult<List<GetTasksVM>>> GetTasksById(string id)
        {
           var listOfTasks = await tasksDataService.GetTasksById(id);
           return Ok(listOfTasks);
        }

        [HttpPut]
        public async Task<ActionResult<Project>> UpdateProject([FromBody]ProjectVM projectvm)
        {
            var project = await projectsDataService.UpdateProjects(projectvm);
            return Ok(project);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<AppUser>> GetUserId(string name)
        {
            var user = await usersDataService.GetUserId(name);
            if (user==null) return NotFound();
            return Ok(user);
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<AppUser>> GetUserByEmail(string email)
        {
            var user = await usersDataService.GetUserByEmail(email);

            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult<PrTask>> UpdateTask([FromBody] PrTaskVM taskvm)
        {
            var task = await tasksDataService.UpdateTask(taskvm);

            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProject(int id)
        {
            projectsDataService.DeleteProject(id);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            tasksDataService.DeleteTask(id);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<AppUser>>> GetUsers()
        {
            var users = await db.Users.ToListAsync();

            if (users == null) return NotFound();

            return users;
        }

        [HttpPut]
        public async Task<ActionResult<AppUser>> UpdateUser([FromBody] AppUserVM uservm)
        {
            var user = await usersDataService.UpdateUser(uservm);
            return Ok(user);
        }

        [HttpDelete("{id}/{email}")]
        public async Task<ActionResult> DeleteUser(int id, string email)
        {
            usersDataService.DeleteUser(id, email);
            return Ok();       
        }
    }
}

