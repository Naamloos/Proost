using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeestSpel.Entities
{
    public class Room
    {
        public string RoomCode { get; set; }

        /// <summary>
        /// Key stored in hosts' session.
        /// </summary>
        public string HostKey { get; set; }

        /// <summary>
        /// index of selected pack
        /// </summary>
        public int SelectedPack { get; set; }
    }
}
