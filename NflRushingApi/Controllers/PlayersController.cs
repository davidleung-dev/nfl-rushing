﻿using System;
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
            return _playersService.getPlayers(filter, sortField, sortOrder, pageNumber, pageSize);
        }

        [HttpGet("download")]
        public async Task<IActionResult> CsvDownload(
            [FromQuery(Name = "filter")] String filter,
            [FromQuery(Name = "sortField")] String sortField,
            [FromQuery(Name = "sortOrder")] String sortOrder,
            [FromQuery(Name = "pageNumber")] int pageNumber,
            [FromQuery(Name = "pageSize")] int pageSize)
        {
            return new PlayerCsvResult(_playersService.getPlayers(filter, sortField, sortOrder, pageNumber, pageSize).Players, "rushing.csv");
        }
    }
}
