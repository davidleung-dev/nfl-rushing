using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NflRushingApi.Services;

namespace NflRushingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase
    {

        private readonly ILogger<PlayersController> _logger;
        private readonly IPlayersService _playersService;

        public PlayersController(ILogger<PlayersController> logger,
            IPlayersService playersService)
        {
            _logger = logger;
            _playersService = playersService;
        }

        [HttpGet]
        public IEnumerable<Player> Get(
            [FromQuery(Name = "filter")] String filter,
            [FromQuery(Name = "sortOrder")] String sortOrder,
            [FromQuery(Name = "pageNumber")] int pageNumer,
            [FromQuery(Name = "pageSize")] int pageSize)
        {
            return _playersService.getPlayers();
        }
    }
}
