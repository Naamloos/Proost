using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeestSpel.Entities
{
    public class SubMission
    {
        public string Activation { get; set; } = "{0} is nu duits! Hij of zij moet nu met een duits accent praten. Elke fout wordt bestraft met een slok.";

        public string Deactivation { get; set; } = "{0} is niet duits meer. Hij of zijn hoeft niet meer met een duits accent te praten.";

        public int SubjectCount { get; set; } = 1;
    }

    public class ActiveSubMission
    {
        public List<string> Players;

        public SubMission SubMission;

        public int Duration;

        public ActiveSubMission(List<string> players, SubMission submission, int duration)
        {
            this.Players = players;
            this.SubMission = submission;
            this.Duration = duration;
        }
    }
}
