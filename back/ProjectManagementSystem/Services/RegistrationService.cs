using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Entity;
using ProjectManagementSystem.Interfaces;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Services
{
    public class RegistrationService : ARegistrationService
    {
        private ApplicationContext db;
        public RegistrationService(ApplicationContext context)
        {
            db = context;
        }

        internal override string HashThePassword(string password)
        {
            string newPassword;
            newPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return newPassword;
        }

        internal async override Task SaveToDB(AppUser obj)
        {
            try
            {
                db.Users.Add(obj);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        internal override void ValidCorrectEmailStyle(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (!match.Success)
            {
                throw new Exception("Invalid email");
            }
        }
    }
}
