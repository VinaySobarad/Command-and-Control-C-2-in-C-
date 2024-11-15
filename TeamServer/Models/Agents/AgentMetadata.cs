using System.Globalization;

namespace TeamServer.Models.Agents
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

    }
}
