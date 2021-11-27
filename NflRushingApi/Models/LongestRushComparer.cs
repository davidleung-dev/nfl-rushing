using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace NflRushingApi.Models
{
    public class LongestRushComparer : IComparer<Player>
    {
        public int Compare([AllowNull] Player x, [AllowNull] Player y)
        {
            if (x.Longest.Yards == y.Longest.Yards)
            {
                int retVal = 0;

                if (x.Longest.Touchdown == y.Longest.Touchdown)
                {
                    retVal = 0;
                }
                else if (x.Longest.Touchdown)
                {
                    retVal = 1;
                }
                else
                {
                    retVal = -1;
                }

                // TODO - Compare touchdowns
                return retVal;
            }

            return x.Longest.Yards.CompareTo(y.Longest.Yards);
        }
    }
}
