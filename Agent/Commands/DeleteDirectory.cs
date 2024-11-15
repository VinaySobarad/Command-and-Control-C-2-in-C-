using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Models;

namespace Agent.Commands
{
    public class DeleteDirectory : AgentCommand
    {
        public override string Name => "rmdir";

        public override string Execute(AgentTask task)
        {

            if (task.Arguments is null || task.Arguments.Length == 0)
            {
                return "No path provided";
            }

            var path = task.Arguments[0];

            //var recurse = bool.Parse(task.Arguments[1]);

            Directory.Delete(path, true);           
            
            if (!Directory.Exists(path))
            {
                return $"{path} deleted";
            }

            return $"Failed to delete the {path}";
        }
    }
}
