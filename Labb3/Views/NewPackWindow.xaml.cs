using System.Windows;

namespace Labb3.Views
{
    public partial class NewPackWindow : Window
    {
        public NewPackWindow()
        {
            InitializeComponent();
        }

        private void CreatePack_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}