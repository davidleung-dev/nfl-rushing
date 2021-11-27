using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NflRushingApi.Models
{
    public class GetPlayersResponse
    {
        public IList<Player> Players { get; set; }

        public int TotalPlayerCount { get; set; }
    }
}
