using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Labb_3.Models;

namespace Labb_3.Views
{
    public partial class PlayView : UserControl
    {
        private QuestionPack? currentPack;
        private List<Question> quizQuestions;
        private int currentQuestionIndex;
        private int rightAnswers;
        private int secondsLeft;
        private bool answerSelected;
        private DispatcherTimer questionTimer;

        public PlayView()
        {
            InitializeComponent();

            quizQuestions = new List<Question>();

            questionTimer = new DispatcherTimer();
            questionTimer.Interval = TimeSpan.FromSeconds(1);
            questionTimer.Tick += QuestionTimer_Tick;
        }

        public void StartQuiz(QuestionPack pack)
        {
            currentPack = pack;
            quizQuestions = new List<Question>(pack.Questions);

            if (pack.RandomOrder)
            {
                Random random = new Random();

                for (int i = quizQuestions.Count - 1; i > 0; i--)
                {
                    int otherIndex = random.Next(i + 1);
                    Question savedQuestion = quizQuestions[i];
                    quizQuestions[i] = quizQuestions[otherIndex];
                    quizQuestions[otherIndex] = savedQuestion;
                }
            }

            currentQuestionIndex = 0;
            rightAnswers = 0;
            questionTimer.Stop();
            ShowQuestion();
        }

        private void ShowQuestion()
        {
            if (currentPack == null)
            {
                return;
            }

            if (currentQuestionIndex >= quizQuestions.Count)
            {
                questionTimer.Stop();
                questionNumberText.Text = "QUIZ COMPLETE";
                scoreText.Text = "";
                timerText.Text = "";
                questionText.Text = "You got " + rightAnswers + " out of " + quizQuestions.Count + " right!";
                feedbackText.Text = "Nice work!";
                HideAnswerButtons();
                nextButton.Content = "Back to edit";
                nextButton.Visibility = Visibility.Visible;
                return;
            }

            Question question = quizQuestions[currentQuestionIndex];
            questionTimer.Stop();
            questionNumberText.Text = "QUESTION " + (currentQuestionIndex + 1) + " OF " + quizQuestions.Count;
            scoreText.Text = "Score: " + rightAnswers;
            secondsLeft = currentPack.SecondsPerQuestion;
            timerText.Text = "Time left: " + secondsLeft + " seconds";
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
            questionTimer.Start();
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPack == null || answerSelected)
            {
                return;
            }

            questionTimer.Stop();

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

            Question question = quizQuestions[currentQuestionIndex];

            if (answerNumber == question.CorrectAnswer)
            {
                rightAnswers++;
                feedbackText.Text = "Correct!";
            }
            else
            {
                feedbackText.Text = "Not quite. The answer was: " + GetCorrectAnswer(question);
            }

            FinishQuestion();
        }

        private void QuestionTimer_Tick(object? sender, EventArgs e)
        {
            if (currentPack == null || answerSelected)
            {
                questionTimer.Stop();
                return;
            }

            secondsLeft--;
            timerText.Text = "Time left: " + secondsLeft + " seconds";

            if (secondsLeft <= 0)
            {
                questionTimer.Stop();
                Question question = quizQuestions[currentQuestionIndex];
                feedbackText.Text = "Time's up. The answer was: " + GetCorrectAnswer(question);
                FinishQuestion();
            }
        }

        private string GetCorrectAnswer(Question question)
        {
            if (question.CorrectAnswer == 1)
            {
                return question.Answer1;
            }
            else if (question.CorrectAnswer == 2)
            {
                return question.Answer2;
            }
            else if (question.CorrectAnswer == 3)
            {
                return question.Answer3;
            }

            return question.Answer4;
        }

        private void FinishQuestion()
        {
            answerSelected = true;
            answer1Button.IsEnabled = false;
            answer2Button.IsEnabled = false;
            answer3Button.IsEnabled = false;
            answer4Button.IsEnabled = false;
            scoreText.Text = "Score: " + rightAnswers;

            if (currentPack != null && currentQuestionIndex == quizQuestions.Count - 1)
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
            questionTimer.Stop();

            if (currentPack != null && currentQuestionIndex >= quizQuestions.Count)
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
