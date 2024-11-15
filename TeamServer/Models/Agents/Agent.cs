using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace TeamServer.Models.Agents
{
    public class Agent
    {
        public AgentMetada Metadata { get; }
        public DateTime LastSeen { get; private set; }

        private readonly ConcurrentQueue<AgentTask> _pendingTasks = new();
        private readonly List<AgentTask> _taskResults = new();

        public Agent(AgentMetada metadata)
        {
            Metadata = metadata;
        }

        public void CheckIn()
        {
            LastSeen = DateTime.UtcNow;
        }

        public void QueueTask(AgentTask task)
        {
            _pendingTasks.Enqueue(task);
        }

        public IEnumerable<AgentTask> GetPendingTasks()
        {
            List<AgentTask> tasks = new();

            while (_pendingTasks.TryDequeue(out var task))
            {
                tasks.Add(task);


            }
            return tasks;

        }
        public AgentTaskResults GetTaskResult(string taskId)
        {
            return GetTaskResults().FirstOrDefault(a => a.Id.Equals(taskId));

        }

        public IEnumerable<AgentTaskResults> GetTaskResults()
        {
            return (IEnumerable<AgentTaskResults>)_taskResults;
        }

        public void AddTaskResults(IEnumerable<AgentTaskResults> results)
        {
            _taskResults.AddRange((IEnumerable<AgentTask>)results);
        }

    }
}
