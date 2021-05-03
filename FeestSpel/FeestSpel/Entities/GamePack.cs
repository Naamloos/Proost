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
    }
}