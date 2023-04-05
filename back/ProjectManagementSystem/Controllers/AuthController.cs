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

        [HttpPost]
        public IActionResult Token(UserSignInRequest obj)
        {
            var identity = GetIdentity(obj.Email, obj.Password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
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
