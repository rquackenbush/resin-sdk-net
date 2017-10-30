using Cas.Common.WPF.Behaviors;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Input;

namespace ResinExplorer.ViewModel
{
    public class CreateApplicationDialogViewModel : ViewModelBase, ICloseableViewModel
    {
        public ICommand OkCommand { get; private set; }

        public event EventHandler<CloseEventArgs> Close;

        public bool CanClose() => true;

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(); }
        }

        private string _deviceType;
        public string DeviceType
        {
            get { return _deviceType; }
            set { _deviceType = value; RaisePropertyChanged(); }
        }

        public CreateApplicationDialogViewModel()
        {
            OkCommand = new RelayCommand(Ok, CanOk);
        }

        private void Ok()
        {
            Close?.Invoke(this, new CloseEventArgs(true));
        }

        private bool CanOk()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return false;

            if (string.IsNullOrWhiteSpace(DeviceType))
                return false;

            return true;
        }

        public void Closed()
        {
        }
    }
}
