using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NflRushingApi.Models
{
    public class RawJsonPlayer
    {
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
        public dynamic Yards { get; set; }

        [JsonPropertyName("Avg")]
        public float Average { get; set; }

        [JsonPropertyName("Yds/G")]
        public float Yards_Game { get; set; }

        // Sorting Field
        [JsonPropertyName("TD")]
        public int Touchdowns { get; set; }

        // Sorting Field
        [JsonPropertyName("Lng")]
        public dynamic Longest { get; set; }

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
