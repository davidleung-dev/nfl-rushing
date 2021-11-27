using System;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace NflRushingApi.Models
{
    public class Player
    {
        public Player(RawJsonPlayer jsonPlayer)
        {
            Name = jsonPlayer.Name;
            Team = jsonPlayer.Team;
            Position = jsonPlayer.Position;
            Attempts = jsonPlayer.Attempts;
            Attempts_Game = jsonPlayer.Attempts_Game;

            String jsonYds = jsonPlayer.Yards.GetRawText();
            jsonYds = jsonYds.Replace(",", "");
            jsonYds = jsonYds.Replace("\"", "");
            Yards = Int32.Parse(jsonYds, NumberStyles.Integer);

            Average = jsonPlayer.Average;
            Yards_Game = jsonPlayer.Yards_Game;
            Touchdowns = jsonPlayer.Touchdowns;

            int longYards = 0;
            bool longTd = false;
            String jsonLng = jsonPlayer.Longest.GetRawText();

            // Parse out the T, if exists
            Match lngMatch = Regex.Match(jsonLng, @"(\d+)(T)?");
            if (lngMatch.Success)
            {
                longYards = Int32.Parse(lngMatch.Groups[1].Value);
                longTd = lngMatch.Groups[2].Value == "T";
            }

            Longest = new LongestRush() { Yards = longYards, Touchdown = longTd };

            First = jsonPlayer.First;
            First_Pct = jsonPlayer.First_Pct;
            Twenty_Plus = jsonPlayer.Twenty_Plus;
            Forty_Plus = jsonPlayer.Forty_Plus;
            Fumbles = jsonPlayer.Fumbles;
        }

        [JsonPropertyName("Player")]
        public String Name { get; set; }

        public String Team { get; set; }

        [JsonPropertyName("Pos")]
        public String Position { get; set; }

        [JsonPropertyName("Att")]
        public int Attempts { get; set; }

        [JsonPropertyName("Att/G")]
        public float Attempts_Game { get; set; }

        // Sorting Field
        [JsonPropertyName("Yds")]
        public int Yards { get; set; }

        [JsonPropertyName("Avg")]
        public float Average { get; set; }

        [JsonPropertyName("Yds/G")]
        public float Yards_Game { get; set; }

        // Sorting Field
        [JsonPropertyName("TD")]
        public int Touchdowns { get; set; }

        // Sorting Field
        [JsonPropertyName("Lng")]
        public LongestRush Longest { get; set; }

        [JsonPropertyName("1st")]
        public int First { get; set; }

        [JsonPropertyName("1st%")]
        public float First_Pct { get; set; }

        [JsonPropertyName("20+")]
        public int Twenty_Plus { get; set; }

        [JsonPropertyName("40+")]
        public int Forty_Plus { get; set; }

        [JsonPropertyName("FUM")]
        public int Fumbles { get; set; }

    }
}
