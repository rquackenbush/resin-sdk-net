using Cas.Common.WPF;
using Cas.Common.WPF.Interfaces;
using GalaSoft.MvvmLight;
using ResinExplorer.Core;
using System;

namespace ResinExplorer.ViewModel
{
    public class EnvironmentVariableViewModel : ViewModelBase
    {
        private readonly GenericEnvironmentVariable _model;
        private readonly IDirtyService _dirtyService = new DirtyService();
        public IDirtyService DirtyService => _dirtyService;

        public EnvironmentVariableViewModel(GenericEnvironmentVariable model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _dirtyService.MarkClean();
        }

        public int Id
        {
            get { return _model.Id; }
            set
            {
                _model.Id = value;
                RaisePropertyChanged();
                RaisePropertyChanged(() => CanEdit);
            }
        }

        public string Value
        {
            get { return _model.Value; }
            set
            {
                _model.Value = value;
                RaisePropertyChanged();
                _dirtyService.MarkDirty();
            }
        }

        public string Name
        {
            get { return _model.Name; }
            set
            {
                _model.Name = value;
                RaisePropertyChanged();
                _dirtyService.MarkDirty();
            }
        }

        public bool CanEdit
        {
            get { return Id == 0; }
        }


        public GenericEnvironmentVariable GetModel()
        {
            // TODO Clone
            return _model;
        }
    }
}
