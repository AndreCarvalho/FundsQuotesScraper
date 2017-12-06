using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MorningstarQuotesScraper.Services;

namespace MorningstarQuotesScraper.Tests.Unit
{
    [TestClass]
    public class EtfQuoteScraperTests
    {
        [TestMethod]
        public void ShouldHandleCorrectFundType()
        {
            var sut = new EtfQuoteScraper();

            sut.HandlesFundType("etf").Should().BeTrue();
        }

        [TestMethod]
        public void ShouldNotHandleIncorrectFundType()
        {
            var sut = new EtfQuoteScraper();

            sut.HandlesFundType("mf").Should().BeFalse();
        }

        [TestMethod]
        public async Task EtfQuoteScraping()
        {
            var sut = new EtfQuoteScraper();

            var htmlDocument = new HtmlDocument();

            var fileText = await File.ReadAllTextAsync("TestPages\\Etf01.txt");
            htmlDocument.LoadHtml(fileText);

            var quoteData = sut.Scrape(htmlDocument, new CultureInfo("es-ES"));

            quoteData.Quote.Should().Be(178.28f);
            quoteData.Date.Should().Be("17/11/2017");

        }

        [TestMethod]
        public async Task MutualFundScraping_02()
        {
            var sut = new EtfQuoteScraper();

            var htmlDocument = new HtmlDocument();

            var fileText = await File.ReadAllTextAsync("TestPages\\Etf02.txt");
            htmlDocument.LoadHtml(fileText);

            var quoteData = sut.Scrape(htmlDocument, new CultureInfo("es-ES"));

            quoteData.Quote.Should().Be(57.8f);
            quoteData.Date.Should().Be("17/11/2017");
        }
    }
}