using GalaSoft.MvvmLight;
using GPVP.Entities;
using GPVP.Services;
using GPVP.ViewModels.Interfaces;
using System.Collections.Generic;
using System.Windows;

namespace GPVP.ViewModels
{
    public class VideoListViewModel : ViewModelBase, IPageViewModel
    {
        public string Name { get => "Videos"; }

        private IEnumerable<Video> videoList;
        public IEnumerable<Video> VideoList
        {
            get { return videoList; }
            set
            {
                if (value != videoList)
                    videoList = value;
                RaisePropertyChanged(nameof(VideoList));
            }
        }

        private IVideoService videoService;
        public VideoListViewModel()
        {
            videoService = new AwEmpireApiVideoService();
            LoadVideos();
        }

        private void LoadVideos()
        {
            Application.Current.Dispatcher.Invoke( async () => VideoList = await videoService.GetVideos());
        }
    }
}
