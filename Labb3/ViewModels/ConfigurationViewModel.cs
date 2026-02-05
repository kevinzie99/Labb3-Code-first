using Labb3.Command;
using Labb3.Data;
using Labb3.Models;
using System;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;


namespace Labb3.ViewModels
{
    class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;

        private Question _activeQuestion;
        public QuestionPackViewModel? ActivePack => _mainWindowViewModel?.ActivePack;

        public DelegateCommand OpenPackOptionsCommand { get; }
        public DelegateCommand AddQuestionCommand { get; }
        public DelegateCommand RemoveQuestionCommand { get; }

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


        private void OpenPackOptions()
        {
            if (ActivePack != null)
            {
                var viewModel = new PackOptionsViewModel(ActivePack, SavePacks);
                var window = new Views.PackOptionsWindow
                {
                    DataContext = viewModel
                };

                window.ShowDialog(); 
            }
        }


        private void AddQuestion()
        {
            if (ActivePack != null)
            {
                var newQuestion = new Question(
                    "New Question",
                    "",
                    "",
                    "",
                    "");

                ActivePack.Questions.Add(newQuestion);
                ActiveQuestion = newQuestion; 

                SavePacks(); 
            }
        }

        private void RemoveQuestion()
        {
            if (ActivePack != null && ActiveQuestion != null)
            {
                ActivePack.Questions.Remove(ActiveQuestion);
                ActiveQuestion = ActivePack.Questions.FirstOrDefault(); 
            }

            SavePacks(); 
        }

     
        private void SavePacks()
        {
            if (_mainWindowViewModel != null)
            {
                JSON.SaveQuestionPacks(_mainWindowViewModel.Packs.Select(p => p.Model).ToList());
            }
        }

        
        private void ActiveQuestion_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            SavePacks(); 
        }
        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;

            AddQuestionCommand = new DelegateCommand(_ => AddQuestion());
            RemoveQuestionCommand = new DelegateCommand(_ => RemoveQuestion());
            OpenPackOptionsCommand = new DelegateCommand(_ => OpenPackOptions());
        }


    }
}
