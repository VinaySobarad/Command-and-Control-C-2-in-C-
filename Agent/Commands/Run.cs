using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Models;

namespace Agent.Commands
{
    public class Run : AgentCommand
    {
        public override string Name => "run";

        public override string Execute(AgentTask task)
        {
            var fileName = task.Arguments[0];
            var args = string.Join(" ", task.Arguments.Skip(1));
                
            return Internal.Execute.ExecuteCommand(fileName, args);
        }
    }
}
