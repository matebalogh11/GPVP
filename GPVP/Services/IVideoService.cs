using GPVP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPVP.Services
{
    public interface IVideoService
    {
        Video GetVideoById(long id);

        Task<VideoPage> GetVideos( int pageNumber );

    }
}
