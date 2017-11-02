using Cas.Common.WPF.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Resin.Api.Client;
using Resin.Api.Client.Domain;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ResinExplorer.ViewModel
{
    public class DeviceViewModel : ViewModelBase
    {
        public ICommand EditNameCommand { get; private set; }
        public ICommand EditNoteCommand { get; private set; }

        private ResinDevice _model;
        private readonly ITextEditService _textEditService;
        private readonly ResinApiClient _client;

        public DeviceViewModel(ResinDevice model, ITextEditService textEditService, ResinApiClient client)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _textEditService = textEditService ?? throw new ArgumentNullException(nameof(textEditService));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            EditNameCommand = new RelayCommand(EditName);
            EditNoteCommand = new RelayCommand(EditNote);
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
                await _client.RenameDeviceAsync(Id, name);
                await ModelUpdate();
                RaisePropertyChanged(nameof(Name));
            }
        }

        private async Task ModelUpdate()
        {
            ResinDevice newModel = await _client.GetDeviceAsync(Id);
            _model = newModel;
        }

        public int Id
        {
            get { return _model.Id; }
        }

        public string Name
        {
            get { return _model.Name; }
        }

        public string DeviceType
        {
            get { return _model.DeviceType; }
        }

        public string CommitReference
        {
            get { return _model.Commit; }
        }
    }
}
