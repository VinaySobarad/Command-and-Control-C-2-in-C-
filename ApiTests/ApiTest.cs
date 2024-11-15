using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using TeamServer;

namespace ApiTests
{
    public abstract class ApiTest
    {
        protected HttpClient Client;
        protected ApiTest()
        {
            var factory = new WebApplicationFactory<Startup>();
            Client = factory.CreateClient();



        }

    }
}
