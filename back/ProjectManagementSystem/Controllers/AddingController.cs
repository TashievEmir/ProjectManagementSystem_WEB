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

namespace ProjectManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AddingController:ControllerBase
    {
        ApplicationContext db;
        private readonly IMapper _mapper;
        private readonly ISaveProjectService saveProjectService;
        private readonly ISaveTaskService saveTaskService;
        public AddingController(ApplicationContext db, IMapper mapper, IMemoryCache cache, ISaveTaskService saveTaskService, ISaveProjectService saveProjectService)
        {
            this.db = db;
            _mapper = mapper;
            this.saveTaskService = saveTaskService;
            this.saveProjectService = saveProjectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> Get()
        {
            var users= await db.Users.ToListAsync();
            return users;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetManagers()
        {
            var managers= await db.Users.Where(x => x.Role == 1).ToListAsync();
            return managers;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            var projects = await db.Projects.ToListAsync();
            return projects;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectVM>> SaveProject(ProjectVM obj)
        {
            await saveProjectService.SaveProject(obj);

            await saveProjectService.SavePeopleInProject(obj);  
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<PrTaskVM>> SaveTask(PrTaskVM obj)
        {
            saveTaskService.SaveTask(obj);
            return Ok();
        }
    }
}
