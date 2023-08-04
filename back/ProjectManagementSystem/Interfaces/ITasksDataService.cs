using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Entity;
using ProjectManagementSystem.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Interfaces
{
    public interface ITasksDataService
    {
        Task<List<GetTasksVM>> GetTasks(string email);
        Task<List<GetTasksVM>> GetTasksById(string id);
        Task<ActionResult<PrTask>> UpdateTask(PrTaskVM taskvm);
        Task DeleteTask(int id);
    }
}
