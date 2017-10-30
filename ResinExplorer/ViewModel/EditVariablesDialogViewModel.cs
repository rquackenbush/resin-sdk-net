using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace ResinExplorer.ViewModel
{
    public class EditVariablesDialogViewModel : ViewModelBase
    {
        private List<EnvironmentVariableViewModel> _variables;
        public List<EnvironmentVariableViewModel> Variables
        {
            get { return _variables; }
            set { _variables = value; RaisePropertyChanged(); }
        }
    }
}
