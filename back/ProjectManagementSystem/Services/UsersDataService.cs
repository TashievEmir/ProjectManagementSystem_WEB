using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProjectManagementSystem.Entity;
using ProjectManagementSystem.Interfaces;
using ProjectManagementSystem.ViewModels;
using System.Linq;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Services
{
    public class UsersDataService : IUsersDataService
    {
        private IMemoryCache cache;
        private ApplicationContext db;
        private readonly IMapper _mapper;
        public UsersDataService(ApplicationContext context, IMemoryCache cache, IMapper mapper)
        {
            db = context;
            this.cache = cache;
            _mapper = mapper;
        }
        public async Task DeleteUser(int id, string email)
        {
            AppUser userDelete = db.Users.FirstOrDefault(x => x.Id == id);
            var userEmail = GetUserByEmail(email);

            if (userDelete is null)
            {
                return;
            }
            try
            {
                if (userDelete.Role == 1)
                {
                    if (userDelete.Email == email) return;

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
                        foreach (var project in projects)
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

                    db.Users.RemoveRange(userDelete);
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

                    db.Users.RemoveRange(userDelete);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<AppUser> GetUserByEmail(string email)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null) return null;

            return user;
        }

        public async Task<AppUser> GetUserId(string name)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Name == name);
            if (user == null) return null;
            return user;
        }


        public async Task<ActionResult<AppUser>> UpdateUser(AppUserVM uservm)
        {
            if (!db.Users.Any(x => x.Id == uservm.Id))
            {
                return null;
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
            return user;
        }
    }
}
