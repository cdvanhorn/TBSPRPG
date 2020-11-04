using AdventureApi.Services;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace AdventureApi.Controllers {

    [ApiController]
    [Route("/api/[controller]")]
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


        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name) {
            var adventure = await _adventuresService.GetByName(name);
            return Ok(adventure);
        }
    }
}