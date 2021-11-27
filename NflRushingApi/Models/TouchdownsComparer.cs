using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace NflRushingApi.Models
{
    public class TouchdownsComparer : IComparer<Player>
    {
        public int Compare([AllowNull] Player x, [AllowNull] Player y)
        {
            return x.Touchdowns.CompareTo(y.Touchdowns);
        }
    }
}
