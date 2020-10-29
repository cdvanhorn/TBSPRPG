using Microsoft.AspNetCore.Mvc;

using RestSharp;

using System.Threading.Tasks;

using UserApi.Models;
using UserApi.Services;

namespace UserApi.Controllers {
    
    [ApiController]
    [Route("api/[controller]")]
    public class AdventuresController : ControllerBase {

        public AdventuresController() {
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var client = new RestClient($"http://adventureapi:8001/adventures");
            var request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            return Ok(response.Content);
        }
    }
}