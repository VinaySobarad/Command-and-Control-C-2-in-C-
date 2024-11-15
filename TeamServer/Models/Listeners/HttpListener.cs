


namespace TeamServer.Models
{
    public class HttpListener : Listener
    {
        public override string Name { get; }
        public int BindPort { get; }



        //private CancellationTokenSource _tokenSource;
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();  // Initialize _tokenSource

        public HttpListener(string name, int bindPort)
        {
            Name = name;   
            BindPort = bindPort;

        }

        public override async Task Start()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHostDefaults(host =>
                {
                    host.UseUrls($"http://0.0.0.0:{BindPort}");
                    host.Configure(ConfigureApp);
                    host.ConfigureServices(ConfigureServices);
                });
            var host = hostBuilder.Build();


            _tokenSource = new CancellationTokenSource();



        //host.RunAsync(_tokenSource.Token);
            host.RunAsync(_tokenSource.Token);  // Add 'await' to ensure the async method behaves correctly



        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton(AgentService);


        }

        private void ConfigureApp(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(e=>
            {
                e.MapControllerRoute("/", "/", new {controller = "HttpListener", action = "HandleImplant" });
            });

        }

        public override void Stop()
        {
            _tokenSource.Cancel();
        }
    }
}
