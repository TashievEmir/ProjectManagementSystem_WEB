using ProjectManagementSystem.Entity;
using ProjectManagementSystem.ViewModels;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Interfaces
{
    public interface ISaveTaskService
    {
        Task SaveTask(PrTaskVM obj);
    }
}
