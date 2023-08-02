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
using Microsoft.IdentityModel.Tokens;
using ProjectManagementSystem.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ProjectManagementSystem.Interfaces;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        ApplicationContext db;
        private readonly IMapper _mapper;
        private ARegistrationService registration;
        private IResetPassword resetPassword;
        public AuthController(ApplicationContext db, IMapper mapper, ARegistrationService registration, IResetPassword resetPassword)
        {
            this.db = db;
            _mapper = mapper;
            this.registration = registration;
            this.resetPassword = resetPassword;
        }

        [HttpPost]
        public IActionResult Token(UserSignInRequest obj)
        {
            var identity = GetIdentity(obj.Email, obj.Password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name,
                role = identity.Claims.ElementAt(1).Value
            };

            return new JsonResult(response);
        }

        private ClaimsIdentity GetIdentity(string email, string password)
        {
            AppUser person = db.Users.FirstOrDefault(x => x.Email == email);
            if (!BCrypt.Net.BCrypt.Verify(password, person.Password))
            {
                person=null;
            }
            if (person != null)
            {
                List<Claim> claims;
                if (person.Role==1)
                {
                    claims = new List<Claim>
                    {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "manager")
                    };
                }
                else
                {
                    claims = new List<Claim>
                    {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "employee")
                    };
                }
                
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            // если пользователя не найдено
            return null;
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(UserSignUpRequest obj)
        {
            if (obj == null)
            {
                return BadRequest();
            }

            var user = _mapper.Map<AppUser>(obj);
            registration.SaveUser(user);
            
            return Ok(user);
        }        

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(UserSignInRequest obj)
        {
            var user = resetPassword.CheckEmailExistence(obj.Email);
            if (user != null)
            {
                    resetPassword.SaveCodeInCache(user);
                    
                    return Ok();
            }
            else return NotFound();
        }

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

        [HttpPost]
        public async Task<ActionResult<AppUser>> CheckCodeEmail(UserCheckCode obj)
        {
            var answer = resetPassword.CheckCodeFromEmail(obj);
            if(answer != null)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<AppUser>> CreateNewPassword(UserSignInRequest obj)
        {
            resetPassword.CreateNewPassword(obj);
            return Ok();
        }
    }
}
