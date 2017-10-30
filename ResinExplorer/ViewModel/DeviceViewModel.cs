using Cas.Common.WPF.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Resin.Api.Client;
using Resin.Api.Client.Domain;
using System;
using System.Windows.Input;

namespace ResinExplorer.ViewModel
{
    public class DeviceViewModel : ViewModelBase
    {
        public ICommand EditNameCommand { get; private set; }

        private readonly ResinDevice _model;
        private readonly ITextEditService _textEditService;
        private readonly ResinApiClient _client;

        public DeviceViewModel(ResinDevice model, ITextEditService textEditService, ResinApiClient client)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _textEditService = textEditService ?? throw new ArgumentNullException(nameof(textEditService));
            _client = client ?? throw new ArgumentNullException(nameof(client));

            EditNameCommand = new RelayCommand(EditName);
        }

        private async void EditName()
        {
            string name = Name;
            _textEditService.EditText(Name, "Edit the text:", "Edit Application Name", t => name = t, t => !string.IsNullOrWhiteSpace(t));

            if (name != Name)
            {
                await _client.RenameDeviceAsync(Id, name);
            }

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
