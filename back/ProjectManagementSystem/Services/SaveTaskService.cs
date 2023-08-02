using AutoMapper;
using ProjectManagementSystem.Entity;
using ProjectManagementSystem.Interfaces;
using ProjectManagementSystem.ViewModels;
using System;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Services
{
    public class SaveTaskService : ISaveTaskService
    {
        private ApplicationContext db;
        private readonly IMapper _mapper;

        public SaveTaskService(ApplicationContext dbContext, IMapper mapper)
        {
            this.db = dbContext;
            this._mapper = mapper;
        }

        public async Task SaveTask(PrTaskVM obj)
        {
            try
            {
                PrTask task = _mapper.Map<PrTask>(obj);
                await db.Tasks.AddAsync(task);
                await db.SaveChangesAsync();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }
    }
}
