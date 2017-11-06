using Resin.Api.Client;
using Resin.Api.Client.Domain;
using ResinExplorer.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResinExplorer.ViewModel
{
    public class EditApplicationVariablesDialogViewModel : EditVariablesBaseViewModel
    {
        // Application id
        private int _applicationId;

        public EditApplicationVariablesDialogViewModel(ResinApiClient client, int applicationId, IEnumerable<EnvironmentVariable> variables)
            : base(client, variables.Select(v => new GenericEnvironmentVariable { Id = v.Id, Name = v.Name, Value = v.Value }))
        {
            _applicationId = applicationId;
        }

        protected override IEnumerable<Task> AddVariablesAsync(Dictionary<string, string> variables)
        {
            return Client.CreateApplicationVariableAsync(_applicationId, variables);
        }
    }
}
