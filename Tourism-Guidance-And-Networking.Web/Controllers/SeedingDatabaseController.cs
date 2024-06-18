using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tourism_Guidance_And_Networking.Core.Consts;
using Tourism_Guidance_And_Networking.Core.Helper;

namespace Tourism_Guidance_And_Networking.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =Roles.Admin)]
    public class SeedingDatabaseController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public SeedingDatabaseController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("SeedingUsers")]
        public async Task<IActionResult> SeedingUsers()
        {
            var user = await _unitOfWork.ApplicationUsers.FindAsync(u=>u.Id== "60728638-96b8-4576-ac14-da785002ee04");

            if (user is not null)
                return BadRequest("Users Already Exists"); 

            List<ApplicationUser> applicationUsers = new()
            {
                new ApplicationUser()
                {
                    Id="60728638-96b8-4576-ac14-da785002ee04",
                    UserName = "user1",
                    Email = "user1@gmail.com",
                    FirstName = "user1",
                    LastName = "user1",
                    PhoneNumber = "1112223333",
                    Address = "10th of ramadan"
                },
                new ApplicationUser()
                {
                   Id="60728638-96c8-4576-ac14-da785002ee04",
                    UserName = "user2",
                    Email = "user2@gmail.com",
                    FirstName = "user2",
                    LastName = "user2",
                    PhoneNumber = "1112223333",
                    Address = "10th of ramadan"

                },
                new ApplicationUser()
                {
                     Id="60728638-96d8-4576-ac14-da785002ee04",
                    UserName = "user3",
                    Email = "user3@gmail.com",
                    FirstName = "user3",
                    LastName = "user3",
                    PhoneNumber = "1112223333",
                    Address = "10th of ramadan"
                },
                new ApplicationUser()
                {
                    Id="60728638-96e8-4576-ac14-da785002ee04",
                    UserName = "user4",
                    Email = "user4@gmail.com",
                    FirstName = "user4",
                    LastName = "user4",
                    PhoneNumber = "1112223333",
                    Address = "10th of ramadan"
                },
                new ApplicationUser()
                {
                     Id="60728638-96f8-4576-ac14-da785002ee04",
                    UserName = "user5",
                    Email = "user5@gmail.com",
                    FirstName = "user5",
                    LastName = "user5",
                    PhoneNumber = "1112223333",
                    Address = "10th of ramadan"
                },
            };

            foreach (var item in applicationUsers)
            {
                await _userManager.CreateAsync(item, "User@2001");
                _userManager.AddToRoleAsync(item, Roles.Tourist).GetAwaiter().GetResult();
            }

            return Ok();
        }
    }
}
