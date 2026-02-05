using Labb3.Models;
using Labb3.Services;
using Labb3.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Labb3;

public partial class MainWindow : Window
{
    private int count = 1;
    public MainWindow()
    {
        InitializeComponent();
        

        DataContext = new MainWindowViewModel();


        var repo = new QuestionPackRepository();
        var allPacks = repo.GetAll();

        foreach (var pack in allPacks)
        {
            Console.WriteLine(pack.Name);
        }
    }

   
}