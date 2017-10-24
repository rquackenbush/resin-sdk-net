using System;
using System.Windows;
using Autofac;
using Cas.Common.WPF.Interfaces;
using Resin.Api.Client;
using ResinExplorer.ViewModel;

namespace ResinExplorer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private async void App_OnStartup(object sender, StartupEventArgs e)
        {
            using (var container = ApplicationContainerFactory.Create())
            {
                IViewService viewService = container.Resolve<IViewService>();

                var logonViewModel = container.Resolve<LogonDialogViewModel>();

                while (true)
                {
                    try
                    {
                        if (viewService.ShowDialog(logonViewModel) == true)
                        {
                            //Create a temporary token provider
                            var tokenProvider = new SimpleTokenProvider(logonViewModel.Token);

                            //Use a temp client to update the token / verify the current token
                            var tempClient = new ResinApiClient(tokenProvider, logonViewModel.ApiAddress);

                            //Get the new token (this also verifies the old one)
                            string newToken = await tempClient.WhoamiAsync();

                            //Create a new client
                            var client = new ResinApiClient(new SimpleTokenProvider(newToken), logonViewModel.ApiAddress);

                            //Create the view model
                            var mainViewModel = container.Resolve<MainViewModel>(new TypedParameter(typeof(ResinApiClient), client));

                            //Finally - we can start up the real thing!
                            viewService.ShowDialog(mainViewModel);

                            return;
                        }
                        
                        //The user cancelled. Exit the application.
                        return;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ex.Source);
                    }
                }
            }
        }
    }
}
