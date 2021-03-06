﻿using GPVP.Entities;
using GPVP.Properties;
using GPVP.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
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
            videoService = new AwEmpireApiVideoService();
        }

        #region Properties

        public int CachedVideoCount
        {
            get { return cachedVideoPages == null ? 0 : cachedVideoPages.Count; }
        }

        public Dictionary<string, BitmapImage> ThumbnailDict { get; set; }

        #endregion

        #region Fields

        private readonly Dictionary<int, VideoPage> cachedVideoPages;

        private readonly IVideoService videoService;

        #endregion


        #region Public methods

        public async void UpdateVideoCache(VideoPage videoPage)
        {
            try
            {
                foreach (var video in videoPage.Videos)
                {
                    if (!ThumbnailDict.ContainsKey(video.Id))
                    {
                        var success = await DownloadImage(video.ImgString, video.Id);
                        if (!success)
                        {
                            throw new Exception($"Image download was not successful! Video id: {video.Id}");
                        }
                    }
                }

                if ( !cachedVideoPages.ContainsKey(videoPage.Pagination.CurrentPage))
                    cachedVideoPages.Add(videoPage.Pagination.CurrentPage, videoPage);
            }
            catch
            {
                //logging needed
            }
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


        private async Task<bool> DownloadImage(Uri source, string id)
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

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        public void StartLoadingVideosAsync()
        {
            var bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(async (object sender, DoWorkEventArgs e) =>
            {
                while (CachedVideoCount < Settings.Default.PageNumberToCache)
                {
                    try
                    {
                        var result = await videoService.GetVideoByPageNumberAsync(CachedVideoCount + 1);
                        if ( result != null )
                            UpdateVideoCache(result);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Handled exception during caching videos: {ex.Message}",
                            Displayresource.Warning, MessageBoxButton.OK, MessageBoxImage.Warning);

                        break;
                    }
                }
            });

            bw.RunWorkerAsync();
        }
    }
}
