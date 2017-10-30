using Cas.Common.WPF.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Resin.Api.Client;
using Resin.Api.Client.Domain;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace ResinExplorer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IViewService _viewService;
        private readonly ResinApiClient _client;
        private bool _isBusy;
        private ObservableCollection<ApplicationViewModel> _applications;
        private DeviceViewModel[] _devices;
        private ApplicationViewModel _selectedApplication;
        private DeviceViewModel _selectedDevice;


        public MainViewModel(IViewService viewService, ResinApiClient client)
        {
            if (viewService == null) throw new ArgumentNullException(nameof(viewService));
            if (client == null) throw new ArgumentNullException(nameof(client));

            _viewService = viewService;
            _client = client;
            InitializeCommands();

            Refresh();
        }

        private void InitializeCommands()
        {
            RefreshCommand = new RelayCommand(Refresh);
            CreateApplicationCommand = new RelayCommand(CreateApplication);
            DeleteApplicationCommand = new RelayCommand(DeleteApplication, CanDelete);
            CreateDeviceCommand = new RelayCommand(CreateDevice);
            DeleteDeviceCommand = new RelayCommand(DeleteDevice, CanDeleteDevice);
        }

        private void DeleteDevice()
        {
        }

        private bool CanDeleteDevice()
        {
            return SelectedDevice != null;
        }

        private void CreateDevice()
        {
            //throw new NotImplementedException();
        }

        private void Refresh()
        {
            RefreshApplications();
            RefreshDevices();
        }

        private bool CanDelete()
        {
            return SelectedApplication != null;
        }

        private void DeleteApplication()
        {
            throw new NotImplementedException();
        }

        private async void CreateApplication()
        {
            try
            {
                var viewModel = new CreateApplicationDialogViewModel();

                if (_viewService.ShowDialog(viewModel) == true)
                {
                    var newResinApplication = await _client.CreateApplicationAsync(viewModel.Name, viewModel.DeviceType);
                    ApplicationViewModel newApplication = new ApplicationViewModel(newResinApplication);
                    Applications.Add(newApplication);
                }
            }
            catch (Exception ex)
            {
                // TODO show exception dialog
            }
        }

        public ICommand RefreshCommand { get; private set; }
        public ICommand CreateApplicationCommand { get; private set; }
        public ICommand DeleteApplicationCommand { get; private set; }
        public ICommand CreateDeviceCommand { get; private set; }
        public ICommand DeleteDeviceCommand { get; private set; }

        private CancellationTokenSource CreateCancellationTokenSource(double timeoutSeconds = 30)
        {
            return new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
        }

        private async void RefreshApplications()
        {
            try
            {
                IsBusy = true;

                var cts = CreateCancellationTokenSource();

                ResinApplication[] applications = await _client.GetApplicationsAsync(cts.Token);

                Applications = new ObservableCollection<ApplicationViewModel>(
                        applications
                        .Select(a => new ApplicationViewModel(a))
                        .OrderBy(a => a.Name)
                    );
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

        private async void RefreshDevices()
        {
            try
            {
                IsBusy = true;

                var cts = CreateCancellationTokenSource();

                ResinDevice[] devices = await _client.GetDevicesAsync(cts.Token);

                Devices = devices.Select(d => new DeviceViewModel(d)).OrderBy(d => d.Name).ToArray();


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

        public ObservableCollection<ApplicationViewModel> Applications
        {
            get { return _applications; }
            private set
            {
                _applications = value;
                RaisePropertyChanged();
            }
        }

        public DeviceViewModel[] Devices
        {
            get { return _devices; }
            set { _devices = value; RaisePropertyChanged(); }
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

        public DeviceViewModel SelectedDevice
        {
            get { return _selectedDevice; }
            set { _selectedDevice = value; RaisePropertyChanged(); }
        }
    }
}