using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Entity;
using ProjectManagementSystem.ViewModels;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Interfaces
{
    public interface IUsersDataService
    {
        Task<AppUser> GetUserId(string name);
        Task<AppUser> GetUserByEmail(string email);
        Task<ActionResult<AppUser>> UpdateUser(AppUserVM uservm);
        Task DeleteUser(int id, string email);


    }
}
