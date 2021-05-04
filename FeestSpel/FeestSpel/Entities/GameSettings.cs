using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeestSpel.Entities
{
    public class GameSettings
    {
        public Difficulty Difficulty { get; set; }

        public List<string> Players { get; set; } = new List<string>();
    }

    public enum Difficulty
    {
        Sober,
        Normal,
        Drunk
    }
}
