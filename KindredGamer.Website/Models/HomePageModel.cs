using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KindredGamer.Website.Models
{
    public class HomePageModel
    {
        public List<ApiGame> Games { get; set; }
        public List<ApiUser> Users { get; set; }
    }
}
