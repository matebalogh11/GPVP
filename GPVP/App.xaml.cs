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
            InitWindow();
        }

        private void InitWindow()
        {
            var mainWindow = new Views.ApplicationView();
            mainWindow.DataContext = new ViewModels.ApplicationViewModel();

            Current.MainWindow = mainWindow;
            Current.MainWindow.Title = Displayresource.AppName;
            Current.MainWindow.Show();
        }
    }
}
