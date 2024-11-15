using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Agent.Models;

namespace Agent.Commands
{
    public class WhoAmi : AgentCommand
    {
        public override string Name => "whoami"; 

        public override string Execute(AgentTask task)
        {
            var identity = WindowsIdentity.GetCurrent();
            return identity.Name;
        }
    }
}
