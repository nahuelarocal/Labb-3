using System.Windows.Controls;
using Labb_3.Dialogs;
using Labb_3.Models;

namespace Labb_3.Views
{
    public partial class ConfigView : UserControl
    {
        public ConfigView()
        {
            InitializeComponent();

            packList.Items.Add(new QuestionPack("General knowledge", "Medium", 20));
            packList.Items.Add(new QuestionPack("Programming", "Easy", 25));
            packList.Items.Add(new QuestionPack("History", "Hard", 15));
        }

        private void CreatePackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NewPackDialog dialog = new NewPackDialog();
            dialog.Owner = System.Windows.Window.GetWindow(this);

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                QuestionPack newPack = new QuestionPack(dialog.PackName, "Medium", 20);
                packList.Items.Add(newPack);
            }
        }

        private void RemovePackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (packList.SelectedItem != null)
            {
                packList.Items.Remove(packList.SelectedItem);
            }
        }

        private void PackSettingsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (packList.SelectedItem != null)
            {
                QuestionPack selectedPack = (QuestionPack)packList.SelectedItem;
                PackSettingsDialog dialog = new PackSettingsDialog(selectedPack);
                dialog.Owner = System.Windows.Window.GetWindow(this);

                if (dialog.ShowDialog() == true)
                {
                    packList.Items.Refresh();
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Select a pack first.");
            }
        }
    }
}
