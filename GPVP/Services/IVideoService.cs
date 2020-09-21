using GPVP.Entities;
using System.Threading.Tasks;

namespace GPVP.Services
{
    public interface IVideoService
    {
        Task<VideoPage> GetVideoByPageNumberAsync(long pageNumber);

        Task<VideoPage> GetVideoBySecualOrientationAsync(string orientation);

        VideoPage GetVideosByPageNumber( int pageNumber );

        VideoPage GetVideosBySexualOrientation(string orientation);
    }
}
