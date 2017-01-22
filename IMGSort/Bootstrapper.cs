using Microsoft.Practices.Unity;
using Prism.Unity;
using IMGSort.Views;
using System.Windows;

namespace IMGSort
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
