using Cas.Common.WPF.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Resin.Api.Client;
using Resin.Api.Client.Domain;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ResinExplorer.ViewModel
{
    using System.Threading;
    using Resin.Api.Client.Interfaces;
    using Resin.SupervisorApi.Client;

    public class DeviceViewModel : ViewModelBase
    {
        public ICommand EditNameCommand { get; private set; }
        public ICommand EditNoteCommand { get; private set; }
        public ICommand EditVariablesCommand { get; private set; }
        public ICommand BlinkCommand { get; private set; }
        public ICommand RebootCommand { get; private set; }
        public ICommand RestartCommand { get; private set; }
        public ICommand ShutdownCommand { get; private set; }


        private readonly ITokenProvider _tokenProvider;
        private ResinDevice _model;
        private readonly ITextEditService _textEditService;
        private readonly ResinApiClient _client;
        private readonly IViewService _viewService;

        public DeviceViewModel(ITokenProvider tokenProvider, ResinDevice model, ITextEditService textEditService, ResinApiClient client, IViewService viewService)
        {
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _textEditService = textEditService ?? throw new ArgumentNullException(nameof(textEditService));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _viewService = viewService ?? throw new ArgumentNullException(nameof(viewService));
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            EditNameCommand = new RelayCommand(EditName);
            EditNoteCommand = new RelayCommand(EditNote);
            EditVariablesCommand = new RelayCommand(EditDeviceVariables);
            BlinkCommand = new RelayCommand(Blink);
            RebootCommand = new RelayCommand(Reboot);
            ShutdownCommand = new RelayCommand(Shutdown);
            RestartCommand = new RelayCommand(Restart);
        }

        private async void Blink()
        {
            var client = new ProxySupervisorClient(_model.Uuid, await _tokenProvider.GetTokenAsync(CancellationToken.None));

            await client.BlinkAsync();

            //await _client.BlinkDeviceAsync(Id);
        }

        private async void Shutdown()
        {
            if (MessageBox.Show("Are you sure?", "Device shutdown", MessageBoxButton.YesNo) == MessageBoxResult.OK)
            {
                await _client.ShutdownDeviceAsync(Id);
            }
        }

        private async void Reboot()
        {
            if (MessageBox.Show("Are you sure?", "Device reboot", MessageBoxButton.YesNo) == MessageBoxResult.OK)
            {
                await _client.RebootDeviceAsync(Id);
            }
        }

        private async void Restart()
        {
            if (MessageBox.Show("Are you sure?", "Device restart", MessageBoxButton.YesNo) == MessageBoxResult.OK)
            {
                await _client.RestartDeviceAsync(Id);
            }
        }

        private async void EditDeviceVariables()
        {
            var variables = await _client.GetDeviceEnvironmentalVariablesAsync(Id);
            EditDeviceVariablesDialogViewModel vm = new EditDeviceVariablesDialogViewModel(_client, Id, variables);

            _viewService.ShowDialog(vm);
        }

        private async void EditNote()
        {
            string note = _model.Note;
            _textEditService.EditText(note, "Edit the note:", "Note Edit", t => note = t, t => !string.IsNullOrWhiteSpace(t));

            if (note != _model.Note)
            {
                await _client.AddNoteAsync(Id, note);
                await ModelUpdate();
            }
        }

        private async void EditName()
        {
            string name = Name;
            _textEditService.EditText(name, "Edit the text:", "Edit Device Name", t => name = t, t => !string.IsNullOrWhiteSpace(t));

            if (name != Name)
            {
                var result = await _client.RenameDeviceAsync(Id, name);
                await ModelUpdate();
            }
        }

        private async Task ModelUpdate()
        {
            ResinDevice newModel = await _client.GetDeviceAsync(Id);
            _model = newModel;
            RaisePropertyChanged(null);
        }

        public int Id => _model.Id;

        public string Name => _model.Name;

        public string DeviceType => _model.DeviceType;

        public string CommitReference => _model.Commit;

    }
}
