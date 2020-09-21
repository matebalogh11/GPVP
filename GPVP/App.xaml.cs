using GPVP.HelperClasses;
using GPVP.Properties;
using System.Windows;

namespace GPVP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ApiHelper.InitializeClinet();
            VideoCache.Instance.StartLoadingVideosAsync();

            InitWindow();
        }


        protected override void OnExit(ExitEventArgs e)
        {
            foreach (var img in VideoCache.Instance.ThumbnailDict)
            {
                if ( img.Value.StreamSource != null)
                    img.Value.StreamSource.Dispose();
            }

            base.OnExit(e);
        }

        private void InitWindow()
        {
            var mainWindow = new Views.ApplicationView
            {
                DataContext = new ViewModels.ApplicationViewModel()
            };

            Current.MainWindow = mainWindow;
            Current.MainWindow.Title = Displayresource.AppName;
            Current.MainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"{Displayresource.UnhandledEx} {e.Exception.Message}", 
                Displayresource.Warning, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
