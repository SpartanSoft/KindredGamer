using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KindredGamer.Session.Interfaces
{
    public class SessionData
    {
        public string Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string GameId { get; set; }
        public string Title { get; set; }
        public string OwnerId { get; set; }
        public int TotalDesiredPlayers { get; set; }

        public List<string> CurrentPlayers { get; set; } = new List<string>();
    }
}
