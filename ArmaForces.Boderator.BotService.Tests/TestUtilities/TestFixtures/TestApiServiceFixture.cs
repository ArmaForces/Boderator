using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ArmaForces.Boderator.BotService.Tests.TestUtilities.TestFixtures
{
    // This class is created by XUnit test runner once for TestApiCollection
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TestApiServiceFixture : IDisposable
    {
        private readonly WebApplicationFactory<Program> _testAppFactory;

        public HttpClient HttpClient { get; }

        internal IServiceProvider ServiceProvider => _testAppFactory.Services;
        
        public TestApiServiceFixture()
        {
            _testAppFactory = new TestApplicationFactory();
            HttpClient = _testAppFactory.CreateClient();
        }

        public void Dispose()
        {
            HttpClient.Dispose();
            _testAppFactory.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
