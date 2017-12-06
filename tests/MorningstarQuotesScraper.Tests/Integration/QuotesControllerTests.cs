using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MorningstarQuotesScraper.Models;
using MorningstarQuotesScraper.Services;
using Newtonsoft.Json;

namespace MorningstarQuotesScraper.Tests.Integration
{
    [TestClass]
    public class QuotesControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;
        private static FakeWebPageDownloader _fakeWebPageDownloader;

        // TODO: replace with other test FW
        // dont't like this MS Test weirdness
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _fakeWebPageDownloader = new FakeWebPageDownloader(new Dictionary<string, string>
            {
                {"http://www.morningstar.es/es/mf/snapshot/snapshot.aspx?id=12345", "MutualFund01.txt" },
                {"http://www.morningstar.es/es/etf/snapshot/snapshot.aspx?id=54321", "Etf01.txt" },
            });

            _testServer =
                new TestServer(
                    new WebHostBuilder()
                    .UseStartup<Startup>()
                    .ConfigureServices(services =>
                        {
                            services.AddSingleton<IWebPageDownloader, FakeWebPageDownloader>(_ => _fakeWebPageDownloader);
                        }));
            _httpClient = _testServer.CreateClient();
        }

        [TestMethod]
        public async Task ShouldReturnBadRequestIfFundTypeIsEmpty()
        {
            var response = await _httpClient.GetAsync("api/quotes/%20/12345");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task ShouldReturnBadRequestIfFundIdIsEmpty()
        {
            var response = await _httpClient.GetAsync("api/quotes/mf/%20");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task ShouldReturn404ForUnknowFundType()
        {
            var response = await _httpClient.GetAsync("api/quotes/aaaa/12345");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task ShouldRetrieveMutualFundQuoteCorrectly()
        {
            var response = await _httpClient.GetAsync("api/quotes/mf/12345");
            var content = await response.Content.ReadAsStringAsync();

            var quoteData = JsonConvert.DeserializeObject<QuoteData>(content);

            quoteData.Date.Should().Be("16/11/2017");
            quoteData.Quote.Should().Be(24.4f);
        }

        [TestMethod]
        public async Task ShouldRetrieveEtfFundQuoteCorrectly()
        {
            var response = await _httpClient.GetAsync("api/quotes/etf/54321");
            var content = await response.Content.ReadAsStringAsync();

            var quoteData = JsonConvert.DeserializeObject<QuoteData>(content);

            quoteData.Date.Should().Be("17/11/2017");
            quoteData.Quote.Should().Be(178.28f);
        }

        [TestMethod]
        public async Task ShouldCacheFundQuote()
        {
            var response = await _httpClient.GetAsync("api/quotes/etf/54321");

            response.Headers.CacheControl.Should().NotBeNull();
            response.Headers.CacheControl.MaxAge.Should().Be(TimeSpan.FromHours(8));
        }
    }
}