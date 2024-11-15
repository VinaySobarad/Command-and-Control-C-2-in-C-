using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Models
{
    public class AgentMetada
    {
        public string Id { get; set; }
        public string HostName { get; set; }
        public string Username { get; set; }
        public string ProcessName { get; set; }
        public int ProcessId { get; set; }
        public string Integrity { get; set; }
        public string Architecture { get; set; }

        internal static byte[] Serialise()
        {
            throw new NotImplementedException();
        }
    }
}
