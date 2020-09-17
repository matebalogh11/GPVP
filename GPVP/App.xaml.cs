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
            Current.MainWindow = new Views.MainWindow(new ViewModels.MainWindowModel());
            Current.MainWindow.Title = Displayresource.AppName;
            Current.MainWindow.Show();
        }
    }
}
