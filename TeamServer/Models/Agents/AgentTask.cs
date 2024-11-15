namespace TeamServer.Models.Agents
{
    public class AgentTask
    {
        public string Id { get; set; }  

        public string command { get; set; } 

        public string[] Arguments { get; set; }

        public byte[] file { get; set; }
    }
}
