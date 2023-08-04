using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Entity;
using ProjectManagementSystem.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Interfaces
{
    public interface IProjectsDataService
    {
        Task<List<GetProjectsVM>> GetProjects(string email);
        Task<ActionResult<Project>> UpdateProjects(ProjectVM projectvm);
        Task DeleteProject(int id);

    }
}
