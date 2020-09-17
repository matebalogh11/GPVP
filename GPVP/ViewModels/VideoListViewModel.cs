using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GPVP.Entities;
using GPVP.Services;
using GPVP.ViewModels.Interfaces;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace GPVP.ViewModels
{
    public class VideoListViewModel : ViewModelBase, IPageViewModel
    {
        public Page ActualPage { get; set; }

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
                    videoList = value;
                RaisePropertyChanged(nameof(VideoList));
            }
        }
        

        private IVideoService videoService;
        public VideoListViewModel()
        {
            ActualPage = new Page();
            videoService = new AwEmpireApiVideoService();
            LoadVideos();
        }

        private void LoadVideos( int pageNumber = 1)
        {
            Application.Current.Dispatcher.Invoke( async () => 
            {
                var videoPage = await videoService.GetVideos(pageNumber);
                VideoList = videoPage.Videos;
                ActualPage = videoPage.Pagination;
                PageNumber = ActualPage.CurrentPage;
                TotalPages = ActualPage.TotalPages;

                RaisePropertyChanged(nameof(NextBtnEnabled));
                RaisePropertyChanged(nameof(PrevBtnEnabled));
            });
        }

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


        #endregion
    }
}
