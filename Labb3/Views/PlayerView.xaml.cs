using Labb3.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Labb3.Views
{
    public partial class PlayerView : UserControl
    {
        public PlayerView()
        {
            InitializeComponent();
            Loaded += PlayerView_loaded;
            DataContextChanged += PlayerView_DataContextChanged;
        }

        private void PlayerView_loaded(object sender, EventArgs e)
        {
            if (DataContext is PlayerViewModel vm)
            {
                vm.Initiate();
            }
        }

        private void PlayerView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is PlayerViewModel oldVm)
            {
                oldVm.PropertyChanged -= ViewModel_PropertyChanged;
            }

            if (e.NewValue is PlayerViewModel newVm)
            {
                newVm.PropertyChanged += ViewModel_PropertyChanged;
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PlayerViewModel.IsQuizFinished))
            {
                if (DataContext is PlayerViewModel vm)
                {
                    if (vm.IsQuizFinished)
                    {
                        QuizPanel.Visibility = Visibility.Collapsed;
                        TimerText.Visibility = Visibility.Collapsed;
                        ResultsPanel.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        QuizPanel.Visibility = Visibility.Visible;
                        TimerText.Visibility = Visibility.Visible;
                        ResultsPanel.Visibility = Visibility.Collapsed;
                    }
                }
            }

            
            if (e.PropertyName == nameof(PlayerViewModel.SelectedAnswer))
            {
                if (DataContext is PlayerViewModel vm && vm.SelectedAnswer != null)
                {
               
                    Button1.Background = SystemColors.ControlBrush;
                    Button2.Background = SystemColors.ControlBrush;
                    Button3.Background = SystemColors.ControlBrush;
                    Button4.Background = SystemColors.ControlBrush;

                 
                    if (vm.ShuffledAnswers[0] == vm.CurrentQuestion.CorrectAnswer)
                        Button1.Background = Brushes.LightGreen;
                    if (vm.ShuffledAnswers[1] == vm.CurrentQuestion.CorrectAnswer)
                        Button2.Background = Brushes.LightGreen;
                    if (vm.ShuffledAnswers[2] == vm.CurrentQuestion.CorrectAnswer)
                        Button3.Background = Brushes.LightGreen;
                    if (vm.ShuffledAnswers[3] == vm.CurrentQuestion.CorrectAnswer)
                        Button4.Background = Brushes.LightGreen;

                  
                    if (vm.SelectedAnswer != vm.CurrentQuestion.CorrectAnswer)
                    {
                        if (vm.ShuffledAnswers[0] == vm.SelectedAnswer)
                            Button1.Background = Brushes.LightCoral;
                        if (vm.ShuffledAnswers[1] == vm.SelectedAnswer)
                            Button2.Background = Brushes.LightCoral;
                        if (vm.ShuffledAnswers[2] == vm.SelectedAnswer)
                            Button3.Background = Brushes.LightCoral;
                        if (vm.ShuffledAnswers[3] == vm.SelectedAnswer)
                            Button4.Background = Brushes.LightCoral;
                    }
                }
                else
                {
                 
                    Button1.Background = SystemColors.ControlBrush;
                    Button2.Background = SystemColors.ControlBrush;
                    Button3.Background = SystemColors.ControlBrush;
                    Button4.Background = SystemColors.ControlBrush;
                }
            }
        }
    }
}