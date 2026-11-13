using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using Labb_3.Dialogs;
using Labb_3.Models;

namespace Labb_3.Views
{
    public partial class ConfigView : UserControl
    {
        public QuestionPack? SelectedPack
        {
            get { return packList.SelectedItem as QuestionPack; }
        }

        public ConfigView()
        {
            InitializeComponent();

            QuestionPack generalPack = new QuestionPack("General knowledge", "Medium", 5);
            generalPack.Questions.Add(new Question("What is the capital of Sweden?", "Oslo", "Stockholm", "Copenhagen", "Helsinki", 2));
            packList.Items.Add(generalPack);

            QuestionPack programmingPack = new QuestionPack("Programming", "Easy", 5);
            programmingPack.Questions.Add(new Question("Which language is used with WPF?", "C#", "HTML", "Python", "SQL", 1));
            packList.Items.Add(programmingPack);

            packList.Items.Add(new QuestionPack("History", "Hard", 5));
            packList.SelectedIndex = 0;
        }

        public async Task<bool> LoadPacksAsync()
        {
            string filePath = GetPacksFilePath();

            if (!File.Exists(filePath))
            {
                return false;
            }

            try
            {
                string json = await File.ReadAllTextAsync(filePath);
                List<QuestionPack>? savedPacks = JsonSerializer.Deserialize<List<QuestionPack>>(json);

                if (savedPacks == null)
                {
                    return false;
                }

                packList.Items.Clear();

                foreach (QuestionPack pack in savedPacks)
                {
                    packList.Items.Add(pack);
                }

                if (packList.Items.Count > 0)
                {
                    packList.SelectedIndex = 0;
                }
                else
                {
                    questionList.Items.Clear();
                }

                return true;
            }
            catch (System.Exception error)
            {
                System.Windows.MessageBox.Show("Could not load packs: " + error.Message);
                return false;
            }
        }

        private async void SavePacksButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await SavePacksAsync();
        }

        public async Task SavePacksAsync()
        {
            try
            {
                List<QuestionPack> packs = new List<QuestionPack>();

                foreach (QuestionPack pack in packList.Items)
                {
                    packs.Add(pack);
                }

                string folder = GetPacksFolder();
                Directory.CreateDirectory(folder);

                string json = JsonSerializer.Serialize(packs, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                await File.WriteAllTextAsync(GetPacksFilePath(), json);
                System.Windows.MessageBox.Show("Packs saved.");
            }
            catch (System.Exception error)
            {
                System.Windows.MessageBox.Show("Could not save packs: " + error.Message);
            }
        }

        private async void LoadPacksButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            bool loaded = await LoadPacksAsync();

            if (loaded)
            {
                System.Windows.MessageBox.Show("Packs loaded.");
            }
            else
            {
                System.Windows.MessageBox.Show("No saved packs were found.");
            }
        }

        private string GetPacksFolder()
        {
            string appDataFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(appDataFolder, "QuizConfigurator");
        }

        private string GetPacksFilePath()
        {
            return Path.Combine(GetPacksFolder(), "questionPacks.json");
        }

        private void CreatePackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NewPackDialog dialog = new NewPackDialog();
            dialog.Owner = System.Windows.Window.GetWindow(this);

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                QuestionPack newPack = new QuestionPack(dialog.PackName, "Medium", 5);
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
            OpenPackSettings();
        }

        public void OpenPackSettings()
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

        public void RefreshQuestions()
        {
            ShowQuestions();
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
