using System.Windows.Controls;
using Labb_3.Dialogs;

namespace Labb_3.Views
{
    public partial class ConfigView : UserControl
    {
        public ConfigView()
        {
            InitializeComponent();
        }

        private void CreatePackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NewPackDialog dialog = new NewPackDialog();
            dialog.Owner = System.Windows.Window.GetWindow(this);

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                packList.Items.Add(dialog.PackName);
            }
        }

        private void RemovePackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (packList.SelectedItem != null)
            {
                packList.Items.Remove(packList.SelectedItem);
            }
        }
    }
}
