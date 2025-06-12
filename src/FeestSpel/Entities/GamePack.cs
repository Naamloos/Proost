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

        public SubMission GetNewSubMission(SubMission previous)
        {
            if (SubMissions.Count() == 0)
                return null;

            var selection = SubMissions.ElementAt(new Random().Next(0, SubMissions.Count()));

            if (selection != previous || SubMissions.Count < 2)
                return selection;

            return GetNewSubMission(previous);
        }

        public (string, Mission) BuildNewMissionString(GameSettings settings, Mission previous)
        {
            var rng = new Random();

            var missionCount = Missions.Count();
            var selectedMission = Missions.ElementAt(rng.Next(0, missionCount));

            if (selectedMission == previous && Missions.Count() > 1)
                return BuildNewMissionString(settings, previous);

            var maxSelection = settings.Players.Count() - (selectedMission.SubjectCount - 1);

            // using ToList to get a NEW list without shuffling the original list.
            // Shuffle the player list so we can easily pick a random selection
            var players = settings.Players.OrderBy(x => rng.Next()).ToList();
            // Grab the amount of players that this mission requires
            var subjects = players.Take(selectedMission.SubjectCount).ToList();

            var consequence = "";

            switch (settings.Difficulty)
            {
                default:
                case Difficulty.Normal:
                    if (rng.Next(0, 2) == 1)
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

            return (string.Format(selectedMission.MissionText, subjects.ToArray()) + " " + consequence, selectedMission);
        }

        public int GetMinimumPlayers()
        {
            return Missions.Select(x => x.SubjectCount).Max();
        }
    }
}