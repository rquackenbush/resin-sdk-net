using Resin.Api.Client;
using Resin.Api.Client.Domain;
using ResinExplorer.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResinExplorer.ViewModel
{
    public class EditDeviceVariablesDialogViewModel : EditVariablesBaseViewModel
    {
        private int _deviceId;

        public EditDeviceVariablesDialogViewModel(ResinApiClient client, int deviceId, IEnumerable<EnvironmentVariable> variables)
            : base(client, variables.Select(v => new GenericEnvironmentVariable { Id = v.Id, Name = v.Name, Value = v.Value }))
        {
            _deviceId = deviceId;
        }

        protected override IEnumerable<Task> AddVariablesAsync(Dictionary<string, string> variables)
        {
            return Client.CreateDeviceEnvironmentVariableAsync(_deviceId, variables);
        }
    }
}
