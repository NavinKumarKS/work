using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace task_scheduler.Model
{
    public class CalanderTimeModel : INotifyPropertyChanged
    {
        private bool _ispreviousMonth;

        public bool IsPrevious
        {
            get { return _ispreviousMonth; }
            set { _ispreviousMonth = value; OnPropertyChanged(); }
        }

        private bool _isNext;

        public bool IsNext
        {
            get { return _isNext; }
            set { _isNext = value; OnPropertyChanged(); }
        }

        private DateTime _date;

        public DateTime CurrentDate
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(); }
        }

        public int Day { get { return CurrentDate.Day; } }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
    }
}
