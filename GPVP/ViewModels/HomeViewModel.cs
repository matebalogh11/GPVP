using GalaSoft.MvvmLight;
using GPVP.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPVP.ViewModels
{
    public class HomeViewModel : ViewModelBase, IPageViewModel
    {
        public string Name => "Home";
    }
}
