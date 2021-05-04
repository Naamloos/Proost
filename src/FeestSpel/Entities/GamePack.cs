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

        public List<Mission> Missions { get; set; } = new List<Mission>() { new Mission() };

        public List<SubMission> SubMissions { get; set; } = new List<SubMission>() { new SubMission() };

        public SubMission GetNewSubMission()
        {
            if (SubMissions.Count() == 0)
                return null;

            return SubMissions.ElementAt(new Random().Next(0, SubMissions.Count()));
        }

        public string BuildNewMissionString(GameSettings settings)
        {
            var rng = new Random();

            var missionCount = Missions.Count();
            var selectedMission = Missions.ElementAt(rng.Next(0, missionCount));

            var maxSelection = settings.Players.Count() - (selectedMission.SubjectCount - 1);

            // using ToList to get a NEW list without shuffling the original list.
            var players = settings.Players.ToList().OrderBy(x => rng.Next());
            // Get correct amount of random players, and shuffle selection
            var subjects = settings.Players.GetRange(rng.Next(0, maxSelection), selectedMission.SubjectCount).OrderBy(x => rng.Next());

            var consequence = "";

            switch (settings.Difficulty)
            {
                default:
                case Difficulty.Normal:
                    if(rng.Next(0, 2) == 1)
                    {
                        consequence = selectedMission.FinishesGlass;
                    }
                    else
                    {
                        consequence = string.Format(selectedMission.TakesDrinks, rng.Next(1, 5));
                    }
                    break;

                case Difficulty.Drunk:
                    consequence = selectedMission.FinishesGlass;
                    break;

                case Difficulty.Sober:
                    consequence = string.Format(selectedMission.TakesDrinks, rng.Next(2, 5));
                    break;
            }

            return string.Format(selectedMission.MissionText, subjects.ToArray()) + " " + consequence;
        }

        public int GetMinimumPlayers()
        {
            return Missions.Select(x => x.SubjectCount).Max();
        }
    }
}