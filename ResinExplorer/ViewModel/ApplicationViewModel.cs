using Cas.Common.WPF.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Resin.Api.Client;
using Resin.Api.Client.Domain;
using System;
using System.Windows.Input;

namespace ResinExplorer.ViewModel
{
    public class ApplicationViewModel : ViewModelBase
    {
        public ICommand EditNameCommand { get; private set; }
        public ICommand EditVariablesCommand { get; private set; }

        private readonly ResinApplication _model;
        private ITextEditService _textEditService;
        private ResinApiClient _client;
        private IViewService _viewService;

        public ApplicationViewModel(ResinApplication model, ITextEditService textEditService, ResinApiClient client, IViewService viewService)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            _textEditService = textEditService ?? throw new ArgumentNullException(nameof(model));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _viewService = viewService ?? throw new ArgumentNullException(nameof(viewService));
            _model = model;
            EditNameCommand = new RelayCommand(EditName);
            EditVariablesCommand = new RelayCommand(EditVariables);
        }

        private void EditVariables()
        {

        }

        private async void EditName()
        {
            var name = Name;
            _textEditService.EditText(Name, "Edit the text:", "Edit Application Name", t => name = t, t => !string.IsNullOrWhiteSpace(t));

            if (name != Name)
            {
                await _client.RenameApplicationAsync(Id, name);
            }
        }

        public int Id
        {
            get { return _model.Id; }
        }

        public string Name
        {
            get { return _model.AppName; }
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