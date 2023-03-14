using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Entity;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SignUpController : Controller
    {
        ApplicationContext db;
        public SignUpController(ApplicationContext db)
        {
            this.db = db;
        }

        
    }
}
