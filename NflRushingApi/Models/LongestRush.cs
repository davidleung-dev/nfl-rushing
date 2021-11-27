using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NflRushingApi.Models
{
    public class LongestRush
    {
        [JsonPropertyName("yards")]
        public int Yards { get; set; }

        [JsonPropertyName("touchdown")]
        public bool Touchdown { get; set; }
    }
}
