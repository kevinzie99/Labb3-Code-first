using System.ComponentModel;

namespace Labb3.Models
{
    internal class Question : INotifyPropertyChanged
    {
        public Question() { } 

        public Question(string query, string correctAnswer,
            string incorrectAnswer1, string incorrectAnswer2, string incorrectAnswer3)
        {
            _query = query;
            _correctAnswer = correctAnswer;
            _incorrectAnswers = new string[] { incorrectAnswer1, incorrectAnswer2, incorrectAnswer3 };
        }

        private string _query;
        public string Query
        {
            get => _query;
            set
            {
                if (_query != value)
                {
                    _query = value;
                    OnPropertyChanged(nameof(Query));
                }
            }
        }

        private string _correctAnswer;
        public string CorrectAnswer
        {
            get => _correctAnswer;
            set
            {
                if (_correctAnswer != value)
                {
                    _correctAnswer = value;
                    OnPropertyChanged(nameof(CorrectAnswer));
                }
            }
        }

        private string[] _incorrectAnswers;
        public string[] IncorrectAnswers
        {
            get => _incorrectAnswers;
            set
            {
                if (_incorrectAnswers != value)
                {
                    _incorrectAnswers = value;
                    OnPropertyChanged(nameof(IncorrectAnswers));
                }
            }
        }

       
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
