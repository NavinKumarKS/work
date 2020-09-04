using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using task_scheduler.Control;
using System.Collections.ObjectModel;
using task_scheduler.Model;
using System;
using System.Linq;
using System.Globalization;

namespace task_scheduler
{
    public partial class MainWindow
    {
        private readonly DateTimeFormatInfo DateInfo;

        private ObservableCollection<CalanderTimeModel> _daysList;

        public ObservableCollection<CalanderTimeModel> DaysList
        {
            get { return _daysList; }
            set { _daysList = value; }
        }


        private ObservableCollection<MonthModel> _monthModels;

        public ObservableCollection<MonthModel> MonthList
        {
            get { return _monthModels; }
            set { _monthModels = value; }
        }

        private ObservableCollection<DaysModel> _dayList;

        public ObservableCollection<DaysModel> DayList
        {
            get { return _dayList; }
            set { _dayList = value; }
        }


        public MainWindow()
        {
            InitializeComponent();
            DateInfo = new DateTimeFormatInfo();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GetYears();
            GetDays();
            GetMonthList();
            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(Theme.Dark);
            paletteHelper.SetTheme(theme);
        }

        private void GetYears()
        {
            var year = Enumerable.Range(1930, 100).ToList();
            for (int i = 0; i < year.Count; i++)
            {
                cmbyear.Items.Add(year[i]);
            }
            cmbyear.SelectedItem = DateTime.Now.Year;
        }

        private void GetDays()
        {
            DayList = new ObservableCollection<DaysModel>();
            for (int i = 0; i < DateInfo.AbbreviatedDayNames.Length; i++)
            {
                var _mon = DateInfo.AbbreviatedDayNames[i];
                if (!string.IsNullOrEmpty(_mon))
                    DayList.Add(new DaysModel { Month = i + 1, Name = _mon });
            }
            days.ItemsSource = DayList;
        }

        private void GetMonthList()
        {
            MonthList = new ObservableCollection<MonthModel>();

            for (int i = 0; i < DateInfo.MonthNames.Length; i++)
            {
                var _mon = DateInfo.MonthNames[i];
                if (!string.IsNullOrEmpty(_mon))
                    MonthList.Add(new MonthModel { Month = i + 1, Name = DateInfo.MonthNames[i] });
            }
            cmbmonth.SelectedValue = DateTime.Now.Month;
            cmbmonth.ItemsSource = MonthList;
        }

        private void CreateDate(int month, int Year)
        {
            int i;
            DaysList = new ObservableCollection<CalanderTimeModel>();
            DateTime now = new DateTime(Year, month, DateTime.Now.Day);


            var currentMonth = new DateTime(now.Year, now.Month, 1);
            var endofMonth = currentMonth.AddMonths(1).AddDays(-1);
            var previousMonth = currentMonth.AddMonths(-1).AddDays(1);
            var nextmonth = currentMonth.AddMonths(1 + 1).AddDays(-1);
            var prevdays = DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
            //DaysList.Add(new CalanderTimeModel { CurrentDate = _dt });

            if (currentMonth.DayOfWeek != DayOfWeek.Sunday)
            {
                for (int j = (int)currentMonth.DayOfWeek; j >= 1; j--)
                {
                    var _dt = new DateTime(previousMonth.Year, previousMonth.Month, (DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month) - j) + 1);

                    DaysList.Add(new CalanderTimeModel { CurrentDate = _dt, IsPrevious = true });
                }
            }

            for (i = 1; i <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
            {
                var _dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, i);

                DaysList.Add(new CalanderTimeModel { CurrentDate = _dt });
            }
            if (DaysList.Count < 42)
            {
                var _rr = DaysList.ToList().Count();
                for (int k = 1; k <= (42 - _rr); k++)
                {
                    var _dt = new DateTime(previousMonth.Year, previousMonth.Month, k);

                    DaysList.Add(new CalanderTimeModel { CurrentDate = _dt, IsNext = true });
                }
            }
            calander.ItemsSource = DaysList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            leftopen.IsLeftDrawerOpen = true;
        }

        private void OnMoveWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private static void ModifyTheme(bool isDarkTheme)
        {
            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(isDarkTheme ? Theme.Dark : Theme.Light);
            paletteHelper.SetTheme(theme);
        }

        private async void MenuPopupButton_OnClick(object sender, RoutedEventArgs e)
        {
            var sampleMessageDialog = new Schedule()
            {
                Message = { Text = ((ButtonBase)sender).Content.ToString() }
            };
            await DialogHost.Show(sampleMessageDialog, "RootDialog");
        }

        private void cmbmonth_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbyear != null && cmbmonth.SelectedValue != null)
            {
                var _year = Convert.ToInt32(cmbyear.SelectedItem);
                var _month = Convert.ToInt32(cmbmonth.SelectedValue);
                CreateDate(_month, _year);
            }

        }

        private void cmbyear_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbyear != null && cmbmonth.SelectedValue != null)
            {
                var _year = Convert.ToInt32(cmbyear.SelectedItem);
                var _month = Convert.ToInt32(cmbmonth.SelectedValue);
                CreateDate(_month, _year);
            }

        }
    }
}
