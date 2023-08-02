using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProjectManagementSystem.Entity;
using ProjectManagementSystem.Interfaces;
using ProjectManagementSystem.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Services
{
    public class ResetPasswordService : IResetPassword
    {
        private IMemoryCache cache;
        private ApplicationContext db;
        public ResetPasswordService(ApplicationContext context, IMemoryCache cache)
        {
            db = context;
            this.cache = cache;
        }

         string IResetPassword.CheckCodeFromEmail(UserCheckCode obj)
        {
            ObjectCache objectCache = new ObjectCache();
            var user = db.Users.FirstOrDefault(x => x.Email == obj.Email);
            if (!cache.TryGetValue(user.Id, out objectCache))
            {
                return null;
            }
            else
            {
                if (obj.Code == objectCache.code) return "Ok";
                return null;
            }
        }

        AppUser IResetPassword.CheckEmailExistence(string email)
        {
            var appUser = db.Users.FirstOrDefault(x => x.Email == email);
            return appUser;
        }

        string IResetPassword.CreateNewPassword(UserSignInRequest obj)
        {
            var user = db.Users.FirstOrDefault(x => x.Email == obj.Email);
            if (user == null) return null;

            user.Password = obj.Password;
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            try
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch(Exception error)
            {
                Console.WriteLine(error.Message);
            }

            return "Ok";
        }

        void IResetPassword.SaveCodeInCache(AppUser user)
        {
            Random random = new Random();
            ObjectCache objcache = new ObjectCache();

            objcache.code = random.Next(1000, 10000);

            SendEmail sendEmail = new SendEmail();
            sendEmail.SendGmail(objcache.code, user.Email);

            cache.Set(user.Id, objcache, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
            });
        }
    }
}
