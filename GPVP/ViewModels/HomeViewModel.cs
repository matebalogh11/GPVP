using GalaSoft.MvvmLight;
using GPVP.ViewModels.Interfaces;
using System;
using System.Diagnostics;
using System.Reflection;

namespace GPVP.ViewModels
{
    public class HomeViewModel : ViewModelBase, IPageViewModel
    {
        public string Name => Properties.Displayresource.HomePageName;

        public int Year => DateTime.Now.Year;

        public string VersionNumber
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                var fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
                return $"{Properties.Displayresource.VersionTag}{fileVersion.ProductVersion}";
            }
        }
    }
}
