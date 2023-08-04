using ProjectManagementSystem.Interfaces;
using ProjectManagementSystem.ViewModels;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using static ProjectManagementSystem.Enums.Enum;
using Microsoft.Extensions.Caching.Memory;
using ProjectManagementSystem.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Services
{
    public class TasksDataService : ITasksDataService
    {
        private IMemoryCache cache;
        private ApplicationContext db;
        private readonly IMapper _mapper;
        public TasksDataService(ApplicationContext context, IMemoryCache cache, IMapper mapper)
        {
            db = context;
            this.cache = cache;
            this._mapper = mapper;
        }
        public async Task DeleteTask(int id)
        {
            PrTask task = db.Tasks.FirstOrDefault(x => x.Id == id);
            if (task == null)
            {
                return;
            }
            try
            {
                db.Tasks.Remove(task);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<List<GetTasksVM>> GetTasks(string email)
        {
            var userId = await db.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (userId == null) return null;

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

        public async Task<List<GetTasksVM>> GetTasksById(string id)
        {
            var userId = await db.Users.FirstOrDefaultAsync(x => x.Id == Convert.ToInt64(id));

            if (userId == null) return null;

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

        public async Task<ActionResult<PrTask>> UpdateTask(PrTaskVM taskvm)
        {
            if (!db.Tasks.Any(x => x.Id == taskvm.Id))
            {
                return null;
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
            return task;
        }
    }
}
