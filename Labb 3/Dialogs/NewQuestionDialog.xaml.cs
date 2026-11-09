using System.Windows;
using Labb_3.Models;

namespace Labb_3.Dialogs
{
    public partial class NewQuestionDialog : Window
    {
        public Question Question { get; private set; }

        public NewQuestionDialog()
        {
            InitializeComponent();
            Question = new Question("", "", "", "", "", 1);
        }

        public void LoadQuestion(Question question)
        {
            Question = question;
            titleText.Text = "Edit question";
            questionTextBox.Text = question.Text;
            answer1Box.Text = question.Answer1;
            answer2Box.Text = question.Answer2;
            answer3Box.Text = question.Answer3;
            answer4Box.Text = question.Answer4;
            correctAnswerBox.SelectedIndex = question.CorrectAnswer - 1;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (questionTextBox.Text.Trim() != "" &&
                answer1Box.Text.Trim() != "" &&
                answer2Box.Text.Trim() != "" &&
                answer3Box.Text.Trim() != "" &&
                answer4Box.Text.Trim() != "")
            {
                Question.Text = questionTextBox.Text.Trim();
                Question.Answer1 = answer1Box.Text.Trim();
                Question.Answer2 = answer2Box.Text.Trim();
                Question.Answer3 = answer3Box.Text.Trim();
                Question.Answer4 = answer4Box.Text.Trim();
                Question.CorrectAnswer = correctAnswerBox.SelectedIndex + 1;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Enter the question and all four answers.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
