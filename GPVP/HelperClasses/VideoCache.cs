using GPVP.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Media.Imaging;

namespace GPVP.HelperClasses
{
    public class VideoCache
    {
        #region Singleton

        private static VideoCache instance;
        public static VideoCache Instance
        {
            get
            {
                if (instance == null)
                    instance = new VideoCache();
                return instance;
            }
        }

        #endregion

        private VideoCache()
        {
            ThumbnailDict = new Dictionary<string, BitmapImage>();
            cachedVideoPages = new Dictionary<int, VideoPage>();
        }

        #region Properties

        public int CachedVideoCount
        {
            get { return cachedVideoPages == null ? 0 : cachedVideoPages.Count; }
        }

        public Dictionary<string, BitmapImage> ThumbnailDict { get; set; }

        #endregion

        #region Fields

        private Dictionary<int, VideoPage> cachedVideoPages;

        #endregion


        #region Public methods

        public void UpdateVideoCache(VideoPage videoPage)
        {
            foreach (var video in videoPage.Videos)
            {
                if (!ThumbnailDict.ContainsKey(video.Id))
                {
                    DownloadImage(video.ImgString, video.Id);
                }
            }
            cachedVideoPages.Add(videoPage.Pagination.CurrentPage, videoPage);
        }

        public VideoPage GetVideoPage(int pageNumber)
        {
            try
            {
                if (cachedVideoPages != null && cachedVideoPages.ContainsKey(pageNumber))
                {
                    return cachedVideoPages[pageNumber];
                }
            }
            catch
            {

            }
            return null;
        }

        #endregion

        #region Private methods

        private async void DownloadImage(Uri source, string id)
        {
            try
            {
                using (var client = new WebClient())
                {
                    var bytes = await client.DownloadDataTaskAsync(source);
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = new MemoryStream(bytes);
                    image.EndInit();

                    image.Freeze();
                    if (!ThumbnailDict.ContainsKey(id))
                        ThumbnailDict.Add(id, image);
                }

            }
            catch( WebException ex )
            {
                
            }
        }

        #endregion
    }
}
