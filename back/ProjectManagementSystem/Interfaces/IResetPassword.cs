using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Entity;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Interfaces
{
    public interface IResetPassword
    {
        internal AppUser CheckEmailExistence(string email);
        internal void SaveCodeInCache(AppUser user);
        internal string CheckCodeFromEmail(UserCheckCode obj);
        internal string CreateNewPassword(UserSignInRequest obj);
    }
}
