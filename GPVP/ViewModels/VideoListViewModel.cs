using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GPVP.Entities;
using GPVP.HelperClasses;
using GPVP.Properties;
using GPVP.Services;
using GPVP.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GPVP.ViewModels
{
    public class VideoListViewModel : ViewModelBase, IPageViewModel
    {
        #region Properties

        public Page ActualPage { get; set; }

        public IEnumerable<Video> OriginalVideoList { get; set; }

        public bool NextBtnEnabled
        {
            get { return !(ActualPage.CurrentPage == ActualPage.TotalPages); }
        }

        public bool PrevBtnEnabled
        {
            get { return !(ActualPage.CurrentPage == 1); }

        }

        private int pageNumber;
        public int PageNumber
        {
            get { return pageNumber; }
            set
            {
                if (value != pageNumber)
                    pageNumber = value;
                RaisePropertyChanged(nameof(PageNumber));
            }
        }

        private int totalPages;
        public int TotalPages
        {
            get { return totalPages; }
            set
            {
                if (value != totalPages)
                    totalPages = value;
                RaisePropertyChanged(nameof(TotalPages));
            }
        }

        public string Name { get => "Videos"; }

        private IEnumerable<Video> videoList;
        public IEnumerable<Video> VideoList
        {
            get { return videoList; }
            set
            {
                if (value != videoList)
                {
                    SetCachedImages(value);
                    videoList = value;
                    RaisePropertyChanged(nameof(VideoList));
                }
            }
        }

        private IEnumerable<string> videoQualityList;
        public IEnumerable<string> VideoQualityList
        {
            get { return videoQualityList; }
            set
            {
                if (value != videoQualityList)
                    videoQualityList = value;
                RaisePropertyChanged(nameof(VideoQualityList));
            }
        }

        public string VideoQuality { get; set; } = "Video quality: ";
        public string Duration { get; set; } = "Duration: ";

        public bool TagsVisible { get; set; } = true;

        private List<Tag> videoTagList;
        public List<Tag> VideoTagList
        {
            get { return videoTagList; }
            set
            {
                videoTagList = value;
                RaisePropertyChanged(nameof(VideoTagList));
            }
        }

        private string selectedQuality;
        public string SelectedQuality
        {
            get { return selectedQuality; }
            set
            {
                if (value != selectedQuality)
                {
                    selectedQuality = value;
                    SetFilteredVideoList();
                    RaisePropertyChanged(nameof(SelectedQuality));
                }
            }
        }

        public IEnumerable<string> DurationList { get; set; }

        private string selectedDuration;
        public string SelectedDuration
        {
            get { return selectedDuration; }
            set
            {
                if (value != selectedDuration)
                {
                    selectedDuration = value;
                    SetFilteredVideoList();
                    RaisePropertyChanged(nameof(SelectedDuration));
                }
            }
        }


        #endregion

        #region Fields

        private IVideoService videoService;

        #endregion

        public VideoListViewModel()
        {
            ActualPage = new Page();
            videoService = new AwEmpireApiVideoService();

            DurationList = new List<string> { VideoLength.Short.ToString(), VideoLength.Medium.ToString(), VideoLength.Long.ToString() };
            LoadVideos();
        }

        #region Private methods

        private void LoadVideos( int pageNumber = 1)
        {
            var result = VideoCache.Instance.GetVideoPage(pageNumber);
            if ( result != null )
            {
                SetVideoProperties(result);
            }
            else
            {
                Application.Current.Dispatcher.Invoke( async () => 
                {
                    var videoPage = await videoService.GetVideos(pageNumber);
                    SetVideoProperties(videoPage);
                });
            }
        }

        private void SetVideoProperties( VideoPage videoPage )
        {
            OriginalVideoList = new List<Video>(videoPage.Videos);
            VideoList = videoPage.Videos;
            ActualPage = videoPage.Pagination;
            PageNumber = ActualPage.CurrentPage;
            TotalPages = ActualPage.TotalPages;

            FillQualityList();
            FillTagList();

            RaisePropertyChanged(nameof(NextBtnEnabled));
            RaisePropertyChanged(nameof(PrevBtnEnabled));
        }

        private void FillQualityList()
        {
            var tempList = VideoList.Select(v => v.Quality).Distinct().ToList();
            tempList.Insert(0, string.Empty);

            VideoQualityList = tempList;
        }

        private void FillTagList()
        {
            var tempList = new List<Tag>();
            foreach (var tag in OriginalVideoList.SelectMany(o => o.Tags).Distinct())
            {
                var myTag = new Tag { Name = tag, Activated = true };
                myTag.ActivateEvent += TagActivityEvent;
                tempList.Add(myTag);
            }
            VideoTagList = tempList.OrderBy(t => t.Name).ToList();
        }

        private void TagActivityEvent(object sender, EventArgs e)
        {
            if ( sender is Tag tag)
                SetFilteredVideoList();
        }

        private void SetFilteredVideoList()
        {
            var result = OriginalVideoList.Where(o => o.Quality == (string.IsNullOrEmpty(selectedQuality) ? o.Quality : selectedQuality)
                                                   && o.Tags.Any(t => VideoTagList.Where(g => g.Activated).Select(g => g.Name).Contains(t))
                                                   && GetVideoIdsByDuration(SelectedDuration).Contains(o.Id));

            VideoList = result;
        }

        private IEnumerable<Video> GetVideoByDuration(string length)
        {
            IEnumerable<Video> result = new List<Video>(OriginalVideoList);
            if (!string.IsNullOrEmpty(length))
            {
                switch (Enum.Parse(typeof(VideoLength), length))
                {
                    case VideoLength.Short:
                        result = OriginalVideoList.Where(v => v.Duration <= Settings.Default.ShortLength);
                        break;
                    case VideoLength.Medium:
                        result = OriginalVideoList.Where(v => v.Duration > Settings.Default.ShortLength && v.Duration < Settings.Default.LongLegth);
                        break;
                    case VideoLength.Long:
                        result = OriginalVideoList.Where(v => v.Duration > Settings.Default.LongLegth);
                        break;
                }
            }
            return result;
        }

        private IEnumerable<string> GetVideoIdsByDuration(string length)
        {
            return GetVideoByDuration(length).Select(v => v.Id);
        }

        private void SetCachedImages(IEnumerable<Video> videos)
        {
            foreach (var video in videos)
            {
                if (VideoCache.Instance.ThumbnailDict.ContainsKey(video.Id))
                    video.CachedImage = VideoCache.Instance.ThumbnailDict[video.Id];
            }
        }

        #endregion

        #region Commands

        private ICommand changePageCommand;
        public ICommand ChangePageCommand
        {
            get
            {
                if (changePageCommand == null)
                    changePageCommand = new RelayCommand<string>(changePageExecute);

                return changePageCommand;
            }
        }

        private void changePageExecute( string direction )
        {
            switch(direction)
            {
                case "n":
                    LoadVideos(++ActualPage.CurrentPage);
                    break;
                case "p":
                    LoadVideos(--ActualPage.CurrentPage);
                    break;
            }
        }

        private ICommand clearAllCommand;
        public ICommand ClearAllCommand
        {
            get
            {
                if (clearAllCommand == null)
                    clearAllCommand = new RelayCommand(clearAllExecute);

                return clearAllCommand;
            }
        }
        
        private void clearAllExecute()
        {
            SelectedQuality = null;
            SelectedDuration = null;
            VideoTagList.ForEach(t => t.Activated = true);
            RaisePropertyChanged(nameof(VideoTagList));
        }

        #endregion

        public enum VideoLength
        {
            Short,
            Medium,
            Long
        }
    }
}
