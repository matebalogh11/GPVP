using GalaSoft.MvvmLight;
using GPVP.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPVP.ViewModels
{
    public class VideoListViewModel : ViewModelBase, IPageViewModel
    {
        public string Name { get => "Videos"; }
 
        
    }
}
