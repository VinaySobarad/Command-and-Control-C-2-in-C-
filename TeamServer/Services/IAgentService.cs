using TeamServer.Models;
using TeamServer.Models.Agents;

namespace TeamServer.Services
{
    public interface IAgentService
    {
        void AddAgent(Agent agent);
        IEnumerable<Agent> GetAgent();
        Agent? GetAgent(string id);
        void RemoveAgent(Agent agent);
        
    }

    public class AgentService : IAgentService
    {

        private readonly List<Agent> _agents = new();



        public void AddAgent(Agent agent)
        {
            _agents.Add(agent);
        }

        public IEnumerable<Agent> GetAgent()
        {
            return _agents;
        }

        public Agent? GetAgent(string id)
        {
            return GetAgent().FirstOrDefault(a => a.Metadata.Id.Equals(id));
        }

        public void RemoveAgent(Agent agent)
        {
            _agents.Remove(agent);
        }
    }

}
