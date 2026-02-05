using Labb3.Command;
using Labb3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Labb3.ViewModels
{
    class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;

        public DelegateCommand SetPackNameCommand { get; }
        public DelegateCommand AnswerCommand { get; }
        public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }

        private DispatcherTimer _quizTimer;

        public DelegateCommand RestartQuizCommand { get; }

        private string _demoText;

        public string DemoText
        {
            get { return _demoText; }
            set
            {
                _demoText = value;
                RaisePropertyChanged();
                SetPackNameCommand.RaiseCanExecuteChanged();
            }
        }

        private void RestartQuiz(object? obj)
        {
            Score = 0;
            IsQuizFinished = false;
            LoadQuestion(0);
            StartTimer(ActivePack?.TimeLimitInSeconds ?? 30);
        }

        private bool CanSetPackName(object? arg)
        {
            return DemoText.Length > 0;
        }

        private void SetPackName(object? obj)
        {
            ActivePack.Name = DemoText;
        }

        private Question? _currentQuestion;
        public Question? CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ShuffledAnswers));
            }
        }

       
        private int _score;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                RaisePropertyChanged();
            }
        }

      
        private bool _isQuizFinished;
        public bool IsQuizFinished
        {
            get => _isQuizFinished;
            set
            {
                _isQuizFinished = value;
                RaisePropertyChanged();
            }
        }

        public void Initiate()
        {
            Score = 0;
            IsQuizFinished = false;
            LoadQuestion(0);
            StartTimer(ActivePack?.TimeLimitInSeconds ?? 30);
        }

        private int _timeLeft;

        public int TimeLeft
        {
            get { return _timeLeft; }
            set
            {
                _timeLeft = value;
                RaisePropertyChanged();
            }
        }

        public void StartTimer(int seconds)
        {
            _quizTimer?.Stop(); 

            TimeLeft = seconds;

            _quizTimer = new DispatcherTimer();
            _quizTimer.Interval = TimeSpan.FromSeconds(1);
            _quizTimer.Tick += QuizTimer_Tick;

            _quizTimer.Start();
        }

        private void QuizTimer_Tick(object sender, EventArgs e)
        {
            if (TimeLeft > 0)
            {
                TimeLeft--;
            }
            else
            {
                _quizTimer.Stop();
                
                if (CurrentQuestionIndex < TotalQuestions - 1)
                {
                    LoadQuestion(CurrentQuestionIndex + 1);
                    StartTimer(ActivePack?.TimeLimitInSeconds ?? 30);
                }
                else
                {
                    IsQuizFinished = true;
                }
            }
        }

        private int _currentQuestionIndex;
        public int CurrentQuestionIndex
        {
            get => _currentQuestionIndex;
            set
            {
                _currentQuestionIndex = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(QuestionNumber));
                RaisePropertyChanged(nameof(TotalQuestions));
            }
        }

        public int QuestionNumber => CurrentQuestionIndex + 1;
        public int TotalQuestions => ActivePack?.Questions.Count ?? 0;

        private List<string> _shuffledAnswers;

        public List<string> ShuffledAnswers
        {
            get
            {
                if (_shuffledAnswers != null)
                    return _shuffledAnswers;

                if (CurrentQuestion == null)
                    return new List<string> { "", "", "", "" };

                var answers = new List<string> { CurrentQuestion.CorrectAnswer };

                if (CurrentQuestion.IncorrectAnswers != null && CurrentQuestion.IncorrectAnswers.Length >= 3)
                {
                    answers.AddRange(CurrentQuestion.IncorrectAnswers.Take(3));
                }

                var random = new Random();
                _shuffledAnswers = answers.OrderBy(x => random.Next()).ToList();

                return _shuffledAnswers;
            }
        }

        public void LoadQuestion(int index)
        {
            if (ActivePack != null && ActivePack.Questions != null &&
                index >= 0 && index < ActivePack.Questions.Count)
            {
                CurrentQuestionIndex = index;
                CurrentQuestion = ActivePack.Questions[index];
                _shuffledAnswers = null; 
                RaisePropertyChanged(nameof(ShuffledAnswers));
            }
        }

   
        private string _selectedAnswer;
        public string SelectedAnswer
        {
            get => _selectedAnswer;
            set
            {
                _selectedAnswer = value;
                RaisePropertyChanged();
            }
        }

        
        private void CheckAnswer(object? parameter)
        {
            if (parameter is string selectedAnswer && CurrentQuestion != null)
            {
                _quizTimer?.Stop();
                SelectedAnswer = selectedAnswer;

         
                if (selectedAnswer == CurrentQuestion.CorrectAnswer)
                {
                    Score++;
                }



                var timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += (s, e) =>
                {
                    timer.Stop();
                    SelectedAnswer = null;

                    if (CurrentQuestionIndex < TotalQuestions - 1)
                    {
                        LoadQuestion(CurrentQuestionIndex + 1);
                        StartTimer(ActivePack?.TimeLimitInSeconds ?? 30);
                    }
                    else
                    {
                        IsQuizFinished = true;
                    }
                };
                timer.Start();
            }
        }



        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this._mainWindowViewModel = mainWindowViewModel;
            SetPackNameCommand = new DelegateCommand(SetPackName, CanSetPackName);
            AnswerCommand = new DelegateCommand(CheckAnswer);
            RestartQuizCommand = new DelegateCommand(RestartQuiz);
            DemoText = string.Empty;
            Score = 0;
            IsQuizFinished = false;
        }
    }
}