using System.Windows;
using task_scheduler.ViewModel;

namespace task_scheduler.View
{
    /// <summary>
    /// Interaction logic for CalanderView.xaml
    /// </summary>
    public partial class CalanderView
    {
        public CalanderView()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = new CalanderViewModel();
        }
    }
}
