using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KindredGamer.Website.Models
{
    public class ApiUser
    {
        public bool IsInSession { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string GamerTag { get; set; }
    }
}
