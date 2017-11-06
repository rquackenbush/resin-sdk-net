using Cas.Common.WPF.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Resin.Api.Client;
using Resin.Api.Client.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace ResinExplorer.ViewModel
{
    public class ApplicationViewModel : ViewModelBase
    {
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
            EditVariablesCommand = new RelayCommand(EditVariables);
        }

        private async void EditVariables()
        {
            var variables = await _client.GetApplicationEnvironmentVariablesAsync(Id);
            EditApplicationVariablesDialogViewModel vm = new EditApplicationVariablesDialogViewModel(_client, Id, variables);

            _viewService.ShowDialog(vm);
        }

        private List<EnvironmentVariable> GetVariablesForAdd(IEnumerable<EnvironmentVariable> original, IEnumerable<EnvironmentVariable> current)
        {
            return current.Where(x => !original.Any(y => y.Name == x.Name && y.Value == x.Value)).ToList();
        }

        private List<EnvironmentVariable> GetVariablesForDelete(IEnumerable<EnvironmentVariable> original, IEnumerable<EnvironmentVariable> current)
        {
            return original.Where(x => !current.Any(y => y.Name == x.Name && y.Value == x.Value)).ToList();
        }

        public int Id => _model.Id;

        public string Name => _model.AppName;

        public string DeviceType => _model.DeviceType;

        public string CommitReference => _model.Commit;
    }
}