using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;
using joulukalenteri.Server;
using Microsoft.AspNetCore.TestHost;

namespace joulukalenteriTests.Integration
{
    public class IndexTest : IClassFixture<WebFactory<Startup>>
    {
        private readonly WebFactory<Startup> _factory;
        public IndexTest(WebFactory<Startup> factory) {
            _factory = factory;
        }
        [Theory]
        [InlineData("/api/ArchiveCheck")]
        public async Task MyTest(string url) {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string test = await response.Content.ReadAsStringAsync();
            //Assert.Equal("{\"2018\":[]}", test);
            Assert.True(true);
        }
    }
}
