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
        public GetPlayersResponse getPlayers(string filter, string sortField, string sortOrder, int pageNumber, int pageSize)
        {
            Console.WriteLine(
                $"Player count: {_playersList.Count()}," +
                $" filter: '{filter}'," +
                $" sortField: {sortField}," +
                $" sortOrder: {sortOrder}," +
                $" pageNumber: {pageNumber}," +
                $" pageSize: {pageSize}");

            List<Player> result;

            // Filter fields by player name
            if (filter == null)
            {
                filter = "";
            }
            result = _playersList.Where(x => x.Name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            int totalCount = result.Count();

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

            // Apply pagination
            if (pageSize == 0)
            {
                pageSize = 10;
            }

            int startIdx = pageNumber * pageSize;                           // Calculate the starting index
            int pageCount = Math.Min(pageSize, result.Count - startIdx);    // Calculate the number of items to take: minimum
                                                                            //  of the page size and the number of items remaining from the start index

            result = result.GetRange(startIdx, pageCount);                  // Get the subset

            return new GetPlayersResponse()
            {
                Players = result,
                TotalPlayerCount = totalCount
            };
        }
    }
}
