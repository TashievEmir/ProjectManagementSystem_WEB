using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Caching.Memory;
using MimeKit.Encodings;
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

        //get projects with current user
        [HttpGet("{email}")]
        public async Task<ActionResult<List<GetProjectsVM>>> GetProjects(string email)
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
                    var managerName = await db.Users.FirstOrDefaultAsync(x => x.Id == item.Manager);
                    int enumNum = Convert.ToInt32(item.Status);
                    GetProjectsVM vm = new GetProjectsVM()
                    {
                        Id=item.Id,
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
            return projectList;
        }

        //get user's tasks 
        [HttpGet("{email}")]
        public async Task<ActionResult<List<GetTasksVM>>> GetTasks(string email)
        {
            var userId = await db.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (userId == null) return NotFound();

            var tasks = await db.Tasks.Where(x => x.UserId == userId.Id).ToListAsync();

            List<GetTasksVM> tasksList=new List<GetTasksVM>();

            foreach (var item in tasks)
            {
                var managerName = await db.Users.FirstOrDefaultAsync(x => x.Id == item.Manager);
                int enumNum = Convert.ToInt32(item.Status);
                GetTasksVM tasksvm = new GetTasksVM()
                {
                    Id = item.Id,
                    Name=item.Name,
                    Manager=managerName.Name,
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

        //get user's tasks 
        [HttpGet("{id}")]
        public async Task<ActionResult<List<GetTasksVM>>> GetTasksById(string id)
        {
            var userId = await db.Users.FirstOrDefaultAsync(x => x.Id == Convert.ToInt64(id));

            if (userId == null) return NotFound();

            var tasks = await db.Tasks.Where(x => x.UserId == userId.Id).ToListAsync();

            List<GetTasksVM> tasksList = new List<GetTasksVM>();

            foreach (var item in tasks)
            {
                var managerName = await db.Users.FirstOrDefaultAsync(x => x.Id == item.Manager);
                int enumNum = Convert.ToInt32(item.Status);
                GetTasksVM tasksvm = new GetTasksVM()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Manager = managerName.Name,
                    Status = Enum.GetName(typeof(EnumStatus), enumNum),
                    StartDate = item.StartDate.Date.ToString("yyyy-MM-dd"),
                    EndDate = item.EndDate.Date.ToString("yyyy-MM-dd"),
                    UserId = item.UserId,
                    ProjectId = item.ProjectId
                };
                tasksList.Add(tasksvm);
            }
            return tasksList;
        }

        [HttpPut]
        public async Task<ActionResult<Project>> UpdateProject([FromBody]ProjectVM projectvm)
        {
            if (!db.Projects.Any(x => x.Id == projectvm.Id))
            {
                return NotFound();
            }
            Project project = _mapper.Map<Project>(projectvm);
            try
            {
                db.Update(project);
                await db.SaveChangesAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Ok();
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<AppUser>> GetUserId(string name)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Name == name);
            if (user==null) return NotFound();
            return Ok(user);
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<AppUser>> GetUserByEmail(string email)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult<PrTask>> UpdateTask([FromBody] PrTaskVM taskvm)
        {
            if (!db.Tasks.Any(x => x.Id == taskvm.Id))
            {
                return NotFound();
            }
            PrTask task = _mapper.Map<PrTask>(taskvm);
            try
            {
                db.Update(task);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProject(int id)
        {
            var project = db.Projects.FirstOrDefault(x => x.Id == id);
            var projectUsers = db.ProjectUsers.Where(x => x.ProjectId == id);
            if (project == null || projectUsers==null)
            {
                return NotFound();
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

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            PrTask task = db.Tasks.FirstOrDefault(x => x.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            try
            {
                db.Tasks.Remove(task);
                await db.SaveChangesAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Ok();
        }

        //get all user 
        [HttpGet]
        public async Task<ActionResult<List<AppUser>>> GetUsers()
        {
            var users = await db.Users.ToListAsync();

            if (users == null) return NotFound();

            return users;
        }

        [HttpPut]
        public async Task<ActionResult<PrTask>> UpdateUser([FromBody] AppUserVM uservm)
        {

            if (!db.Users.Any(x => x.Id == uservm.Id))
            {
                return NotFound();
            }

            AppUser user = _mapper.Map<AppUser>(uservm);
            try
            {
                db.Update(user);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Ok();
        }

        [HttpDelete("{id}/{email}")]
        public async Task<ActionResult> DeleteUser(int id, string email)
        {
            AppUser userDelete = db.Users.FirstOrDefault(x => x.Id == id);
            var userEmail = GetUserByEmail(email);

            if (userDelete is null)
            {
                return NotFound();
            }
            try
            {
                if (user.Role == 1)
                {
                    if (user.Email == email) return BadRequest();

                    var tasksAsUser = db.Tasks.Where(x => x.UserId == id);
                    if (tasksAsUser is not null)
                    {
                        db.Tasks.RemoveRange(tasksAsUser);
                        await db.SaveChangesAsync();
                    }

                    var tasksAsManager = db.Tasks.Where(x => x.Manager == id);
                    if (tasksAsManager is not null)
                    {
                        db.Tasks.RemoveRange(tasksAsManager);
                        await db.SaveChangesAsync();
                    }

                    var projects = db.Projects.Where(x => x.Manager == id).ToList();
                    if (projects is not null)
                    {
                        foreach(var project in projects)
                        {
                            var pruser = db.ProjectUsers.Where(x => x.ProjectId == project.Id).ToList();
                            if (pruser is not null)
                            {
                                db.ProjectUsers.RemoveRange(pruser);
                                await db.SaveChangesAsync();
                            }
                        }
                        db.Projects.RemoveRange(projects);
                    }
                    await db.SaveChangesAsync();

                    db.Users.RemoveRange(user);
                    await db.SaveChangesAsync();
                }
                else
                {
                    var pruser = db.ProjectUsers.Where(x => x.UserId == id);
                    if (pruser is not null)
                    {
                        db.ProjectUsers.RemoveRange(pruser);
                    }
                    await db.SaveChangesAsync();

                    var tasks = db.Tasks.Where(x => x.UserId == id);
                    if (tasks is not null)
                    {
                        db.Tasks.RemoveRange(tasks);
                    }
                    await db.SaveChangesAsync();

                    db.Users.RemoveRange(user);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Ok();       
        }
    }
}

