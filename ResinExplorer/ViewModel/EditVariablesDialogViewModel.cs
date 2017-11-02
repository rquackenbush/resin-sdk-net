using Cas.Common.WPF;
using Cas.Common.WPF.Behaviors;
using Cas.Common.WPF.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Resin.Api.Client.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ResinExplorer.ViewModel
{
    public class EditVariablesDialogViewModel : ViewModelBase, ICloseableViewModel
    {
        public event EventHandler<CloseEventArgs> Close;

        private IDirtyService _dirtyService = new DirtyService();

        public IDirtyService DirtyService => _dirtyService;

        public EditVariablesDialogViewModel(IEnumerable<EnvironmentVariable> variables)
        {
            //Variables.AddRange(variables.Select(v => new EnvironmentVariableViewModel(v)));
            //InitializeCommands();
        }

        private void InitializeCommands()
        {
            AddVariableCommand = new RelayCommand(AddVariable);
            RemoveVariableCommand = new RelayCommand(RemoveVariable, CanRemoveVariable);
            OkCommand = new RelayCommand(Ok, CanOk);
        }

        private void Ok()
        {
            Close?.Invoke(this, new CloseEventArgs(true));
        }

        private bool CanOk() => true;

        private bool CanRemoveVariable()
        {
            return SelectedVariable != null;
        }

        private void RemoveVariable()
        {
            Variables.Remove(SelectedVariable);
            DirtyService.MarkDirty();
        }

        private void AddVariable()
        {
            //Variables.Add(new EnvironmentVariableViewModel(new ApplicationEnvironmentVariable()));
            DirtyService.MarkDirty();
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

        public void Closed()
        {
        }

        public ICommand AddVariableCommand { get; private set; }
        public ICommand RemoveVariableCommand { get; private set; }
        public ICommand OkCommand { get; private set; }


        private EnvironmentVariableViewModel _selectedVariable = null;
        public EnvironmentVariableViewModel SelectedVariable
        {
            get { return _selectedVariable; }
            set { _selectedVariable = value; RaisePropertyChanged(); }
        }


        private ObservableCollection<EnvironmentVariableViewModel> _variables = new ObservableCollection<EnvironmentVariableViewModel>();
        public ObservableCollection<EnvironmentVariableViewModel> Variables
        {
            get { return _variables; }
            set { _variables = value; RaisePropertyChanged(); }
        }
    }
}
