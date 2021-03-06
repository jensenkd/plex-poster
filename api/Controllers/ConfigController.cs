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
    }
}