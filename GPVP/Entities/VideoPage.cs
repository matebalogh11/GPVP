using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPVP.Entities
{
    public class VideoPage
    {
        public IEnumerable<Video> Videos { get; set; }

        public Page Pagination { get; set; }
    }
}
