using System.Windows;
using Labb_3.Models;

namespace Labb_3.Dialogs
{
    public partial class PackSettingsDialog : Window
    {
        private QuestionPack selectedPack;

        public PackSettingsDialog(QuestionPack pack)
        {
            InitializeComponent();

            selectedPack = pack;
            packNameBox.Text = selectedPack.Name;
            difficultyBox.SelectedValue = selectedPack.Difficulty;
            secondsBox.Text = selectedPack.SecondsPerQuestion.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int seconds;

            if (packNameBox.Text.Trim() != "" && int.TryParse(secondsBox.Text, out seconds) && seconds > 0)
            {
                selectedPack.Name = packNameBox.Text;
                selectedPack.Difficulty = difficultyBox.Text;
                selectedPack.SecondsPerQuestion = seconds;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Enter a pack name and a number of seconds greater than zero.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
