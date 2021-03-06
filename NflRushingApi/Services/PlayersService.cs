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
        private readonly IList<Player> _playersList;

        private readonly Dictionary<String, IComparer<Player>> _sortFunction = new Dictionary<string, IComparer<Player>>()
        {
            { "yards", new YardsComparer() },
            { "longest", new LongestRushComparer() },
            { "touchdowns", new TouchdownsComparer() }
        };

        public PlayersService(IConfiguration configuration)
        {
            Configuration = configuration;
            // Configuration["DataFile"]
            String jsonString = File.ReadAllText(Configuration["DataFile"]);

            IList<RawJsonPlayer> rawJsonPlayer = JsonSerializer.Deserialize<IList<RawJsonPlayer>>(jsonString);
            List<Player> players = new List<Player>();
            foreach (var jsonPlayer in rawJsonPlayer)
            {
                players.Add(new Player(jsonPlayer));
            }
            _playersList = players;
        }

        [EnableCors("_myAllowSpecificOrigins")]
        public IList<Player> getPlayers(string filter, string sortField, string sortOrder)
        {
            Console.WriteLine(
                $"Player count: {_playersList.Count()}," +
                $" filter: '{filter}'," +
                $" sortField: {sortField}," +
                $" sortOrder: {sortOrder}");

            List<Player> result;

            // Filter fields by player name
            if (filter == null)
            {
                filter = "";
            }
            result = _playersList.Where(x => x.Name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            // Sort fields
            IComparer<Player> sortingFunction;

            if (sortField == null || !_sortFunction.ContainsKey(sortField))
            {
                // Default to sorting by yards
                Console.WriteLine($"Could not find sorting field '{sortField}' - defaulting to 'yards'");
                sortField = "yards";
            }
            _sortFunction.TryGetValue(sortField, out sortingFunction);
            result.Sort(sortingFunction);

            if (sortOrder == null || sortOrder == "desc")
            {
                result.Reverse();
            }

            return result;
        }
    }
}
