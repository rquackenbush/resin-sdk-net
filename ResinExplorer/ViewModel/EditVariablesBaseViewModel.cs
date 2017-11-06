using Cas.Common.WPF;
using Cas.Common.WPF.Behaviors;
using Cas.Common.WPF.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Resin.Api.Client;
using ResinExplorer.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ResinExplorer.ViewModel
{
    public abstract class EditVariablesBaseViewModel : ViewModelBase, ICloseableViewModel
    {

        private EnvironmentVariableViewModel _selectedVariable;
        private ObservableCollection<EnvironmentVariableViewModel> _variables = new ObservableCollection<EnvironmentVariableViewModel>();

        public event EventHandler<CloseEventArgs> Close;

        public IDirtyService DirtyService => _dirtyService;

        public ICommand AddVariableCommand { get; private set; }
        public ICommand RemoveVariableCommand { get; private set; }
        public ICommand OkCommand { get; private set; }

        protected List<int> VariablesForDelete = new List<int>();
        protected readonly ResinApiClient Client;
        private readonly IDirtyService _dirtyService = new DirtyService();

        protected EditVariablesBaseViewModel(ResinApiClient client, IEnumerable<GenericEnvironmentVariable> variables)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
            Variables.AddRange(variables.Select(v => new EnvironmentVariableViewModel(v)));
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            AddVariableCommand = new RelayCommand(AddVariable);
            RemoveVariableCommand = new RelayCommand(RemoveVariable, CanRemoveVariable);
            OkCommand = new RelayCommand(Ok, CanOk);
        }

        public bool CanClose()
        {
            if (DirtyService.IsDirty)
            {
                var result = MessageBox.Show("Really close?", "Unsaved changes", MessageBoxButton.YesNo);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        return true;
                    case MessageBoxResult.No:
                        return false;
                }
            }

            return true;
        }

        private async void Ok()
        {
            await SaveAsync();
            Close?.Invoke(this, new CloseEventArgs(true));
        }

        private async Task SaveAsync()
        {
            var tasks = new List<Task>();

            // add
            var variablesForAdd = Variables.Where(v => v.Id == default(int)).ToDictionary(v => v.Name, v => v.Value);

            tasks.AddRange(AddVariablesAsync(variablesForAdd));

            // delete
            tasks.AddRange(Client.DeleteEnvironmentVariableAsync(VariablesForDelete));

            // update
            var variablesForUpdate = Variables.Where(v => v.Id != default(int) && v.DirtyService.IsDirty).ToDictionary(v => v.Id, v => v.Value);
            tasks.AddRange(Client.UpdateEnvironmentVariableAsync(variablesForUpdate));

            await Task.WhenAll(tasks);

            DirtyService.MarkClean();
        }

        protected abstract IEnumerable<Task> AddVariablesAsync(Dictionary<string, string> variables);

        private bool CanOk() => true;

        private bool CanRemoveVariable()
        {
            return SelectedVariable != null;
        }

        private void RemoveVariable()
        {
            if (SelectedVariable.Id != default(int))
            {
                VariablesForDelete.Add(SelectedVariable.Id);
            }

            Variables.Remove(SelectedVariable);
            DirtyService.MarkDirty();
        }

        private void AddVariable()
        {
            Variables.Add(new EnvironmentVariableViewModel(new GenericEnvironmentVariable { Name = GetNewName(), Value = "new value" }));
            DirtyService.MarkDirty();
        }

        private string GetNewName()
        {
            string name = "name_";
            List<string> currentNames = Variables.Select(v => v.Name).ToList();

            for (int i = 1; ; i++)
            {
                string tempName = name + i;

                if (!currentNames.Contains(tempName))
                {
                    return tempName;
                }
            }
        }

        public void Closed()
        {
        }

        public EnvironmentVariableViewModel SelectedVariable
        {
            get { return _selectedVariable; }
            set
            {
                _selectedVariable = value;
                RaisePropertyChanged();
            }
        }
        
        public ObservableCollection<EnvironmentVariableViewModel> Variables
        {
            get { return _variables; }
            set
            {
                _variables = value;
                RaisePropertyChanged();
            }
        }

    }
}
