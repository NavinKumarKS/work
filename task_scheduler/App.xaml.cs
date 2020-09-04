using Prism.Ioc;
using Prism.Unity;
using System.Windows;

namespace task_scheduler
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            var w = Container.Resolve<MainWindow>();
            return w;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
