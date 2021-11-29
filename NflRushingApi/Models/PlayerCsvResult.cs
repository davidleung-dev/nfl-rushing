using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NflRushingApi.Models
{
    public class PlayerCsvResult : FileResult
    {
        private readonly IList<Player> _playersData;

        public PlayerCsvResult(IList<Player> players, string fileName) : base("text/csv")
        {
            _playersData = players;
            FileDownloadName = fileName;
        }

        public async override Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            context.HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + FileDownloadName });

            using (var streamWriter = new StreamWriter(response.Body))
            {
                await streamWriter.WriteLineAsync(
                    $"Player,Team,Position,Attempts,Attempts/Game," +
                    $"Yards,Average,Yards/Game,Touchdowns,LongestRush," +
                    $"1stDowns,1stDowns%,20+Yards,40+Yards,Fumbles"
                    );

                foreach(var p in _playersData)
                {
                    await streamWriter.WriteLineAsync(
                        $"{p.Name},{p.Team},{p.Position},{p.Attempts},{p.Attempts_Game}," +
                        $"{p.Yards},{p.Average},{p.Yards_Game},{p.Touchdowns},{p.Longest}," +
                        $"{p.First},{p.First_Pct},{p.Twenty_Plus},{p.Forty_Plus},{p.Fumbles}"
                        );
                    await streamWriter.FlushAsync();
                }
                await streamWriter.FlushAsync();
            }
        }
    }
}
