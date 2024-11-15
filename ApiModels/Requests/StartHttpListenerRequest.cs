namespace ApiModels.Requests
{
    public class StartHttpListenerRequest
    {
        //public StartHttpListenerRequest(string name, int bindPort)
        //{
        //    Name = name;
        //    BindPort = bindPort;
        //}

        public required string Name { get; set; }
        public int BindPort { get; set; }
    }
}
