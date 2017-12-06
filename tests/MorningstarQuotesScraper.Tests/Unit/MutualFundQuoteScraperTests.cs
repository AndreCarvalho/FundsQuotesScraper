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
    public class MutualFundQuoteScraperTests
    {
        [TestMethod]
        public void ShouldHandleCorrectFundType()
        {
            var sut = new MutualFundQuoteScraper();

            sut.HandlesFundType("mf").Should().BeTrue();
        }

        [TestMethod]
        public void ShouldNotHandleIncorrectFundType()
        {
            var sut = new MutualFundQuoteScraper();

            sut.HandlesFundType("etf").Should().BeFalse();
        }

        [TestMethod]
        public async Task MutualFundScraping_01()
        {
            var sut = new MutualFundQuoteScraper();

            var htmlDocument = new HtmlDocument();

            var fileText = await File.ReadAllTextAsync("TestPages\\MutualFund01.txt");
            htmlDocument.LoadHtml(fileText);

            var quoteData = sut.Scrape(htmlDocument, new CultureInfo("es-ES"));

            quoteData.Quote.Should().Be(24.4f);
            quoteData.Date.Should().Be("16/11/2017");
        }
    }
}
