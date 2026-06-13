using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Models.Response.Export
{
    internal sealed class IdSwitchboard
    {
        private readonly Dictionary<string, string> _map;

        public IdSwitchboard(IEnumerable<string> originalIds)
        {
            _map = new Dictionary<string, string>();

            foreach (var oldId in originalIds)
            {
                _map[oldId] = Guid.NewGuid().ToString();
            }
        }

        public string Switcharoo(string oldId)
        {
            return _map.TryGetValue(oldId, out var newId)
                ? newId
                : oldId;
        }

        public IReadOnlyDictionary<string, string> GetMap()
        {
            return _map;
        }
    }
}
