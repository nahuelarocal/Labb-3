using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using Labb_3.Models;

namespace Labb_3.Dialogs
{
    public partial class ImportQuestionsDialog : Window
    {
        private HttpClient client = new HttpClient();
        public List<Question> ImportedQuestions { get; set; }

        public ImportQuestionsDialog()
        {
            InitializeComponent();
            ImportedQuestions = new List<Question>();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            importButton.IsEnabled = false;
            statusText.Text = "Loading categories...";

            try
            {
                string json = await client.GetStringAsync("https://opentdb.com/api_category.php");
                CategoryResponse? response = JsonSerializer.Deserialize<CategoryResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (response != null)
                {
                    categoryBox.ItemsSource = response.Categories;
                    categoryBox.SelectedIndex = 0;
                    statusText.Text = "Choose a category and import questions.";
                    importButton.IsEnabled = true;
                }
            }
            catch (Exception error)
            {
                statusText.Text = "Could not load categories. Check your internet connection.";
                MessageBox.Show("Could not connect to OpenTDB: " + error.Message);
            }
        }

        private async void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            int amount;

            if (!int.TryParse(amountBox.Text, out amount) || amount < 1 || amount > 50)
            {
                MessageBox.Show("Enter a number between 1 and 50.");
                return;
            }

            if (categoryBox.SelectedValue == null)
            {
                MessageBox.Show("Choose a category first.");
                return;
            }

            int categoryId = (int)categoryBox.SelectedValue;
            string difficulty = "easy";

            if (difficultyBox.SelectedIndex == 1)
            {
                difficulty = "medium";
            }
            else if (difficultyBox.SelectedIndex == 2)
            {
                difficulty = "hard";
            }

            string url = "https://opentdb.com/api.php?amount=" + amount
                         + "&category=" + categoryId
                         + "&difficulty=" + difficulty
                         + "&type=multiple&encode=url3986";

            importButton.IsEnabled = false;
            statusText.Text = "Loading questions...";

            try
            {
                string json = await client.GetStringAsync(url);
                TriviaResponse? response = JsonSerializer.Deserialize<TriviaResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (response == null || response.ResponseCode != 0 || response.Results.Count == 0)
                {
                    MessageBox.Show("No questions were found. Try another category or difficulty.");
                    importButton.IsEnabled = true;
                    statusText.Text = "Choose a category and import questions.";
                    return;
                }

                Random random = new Random();

                foreach (TriviaItem item in response.Results)
                {
                    string questionText = Uri.UnescapeDataString(item.Question);
                    string correctAnswer = Uri.UnescapeDataString(item.CorrectAnswer);
                    List<string> answers = new List<string>();
                    answers.Add(correctAnswer);

                    foreach (string answer in item.IncorrectAnswers)
                    {
                        answers.Add(Uri.UnescapeDataString(answer));
                    }

                    for (int i = answers.Count - 1; i > 0; i--)
                    {
                        int otherIndex = random.Next(i + 1);
                        string savedAnswer = answers[i];
                        answers[i] = answers[otherIndex];
                        answers[otherIndex] = savedAnswer;
                    }

                    int correctAnswerNumber = answers.IndexOf(correctAnswer) + 1;
                    Question question = new Question(questionText, answers[0], answers[1],
                        answers[2], answers[3], correctAnswerNumber);
                    ImportedQuestions.Add(question);
                }

                DialogResult = true;
            }
            catch (Exception error)
            {
                MessageBox.Show("Could not import questions: " + error.Message);
                importButton.IsEnabled = true;
                statusText.Text = "Choose a category and import questions.";
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public class CategoryResponse
        {
            [JsonPropertyName("trivia_categories")]
            public List<ApiCategory> Categories { get; set; } = new List<ApiCategory>();
        }

        public class ApiCategory
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
        }

        public class TriviaResponse
        {
            [JsonPropertyName("response_code")]
            public int ResponseCode { get; set; }

            public List<TriviaItem> Results { get; set; } = new List<TriviaItem>();
        }

        public class TriviaItem
        {
            public string Question { get; set; } = "";

            [JsonPropertyName("correct_answer")]
            public string CorrectAnswer { get; set; } = "";

            [JsonPropertyName("incorrect_answers")]
            public List<string> IncorrectAnswers { get; set; } = new List<string>();
        }
    }
}
