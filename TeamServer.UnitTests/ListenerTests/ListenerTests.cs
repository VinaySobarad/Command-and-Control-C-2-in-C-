using TeamServer.Models;
using TeamServer.Services;

namespace TeamServer.UnitTests.ListenersTests
{
    public class ListenerTests
    {

        private readonly IListenerService _listeners;

        public ListenerTests(IListenerService listeners)
        {
            _listeners = listeners;
        }

        [Fact]
        public void TestCreateGetListener()
        {

           // var listenerName = "TestLiestener";

            var origlistener = new HttpListener("TestLiestener", 4444);
            _listeners.AddListener(origlistener);

            var newListener = (HttpListener)_listeners.GetListener(origlistener.Name);

            Assert.NotNull(newListener);
            Assert.Equal(origlistener.Name, newListener.Name);
            Assert.Equal(origlistener.BindPort, newListener.BindPort);







        }
    }
}