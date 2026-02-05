using Labb3.Models;

namespace Labb3.ViewModels
{
    class PackOptionsViewModel : ViewModelBase
    {
        private readonly QuestionPackViewModel _pack;
        private readonly System.Action _saveAction;

        private string _packName;
        public string PackName
        {
            get => _packName;
            set
            {
                _packName = value;
                RaisePropertyChanged();
                _pack.Name = value;
                _saveAction?.Invoke();
            }
        }

        private int _selectedDifficultyIndex;
        public int SelectedDifficultyIndex
        {
            get => _selectedDifficultyIndex;
            set
            {
                _selectedDifficultyIndex = value;
                RaisePropertyChanged();
                _saveAction?.Invoke();
            }
        }

        private int _timeLimit;
        public int TimeLimit
        {
            get => _timeLimit;
            set
            {
                _timeLimit = value;
                RaisePropertyChanged();
                _pack.TimeLimitInSeconds = value;
                _saveAction?.Invoke();
            }
        }

        public PackOptionsViewModel(QuestionPackViewModel pack, System.Action saveAction)
        {
            _pack = pack;
            _saveAction = saveAction;

            PackName = pack.Name;
            TimeLimit = pack.TimeLimitInSeconds;
            SelectedDifficultyIndex = 0; 
        }
    }
}