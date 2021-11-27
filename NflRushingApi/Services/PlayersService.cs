using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using NflRushingApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace NflRushingApi.Services
{
    public class PlayersService : IPlayersService
    {
        private readonly IConfiguration Configuration;
        private readonly IEnumerable<Player> _playersList;

        public PlayersService(IConfiguration configuration)
        {
            Configuration = configuration;
            // Configuration["DataFile"]
            String jsonString = File.ReadAllText(Configuration["DataFile"]);

            IEnumerable<RawJsonPlayer> rawJsonPlayer = JsonSerializer.Deserialize<IEnumerable<RawJsonPlayer>>(jsonString);
            List<Player> players = new List<Player>();
            foreach (var jsonPlayer in rawJsonPlayer)
            {
                players.Add(new Player(jsonPlayer));
            }
            _playersList = players;
        }

        [EnableCors("_myAllowSpecificOrigins")]
        public IEnumerable<Player> getPlayers(string filter, string sortField, string sortOrder, int pageNumber, int pageSize)
        {
            Console.WriteLine(
                $"Player count: {_playersList.Count()}," +
                $" filter: '{filter}'," +
                $" sortField: {sortField}," +
                $" sortOrder: {sortOrder}," +
                $" pageNumber: {pageNumber}," +
                $" pageSize: {pageSize}");
            var result = _playersList;

            return result;
        }
    }
}
