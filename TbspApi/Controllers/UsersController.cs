using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using TbspApi.Models;
using TbspApi.Services;

namespace TbspApi.Controllers {
    
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase {
        private IUserService _userService;

        public UsersController(IUserService userService) {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = await _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }
    }
}