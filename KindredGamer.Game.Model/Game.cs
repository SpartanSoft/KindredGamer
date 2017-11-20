using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KindredGamer.Game
{
    [DataContract]
    public class Game
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public int MaxPlayersSupported { get; set; }

    }
}
