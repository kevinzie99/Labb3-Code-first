using Labb3.Models;
using Labb3.Services;
using System.Collections.ObjectModel;
using System.Linq;
using Labb3.Command;

namespace Labb3.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private readonly QuestionPackRepository _packRepo = new QuestionPackRepository();
        private readonly CategoryRepository _categoryRepo = new CategoryRepository();

        private object _currentViewModel;
        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<QuestionPackViewModel> Packs { get; } = new();
        public DelegateCommand ShowConfigurationViewCommand { get; }
        public DelegateCommand ShowPlayerViewCommand { get; }
        public DelegateCommand NewPackCommand { get; }

        private QuestionPackViewModel _activePack;
        public DelegateCommand ToggleFullscreenCommand { get; }
        public DelegateCommand SelectPackCommand { get; }
        public DelegateCommand DeletePackCommand { get; }
        public DelegateCommand ExitCommand { get; }

        public QuestionPackViewModel ActivePack
        {
            get => _activePack;
            set
            {
                _activePack = value;
                RaisePropertyChanged();
                PlayerViewModel?.RaisePropertyChanged(nameof(PlayerViewModel.ActivePack));
                ConfigurationViewModel?.RaisePropertyChanged(nameof(ConfigurationViewModel.ActivePack));
            }
        }

        private void CreateNewPack()
        {
            var viewModel = new NewPackViewModel();
            var window = new Views.NewPackWindow
            {
                DataContext = viewModel
            };

            if (window.ShowDialog() == true)
            {
                var newPack = new QuestionPack(viewModel.PackName)
                {
                    TimeLimitInSeconds = viewModel.TimeLimit
                };

               
                _packRepo.Create(newPack);

                var newPackViewModel = new QuestionPackViewModel(newPack);
                Packs.Add(newPackViewModel);
                ActivePack = newPackViewModel;

                var defaultQuestion = new Question("New Question", "", "", "", "");
                newPackViewModel.Questions.Add(defaultQuestion);
                ConfigurationViewModel.ActiveQuestion = defaultQuestion;

                
                _packRepo.Update(newPack);
            }
        }

        private void ToggleFullscreen(object? obj = null)
        {
            var mainWindow = System.Windows.Application.Current.MainWindow;
            if (mainWindow != null)
            {
                if (mainWindow.WindowStyle == System.Windows.WindowStyle.None)
                {
                    mainWindow.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                }
                else
                {
                    mainWindow.WindowStyle = System.Windows.WindowStyle.None;
                }
            }
        }

        private void ExitApplication()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void SelectPack(object? parameter)
        {
            if (parameter is QuestionPackViewModel selectedPack)
            {
                ActivePack = selectedPack;

                if (ActivePack.Questions.Count > 0)
                {
                    ConfigurationViewModel.ActiveQuestion = ActivePack.Questions[0];
                }
            }
        }

        private void DeleteCurrentPack()
        {
            if (ActivePack == null || Packs.Count <= 1)
            {
                System.Windows.MessageBox.Show(
                    "Cannot delete the last question pack!",
                    "Delete Failed",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);
                return;
            }

            var result = System.Windows.MessageBox.Show(
                $"Are you sure you want to delete '{ActivePack.Name}'?",
                "Confirm Delete",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                var packToDelete = ActivePack;
                var currentIndex = Packs.IndexOf(packToDelete);

                if (currentIndex > 0)
                    ActivePack = Packs[currentIndex - 1];
                else
                    ActivePack = Packs[1];

                
                _packRepo.Delete(packToDelete.Model.Id);
                Packs.Remove(packToDelete);

                if (ActivePack.Questions.Count > 0)
                {
                    ConfigurationViewModel.ActiveQuestion = ActivePack.Questions[0];
                }
            }
        }

        public PlayerViewModel? PlayerViewModel { get; private set; }
        public ConfigurationViewModel? ConfigurationViewModel { get; private set; }

        public MainWindowViewModel()
        {
            ConfigurationViewModel = new ConfigurationViewModel(this);
            PlayerViewModel = new PlayerViewModel(this);

            ShowConfigurationViewCommand = new DelegateCommand(obj => CurrentViewModel = ConfigurationViewModel);
            ShowPlayerViewCommand = new DelegateCommand(obj =>
            {
                CurrentViewModel = PlayerViewModel;
                PlayerViewModel?.LoadQuestion(0);
            });

          
            _categoryRepo.EnsureDefaultCategories();

           
            var loadedPacks = _packRepo.GetAll();
            foreach (var pack in loadedPacks)
            {
                Packs.Add(new QuestionPackViewModel(pack));
            }

            
            if (Packs.Count == 0)
            {
                var defaultPack = new QuestionPack("Default Question Pack");
                _packRepo.Create(defaultPack);
                Packs.Add(new QuestionPackViewModel(defaultPack));
            }

            ActivePack = Packs[0];

            if (ActivePack.Questions.Count > 0)
            {
                ConfigurationViewModel.ActiveQuestion = ActivePack.Questions[0];
            }

            NewPackCommand = new DelegateCommand(_ => CreateNewPack());
            ToggleFullscreenCommand = new DelegateCommand(_ => ToggleFullscreen());
            SelectPackCommand = new DelegateCommand(obj => SelectPack(obj));
            DeletePackCommand = new DelegateCommand(_ => DeleteCurrentPack());
            ExitCommand = new DelegateCommand(_ => ExitApplication());

            CurrentViewModel = ConfigurationViewModel;
        }
    }
}