using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NflRushingApi.Services;
using NflRushingApi.Models;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

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
        public GetPlayersResponse Get(
            [FromQuery(Name = "filter")] String filter,
            [FromQuery(Name = "sortField")] String sortField,
            [FromQuery(Name = "sortOrder")] String sortOrder,
            [FromQuery(Name = "pageNumber")] int pageNumber,
            [FromQuery(Name = "pageSize")] int pageSize)
        {
            List<Player> players = _playersService.getPlayers(filter, sortField, sortOrder).ToList();
            int totalCount = players.Count();

            // Apply pagination
            if (pageSize == 0)
            {
                pageSize = 10;
            }

            players = reducePlayersToPage(players, pageNumber, pageSize);

            return new GetPlayersResponse()
            {
                Players = players,
                TotalPlayerCount = totalCount
            };
        }

        [HttpGet("download")]
        public async Task<IActionResult> CsvDownload(
            [FromQuery(Name = "filter")] String filter,
            [FromQuery(Name = "sortField")] String sortField,
            [FromQuery(Name = "sortOrder")] String sortOrder,
            [FromQuery(Name = "pageNumber")] int pageNumber,
            [FromQuery(Name = "pageSize")] int pageSize)
        {
            List<Player> players = _playersService.getPlayers(filter, sortField, sortOrder).ToList();

            // If page number and page size specified, reduce list
            if (pageSize > 0)
            {
                players = reducePlayersToPage(players, pageNumber, pageSize);
            }

            return new PlayerCsvResult(players, "rushing.csv");
        }

        private List<Player> reducePlayersToPage(List<Player> players, int pageNumber, int pageSize)
        {
            int startIdx = pageNumber * pageSize;                           // Calculate the starting index
            int pageCount = Math.Min(pageSize, players.Count - startIdx);   // Calculate the number of items to take: minimum
                                                                            //  of the page size and the number of items remaining from the start index

            return players.GetRange(startIdx, pageCount);                   // Get the subset
        }
    }
}
