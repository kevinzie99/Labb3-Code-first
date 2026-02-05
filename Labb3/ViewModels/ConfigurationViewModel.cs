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

        
        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;

            AddQuestionCommand = new DelegateCommand(_ => AddQuestion());
            RemoveQuestionCommand = new DelegateCommand(_ => RemoveQuestion());
            OpenPackOptionsCommand = new DelegateCommand(_ => OpenPackOptions());

            LoadCategories();
        }

       
        private void LoadCategories()
        {
            if (!_categoryRepo.GetAll().Any())
            {
                _categoryRepo.Create(new Category("Matte"));
                _categoryRepo.Create(new Category("Historia"));
                _categoryRepo.Create(new Category("Sport"));
            }

            Categories.Clear();
            var cats = _categoryRepo.GetAll();
            foreach (var cat in cats)
            {
                Categories.Add(cat);
            }

            if (ActivePack != null)
            {
                SelectedCategory = Categories.FirstOrDefault(c => c.Id == ActivePack.Model.CategoryId);
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
            {
                _packRepo.Update(ActivePack.Model);
            }
        }

        
        private void ActiveQuestion_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            SavePack();
        }
    }
}
