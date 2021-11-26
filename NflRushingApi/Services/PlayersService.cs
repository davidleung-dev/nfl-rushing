using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
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

            _playersList = JsonSerializer.Deserialize<IEnumerable<Player>>(jsonString);
        }

        [EnableCors("_myAllowSpecificOrigins")]
        public IEnumerable<Player> getPlayers(string filter, string sortOrder, int pageNumber, int pageSize)
        {
            Console.WriteLine($"Player count: {_playersList.Count()}, filter: '{filter}', sortOrder: {sortOrder}, pageNumber: {pageNumber}, pageSize: {pageSize}");

            return _playersList;
        }
    }
}
