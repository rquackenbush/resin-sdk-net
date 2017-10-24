using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Cas.Common.WPF.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Resin.Api.Client;
using Resin.Api.Client.Domain;

namespace ResinExplorer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IViewService _viewService;
        private readonly ResinApiClient _client;
        private bool _isBusy;
        private ApplicationViewModel[] _applications;
        private ApplicationViewModel _selectedApplication;

        public MainViewModel(IViewService viewService, ResinApiClient client)
        {
            if (viewService == null) throw new ArgumentNullException(nameof(viewService));
            if (client == null) throw new ArgumentNullException(nameof(client));

            _viewService = viewService;
            _client = client;

            RefreshCommand = new RelayCommand(Refresh);

            Refresh();
        }

        public ICommand RefreshCommand { get; }

        private CancellationTokenSource CreateCancellationTokenSource(double timeoutSeconds = 30)
        {
            return new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
        }

        private async void Refresh()
        {
            try
            {
                IsBusy = true;

                var cts = CreateCancellationTokenSource();

                ResinApplication[] applications = await _client.GetApplicationsAsync(cts.Token);

                Applications = applications
                    .Select(a => new ApplicationViewModel(a))
                    .OrderBy(a => a.Name)
                    .ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                _isBusy = value; 
                RaisePropertyChanged();
            }
        }

        public ApplicationViewModel[] Applications
        {
            get { return _applications; }
            private set
            {
                _applications = value; 
                RaisePropertyChanged();
            }
        }

        public ApplicationViewModel SelectedApplication
        {
            get { return _selectedApplication; }
            set
            {
                _selectedApplication = value; 
                RaisePropertyChanged();
            }
        }
    }
}