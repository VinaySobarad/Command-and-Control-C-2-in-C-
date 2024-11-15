using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels.Requests
{
    public class TaskAgentRequest
    {
        public string command { get; set; }

        public string[] Arguments { get; set; }

        public byte[] file { get; set; }
    }
}
