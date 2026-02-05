using Labb3.Models;
using Labb3.Services;
using Labb3.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Labb3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            
            InitializeDatabase();

            DataContext = new MainWindowViewModel();
        }

        private void InitializeDatabase()
        {
            var categoryRepo = new CategoryRepository();
            var packRepo = new QuestionPackRepository();

          
            if (!categoryRepo.GetAll().Any())
            {
                categoryRepo.Create(new Category("Matte"));
                categoryRepo.Create(new Category("Historia"));
                categoryRepo.Create(new Category("Sport"));
            }

          
            if (!packRepo.GetAll().Any())
            {
                var firstCategory = categoryRepo.GetAll().First();
                var pack = new QuestionPack("Första Testpack")
                {
                    Difficulty = Difficulty.Medium,
                    TimeLimitInSeconds = 30,
                    Questions = new List<Question>
                    {
                        new Question(
                            "Vad är 2 + 2?",
                            "4",
                            "3",
                            "5",
                            "6"
                        )
                    },
                    CategoryId = firstCategory.Id
                };
                packRepo.Create(pack);
            }
        }
    }
}