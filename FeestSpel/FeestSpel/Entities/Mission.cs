using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeestSpel.Entities
{
    public class Mission
    {
        public string MissionText { get; set; } = "{0} en {1} spelen steen-papier-schaar.";

        public int SubjectCount { get; set; } = 2;

        public string TakesDrinks { get; set; } = "De verliezer neemt {0} slokken.";

        public string FinishesGlass { get; set; } = "De verliezer drinkt zijn glas leeg.";
    }
}
