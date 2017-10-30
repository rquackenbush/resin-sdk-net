using GalaSoft.MvvmLight;
using Resin.Api.Client.Domain;
using System;

namespace ResinExplorer.ViewModel
{
    public class EnvironmentVariableViewModel : ViewModelBase
    {
        private EnvironmentVariable _model;

        public EnvironmentVariableViewModel(EnvironmentVariable model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged(); }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set { _value = value; RaisePropertyChanged(); }
        }


        public EnvironmentVariable GetModel()
        {
            // TODO Clone
            return _model;
        }
    }
}
