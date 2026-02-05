namespace Labb3.ViewModels
{
    class NewPackViewModel : ViewModelBase
    {
        private string _packName = "";
        public string PackName
        {
            get => _packName;
            set
            {
                _packName = value;
                RaisePropertyChanged();
            }
        }

        private int _selectedDifficultyIndex = 0;
        public int SelectedDifficultyIndex
        {
            get => _selectedDifficultyIndex;
            set
            {
                _selectedDifficultyIndex = value;
                RaisePropertyChanged();
            }
        }

        private int _timeLimit = 30;
        public int TimeLimit
        {
            get => _timeLimit;
            set
            {
                _timeLimit = value;
                RaisePropertyChanged();
            }
        }
    }
}