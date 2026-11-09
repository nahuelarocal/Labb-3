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

            QuestionPack generalPack = new QuestionPack("General knowledge", "Medium", 20);
            generalPack.Questions.Add(new Question("What is the capital of Sweden?", "Oslo", "Stockholm", "Copenhagen", "Helsinki", 2));
            packList.Items.Add(generalPack);

            QuestionPack programmingPack = new QuestionPack("Programming", "Easy", 25);
            programmingPack.Questions.Add(new Question("Which language is used with WPF?", "C#", "HTML", "Python", "SQL", 1));
            packList.Items.Add(programmingPack);

            packList.Items.Add(new QuestionPack("History", "Hard", 15));
            packList.SelectedIndex = 0;
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
                packList.SelectedItem = newPack;
            }
        }

        private void RemovePackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (packList.SelectedItem != null)
            {
                packList.Items.Remove(packList.SelectedItem);
                ShowQuestions();
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

        private void PackList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowQuestions();
        }

        private void ShowQuestions()
        {
            questionList.Items.Clear();

            if (packList.SelectedItem != null)
            {
                QuestionPack selectedPack = (QuestionPack)packList.SelectedItem;

                foreach (Question question in selectedPack.Questions)
                {
                    questionList.Items.Add(question);
                }
            }
        }

        private void AddQuestionButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (packList.SelectedItem != null)
            {
                QuestionPack selectedPack = (QuestionPack)packList.SelectedItem;
                NewQuestionDialog dialog = new NewQuestionDialog();
                dialog.Owner = System.Windows.Window.GetWindow(this);

                if (dialog.ShowDialog() == true)
                {
                    selectedPack.Questions.Add(dialog.Question);
                    ShowQuestions();
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Select a pack first.");
            }
        }

        private void EditQuestionButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (questionList.SelectedItem != null)
            {
                Question selectedQuestion = (Question)questionList.SelectedItem;
                NewQuestionDialog dialog = new NewQuestionDialog();
                dialog.LoadQuestion(selectedQuestion);
                dialog.Owner = System.Windows.Window.GetWindow(this);

                if (dialog.ShowDialog() == true)
                {
                    ShowQuestions();
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Select a question first.");
            }
        }

        private void RemoveQuestionButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (packList.SelectedItem != null && questionList.SelectedItem != null)
            {
                QuestionPack selectedPack = (QuestionPack)packList.SelectedItem;
                Question selectedQuestion = (Question)questionList.SelectedItem;
                selectedPack.Questions.Remove(selectedQuestion);
                ShowQuestions();
            }
        }
    }
}
