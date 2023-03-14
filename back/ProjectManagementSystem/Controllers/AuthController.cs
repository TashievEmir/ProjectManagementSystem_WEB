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
                String SendMailFrom = "tes01.star@gmail.com";
                String SendMailTo = "tashievemir01@gmail.com";
                String SendMailSubject = "Email Subject";
                String SendMailBody = "Email Body";
                try 
                {
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 465);
                    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                    MailMessage email = new MailMessage();
                    // START
                    email.From = new MailAddress(SendMailFrom);
                    email.To.Add(SendMailTo);
                    email.CC.Add(SendMailFrom);
                    email.Subject = SendMailSubject;
                    email.Body = SendMailBody;
                    //END
                    SmtpServer.Timeout = 10000;
                    SmtpServer.EnableSsl = true;
                    SmtpServer.UseDefaultCredentials = false;
                    SmtpServer.Credentials = new NetworkCredential(SendMailFrom, "Kandaysinar*2018");
                    SmtpServer.Send(email);

                    /*var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Emir", "sender@yourdomain.com"));
                    message.To.Add(new MailboxAddress("Emir", "recipient@theirdomain.com"));
                    message.Subject = "MailKit Test";
                    message.Body = new TextPart("plain") { Text = "Hi from MailKit!" };

                    using (var client = new SmtpClient())
                    {
                        client.Connect("mail.gmail.com", 587);
                        client.Authenticate("your_username", "your_password");
                        client.Send(message);
                        client.Disconnect(true);
                    }
                    string senderEmail = "tes01.star@gmail.com";
                    string senderPassword = "Kandaysinar*2018";*/

                    //string receiverEmail = "tashievemir01@gmail.com";

                    // Create SMTP client
                    //SmtpClient client = new SmtpClient("smtp.example.com", 465);
                    //client.UseDefaultCredentials = false;
                    //client.EnableSsl = true;
                    //client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                    /*MailMessage message = new MailMessage(senderEmail, receiverEmail);
                    message.Subject = "Test email";
                    message.Body = "This is a test email sent using C#.";*/

                    //client.Send(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                return Ok();
            }
            else return NotFound();
        }

        [HttpPost("action")]
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
        public string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }*/
    }
}
