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
            if (DataContext is ViewModels.NewPackViewModel vm)
            {
                if (string.IsNullOrWhiteSpace(vm.PackName))
                {
                    MessageBox.Show("Please enter a pack name.", "Invalid Name",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DialogResult = true;
                Close();
            }
        }
    }
}