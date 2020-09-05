using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task_scheduler.Model;

namespace task_scheduler.ViewModel
{
    public class CalanderViewModel : BindableBase
    {
        private readonly DateTimeFormatInfo DateInfo;



        public ObservableCollection<MonthModel> MonthList { get; set; }

        public ObservableCollection<DaysModel> DayList { get; set; }

        public ObservableCollection<int> YearsList { get; set; }

        public CalanderViewModel()
        {
            // InitializeCommand();
            DateInfo = new DateTimeFormatInfo();
            GetYears();
            GetDays();
            GetMonthList();
        }

        #region Properties

        private ObservableCollection<CalanderTimeModel> _daysList;

        public ObservableCollection<CalanderTimeModel> DaysList
        {
            get { return _daysList; }
            set { _daysList = value; RaisePropertyChanged(); }
        }


        private int _selectedMonth = DateTime.Now.Month;

        public int SelectedMonth
        {
            get { return _selectedMonth; }
            set { _selectedMonth = value; RaisePropertyChanged(); OncmbmonthSelectionChanged(); }
        }

        private object _selectedYear = DateTime.Now.Year;

        public object SelectedYear
        {
            get { return _selectedYear; }
            set { _selectedYear = value; RaisePropertyChanged(); cmbyearSelectionChanged(); }
        }


        #endregion

        #region Command

        private void OncmbmonthSelectionChanged()
        {
            if (SelectedYear != null && SelectedMonth > 0)
            {
                var year = Convert.ToInt32(SelectedYear);
                var month = SelectedMonth;
                CreateDate(month, year);
            }
        }

        private void cmbyearSelectionChanged()
        {
            if (SelectedYear != null && SelectedMonth > 0)
            {
                var year = Convert.ToInt32(SelectedYear);
                var month = SelectedMonth;
                CreateDate(month, year);
            }
        }

        #endregion

        #region Private Mentods


        /// <summary>
        /// Get years of range from 1930 to 1930+100.
        /// </summary>
        private void GetYears()
        {
            YearsList = new ObservableCollection<int>();
            var year = Enumerable.Range(1930, 100).ToList();
            for (int i = 0; i < year.Count; i++)
            {
                YearsList.Add(year[i]);
            }
        }

        /// <summary>
        /// Gets Days in a Week.
        /// </summary>
        private void GetDays()
        {
            DayList = new ObservableCollection<DaysModel>();
            for (int i = 0; i < DateInfo.AbbreviatedDayNames.Length; i++)
            {
                var month = DateInfo.AbbreviatedDayNames[i];
                if (!string.IsNullOrEmpty(month))
                    DayList.Add(new DaysModel { Month = i + 1, Name = month });
            }
        }

        /// <summary>
        /// Get List of Month .
        /// </summary>
        private void GetMonthList()
        {
            MonthList = new ObservableCollection<MonthModel>();

            for (int i = 0; i < DateInfo.MonthNames.Length; i++)
            {
                var month = DateInfo.MonthNames[i];
                if (!string.IsNullOrEmpty(month))
                    MonthList.Add(new MonthModel { Month = i + 1, Name = DateInfo.MonthNames[i] });
            }
        }

        private void CreateDate(int month, int Year)
        {
            int i;
            DaysList = new ObservableCollection<CalanderTimeModel>();
            DateTime now = new DateTime(Year, month, DateTime.Now.Day);
            var currentMonth = new DateTime(now.Year, now.Month, 1);
            var previousMonth = currentMonth.AddMonths(-1).AddDays(1);
            var nextmonth = currentMonth.AddMonths(+1).AddDays(1);

            AddPassedMonth(currentMonth, previousMonth);

            for (i = 1; i <= DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month); i++)
            {
                var date = new DateTime(currentMonth.Year, currentMonth.Month, i);

                DaysList.Add(new CalanderTimeModel { CurrentDate = date });
            }

            AddNextMonth(nextmonth);
        }

        /// <summary>
        /// Add Next Month Dates
        /// </summary>
        /// <param name="nextmonth">Next Month </param>
        private void AddNextMonth(DateTime nextmonth)
        {
            if (DaysList.Count < 42)
            {
                var count = DaysList.ToList().Count();
                for (int k = 1; k <= (42 - count); k++)
                {
                    var date = new DateTime(nextmonth.Year, nextmonth.Month, k);

                    DaysList.Add(new CalanderTimeModel { CurrentDate = date, IsNext = true });
                }
            }
        }

        /// <summary>
        /// Add Passed Month Dates.
        /// </summary>
        /// <param name="currentMonth">Current Month.</param>
        /// <param name="previousMonth">Passed Month.</param>
        private void AddPassedMonth(DateTime currentMonth, DateTime previousMonth)
        {
            if (currentMonth.DayOfWeek != DayOfWeek.Sunday)
            {
                for (int j = (int)currentMonth.DayOfWeek; j >= 1; j--)
                {
                    var date = new DateTime(previousMonth.Year, previousMonth.Month, (DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month) - j) + 1);

                    DaysList.Add(new CalanderTimeModel { CurrentDate = date, IsPrevious = true });
                }
            }
        }

        #endregion
    }
}
