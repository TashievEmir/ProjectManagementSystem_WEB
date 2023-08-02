using ProjectManagementSystem.Entity;
using ProjectManagementSystem.ViewModels;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Interfaces
{
    public interface ISaveProjectService
    {
         Task SaveProject(ProjectVM obj);
         Task SavePeopleInProject(ProjectVM obj);
    }
}
