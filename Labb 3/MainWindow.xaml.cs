using System.Text;
using System.Windows;
using Labb_3.Models;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Labb_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
