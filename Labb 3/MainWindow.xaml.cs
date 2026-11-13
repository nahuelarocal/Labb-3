using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Labb_3.Models;
using Labb_3.Dialogs;

namespace Labb_3
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await configView.LoadPacksAsync();
        }

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                await SavePacksAsync();
                e.Handled = true;
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.O)
            {
                await LoadPacksAsync();
                e.Handled = true;
            }
        }

        public async Task SavePacksAsync()
        {
            await configView.SavePacksAsync();
        }

        public async Task LoadPacksAsync()
        {
            bool loaded = await configView.LoadPacksAsync();

            if (loaded)
            {
                MessageBox.Show("Packs loaded.");
            }
            else
            {
                MessageBox.Show("No saved packs were found.");
            }
        }

        public void ShowPackSettings()
        {
            configView.OpenPackSettings();
        }

        public void ImportQuestions()
        {
            QuestionPack? selectedPack = configView.SelectedPack;

            if (selectedPack == null)
            {
                MessageBox.Show("Select a question pack first.");
                return;
            }

            ImportQuestionsDialog dialog = new ImportQuestionsDialog();
            dialog.Owner = this;

            if (dialog.ShowDialog() == true)
            {
                foreach (Question question in dialog.ImportedQuestions)
                {
                    selectedPack.Questions.Add(question);
                }

                configView.RefreshQuestions();
                MessageBox.Show(dialog.ImportedQuestions.Count + " questions imported.");
            }
        }

        public void ShowConfigView()
        {
            configView.Visibility = Visibility.Visible;
            playView.Visibility = Visibility.Collapsed;
        }

        public void ShowPlayView()
        {
            QuestionPack? selectedPack = configView.SelectedPack;

            if (selectedPack == null)
            {
                MessageBox.Show("Select a question pack first.");
                return;
            }

            if (selectedPack.Questions.Count == 0)
            {
                MessageBox.Show("Add at least one question to this pack first.");
                return;
            }

            playView.StartQuiz(selectedPack);
            configView.Visibility = Visibility.Collapsed;
            playView.Visibility = Visibility.Visible;
        }
    }
}

