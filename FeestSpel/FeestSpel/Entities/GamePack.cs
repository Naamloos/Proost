using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeestSpel.Entities
{
    public class GamePack
    {
        public string Title { get; set; } = "Pack Template";

        public string Description { get; set; } = "Pack Template Description";

        public string Author { get; set; } = "Naamloos";

        public string AuthorUrl { get; set; } = "https://github.com/Naamloos/";

        public List<Mission> Missions { get; set; } = 
            new List<Mission>() 
            { 
                new Mission() 
                {
                    SubjectCount = 2, 
                    MissionText = "{0} geeft een kusje aan {1} of beiden moeten hun glas leeg drinken." 
                } 
            };

        public string BuildNewMissionString(GameSettings settings)
        {
            var rng = new Random();

            var missionCount = Missions.Count();
            var selectedMission = Missions.ElementAt(rng.Next(0, missionCount - 1));

            var maxSelection = settings.Players.Count() - (selectedMission.SubjectCount + 1);

            // using ToList to get a NEW list without shuffling the original list.
            var players = settings.Players.ToList().OrderBy(x => rng.Next());
            // Get correct amount of random players, and shuffle selection
            var subjects = settings.Players.GetRange(rng.Next(0, maxSelection), selectedMission.SubjectCount).OrderBy(x => rng.Next());

            return string.Format(selectedMission.MissionText, subjects.ToArray());
        }

        public int GetMinimumPlayers()
        {
            return Missions.Select(x => x.SubjectCount).Max();
        }
    }
}