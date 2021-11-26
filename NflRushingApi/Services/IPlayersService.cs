using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NflRushingApi.Services
{
    public interface IPlayersService
    {
        public IEnumerable<Player> getPlayers(
            String filter = "",
            String sortOrder = "desc",
            int pageNumber = 0,
            int pageSize = 10);
    }
}
