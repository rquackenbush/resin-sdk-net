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

        private async void EditVariables()
        {
            var variables = await _client.GetApplicationEnvironmentVariablesAsync(Id);
            EditVariablesDialogViewModel vm = new EditVariablesDialogViewModel(variables);

            if (_viewService.ShowDialog(vm) == true)
            {
                if (vm.DirtyService.IsDirty)
                {
                    List<EnvironmentVariable> currentVariables = vm.Variables.Select(v => v.GetModel()).ToList();
                    List<int> originalIds = variables.Select(v => v.Id).ToList();

                    foreach (var variable in currentVariables)
                    {
                        if (variables.Any(x => x.Id == variable.Id && x.Name == variable.Name && x.Value == variable.Value))
                        {
                            // No change, do nothing
                        }
                        else if (originalIds.Contains(variable.Id))
                        {
                            // Update needed
                            await _client.UpdateApplicationVariableAsync(variable.Id, variable.Name, variable.Value);
                        }
                        else
                        {
                            // Id is not there, we have to add it
                            await _client.CreateApplicationVariableAsync(Id, variable.Name, variable.Value);
                        }
                    }

                    List<EnvironmentVariable> variablesForDelete = GetVariablesForDelete(variables, currentVariables);

                    foreach (var variable in variablesForDelete)
                    {
                        await _client.DeleteApplicationEnvironmentVariableAsync(variable.Id);
                    }
                }
            }
        }

        private List<EnvironmentVariable> GetVariablesForAdd(IEnumerable<EnvironmentVariable> original, IEnumerable<EnvironmentVariable> current)
        {
            return current.Where(x => !original.Any(y => y.Name == x.Name && y.Value == x.Value)).ToList();
        }

        private List<EnvironmentVariable> GetVariablesForDelete(IEnumerable<EnvironmentVariable> original, IEnumerable<EnvironmentVariable> current)
        {
            return original.Where(x => !current.Any(y => y.Name == x.Name && y.Value == x.Value)).ToList();
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