using System.Windows;
using System.Windows.Controls;
using Labb_3;

namespace Labb_3.Views
{
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
        }

        private async void SavePacksMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            await mainWindow.SavePacksAsync();
        }

        private async void LoadPacksMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            await mainWindow.LoadPacksAsync();
        }

        private void ConfigureMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.ShowConfigView();
        }

        private void PackOptionsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.ShowPackSettings();
        }

        private void PlayMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.ShowPlayView();
        }
    }
}
