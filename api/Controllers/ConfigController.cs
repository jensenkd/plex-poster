using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlexPoster.Api.ResourceModels;
using PlexPoster.Api.Services;

namespace PlexPoster.Api.Controllers
{
    public class ConfigController : Controller
    {
        private readonly PlexService _plexService;

        public ConfigController(PlexService plexService)
        {
            _plexService = plexService;
        }
        
        [HttpGet]
        [Route("api/config")]
        public async Task<IActionResult> Index()
        {
            var config = await _plexService.GetConfig();
            return Ok(config);
        }
        
        [HttpPost]
        [Route("api/config")]
        public IActionResult UpdateConfig(ConfigModel model)
        {
             _plexService.UpdateConfig(model);
            return Ok(model);
        }
    }
}