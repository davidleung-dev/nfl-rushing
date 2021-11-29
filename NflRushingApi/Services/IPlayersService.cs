using NflRushingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NflRushingApi.Services
{
    public interface IPlayersService
    {
        public IList<Player> getPlayers(
            String filter,
            String sortField,
            String sortOrder);
    }
}
