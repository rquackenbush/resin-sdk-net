using GalaSoft.MvvmLight;
using Resin.Api.Client.Domain;
using System;

namespace ResinExplorer.ViewModel
{
    public class DeviceViewModel : ViewModelBase
    {
        private readonly ResinDevice _model;

        public DeviceViewModel(ResinDevice model)
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
            get { return _model.Name; }
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
