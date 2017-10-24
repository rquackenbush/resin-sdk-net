using Autofac;
using Cas.Common.WPF;
using Cas.Common.WPF.Interfaces;
using ResinExplorer.View;
using ResinExplorer.ViewModel;

namespace ResinExplorer
{
    public static class ApplicationContainerFactory
    {
        public static IContainer Create()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainViewModel>();
            builder.RegisterType<LogonDialogViewModel>();


            var viewService = new ViewService();
            viewService.Register<LogonDialogViewModel, LogonDialogView>();
            viewService.Register<MainViewModel, MainWindow>();

            builder.RegisterInstance(viewService)
                .As<IViewService>();

            return builder.Build();
        }
    }
}