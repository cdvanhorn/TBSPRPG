using AdventureApi.Services;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace AdventureApi.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class AdventuresController : ControllerBase {
        IAdventuresService _adventuresService;

        public AdventuresController(IAdventuresService adventuresService) {
            _adventuresService = adventuresService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var adventures = await _adventuresService.GetAll();
            return Ok(adventures);
        }
    }
}