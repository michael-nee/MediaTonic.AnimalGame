using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MediaTonic.AnimalGame.Tests.Integration
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testServer;
        public HttpClient Client { get; }

        public TestServerFixture()
        {
            var builder = new WebHostBuilder()
                   .UseEnvironment("Development")
                   .UseStartup<Startup>();  // Uses Start up class from your API Host project to configure the test server

            _testServer = new TestServer(builder);
            Client = _testServer.CreateClient();

        }

        //private string GetContentRootPath()
        //{
        //    var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
        //    var relativePathToHostProject = @"..\..\..\..\..\..\InMemoryEFCore";
        //    return Path.Combine(testProjectPath, relativePathToHostProject);
        //}

        public void Dispose()
        {
            Client.Dispose();
            _testServer.Dispose();
        }
    }
}
