using GalaSoft.MvvmLight;
using Resin.Api.Client.Domain;
using System;

namespace ResinExplorer.ViewModel
{
    public class ApplicationViewModel : ViewModelBase
    {
        private readonly ResinApplication _model;

        public ApplicationViewModel(ResinApplication model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            _model = model;
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