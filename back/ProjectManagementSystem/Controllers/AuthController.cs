using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using ProjectManagementSystem.ViewModels;
using MimeKit;
using MimeKit.Text;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using MailKit.Security;

namespace ProjectManagementSystem.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private int code;
        ApplicationContext db;
        private readonly IMapper _mapper;
        public AuthController(ApplicationContext db, IMapper mapper)
        {
            this.db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> Get()
        {
            return await db.Users.ToListAsync();
        }

        // POST auth?name=emir&password=123
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

        [HttpPost]
        public async Task<ActionResult<AppUser>> CheckSuchEmailExist(UserSignInRequest obj)
        {
            var user = db.Users.FirstOrDefaultAsync(x => x.Email == obj.Email);
            if (user != null)
            {
                try 
                {
                    SendEmail();    
                    return Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                return Ok();
            }
            else return NotFound();
        }
        public void SendEmail()
        {
            String SendMailFrom = "tes01.star@gmail.com";
            String SendMailTo = "tashievemir01@gmail.com";
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(SendMailFrom));

            email.To.Add(MailboxAddress.Parse(SendMailTo));

            email.Subject = "Title";

            email.Body = new TextPart(TextFormat.Html) { Text = $"<a href=\"http://localhost:63342/front/pages/main.html?_ijt=8c8usg8e3a0u0l4tq1rn6sl4dl&_ij_reload=RELOAD_ON_SAVE\">Reset Password</a>" };


            using var smtp = new SmtpClient();

            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

            smtp.Authenticate(SendMailFrom, "hwxrwoardwqjpdkn");

            smtp.Send(email);

            smtp.Disconnect(true);
        }

        [HttpPost]
        public async Task<ActionResult<AppUser>> CheckCodeEmail(int receiveCode)
        {
            if(receiveCode==code) return Ok();
            else return BadRequest();
        }
        // PUT api/users/
        /*[HttpPut]
        public async Task<ActionResult<AppUser>> Put(AppUser user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            if (!db.Users.Any(x => x.Id == user.Id))
            {
                return NotFound();
            }

            db.Update(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AppUser>> Delete(int id)
        {
            AppUser user = db.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }
        */
    }
}
