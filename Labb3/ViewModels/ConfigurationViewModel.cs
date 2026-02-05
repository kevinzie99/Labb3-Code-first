using Labb3.Command;
using Labb3.Models;
using Labb3.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Labb3.ViewModels
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private readonly QuestionPackRepository _packRepo = new QuestionPackRepository();
        private readonly CategoryRepository _categoryRepo = new CategoryRepository();

       
        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    RaisePropertyChanged();

                    if (ActivePack != null && _selectedCategory != null)
                    {
                        ActivePack.Model.CategoryId = _selectedCategory.Id;
                        _packRepo.Update(ActivePack.Model);
                    }
                }
            }
        }

        public QuestionPackViewModel? ActivePack => _mainWindowViewModel?.ActivePack;

        private Question _activeQuestion;
        public Question ActiveQuestion
        {
            get => _activeQuestion;
            set
            {
                if (_activeQuestion != null)
                    _activeQuestion.PropertyChanged -= ActiveQuestion_PropertyChanged;

                _activeQuestion = value;

                if (_activeQuestion != null)
                    _activeQuestion.PropertyChanged += ActiveQuestion_PropertyChanged;

                RaisePropertyChanged();
            }
        }

        
        public DelegateCommand OpenPackOptionsCommand { get; }
        public DelegateCommand AddQuestionCommand { get; }
        public DelegateCommand RemoveQuestionCommand { get; }
        public DelegateCommand AddCategoryCommand { get; }
        public DelegateCommand RemoveCategoryCommand { get; }
        public DelegateCommand AddPackCommand { get; }
        public DelegateCommand RemovePackCommand { get; }

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;

            AddQuestionCommand = new DelegateCommand(_ => AddQuestion());
            RemoveQuestionCommand = new DelegateCommand(_ => RemoveQuestion());
            OpenPackOptionsCommand = new DelegateCommand(_ => OpenPackOptions());
            AddCategoryCommand = new DelegateCommand(_ => AddCategory());
            RemoveCategoryCommand = new DelegateCommand(_ => RemoveCategory());
            AddPackCommand = new DelegateCommand(_ => AddNewQuestionPack());
            RemovePackCommand = new DelegateCommand(_ => RemoveActivePack());

            LoadCategories();
        }

        private void LoadCategories()
        {
            _categoryRepo.EnsureDefaultCategories();

            Categories.Clear();
            foreach (var cat in _categoryRepo.GetAll())
            {
                Categories.Add(cat);
            }

            if (ActivePack != null)
            {
                SelectedCategory = Categories.FirstOrDefault(c => c.Id == ActivePack.Model.CategoryId);
            }
        }

        private void AddCategory()
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Ange namn på ny kategori:", "Ny kategori", "");
            if (!string.IsNullOrWhiteSpace(input))
            {
                var category = new Category(input);
                _categoryRepo.Create(category);
                Categories.Add(category);
            }
        }

        private void RemoveCategory()
        {
            if (SelectedCategory != null)
            {
                _categoryRepo.Delete(SelectedCategory.Id);
                Categories.Remove(SelectedCategory);
                SelectedCategory = null;
            }
        }

        private void AddNewQuestionPack()
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("Ange namn på nytt frågepaket:", "Nytt QuestionPack", "Nytt Pack");
            if (string.IsNullOrWhiteSpace(name)) return;

            var firstCategory = Categories.FirstOrDefault();
            var newPack = new QuestionPack(name)
            {
                CategoryId = firstCategory?.Id ?? ""
            };

          
            _packRepo.Create(newPack);

            _mainWindowViewModel?.Packs.Add(new QuestionPackViewModel(newPack));
            _mainWindowViewModel.ActivePack = _mainWindowViewModel.Packs.Last();
        }

        private void RemoveActivePack()
        {
            if (ActivePack != null)
            {
                _packRepo.Delete(ActivePack.Model.Id);
                _mainWindowViewModel?.Packs.Remove(ActivePack);
            }
        }

   
        private void AddQuestion()
        {
            if (ActivePack != null)
            {
                var newQuestion = new Question("New Question", "", "", "", "");
                ActivePack.Questions.Add(newQuestion);
                ActiveQuestion = newQuestion;
                _packRepo.Update(ActivePack.Model);
            }
        }

        private void RemoveQuestion()
        {
            if (ActivePack != null && ActiveQuestion != null)
            {
                ActivePack.Questions.Remove(ActiveQuestion);
                ActiveQuestion = ActivePack.Questions.FirstOrDefault();
                _packRepo.Update(ActivePack.Model);
            }
        }

        private void OpenPackOptions()
        {
            if (ActivePack != null)
            {
                var viewModel = new PackOptionsViewModel(ActivePack, SavePack);
                var window = new Views.PackOptionsWindow
                {
                    DataContext = viewModel
                };
                window.ShowDialog();
            }
        }

        private void SavePack()
        {
            if (ActivePack != null)
                _packRepo.Update(ActivePack.Model);
        }

        private void ActiveQuestion_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            SavePack();
        }
    }
}
