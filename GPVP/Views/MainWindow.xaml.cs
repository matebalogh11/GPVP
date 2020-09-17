using GPVP.ViewModels;
using System.Windows;

namespace GPVP.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow( MainWindowModel viewModel )
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
