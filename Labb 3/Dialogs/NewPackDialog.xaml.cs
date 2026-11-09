using System.Windows;

namespace Labb_3.Dialogs
{
    public partial class NewPackDialog : Window
    {
        public NewPackDialog()
        {
            InitializeComponent();
        }

        public string PackName
        {
            get { return packNameBox.Text.Trim(); }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (packNameBox.Text.Trim() != "")
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please enter a pack name.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
