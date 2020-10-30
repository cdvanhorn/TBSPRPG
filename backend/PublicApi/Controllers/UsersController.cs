using Microsoft.AspNetCore.Mvc;

using RestSharp;

using System;
using System.Threading.Tasks;

using PublicApi.Models;
using PublicApi.Utilities;

namespace UserApi.Controllers {
    
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase {
        private IJwtHelper _jwtHelper;

        public UsersController(IJwtHelper jwtHelper) {
            _jwtHelper = jwtHelper;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            //make a request to our user microservice, we're doing this synchronouse
            var client = new RestClient("http://userapi:8001/api/");
            var request = new RestRequest("users/authenticate", DataFormat.Json);
            request.AddJsonBody(model);
            var response = await client.ExecutePostAsync<AuthenticateResponse>(request);

            if(!response.IsSuccessful)
                return BadRequest(new { message = "Username or password is incorrect" });

            response.Data.Token = _jwtHelper.GenerateToken(response.Data.Id);

            return Ok(response.Data);
        }

        // [HttpGet]
        // public async Task<IActionResult> GetAll()
        // {
        //     //var users = await _userService.GetAll();
        //     return Ok(/*users*/);
        // }
    }
}