using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Agent.Models
{
    public class HttpCommModule : CommModule
    {
        public string ConnectAddress {  get; set; } 
        
        public int ConnectPort { get; set; }

        private CancellationTokenSource _tokensource;

        private HttpClient _client;


        public HttpCommModule(string connectAddress, int connectPort) 
        {
            ConnectAddress = connectAddress;
            ConnectPort = connectPort;
        }


        public override void Init(AgentMetada metadata)
        {
            base.Init(metadata);

            _client  = new HttpClient();
            _client.BaseAddress = new Uri($"http://{ConnectAddress}:{ConnectPort}");
            _client.DefaultRequestHeaders.Clear();

            var encodedMetadata = Convert.ToBase64String(AgentMetadata.Serialise());

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {encodedMetadata}");

        
        }


          


        public override async Task Start()
        {
            _tokensource = new CancellationTokenSource();
            while (!_tokensource.IsCancellationRequested)
            {
                //check to see if we have data to send:

                if (!Outbound.IsEmpty)
                {
                    await PostData();
                }
                else
                {
                    await CheckIn();
                }

                //checkin
                //get task
                //sleep

                await Task.Delay(1000);

            }
        }


        private async Task CheckIn()
        {
            var response = await _client.GetByteArrayAsync("/");
            HandleResponse(response);
        }

        private async Task PostData()
        {
            var outbound = GetOutbound().Serialise();


            var content = new StringContent(Encoding.UTF8.GetString(outbound), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/", content);

            var responseContent = await response.Content.ReadAsByteArrayAsync();

            HandleResponse(responseContent);
        }


        private void HandleResponse(byte[] response)
        {
            var tasks = response.Deseiralize<AgentTask[]>();
            if (tasks != null && tasks.Any())
            {
                foreach (var task in tasks) 
                {
                    Inbound.Enqueue(task);
                }

            }
        }
        public override Task Stop()
        {
            _tokensource.Cancel();
            return Task.CompletedTask;
        }

        //public override void  Stop()
        //{
        //    _tokensource.Cancel();
        //}

    }
}
