using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using ProjectManagementSystem.ViewModels;
using MimeKit;
using MimeKit.Text;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using MailKit.Security;
using Microsoft.Extensions.Caching.Memory;

namespace ProjectManagementSystem.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IMemoryCache cache;
        ApplicationContext db;
        private readonly IMapper _mapper;
        public AuthController(ApplicationContext db, IMapper mapper, IMemoryCache cache)
        {
            this.db = db;
            _mapper = mapper;
            this.cache=cache;
        }

        // check is the user exist
        [HttpPost]
        public async Task<ActionResult<AppUser>> Authent(UserSignInRequest obj)
        {
            var user = db.Users.FirstOrDefault(x => x.Email == obj.Email);
            if (user == null) return Unauthorized();
            if(!BCrypt.Net.BCrypt.Verify(obj.Password, user.Password))
            {
                return Unauthorized();
            }
            return Ok();
        }

        //create new user
        [HttpPost]
        public async Task<ActionResult<AppUser>> SignUp(UserSignUpRequest obj)
        {
            if (obj == null)
            {
                return BadRequest();
            }
            var user = _mapper.Map<AppUser>(obj);
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(user.Email);
            if (!match.Success)
            {
                return BadRequest();
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        // check email for verification for create new password
        [HttpPost]
        public async Task<ActionResult<AppUser>> CheckSuchEmailExist(UserSignInRequest obj)
        {
            ObjectCache objcache=new ObjectCache();
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == obj.Email);
            if (user != null)
            {
                try
                {
                    Random random = new Random();
                    objcache.code = random.Next(1000, 10000);
                    SendEmail(objcache.code, obj.Email);
                    cache.Set(user.Id, objcache, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
                    });
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }
            }
            else return NotFound();
        }

        //send verification code to the user
        public void SendEmail(int code, string mail)
        {
            String SendMailFrom = "tes01.star@gmail.com";
            String SendMailTo = mail;
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(SendMailFrom));

            email.To.Add(MailboxAddress.Parse(SendMailTo));

            email.Subject = "Title";

            email.Body = new TextPart(TextFormat.Text) { Text = $" Your verification code: {code} " };

            using var smtp = new SmtpClient();

            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

            smtp.Authenticate(SendMailFrom, "hwxrwoardwqjpdkn");

            smtp.Send(email);

            smtp.Disconnect(true);
        }

        //this func check code which was has sent to user's email
        [HttpPost]
        public async Task<ActionResult<AppUser>> CheckCodeEmail(UserCheckCode obj)
        {
            ObjectCache objectCache = new ObjectCache();
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == obj.Email);
            if (!cache.TryGetValue(user.Id,out objectCache))
            {
                return BadRequest();
            }
            else
            {
                if (obj.Code == objectCache.code) return Ok();
                return BadRequest();
            }
        }

        //create new password for the user
        [HttpPost]
        public async Task<ActionResult<AppUser>> CreateNewPassword(UserSignInRequest obj)
        {
            var user = db.Users.FirstOrDefault(x => x.Email == obj.Email);
            if (user == null) return NotFound();
            user.Password=obj.Password;
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }
    }
}
