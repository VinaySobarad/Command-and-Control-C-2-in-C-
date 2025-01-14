﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Models
{

    [DataContract]
    public class AgentTask
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "command")]
        public string command { get; set; }

        [DataMember(Name = "arguments")]
        public string[] Arguments { get; set; }

        [DataMember(Name = "file")]
        public string file { get; set; }

        public byte[] FileBytes
        {
            get
            {
                if (string.IsNullOrEmpty(file)) return new byte[0];
                return Convert.FromBase64String(file);

            }
        }
    }
}
