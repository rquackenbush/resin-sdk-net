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
    using Resin.Api.Client.Interfaces;

    public class MainViewModel : ViewModelBase
    {

        public ICommand RefreshCommand { get; private set; }
        public ICommand CreateApplicationCommand { get; private set; }
        public ICommand DeleteApplicationCommand { get; private set; }
        public ICommand CreateDeviceCommand { get; private set; }
        public ICommand DeleteDeviceCommand { get; private set; }

        private readonly IViewService _viewService;
        private readonly ResinApiClient _client;
        private bool _isBusy;
        private ObservableCollection<ApplicationViewModel> _applications;
        private ObservableCollection<DeviceViewModel> _devices;
        private ApplicationViewModel _selectedApplication;
        private DeviceViewModel _selectedDevice;
        private readonly ITokenProvider _tokenProvider;
        private ITextEditService _textEditService;

        public MainViewModel(ITokenProvider tokenProvider, IViewService viewService, ResinApiClient client, ITextEditService textEditService)
        {
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _textEditService = textEditService ?? throw new ArgumentNullException(nameof(textEditService));
            _viewService = viewService ?? throw new ArgumentNullException(nameof(viewService));
            _client = client ?? throw new ArgumentNullException(nameof(client));

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

        private async void DeleteDevice()
        {
            if (MessageBox.Show("Are you sure?", "Delete device", MessageBoxButton.YesNo) == MessageBoxResult.OK)
            {
                await _client.DeleteDeviceAsync(SelectedDevice.Id);
                Devices.Remove(SelectedDevice);
            }
        }

        private bool CanDeleteDevice()
        {
            return SelectedDevice != null;
        }

        private async void CreateDevice()
        {
            var viewModel = new CreateDeviceDialogViewModel(Applications);

            if (_viewService.ShowDialog(viewModel) == true)
            {
                var deviceResult = await _client.RegisterDeviceAsync(viewModel.SelectedApplication.Id, Guid.NewGuid().ToString("N"));
                await _client.RenameDeviceAsync(deviceResult.Id, viewModel.Name);
                var newDevice = await _client.GetDeviceAsync(deviceResult.Id);
                Devices.Add(new DeviceViewModel(_tokenProvider, newDevice, _textEditService, _client, _viewService));
            }
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

        private async void DeleteApplication()
        {
            try
            {
                if (MessageBox.Show("Are you sure?", "Application Delete", MessageBoxButton.YesNo) == MessageBoxResult.OK)
                {
                    await _client.DeleteApplicationAsync(SelectedApplication.Id);
                    Applications.Remove(SelectedApplication);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }

        private async void CreateApplication()
        {
            try
            {
                var viewModel = new CreateApplicationDialogViewModel();

                if (_viewService.ShowDialog(viewModel) == true)
                {
                    var newResinApplication = await _client.CreateApplicationAsync(viewModel.Name, viewModel.DeviceType);
                    ApplicationViewModel newApplication = new ApplicationViewModel(newResinApplication, _textEditService, _client, _viewService);
                    Applications.Add(newApplication);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }

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
                        .Select(a => new ApplicationViewModel(a, _textEditService, _client, _viewService))
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

                Devices = new ObservableCollection<DeviceViewModel>(devices.Select(d => new DeviceViewModel(_tokenProvider, d, _textEditService, _client, _viewService)).OrderBy(d => d.Name));

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

        public ObservableCollection<DeviceViewModel> Devices
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