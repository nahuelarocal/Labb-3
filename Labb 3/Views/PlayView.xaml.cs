using System.Windows;
using System.Windows.Controls;
using Labb_3.Models;

namespace Labb_3.Views
{
    public partial class PlayView : UserControl
    {
        private QuestionPack? currentPack;
        private int currentQuestionIndex;
        private int rightAnswers;
        private bool answerSelected;

        public PlayView()
        {
            InitializeComponent();
        }

        public void StartQuiz(QuestionPack pack)
        {
            currentPack = pack;
            currentQuestionIndex = 0;
            rightAnswers = 0;
            ShowQuestion();
        }

        private void ShowQuestion()
        {
            if (currentPack == null)
            {
                return;
            }

            if (currentQuestionIndex >= currentPack.Questions.Count)
            {
                questionNumberText.Text = "QUIZ COMPLETE";
                scoreText.Text = "";
                questionText.Text = "You got " + rightAnswers + " out of " + currentPack.Questions.Count + " right!";
                feedbackText.Text = "Nice work!";
                HideAnswerButtons();
                nextButton.Content = "Back to edit";
                nextButton.Visibility = Visibility.Visible;
                return;
            }

            Question question = currentPack.Questions[currentQuestionIndex];
            questionNumberText.Text = "QUESTION " + (currentQuestionIndex + 1) + " OF " + currentPack.Questions.Count;
            scoreText.Text = "Score: " + rightAnswers;
            questionText.Text = question.Text;
            answer1Button.Content = question.Answer1;
            answer2Button.Content = question.Answer2;
            answer3Button.Content = question.Answer3;
            answer4Button.Content = question.Answer4;
            answer1Button.IsEnabled = true;
            answer2Button.IsEnabled = true;
            answer3Button.IsEnabled = true;
            answer4Button.IsEnabled = true;
            answer1Button.Visibility = Visibility.Visible;
            answer2Button.Visibility = Visibility.Visible;
            answer3Button.Visibility = Visibility.Visible;
            answer4Button.Visibility = Visibility.Visible;
            feedbackText.Text = "";
            nextButton.Visibility = Visibility.Collapsed;
            answerSelected = false;
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPack == null || answerSelected)
            {
                return;
            }

            Button clickedButton = (Button)sender;
            int answerNumber = 1;

            if (clickedButton == answer2Button)
            {
                answerNumber = 2;
            }
            else if (clickedButton == answer3Button)
            {
                answerNumber = 3;
            }
            else if (clickedButton == answer4Button)
            {
                answerNumber = 4;
            }

            Question question = currentPack.Questions[currentQuestionIndex];

            if (answerNumber == question.CorrectAnswer)
            {
                rightAnswers++;
                feedbackText.Text = "Correct!";
            }
            else
            {
                string correctAnswer = question.Answer1;

                if (question.CorrectAnswer == 2)
                {
                    correctAnswer = question.Answer2;
                }
                else if (question.CorrectAnswer == 3)
                {
                    correctAnswer = question.Answer3;
                }
                else if (question.CorrectAnswer == 4)
                {
                    correctAnswer = question.Answer4;
                }

                feedbackText.Text = "Not quite. The answer was: " + correctAnswer;
            }

            answerSelected = true;
            answer1Button.IsEnabled = false;
            answer2Button.IsEnabled = false;
            answer3Button.IsEnabled = false;
            answer4Button.IsEnabled = false;
            scoreText.Text = "Score: " + rightAnswers;

            if (currentQuestionIndex == currentPack.Questions.Count - 1)
            {
                nextButton.Content = "See result";
            }
            else
            {
                nextButton.Content = "Next question";
            }

            nextButton.Visibility = Visibility.Visible;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPack != null && currentQuestionIndex >= currentPack.Questions.Count)
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.ShowConfigView();
            }
            else
            {
                currentQuestionIndex++;
                ShowQuestion();
            }
        }

        private void HideAnswerButtons()
        {
            answer1Button.Visibility = Visibility.Collapsed;
            answer2Button.Visibility = Visibility.Collapsed;
            answer3Button.Visibility = Visibility.Collapsed;
            answer4Button.Visibility = Visibility.Collapsed;
        }
    }
}
