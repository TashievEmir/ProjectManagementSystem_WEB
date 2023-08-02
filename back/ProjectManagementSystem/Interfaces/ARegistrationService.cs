using ProjectManagementSystem.Entity;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Interfaces
{
    public abstract class ARegistrationService
    {
        internal abstract void ValidCorrectEmailStyle(string email);
        internal abstract string HashThePassword(string password);
        internal abstract Task SaveToDB(AppUser obj);
        internal void SaveUser (AppUser obj)
        {
            ValidCorrectEmailStyle(obj.Email);

            obj.Password = HashThePassword(obj.Password);
            
            SaveToDB(obj);
        }
    }
}
