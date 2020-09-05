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
using task_scheduler.View;

namespace task_scheduler
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        #region Events

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(Theme.Dark);
            paletteHelper.SetTheme(theme);

            main.Children.Add(new CalanderView());

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            leftopen.IsLeftDrawerOpen = true;
        }

        private void OnMoveWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private async void MenuPopupButton_OnClick(object sender, RoutedEventArgs e)
        {
            var sampleMessageDialog = new Schedule()
            {
                Message = { Text = ((ButtonBase)sender).Content.ToString() }
            };
            await DialogHost.Show(sampleMessageDialog, "RootDialog");
        }

        #endregion
    }
}
