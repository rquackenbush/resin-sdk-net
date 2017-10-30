using Autofac;
using Cas.Common.WPF;
using Cas.Common.WPF.Interfaces;
using ResinExplorer.Core;
using ResinExplorer.Interfaces;
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
            builder.RegisterType<CreateApplicationDialogViewModel>();
            builder.RegisterType<EditVariablesDialogViewModel>();
            builder.RegisterType<CreateDeviceDialogViewModel>();

            var viewService = new ViewService();
            viewService.Register<LogonDialogViewModel, LogonDialogView>();
            viewService.Register<MainViewModel, MainWindow>();
            viewService.Register<CreateApplicationDialogViewModel, CreateApplicationDialogView>();
            viewService.Register<EditVariablesDialogViewModel, EditVariablesDialogView>();
            viewService.Register<CreateDeviceDialogViewModel, CreateDeviceDialogView>();

            builder.RegisterInstance(viewService)
                .As<IViewService>();

            builder.RegisterType<UserSettings>().As<ISettings>().SingleInstance();
            builder.RegisterType<TextEditService>().As<ITextEditService>();

            return builder.Build();
        }
    }
}