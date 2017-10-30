using Cas.Common.WPF.Behaviors;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Resin.Api.Client;
using System;
using System.Windows.Input;

namespace ResinExplorer.ViewModel
{
    public class LogonDialogViewModel : ViewModelBase, ICloseableViewModel
    {
        private string _token;
        private string _apiAddress = ResinApiClient.DefaultApiAddress;

        public LogonDialogViewModel()
        {
            OkCommand = new RelayCommand(Ok, CanOk);
        }

        public ICommand OkCommand { get; }

        private void Ok()
        {
            Close?.Invoke(this, new CloseEventArgs(true));
        }

        private bool CanOk()
        {
            if (string.IsNullOrWhiteSpace(Token))
                return false;

            if (string.IsNullOrWhiteSpace(ApiAddress))
                return false;

            return true;
        }

        private bool _shouldRememberToken;
        public bool ShouldRememberToken
        {
            get { return _shouldRememberToken; }
            set { _shouldRememberToken = value; RaisePropertyChanged(); }
        }

        public string Token
        {
            get { return _token; }
            set
            {
                _token = value;
                RaisePropertyChanged();
            }
        }

        public string ApiAddress
        {
            get { return _apiAddress; }
            set
            {
                _apiAddress = value;
                RaisePropertyChanged();
            }
        }

        public bool CanClose()
        {
            return true;
        }

        public void Closed()
        {
        }

        public event EventHandler<CloseEventArgs> Close;
    }
}