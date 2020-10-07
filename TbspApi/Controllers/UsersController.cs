using Microsoft.AspNetCore.Mvc;
using TbspApi.Services;

namespace TbspApi.Controllers {
    
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase {
        private IUserService _userService;

        public UsersController(IUserService userService) {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}