using Cas.Common.WPF.Behaviors;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace ResinExplorer.ViewModel
{
    public class CreateDeviceDialogViewModel : ViewModelBase, ICloseableViewModel
    {
        public CreateDeviceDialogViewModel(IEnumerable<ApplicationViewModel> applications)
        {
            Applications = applications;
            OkCommand = new RelayCommand(Ok, CanOk);
        }

        public ICommand OkCommand { get; private set; }

        public IEnumerable<ApplicationViewModel> Applications { get; private set; }

        public event EventHandler<CloseEventArgs> Close;

        private ApplicationViewModel _selectedApplication = null;
        public ApplicationViewModel SelectedApplication
        {
            get { return _selectedApplication; }
            set { _selectedApplication = value; RaisePropertyChanged(); }
        }

        public bool CanClose() => true;

        public void Closed()
        {
        }

        private bool CanOk()
        {
            return SelectedApplication != null;
        }

        private void Ok()
        {
            Close?.Invoke(this, new CloseEventArgs(true));
        }
    }
}
